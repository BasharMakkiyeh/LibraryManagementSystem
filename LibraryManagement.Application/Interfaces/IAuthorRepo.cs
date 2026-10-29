
using LibraryManagement.Domain.Entities;
namespace LibraryManagement.Application.Interfaces;

public interface IAuthorRepo
{
    List<Author> GetAllAuthors();
    Author? GetAuthorById(int id);
    void UpdateAuthor(Author author);
    void DeleteAuthor(int id);
    int AddAuthor(Author author);
}