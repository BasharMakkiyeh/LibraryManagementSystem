using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Application.Mappers;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService _bookService)
    {
        this._bookService = _bookService;
    }


    [HttpGet]
    public IActionResult GetAllBooks()
    {
        var books = _bookService.GetAllBooks().Select(BookMapper.ToReadDto).ToList();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public IActionResult GetBookByID(int id)
    {
        var book = _bookService.GetBookById(id);
        if (book == null)
        {
            return NotFound();
        }
        var result = BookMapper.ToReadDto(book);
        return Ok(result);
    }

    [Authorize(Roles = "Admin,Librarian")]
    [HttpPost]
    public IActionResult AddBook([FromBody] CreateBookDto Dto)
    {
        var book = new Book()
        {
            Title = Dto.Title,
            ISBN = Dto.ISBN,
            PublishYear = Dto.PublishYear,
            AuthorId = Dto.AuthorId,
            CategoryId = Dto.CategoryId,
        };
        var addResult = _bookService.AddBook(book);

        if (addResult == AddBookResult.DuplicateIsbn)
        {
            return Conflict(new { message = "A book with this ISBN Already Exists." });
        }

        var saved = _bookService.GetBookById(book.Id);
        var readDto = BookMapper.ToReadDto(saved!);
        return CreatedAtAction(nameof(GetBookByID), new { id = book.Id }, readDto);
    }

    [Authorize(Roles = "Admin,Librarian")]
    [HttpPut("{id}")]
    public IActionResult UpdateBook(int id, [FromBody] CreateBookDto Dto)
    {
        var book = new Book()
        {
            Id = id,
            Title = Dto.Title,
            ISBN = Dto.ISBN,
            PublishYear = Dto.PublishYear,
            AuthorId = Dto.AuthorId,
            CategoryId = Dto.CategoryId
        };
        var result = _bookService.UpdateBook(book);

        return result switch
        {
            AddBookResult.NotFound => NotFound(),
            AddBookResult.DuplicateIsbn => Conflict(new { message = "A book with this ISBN Already Exists." }),
            AddBookResult.Success => NoContent(),
            _ => BadRequest()
        };
    }

    [Authorize(Roles = "Admin,Librarian")]
    [HttpDelete("{id}")]
    public IActionResult DeleteBook(int id)
    {
        var existingBook = _bookService.GetBookById(id);
        if (existingBook != null)
        {
            _bookService.DeleteBook(id);
            return NoContent();
        }
        return NotFound();
    }

    [HttpGet("search")]
    public IActionResult SearchBooks(string? title, string? author, int? categoryId, bool? isAvailable, int page = 1, int pageSize = 10)
    {
        var result=_bookService.SearchBooks(title,author,categoryId,isAvailable,page,pageSize);
        return Ok(result);

    }
}