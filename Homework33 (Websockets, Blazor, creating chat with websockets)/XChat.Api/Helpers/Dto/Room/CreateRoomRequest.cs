namespace XChat.Api.Helpers.Dto.Room;

internal record CreateRoomRequest
(
    Guid OwnerId, 
    string Name
);
