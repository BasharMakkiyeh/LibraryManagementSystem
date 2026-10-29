using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LibraryManagement.Application.Mappers;
namespace LibraryManagement.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
private readonly ICategoryService _CategoryService;

public CategoriesController(ICategoryService _CategoryService)
    {
        this._CategoryService=_CategoryService;
    }
    
[HttpGet]
public IActionResult GetAllCategories()
    {
        var result=_CategoryService.GetAllCategories().Select(CategoriesMapper.ToReadDto)
        .ToList();
        return Ok(result);
    }

[HttpGet("{id}")]
public IActionResult GetCategoryById(int id)
    {
        var category=_CategoryService.GetCategoryByID(id);
        if (category == null)
        {
            return NotFound();
        }
        var result= CategoriesMapper.ToReadDto(category);
            return Ok(result);
    }

[Authorize(Roles = "Admin,Librarian")]
[HttpPost]
public IActionResult AddCategory([FromBody] CreateCategoryDto Dto)
    {
        var category = new Category()
        {
            Name=Dto.Name,
            Description=Dto.Description,
        };

        var newCategoryId=_CategoryService.AddCategory(category);

        var result = CategoriesMapper.ToReadDto(category);
        return CreatedAtAction(nameof (GetCategoryById), new {id=newCategoryId}, result);
    }

[Authorize(Roles = "Admin,Librarian")]
[HttpPut("{id}")]
public IActionResult UpdateCategory(int id, [FromBody] CreateCategoryDto Dto)
    {
        var existingCategory=_CategoryService.GetCategoryByID(id);
        if (existingCategory == null)
        {
            return NotFound();
        }
        existingCategory.Name=Dto.Name;
        existingCategory.Description=Dto.Description;
        _CategoryService.UpdateCategory(existingCategory);
        return NoContent();
    }

[Authorize(Roles = "Admin")]
[HttpDelete("{id}")]
public IActionResult DeleteCategory(int id)
    {
        var existingCategory=_CategoryService.GetCategoryByID(id);
        if (existingCategory != null)
        {
            _CategoryService.DeleteCategory(id);
            return NoContent();
        }
        return NotFound();
    }
















}