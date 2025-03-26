using LibraryMVC.Domain.ValueObjects;

namespace LibraryMVC.Infrastructure.Extensions;

public static class TypeConversionExtensions
{
    public static object ChangeTypeExtended(this object value, Type targetType)
    {
        if (value == null)
            return null;

        if (value.GetType() == targetType)
            return value;

        if (targetType == typeof(Guid))
        {
            if (value is string stringValue && Guid.TryParse(stringValue, out Guid guid))
                return guid;
            throw new InvalidCastException($"Cannot convert '{value}' to Guid.");
        }

        if (targetType == typeof(Name))
        {
            if (value is string stringValue)
            {
                var parts = stringValue.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2)
                    throw new InvalidCastException($"Cannot convert '{stringValue}' to Name: at least FirstName and LastName required.");

                string firstName = parts[1];
                string lastName = parts[0];
                string? middleName = parts.Length > 2 ? parts[2] : null;

                return new Name(firstName, lastName, middleName);
            }
            throw new InvalidCastException($"Cannot convert type '{value.GetType()}' to Name.");
        }

        return Convert.ChangeType(value, targetType);
    }
}
