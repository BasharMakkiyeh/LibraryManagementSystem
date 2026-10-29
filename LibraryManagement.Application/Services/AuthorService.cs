using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Entities;
namespace LibraryManagement.Application.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepo _repo;

    public AuthorService(IAuthorRepo repo)
    {
        _repo = repo;
    }

    public List<Author> GetAllAuthors()
    {
        return _repo.GetAllAuthors();
    }
    public Author? GetAuthorById(int id)
    {
     return _repo.GetAuthorById(id);
    }
    public void UpdateAuthor(Author author)
    {
        _repo.UpdateAuthor(author);
    }
    public void DeleteAuthor(int id)
    {
        _repo.DeleteAuthor(id);
    }
    public int AddAuthor(Author author)
    {
        return _repo.AddAuthor(author);
    }
}