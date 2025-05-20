namespace CricketService.Domain.Exceptions
{
    public class CricketPlayerNotFoundException : Exception
    {
        public CricketPlayerNotFoundException()
        {
        }

        public CricketPlayerNotFoundException(string message)
            : base(message)
        {
        }

        public CricketPlayerNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
