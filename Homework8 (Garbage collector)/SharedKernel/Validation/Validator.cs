namespace SharedKernel.Validation;

public static class Validator
{
    public static T GetValidatedValue<T>(T value, Predicate<T> isInvalid, Exception exception)
    {
        if (isInvalid(value))
            throw exception;

        return value;
    }
}
