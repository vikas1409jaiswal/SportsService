namespace CricketService.Data.Options.Configs;

public class CricketServiceContextOptions
{
    public const string SectionName = "PostgresServer";

    public string ConnectionString { get; set; } = null!;
}