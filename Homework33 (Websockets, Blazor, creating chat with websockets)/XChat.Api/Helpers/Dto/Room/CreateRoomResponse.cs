using XChat.Api.Enums;

namespace XChat.Api.Helpers.Dto.Room;

internal record CreateRoomResponse
(
    Guid Id,
    string Name,
    RoomType Type
);
