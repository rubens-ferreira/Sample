using BankApi.Accounts;
using BankApi.Transactions;
using BankApi.Users;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Tests
{
    public class TransactionRepositoryTests : IDisposable
    {
        private readonly BankDb _context;
        private readonly TransactionRepository _repository;
        private readonly AccountRepository _accountRepository;
        private readonly UserRepository _userRepository;

        public TransactionRepositoryTests()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<BankDb>().UseSqlite(connection).Options;
            _context = new BankDb(options);
            _context.Database.EnsureCreated();
            _repository = new TransactionRepository(_context);
            _accountRepository = new AccountRepository(_context);
            _userRepository = new UserRepository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task GetTransactionByIdAsync_ShouldReturnCorrectTransaction()
        {
            // Arrange
            var testUser = new User { Name = "test" };
            _context.Users.Add(testUser);
            await _context.SaveChangesAsync();
            var testAccount = new Account { Balance = 10, UserID = testUser.UserID };
            _context.Accounts.Add(testAccount);
            await _context.SaveChangesAsync();
            var testTransaction = new Transaction
            {
                AccountID = testAccount.AccountID,
                Amount = 10
            };
            _context.Transactions.Add(testTransaction);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetTransactionByIdAsync(testTransaction.TransactionID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testTransaction.TransactionID, result.TransactionID);
            Assert.Equal(testTransaction.Amount, result.Amount);
        }

        [Fact]
        public async Task GetAccountByIdAsync_ShouldReturnNullForNonexistentAccount()
        {
            // Arrange

            // Act
            var result = await _repository.GetTransactionByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddTransactionAsync_ShouldAddTransactionToDatabase()
        {
            // Arrange
            var user = new User { Name = "name" };
            var addedUser = await _userRepository.AddUserAsync(user);
            var account = new Account { UserID = addedUser.UserID, Balance = 0 };
            var addedAccount = await _accountRepository.AddAccountAsync(account);

            var transaction = new Transaction { AccountID = addedAccount.AccountID, Amount = 1 };

            // Act
            var addedTransaction = await _repository.AddTransactionAsync(transaction);

            // Assert
            Assert.NotNull(addedTransaction);
            Assert.Equal(addedAccount.AccountID, addedTransaction.AccountID);

            var retrievedTransaction = await _context.Transactions.FirstOrDefaultAsync(
                u => u.TransactionID == addedTransaction.TransactionID
            );
            Assert.NotNull(retrievedTransaction);
            Assert.Equal(addedTransaction.TransactionID, retrievedTransaction.TransactionID);
            Assert.Equal(addedTransaction.AccountID, retrievedTransaction.AccountID);
        }
    }
}
