using System.Linq;
using System.Security.Claims;
using System.Threading;
using Fred.Entities;

namespace Fred.Web.Modules.Authorization.Common
{
    /// <summary>
    /// Helper pour les  AuthorizeAttibute.
    /// 
    /// </summary>
    public static class AuthorizeAttibuteHelper
    {

        public static bool IsSuperAdmin()
        {
            System.Security.Principal.IPrincipal principle = System.Threading.Thread.CurrentPrincipal;
            bool isSuperAdmin = principle.IsInRole("SuperAdmin");
            return isSuperAdmin;
        }

        public static bool HasPermissionClaim(string globalPermissionKey)
        {
            var identity = (ClaimsIdentity)Thread.CurrentPrincipal.Identity;
            var hasClaimPermission = identity.Claims.FirstOrDefault(c => c.Type == globalPermissionKey) != null;
            return hasClaimPermission;
        }

        public static bool HasPermissionClaimOnMode(string globalPermissionKey, FonctionnaliteTypeMode mode)
        {
            var identity = (ClaimsIdentity)Thread.CurrentPrincipal.Identity;

            var claim = identity.Claims.FirstOrDefault(c => c.Type == globalPermissionKey);

            var claimMode = claim.Value;
            
            return claimMode != null ? int.Parse(claimMode) >= (int)mode : true;
        }
    }
}
