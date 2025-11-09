using FluentResults;
using Microsoft.EntityFrameworkCore;
using XChat.Api.Persistence;

namespace XChat.Api.Services.Room;

internal class RoomService : IRoomService
{
    private readonly XChatContext _context;

    public RoomService(XChatContext context)
    {
        _context = context;
    }
    
    public async Task<Result<IEnumerable<Models.Room>>> GetAllAsync()
    {
        try
        {
            var rooms = await _context.Rooms
                .Include(r => r.Users)
                .Include(r => r.BannedUsers)
                .Include(r => r.Messages)
                .ToListAsync();
            return Result.Ok(rooms.AsEnumerable());
        }
        catch (Exception ex)
        {
            return Result.Fail($"Failed to get all rooms: {ex.Message}");
        }
    }

    public async Task<Result<Models.Room>> GetByIdAsync(Guid id)
    {
        try
        {
            var room = await _context.Rooms
                .Include(r => r.Users)
                .Include(r => r.BannedUsers)
                .Include(r => r.Messages)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
                return Result.Fail("Room not found");

            return Result.Ok(room);
        }
        catch (Exception ex)
        {
            return Result.Fail($"Failed to get room by id: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<Models.Room>>> GetByUserIdAsync(Guid userId)
    {
        try
        {
            var rooms = await _context.Rooms
                .Include(r => r.Users)
                .Where(r => r.Users.Any(u => u.Id == userId))
                .ToListAsync();

            return Result.Ok(rooms.AsEnumerable());
        }
        catch (Exception ex)
        {
            return Result.Fail($"Failed to get rooms for user: {ex.Message}");
        }
    }

    public async Task<Result<Models.Room>> CreateAsync(Models.Room room)
    {
        try
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return Result.Ok(room);
        }
        catch (Exception ex)
        {
            return Result.Fail($"Failed to create room: {ex.Message}");
        }
    }

    public async Task<Result<Models.Room>> UpdateAsync(Models.Room room)
    {
        try
        {
            var existing = await _context.Rooms
                .Include(r => r.Users)
                .Include(r => r.BannedUsers)
                .Include(r => r.Messages)
                .FirstOrDefaultAsync(r => r.Id == room.Id);

            if (existing == null)
                return Result.Fail("Room not found");

            _context.Entry(existing).CurrentValues.SetValues(room);

            existing.Users.Clear();
            existing.Users.AddRange(room.Users);

            existing.BannedUsers.Clear();
            existing.BannedUsers.AddRange(room.BannedUsers);

            existing.Messages.Clear();
            existing.Messages.AddRange(room.Messages);

            await _context.SaveChangesAsync();
            return Result.Ok(room);
        }
        catch (Exception ex)
        {
            return Result.Fail($"Failed to update room: {ex.Message}");
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
                return false;

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<Result> AddUserToRoom(Models.Room room, Models.User user)
    {
        try
        {
            var existingRoom = await _context.Rooms
                .Include(r => r.Users)
                .FirstOrDefaultAsync(r => r.Id == room.Id);

            if (existingRoom == null)
                return Result.Fail("Room not found");

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (existingUser == null)
                return Result.Fail("User not found");

            if (existingRoom.Users.Any(u => u.Id == existingUser.Id))
                return Result.Fail("User already in room");

            existingRoom.Users.Add(existingUser);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Failed to add user to room: {ex.Message}");
        }
    }

}
