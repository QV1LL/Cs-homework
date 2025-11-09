namespace XChat.Api.Models;

internal class Message
{
    public Guid Id { get; private set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid RoomId { get; set; }
    public Room Room { get; set; }
    public string Text { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public Message() { }

    public Message(User user, string text)
    {
        Id = Guid.NewGuid();
        User = user;
        Text = text;
        CreatedAt = DateTimeOffset.Now;
    }
}
