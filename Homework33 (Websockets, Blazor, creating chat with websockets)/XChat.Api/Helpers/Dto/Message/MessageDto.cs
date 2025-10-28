namespace XChat.Api.Helpers.Dto.Message;

public record MessageDto(
    string User, 
    string Text, 
    DateTimeOffset CreatedAt
);

