using Task8.ValueObjects;

namespace Task8.Entities;

internal class StudentStatistic
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public Student Student { get; set; }

    public List<StudentGrade> grades { get; private set; }

    public StudentGrade? MaxGrade { get => grades.Max(); }

    public int AverageGrade
    {
        get
        {
            int sum = 0;

            foreach (var grade in grades) sum += grade.Grade;

            return sum / grades.Count;
        }
    }

    public StudentStatistic(Student student, List<StudentGrade> grades)
    {
        Student = student;
        this.grades = grades;
    }
}
