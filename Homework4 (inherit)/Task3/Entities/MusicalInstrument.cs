namespace Task3.Entities;

internal abstract class MusicalInstrument
{
    public string Title
    {
        get => field;
        init
        {
            if (value.IsWhiteSpace() || value == string.Empty || value == null)
                throw new ArgumentNullException("Title cannot contain empty, null or white space string");

            field = value;
        }
    }
    public string Descriprion
    {
        get => field;
        init
        {
            if (value.IsWhiteSpace() || value == string.Empty || value == null)
                throw new ArgumentNullException("Description cannot contain empty, null or white space string");

            field = value;
        }
    }
    public string History
    {
        get => field;
        init
        {
            if (value.IsWhiteSpace() || value == string.Empty || value == null)
                throw new ArgumentNullException("History cannot contain empty, null or white space string");

            field = value;
        }
    }

    protected MusicalInstrument(string title, string descriprion, string history)
    {
        Title = title;
        Descriprion = descriprion;
        History = history;
    }

    public abstract void PlaySound();
}
