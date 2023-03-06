namespace CricketService.Seeder.Options;

public class SeedDataFeatureOptions
{
    public const string SectionName = "SeedDataFeatureOptions";

    public bool ODIMatches { get; set; } = false;

    public bool T20IMatches { get; set; } = false;
}