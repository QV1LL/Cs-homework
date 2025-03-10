namespace Task8.ValueObjects;

internal record class StudentGrade(int grade, string lesson) : IComparable<StudentGrade>
{
    public int Grade
    {
        get => grade;
        init
        {
            if (value < 0 || value > 100)
                throw new ArgumentOutOfRangeException("Grade must be in range from 0 to 100");

            grade = value;
        }
    }

    public string Lesson
    {
        get => lesson;
        init
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("Lesson property must be a non empty string");

            lesson = value;
        }
    }

    public int CompareTo(StudentGrade? other)
    {
        if (other == null) return 1;
        if (this == other) return 0;

        return Grade.CompareTo(other.Grade);
    }
}
