using CricketService.Domain.Attributes.ValidationAttributes;

namespace CricketService.Domain.BaseDomains
{
    public class CricketTeam
    {
        private string name = string.Empty;

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

        [NoConsecutiveCaps]
        [NoAbbreviationWithDot]
        public string Name
        {
            get => name;
            set => name = TeamNameConverter.Replace(value);
        }

        public string? LogoUrl { get; set; }
    }
}
