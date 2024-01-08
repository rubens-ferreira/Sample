using System.Text.Json;
using BankApi.Accounts;
using BankApi.Transactions;
using BankApi.Users;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BankApi.Commands;

public static class CommandHandler
{
    public static async Task AddNewCurrentAccount(
        NewCurrentAccountRequest request,
        ICommandRepository commandRepository,
        IUserRepository userRepository,
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository
    )
    {
        NewCurrentAccountCommand command = null;
        try
        {
            PreValidation(request);

            command = await InitCommand(request, commandRepository);

            var account = await DoAccount(command, accountRepository);

            var user = await DoUser(command, userRepository);

            var transaction = await DoTransaction(command, transactionRepository);

            await FinishCommand(command, commandRepository);
        }
        catch (Exception exc)
        {
            var finished = await InitUndoCommand(command, exc, commandRepository);
            finished = finished ? finished : await UndoTransaction(command, transactionRepository);
            finished = finished ? finished : await UndoUser(command, userRepository);
            finished = finished ? finished : await UndoAccount(command, accountRepository);
            await FinishUndoCommand(command, commandRepository);
        }
    }

    private static void PreValidation(NewCurrentAccountRequest request)
    {
        if (request.InitialCredit < 0)
        {
            throw new ArgumentException(
                "Invalid value for 'initialCredit'. Please provide a valid value."
            );
        }
    }

    private static async Task<NewCurrentAccountCommand> InitCommand(
        NewCurrentAccountRequest request,
        ICommandRepository commandRepository
    )
    {
        var command = new NewCurrentAccountCommand
        {
            CommandID = Guid.NewGuid(),
            CommandType = CommandType.NewCurrentAccount,
            CommandStatus = CommandStatus.Created,
            UserID = request.UserID,
            AccountID = Guid.NewGuid(),
            TransactionID = Guid.NewGuid(),
            InitialCredit = request.InitialCredit
        };
        command.Json = JsonSerializer.Serialize(command);
        await commandRepository.AddCommandAsync(command);
        return command;
    }

    private static async Task<User> DoUser(
        NewCurrentAccountCommand command,
        IUserRepository userRepository
    )
    {
        var getUserResponse = await UserHandler.GetUserById(command.UserID, userRepository);
        var user = getUserResponse.Result is Ok<User> result
            ? result.Value
            : throw new Exception("User not found");

        user.CurrentAccountID = command.AccountID;
        var updateUserResponse = await UserHandler.UpdateUser(user, userRepository);
        if (updateUserResponse.Result is NotFound)
            throw new Exception("User not found");

        return user;
    }

    private static async Task<Account> DoAccount(
        NewCurrentAccountCommand command,
        IAccountRepository accountRepository
    )
    {
        var account = new Account
        {
            AccountID =
                command.AccountID ?? throw new ArgumentNullException(nameof(command.AccountID)),
            Balance = command.InitialCredit,
            UserID = command.UserID
        };
        var response = await AccountHandler.AddAccount(account, accountRepository);
        return response.Value;
    }

    private static async Task<Transaction> DoTransaction(
        NewCurrentAccountCommand command,
        ITransactionRepository transactionRepository
    )
    {
        Transaction transaction = null;
        if (command.InitialCredit > 0)
        {
            transaction = new Transaction
            {
                TransactionID =
                    command.TransactionID
                    ?? throw new ArgumentNullException(nameof(command.TransactionID)),
                AccountID =
                    command.AccountID ?? throw new ArgumentNullException(nameof(command.AccountID)),
                Amount = command.InitialCredit
            };
            var response = await TransactionHandler.AddTransaction(
                transaction,
                transactionRepository
            );
            transaction = response.Value;
        }
        return transaction;
    }

    private static async Task FinishCommand(
        NewCurrentAccountCommand command,
        ICommandRepository commandRepository
    )
    {
        command.CommandStatus = CommandStatus.Executed;
        await commandRepository.UpdateCommandAsync(command);
    }

    private static async Task<bool> InitUndoCommand(
        NewCurrentAccountCommand command,
        Exception exception,
        ICommandRepository commandRepository
    )
    {
        if (command == null)
            return true;
        command.CommandStatus = CommandStatus.Compensating;
        command.FailureMessage = exception.Message;
        await commandRepository.UpdateCommandAsync(command);
        return false;
    }

    private static async Task<bool> UndoUser(
        NewCurrentAccountCommand command,
        IUserRepository userRepository
    )
    {
        var user =
            await userRepository.GetUserByIdAsync(command.UserID)
            ?? throw new Exception("User Not Found");
        user.CurrentAccountID = null;
        await userRepository.UpdateUserAsync(user);
        return false;
    }

    private static async Task<bool> UndoTransaction(
        NewCurrentAccountCommand command,
        ITransactionRepository transactionRepository
    )
    {
        if (command.InitialCredit > 0)
        {
            var response = await TransactionHandler.GetTransactionById(
                command.TransactionID
                    ?? throw new ArgumentNullException(nameof(command.TransactionID)),
                transactionRepository
            );
            if (response.Result is Ok<Transaction> result)
            {
                await TransactionHandler.RemoveTransactionById(
                    result.Value.TransactionID,
                    transactionRepository
                );
            }
        }
        return false;
    }

    private static async Task<bool> UndoAccount(
        NewCurrentAccountCommand command,
        IAccountRepository accountRepository
    )
    {
        var response = await AccountHandler.GetAccountById(
            command.AccountID ?? throw new ArgumentNullException(nameof(command.AccountID)),
            accountRepository
        );
        if (response.Result is Ok<Account> result)
        {
            await AccountHandler.RemoveAccountById(result.Value.AccountID, accountRepository);
        }
        return false;
    }

    private static async Task FinishUndoCommand(
        NewCurrentAccountCommand command,
        ICommandRepository commandRepository
    )
    {
        command.CommandStatus = CommandStatus.Failed;
        await commandRepository.UpdateCommandAsync(command);
    }
}
