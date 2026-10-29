using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Entities;
namespace LibraryManagement.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepo _repo;

    public CategoryService(ICategoryRepo repo)
    {
        _repo = repo;
    }

    public List<Category> GetAllCategories() => _repo.GetAllCategories();
    public Category? GetCategoryByID(int id) => _repo.GetCategoryByID(id);
    public void UpdateCategory(Category category) => _repo.UpdateCategory(category);
    public void DeleteCategory(int id) => _repo.DeleteCategory(id);
    public int AddCategory(Category category) => _repo.AddCategory(category);
}