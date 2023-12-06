using System.Security.Claims;
using TeduBlog.Core.SeedWorks.Constants;

namespace TeduBlog.WebApp.Extensions
{
    public static class IdentityExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var subjectId = claimsPrincipal.GetSpecificClaim(ClaimTypes.NameIdentifier);
            return Guid.Parse(subjectId);
        }
        public static string GetFirstName(this ClaimsPrincipal claimsPrincipal)
        {
            var subjectId = claimsPrincipal.GetSpecificClaim(UserClaims.FirstName);
            return subjectId;
        }

        public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
        {
            var subjectId = claimsPrincipal.GetSpecificClaim(ClaimTypes.Email);
            return subjectId;
        }
        public static string GetSpecificClaim(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity)?.Claims.FirstOrDefault(x => x.Type == claimType);

            return claim != null ? claim.Value : string.Empty;
        }
    }
}
