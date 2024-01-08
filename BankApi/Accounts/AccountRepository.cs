using Microsoft.EntityFrameworkCore;

namespace BankApi.Accounts;

public interface IAccountRepository
{
    public Task<Account> GetAccountByIdAsync(Guid id);
    public Task<Account> AddAccountAsync(Account account);
    public Task<Account> RemoveAccountByIdAsync(Guid id);
}

public class AccountRepository(BankDb db) : IAccountRepository
{
    private readonly BankDb _db = db;

    public async Task<Account> GetAccountByIdAsync(Guid id) =>
        await _db.Accounts.Where(a => a.AccountID == id).FirstOrDefaultAsync();

    public async Task<Account> AddAccountAsync(Account account)
    {
        _db.Accounts.Add(account);
        await _db.SaveChangesAsync();
        return account;
    }

    public async Task<Account> RemoveAccountByIdAsync(Guid id)
    {
        var account = await _db.Accounts.FindAsync(id);
        if (account == null)
            return null;
        _db.Accounts.Remove(account);
        await _db.SaveChangesAsync();
        return account;
    }
}
