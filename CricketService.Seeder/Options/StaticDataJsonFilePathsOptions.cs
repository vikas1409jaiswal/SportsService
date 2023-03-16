namespace CricketService.Seeder.Options;

public class StaticDataJsonFilePathsOptions
{
    public const string SectionName = "StaticDataJsonFilesPath";

    public string ODIMatchesData { get; set; } = null!;

    public string T20IMatchesData { get; set; } = null!;

    public string TestMatchesData { get; set; } = null!;
}