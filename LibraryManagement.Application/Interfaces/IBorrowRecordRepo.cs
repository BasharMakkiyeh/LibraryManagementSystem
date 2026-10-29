using LibraryManagement.Domain.Entities;
namespace LibraryManagement.Application.Interfaces;

public interface IBorrowRecordRepo
{
    BorrowRecord? GetOpenBorrowByBookId(int bookId);
    BorrowRecord? GetBorrowRecordById(int recordId);
    List<BorrowRecord> GetHistoryByMemberId(int memberId);
    List<BorrowRecord> GetHistoryByBookId(int bookId);
    int AddBorrowRecord(BorrowRecord record);
    void UpdateBorrowRecord(BorrowRecord record);
    public int CountActiveLoans(int memberid);
    List<BorrowRecord> ActiveLoans();
    List<BorrowRecord> GetOverdueLoans(DateTime now);
    public int CountActiveLoans();
    public int CountOverdueLoans();

}