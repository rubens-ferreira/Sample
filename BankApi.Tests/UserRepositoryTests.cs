using BankApi.Users;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Tests
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly BankDb _context;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<BankDb>().UseSqlite(connection).Options;
            _context = new BankDb(options);
            _context.Database.EnsureCreated();
            _repository = new UserRepository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task AddUserAsync_ShouldAddUserToDatabase()
        {
            // Arrange
            var user = new User { Name = "test", SurName = "test" };

            // Act
            var addedUser = await _repository.AddUserAsync(user);

            // Assert
            Assert.NotNull(addedUser);
            Assert.Equal(user.Name, addedUser.Name);
            Assert.Equal(user.SurName, addedUser.SurName);

            var retrievedUser = await _context.Users.FirstOrDefaultAsync(
                u => u.UserID == user.UserID
            );
            Assert.NotNull(retrievedUser);
            Assert.Equal(addedUser.UserID, retrievedUser.UserID);
            Assert.Equal(addedUser.Name, retrievedUser.Name);
            Assert.Equal(addedUser.SurName, retrievedUser.SurName);
        }
    }
}
