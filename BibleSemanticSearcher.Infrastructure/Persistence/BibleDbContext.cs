using BibleSemanticSearcher.Domain.Bible;

using Microsoft.EntityFrameworkCore;

namespace BibleSemanticSearcher.Infrastructure.Persistence;
public class BibleDbContext(DbContextOptions<BibleDbContext> options) : DbContext(options)
{
    public DbSet<Verse> Verses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");

        modelBuilder.Entity<Verse>()
            .HasKey(verse => new { verse.Book, verse.Chapter, verse.VerseNumber });
        modelBuilder.Entity<Verse>()
            .Property(verse => verse.Source)
            .HasConversion(
                uri => uri.ToString(),
                str => new Uri(str)
            );
        modelBuilder.Entity<Verse>()
            .Property(verse => verse.Text)
            .HasColumnType("text");
        modelBuilder.Entity<Verse>()
            .Property(verse => verse.Book)
            .HasColumnType("varchar(50)");
        modelBuilder.Entity<Verse>()
            .Property(verse => verse.Chapter)
            .HasColumnType("int");
        modelBuilder.Entity<Verse>()
            .Property(verse => verse.VerseNumber)
            .HasColumnType("int");
        modelBuilder.Entity<Verse>()
            .Property(verse => verse.Source)
            .HasColumnType("varchar(255)");
        modelBuilder.Entity<Verse>()
            .ToTable("verses");

        modelBuilder.Entity<Verse>()
            .HasIndex(verse => verse.Text)
            .HasMethod("GIN")
            .IsTsVectorExpressionIndex("spanish");

        modelBuilder.Entity<Verse>()
            .HasIndex(i => i.Embedding)
            .HasMethod("hnsw")
            .HasOperators("vector_l2_ops")
            .HasStorageParameter("m", 16)
            .HasStorageParameter("ef_construction", 64);

        base.OnModelCreating(modelBuilder);
    }
}
