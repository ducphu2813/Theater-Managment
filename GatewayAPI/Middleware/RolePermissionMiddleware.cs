namespace GatewayAPI.Middleware;

public class RolePermissionMiddleware
{
    // private readonly RequestDelegate _next;
    // private readonly HttpClient _httpClient;
    //
    // public RolePermissionMiddleware(RequestDelegate next, HttpClient httpClient)
    // {
    //     _next = next;
    //     _httpClient = httpClient;
    // }
    //
    // public async Task InvokeAsync(HttpContext context)
    // {
    //     var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
    //     if (string.IsNullOrEmpty(token))
    //     {
    //         context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    //         return;
    //     }
    //
    //     var userRolesPermissions = await GetUserRolesPermissions(token);
    //     if (userRolesPermissions == null)
    //     {
    //         context.Response.StatusCode = StatusCodes.Status403Forbidden;
    //         return;
    //     }
    //
    //     context.Items["UserRolesPermissions"] = userRolesPermissions;
    //     await _next(context);
    // }
}