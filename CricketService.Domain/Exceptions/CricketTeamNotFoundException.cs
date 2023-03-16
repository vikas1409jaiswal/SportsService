namespace CricketService.Domain.Exceptions
{
    public class CricketTeamNotFoundException : Exception
    {
        public CricketTeamNotFoundException()
        {
        }

        public CricketTeamNotFoundException(string message)
            : base(message)
        {
        }

        public CricketTeamNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
