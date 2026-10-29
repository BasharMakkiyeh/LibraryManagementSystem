using System.ComponentModel.DataAnnotations;
namespace LibraryManagement.Application.DTOs;

public class CreateCategoryDto
{
    [Required]
    [MaxLength(50)]
    public string Name {get; set;} = string.Empty;
    [MaxLength(500)]
    public string Description {get; set;} = string.Empty;
}

public class CategoryReadDto
{
    public int Id {get; set;}
    public string Name {get; set;} = string.Empty;
    public string Description {get; set;} = string.Empty;
}
