using System.Net;
using System.Web;
using XChat.Api.Helpers.Dto;
using XChat.Api.Helpers.Dto.Room;
using XChat.Api.Helpers.Http;
using XChat.Api.Models;
using XChat.Api.Services.Room;
using XChat.Api.Services.User;

namespace XChat.Api.Controllers;

internal class RoomController
{
    private readonly IRoomService _roomService;
    private readonly IUserService _userService;

    public RoomController(IRoomService roomService, IUserService userService)
    {
        _roomService = roomService;
        _userService = userService;
    }

    public async Task<Response> GetUserRooms(Request request)
    {
        var uri = new Uri("http://dummy" + request.Route);
        var query = HttpUtility.ParseQueryString(uri.Query);
        
        if (!Guid.TryParse(query["userId"], out var userId))
            return new Response<string>(HttpStatusCode.BadRequest, "Invalid or missing userId");

        var result = await _roomService.GetByUserIdAsync(userId);

        if (result.IsFailed)
            return new Response<string>(HttpStatusCode.InternalServerError, string.Join("; ", result.Errors));

        var responseList = result.Value!.Select(r => new CreateRoomResponse(r.Id, r.Name, r.Type)).ToList();
        
        return new Response<List<CreateRoomResponse>>(HttpStatusCode.OK, responseList);
    }

    public async Task<Response> AddUserToChat(Request request)
    {
        var uri = new Uri("http://dummy" + request.Route);
        var query = HttpUtility.ParseQueryString(uri.Query);

        if (!Guid.TryParse(query["roomId"], out var roomId))
            return new Response<string>(HttpStatusCode.BadRequest, "Invalid or missing roomId");

        var addUserToChatRequestResult = DtoHelper.DeserializeRequest<AddUserToChatRequest>(request.Body);

        if (addUserToChatRequestResult.IsFailed)
            return new Response<string>(HttpStatusCode.BadRequest, string.Join("; ", addUserToChatRequestResult.Errors));

        var addRequest = addUserToChatRequestResult.Value;
        var roomResult = await _roomService.GetByIdAsync(roomId);

        if (roomResult.IsFailed)
            return new Response<string>(HttpStatusCode.NotFound, string.Join("; ", roomResult.Errors));

        var userResult = await _userService.GetByUsernameAsync(addRequest.Username);

        if (userResult.IsFailed)
            return new Response<string>(HttpStatusCode.NotFound, string.Join("; ", userResult.Errors));

        var user = userResult.Value;
        var addResult = await _roomService.AddUserToRoom(roomResult.Value, user);

        if (addResult.IsFailed)
            return new Response<string>(HttpStatusCode.NotFound, string.Join("; ", addResult.Errors));

        return new Response<string>(HttpStatusCode.OK, $"Success add user '{user.Name}' to chat");
    }

    public async Task<Response> CreateChat(Request request)
    {
        var createChatResult = DtoHelper.DeserializeRequest<CreateRoomRequest>(request.Body);

        if (createChatResult.IsFailed)
            return new Response<string>(HttpStatusCode.BadRequest, string.Join("; ", createChatResult.Errors));

        var createChat = createChatResult.Value;

        if (string.IsNullOrWhiteSpace(createChat.Name))
            return new Response<string>(HttpStatusCode.BadRequest, "Chat name cannot be empty");

        var ownerResult = await _userService.GetByIdAsync(createChat.OwnerId);

        if (ownerResult.IsFailed)
            return new Response<string>(HttpStatusCode.BadRequest, string.Join("; ", ownerResult.Errors));

        var roomResult = Room.CreateGroup(createChat.Name);

        if (roomResult.IsFailed)
            return new Response<string>(HttpStatusCode.BadRequest, string.Join("; ", roomResult.Errors));

        var room = roomResult.Value;
        room.Users.Add(ownerResult.Value);
        var createRoomResult = await _roomService.CreateAsync(room);

        if (createRoomResult.IsFailed)
            return new Response<string>(HttpStatusCode.InternalServerError, string.Join("; ", createRoomResult.Errors));

        var response = new CreateRoomResponse(room.Id, room.Name, room.Type);

        return new Response<CreateRoomResponse>(HttpStatusCode.OK, response);
    }

    public async Task<Response> CreatePersonalChat(Request request)
    {
        var createChatResult = DtoHelper.DeserializeRequest<CreatePersonalRoomRequest>(request.Body);

        if (createChatResult.IsFailed)
            return new Response<string>(HttpStatusCode.BadRequest, string.Join("; ", createChatResult.Errors));

        var createChat = createChatResult.Value;

        if (createChat.RequestUserName == createChat.AnotherUserName)
            return new Response<string>(HttpStatusCode.BadRequest, "Cannot create chat between the same user");

        var requestUserResult = await _userService.GetByUsernameAsync(createChat.RequestUserName);

        if (requestUserResult.IsFailed)
            return new Response<string>(HttpStatusCode.NotFound, string.Join("; ", requestUserResult.Errors));

        var anotherUserResult = await _userService.GetByUsernameAsync(createChat.AnotherUserName);

        if (anotherUserResult.IsFailed)
            return new Response<string>(HttpStatusCode.NotFound, string.Join("; ", anotherUserResult.Errors));

        var requestUser = requestUserResult.Value;
        var anotherUser = anotherUserResult.Value;

        var roomResult = Room.CreatePersonalChat(requestUser, anotherUser);

        if (roomResult.IsFailed)
            return new Response<string>(HttpStatusCode.BadRequest, string.Join("; ", roomResult.Errors));

        var room = roomResult.Value;
        var createRoomResult = await _roomService.CreateAsync(room);

        if (createRoomResult.IsFailed)
            return new Response<string>(HttpStatusCode.BadRequest, string.Join("; ", createRoomResult.Errors));

        var response = new CreateRoomResponse(room.Id, room.Name, room.Type);

        return new Response<CreateRoomResponse>(HttpStatusCode.OK, response);
    }
}
