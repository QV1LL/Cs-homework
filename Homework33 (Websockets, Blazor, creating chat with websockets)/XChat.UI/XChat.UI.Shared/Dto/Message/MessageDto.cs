namespace XChat.UI.Shared.Dto.Message;

public record MessageDto
(
    string User,
    string Text,
    DateTimeOffset CreatedAt
);
