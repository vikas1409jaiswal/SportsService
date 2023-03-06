using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain;
using CricketService.Seeder.Options;
using Newtonsoft.Json;

namespace CricketService.Seeder
{
    public class Seeder
    {
        private readonly ILogger<Seeder> logger;
        private readonly ICricketMatchRepository repository;

        public Seeder(ICricketMatchRepository repository, ILogger<Seeder> logger)
        {
             this.logger = logger;
             this.repository = repository;
        }

        public async Task RunSeeder(IConfiguration configs)
        {
            List<CricketMatchInfoRequest> matchesData = new List<CricketMatchInfoRequest>();
            var jsonFilePathsOptions = configs.GetSection(StaticDataJsonFilePathsOptions.SectionName).Get<StaticDataJsonFilePathsOptions>();
            var seedDataFeatures = configs.GetSection(SeedDataFeatureOptions.SectionName).Get<SeedDataFeatureOptions>();

            if (seedDataFeatures!.T20IMatches && jsonFilePathsOptions!.T20IMatchesData is not null)
            {
                StreamReader r = new StreamReader(jsonFilePathsOptions!.T20IMatchesData);

                matchesData = JsonConvert.DeserializeObject<List<CricketMatchInfoRequest>>(r.ReadToEnd())!;

                if (matchesData.Count > 0)
                {
                    int counter = 0;

                    foreach (var match in matchesData)
                    {
                        await repository.AddMatchT20I(match);
                        counter++;

                        if (counter % 100 == 0)
                        {
                            logger.LogInformation($"{counter} T20I matches seeded.");
                        }
                    }
                }
            }

            if (seedDataFeatures!.ODIMatches && jsonFilePathsOptions!.ODIMatchesData is not null)
            {
                StreamReader r = new StreamReader(jsonFilePathsOptions!.ODIMatchesData);

                matchesData = JsonConvert.DeserializeObject<List<CricketMatchInfoRequest>>(r.ReadToEnd())!;

                if (matchesData.Count > 0)
                {
                    int counter = 0;

                    foreach (var match in matchesData)
                    {
                        await repository.AddMatchODI(match);
                        counter++;

                        if (counter % 100 == 0)
                        {
                            logger.LogInformation($"{counter} ODI matches seeded.");
                        }
                    }
                }
            }
        }
    }
}
