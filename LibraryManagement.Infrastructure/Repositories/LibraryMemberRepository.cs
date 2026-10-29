using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace LibraryManagement.Infrastructure.Repositories;

public class LibraryMemberRepository : ILibraryMemberRepo
{
    private readonly AppDbContext _context;
    public LibraryMemberRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<LibraryMember> GetAllLibraryMembers()
    {
        return _context.LibraryMembers.ToList();
    }

    public LibraryMember? GetLibraryMemberByID(int id)
    {
        return _context.LibraryMembers.FirstOrDefault(b => b.Id == id);
    }
    public int CountMembers()
    {
        return _context.LibraryMembers.Count();
    }
    public LibraryMember? GetByEmail(string email)
    {
        return _context.LibraryMembers.FirstOrDefault(e => e.Email.ToLower() == email.ToLower());
    }

    public bool EmailExists(string email, int? excludeMemberId = null)
    {
        return _context.LibraryMembers.Any(m => m.Email == email && m.Id != excludeMemberId);
    }

    public void UpdateLibraryMember(LibraryMember libraryMember)
    {
        _context.LibraryMembers.Update(libraryMember);
        _context.SaveChanges();
    }

    public void DeleteLibraryMember(int id)
    {
        var libraryMember = _context.LibraryMembers.FirstOrDefault(b => b.Id == id);
        if (libraryMember != null)
        {
            _context.LibraryMembers.Remove(libraryMember);
            _context.SaveChanges();
        }
    }

    public int AddLibraryMember(LibraryMember libraryMember)
    {
        _context.LibraryMembers.Add(libraryMember);
        _context.SaveChanges();
        return libraryMember.Id;
    }
}