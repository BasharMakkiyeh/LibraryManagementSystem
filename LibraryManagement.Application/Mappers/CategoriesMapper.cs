using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Mappers;

public class CategoriesMapper
{
    public static CategoryReadDto ToReadDto(Category c)
    {
        return new CategoryReadDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        };

    }
}