using CricketService.Domain.Enums;

namespace CricketService.Domain.Common
{
    public class PlayersFilters
    {
        public PlayersFilters(
            CricketFormat format,
            string? teamName,
            string? dateOfBirth,
            int? birthYear,
            bool? isExpired,
            string? playingRole,
            string? nameStartsWith)
        {
            Format = format;
            TeamName = teamName;
            DateOfBirth = dateOfBirth;
            BirthYear = birthYear;
            IsExpired = isExpired;
            PlayingRole = playingRole;
            NameStartsWith = nameStartsWith;
        }

        public CricketFormat Format { get; }

        public string? TeamName { get; } = null;

        public string? DateOfBirth { get; } = null;

        public int? BirthYear { get; } = null;

        public bool? IsExpired { get; } = null;

        public string? PlayingRole { get; } = null;

        public string? NameStartsWith { get; } = null;
    }
}
