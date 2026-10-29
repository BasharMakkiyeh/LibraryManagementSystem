using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.Interfaces;
namespace LibraryManagement.Infrastructure.Repositories;

public class AuthorRepository : IAuthorRepo
{
    private readonly AppDbContext _context;
    public AuthorRepository(AppDbContext context)
    {
        _context = context;
    }

public List<Author> GetAllAuthors()
    {
        return _context.Authors.ToList();
    }
    

public Author? GetAuthorById(int id)
    {
        return _context.Authors.FirstOrDefault(a => a.Id == id);
    }

public int AddAuthor(Author author)
    {
        _context.Authors.Add(author);
        _context.SaveChanges();        
        return author.Id;
    }
public void UpdateAuthor(Author author)
    {
        _context.Authors.Update(author);
        _context.SaveChanges();
    }

public void DeleteAuthor(int id){
    var author = _context.Authors.FirstOrDefault(a => a.Id == id);
    if (author != null)
        {
            _context.Authors.Remove(author);
            _context.SaveChanges();
        }
}
}