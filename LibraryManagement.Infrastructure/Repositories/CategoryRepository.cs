using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.Interfaces;
namespace LibraryManagement.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepo
{
    private readonly AppDbContext _context;
    public CategoryRepository(AppDbContext context)
    {
        _context=context;
    }

    public List<Category> GetAllCategories()
    {
        return _context.Categories.ToList();
    }

    public Category? GetCategoryByID(int id)
    {
        return _context.Categories.FirstOrDefault(b => b.Id ==id);
    }

    public void UpdateCategory(Category category)
    {
        _context.Categories.Update(category);
        _context.SaveChanges();
    }

    public void DeleteCategory(int id)
    {
        var category =_context.Categories.FirstOrDefault(b => b.Id == id);
        if(category != null)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }

    public int AddCategory(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
        return category.Id;
    }
}