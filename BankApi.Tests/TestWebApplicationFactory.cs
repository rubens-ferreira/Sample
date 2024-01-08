namespace BankApi.IntegrationTests;

using Microsoft.AspNetCore.Mvc.Testing;

public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    // TODO in a real project, the database context needs to be overriden by an in memory version
}
