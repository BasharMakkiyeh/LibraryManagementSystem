using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using Microsoft.Extensions.Options;
using LibraryManagement.Application.Settings;
namespace LibraryManagement.Application.Services;

public class BorrowService : IBorrowRecordService
{
    private readonly IBorrowRecordRepo _repo;
    private readonly ILibraryMemberService _memberService;
    private readonly IBookService _bookService;
    private readonly LibrarySettings settings;


    public BorrowService(IBorrowRecordRepo _repo, ILibraryMemberService _service, IBookService _bookService, IOptions<LibrarySettings> options)
    {
        this._repo = _repo;
        _memberService = _service;
        this._bookService = _bookService;
        settings = options.Value;
    }


    public BorrowResult Borrow(int bookId, int memberId)
    {

        if (_memberService.GetLibraryMemberByID(memberId) == null)
            return BorrowResult.MemberNotFound;

        if (_bookService.GetBookById(bookId) == null)
            return BorrowResult.BookNotFound;

        if (_repo.GetOpenBorrowByBookId(bookId) != null)
            return BorrowResult.AlreadyBorrowed;

        if (_repo.CountActiveLoans(memberId) >= settings.MaxActiveLoans)
            return BorrowResult.LimitReached;

        var record = new BorrowRecord
        {
            BookId = bookId,
            LibraryMemberId = memberId,
            BorrowedAt = DateTime.UtcNow,
            ReturnedAt = null,
            DueDate = DateTime.UtcNow.AddDays(settings.LoanPeriodDays)

        };
        _repo.AddBorrowRecord(record);
        return BorrowResult.Success;
    }

    public bool CheckAvailability(int bookId)
    {
        if (_repo.GetOpenBorrowByBookId(bookId) != null)
        {
            return false;
        }
        return true;
    }

    public List<BorrowRecord> ViewHistoryByBook(int bookId)
    {
        return _repo.GetHistoryByBookId(bookId);
    }

    public List<BorrowRecord> ViewHistoryByMember(int memberId)
    {
        return _repo.GetHistoryByMemberId(memberId);
    }
    public ReturnResult Return(int borrowRecordId)
    {
        var record = _repo.GetBorrowRecordById(borrowRecordId);
        if (record == null)
        {
            return ReturnResult.NotFound;
        }
        if (record.ReturnedAt != null)
        {
            return ReturnResult.AlreadyReturned;
        }
        record.ReturnedAt = DateTime.UtcNow;
        _repo.UpdateBorrowRecord(record);
        return ReturnResult.Success;
    }
    public List<BorrowRecord> ViewActiveLoans()
    {
        return _repo.ActiveLoans();
    }
    public List<BorrowRecord> ViewOverdueLoans()
    {
        return _repo.GetOverdueLoans(DateTime.UtcNow);
    }
}