namespace Task3.Domain.ValueObjects
{
    internal class FullName(string firstName, string lastName, string? middleName = null)
    {
        public string FirstName { get; } = firstName;
        public string? MiddleName { get; } = middleName;
        public string LastName { get; } = lastName;
        public override string ToString() => $"{this.LastName} {this.FirstName} {this.MiddleName}";
    }
}
