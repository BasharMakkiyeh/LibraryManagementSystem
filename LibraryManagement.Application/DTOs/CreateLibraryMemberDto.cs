using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTOs;

public class CreateLibraryMemberDto
{
    [Required]
    [MaxLength(50)]
    public string FirstName {get; set;} = string.Empty;
    [Required]
    [MaxLength(50)]
    public string LastName {get; set;} = string.Empty;
    [Required]
    [MaxLength(50)]
    [EmailAddress]
    public string Email {get; set;} = string.Empty;
    
}
public class LibraryMemberReadDto
{
    public int Id {get; set;}
    public string FirstName {get; set;} = string.Empty;
    public string LastName {get; set;} = string.Empty;
    public string Email {get; set;} = string.Empty;
    public DateTime MembershipDate {get; set;}
    
}