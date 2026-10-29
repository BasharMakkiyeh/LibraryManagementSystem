using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Interfaces;
namespace LibraryManagement.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IBookRepo _book;
    private readonly ILibraryMemberRepo _member;
    private readonly IBorrowRecordRepo _record;
    public DashboardService(IBookRepo _book, ILibraryMemberRepo _member, IBorrowRecordRepo _record)
    {
        this._book = _book;
        this._member = _member;
        this._record = _record;
    }

    public DashboardDto GetDashboard()
    {
        return new DashboardDto
        {
            TotalBooks = _book.CountBooks(),
            TotalMembers = _member.CountMembers(),
            ActiveLoans = _record.CountActiveLoans(),
            OverdueLoans = _record.CountOverdueLoans()
        };
    }
}