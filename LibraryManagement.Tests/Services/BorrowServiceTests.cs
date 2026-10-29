using Moq;
using Microsoft.Extensions.Options;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.Services;
using LibraryManagement.Application.Settings;

namespace LibraryManagement.Tests.Services;

public class BorrowServiceTests
{
    private readonly Mock<IBorrowRecordRepo> _repo = new();
    private readonly Mock<ILibraryMemberService> _members = new();
    private readonly Mock<IBookService> _books = new();
    private readonly BorrowService _service;

    public BorrowServiceTests()
    {
        var settings = Options.Create(new LibrarySettings { MaxActiveLoans = 5, LoanPeriodDays = 14 });
        _service = new BorrowService(_repo.Object, _members.Object, _books.Object, settings);
    }

    [Fact]
    public void Borrow_MemberNotFound_ReturnsMemberNotFound()
    {
        _members.Setup(m => m.GetLibraryMemberByID(99)).Returns((LibraryMember?)null);
        var result = _service.Borrow(bookId: 1, memberId: 99);

        Assert.Equal(BorrowResult.MemberNotFound, result);
        _repo.Verify(r => r.AddBorrowRecord(It.IsAny<BorrowRecord>()), Times.Never);
    }

    [Fact]
    public void Borrow_BookNotFound_ReturnsBookNotFound()
    {
        _members.Setup(m => m.GetLibraryMemberByID(1)).Returns(new LibraryMember());
        _books.Setup(b => b.GetBookById(99)).Returns((Book?)null);

        var result = _service.Borrow(bookId: 99, memberId: 1);

        Assert.Equal(BorrowResult.BookNotFound, result);
        _repo.Verify(r => r.AddBorrowRecord(It.IsAny<BorrowRecord>()), Times.Never);
    }

    [Fact]
    public void Borrow_AlreadyBorrowed_ReturnsAlreadyBorrowed()
    {
        _members.Setup(m => m.GetLibraryMemberByID(1)).Returns(new LibraryMember());
        _books.Setup(b => b.GetBookById(99)).Returns(new Book());
        _repo.Setup(r => r.GetOpenBorrowByBookId(99)).Returns(new BorrowRecord());

        var result = _service.Borrow(bookId: 99, memberId: 1);
        Assert.Equal(BorrowResult.AlreadyBorrowed, result);
        _repo.Verify(r => r.AddBorrowRecord(It.IsAny<BorrowRecord>()), Times.Never);
    }

    [Fact]
    public void Borrow_LimitReached_ReturnsLimitReached()
    {
        _books.Setup(b => b.GetBookById(99)).Returns(new Book());
        _members.Setup(m => m.GetLibraryMemberByID(1)).Returns(new LibraryMember());
        _repo.Setup(r => r.CountActiveLoans(1)).Returns(5);

        var result = _service.Borrow(bookId: 99, memberId: 1);
        Assert.Equal(BorrowResult.LimitReached, result);
        _repo.Verify(r => r.AddBorrowRecord(It.IsAny<BorrowRecord>()), Times.Never);
    }

    [Fact]
    public void Borrow_Success_ReturnsSuccess()
    {
        _books.Setup(b => b.GetBookById(99)).Returns(new Book());
        _members.Setup(m => m.GetLibraryMemberByID(1)).Returns(new LibraryMember());
        _repo.Setup(r => r.GetOpenBorrowByBookId(99)).Returns((BorrowRecord?)null);
        _repo.Setup(r => r.CountActiveLoans(1)).Returns(2);

        var result = _service.Borrow(bookId: 99, memberId: 1);
        Assert.Equal(BorrowResult.Success, result);
        _repo.Verify(r => r.AddBorrowRecord(It.Is<BorrowRecord>(x => x.BookId == 99 && x.LibraryMemberId == 1)), Times.Once);

    }

    [Fact]
    public void Return_NotFound_RetunsNotFound()
    {
        _repo.Setup(r => r.GetBorrowRecordById(99)).Returns((BorrowRecord?)null);

        var result = _service.Return(borrowRecordId: 99);
        Assert.Equal(ReturnResult.NotFound, result);
        _repo.Verify(r => r.UpdateBorrowRecord(It.IsAny<BorrowRecord>()), Times.Never);
    }

    [Fact]
    public void Return_AlreadyReturned_ReturnsAlreadyReturned()
    {
        _repo.Setup(r => r.GetBorrowRecordById(1)).Returns(new BorrowRecord { ReturnedAt = DateTime.UtcNow });

        var result = _service.Return(borrowRecordId: 1);
        Assert.Equal(ReturnResult.AlreadyReturned, result);
        _repo.Verify(r => r.UpdateBorrowRecord(It.IsAny<BorrowRecord>()), Times.Never);
    }

    [Fact]
    public void Return_Success_ReturnsSuccess()
    {
        _repo.Setup(r => r.GetBorrowRecordById(1)).Returns(new BorrowRecord() { ReturnedAt = null });

        var result = _service.Return(borrowRecordId: 1);
        Assert.Equal(ReturnResult.Success, result);
        _repo.Verify(r => r.UpdateBorrowRecord(It.Is<BorrowRecord>(x => x.ReturnedAt != null)), Times.Once);
    }
    
    
}