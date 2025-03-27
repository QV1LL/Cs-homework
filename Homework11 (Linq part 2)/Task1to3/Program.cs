using Task1to3.Aggregates;
using Task1to3.Extensions;
using Task1to3.Entities.Enums;
using Task1to3.Entities;
using Task1to3.ValueObjects;

namespace Task1to3;

internal static class Program
{
    static void Main(string[] args)
    {
        var firms = new Firm[]
        {
            new Firm(
                name: "TechVision",
                foundingDate: new DateTime(2015, 6, 15),
                businessProfile: "IT",
                directorFullName: "Ivanov Ivan Ivanovich",
                employeeCount: 45,
                address: "Kyiv, Khreshchatyk St., 25",
                employeeInfos: new List<EmployeeInfo>
                {
                    new EmployeeInfo
                        (new Name("Lionel", "Smith", "Edward"),
                        EmployeePosition.Developer,
                        "555-123-4567",
                        "john.smith@example.com",
                        new Money(60000, 0)),

                    new EmployeeInfo(
                        new Name("Alice", "Johnson"),
                        EmployeePosition.SeniorDeveloper,
                        "555-234-5678",
                        "alice.johnson@example.com",
                        new Money(80000, 0))
                }
            ),
            new Firm(
                name: "MarketPro",
                foundingDate: new DateTime(2018, 3, 22),
                businessProfile: "Marketing",
                directorFullName: "Petrova Olga Serhiyivna",
                employeeCount: 28,
                address: "Lviv, Horodotska St., 12",
                employeeInfos: new List<EmployeeInfo>()
                {
                    new EmployeeInfo(
                        new Name("Robert", "Brown", "James"),
                        EmployeePosition.Manager,
                        "555-345-6789",
                        "robert.brown@example.com",
                        new Money(95000, 0)),

                    new EmployeeInfo(
                        new Name("Lionel", "Davis"),
                        EmployeePosition.JuniorDeveloper,
                        "555-456-7890",
                        "emma.davis@example.com",
                        new Money(50000, 0)),

                    new EmployeeInfo(
                        new Name("Michael", "Carter"),
                        EmployeePosition.Director,
                        "555-567-8901",
                        "michael.carter@example.com",
                        new Money(120000, 0))
                }
            ),
            new Firm(
                name: "BuildCorp",
                foundingDate: new DateTime(2010, 11, 30),
                businessProfile: "Construction",
                directorFullName: "Sydorenko Petro Vasylovych",
                employeeCount: 150,
                address: "Odesa, Derybasivska St., 8",
                employeeInfos: new List<EmployeeInfo>()
                {
                    new EmployeeInfo(
                        new Name("Sarah", "Lee", "Anne"),
                        EmployeePosition.TeamLead,
                        "555-678-9012",
                        "sarah.lee@example.com",
                        new Money(85000, 0)),

                    new EmployeeInfo(
                        new Name("Lionel", "Wilson"),
                        EmployeePosition.SeniorManager,
                        "235-789-0123",
                        "david.wilson@example.com",
                        new Money(105000, 0)),

                    new EmployeeInfo(
                        new Name("Lisa", "Martinez", "Marie"),
                        EmployeePosition.Developer,
                        "235-890-1234",
                        "lisa.martinez@example.com",
                        new Money(62000, 0)),
                }
            ),
            new Firm(
                name: "Some corp",
                foundingDate: new DateTime(2002, 9, 12),
                businessProfile: "...",
                directorFullName: "Walter White Heisenberg",
                employeeCount: 150,
                address: "New Mexico, Armstrong St., 10",
                employeeInfos: new List<EmployeeInfo>()
                {
                    new EmployeeInfo(
                        new Name("James", "Taylor"),
                        EmployeePosition.JuniorDeveloper,
                        "555-901-2345",
                        "james.taylor@example.com",
                        new Money(48000, 0)),

                    new EmployeeInfo(
                        new Name("Emily", "Clark", "Rose"),
                        EmployeePosition.SeniorDeveloper,
                        "555-012-3456",
                        "emily.clark@example.com",
                        new Money(82000, 0)),

                    new EmployeeInfo(
                        new Name("Thomas", "Adams"),
                        EmployeePosition.Manager,
                        "555-123-5678",
                        "thomas.adams@example.com",
                        new Money(97000, 0))
                }
            )
        };

        TestLinq(firms);
        TestMyIEnumrableExtensions(firms);
        TestLinqWithEmployees(firms);
    }

    private static void TestLinq(IEnumerable<Firm> firms)
    {
        var businessProfiles = firms.Select(f => f.BusinessProfile);
        DisplayAll(businessProfiles, "Firms profiles: ");

        var firmsWhichContainFood = firms.Where(f => f.Name.ToLower().Contains("food"));
        DisplayAll(firmsWhichContainFood, "Containing food firms: ");

        var marketingFirms = firms.Where(f => f.BusinessProfile == "Marketing");
        DisplayAll(marketingFirms, "Marketing firms: ");

        var marketingOrITFirms = firms.Where(f => f.BusinessProfile == "Marketing" ||
                                                  f.BusinessProfile == "IT");
        DisplayAll(marketingOrITFirms, "Marketing or IT firms: ");

        var Above100EmployeeCountFirms = firms.Where(f => f.EmployeeCount > 100);
        DisplayAll(Above100EmployeeCountFirms, "Above 100 employee count firms: ");

        var FirmsWithAverageEmployeeCount = firms.Where(f => f.EmployeeCount >= 100 ||
                                                             f.EmployeeCount <= 300);
        DisplayAll(FirmsWithAverageEmployeeCount, "Above 100 and lower than 300 employee count firms: ");

        var firmsLocatedInLondon = firms.Where(f => f.Address.ToLower().Contains("london"));
        DisplayAll(firmsLocatedInLondon, "Loacted in London: ");

        var directorSurnameContainsWhite = firms.Where(f => f.DirectorFullName.Contains("White"));
        DisplayAll(directorSurnameContainsWhite, "Director surname contains \"White\": ");

        var foundedTwoMoreYearsAgo = firms.Where(f => DateTime.Now.Year - f.FoundingDate.Year > 2);
        DisplayAll(foundedTwoMoreYearsAgo, "Founded two more years ago: ");

        var founded123DaysMoreAgo = firms.Where(f => (DateTime.Now - f.FoundingDate).TotalDays > 123);
        DisplayAll(founded123DaysMoreAgo, "Founded 123 days more ago: ");

        var IDKHowToNameIt = firms.Where(f => f.Name.ToLower().Contains("white") && f.DirectorFullName.Contains("Black"));
        DisplayAll(IDKHowToNameIt, "Director name contains \"Black\" and firm name containts \"white\": ");
    }

    private static void TestMyIEnumrableExtensions(IEnumerable<Firm> firms)
    {
        var businessProfiles = firms.MySelect(f => f.BusinessProfile);
        DisplayAll(businessProfiles, "Firms profiles: ");

        var firmsWhichContainFood = firms.MyWhere(f => f.Name.ToLower().Contains("food"));
        DisplayAll(firmsWhichContainFood, "Containing food firms: ");

        var marketingFirms = firms.MyWhere(f => f.BusinessProfile == "Marketing");
        DisplayAll(marketingFirms, "Marketing firms: ");

        var marketingOrITFirms = firms.MyWhere(f => f.BusinessProfile == "Marketing" ||
                                                  f.BusinessProfile == "IT");
        DisplayAll(marketingOrITFirms, "Marketing or IT firms: ");

        var Above100EmployeeCountFirms = firms.MyWhere(f => f.EmployeeCount > 100);
        DisplayAll(Above100EmployeeCountFirms, "Above 100 employee count firms: ");

        var FirmsWithAverageEmployeeCount = firms.MyWhere(f => f.EmployeeCount >= 100 ||
                                                             f.EmployeeCount <= 300);
        DisplayAll(FirmsWithAverageEmployeeCount, "Above 100 and lower than 300 employee count firms: ");

        var firmsLocatedInLondon = firms.MyWhere(f => f.Address.ToLower().Contains("london"));
        DisplayAll(firmsLocatedInLondon, "Loacted in London: ");

        var directorSurnameContainsWhite = firms.MyWhere(f => f.DirectorFullName.Contains("White"));
        DisplayAll(directorSurnameContainsWhite, "Director surname contains \"White\": ");

        var foundedTwoMoreYearsAgo = firms.MyWhere(f => DateTime.Now.Year - f.FoundingDate.Year > 2);
        DisplayAll(foundedTwoMoreYearsAgo, "Founded two more years ago: ");

        var founded123DaysMoreAgo = firms.MyWhere(f => (DateTime.Now - f.FoundingDate).TotalDays > 123);
        DisplayAll(founded123DaysMoreAgo, "Founded 123 days more ago: ");

        var IDKHowToNameIt = firms.MyWhere(f => f.Name.ToLower().Contains("white") && f.DirectorFullName.Contains("Black"));
        DisplayAll(IDKHowToNameIt, "Director name contains \"Black\" and firm name containts \"white\": ");
    }

    private static void TestLinqWithEmployees(IEnumerable<Firm> firms)
    {
        DisplayAll(firms.First(f => f.Name == "Some corp").EmployeeInfos, "Employees infos of some corp: ");

        DisplayAll(firms
            .First(f => f.Name == "Some corp").EmployeeInfos
            .Where(e => e.Salary > new Money(40000, 0)), 
            "Employees infos of some corp with salary above 40000hrn: ");

        DisplayAll(firms.SelectMany(f => f.EmployeeInfos)
            .Where(e => e.Position == EmployeePosition.Manager), 
            "All managers: ");

        DisplayAll(firms.SelectMany(f => f.EmployeeInfos)
            .Where(e => e.PhoneNumber.StartsWith("23")),
            "All employees whos phone number starts with 23: ");

        DisplayAll(firms.SelectMany(f => f.EmployeeInfos)
            .Where(e => e.Email.StartsWith("di")),
            "All employees whos email stars with di: ");

        DisplayAll(firms.SelectMany(f => f.EmployeeInfos)
            .Where(e => e.Name.FirstName.Contains("Lionel")),
            "All employees whos Lionel: ");
    }

    private static void DisplayAll<T>(IEnumerable<T> elements, string description)
    {
        Console.WriteLine(description);
        
        if (elements != null) 
            foreach (var element in elements) 
                Console.WriteLine(element);
        else
            Console.WriteLine("Elements not found...");
    }

}
