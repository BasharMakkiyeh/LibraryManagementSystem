using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using LibraryManagement.Application.Mappers;
namespace LibraryManagement.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BorrowRecordController : ControllerBase
{
    private readonly IBorrowRecordService _borrowService;
    private readonly IBookService _bookService;
    private readonly ILibraryMemberService _memberService;
    private readonly IAuthenticationService _userContext;

    public BorrowRecordController(IBorrowRecordService _borrowService, IBookService _bookService, IAuthenticationService _userContext, ILibraryMemberService _memberService)
    {
        this._borrowService = _borrowService;
        this._bookService = _bookService;
        this._userContext = _userContext;
        this._memberService = _memberService;
    }


    [HttpPost]
    public IActionResult Borrow([FromBody] CreateBorrowRecordDto Dto)
    {
        if (!_userContext.IsStaff())
        {
            var member = _memberService.GetCurrentMember();
            if (member == null || member.Id != Dto.LibraryMemberId)
                return Forbid();
        }
        var result = _borrowService.Borrow(Dto.BookId, Dto.LibraryMemberId);
        return result switch
        {
            BorrowResult.Success => NoContent(),
            BorrowResult.AlreadyBorrowed => Conflict(new { message = "This Book is Already Borrowed." }),
            BorrowResult.MemberNotFound => NotFound(new { message = "Member not found." }),
            BorrowResult.BookNotFound => NotFound(new { message = "Book not found." }),
            BorrowResult.LimitReached => Conflict(new { message = "Max Loan Count Limit Reached" }),
            _ => BadRequest()
        };
    }

    [HttpPut("{id}/return")]
    public IActionResult Return(int id)
    {
        if (!_userContext.IsStaff())
        {
            var member = _memberService.GetCurrentMember();
            if (member == null)
                return Forbid();

            var ownsRecord = _borrowService.ViewHistoryByMember(member.Id).Any(r => r.Id == id);
            if (!ownsRecord)
                return Forbid();
        }
        var result = _borrowService.Return(id);
        return result switch
        {
            ReturnResult.Success => NoContent(),
            ReturnResult.NotFound => NotFound(new { message = "Borrow record not found." }),
            ReturnResult.AlreadyReturned => Conflict(new { message = "This book was already returned." }),
            _ => BadRequest()
        };
    }

    [HttpGet("my-history")]
    public IActionResult MyHistory()
    {
        var member = _memberService.GetCurrentMember();
        if (member == null)
            return NotFound(new { message = "No member profile is linked to your account." });

        var result = _borrowService.ViewHistoryByMember(member.Id).Select(BorrowRecordsMapper.ToReadDto).ToList();
        return Ok(result);
    }

    [HttpGet("available/{bookId}")]
    public IActionResult CheckAvailability(int bookId)
    {
        var book = _bookService.GetBookById(bookId);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(_borrowService.CheckAvailability(bookId));
    }

    [Authorize(Roles = "Admin,Librarian")]
    [HttpGet("history/member/{memberId}")]
    public IActionResult ViewHistoryMember(int memberId)
    {
        var result = _borrowService.ViewHistoryByMember(memberId).Select(BorrowRecordsMapper.ToReadDto).ToList();
        return Ok(result);
    }

    [Authorize(Roles = "Admin,Librarian")]
    [HttpGet("history/book/{bookId}")]
    public IActionResult ViewHistoryBook(int bookId)
    {
        var result = _borrowService.ViewHistoryByBook(bookId).Select(BorrowRecordsMapper.ToReadDto).ToList();
        return Ok(result);
    }

    [Authorize(Roles = "Admin, Librarian")]
    [HttpGet("active")]
    public IActionResult ViewActiveLoans()
    {
        var result = _borrowService.ViewActiveLoans().Select(BorrowRecordsMapper.ToReadDto).ToList();
        return Ok(result);
    }

    [Authorize(Roles = "Admin, Librarian")]
    [HttpGet("overdue")]
    public IActionResult ViewOverdueLoans()
    {
        var result = _borrowService.ViewOverdueLoans().Select(BorrowRecordsMapper.ToReadDto).ToList();
        return Ok(result);
    }
}
