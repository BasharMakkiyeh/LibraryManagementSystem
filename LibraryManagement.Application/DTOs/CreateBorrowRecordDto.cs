using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTOs;

public class CreateBorrowRecordDto
{
    [Range(1, int.MaxValue)]
    public int BookId {get; set;}
    [Range(1, int.MaxValue)]
    public int LibraryMemberId {get; set;}
}

public class BorrowRecordReadDto
{
    public int Id {get; set;}
    public int BookId {get; set;}
    public string BookTitle { get; set; } = string.Empty;
    public int LibraryMemberId {get; set;}
    public string MemberName { get; set;} = string.Empty;
    public DateTime BorrowedAt{ get; set; }
    public DateTime DueDate {get; set;}
    public DateTime? ReturnedAt{ get; set; }
}