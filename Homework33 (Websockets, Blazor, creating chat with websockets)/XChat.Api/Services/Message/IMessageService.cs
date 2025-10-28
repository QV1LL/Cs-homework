using FluentResults;

namespace XChat.Api.Services.Message;

internal interface IMessageService
{
    Task<Result<IEnumerable<Models.Message>>> GetAllAsync();
    Task<Result<Models.Message>> GetByIdAsync(Guid id);
    Task<Result<IEnumerable<Models.Message>>> GetByUserAsync(Guid userId);
    Task<Result<Models.Message>> CreateAsync(Models.Message message);
    Task<Result<Models.Message>> UpdateAsync(Models.Message message);
    Task<bool> DeleteAsync(Guid id);

    Task<Result<IEnumerable<Models.Message>>> GetRecentAsync(int count);
    Task<Result<IEnumerable<Models.Message>>> GetOlderAsync(DateTimeOffset before, int count);
}
