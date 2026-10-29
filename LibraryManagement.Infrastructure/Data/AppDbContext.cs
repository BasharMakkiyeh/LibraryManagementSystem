using Microsoft.EntityFrameworkCore;
using LibraryManagement.Domain.Entities;
namespace LibraryManagement.Infrastructure.Data;
public class AppDbContext : DbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<LibraryMember> LibraryMembers { get; set; }
    public DbSet<BorrowRecord> BorrowRecords { get; set; }


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

modelBuilder.Entity<BorrowRecord>()
    .HasOne(record => record.Book)
    .WithMany(book => book.BorrowRecords)
    .HasForeignKey(record => record.BookId)
    .OnDelete(DeleteBehavior.Restrict);

modelBuilder.Entity<BorrowRecord>()
    .HasOne(record => record.LibraryMember)
    .WithMany(member => member.BorrowRecords)
    .HasForeignKey(record => record.LibraryMemberId)
    .OnDelete(DeleteBehavior.Restrict);

modelBuilder.Entity<LibraryMember>()
    .HasIndex(member => member.Email)
    .IsUnique();
    
modelBuilder.Entity<Book>()
    .HasOne(book=> book.Author)
    .WithMany(author=> author.Books)
    .HasForeignKey(book=> book.AuthorId)
    .OnDelete(DeleteBehavior.Restrict);

modelBuilder.Entity<Book>()
    .HasIndex(book => book.ISBN)
    .IsUnique();

modelBuilder.Entity<Book>()
    .HasOne(book => book.Category)
    .WithMany(category => category.Books)
    .HasForeignKey(book => book.CategoryId)
    .OnDelete(DeleteBehavior.Restrict);
    
    }
}