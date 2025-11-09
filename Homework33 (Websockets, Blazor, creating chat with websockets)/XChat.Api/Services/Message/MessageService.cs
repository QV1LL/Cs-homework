using FluentResults;
using Microsoft.EntityFrameworkCore;
using XChat.Api.Persistence;
using XChat.Api.Services.Room;

namespace XChat.Api.Services.Message;

internal class MessageService : IMessageService
{
    private readonly IRoomService _roomService;
    private readonly XChatContext _context;

    public MessageService(XChatContext context, IRoomService roomService)
    {
        _context = context;
        _roomService = roomService;
    }

    public async Task<Result<IEnumerable<Models.Message>>> GetAllAsync()
    {
        try
        {
            var messages = await _context.Messages.ToListAsync();
            return Result.Ok(messages.AsEnumerable());
        }
        catch (Exception ex)
        {
            return Result.Fail($"Failed to get all messages: {ex.Message}");
        }
    }

    public async Task<Result<Models.Message>> GetByIdAsync(Guid id)
    {
        try
        {
            var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
                return Result.Fail("Message not found");

            return Result.Ok(message);
        }
        catch (Exception ex)
        {
            return Result.Fail($"Failed to get message by id: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<Models.Message>>> GetByUserAsync(Guid userId)
    {
        try
        {
            var messages = await _context.Messages
                .Where(m => m.UserId == userId)
                .ToListAsync();
            return Result.Ok(messages.AsEnumerable());
        }
        catch (Exception ex)
        {
            return Result.Fail($"Failed to get messages by user: {ex.Message}");
        }
    }

    public async Task<Result<Models.Message>> CreateAsync(Models.Message message, Guid chatId)
    {
        try
        {
            var roomResult = await _roomService.GetByIdAsync(chatId);

            if (roomResult.IsFailed) return roomResult.ToResult();

            var room = roomResult.Value;

            room.Messages.Add(message);
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return Result.Ok(message);
        }
        catch (Exception ex)
        {
            return Result.Fail($"Failed to create message: {ex.Message}");
        }
    }

    public async Task<Result<Models.Message>> UpdateAsync(Models.Message message)
    {
        try
        {
            var existing = await _context.Messages.FindAsync(message.Id);
            if (existing == null)
                return Result.Fail("Message not found");

            _context.Entry(existing).CurrentValues.SetValues(message);
            await _context.SaveChangesAsync();
            return Result.Ok(message);
        }
        catch (Exception ex)
        {
            return Result.Fail($"Failed to update message: {ex.Message}");
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
                return false;

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<Result<IEnumerable<Models.Message>>> GetRecentAsync(int count, Guid chatId)
    {
        try
        {
            var messages = await _context.Messages
                .Include(m => m.User)
                .Include(m => m.Room)
                .Where(m => m.RoomId == chatId)
                .OrderByDescending(m => m.CreatedAt)
                .Take(count)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();

            return Result.Ok(messages.AsEnumerable());
        }
        catch (Exception ex)
        {
            return Result.Fail($"Failed to get recent messages: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<Models.Message>>> GetOlderAsync(DateTimeOffset before, int count, Guid chatId)
    {
        try
        {
            var messages = await _context.Messages
                .Include(m => m.User)
                .Include(m => m.Room)
                .Where(m => m.CreatedAt < before && m.RoomId == chatId)
                .OrderByDescending(m => m.CreatedAt)
                .Take(count)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();

            return Result.Ok(messages.AsEnumerable());
        }
        catch (Exception ex)
        {
            return Result.Fail($"Failed to get older messages: {ex.Message}");
        }
    }
}
