using Microsoft.EntityFrameworkCore;

namespace BankApi.Users;

public interface IUserRepository
{
    public Task<User> AddUserAsync(User user);
    public Task<List<User>> GetUsersAsync();
    public Task<User> UpdateUserAsync(User user);
    public Task<User> GetUserByIdAsync(int id);
}

public class UserRepository(BankDb db) : IUserRepository
{
    private readonly BankDb _db = db;

    public async Task<User> AddUserAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        var theUser = await _db.Users.FindAsync(user.UserID);
        if (theUser == null)
            return null;
        theUser.Name = user.Name;
        theUser.CurrentAccountID = user.CurrentAccountID;
        _db.Users.Update(theUser);
        await _db.SaveChangesAsync();
        return theUser;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _db.Users.ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(int id) =>
        await _db.Users.Where(a => a.UserID == id).FirstOrDefaultAsync();
}
