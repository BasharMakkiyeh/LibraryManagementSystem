
using LibraryManagement.Domain.Entities;
namespace LibraryManagement.Application.Interfaces;
public interface ILibraryMemberRepo
{
    List<LibraryMember> GetAllLibraryMembers(); 
    LibraryMember? GetLibraryMemberByID(int id);
    LibraryMember? GetByEmail(string email);
    int CountMembers();
    bool EmailExists(string Email, int? excludeMemberId = null);
    void UpdateLibraryMember(LibraryMember libraryMember);
    void DeleteLibraryMember(int id);
    int AddLibraryMember(LibraryMember libraryMember);
}