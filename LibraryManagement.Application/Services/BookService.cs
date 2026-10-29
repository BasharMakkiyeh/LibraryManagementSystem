using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Application.Mappers;
using LibraryManagement.Application.DTOs;
namespace LibraryManagement.Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepo _repo;

    public BookService(IBookRepo repo)
    {
        _repo = repo;
    }

    public List<Book> GetAllBooks()
    {
        return _repo.GetAllBooks();
    }
    public Book? GetBookById(int id)
    {
        return _repo.GetBookById(id);
    }
    public AddBookResult UpdateBook(Book book)
    {
        var existing = _repo.GetBookById(book.Id);
        if (existing == null)
        {
            return AddBookResult.NotFound;
        }
        book.ISBN = book.ISBN.Trim();
        if (_repo.IsbnExists(book.ISBN, book.Id)) return AddBookResult.DuplicateIsbn;

        existing.Title = book.Title;
        existing.ISBN = book.ISBN;
        existing.CategoryId = book.CategoryId;
        existing.AuthorId = book.AuthorId;
        existing.PublishYear = book.PublishYear;

        _repo.UpdateBook(existing);
        return AddBookResult.Success;
    }
    public void DeleteBook(int id)
    {
        _repo.DeleteBook(id);
    }
    public AddBookResult AddBook(Book book)
    {
        book.ISBN = book.ISBN.Trim();

        if (_repo.IsbnExists(book.ISBN))
            return AddBookResult.DuplicateIsbn;

        _repo.AddBook(book);
        return AddBookResult.Success;

    }
    public PagedResult<BookReadDto> SearchBooks(string? title, string? author, int? categoryId, bool? isAvailable, int page = 1, int pageSize = 10)
    {
        var result = _repo.SearchBooks(title, author, categoryId, isAvailable, page, pageSize);

        return new PagedResult<BookReadDto>
        {
            Items = result.Items.Select(BookMapper.ToReadDto).ToList(),
            TotalCount= result.TotalCount
        };
    }

}