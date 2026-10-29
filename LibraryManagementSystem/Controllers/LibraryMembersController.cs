using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LibraryManagement.Application.Mappers;
using LibraryManagement.Domain.Enums;
namespace LibraryManagement.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]

public class LibraryMembersController : ControllerBase
{
    private readonly ILibraryMemberService _memberService;

    public LibraryMembersController(ILibraryMemberService _memberService)
    {
        this._memberService = _memberService;
    }

    [HttpGet("me")]
    public IActionResult GetMyProfile()
    {
        var member = _memberService.GetCurrentMember();
        if (member == null)
        {
            return NotFound("No member profile is linked to your account.");
        }
        var result = new LibraryMemberReadDto()
        {
            Id = member.Id,
            FirstName = member.FirstName,
            LastName = member.LastName,
            Email = member.Email,
            MembershipDate = member.MembershipDate
        };
        return Ok(result);
    }

    [Authorize(Roles = "Admin,Librarian")]
    [HttpGet]
    public IActionResult GetAllLibraryMembers()
    {
        var result = _memberService.GetAllLibraryMembers().Select(LibraryMembersMapper.ToReadDto)
        .ToList();
        return Ok(result);
    }

    [Authorize(Roles = "Admin,Librarian")]
    [HttpGet("{id}")]
    public IActionResult GetLibraryMemberById(int id)
    {
        var member = _memberService.GetLibraryMemberByID(id);
        if (member == null)
        {
            return NotFound();
        }
        var result = LibraryMembersMapper.ToReadDto(member);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult AddLibraryMember([FromBody] CreateLibraryMemberDto Dto)
    {
        var member = new LibraryMember()
        {
            FirstName = Dto.FirstName,
            LastName = Dto.LastName,
            Email = Dto.Email,
            MembershipDate = DateTime.Now
        };
        var addResult = _memberService.AddLibraryMember(member);

        if (addResult == MemberResult.DuplicateEmail)
        {
            return Conflict(new { msg = "A member with this Email Already Exists" });
        }

        var readDto = LibraryMembersMapper.ToReadDto(member);

        return CreatedAtAction(nameof(GetLibraryMemberById), new { id = member.Id }, readDto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public IActionResult UpdateLibraryMember(int id, [FromBody] CreateLibraryMemberDto Dto)
    {
        var member = new LibraryMember()
        {
            Id = id,
            FirstName = Dto.FirstName,
            LastName = Dto.LastName,
            Email = Dto.Email
        };
        var result = _memberService.UpdateLibraryMember(member);
        
        return result switch
        {
            MemberResult.Success => NoContent(),
            MemberResult.NotFound => NotFound(),
            MemberResult.DuplicateEmail=> Conflict(new { msg = "A member with this Email Already Exists" }),
            _=> BadRequest()
        };
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public IActionResult DeleteLibraryMember(int id)
    {
        var member = _memberService.GetLibraryMemberByID(id);
        if (member == null)
        {
            return NotFound();
        }
        _memberService.DeleteLibraryMember(id);
        return NoContent();
    }
}