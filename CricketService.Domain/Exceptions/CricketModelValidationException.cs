public class CricketModelValidationException : Exception
{
    public CricketModelValidationException(string key)
    {
        ExceptionKey = key;
    }

    public CricketModelValidationException(string key, string message)
        : base(message)
    {
        ExceptionKey = key;
    }

    public CricketModelValidationException(string key, string message, Exception innerException)
        : base(message, innerException)
    {
        ExceptionKey = key;
    }

    public string ExceptionKey { get; set; }
}
