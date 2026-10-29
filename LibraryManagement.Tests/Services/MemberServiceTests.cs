using Moq;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.Services;

namespace LibraryManagement.Tests.Services;

public class MemberServiceTests
{
    private readonly Mock<ILibraryMemberRepo> _repo = new();
    private readonly Mock<IAuthenticationService> _userContext = new();
    private readonly LibraryMemberService _service;

    public MemberServiceTests()
    {
        _service = new LibraryMemberService(_repo.Object, _userContext.Object);
    }

    [Fact]
    public void AddLibraryMember_DuplicateEmail_ReturnsDuplicateEmail()
    {
        _repo.Setup(r => r.EmailExists("a@b.com")).Returns(true);
        var result = _service.AddLibraryMember(new LibraryMember { Email = "a@b.com" });
        Assert.Equal(MemberResult.DuplicateEmail, result);
        _repo.Verify(r => r.AddLibraryMember(It.IsAny<LibraryMember>()), Times.Never);
    }

    [Fact]
    public void AddLibraryMember_Success_ReturnsSuccess()
    {
        _repo.Setup(r => r.EmailExists("a@b.com")).Returns(false);

        var result = _service.AddLibraryMember(new LibraryMember { Email = "a@b.com" });
        Assert.Equal(MemberResult.Success, result);
        _repo.Verify(r => r.AddLibraryMember(It.Is<LibraryMember>(m => m.Email == "a@b.com")), Times.Once);

    }

    [Fact]
    public void UpdateLibraryMember_NotFound_ReturnsNotFound()
    {
        _repo.Setup(r => r.GetLibraryMemberByID(99)).Returns((LibraryMember?)null);

        var result = _service.UpdateLibraryMember(new LibraryMember { Id = 99, Email = "a@b.com" });
        Assert.Equal(MemberResult.NotFound, result);
        _repo.Verify(r => r.UpdateLibraryMember(It.IsAny<LibraryMember>()), Times.Never);
    }
    [Fact]
    public void UpdateLibraryMember_DuplicateEmail_ReturnsDuplicateEmail()
    {
        _repo.Setup(r => r.GetLibraryMemberByID(99)).Returns(new LibraryMember { Id = 99, Email = "old@b.com" });
        _repo.Setup(r => r.EmailExists("a@b.com", 99)).Returns(true);
        var result = _service.UpdateLibraryMember(new LibraryMember { Id = 99, Email = "a@b.com" });
        Assert.Equal(MemberResult.DuplicateEmail, result);
        _repo.Verify(r => r.UpdateLibraryMember(It.IsAny<LibraryMember>()), Times.Never);
    }

    [Fact]
    public void UpdateLibraryMember_Success_ReturnsSuccess()
    {
        _repo.Setup(r => r.GetLibraryMemberByID(99)).Returns(new LibraryMember { Id = 99, FirstName = "Old", Email = "old@b.com" });
        _repo.Setup(r => r.EmailExists("a@b.com", 99)).Returns(false);
        var result = _service.UpdateLibraryMember(new LibraryMember { Id = 99, FirstName = "New", Email = "a@b.com" });
        Assert.Equal(MemberResult.Success, result);
        _repo.Verify(r => r.UpdateLibraryMember(It.Is<LibraryMember>(m => m.FirstName == "New" && m.Email == "a@b.com")), Times.Once);

    }
}