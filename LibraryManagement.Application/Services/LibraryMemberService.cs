using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
namespace LibraryManagement.Application.Services;

public class LibraryMemberService : ILibraryMemberService
{
    private readonly ILibraryMemberRepo _repo;
    private readonly IAuthenticationService _userContext;

    public LibraryMemberService(ILibraryMemberRepo repo, IAuthenticationService userContext)
    {
        _repo = repo;
        _userContext = userContext;
    }

    public List<LibraryMember> GetAllLibraryMembers() => _repo.GetAllLibraryMembers();
    public LibraryMember? GetLibraryMemberByID(int id) => _repo.GetLibraryMemberByID(id);
    public LibraryMember? GetCurrentMember()
    {
        var Email = _userContext.GetEmail();
        if (string.IsNullOrWhiteSpace(Email))
        {
            return null;
        }
        return _repo.GetByEmail(Email);
    }
    public MemberResult UpdateLibraryMember(LibraryMember libraryMember)
    {
        var existing = _repo.GetLibraryMemberByID(libraryMember.Id);
        if (existing == null)
            return MemberResult.NotFound;

        libraryMember.Email = libraryMember.Email.Trim();
        if (_repo.EmailExists(libraryMember.Email, libraryMember.Id))
            return MemberResult.DuplicateEmail;

        existing.FirstName = libraryMember.FirstName;
        existing.LastName = libraryMember.LastName;
        existing.Email = libraryMember.Email;

        _repo.UpdateLibraryMember(existing);
        return MemberResult.Success;
    }
    public void DeleteLibraryMember(int id) => _repo.DeleteLibraryMember(id);
    public MemberResult AddLibraryMember(LibraryMember libraryMember)
    {
        libraryMember.Email = libraryMember.Email.Trim();
        if (_repo.EmailExists(libraryMember.Email))
            return MemberResult.DuplicateEmail;
        _repo.AddLibraryMember(libraryMember);
        return MemberResult.Success;
    }
}