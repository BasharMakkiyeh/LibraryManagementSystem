
using LibraryManagement.Domain.Entities;
namespace LibraryManagement.Application.Interfaces;
public interface ICategoryRepo
{
    List<Category> GetAllCategories(); 
    Category? GetCategoryByID(int id);
    void UpdateCategory(Category category);
    void DeleteCategory(int id);
    int AddCategory(Category category);
}