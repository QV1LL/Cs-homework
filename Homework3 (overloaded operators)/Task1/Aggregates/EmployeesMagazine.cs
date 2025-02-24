using Task1.Entities;

namespace Task1.Aggregates;

internal class EmployeesMagazine : IComparable<EmployeesMagazine>, IEquatable<EmployeesMagazine>
{
    public Guid Id { get; init; } = Guid.NewGuid();
    private List<Employee> Employees { get; set; }
    public int EmployeesCount { get => this.Employees.Count; }

    public EmployeesMagazine() : this(new())
    {

    }
    public EmployeesMagazine(List<Employee> employees) => Employees = employees;

    public int CompareTo(EmployeesMagazine? other)
    {
        if (other == null) return 1;
        return this.EmployeesCount.CompareTo(other.EmployeesCount);
    }

    public bool Equals(EmployeesMagazine? other) => this.Employees.Equals(other?.Employees);

    public override bool Equals(object? obj) => Equals(obj as EmployeesMagazine);

    public override int GetHashCode() => Employees.GetHashCode();

    public static bool operator <(EmployeesMagazine magazine1, EmployeesMagazine magazine2) 
        => magazine1.EmployeesCount < magazine2.EmployeesCount;

    public static bool operator >(EmployeesMagazine magazine1, EmployeesMagazine magazine2)
        => !(magazine1 < magazine2);

    public static bool operator <=(EmployeesMagazine magazine1, EmployeesMagazine magazine2)
        => magazine1.EmployeesCount <= magazine2.EmployeesCount;

    public static bool operator >=(EmployeesMagazine magazine1, EmployeesMagazine magazine2)
        => !(magazine1 <= magazine2);

    public static bool operator ==(EmployeesMagazine? magazine1, EmployeesMagazine? magazine2)
        => magazine1?.EmployeesCount == magazine2?.EmployeesCount;

    public static bool operator !=(EmployeesMagazine? magazine1, EmployeesMagazine? magazine2)
        => !(magazine1 == magazine2);

    public static EmployeesMagazine operator +(EmployeesMagazine magazine1, List<Employee> employees)
        => new EmployeesMagazine(magazine1.Employees.Concat(employees).ToList());

    public static EmployeesMagazine operator -(EmployeesMagazine magazine1, int quantity)
        => new EmployeesMagazine(magazine1.Employees.FindAll(employee => magazine1.Employees.IndexOf(employee) < magazine1.EmployeesCount - quantity));
}
