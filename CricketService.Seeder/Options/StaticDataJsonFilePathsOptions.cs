namespace CricketService.Seeder.Options;

public class StaticDataJsonFilePathsOptions
{
    public const string SectionName = "StaticDataJsonFilesPath";

    public string[] ODIMatchesData { get; set; } = Array.Empty<string>();

    public string[] T20IMatchesData { get; set; } = Array.Empty<string>();

    public string[] TestMatchesData { get; set; } = Array.Empty<string>();

    public string[] IPLMatchesData { get; set; } = Array.Empty<string>();
}