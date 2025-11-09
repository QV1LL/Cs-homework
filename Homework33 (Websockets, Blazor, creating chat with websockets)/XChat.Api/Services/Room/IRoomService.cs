using FluentResults;

namespace XChat.Api.Services.Room;

internal interface IRoomService
{
    Task<Result<IEnumerable<Models.Room>>> GetAllAsync();
    Task<Result<Models.Room>> GetByIdAsync(Guid id);
    Task<Result<IEnumerable<Models.Room>>> GetByUserIdAsync(Guid userId);
    Task<Result<Models.Room>> CreateAsync(Models.Room room);
    Task<Result<Models.Room>> UpdateAsync(Models.Room room);
    Task<bool> DeleteAsync(Guid id);

    Task<Result> AddUserToRoom(Models.Room room, Models.User user);
}
