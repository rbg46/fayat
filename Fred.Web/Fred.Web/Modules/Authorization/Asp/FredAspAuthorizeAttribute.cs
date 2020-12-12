using Fred.Web.Modules.Authorization.Common;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fred.Web.Modules.Authorization
{
  /// <summary>
  /// Attribut qui authorize l'acces au page en fonction de permission non contextuelle.
  /// globalPermissionKey est la valeur de PermissionKey d'une PermissionEnt.
  /// </summary>
  [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false)]
  public class FredAspAuthorizeAttribute : AuthorizeAttribute
  {
    private readonly string globalPermissionKey;

    public FredAspAuthorizeAttribute(string globalPermissionKey)
    {
      this.globalPermissionKey = globalPermissionKey;
    }

    /// <summary>
    /// OnAuthorization
    /// </summary>
    /// <param name="filterContext">filterContext</param>
    public override void OnAuthorization(AuthorizationContext filterContext)
    {
      bool isSuperAdmin = AuthorizeAttibuteHelper.IsSuperAdmin();
      if (isSuperAdmin)
      {
        base.OnAuthorization(filterContext);
        return;
      }
      bool hasClaimPermission = AuthorizeAttibuteHelper.HasPermissionClaim(globalPermissionKey);
      if (hasClaimPermission)
      {
        base.OnAuthorization(filterContext);
      }
      else
      {
        HandleUnauthorizedRequest(filterContext);
      }
    }

    /// <summary>
    /// Override pour rediriger vers la page /Error/UnAuthorized
    /// 
    /// </summary>
    /// <param name="filterContext">filterContext</param>
    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
      if (filterContext.HttpContext.User.Identity.IsAuthenticated)
      {
        filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                              Area = string.Empty,
                              controller = "Error",
                              action = "UnAuthorized"

                            }));
      }
      else
      {
        filterContext.Result = new HttpUnauthorizedResult();
      }
    }
  }
}