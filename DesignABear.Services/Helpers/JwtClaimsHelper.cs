using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DesignABear.Services.Helpers
{
    /// <summary>
    /// Utility for reading claims directly from a raw JWT Bearer token string.
    /// </summary>
    public static class JwtClaimsHelper
    {
        /// <summary>
        /// Extract a single claim value from a raw Bearer token string (e.g. from Authorization header).
        /// </summary>
        /// <param name="authHeader">Value of the Authorization header, e.g. "Bearer eyJ..."</param>
        /// <param name="claimType">Claim type to look for (e.g. "id", "email", ClaimTypes.Role).</param>
        public static string? GetClaim(string authHeader, string claimType)
        {
            try
            {
                var tokenString = authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                    ? authHeader["Bearer ".Length..].Trim()
                    : authHeader.Trim();

                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(tokenString);
                return token.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the user's Guid from the "id" claim of the Authorization header.
        /// </summary>
        public static Guid? GetUserId(string authHeader)
        {
            var raw = GetClaim(authHeader, "id");
            return Guid.TryParse(raw, out var id) ? id : null;
        }

        /// <summary>
        /// Returns the user's email from the "email" claim.
        /// </summary>
        public static string? GetEmail(string authHeader) =>
            GetClaim(authHeader, "email");

        /// <summary>
        /// Returns role IDs from ClaimTypes.Role claims.
        /// </summary>
        public static IEnumerable<string> GetRoles(string authHeader)
        {
            try
            {
                var tokenString = authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                    ? authHeader["Bearer ".Length..].Trim()
                    : authHeader.Trim();

                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(tokenString);
                return token.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value);
            }
            catch
            {
                return Enumerable.Empty<string>();
            }
        }
    }
}
