using CricketService.Data.Entities;
using CricketService.Domain;
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

        modelBuilder.Entity<CricketTeamInfo>()
         .Property(p => p.Formats)
         .HasConversion(
             v => string.Join(",", v),
             v => v.Split(",", StringSplitOptions.None),
             new ValueComparer<ICollection<string>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));

        modelBuilder.Entity<Entities.CricketPlayerInfo>()
          .HasKey(e => e.Uuid);

        modelBuilder.Entity<CricketPlayerInfo>()
          .Property(p => p.InternationalTeamNames)
          .HasConversion(
              v => string.Join(",", v),
              v => v.Split(",", StringSplitOptions.None),
              new ValueComparer<ICollection<string>>(
                 (c1, c2) => c1.SequenceEqual(c2),
                 c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                 c => c.ToList()));

        modelBuilder.Entity<CricketPlayerInfo>()
         .Property(p => p.Formats)
         .HasConversion(
             v => string.Join(",", v),
             v => v.Split(",", StringSplitOptions.None),
             new ValueComparer<ICollection<string>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));
    }
}