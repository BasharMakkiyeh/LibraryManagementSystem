using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace LibraryManagement.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]

public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorsController(IAuthorService _service)
    {
        _authorService=_service;
    }

[HttpGet]
public IActionResult GetAllAuthors()
    {
        var result=_authorService.GetAllAuthors().Select(AuthorMapper.ToReadDto).ToList();
        return Ok(result);
    }

[HttpGet("{id}")]
public IActionResult GetAuthorById(int id)
    {
        
        var author=_authorService.GetAuthorById(id);
       
        if (author == null)
        {
            return NotFound();
        }
         var result =AuthorMapper.ToReadDto(author);
        return Ok(result);
    }

[Authorize(Roles = "Admin,Librarian")]
[HttpPost]
public IActionResult AddAuthor([FromBody] CreateAuthorDto Dto)
    {
        var author= new Author()
        {
            FirstName=Dto.FirstName,
            LastName=Dto.LastName,
            Bio=Dto.Bio
        };

        var newAuthorId=_authorService.AddAuthor(author);

        var result = AuthorMapper.ToReadDto(author);

        return CreatedAtAction(nameof (GetAuthorById),new {id = newAuthorId }, result);
    }

[Authorize(Roles = "Admin,Librarian")]
[HttpPut("{id}")]
public IActionResult UpdateAuthor(int id,[FromBody] CreateAuthorDto Dto)
    {        
    var existingAuthor=_authorService.GetAuthorById(id);
    
        if (existingAuthor == null)
        {
            return NotFound();
        }
        existingAuthor.FirstName = Dto.FirstName;
        existingAuthor.LastName = Dto.LastName;
        existingAuthor.Bio = Dto.Bio;
        _authorService.UpdateAuthor(existingAuthor);
        return NoContent();
    }

[Authorize(Roles = "Admin")]
[HttpDelete("{id}")]
public IActionResult DeleteAuthor(int id)
    {
        var ExistingAuthor=_authorService.GetAuthorById(id);
        if(ExistingAuthor != null)
        {
            _authorService.DeleteAuthor(id);
            return NoContent();
        }
        return NotFound();
    }

}
