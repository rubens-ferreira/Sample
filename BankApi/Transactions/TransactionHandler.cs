namespace BankApi.Transactions;

using Microsoft.AspNetCore.Http.HttpResults;

public static class TransactionHandler
{
    public static async Task<Results<Ok<Transaction>, NotFound>> GetTransactionById(
        Guid id,
        ITransactionRepository repository
    )
    {
        var transaction = await repository.GetTransactionByIdAsync(id);
        return transaction != null ? TypedResults.Ok(transaction) : TypedResults.NotFound();
    }

    public static async Task<Created<Transaction>> AddTransaction(
        Transaction transaction,
        ITransactionRepository repository
    )
    {
        var theTransaction = await repository.AddTransactionAsync(transaction);
        return TypedResults.Created(
            $"/Transactions/{theTransaction.TransactionID}",
            theTransaction
        );
    }

    public static async Task<Results<Ok, NotFound>> RemoveTransactionById(
        Guid id,
        ITransactionRepository repository
    )
    {
        var transaction = await repository.RemoveTransactionByIdAsync(id);
        if (transaction == null)
            return TypedResults.NotFound();
        return TypedResults.Ok();
    }
}
