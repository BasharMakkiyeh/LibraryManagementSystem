using Moq;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.Services;

namespace LibraryManagement.Tests.Services;

public class BookServiceTests
{
    private readonly Mock<IBookRepo> _repo = new();
    private readonly BookService _service;

    public BookServiceTests()
    {
        _service = new BookService(_repo.Object);
    }

    [Fact]
    public void AddBook_DuplicateIsbn_ReturnsDuplicateIsbn()
    {
        _repo.Setup(r => r.IsbnExists("123")).Returns(true);

        var result = _service.AddBook(new Book { ISBN = "123" });

        Assert.Equal(AddBookResult.DuplicateIsbn, result);
        _repo.Verify(r => r.AddBook(It.IsAny<Book>()), Times.Never);
    }

    [Fact]
    public void AddBook_Success_ReturnsSuccess()
    {
        _repo.Setup(r => r.IsbnExists("123")).Returns(false);

        var result = _service.AddBook(new Book { ISBN = "123" });

        Assert.Equal(AddBookResult.Success, result);
        _repo.Verify(r => r.AddBook(It.Is<Book>(b => b.ISBN == "123")), Times.Once);
    }

    [Fact]
    public void UpdateBook_NotFound_ReturnsNotFound()
    {
        _repo.Setup(r => r.GetBookById(99)).Returns((Book?)null);

        var result = _service.UpdateBook(new Book { Id = 99, ISBN = "123" });

        Assert.Equal(AddBookResult.NotFound, result);
        _repo.Verify(r => r.UpdateBook(It.IsAny<Book>()), Times.Never);
    }

    [Fact]
    public void UpdateBook_DuplicateISBN_ReturnsDuplicateISBN()
    {
        _repo.Setup(r => r.GetBookById(99)).Returns(new Book { Id = 99, ISBN = "999" });
        _repo.Setup(r => r.IsbnExists("123", 99)).Returns(true);
        var result = _service.UpdateBook(new Book { Id = 99, ISBN = "123" });

        Assert.Equal(AddBookResult.DuplicateIsbn, result);
        _repo.Verify(r => r.UpdateBook(It.IsAny<Book>()), Times.Never);
    }

    [Fact]
    public void UpdateBook_Success_ReturnsSuccess()
    {
        _repo.Setup(r => r.IsbnExists("123", 99)).Returns(false);
        _repo.Setup(r => r.GetBookById(99)).Returns(new Book { Id = 99, Title = "Old", ISBN = "999" });
        var result = _service.UpdateBook(new Book { Id = 99, Title = "New", ISBN = "123" });
        Assert.Equal(AddBookResult.Success, result);
        _repo.Verify(r => r.UpdateBook(It.Is<Book>(b => b.Title == "New" && b.ISBN == "123")), Times.Once);
    }
}