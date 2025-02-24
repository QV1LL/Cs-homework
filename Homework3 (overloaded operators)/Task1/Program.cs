using Task1.Aggregates;
using Task1.Entities;

namespace Task1;

public static class Program
{
    public static void Main(string[] args)
    {
        TestMagazine();
    }

    private static void TestMagazine()
    {
        var magazine = new EmployeesMagazine();

        magazine = magazine + new List<Employee> { new Employee("name", DateTime.MinValue), new Employee("name", DateTime.MinValue), new Employee("name", DateTime.MinValue) };
        magazine = magazine - 2;
        Console.WriteLine(magazine.EmployeesCount);
    }

    private static void TestShop()
    {

    }
}