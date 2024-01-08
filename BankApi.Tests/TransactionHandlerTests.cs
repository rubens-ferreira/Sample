using System.Threading.Tasks;
using BankApi.Transactions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Xunit;

namespace BankApi.Tests
{
    public class TransactionHandlerTests
    {
        [Fact]
        public async Task GetTransactionById_ReturnsOkObjectResult_WhenTransactionExists()
        {
            // Arrange
            var transactionID = Guid.NewGuid();
            var expectedTransaction = new Transaction { TransactionID = transactionID, };

            var mockRepository = new Mock<ITransactionRepository>();
            mockRepository
                .Setup(repo => repo.GetTransactionByIdAsync(transactionID))
                .ReturnsAsync(expectedTransaction);

            // Act
            var response = await TransactionHandler.GetTransactionById(
                transactionID,
                mockRepository.Object
            );

            // Assert
            Assert.NotNull(response);
            var okResult = Assert.IsType<Ok<Transaction>>(response.Result);
            var returnedTransaction = okResult.Value;
            Assert.NotNull(returnedTransaction);
            Assert.Equal(expectedTransaction.TransactionID, returnedTransaction.TransactionID);
        }

        [Fact]
        public async Task GetTransactionById_ReturnsNotFoundObjectResult_WhenTransactionDoesNotExist()
        {
            // Arrange
            var transactionId = Guid.NewGuid();

            var mockRepository = new Mock<ITransactionRepository>();
            mockRepository
                .Setup(repo => repo.GetTransactionByIdAsync(transactionId))
                .ReturnsAsync((Transaction)null);

            // Act
            var response = await TransactionHandler.GetTransactionById(
                transactionId,
                mockRepository.Object
            );

            // Assert
            Assert.IsType<NotFound>(response.Result);
        }

        [Fact]
        public async Task AddTransaction_ShouldReturnCreatedResult_WhenTransactionIsAddedSuccessfully()
        {
            // Arrange
            var transaction = new Transaction { AccountID = Guid.NewGuid(), Amount = 10 };

            var mockRepository = new Mock<ITransactionRepository>();
            mockRepository
                .Setup(repo => repo.AddTransactionAsync(transaction))
                .ReturnsAsync(transaction);
            // Act
            var response = await TransactionHandler.AddTransaction(
                transaction,
                mockRepository.Object
            );

            // Assert
            Assert.NotNull(response);
            var createdResult = Assert.IsType<Created<Transaction>>(response);
            Assert.NotNull(createdResult);
            Assert.Equal(transaction.AccountID, createdResult.Value?.AccountID);
            Assert.Equal($"/Transactions/{transaction.TransactionID}", createdResult.Location);
        }
    }
}
