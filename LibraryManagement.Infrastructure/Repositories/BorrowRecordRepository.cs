using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace LibraryManagement.Infrastructure.Repositories;

public class BorrowRecordRepository : IBorrowRecordRepo
{
    private readonly AppDbContext _context;

    public BorrowRecordRepository(AppDbContext context)
    {
        _context = context;
    }
    public BorrowRecord? GetOpenBorrowByBookId(int bookId)
    {
        return _context.BorrowRecords.FirstOrDefault(b => b.BookId == bookId && b.ReturnedAt == null);
    }
    public BorrowRecord? GetBorrowRecordById(int recordId)
    {
        return _context.BorrowRecords.FirstOrDefault(b => b.Id == recordId);
    }
    public int AddBorrowRecord(BorrowRecord record)
    {
        _context.BorrowRecords.Add(record);
        _context.SaveChanges();
        return record.Id;
    }
    public void UpdateBorrowRecord(BorrowRecord record)
    {
        _context.BorrowRecords.Update(record);
        _context.SaveChanges();
    }
    public List<BorrowRecord> GetHistoryByMemberId(int memberId)
    {
        return _context.BorrowRecords
         .Where(b => b.LibraryMemberId == memberId)
         .Include(r => r.Book)
         .Include(r => r.LibraryMember)
         .ToList();
    }
    public List<BorrowRecord> GetHistoryByBookId(int bookId)
    {
        return _context.BorrowRecords
        .Where(b => b.BookId == bookId)
        .Include(r => r.Book)
        .Include(r => r.LibraryMember)
        .ToList();
    }
    public int CountActiveLoans(int memberid)
    {
        return _context.BorrowRecords.Count(r => r.LibraryMemberId == memberid && r.ReturnedAt == null);
    }
    public List<BorrowRecord> ActiveLoans()
    {
        return _context.BorrowRecords
        .Include(r => r.Book)
        .Include(r => r.LibraryMember)
        .Where(r => r.ReturnedAt == null)
        .ToList();
    }
    public List<BorrowRecord> GetOverdueLoans(DateTime now)
    {
        return _context.BorrowRecords
        .Include(r => r.Book)
        .Include(r => r.LibraryMember)
        .Where(r => r.ReturnedAt == null && r.DueDate < now)
        .ToList();
    }
    public int CountActiveLoans()
    {
        return _context.BorrowRecords.Count(r=>r.ReturnedAt==null);
    }
    public int CountOverdueLoans()
    {
        return _context.BorrowRecords.Count(r=>r.ReturnedAt==null && r.DueDate<DateTime.UtcNow);
    }
}