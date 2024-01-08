using System.Threading.Tasks;
using BankApi.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Xunit;

namespace BankApi.Tests
{
    public class UserHandlerTests
    {
        [Fact]
        public async Task AddUser_ShouldReturnCreatedResult_WhenUserIsAddedSuccessfully()
        {
            // Arrange
            var user = new User
            {
                UserID = 1,
                Name = "name",
                SurName = "surname"
            };

            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.AddUserAsync(user)).ReturnsAsync(user);

            // Act
            var response = await UserHandler.AddUser(user, mockRepository.Object);

            // Assert
            Assert.NotNull(response);
            var createdResult = Assert.IsType<Created<User>>(response.Result);
            Assert.Equal(user, createdResult.Value);
            Assert.Equal($"/Users/{user.UserID}", createdResult.Location);
        }
    }
}
