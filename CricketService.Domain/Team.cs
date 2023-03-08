namespace CricketService.Domain
{
    public class Team
    {
        public Team(
            Guid uuid,
            string teamName,
            ICollection<string> formats,
            string logoUrl,
            string flagUrl)
        {
            Uuid = uuid;
            TeamName = teamName;
            Formats = formats;
            LogoUrl = logoUrl;
            FlagUrl = flagUrl;
        }

        public Guid Uuid { get; set; }

        public string TeamName { get; set; }

        public ICollection<string> Formats { get; set; }

        public string LogoUrl { get; set; }

        public string FlagUrl { get; set; }
    }
}
