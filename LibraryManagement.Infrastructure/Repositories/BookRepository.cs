using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace LibraryManagement.Infrastructure.Repositories;

public class BookRepository : IBookRepo
{
    private readonly AppDbContext _context;
    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Book> GetAllBooks()
    {
        return _context.Books
        .Include(b => b.Author)
        .Include(b => b.Category)
        .ToList();
    }

    public Book? GetBookById(int id)
    {
        return _context.Books
        .Include(b => b.Author)
        .Include(b => b.Category)
        .Include(b => b.BorrowRecords)
        .FirstOrDefault(b => b.Id == id);
    }
    public int CountBooks()
    {
        return _context.Books.Count();
    }
    public bool IsbnExists(string isbn, int? excludeBookId = null)
    {
        return _context.Books.Any(b => b.ISBN == isbn && b.Id != excludeBookId);
    }

    public void UpdateBook(Book book)
    {
        _context.Books.Update(book);
        _context.SaveChanges();

    }

    public void DeleteBook(int id)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == id);
        if (book != null)
        {
            _context.Books.Remove(book);
            _context.SaveChanges();
        }
    }

    public int AddBook(Book book)
    {
        _context.Books.Add(book);
        _context.SaveChanges();
        return book.Id;
    }

    public PagedResult<Book> SearchBooks(string? title, string? author, int? categoryId, bool? isAvailable, int page = 1, int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 50) pageSize = 50;

        var query = _context.Books
        .Include(b => b.Author)
        .Include(b => b.Category)
        .Include(b => b.BorrowRecords)
        .AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(b => b.Title.Contains(title));
        if (!string.IsNullOrWhiteSpace(author))
            query = query.Where(b => b.Author!.FirstName.Contains(author.Trim()) || b.Author.LastName.Contains(author.Trim()));
        if (categoryId.HasValue)
            query = query.Where(b => b.CategoryId == categoryId.Value);
        if (isAvailable.HasValue)
        {
            if (isAvailable.Value)
                query = query.Where(b => !b.BorrowRecords.Any(r => r.ReturnedAt == null));
            else
                query = query.Where(b => b.BorrowRecords.Any(r => r.ReturnedAt == null));
        }
        var totalCount = query.Count();

        var books = query
        .OrderBy(b => b.Title)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToList();

        return new PagedResult<Book>
        {
            Items = books,
            TotalCount = totalCount
        };
    }
}