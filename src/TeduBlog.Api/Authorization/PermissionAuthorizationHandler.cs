using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TeduBlog.Core.Domain.Identity;
using TeduBlog.Core.SeedWorks.Constants;

namespace TeduBlog.Api.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        public PermissionAuthorizationHandler(RoleManager<AppRole> roleManager,
            UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User.Identity.IsAuthenticated == false)
            {
                return;
            }
            var user = await _userManager.FindByNameAsync(context.User.Identity.Name);
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(Roles.Admin))
            {
                context.Succeed(requirement);
                return;
            }
            var allPermissions = new List<Claim>();
            foreach (var role in roles)
            {
                var roleEntity = await _roleManager.FindByNameAsync(role);
                var roleClaims = await _roleManager.GetClaimsAsync(roleEntity);
                allPermissions.AddRange(roleClaims);

            }
            var permissions = allPermissions.Where(x => x.Type == "Permission" &&
                                                                x.Value == requirement.Permission &&
                                                                x.Issuer == "LOCAL AUTHORITY");
            if (permissions.Any())
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}
