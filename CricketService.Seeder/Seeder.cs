using CricketService.Data.Repositories.Interfaces;
using CricketService.Data.Utils;
using CricketService.Domain.Enums;
using CricketService.Domain.RequestDomains;
using CricketService.Seeder.Options;
using Newtonsoft.Json;

namespace CricketService.Seeder
{
    public class Seeder
    {
        private readonly ILogger<Seeder> logger;
        private readonly ICricketMatchRepository cricketMatchRepository;
        private readonly ICricketTeamRepository cricketTeamRepository;
        private readonly ICricketPlayerRepository cricketPlayerRepository;

        public Seeder(
            ICricketMatchRepository cricketMatchRepository,
            ICricketTeamRepository cricketTeamRepository,
            ICricketPlayerRepository cricketPlayerRepository,
            ILogger<Seeder> logger)
        {
            this.logger = logger;
            this.cricketMatchRepository = cricketMatchRepository;
            this.cricketTeamRepository = cricketTeamRepository;
            this.cricketPlayerRepository = cricketPlayerRepository;
        }

        public async Task RunSeeder(IConfiguration configs)
        {
            List<InternationalCricketMatchRequest> matchesData = new List<InternationalCricketMatchRequest>();
            var jsonFilePathsOptions = configs.GetSection(StaticDataJsonFilePathsOptions.SectionName).Get<StaticDataJsonFilePathsOptions>();
            var seedDataFeatures = configs.GetSection(SeedDataFeatureOptions.SectionName).Get<SeedDataFeatureOptions>();

            if (seedDataFeatures!.T20IMatches && jsonFilePathsOptions!.T20IMatchesData is not null)
            {
                StreamReader r = new StreamReader(jsonFilePathsOptions!.T20IMatchesData);

                matchesData = JsonConvert.DeserializeObject<List<InternationalCricketMatchRequest>>(r.ReadToEnd())!
                    .OrderBy(m => Convert.ToInt32(m.MatchNumber.Replace("T20I no. ", string.Empty))).ToList();

                if (seedDataFeatures.WritePdfs)
                {
                    await cricketMatchRepository.GeneratedPDFForMatches(matchesData, CricketFormat.T20I);
                }

                if (matchesData.Count > 0 && seedDataFeatures.WriteDB)
                {
                    int counter = 0;

                    foreach (var match in matchesData)
                    {
                        var matchResult = await cricketMatchRepository.AddLimitedOverInternationalMatch(match, CricketFormat.T20I);
                        counter++;

                        if (seedDataFeatures.WriteFiles)
                        {
                            FileHandler.WriteObjectToJsonFile($"D:/CricketData/JsonFiles/T20IMatches/{match.MatchNumber}_{match.MatchTitle}", matchResult);
                        }

                        if (counter % 5 == 0)
                        {
                            logger.LogInformation($"{counter} T20I matches seeded.");
                        }
                    }
                }
            }

            if (seedDataFeatures!.ODIMatches && jsonFilePathsOptions!.ODIMatchesData is not null)
            {
                StreamReader r = new StreamReader(jsonFilePathsOptions!.ODIMatchesData);

                matchesData = JsonConvert.DeserializeObject<List<InternationalCricketMatchRequest>>(r.ReadToEnd())!;

                if (seedDataFeatures.WritePdfs)
                {
                    await cricketMatchRepository.GeneratedPDFForMatches(matchesData, CricketFormat.ODI);
                }

                if (matchesData.Count > 0 && seedDataFeatures.WriteDB)
                {
                    int counter = 0;

                    foreach (var match in matchesData)
                    {
                        await cricketMatchRepository.AddLimitedOverInternationalMatch(match, CricketFormat.ODI);
                        counter++;

                        if (counter % 5 == 0)
                        {
                            logger.LogInformation($"{counter} ODI matches seeded.");
                        }
                    }
                }
            }

            if (seedDataFeatures!.TestMatches && jsonFilePathsOptions!.TestMatchesData is not null)
            {
                StreamReader r = new StreamReader(jsonFilePathsOptions!.TestMatchesData);

                var testMatchesData = JsonConvert.DeserializeObject<List<TestCricketMatchRequest>>(r.ReadToEnd())!;

                if (seedDataFeatures.WritePdfs)
                {
                    await cricketMatchRepository.GeneratedPDFForMatches(testMatchesData, CricketFormat.TestCricket);
                }

                if (testMatchesData.Count > 0 && seedDataFeatures.WriteDB)
                {
                    int counter = 0;

                    foreach (var match in testMatchesData)
                    {
                        await cricketMatchRepository.AddMatchTest(match);
                        counter++;

                        if (counter % 5 == 0)
                        {
                            logger.LogInformation($"{counter} Test matches seeded.");
                        }
                    }
                }
            }

            if (seedDataFeatures!.T20DMatches && jsonFilePathsOptions!.IPLMatchesData is not null)
            {
                StreamReader r = new StreamReader(jsonFilePathsOptions!.IPLMatchesData);

                var iplMatchesData = JsonConvert.DeserializeObject<List<DomesticCricketMatchRequest>>(r.ReadToEnd())!;

                if (seedDataFeatures.WritePdfs)
                {
                    await cricketMatchRepository.GeneratedPDFForMatches(iplMatchesData, CricketFormat.Twenty20);
                }

                if (iplMatchesData.Count > 0 && seedDataFeatures.WriteDB)
                {
                    int counter = 0;

                    foreach (var match in iplMatchesData)
                    {
                        await cricketMatchRepository.AddT20Match(match);
                        counter++;

                        if (counter % 5 == 0)
                        {
                            logger.LogInformation($"{counter} T20D matches seeded.");
                        }
                    }
                }
            }

            if (seedDataFeatures.CricketTeams && seedDataFeatures.WritePdfs)
            {
                await cricketTeamRepository.GeneratedPDFForTeams();
            }

            if (seedDataFeatures.CricketPlayers && seedDataFeatures.WritePdfs)
            {
                await cricketPlayerRepository.GeneratedPDFForPlayers();
            }
        }
    }
}
