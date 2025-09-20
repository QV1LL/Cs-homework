using System.Drawing;

namespace HorseRaces.Domain;

internal class Horse
{
    public string Name { get; }
    public float DistanceCompleted { get; private set; }
    public Color Color { get; }

    public Horse(string name, Color color)
    {
        Name = name;
        Color = color;
        DistanceCompleted = 0;
    }

    public void RunStep(float percentOfCompletedDistance)
    {
        if (DistanceCompleted + percentOfCompletedDistance >= 100)
        {
            DistanceCompleted = 100;
        }
        else
        {
            DistanceCompleted += percentOfCompletedDistance;
        }
    }

    public void Reset() => DistanceCompleted = 0;
}
