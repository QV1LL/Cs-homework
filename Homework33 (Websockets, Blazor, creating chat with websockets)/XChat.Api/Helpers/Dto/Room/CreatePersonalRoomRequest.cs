namespace XChat.Api.Helpers.Dto.Room;

internal record CreatePersonalRoomRequest
(
    string RequestUserName,
    string AnotherUserName
);
