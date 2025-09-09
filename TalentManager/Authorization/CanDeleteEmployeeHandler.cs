namespace TalentManager.Authorization
{
    using Microsoft.AspNetCore.Authorization;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using TalentManager.Models;

    public sealed class CanDeleteEmployeeHandler : AuthorizationHandler<CanDeleteEmployeeRequirement, Employee>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            CanDeleteEmployeeRequirement requirement, 
            Employee employee)
        {
            var user = context.User;

            // Must be in HR Manager role
            if (!user.IsInRole("Human Resources Manager"))
                return Task.CompletedTask;

            var dept = user.FindFirst("Department")?.Value;
            var country = user.FindFirst("Country")?.Value;

            if (dept == employee.Department && country == employee.Country)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
