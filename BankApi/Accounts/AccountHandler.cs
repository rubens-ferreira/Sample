namespace BankApi.Accounts;

using Microsoft.AspNetCore.Http.HttpResults;

public static class AccountHandler
{
    public static async Task<Results<Ok<Account>, NotFound>> GetAccountById(
        Guid id,
        IAccountRepository repository
    )
    {
        var account = await repository.GetAccountByIdAsync(id);
        return account != null ? TypedResults.Ok(account) : TypedResults.NotFound();
    }

    public static async Task<Created<Account>> AddAccount(
        Account account,
        IAccountRepository repository
    )
    {
        var theAccount = await repository.AddAccountAsync(account);
        return TypedResults.Created($"/Accounts/{theAccount.AccountID}", theAccount);
    }

    public static async Task<Results<Ok, NotFound>> RemoveAccountById(
        Guid id,
        IAccountRepository repository
    )
    {
        var account = await repository.RemoveAccountByIdAsync(id);
        if (account == null)
            return TypedResults.NotFound();
        return TypedResults.Ok();
    }
}
