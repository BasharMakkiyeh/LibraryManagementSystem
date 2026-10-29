using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.DTOs;
namespace LibraryManagement.Application.Mappers;

public class BookMapper(){
    public static BookReadDto ToReadDto(Book b)
    {
        return new BookReadDto
    {
        Id = b.Id,
        Title = b.Title,
        ISBN = b.ISBN,
        PublishYear = b.PublishYear,
        AuthorId = b.AuthorId,
        AuthorName = $"{b.Author?.FirstName} {b.Author?.LastName}",
        CategoryId = b.CategoryId,
        CategoryName = b.Category?.Name ?? string.Empty,
        isAvailable= !b.BorrowRecords.Any(r=>r.ReturnedAt==null)
    };
    }
}
