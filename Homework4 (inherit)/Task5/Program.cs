using Task5.Entities;

namespace Task5;

internal class Program
{
    static void Main(string[] args)
    {
        var course = new Course("C++ from zero to minus one", new TimeSpan(1, 59, 59));
        var onlineCourse = new OnlineCourse("C# iz hrazy v hrazy (za 10 hodin)", new TimeSpan(10, 0, 0), "Zoom");

        Console.WriteLine(course);
        Console.WriteLine(onlineCourse);
    }
}
