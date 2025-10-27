namespace XChat.Api.Models;

internal class User
{
    public Guid Id { get; private set; }
    public string Name
    {
        get => field;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            field = value;
        }
    }
    public string PasswordHash
    {
        get => field;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            field = value;
        }
    }
    public List<Message> Messages { get; set; }

    public User() { }

    public User(string name, string passwordHash)
    {
        Id = Guid.NewGuid();
        Name = name;
        PasswordHash = passwordHash;
    }
}
