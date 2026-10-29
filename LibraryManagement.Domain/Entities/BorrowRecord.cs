namespace LibraryManagement.Domain.Entities;
public class BorrowRecord
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int LibraryMemberId { get; set; }
    public DateTime BorrowedAt { get; set; }
    public DateTime DueDate {get; set;}
    public DateTime? ReturnedAt { get; set; }
    public Book? Book { get; set; }
    public LibraryMember? LibraryMember { get; set; }

public BorrowRecord()
    {
        BorrowedAt = DateTime.UtcNow;
    }
}
