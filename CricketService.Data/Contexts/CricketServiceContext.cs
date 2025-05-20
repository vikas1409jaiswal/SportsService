using CricketService.Data.Entities;
using CricketService.Domain;
using CricketService.Domain.BaseDomains;
using CricketService.Domain.Common;
using CricketService.Domain.ResponseDomains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace CricketService.Data.Contexts;

public class CricketServiceContext : DbContext
{
    public CricketServiceContext(DbContextOptions<CricketServiceContext> options)
        : base(options)
    {
    }

    public DbSet<TestCricketMatchInfoDTO> TestCricketMatchInfo { get; set; }

    public DbSet<LimitedOverInternationalMatchInfoDTO> LimitedOverInternationalMatchesInfo { get; set; }

    public DbSet<T20MatchInfoDTO> T20MatchesInfo { get; set; }

    public DbSet<CricketTeamInfoDTO> CricketTeamInfo { get; set; }

    public DbSet<CricketPlayerInfoDTO> CricketPlayerInfo { get; set; }

    public DbSet<CricketTeamPlayerInfos> CricketTeamPlayerInfos { get; set; }

    public DbSet<CricketTeamHistoryDTO> CricketTeamsHistory { get; set; }

    public DbSet<CricketTeamHistoryH2hDTO> CricketTeamsHistoryH2H { get; set; }

    public Task<IEnumerable<string>> GetPendingMigrationsAsync()
    {
        return Database.GetPendingMigrationsAsync();
    }

    public Task MigrateAsync()
    {
        return Database.MigrateAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        OMCTestCricketMatches(modelBuilder);

        OMCLimitedOverInternationalMatches(modelBuilder);

        OMCT20Matches(modelBuilder);

        OMCCricketTeams(modelBuilder);

        OMCCricketPlayers(modelBuilder);

        OMCCricketTeamsPlayers(modelBuilder);

        OMCCricketTeamsHistory(modelBuilder);

        OMCCricketTeamsHistoryH2H(modelBuilder);
    }

    private static void OMCCricketTeamsHistoryH2H(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CricketTeamHistoryH2hDTO>()
            .HasKey(cth => cth.MatchUuid);

        modelBuilder.Entity<CricketTeamHistoryH2hDTO>()
            .Property(cth => cth.InstantTeamsRecords)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<TeamFormatRecords>>(v)!)
            .HasColumnName("instant_teams_records")
            .HasColumnType("jsonb");

        modelBuilder.Entity<CricketTeamHistoryH2hDTO>()
            .Property(cth => cth.Format)
            .IsRequired()
            .HasMaxLength(50);
    }

    private static void OMCCricketTeamsHistory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CricketTeamHistoryDTO>()
            .HasKey(cth => cth.MatchUuid);

        modelBuilder.Entity<CricketTeamHistoryDTO>()
            .Property(cth => cth.InstantTeamsRecords)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<TeamFormatRecords>>(v)!)
            .HasColumnName("instant_teams_records")
            .HasColumnType("jsonb");

        modelBuilder.Entity<CricketTeamHistoryDTO>()
            .Property(cth => cth.Format)
            .IsRequired()
            .HasMaxLength(50);
    }

    private static void OMCCricketTeamsPlayers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CricketTeamPlayerInfos>()
            .HasKey(x => new { x.TeamUuid, x.PlayerUuid });

        modelBuilder.Entity<CricketTeamPlayerInfos>()
            .HasOne<CricketPlayerInfoDTO>(p => p.PlayerInfo)
            .WithMany(p => p.TeamsPlayersInfos)
            .HasForeignKey(x => x.PlayerUuid);

        modelBuilder.Entity<CricketTeamPlayerInfos>()
            .HasOne<CricketTeamInfoDTO>(p => p.TeamInfo)
            .WithMany(p => p.TeamsPlayersInfos)
            .HasForeignKey(x => x.TeamUuid);

        modelBuilder.Entity<CricketTeamPlayerInfos>()
          .Property(e => e.CareerStatistics)
          .HasConversion(
          dd => JsonConvert.SerializeObject(dd),
          dd => JsonConvert.DeserializeObject<CareerDetailsInfo>(dd)!)
          .HasColumnName("career_statistics")
          .HasColumnType("jsonb");
    }

    private static void OMCCricketPlayers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CricketPlayerInfoDTO>()
                  .HasKey(e => e.Uuid);

        modelBuilder.Entity<CricketPlayerInfoDTO>()
          .Property(p => p.InternationalTeamNames)
          .HasConversion(
              v => string.Join(",", v),
              v => v.Split(",", StringSplitOptions.None),
              new ValueComparer<ICollection<string>>(
                 (c1, c2) => c1!.SequenceEqual(c2!),
                 c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                 c => c.ToList()));

        modelBuilder.Entity<CricketPlayerInfoDTO>()
         .Property(p => p.Formats)
         .HasConversion(
             v => string.Join(",", v),
             v => v.Split(",", StringSplitOptions.None),
             new ValueComparer<ICollection<string>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));

        modelBuilder.Entity<CricketPlayerInfoDTO>()
          .Property(e => e.DateOfBirth)
          .HasConversion(
          db => JsonConvert.SerializeObject(db),
          db => JsonConvert.DeserializeObject<DateOfEvent>(db))
          .HasColumnName("date_of_birth")
          .HasColumnType("jsonb");

        modelBuilder.Entity<CricketPlayerInfoDTO>()
         .Property(e => e.DateOfDeath)
         .HasConversion(
         db => JsonConvert.SerializeObject(db),
         db => JsonConvert.DeserializeObject<DateOfEvent>(db))
         .HasColumnName("date_of_death")
         .HasColumnType("jsonb");

        modelBuilder.Entity<CricketPlayerInfoDTO>()
           .Property(e => e.DebutDetails)
           .HasConversion(
           dd => JsonConvert.SerializeObject(dd),
           dd => JsonConvert.DeserializeObject<DebutDetailsInfo>(dd)!)
           .HasColumnName("debut_details")
           .HasColumnType("jsonb");

        modelBuilder.Entity<CricketPlayerInfoDTO>()
          .Property(e => e.ExtraInfo)
          .HasConversion(
          dd => JsonConvert.SerializeObject(dd),
          dd => JsonConvert.DeserializeObject<PlayerExtraInfo>(dd)!)
          .HasColumnName("extra_info")
          .HasColumnType("jsonb");

        modelBuilder.Entity<CricketPlayerInfoDTO>()
            .Property(cp => cp.Contents)
            .IsRequired(false);
    }

    private static void OMCCricketTeams(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CricketTeamInfoDTO>()
                 .HasKey(e => e.Uuid);

        modelBuilder.Entity<CricketTeamInfoDTO>()
         .Property(p => p.Formats)
         .HasConversion(
             v => string.Join(",", v),
             v => v.Split(",", StringSplitOptions.None),
             new ValueComparer<ICollection<string>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));

        modelBuilder.Entity<CricketTeamInfoDTO>()
         .Property(e => e.T20IRecords)
         .HasConversion(
         dd => JsonConvert.SerializeObject(dd),
         dd => JsonConvert.DeserializeObject<TeamFormatRecordDetails>(dd)!)
         .HasColumnName("t20i_records")
         .HasColumnType("jsonb");

        modelBuilder.Entity<CricketTeamInfoDTO>()
        .Property(e => e.ODIRecords)
        .HasConversion(
        dd => JsonConvert.SerializeObject(dd),
        dd => JsonConvert.DeserializeObject<TeamFormatRecordDetails>(dd)!)
        .HasColumnName("odi_records")
        .HasColumnType("jsonb");

        modelBuilder.Entity<CricketTeamInfoDTO>()
        .Property(e => e.TestRecords)
        .HasConversion(
        dd => JsonConvert.SerializeObject(dd),
        dd => JsonConvert.DeserializeObject<TeamFormatRecordDetails>(dd)!)
        .HasColumnName("test_records")
        .HasColumnType("jsonb");
    }

    private static void OMCT20Matches(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<T20MatchInfoDTO>()
            .HasKey(e => e.Uuid);

        modelBuilder.Entity<T20MatchInfoDTO>()
            .Property(e => e.Team1)
            .IsRequired()
            .HasConversion(
            t1 => JsonConvert.SerializeObject(t1),
            t1 => JsonConvert.DeserializeObject<SingleInningTeamScoreboardResponse>(t1)!)
            .HasColumnName("team1_details")
            .HasColumnType("jsonb");

        modelBuilder.Entity<T20MatchInfoDTO>()
            .Property(e => e.Team2)
            .IsRequired()
            .HasConversion(
            t1 => JsonConvert.SerializeObject(t1),
            t1 => JsonConvert.DeserializeObject<SingleInningTeamScoreboardResponse>(t1)!)
            .HasColumnName("team2_details")
            .HasColumnType("jsonb");

        modelBuilder.Entity<T20MatchInfoDTO>()
           .Property(e => e.PlayerOfTheMatch)
           .HasConversion(
           potm => JsonConvert.SerializeObject(potm),
           potm => JsonConvert.DeserializeObject<PlayerOfTheMatch>(potm)!)
           .HasColumnName("player_of_the_match")
           .HasColumnType("jsonb");

        modelBuilder.Entity<T20MatchInfoDTO>()
           .Property(e => e.FormatDebut)
           .HasConversion(
              debutList => JsonConvert.SerializeObject(debutList),
              debutList => JsonConvert.DeserializeObject<List<CricketPlayer>>(debutList) ?? new List<CricketPlayer>())
           .HasColumnName("format_debut")
           .HasColumnType("jsonb");
    }

    private static void OMCLimitedOverInternationalMatches(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LimitedOverInternationalMatchInfoDTO>()
            .HasKey(e => e.Uuid);

        modelBuilder.Entity<LimitedOverInternationalMatchInfoDTO>()
            .Property(e => e.Team1)
            .IsRequired()
            .HasConversion(
            t1 => JsonConvert.SerializeObject(t1),
            t1 => JsonConvert.DeserializeObject<SingleInningTeamScoreboardResponse>(t1)!)
            .HasColumnName("team1_details")
            .HasColumnType("jsonb");

        modelBuilder.Entity<LimitedOverInternationalMatchInfoDTO>()
            .Property(e => e.Team2)
            .IsRequired()
            .HasConversion(
            t1 => JsonConvert.SerializeObject(t1),
            t1 => JsonConvert.DeserializeObject<SingleInningTeamScoreboardResponse>(t1)!)
            .HasColumnName("team2_details")
            .HasColumnType("jsonb");

        modelBuilder.Entity<LimitedOverInternationalMatchInfoDTO>()
           .Property(e => e.PlayerOfTheMatch)
           .HasConversion(
           potm => JsonConvert.SerializeObject(potm),
           potm => JsonConvert.DeserializeObject<PlayerOfTheMatch>(potm)!)
           .HasColumnName("player_of_the_match")
           .HasColumnType("jsonb");

        modelBuilder.Entity<LimitedOverInternationalMatchInfoDTO>()
           .Property(e => e.InternationalDebut)
           .HasConversion(
              debutList => JsonConvert.SerializeObject(debutList),
              debutList => JsonConvert.DeserializeObject<List<CricketPlayer>>(debutList) ?? new List<CricketPlayer>())
           .HasColumnName("international_debut")
           .HasColumnType("jsonb");
    }

    private static void OMCTestCricketMatches(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestCricketMatchInfoDTO>()
            .HasKey(e => e.Uuid);

        modelBuilder.Entity<TestCricketMatchInfoDTO>()
            .Property(e => e.Team1)
            .IsRequired()
            .HasConversion(
            t1 => JsonConvert.SerializeObject(t1),
            t1 => JsonConvert.DeserializeObject<DoubleInningTeamScoreboardResponse>(t1)!)
            .HasColumnName("team1_details")
            .HasColumnType("jsonb");

        modelBuilder.Entity<TestCricketMatchInfoDTO>()
            .Property(e => e.Team2)
            .IsRequired()
            .HasConversion(
            t1 => JsonConvert.SerializeObject(t1),
            t1 => JsonConvert.DeserializeObject<DoubleInningTeamScoreboardResponse>(t1)!)
            .HasColumnName("team2_details")
            .HasColumnType("jsonb");

        modelBuilder.Entity<TestCricketMatchInfoDTO>()
            .Property(e => e.PlayerOfTheMatch)
            .HasConversion(
            potm => JsonConvert.SerializeObject(potm),
            potm => JsonConvert.DeserializeObject<PlayerOfTheMatch>(potm)!)
            .HasColumnName("player_of_the_match")
            .HasColumnType("jsonb");

        modelBuilder.Entity<TestCricketMatchInfoDTO>()
           .Property(e => e.InternationalDebut)
           .HasConversion(
              debutList => JsonConvert.SerializeObject(debutList),
              debutList => JsonConvert.DeserializeObject<List<CricketPlayer>>(debutList) ?? new List<CricketPlayer>())
           .HasColumnName("international_debut")
           .HasColumnType("jsonb");
    }
}