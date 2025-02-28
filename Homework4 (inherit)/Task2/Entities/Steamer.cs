namespace Task2.Entities;

internal class Steamer : Vehicle
{
    public Steamer(string title, string description, string soundKey, int workDelay) : base(title, description, soundKey, workDelay)
    {

    }

    public override void PrintSubtitlesToSound()
    {
        Thread.Sleep(WorkDelay);
        Console.Write("(sound of horn)");
    }
}
