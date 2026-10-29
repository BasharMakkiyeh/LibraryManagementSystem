namespace LibraryManagement.Application.Settings;

public class LibrarySettings
{
    public int LoanPeriodDays { get; set; } = 14;
    public int MaxActiveLoans { get; set; } = 5;
}