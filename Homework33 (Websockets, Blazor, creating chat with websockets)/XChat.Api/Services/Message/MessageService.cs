using FluentResults;
using Microsoft.EntityFrameworkCore;
using XChat.Api.Persistence;

namespace XChat.Api.Services.Message;

internal class MessageService : IMessageService
{
    private readonly XChatContext _context;

    public MessageService(XChatContext context)
    {
        _context = context;
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

    public async Task<Result<Models.Message>> CreateAsync(Models.Message message)
    {
        try
        {
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

    public async Task<Result<IEnumerable<Models.Message>>> GetRecentAsync(int count)
    {
        try
        {
            var messages = await _context.Messages
                .Include(m => m.User)
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

    public async Task<Result<IEnumerable<Models.Message>>> GetOlderAsync(DateTimeOffset before, int count)
    {
        try
        {
            var messages = await _context.Messages
                .Include(m => m.User)
                .Where(m => m.CreatedAt < before)
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
