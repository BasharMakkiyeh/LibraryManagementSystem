using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Mappers;

public class LibraryMembersMapper
{
    public static LibraryMemberReadDto ToReadDto(LibraryMember m)
    {
        return new LibraryMemberReadDto    
        {
            Id=m.Id,
            FirstName=m.FirstName,
            LastName=m.LastName,
            Email=m.Email,
            MembershipDate=m.MembershipDate
        };
        
    }
}