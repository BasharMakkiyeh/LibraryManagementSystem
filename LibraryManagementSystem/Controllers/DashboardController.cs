using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashService;

    public DashboardController(IDashboardService _dashService)
    {
        this._dashService=_dashService;
    }

[Authorize(Roles = "Admin,Librarian")]
[HttpGet]
public IActionResult GetDashboard()
    {
        return Ok(_dashService.GetDashboard());
    }
}