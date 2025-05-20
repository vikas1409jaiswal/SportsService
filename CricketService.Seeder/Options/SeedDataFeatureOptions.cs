namespace CricketService.Seeder.Options;

public class SeedDataFeatureOptions
{
    public const string SectionName = "SeedDataFeatureOptions";

    public bool ODIMatches { get; set; } = false;

    public bool T20IMatches { get; set; } = false;

    public bool TestMatches { get; set; } = false;

    public bool T20DMatches { get; set; } = false;

    public bool WriteFiles { get; set; } = false;

    public bool WritePdfs { get; set; } = false;

    public bool WriteDB { get; set; } = false;

    public bool CricketTeams { get; set; } = false;

    public bool CricketPlayers { get; set;} = false;
}