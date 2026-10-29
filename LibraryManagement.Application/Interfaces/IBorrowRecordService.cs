using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
namespace LibraryManagement.Application.Interfaces;

public interface IBorrowRecordService
{
    bool CheckAvailability(int bookId);
    List<BorrowRecord> ViewHistoryByMember(int memberId);
    List<BorrowRecord> ViewHistoryByBook(int bookId);
    BorrowResult Borrow(int bookId, int memberId);
    ReturnResult Return(int BorrowRecordId);
    List<BorrowRecord> ViewActiveLoans();
    List<BorrowRecord> ViewOverdueLoans();
}