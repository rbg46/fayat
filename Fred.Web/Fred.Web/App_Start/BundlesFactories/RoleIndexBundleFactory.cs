using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour RoleIndex
  /// </summary>
  public class RoleIndexBundleFactory : BundleFactory
  {
    public RoleIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/RoleIndexBundle.js");
      Js("~/Areas/Role/Scripts/role.service.js",
        "~/Areas/Role/Scripts/role.controller.js",
        "~/Areas/Role/Scripts/components/fred-tree-state-toggle.component.js",
        "~/Areas/Role/Scripts/modals/role/create-role-modal.controller.js",
        "~/Areas/Role/Scripts/modals/role/edit-role-modal.controller.js",    
        "~/Areas/Role/Scripts/modals/role-duplicate/duplicate-role-modal.controller.js",
        "~/Areas/Role/Scripts/modals/role-fonctionnalite/create-role-fonctionnalite-modal.controller.js",
        "~/Areas/Role/Scripts/modals/role-fonctionnalite/role-fonctionnalite-detail.controller.js",
        "~/Areas/Role/Scripts/modals/role-seuil-validation/create-role-seuil-validation-modal.controller.js",
        "~/Areas/Role/Scripts/modals/role-seuil-validation/edit-role-seuil-validation-modal.controller.js",
        "~/Areas/Role/Scripts/models/code-role.model.js",
        "~/Areas/Role/Scripts/services/fonctionnalite-mode.service.js",
        "~/Areas/Role/Scripts/services/role-state-manager.service.js",
        "~/Areas/Role/Scripts/services/role-provider.service.js",                        
        "~/Areas/Role/Scripts/services/role-modal.service.js");

      CssBundleName("~/RoleIndexBundle.css");
      Css("~/Areas/Role/Content/style.css",
          "~/Content/BarreRecherche/barre-recherche-light.css");
    }
  }
}

