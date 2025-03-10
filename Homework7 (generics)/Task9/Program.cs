using Task9.ValueObjects;

namespace Task9;

internal static class Program
{
    static void Main(string[] args)
    {
        var employees = new List<Employee>()
        {
            new Employee("Andriy", "Pishka", 1_000_000),
            new Employee("Starii", "Bog", 1),
            new Employee("Some", "Guy", 25_000)
        };

        var employeeWithMinSallary = employees.Min();
        Console.WriteLine($"Min salary has {employeeWithMinSallary.Surname}: {employeeWithMinSallary.Sallary}");

        var employeeWithMaxSallary = employees.Max();
        Console.WriteLine($"Max salary has {employeeWithMaxSallary.Surname}: {employeeWithMaxSallary.Sallary}");
    }
}
