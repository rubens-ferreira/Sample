using System.Text.Json.Serialization;
using BankApi;
using BankApi.Accounts;
using BankApi.Commands;
using BankApi.Transactions;
using BankApi.Users;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder
        .WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader());
});

// Sqlite in memory connection needs to be left open to solve
// the migration database issue reported in
// https://github.com/dotnet/efcore/issues/9842
using var dbConn = new SqliteConnection("Filename=:memory:");
await dbConn.OpenAsync();

builder.Services.AddDbContext<BankDb>(opt => opt.UseSqlite(dbConn));
builder.Services.AddScoped<ICommandRepository, CommandRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(
    options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddExceptionHandler(options =>
{
    options.ExceptionHandler = ExceptionHandler.HandleException;
});

var app = builder.Build();

app.UseCors();
app.UseExceptionHandler();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<BankDb>();
    dbContext.Database.Migrate();
}

app.MapPost("/api/Accounts/CurrentAccount", CommandHandler.AddNewCurrentAccount);
app.MapPost("/api/Users", UserHandler.AddUser);
app.MapPut("/api/Users", UserHandler.UpdateUser);
app.MapGet("/api/Users", UserHandler.GetUsers);
app.MapGet("/api/Users/{id}", UserHandler.GetUserById);
app.MapGet("/api/Users/Details/{id}", UserHandler.GetDetailedUserById);
app.MapGet("/api/Accounts/{id}", AccountHandler.GetAccountById);
app.MapPost("/api/Transactions", TransactionHandler.AddTransaction);
app.MapGet("/api/Transactions/{id}", TransactionHandler.GetTransactionById);

app.Run();

public partial class Program { }
