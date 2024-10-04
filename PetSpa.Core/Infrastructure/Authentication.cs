using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace PetSpa.Core.Infrastructure
{
    public class Authentication
    {
        public static string GetUserIdFromHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor?.HttpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor), "HttpContextAccessor or HttpContext cannot be null");
            }

            return ExtractClaimFromAuthorizationHeader(httpContextAccessor.HttpContext.Request.Headers["Authorization"], "id");
        }

        public static string GetUserIdFromHttpContext(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext), "HttpContext cannot be null");
            }

            return ExtractClaimFromAuthorizationHeader(httpContext.Request.Headers["Authorization"], "id");
        }

        public static string GetUserRoleFromHttpContext(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext), "HttpContext cannot be null");
            }

            return ExtractClaimFromAuthorizationHeader(httpContext.Request.Headers["Authorization"], ClaimTypes.Role);
        }

        public static string GetFullNameFromClaims(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor?.HttpContext?.User == null)
            {
                return string.Empty;
            }

            var claimsIdentity = httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            return claimsIdentity?.FindFirst("fullName")?.Value ?? string.Empty;
        }

        private static string ExtractClaimFromAuthorizationHeader(string authorizationHeader, string claimType)
        {
            if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedException("Authorization header is missing or not a Bearer token.");
            }

            string jwtToken = authorizationHeader.Substring("Bearer ".Length).Trim();
            var tokenHandler = new JwtSecurityTokenHandler();

            if (!tokenHandler.CanReadToken(jwtToken))
            {
                throw new UnauthorizedException("Invalid token format.");
            }

            var token = tokenHandler.ReadJwtToken(jwtToken);

            // Log all claims
            LogClaims(token.Claims);

            var claim = token.Claims.FirstOrDefault(c => c.Type == claimType);
            if (claim == null)
            {
                throw new UnauthorizedException($"Claim '{claimType}' not found in the token.");
            }

            return claim.Value;
        }

        private static void LogClaims(IEnumerable<Claim> claims)
        {
            foreach (var claim in claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }
        }

        private static async Task ReturnUnauthorizedResponse(HttpContext context, string message)
        {
            if (context != null)
            {
                var errorResponse = new
                {
                    data = "An unexpected error occurred.",
                    additionalData = (object)null,
                    message,
                    statusCode = StatusCodes.Status401Unauthorized,
                    code = "Unauthorized"
                };

                var jsonResponse = JsonSerializer.Serialize(errorResponse);

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message) { }
    }
}