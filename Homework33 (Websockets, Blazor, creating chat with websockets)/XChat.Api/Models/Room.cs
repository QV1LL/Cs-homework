using FluentResults;
using XChat.Api.Enums;

namespace XChat.Api.Models;

internal class Room
{
    public Guid Id { get; private set; }
    public string Name
    {
        get => field;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Name cannot be null");

            field = value;
        }
    }
    public RoomType Type { get; private set; }
    public List<User> Users { get; private set; } = [];
    public List<User> BannedUsers { get; private set; } = [];
    public List<Message> Messages { get; private set; } = [];

    public Room()
    {
        
    }

    private Room(string name, RoomType type = RoomType.Personal)
    {
        Id = Guid.NewGuid();
        Name = name;
        Type = type;
    }

    public static Result<Room> CreateGroup(string name)
    {
        try
        {
            return new Room(name, RoomType.Group);
        }
        catch(Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public static Result<Room> CreatePersonalChat(User user1, User user2)
    {
        try
        {
            var personalChat = new Room($"{user1.Name} and {user2.Name}", RoomType.Personal);
            personalChat.Users.AddRange(user1, user2);
            return personalChat;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}
