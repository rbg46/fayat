using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour OrganisationIndex
  /// </summary>
  public class OrganisationIndexBundleFactory : BundleFactory
  {
    public OrganisationIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/OrganisationIndexBundle.js");
      Js("~/Areas/Organisation/Scripts/organisation.service.js",
              "~/Areas/Organisation/Scripts/modals/devise-seuil-validation-modal.component.js",
           "~/Areas/Organisation/Scripts/organisation.controller.js",
           "~/Scripts/filter/code-libelle.filter.js");

      CssBundleName("~/OrganisationIndexBundle.css");
      Css("~/Areas/Organisation/Content/style.css",
           "~/Content/ReferentialPickList/ReferentialPickListCaller.css");
    }
  }
}

