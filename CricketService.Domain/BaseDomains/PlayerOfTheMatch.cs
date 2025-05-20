namespace CricketService.Domain.BaseDomains
{
    public class PlayerOfTheMatch
    {
        public PlayerOfTheMatch(string playerName, string href)
        {
            PlayerName = playerName;
            Href = href;
        }

        public string PlayerName { get; set; }

        public string Href { get; set; }
    }
}
