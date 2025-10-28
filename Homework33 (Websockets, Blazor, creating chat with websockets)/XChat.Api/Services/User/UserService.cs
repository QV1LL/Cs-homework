using FluentResults;
using Microsoft.EntityFrameworkCore;
using XChat.Api.Helpers.Hasher;
using XChat.Api.Persistence;

namespace XChat.Api.Services.User;

internal class UserService : IUserService
{
    private readonly XChatContext _context;

    public UserService(XChatContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<Models.User>>> GetAllAsync()
    {
        try
        {
            var users = await _context.Users.ToListAsync();
            return Result.Ok(users.AsEnumerable());
        }
        catch (Exception ex)
        {
            return Result.Fail($"Failed to get all users: {ex.Message}");
        }
    }

    public async Task<Result<Models.User>> GetByIdAsync(Guid id)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return Result.Fail("User not found");

            return Result.Ok(user);
        }
        catch (Exception ex)
        {
            return Result.Fail($"Failed to get user by id: {ex.Message}");
        }
    }

    public async Task<Result<Models.User>> GetByUsernameAsync(string username)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == username);
            if (user == null)
                return Result.Fail($"User '{username}' not found");

            return Result.Ok(user);
        }
        catch (Exception ex)
        {
            return Result.Fail($"Failed to get user by username: {ex.Message}");
        }
    }

    public async Task<Result<Models.User>> CreateAsync(Models.User user)
    {
        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Result.Ok(user);
        }
        catch (Exception ex)
        {
            return Result.Fail($"Failed to create user: {ex.Message}");
        }
    }

    public async Task<Result<Models.User>> UpdateAsync(Models.User user)
    {
        try
        {
            var existing = await _context.Users.FindAsync(user.Id);
            if (existing == null)
                return Result.Fail("User not found");

            _context.Entry(existing).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();
            return Result.Ok(user);
        }
        catch (Exception ex)
        {
            return Result.Fail($"Failed to update user: {ex.Message}");
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> VerifyUser(string name, string password)
    {
        var userResult = await GetByUsernameAsync(name);

        if (userResult.IsFailed) return false;

        var user = userResult.Value;

        if (user.Name != name) return false;

        return user.PasswordHash == PasswordHasher.Encrypt(password);
    }
}
