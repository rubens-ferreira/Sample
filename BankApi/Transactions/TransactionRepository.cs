namespace BankApi.Transactions;

using Microsoft.EntityFrameworkCore;

public interface ITransactionRepository
{
    public Task<Transaction> GetTransactionByIdAsync(Guid id);
    public Task<Transaction> AddTransactionAsync(Transaction transaction);
    public Task<Transaction> RemoveTransactionByIdAsync(Guid id);
}

public class TransactionRepository(BankDb db) : ITransactionRepository
{
    private readonly BankDb _db = db;

    public async Task<Transaction> GetTransactionByIdAsync(Guid id) =>
        await _db.Transactions.Where(t => t.TransactionID == id).FirstOrDefaultAsync();

    public async Task<Transaction> AddTransactionAsync(Transaction transaction)
    {
        _db.Transactions.Add(transaction);
        await _db.SaveChangesAsync();
        return transaction;
    }

    public async Task<Transaction> RemoveTransactionByIdAsync(Guid id)
    {
        var transaction = await _db.Transactions.FindAsync(id);
        if (transaction == null)
            return null;
        _db.Transactions.Remove(transaction);
        await _db.SaveChangesAsync();
        return transaction;
    }
}
