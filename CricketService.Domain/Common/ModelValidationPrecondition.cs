namespace CricketService.Domain.Common;

public static class ModelValidationPrecondition
{
    public static void IsTrue(bool condition, string failureMessage, string source)
    {
        IsTrue(condition, new CricketModelValidationException(source, failureMessage));
    }

    public static void IsTrue(bool condition, CricketModelValidationException exception)
    {
        if (!condition)
        {
            throw exception;
        }
    }

    public static T IsNotNull<T>(T value, string paramName, string? source = "")
        where T : class
    {
        return value ?? throw new CricketModelValidationException(source!, paramName);
    }

    public static string IsNotNullOrWhitespace(string value, string paramName, string source)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new CricketModelValidationException(source, $"{paramName} was null or contained whitespace");
        }

        return value;
    }

    public static Guid IsNotEmpty(Guid value, string paramName, string source)
    {
        if (value == Guid.Empty)
        {
            throw new CricketModelValidationException(source, $"{paramName} was empty");
        }

        return value;
    }

    public static T IsDefined<T>(T value, string paramName, string source)
        where T : Enum
    {
        if (!Enum.IsDefined(typeof(T), value!))
        {
            throw new CricketModelValidationException(source, $"{paramName} was empty");
        }

        return value;
    }

    public static T IsValidFlag<T>(T value, string paramName, string source)
        where T : Enum
    {
        var allEnums = Enum.GetValues(typeof(T));
        var highestFlag = allEnums.GetValue(allEnums.Length - 1);
        var combinedFlags = Convert.ToInt32(highestFlag) | (Convert.ToInt32(highestFlag) - 1);

        if (Convert.ToInt32(value) > combinedFlags)
        {
            throw new CricketModelValidationException(source, $"{paramName} was not a valid combination of flags");
        }

        return value;
    }

    public static T[] IsNotEmpty<T>(T[] values, string paramName, string source)
    {
        return values.Length > 0 ?
                values :
                throw new CricketModelValidationException(source, paramName);
    }

    public static ICollection<T> IsNotEmpty<T>(ICollection<T> values, string paramName, string source)
    {
        return values.Count > 0 ?
                values :
                throw new CricketModelValidationException(source, $"{paramName} cannot be empty");
    }
}
