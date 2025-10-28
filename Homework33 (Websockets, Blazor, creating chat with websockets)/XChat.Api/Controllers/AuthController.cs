using System.Net;
using XChat.Api.Helpers.Dto;
using XChat.Api.Helpers.Dto.User;
using XChat.Api.Helpers.Hasher;
using XChat.Api.Helpers.Http;
using XChat.Api.Models;
using XChat.Api.Services.User;

namespace XChat.Api.Controllers;

internal class AuthController
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Response> RegisterAsync(Request request)
    {
        var registerRequestResult = DtoHelper.DeserializeRequest<AuthRequest>(request.Body);

        if (registerRequestResult.IsFailed)
            return new Response<string>(HttpStatusCode.BadRequest, string.Join("; ", registerRequestResult.Errors));

        var registerRequest = registerRequestResult.Value;
        var userResult = await _userService.GetByUsernameAsync(registerRequest.Name);

        if (userResult.IsSuccess)
            return new Response<string>(HttpStatusCode.BadRequest, $"User with name '{registerRequest.Name}' is already registered");

        var user = new User(registerRequest.Name, PasswordHasher.Encrypt(registerRequest.Password));
        var createUserResult = await _userService.CreateAsync(user);

        if (createUserResult.IsFailed)
            return new Response<string>(HttpStatusCode.BadRequest, string.Join("; ", createUserResult.Errors));

        var registerResponse = new AuthResponse(user.Name);

        return new Response<AuthResponse>(HttpStatusCode.OK, registerResponse);
    }

    public async Task<Response> LoginAsync(Request request)
    {
        var registerRequestResult = DtoHelper.DeserializeRequest<AuthRequest>(request.Body);

        if (registerRequestResult.IsFailed)
            return new Response<string>(HttpStatusCode.BadRequest, string.Join("; ", registerRequestResult.Errors));

        var registerRequest = registerRequestResult.Value;

        var isVerified = await _userService.VerifyUser(registerRequest.Name, registerRequest.Password);

        if (!isVerified)
            return new Response<string>(HttpStatusCode.BadRequest, "Authentication failed");

        var response = new AuthResponse(registerRequest.Name);

        return new Response<AuthResponse>(HttpStatusCode.OK, response);
    }
}
