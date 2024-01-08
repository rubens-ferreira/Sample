namespace BankApi;

using BankApi.Accounts;
using BankApi.Commands;
using BankApi.Transactions;
using BankApi.Users;
using Microsoft.EntityFrameworkCore;

public class BankDb(DbContextOptions<BankDb> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Command> Commands => Set<Command>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<User>()
            .HasOne(u => u.CurrentAccount)
            .WithOne()
            .HasForeignKey<User>(u => u.CurrentAccountID)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }

    // TODO this code could be moved to the database maybe
    // It needs to be verified for consistency and check if it will not break transactions
    // I would leave it here at least for development and integrated test use
    public override int SaveChanges()
    {
        ValidateCurrentAccount();

        return base.SaveChanges();
    }

    private void ValidateCurrentAccount()
    {
        var usersWithChangedCurrentAccount = ChangeTracker
            .Entries<User>()
            .Where(
                e =>
                    e.State == EntityState.Modified
                    && e.OriginalValues[nameof(User.CurrentAccountID)]
                        != e.CurrentValues[nameof(User.CurrentAccountID)]
            );

        foreach (var entry in usersWithChangedCurrentAccount)
        {
            var user = entry.Entity;
            var currentAccountId = user.CurrentAccountID;

            if (
                currentAccountId.HasValue
                && !user.Accounts.Any(account => account.AccountID == currentAccountId)
            )
            {
                throw new InvalidOperationException(
                    $"The CurrentAccount ({currentAccountId}) must be one of the user's accounts."
                );
            }
        }
    }
}
