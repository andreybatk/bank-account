using Hangfire.Dashboard;

namespace BankAccount.API.Helpers;

public class AllowAnonymousDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context) => true;
}