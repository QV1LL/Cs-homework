using XChat.UI.Shared.Enums;

namespace XChat.UI.Shared.Models;

public class Room
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public RoomType Type { get; set; }
}
