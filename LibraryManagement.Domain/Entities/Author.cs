namespace LibraryManagement.Domain.Entities;
public class Author
{
    public int Id { get; set; }
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public string Bio { get; set; } = String.Empty;
    public List<Book> Books { get; set; }

    public Author()
    {
        Books = new List<Book>();

    }

    public void AddBook(Book book)
    {
        Books.Add(book);
    }
}