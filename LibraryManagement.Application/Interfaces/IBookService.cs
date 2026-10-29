
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Application.DTOs;
namespace LibraryManagement.Application.Interfaces;

public interface IBookService
{
    List<Book> GetAllBooks();
    Book? GetBookById(int id);
    AddBookResult UpdateBook(Book book);
    void DeleteBook(int id);
    AddBookResult AddBook(Book book);
    public PagedResult<BookReadDto> SearchBooks(string? title, string? author, int? categoryId, bool? isAvailable, int page = 1, int pageSize = 10);
}