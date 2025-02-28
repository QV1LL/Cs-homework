namespace Task2.Entities;

internal class Car : Vehicle
{
    public override string Title
    {
        get => $"Car title: {field}";
        set
        {
            if (value.IsWhiteSpace() || value == string.Empty || value == null)
                throw new ArgumentNullException("Title cannot contain empty, null or white space string");

            field = value;
        }
    }
    public override string Description
    {
        get => $"Car description: {field}";
        set
        {
            if (value.IsWhiteSpace() || value == string.Empty || value == null)
                throw new ArgumentNullException("Description cannot contain empty, null or white space string");

            field = value;
        }
    }

    public Car(string title, string description, string soundKey, int workDelay) : base(title, description, soundKey, workDelay)
    {

    }

    public override void PrintSubtitlesToSound()
    {
        Thread.Sleep(WorkDelay);
        Console.Write("br..");
    }
}
