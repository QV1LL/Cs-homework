using SharedKernel.Validation;

namespace SharedKernel.ValueObjects;

public record Address
{
    public string Street
    {
        get => field;
        set => field = Validator.GetValidatedValue(value, street => string.IsNullOrEmpty(street), new ArgumentNullException("Street field cannot be empty"));
    }

    public string City
    {
        get => field;
        set => field = Validator.GetValidatedValue(value, city => string.IsNullOrEmpty(city), new ArgumentNullException("City field cannot be empty"));
    }

    public string PostalCode
    {
        get => field;
        set => field = Validator.GetValidatedValue(value, 
            postalCode => string.IsNullOrEmpty(postalCode) || 
            postalCode.Length < 5 || 
            postalCode.Length > 10, 
            new ArgumentNullException("Wrong post code template"));
    }
    
    public Address(string street, string city, string postalCode)
    {
        Street = street;
        City = city;
        PostalCode = postalCode;
    }
}
