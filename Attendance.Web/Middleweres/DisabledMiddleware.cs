namespace Attendance.Web.CustomMiddleWhere;
public class DisabledMiddleware
{
    private readonly RequestDelegate _next;
    public DisabledMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext httpContext, UserManager<UserModel> userManager)
    {
        var user = await userManager.GetUserAsync(httpContext.User);
        if (user is not null)
        {
            if (user.Status == UserStatus.Inactive.ToString())
            {
                var path = httpContext.Request.Path.ToString();
                if (!path.ToLower().Contains("Lockout".ToLower()))
                    httpContext.Response.Redirect("/Identity/Account/Lockout", true);
                return;

            }
        }
        await _next(httpContext);
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class DisabledMiddlewareExtensions
{
    public static IApplicationBuilder UseDisabledMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<DisabledMiddleware>();
    }
}
