using System.Web.Optimization;

namespace Fred.Web.App_Start.BundlesFactories
{
  /// <summary>
  /// Personnel equipe bundle factory
  /// </summary>
  public class PersonnelEquipeBundleFactory : BundleFactory
  {
    public PersonnelEquipeBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/PersonnelEquipeBundle.js");
      Js("~/Areas/Personnel/Scripts/personnel-equipe.controller.js",
           "~/Areas/Personnel/Scripts/personnel.service.js",
           "~/Areas/Personnel/Scripts/services/personnel-equipe.service.js",
           "~/Areas/Personnel/Scripts/components/ouvrier-picker.component.js",
           "~/Areas/Personnel/Scripts/services/ouvrier-picker.service.js");

      CssBundleName("~/PersonnelEquipeBundle.css");
      Css("~/Areas/Personnel/Content/style.css",
           "~/Areas/Personnel/Content/equipe.css",
           "~/Content/BarreRecherche/barre-recherche-light.css");
    }
  }
}