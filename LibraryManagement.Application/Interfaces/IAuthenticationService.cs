namespace LibraryManagement.Application.Interfaces;
public interface IAuthenticationService
{
    bool IsAuthenticated { get; }
    string? GetEmail();
    bool IsInRole(string role);
    bool IsStaff();
}
