using System.Net;
using System.Web;
using XChat.Api.Helpers.Dto;
using XChat.Api.Helpers.Dto.Room;
using XChat.Api.Helpers.Http;
using XChat.Api.Models;
using XChat.Api.Services.Room;

namespace XChat.Api.Controllers;

internal class RoomController
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
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

        var dtoList = result.Value!.Select(r => new RoomResponse(r.Id, r.Name)).ToList();
        
        return new Response<List<RoomResponse>>(HttpStatusCode.OK, dtoList);
    }

    public async Task<Response> CreateChat(Request request)
    {
        var createChatResult = DtoHelper.DeserializeRequest<CreateRoomRequest>(request.Body);

        if (createChatResult.IsFailed)
            return new Response<string>(HttpStatusCode.BadRequest, string.Join("; ", createChatResult.Errors));

        var createChat = createChatResult.Value;

        if (string.IsNullOrWhiteSpace(createChat.Name))
            return new Response<string>(HttpStatusCode.BadRequest, "Chat name cannot be empty");

        var roomResult = Room.CreateGroup(createChat.Name);

        if (roomResult.IsFailed)
            return new Response<string>(HttpStatusCode.BadRequest, string.Join("; ", roomResult.Errors));

        var room = roomResult.Value;
        var createRoomResult = await _roomService.CreateAsync(room);

        if (createRoomResult.IsFailed)
            return new Response<string>(HttpStatusCode.InternalServerError, string.Join("; ", createRoomResult.Errors));

        var dto = new RoomResponse(room.Id, room.Name);

        return new Response<RoomResponse>(HttpStatusCode.OK, dto);
    }
}
