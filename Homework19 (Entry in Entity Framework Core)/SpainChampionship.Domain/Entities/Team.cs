namespace SpainChampionship.Domain.Entities;

public class Team
{
    public Guid? Id { get; set; }

    public string Name
    {
        get => field;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(Name));

            field = value;
        }
    }

    public string City
    {
        get => field;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(Name));

            field = value;
        }
    }

    public int CountOfVictories
    {
        get => field;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(CountOfVictories)); 

            field = value;
        }
    }

    public int CountOfDefeats
    {
        get => field;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(CountOfDefeats)); 

            field = value;
        }
    }

    public int CountOfDraws
    {
        get => field;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(CountOfDraws)); 

            field = value;
        }
    }

    public int CountOfGoals
    {
        get => field;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(CountOfGoals));

            field = value;
        }
    }

    public int CountOfSkippedGoals
    {
        get => field;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(CountOfSkippedGoals));

            field = value;
        }
    }
}