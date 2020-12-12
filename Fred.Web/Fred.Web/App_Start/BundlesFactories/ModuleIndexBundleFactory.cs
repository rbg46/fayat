using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour ModuleIndex
  /// </summary>
  public class ModuleIndexBundleFactory : BundleFactory
  {
    public ModuleIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/ModuleIndexBundle.js");
      Js("~/Areas/Module/Scripts/services/module.service.js",
         "~/Areas/Module/Scripts/services/module-modale.service.js",
         "~/Areas/Module/Scripts/services/module-visibility-manager.service.js",
         "~/Areas/Module/Scripts/services/module-permission-helper.service.js",
         "~/Areas/Module/Scripts/modals/feature/open-create-feature-modal.controller.js",        
         "~/Areas/Module/Scripts/modals/feature/open-edit-feature-modal.controller.js",
         "~/Areas/Module/Scripts/modals/module/open-create-module-modal.controller.js",
         "~/Areas/Module/Scripts/modals/module/open-edit-module-modal.controller.js",
         "~/Areas/Module/Scripts/components/module-arbo.component.js",
         "~/Areas/Module/Scripts/components/pole/pole.component.js",
         "~/Areas/Module/Scripts/components/pole/groupe/groupe.component.js",
         "~/Areas/Module/Scripts/components/pole/groupe/societe/societe.component.js",
         "~/Areas/Module/Scripts/components/pole/groupe/societe/child-societe/child-societe.component.js",
         "~/Areas/Module/Scripts/components/services/type-organisation-converter.service.js",
         "~/Areas/Module/Scripts/components/services/module-arbo-data.service.js",
         "~/Areas/Module/Scripts/components/services/module-arbo-store.service.js",
         "~/Areas/Module/Scripts/components/services/module-arbo.service.js",
         "~/Areas/Module/Scripts/directives/indeterminate-checkbox.js",
         "~/Areas/Module/Scripts/models/organisation-type.js",
         "~/Areas/Module/Scripts/directives/item-with-same-property-already-exist-validator.directive.js",


          "~/Areas/Module/Scripts/module.controller.js",
          "~/Scripts/filter/code-libelle.filter.js");

      CssBundleName("~/ModuleIndexBundle.css");
      Css("~/Content/FormPanel/style.css",
           "~/Areas/Module/Content/style.css");
    }
  }
}

