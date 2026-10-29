using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
namespace LibraryManagement.Application.Interfaces;

public interface ILibraryMemberService
{
    List<LibraryMember> GetAllLibraryMembers();
    LibraryMember? GetLibraryMemberByID(int id);
    LibraryMember? GetCurrentMember();
    MemberResult UpdateLibraryMember(LibraryMember libraryMember);
    void DeleteLibraryMember(int id);
    MemberResult AddLibraryMember(LibraryMember libraryMember);
}