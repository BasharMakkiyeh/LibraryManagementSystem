using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Mappers;

public static class BorrowRecordsMapper
{
    public static BorrowRecordReadDto ToReadDto(BorrowRecord r)
    {
        return new BorrowRecordReadDto
        {
            Id = r.Id,
            BookId = r.BookId,
            BookTitle = r.Book?.Title ?? string.Empty,
            LibraryMemberId = r.LibraryMemberId,
            MemberName = $"{r.LibraryMember?.FirstName} {r.LibraryMember?.LastName}",
            BorrowedAt = r.BorrowedAt,
            DueDate=r.DueDate,
            ReturnedAt = r.ReturnedAt
        };
    }
}