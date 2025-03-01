namespace Task5.Entities;

internal class Course(string name, TimeSpan duration)
{
    public string Name
    {
        get => field;
        set
        {
            if (value.IsWhiteSpace() || value == string.Empty || value == null)
                throw new ArgumentNullException("Title cannot contain empty, null or white space string");

            field = value;
        }
    } = name;
    public TimeSpan Duration { get; set; } = duration;

    public override string ToString() => $"Course name: {Name}\nDuration: {Duration}\n";
}
