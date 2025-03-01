using Task4.Entities;

namespace Task4;

internal static class Program
{
    static void Main(string[] args)
    {
        var workers = new List<IWorker> { new President(), new Manager(), new Security(), new Engineer() };

        foreach (var worker in workers)
            worker.Print();
    }
}
