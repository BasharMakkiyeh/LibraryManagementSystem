using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.DTOs;
namespace LibraryManagement.Application.Mappers;

public class AuthorMapper
{
    public static AuthorReadDto ToReadDto(Author a)
    {
        return new AuthorReadDto
        {
            Id = a.Id,
            FirstName = a.FirstName,
            LastName = a.LastName,
            Bio = a.Bio
        };
    }
   
}