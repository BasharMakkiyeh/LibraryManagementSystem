
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
namespace LibraryManagement.Application.Interfaces;

public interface IBookRepo
{
    List<Book> GetAllBooks();
    Book? GetBookById(int id);
    int CountBooks();
    bool IsbnExists(string isbn, int? excludeBookId = null);
    void UpdateBook(Book book);
    void DeleteBook(int id);
    int AddBook(Book book);
    public PagedResult<Book> SearchBooks(string? Title, string? Author, int? CategoryId, bool? isAvailable, int Page = 1, int PageSize = 10);

}