namespace LibraryManagement.Domain.Entities;
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public List<Book> Books { get; set; }

    public Category()
    {
        Books = new List<Book>();
    }
}