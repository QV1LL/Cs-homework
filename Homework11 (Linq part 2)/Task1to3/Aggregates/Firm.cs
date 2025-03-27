using Task1to3.Entities;

namespace Task1to3.Aggregates;

internal class Firm
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; }
    public DateTime FoundingDate { get; set; }
    public string BusinessProfile { get; set; }
    public string DirectorFullName { get; set; }
    public int EmployeeCount { get; set; }
    public string Address { get; set; }
    public List<EmployeeInfo> EmployeeInfos { get; private set; }

    public Firm(string name, DateTime foundingDate, string businessProfile,
                string directorFullName, int employeeCount, string address, 
                List<EmployeeInfo>? employeeInfos = null)
    {
        Name = name;
        FoundingDate = foundingDate;
        BusinessProfile = businessProfile;
        DirectorFullName = directorFullName;
        EmployeeCount = employeeCount;
        Address = address;
        EmployeeInfos = employeeInfos ?? new();
    }

    public override string ToString()
            => $"Firm: {Name}\n" +
               $"Founding Date: {FoundingDate:yyyy-MM-dd}\n" +
               $"Business Profile: {BusinessProfile}\n" +
               $"Director: {DirectorFullName}\n" +
               $"Number of Employees: {EmployeeCount}\n" +
               $"Address: {Address}\n";
}
