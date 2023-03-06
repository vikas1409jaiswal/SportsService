using CricketService.Domain;
using CricketService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CricketService.Data.Contexts;

public class CricketServiceContext : DbContext
{
    public CricketServiceContext(DbContextOptions<CricketServiceContext> options)
        : base(options)
    {
    }

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

        modelBuilder.Entity<Entities.CricketTeamInfo>()
         .Property(e => e.Formats)
         .HasConversion(
            f => JsonConvert.SerializeObject(f.Select(x => x.ToString())),
            f => JsonConvert.DeserializeObject<IEnumerable<CricketFormat>>(f)!);

        modelBuilder.Entity<Entities.CricketPlayerInfo>()
          .HasKey(e => e.Uuid);

        modelBuilder.Entity<Entities.CricketPlayerInfo>()
         .Property(e => e.Formats)
         .HasConversion(
            f => JsonConvert.SerializeObject(f.Select(x => x.ToString())),
            f => JsonConvert.DeserializeObject<IEnumerable<CricketFormat>>(f)!);
    }
}