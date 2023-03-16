using CricketService.Data.Entities;
using CricketService.Domain;
using CricketService.Domain.Common;
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

    public DbSet<Entities.TestCricketMatchInfo> TestCricketMatchInfo { get; set; }

    public DbSet<Entities.T20ICricketMatchInfo> T20ICricketMatchInfo { get; set; }

    public DbSet<Entities.ODICricketMatchInfo> ODICricketMatchInfo { get; set; }

    public DbSet<Entities.CricketTeamInfo> CricketTeamInfo { get; set; }

    public DbSet<Entities.CricketPlayerInfo> CricketPlayerInfo { get; set; }

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

        modelBuilder.Entity<Entities.TestCricketMatchInfo>()
            .HasKey(e => e.Uuid);

        modelBuilder.Entity<Entities.TestCricketMatchInfo>()
            .Property(e => e.Team1)
            .IsRequired()
            .HasConversion(
            t1 => JsonConvert.SerializeObject(t1),
            t1 => JsonConvert.DeserializeObject<TestTeamScoreDetailsRequest>(t1)!)
            .HasColumnName("team1_details")
            .HasColumnType("jsonb");

        modelBuilder.Entity<Entities.TestCricketMatchInfo>()
            .Property(e => e.Team2)
            .IsRequired()
            .HasConversion(
            t1 => JsonConvert.SerializeObject(t1),
            t1 => JsonConvert.DeserializeObject<TestTeamScoreDetailsRequest>(t1)!)
            .HasColumnName("team2_details")
            .HasColumnType("jsonb");

        modelBuilder.Entity<Entities.T20ICricketMatchInfo>()
            .HasKey(e => e.Uuid);

        modelBuilder.Entity<Entities.T20ICricketMatchInfo>()
            .Property(e => e.Team1)
            .IsRequired()
            .HasConversion(
            t1 => JsonConvert.SerializeObject(t1),
            t1 => JsonConvert.DeserializeObject<TeamScoreDetailsRequest>(t1)!)
            .HasColumnName("team1_details")
            .HasColumnType("jsonb");

        modelBuilder.Entity<Entities.T20ICricketMatchInfo>()
            .Property(e => e.Team2)
            .IsRequired()
            .HasConversion(
            t1 => JsonConvert.SerializeObject(t1),
            t1 => JsonConvert.DeserializeObject<TeamScoreDetailsRequest>(t1)!)
            .HasColumnName("team2_details")
            .HasColumnType("jsonb");

        modelBuilder.Entity<Entities.ODICricketMatchInfo>()
           .HasKey(e => e.Uuid);

        modelBuilder.Entity<Entities.ODICricketMatchInfo>()
            .Property(e => e.Team1)
            .IsRequired()
            .HasConversion(
            t1 => JsonConvert.SerializeObject(t1),
            t1 => JsonConvert.DeserializeObject<TeamScoreDetailsRequest>(t1)!)
            .HasColumnName("team1_details")
            .HasColumnType("jsonb");

        modelBuilder.Entity<Entities.ODICricketMatchInfo>()
            .Property(e => e.Team2)
            .IsRequired()
            .HasConversion(
            t1 => JsonConvert.SerializeObject(t1),
            t1 => JsonConvert.DeserializeObject<TeamScoreDetailsRequest>(t1)!)
            .HasColumnName("team2_details")
            .HasColumnType("jsonb");

        modelBuilder.Entity<Entities.CricketTeamInfo>()
         .HasKey(e => e.Uuid);

        modelBuilder.Entity<CricketTeamInfo>()
         .Property(p => p.Formats)
         .HasConversion(
             v => string.Join(",", v),
             v => v.Split(",", StringSplitOptions.None),
             new ValueComparer<ICollection<string>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));

        modelBuilder.Entity<Entities.CricketTeamInfo>()
         .Property(e => e.T20IRecords)
         .HasConversion(
         dd => JsonConvert.SerializeObject(dd),
         dd => JsonConvert.DeserializeObject<TeamFormatRecordDetails>(dd)!)
         .HasColumnName("t20i_records")
         .HasColumnType("jsonb");

        modelBuilder.Entity<Entities.CricketTeamInfo>()
        .Property(e => e.ODIRecords)
        .HasConversion(
        dd => JsonConvert.SerializeObject(dd),
        dd => JsonConvert.DeserializeObject<TeamFormatRecordDetails>(dd)!)
        .HasColumnName("odi_records")
        .HasColumnType("jsonb");

        modelBuilder.Entity<Entities.CricketTeamInfo>()
        .Property(e => e.TestRecords)
        .HasConversion(
        dd => JsonConvert.SerializeObject(dd),
        dd => JsonConvert.DeserializeObject<TestTeamFormatRecordDetails>(dd)!)
        .HasColumnName("test_records")
        .HasColumnType("jsonb");

        modelBuilder.Entity<Entities.CricketPlayerInfo>()
          .HasKey(e => e.Uuid);

        modelBuilder.Entity<CricketPlayerInfo>()
          .Property(p => p.InternationalTeamNames)
          .HasConversion(
              v => string.Join(",", v),
              v => v.Split(",", StringSplitOptions.None),
              new ValueComparer<ICollection<string>>(
                 (c1, c2) => c1!.SequenceEqual(c2!),
                 c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                 c => c.ToList()));

        modelBuilder.Entity<CricketPlayerInfo>()
         .Property(p => p.Formats)
         .HasConversion(
             v => string.Join(",", v),
             v => v.Split(",", StringSplitOptions.None),
             new ValueComparer<ICollection<string>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));

        modelBuilder.Entity<Entities.CricketPlayerInfo>()
           .Property(e => e.DebutDetails)
           .HasConversion(
           dd => JsonConvert.SerializeObject(dd),
           dd => JsonConvert.DeserializeObject<DebutDetailsInfo>(dd)!)
           .HasColumnName("debut_details")
           .HasColumnType("jsonb");

        modelBuilder.Entity<Entities.CricketPlayerInfo>()
           .Property(e => e.CareerStatistics)
           .HasConversion(
           dd => JsonConvert.SerializeObject(dd),
           dd => JsonConvert.DeserializeObject<CareerDetailsInfo>(dd)!)
           .HasColumnName("career_statistics")
           .HasColumnType("jsonb");

        modelBuilder.Entity<Entities.CricketPlayerInfo>()
          .Property(e => e.ExtraInfo)
          .HasConversion(
          dd => JsonConvert.SerializeObject(dd),
          dd => JsonConvert.DeserializeObject<PlayerExtraInfo>(dd)!)
          .HasColumnName("extra_info")
          .HasColumnType("jsonb");
    }
}