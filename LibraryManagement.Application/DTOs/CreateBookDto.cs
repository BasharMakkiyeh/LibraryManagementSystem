using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTOs;

public class CreateBookDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    [Required]
    [StringLength(13, MinimumLength = 10)]
    public string ISBN { get; set; } = string.Empty;
    [Range(1000, 2100)]
    public int PublishYear { get; set; }
    [Range(1, int.MaxValue)]
    public int AuthorId { get; set; }
    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }
}

public class BookReadDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublishYear { get; set; }
    public int AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public bool isAvailable { get; set; }

}