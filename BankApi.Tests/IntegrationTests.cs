namespace BankApi.IntegrationTests;

using System.Net;
using BankApi.Accounts;
using BankApi.Commands;
using BankApi.Transactions;
using BankApi.Users;

/* Integration Test Scenarios
   1. Create a new user assigning a new current account with some initial credit
   2. Same scenario without initial credit
   3. Assign two accounts to a user
   4. Try to assign a new account for a non existing user
   5. Try to input invalid initial credit
   6. Try to get a non existing user
   ... and other scenarios
*/

public class IntegrationTests(TestWebApplicationFactory<Program> factory)
    : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient = factory.CreateClient();

    private async Task<User> CreateValidUser()
    {
        var payload = new User() { Name = "test", SurName = "test" };

        var response = await _httpClient.PostAsJsonAsync("/api/Users", payload);

        response.EnsureSuccessStatusCode();
        var user = await response.Content.ReadFromJsonAsync<User>();
        Assert.NotNull(user);
        Assert.Equal(payload.Name, user.Name);
        Assert.Equal(payload.SurName, user.SurName);
        Assert.Null(user.CurrentAccountID);
        return user;
    }

    private async Task AddCurrentValidAccount(int userID, int initialCredit)
    {
        var payload = new NewCurrentAccountRequest()
        {
            UserID = userID,
            InitialCredit = initialCredit
        };

        var response = await _httpClient.PostAsJsonAsync("/api/Accounts/CurrentAccount", payload);

        response.EnsureSuccessStatusCode();
    }

    private async Task<User?> GetValidUser(int userID) =>
        await _httpClient.GetFromJsonAsync<User>($"/api/Users/{userID}");

    [Fact]
    public async Task NewUserWithNewAssignedAccountWithInitialCredit()
    {
        var initialCredit = 10;
        var user = await CreateValidUser();
        await AddCurrentValidAccount(user.UserID, initialCredit);
        var updatedUser = await GetValidUser(user.UserID);

        Assert.NotNull(updatedUser);
        Assert.Equal(user.UserID, updatedUser.UserID);
        Assert.NotNull(updatedUser.CurrentAccountID);
    }

    [Fact]
    public async Task NewCurrentAccountZeroInitialCreditNoTransactionAssigned()
    {
        var user = await CreateValidUser();
        await AddCurrentValidAccount(user.UserID, 0);
        var updatedUser = await GetValidUser(user.UserID);

        Assert.NotNull(updatedUser);
        Assert.Equal(user.UserID, updatedUser.UserID);
        Assert.NotNull(updatedUser.CurrentAccountID);
    }

    [Fact]
    public async Task TryToAddAccountToNonExistingUser()
    {
        var payload = new NewCurrentAccountRequest() { UserID = int.MaxValue, InitialCredit = 10 };

        var response = await _httpClient.PostAsJsonAsync("/api/Accounts/CurrentAccount", payload);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task TryToAddAccountWithInvalidInitialCredit()
    {
        var user = await CreateValidUser();
        var payload = new NewCurrentAccountRequest() { UserID = user.UserID, InitialCredit = -1 };

        var response = await _httpClient.PostAsJsonAsync("/api/Accounts/CurrentAccount", payload);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task TryToGetInvalidUser()
    {
        var response = await _httpClient.GetAsync($"/api/Users/{int.MaxValue}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
