using System.ComponentModel.DataAnnotations;

namespace App.CustomAttributes;

internal class YearAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is int year)
        {
            int currentYear = DateTime.Now.Year;
            if (year >= 0 && year <= currentYear)
                return ValidationResult.Success;

            return new ValidationResult($"Year must be between 0 and {currentYear}.");
        }
        return new ValidationResult("Invalid year format.");
    }
}
