using Task8.Entities;
using Task8.ValueObjects;

namespace Task8;

internal static class Program
{
    static void Main(string[] args)
    {
        var studentStatistics = new List<StudentStatistic>
        {
            new StudentStatistic(
                new Student("Bohdan"),
                new List<StudentGrade>
                    {
                        new StudentGrade(100, "Math"),
                        new StudentGrade(80, "History"),
                        new StudentGrade(90, "Chemistry")
                    }
                ),

            new StudentStatistic(
                new Student("Pishka"),
                new List<StudentGrade>
                    {
                        new StudentGrade(100, "Math"),
                        new StudentGrade(100, "History"),
                        new StudentGrade(100, "Chemistry"),
                        new StudentGrade(99, "C++"),
                        new StudentGrade(100, "CSharp"),
                    }
                )
        };

        foreach (var studentStatistic in studentStatistics)
        {
            Console.WriteLine($"Max {studentStatistic.Student.Name} grade: {studentStatistic.MaxGrade?.Grade ?? 0}");
            Console.WriteLine($"Average {studentStatistic.Student.Name} grade: {studentStatistic.AverageGrade}");
        }
    }
}
