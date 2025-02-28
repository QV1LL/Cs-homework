namespace Task2.Entities;

internal abstract class Device
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public virtual string Title
    {
        get => field;
        set
        {
            if (value.IsWhiteSpace() || value == string.Empty || value == null)
                throw new ArgumentNullException("Title cannot contain empty, null or white space string");

            field = value;
        }
    }
    public virtual string Description
    {
        get => field;
        set
        {
            if (value.IsWhiteSpace() || value == string.Empty || value == null)
                throw new ArgumentNullException("Description cannot contain empty, null or white space string");

            field = value;
        }
    }

    public string SoundKey
    {
        get => field;
        set
        {
            if (value.IsWhiteSpace() || value == string.Empty || value == null)
                throw new ArgumentNullException("Sound name cannot be null");

            field = value;
        }
    }

    protected Device(string title, string description, string soundKey)
    {
        Title = title;
        Description = description;
        SoundKey = soundKey;
    }

    public abstract void PrintSubtitlesToSound();
}
