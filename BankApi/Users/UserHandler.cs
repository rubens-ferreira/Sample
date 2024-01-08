namespace BankApi.Users;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public static class UserHandler
{
    public static async Task<Results<BadRequest, Created<User>>> AddUser(
        User user,
        IUserRepository repository
    )
    {
        var theUser = await repository.AddUserAsync(user);
        return TypedResults.Created($"/Users/{theUser.UserID}", theUser);
    }

    public static async Task<Results<NotFound, NoContent>> UpdateUser(
        User user,
        IUserRepository repository
    )
    {
        var theUser = await repository.UpdateUserAsync(user);
        if (theUser == null)
        {
            return TypedResults.NotFound();
        }
        return TypedResults.NoContent();
    }

    public static async Task<List<User>> GetUsers(IUserRepository repository)
    {
        return await repository.GetUsersAsync();
    }

    public static async Task<Results<Ok<User>, NotFound>> GetUserById(
        int id,
        IUserRepository repository
    )
    {
        var user = await repository.GetUserByIdAsync(id);
        return user != null ? TypedResults.Ok(user) : TypedResults.NotFound();
    }

    public static async Task<IResult> GetDetailedUserById(int id, BankDb db)
    {
        var user = await db.Users.Include(u => u.Accounts)
            .ThenInclude(a => a.Transactions)
            .FirstOrDefaultAsync(u => u.UserID == id);
        if (user == null)
        {
            return Results.NotFound();
        }
        var result = new
        {
            user.UserID,
            user.Name,
            SurName = user.SurName ?? "",
            user.CurrentAccountID,
            Accounts = user.Accounts.Select(
                account =>
                    new
                    {
                        account.AccountID,
                        Transactions = account.Transactions.Select(
                            transaction => new { transaction.TransactionID, transaction.Amount }
                        )
                    }
            )
                .ToList()
        };
        return Results.Ok(result);
    }
}
