namespace Task3.Entities;

internal class Violin : MusicalInstrument
{
    public Violin(string title, string descriprion, string history) 
        : base(title, descriprion, history)
    {

    }

    public override void PlaySound() => Console.WriteLine("Violin is playing in orchester: (shshshshshhshshshshshsisishisihsis)");
}
