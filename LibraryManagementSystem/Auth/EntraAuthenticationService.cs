using System.Security.Claims;
using LibraryManagement.Application.Interfaces;

namespace LibraryManagement.Auth;

public class EntraAuthenticationService : IAuthenticationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EntraAuthenticationService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor=httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
    public string? GetEmail() =>
        User?.FindFirst("preferred_username")?.Value
        ?? User?.FindFirst(ClaimTypes.Email)?.Value
        ?? User?.FindFirst("email")?.Value;
    public bool IsInRole(string role) => User?.IsInRole(role) ?? false;
    public bool IsStaff() => IsInRole("Admin") || IsInRole("Librarian");
}

