using System.Threading.Channels;

namespace Task4.Entities;

internal class Engineer : IWorker
{
    public void Print() => Console.WriteLine("I`m worker");
}
