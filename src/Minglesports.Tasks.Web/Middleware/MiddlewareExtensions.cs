using Microsoft.AspNetCore.Builder;

namespace Minglesports.Tasks.Web.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseUserContext(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserContextMiddleware>();
        }
    }
}