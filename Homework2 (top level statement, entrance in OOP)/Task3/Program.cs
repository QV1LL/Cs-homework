using System.Globalization;
using Task3.Domain.Entities;
using Task3.Domain.ValueObjects;

namespace Task3
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            string dateString = "2023-06-28 12:12:15";
            string format = "yyyy-MM-dd HH:mm:ss";
            DateTime issueDate = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);

            var passport = new ForeignPassport(Guid.NewGuid(), new FullName("Bohdan", "Melenevych", "Myroslawovich"), issueDate);

            Console.WriteLine(passport);
        }
    }
}