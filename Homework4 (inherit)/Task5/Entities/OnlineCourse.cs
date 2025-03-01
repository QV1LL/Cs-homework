namespace Task5.Entities;

internal class OnlineCourse : Course
{
    public string Platform
    {
        get => field;
        set
        {
            if (value.IsWhiteSpace() || value == string.Empty || value == null)
                throw new ArgumentNullException("Title cannot contain empty, null or white space string");

            field = value;
        }
    }

    public OnlineCourse(string name, TimeSpan duration, string platform)
        : base(name, duration)
    {
        Platform = platform;
    }

    public override string ToString() => base.ToString() + $"Platform: {Platform}";
}
