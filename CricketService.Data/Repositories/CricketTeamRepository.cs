using CricketService.Data.Contexts;
using CricketService.Data.Extensions;
using CricketService.Data.Repositories.Extensions;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain;
using CricketService.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CricketService.Data.Repositories
{
    public class CricketTeamRepository : ICricketTeamRepository
    {
        private ILogger<CricketPlayerRepository> logger;
        private CricketServiceContext context;

        public CricketTeamRepository(ILogger<CricketPlayerRepository> logger, CricketServiceContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public IEnumerable<string> GetAllTeamNamesT20I()
        {
            var query = (from match in context.T20ICricketMatchInfo
                         select match.Team1.TeamName)
            .Union(from match in context.T20ICricketMatchInfo
                   select match.Team2.TeamName)
            .Distinct()
            .OrderBy(x => x);

            return query.ToArray();
        }

        public IEnumerable<string> GetAllTeamNamesODI()
        {
            var query = (from match in context.ODICricketMatchInfo
                         select match.Team1.TeamName)
            .Union(from match in context.ODICricketMatchInfo
                   select match.Team2.TeamName)
            .Distinct()
            .OrderBy(x => x);

            return query.ToArray();
        }

        public CricketTeamInfo GetTeamRecordsByName(string teamName, bool isSingle = false)
        {
            var cricketMatchInfoTableODI = context.ODICricketMatchInfo;
            var cricketMatchInfoTableT20I = context.T20ICricketMatchInfo;

            var countriesData = GetDataFromFile(teamName);

            var allMatchesByTeamODI = cricketMatchInfoTableODI.Where(x => x.Team1.TeamName == teamName || x.Team2.TeamName == teamName);
            var allMatchesByTeamT20I = cricketMatchInfoTableT20I.Where(x => x.Team1.TeamName == teamName || x.Team2.TeamName == teamName);

            CricketTeamInfo teamRecordData = new CricketTeamInfo(teamName, new TeamRecordDetails(null!, null!, null!), countriesData.Flags.Svg);

            if (allMatchesByTeamT20I.Any())
            {
                DebutDetails debutDetails = null!;
                TeamMileStones teamMileStones = null!;

                if (isSingle)
                {
                    var debutMatchDetails = allMatchesByTeamT20I
                                    .OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("T20I no. ", string.Empty))).First();

                    var debutOpponent = debutMatchDetails.Team1.TeamName == teamName ? debutMatchDetails.Team2.TeamName : debutMatchDetails.Team1.TeamName;

                    var cricketMatchInfoResponses = allMatchesByTeamT20I.Select(x => x.ToDomain());

                    teamMileStones = cricketMatchInfoResponses.GetMileStones(teamName);

                    debutDetails = new DebutDetails(
                                                   debutMatchDetails.Uuid,
                                                   Convert.ToDateTime(debutMatchDetails.MatchDate),
                                                   debutOpponent,
                                                   debutMatchDetails.Venue,
                                                   debutMatchDetails.Result,
                                                   debutMatchDetails.MatchNo);
                }

                teamRecordData.TeamRecordDetails.T20IResults = new TeamResultDetails(
                                           debut: debutDetails,
                                           matches: cricketMatchInfoTableT20I.Count(m => m.Team1.TeamName == teamName || m.Team2.TeamName == teamName),
                                           won: cricketMatchInfoTableT20I.Count(m => m.Result.Contains($"{teamName} won by")),
                                           lost: cricketMatchInfoTableT20I.Count(m => (m.Team1.TeamName == teamName && m.Result.Contains(m.Team2.TeamName + " won by"))
                                                 || (m.Team2.TeamName == teamName && m.Result.Contains(m.Team1.TeamName + " won by"))),
                                           tiedAndWon: cricketMatchInfoTableT20I.Count(m => m.Result.Contains($"Match tied ({teamName} won the one-over eliminator)")),
                                           tiedAndLost: cricketMatchInfoTableT20I.Count(m => (m.Team1.TeamName == teamName || m.Team2.TeamName == teamName)
                                                 && m.Result.Contains("Match tied")
                                                 && !m.Result.Contains($"Match tied ({teamName} won the one-over eliminator)")),
                                           noResult: cricketMatchInfoTableT20I.Count(m => (m.Team1.TeamName == teamName || m.Team2.TeamName == teamName)
                                                 && m.Result.Contains("No result")),
                                           teamMileStones: teamMileStones);
            }

            if (allMatchesByTeamODI.Any())
            {
                DebutDetails debutDetails = null!;
                TeamMileStones teamMileStones = null!;

                if (isSingle)
                {
                    var debutMatchDetails = allMatchesByTeamODI
                    .OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("ODI no. ", string.Empty))).First();

                    var debutOpponent = debutMatchDetails.Team1.TeamName == teamName ? debutMatchDetails.Team2.TeamName : debutMatchDetails.Team1.TeamName;

                    var cricketMatchInfoResponses = allMatchesByTeamODI.Select(x => x.ToDomain());

                    teamMileStones = cricketMatchInfoResponses.GetMileStones(teamName);

                    debutDetails = new DebutDetails(
                                                   debutMatchDetails.Uuid,
                                                   Convert.ToDateTime(debutMatchDetails.MatchDate),
                                                   debutOpponent,
                                                   debutMatchDetails.Venue,
                                                   debutMatchDetails.Result,
                                                   debutMatchDetails.MatchNo);
                }

                teamRecordData.TeamRecordDetails.ODIResults = new TeamResultDetails(
                                           debut: debutDetails,
                                           matches: cricketMatchInfoTableODI.Count(m => m.Team1.TeamName == teamName || m.Team2.TeamName == teamName),
                                           won: cricketMatchInfoTableODI.Count(m => m.Result.Contains($"{teamName} won by")),
                                           lost: cricketMatchInfoTableODI.Count(m => (m.Team1.TeamName == teamName && m.Result.Contains(m.Team2.TeamName + " won by"))
                                                 || (m.Team2.TeamName == teamName && m.Result.Contains(m.Team1.TeamName + " won by"))),
                                           tiedAndWon: cricketMatchInfoTableODI.Count(m => m.Result.Contains($"Match tied ({teamName} won the one-over eliminator)")),
                                           tiedAndLost: cricketMatchInfoTableODI.Count(m => (m.Team1.TeamName == teamName || m.Team2.TeamName == teamName)
                                                 && m.Result.Contains("Match tied")
                                                 && !m.Result.Contains($"Match tied ({teamName} won the one-over eliminator)")),
                                           noResult: cricketMatchInfoTableODI.Count(m => (m.Team1.TeamName == teamName || m.Team2.TeamName == teamName)
                                                 && m.Result.Contains("No result")),
                                           teamMileStones: teamMileStones);
            }

            return teamRecordData;
        }

        public object GetAllAgainstRecordsByNameT20I(string teamName)
        {
            var cricketMatchInfoTableT20I = context.T20ICricketMatchInfo;

            var allMatchesByTeamT20I = cricketMatchInfoTableT20I.Where(x => x.Team1.TeamName == teamName || x.Team2.TeamName == teamName);

            var recordAgainstTeams = allMatchesByTeamT20I
                 .GroupBy(m => m.Team1.TeamName == teamName ? m.Team2.TeamName : m.Team1.TeamName);

            return recordAgainstTeams.Select(g => new
            {
                Opponent = g.Key,
                Matches = g.Count(),
                Won = g.Count(x => x.Result.Contains($"{teamName} won by")),
                Lost = g.Count(m => (m.Team1.TeamName == teamName && m.Result.Contains(m.Team2.TeamName + " won by"))
                                                 || (m.Team2.TeamName == teamName && m.Result.Contains(m.Team1.TeamName + " won by"))),
                NoResult = g.Count(x => x.Result.Contains("No result")),
            });
        }

        public object GetAllAgainstRecordsByNameODI(string teamName)
        {
            var cricketMatchInfoTableODI = context.ODICricketMatchInfo;

            var allMatchesByTeamODI = cricketMatchInfoTableODI.Where(x => x.Team1.TeamName == teamName || x.Team2.TeamName == teamName);

            var recordAgainstTeams = allMatchesByTeamODI
                 .GroupBy(m => m.Team1.TeamName == teamName ? m.Team2.TeamName : m.Team1.TeamName);

            return recordAgainstTeams.Select(g => new
            {
                Opponent = g.Key,
                Matches = g.Count(),
                Won = g.Count(x => x.Result.Contains($"{teamName} won by")),
                Lost = g.Count(m => (m.Team1.TeamName == teamName && m.Result.Contains(m.Team2.TeamName + " won by"))
                                                 || (m.Team2.TeamName == teamName && m.Result.Contains(m.Team1.TeamName + " won by"))),
                NoResult = g.Count(x => x.Result.Contains("No result")),
            });
        }

        private CountriesData GetDataFromFile(string teamName)
        {
            List<CountriesData> countriesData = new List<CountriesData>();

            StreamReader r = new StreamReader(@"D:\MyYoutubeRepos\CricketService\CricketService\CricketService.Data\StaticData\CountriesData.json");
            countriesData = JsonConvert.DeserializeObject<List<CountriesData>>(r.ReadToEnd())!;

            return countriesData.FirstOrDefault(x => x.Name!.Common == teamName, new CountriesData(new Name(teamName), new Flags("url_not_found", "url_not_found")));
        }
    }
}