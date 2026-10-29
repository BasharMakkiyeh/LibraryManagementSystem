    namespace LibraryManagement.Domain.Entities;
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string ISBN { get; set; } = String.Empty;
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public int PublishYear { get; set; }
        public Author? Author { get; set; }
        public Category? Category { get; set; }
        public List<BorrowRecord> BorrowRecords { get; set; }

    public Book()
    {
        BorrowRecords = new List<BorrowRecord>();
    }


    }