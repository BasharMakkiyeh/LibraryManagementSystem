namespace LibraryManagement.Domain.Entities;
public class LibraryMember
{
    public int Id { get; set; }
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public DateTime MembershipDate { get; set; }
    public List<BorrowRecord> BorrowRecords { get; set; }

    public LibraryMember()
    {
        BorrowRecords = new List<BorrowRecord>();
    }
}