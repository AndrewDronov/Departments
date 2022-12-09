using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Status.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public AuthenticationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public Task Invoke(HttpContext httpContext)
        {
            string token = null;

            if (httpContext.Request.Headers.Keys.Contains("Authorization"))
            {
                if (httpContext.Request.Headers.TryGetValue("Authorization", out var values))
                {
                    var bearerToken = values.ToString();

                    if (bearerToken.Contains("Bearer"))
                    {
                        token = bearerToken.Replace("Bearer ", "").Trim();
                    }
                }
            }

            if (token == null || token != _configuration["Token"])
            {
                httpContext.Response.StatusCode = 401;
                return Task.CompletedTask;
            }

            return _next(httpContext);
        }
    }
    
    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}