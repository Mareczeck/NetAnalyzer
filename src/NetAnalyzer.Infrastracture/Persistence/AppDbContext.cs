using Microsoft.EntityFrameworkCore;
using NetAnalyzer.Domain.Dataset;

namespace NetAnalyzer.Infrastructure.Persistence;

public class AppDbContext : DbContext
{

    public DbSet<Relation> Relations { get; set; }
    public DbSet<DatasetInfo> DataSets { get; set; }

    public AppDbContext(DbContextOptions <AppDbContext> options) : base(options) {}
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Relation>().ToTable("Relations");
        modelBuilder.Entity<DatasetInfo>().ToTable("DatasetInfos");

        // Configure primary key and foreign key relationship
        modelBuilder.Entity<Relation>()
            .HasKey(r => r.Id);

        modelBuilder.Entity<Relation>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

            
        modelBuilder.Entity<DatasetInfo>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        // Configure primary key and foreign key relationship
        modelBuilder.Entity<DatasetInfo>()
            .HasKey(r => r.Id);

        modelBuilder.Entity<Relation>()
            .HasOne<DatasetInfo>()
            .WithMany()
            .HasForeignKey(r => r.DatasetID)
            .OnDelete(DeleteBehavior.Cascade); // Optional: Define delete behavior

        // Create index on Relation.DatasetID
        modelBuilder.Entity<Relation>()
            .HasIndex(r => r.DatasetID);

    }
}