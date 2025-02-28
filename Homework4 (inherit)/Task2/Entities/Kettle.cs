using System.Diagnostics;

namespace Task2.Entities;

internal class Kettle : Device
{
    public Kettle(string title, string description, string soundKey) : base(title, description, soundKey)
    {

    }

    public override void PrintSubtitlesToSound() 
        => Console.WriteLine("(Kettle sounds)");
}
