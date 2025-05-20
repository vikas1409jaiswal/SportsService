namespace CricketService.Domain.BaseDomains
{
    public class CricketTeam
    {
        public CricketTeam(
            Guid uuid,
            string name,
            string? logoUrl = null)
        {
            Uuid = uuid;
            Name = name;
            LogoUrl = logoUrl;
        }

        public Guid Uuid { get; set; }

        public string Name { get; set; }

        public string? LogoUrl { get; set; }
    }
}
