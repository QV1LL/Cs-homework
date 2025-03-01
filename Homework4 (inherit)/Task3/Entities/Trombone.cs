namespace Task3.Entities;

internal class Trombone : MusicalInstrument
{
    public Trombone(string title, string descriprion, string history)
        : base(title, descriprion, history)
    {

    }

    public override void PlaySound() => Console.WriteLine("Trombone is playing in orchester: (rbtbttbtbttbtbtbtbtb)");
}
