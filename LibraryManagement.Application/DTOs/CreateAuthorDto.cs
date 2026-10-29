using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTOs;

public class CreateAuthorDto
{
    [Required]
    [MaxLength(100)]
    public string FirstName {get; set;} = string.Empty;
    [Required]
    [MaxLength(100)]
    public string LastName {get; set;} = string.Empty;
    [MaxLength(1000)]
    public string Bio {get; set;} = string.Empty;
}
public class AuthorReadDto
{
    public int Id {get; set;}
    public string FirstName {get; set;} = string.Empty;
    public string LastName {get; set;} = string.Empty;
    public string Bio {get; set;} = string.Empty;
}

