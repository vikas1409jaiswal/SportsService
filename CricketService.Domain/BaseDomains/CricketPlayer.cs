namespace CricketService.Domain.BaseDomains
{
    public class CricketPlayer
    {
        public CricketPlayer(string name, string href)
        {
            Name = name;
            Href = href;
        }

        public string Name { get; set; }

        public string Href { get; set; }
    }
}
