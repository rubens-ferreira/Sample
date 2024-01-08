using BankApi.Accounts;
using BankApi.Users;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Tests
{
    public class AccountRepositoryTests : IDisposable
    {
        private readonly BankDb _context;
        private readonly AccountRepository _repository;
        private readonly UserRepository _userRepository;

        public AccountRepositoryTests()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<BankDb>().UseSqlite(connection).Options;
            _context = new BankDb(options);
            _context.Database.EnsureCreated();
            _repository = new AccountRepository(_context);
            _userRepository = new UserRepository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task GetAccountByIdAsync_ShouldReturnCorrectAccount()
        {
            // Arrange
            var testUser = new User { Name = "test" };
            _context.Users.Add(testUser);
            await _context.SaveChangesAsync();
            var testAccount = new Account
            {
                AccountID = Guid.NewGuid(),
                Balance = 10,
                UserID = testUser.UserID
            };
            _context.Accounts.Add(testAccount);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAccountByIdAsync(testAccount.AccountID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testAccount.AccountID, result.AccountID);
        }

        [Fact]
        public async Task GetAccountByIdAsync_ShouldReturnNullForNonexistentAccount()
        {
            // Arrange

            // Act
            var result = await _repository.GetAccountByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAccountAsync_ShouldAddAccountToDatabase()
        {
            // Arrange
            var user = new User { Name = "test", SurName = "test" };
            var addedUser = await _userRepository.AddUserAsync(user);
            var account = new Account { UserID = addedUser.UserID };

            // Act
            var addedAccount = await _repository.AddAccountAsync(account);

            // Assert
            Assert.NotNull(addedAccount);
            Assert.Equal(addedUser.UserID, addedAccount.UserID);

            var retrievedAccount = await _context.Accounts.FirstOrDefaultAsync(
                a => a.AccountID == addedAccount.AccountID
            );
            Assert.NotNull(retrievedAccount);
            Assert.Equal(addedAccount.AccountID, retrievedAccount.AccountID);
            Assert.Equal(addedAccount.UserID, retrievedAccount.UserID);
        }
    }
}
