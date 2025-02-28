namespace Task2.Entities;

internal class MicrowaveOven : Device
{
    public MicrowaveOven(string title, string description, string soundKey) : base(title, description, soundKey)
    {

    }

    public override void PrintSubtitlesToSound()
        => Console.WriteLine("(Microwave oven sounds)");
}
