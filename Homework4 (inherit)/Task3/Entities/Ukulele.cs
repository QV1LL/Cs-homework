namespace Task3.Entities;

internal class Ukulele : MusicalInstrument
{
    public Ukulele(string title, string descriprion, string history)
        : base(title, descriprion, history)
    {

    }

    public override void PlaySound() => Console.WriteLine("Ukulele is playing outdoor: (sound of ukulele)");
}
