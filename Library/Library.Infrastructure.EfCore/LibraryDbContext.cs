using Library.Domain.Model;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Library.Infrastructure.EfCore;

/// <summary>
/// Контекст базы данных для библиотеки на MongoDB
/// </summary>
public class LibraryDbContext(DbContextOptions options) : DbContext(options)
{
    /// <summary>
    /// Коллекция книг
    /// </summary>
    public DbSet<Book> Books { get; set; }

    /// <summary>
    /// Коллекция записей о выдаче книг
    /// </summary>
    public DbSet<BookLoan> Loans { get; set; }

    /// <summary>
    /// Коллекция видов изданий
    /// </summary>
    public DbSet<EditionType> EditionTypes { get; set; }

    /// <summary>
    /// Коллекция издательств
    /// </summary>
    public DbSet<Publisher> Publishers { get; set; }

    /// <summary>
    /// Коллекция читателей
    /// </summary>
    public DbSet<Reader> Readers { get; set; }

    /// <summary>
    /// Настройка моделей и связей
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToCollection("books");
            entity.HasKey(b => b.Id);
            entity.Property(b => b.CatalogCode).HasMaxLength(50);
            entity.Property(b => b.Authors).HasMaxLength(256);
            entity.Property(b => b.Title).HasMaxLength(256);

            entity.HasOne(b => b.EditionType)
                .WithMany()
                .HasForeignKey(b => b.EditionTypeId);

            entity.HasOne(b => b.Publisher)
                .WithMany()
                .HasForeignKey(b => b.PublisherId);
        });

        modelBuilder.Entity<EditionType>(entity =>
        {
            entity.ToCollection("editionTypes");
            entity.HasKey(et => et.Id);
            entity.Property(et => et.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.ToCollection("publishers");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Reader>(entity =>
        {
            entity.ToCollection("readers");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.FullName).HasMaxLength(256);
            entity.Property(r => r.Address).HasMaxLength(512);
            entity.Property(r => r.Phone).HasMaxLength(50);
        });

        modelBuilder.Entity<BookLoan>(entity =>
        {
            entity.ToCollection("bookLoans");
            entity.HasKey(l => l.Id);

            entity.HasOne(l => l.Book)
                .WithMany()
                .HasForeignKey(l => l.BookId);

            entity.HasOne(l => l.Reader)
                .WithMany()
                .HasForeignKey(l => l.ReaderId);
        });

        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
    }
}