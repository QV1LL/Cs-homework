using FluentResults;

namespace XChat.Api.Services.User;

internal interface IUserService
{
    Task<Result<IEnumerable<Models.User>>> GetAllAsync();
    Task<Result<Models.User>> GetByIdAsync(Guid id);
    Task<Result<Models.User>> GetByUsernameAsync(string username);
    Task<Result<Models.User>> CreateAsync(Models.User user);
    Task<Result<Models.User>> UpdateAsync(Models.User user);
    Task<bool> DeleteAsync(Guid id);
}
