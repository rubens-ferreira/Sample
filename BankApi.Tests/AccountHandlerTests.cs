using BankApi.Accounts;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace BankApi.Tests
{
    public class AccountHandlerTests
    {
        [Fact]
        public async Task GetAccountById_ReturnsOkObjectResult_WhenAccountExists()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var expectedAccount = new Account { AccountID = accountId, };

            var mockRepository = new Mock<IAccountRepository>();
            mockRepository
                .Setup(repo => repo.GetAccountByIdAsync(accountId))
                .ReturnsAsync(expectedAccount);

            // Act
            var response = await AccountHandler.GetAccountById(accountId, mockRepository.Object);

            // Assert
            Assert.NotNull(response);
            var okResult = Assert.IsType<Ok<Account>>(response.Result);
            var returnedAccount = okResult.Value;
            Assert.NotNull(returnedAccount);
            Assert.Equal(expectedAccount.AccountID, returnedAccount.AccountID);
        }

        [Fact]
        public async Task GetAccountById_ReturnsNotFoundObjectResult_WhenAccountDoesNotExist()
        {
            // Arrange
            var accountId = Guid.NewGuid();

            var mockRepository = new Mock<IAccountRepository>();
            mockRepository
                .Setup(repo => repo.GetAccountByIdAsync(accountId))
                .ReturnsAsync((Account)null);

            // Act
            var response = await AccountHandler.GetAccountById(accountId, mockRepository.Object);

            // Assert
            Assert.IsType<NotFound>(response.Result);
        }
    }
}
