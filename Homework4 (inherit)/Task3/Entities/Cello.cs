namespace Task3.Entities;

internal class Cello : MusicalInstrument
{
    public Cello(string title, string descriprion, string history)
        : base(title, descriprion, history)
    {

    }

    public override void PlaySound() => Console.WriteLine("Cello is playing in theater: (whwiwhwiwihwiw)");
}
