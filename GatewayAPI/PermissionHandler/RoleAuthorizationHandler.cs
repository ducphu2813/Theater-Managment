using Microsoft.AspNetCore.Authorization;

namespace GatewayAPI.PermissionHandler;

public class RoleAuthorizationHandler : AuthorizationHandler<RoleAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleAuthorizationRequirement requirement)
    {
        var userRoles = context.User.FindAll("roles").Select(c => c.Value).ToList();

        if (userRoles.Any(role => requirement.AllowedRoles.Contains(role)))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

public class RoleAuthorizationRequirement : IAuthorizationRequirement
{
    public string[] AllowedRoles { get; }

    public RoleAuthorizationRequirement(params string[] allowedRoles)
    {
        AllowedRoles = allowedRoles;
    }
}