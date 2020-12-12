using System.Web.Optimization;

namespace Fred.Web.App_Start.BundlesFactories
{
    /// <summary>
    /// Bundle pour RessourcesSpecifiquesCI
    /// </summary>
    public class RessourcesSpecifiquesCIBundleFactory : BundleFactory
    {

        public RessourcesSpecifiquesCIBundleFactory(BundleCollection bundles)
      : base(bundles)
        {
            JsBundleName("~/RessourcesSpecifiquesCIBundle.js");
            Js("~/Areas/RessourcesSpecifiquesCI/Scripts/ressources-specifiques-ci.service.js",
                 "~/Areas/RessourcesSpecifiquesCI/Scripts/ressources-specifiques-ci.controller.js",
                 "~/Areas/ReferentielEtendu/Scripts/referentiel-etendu-filter.service.js");

            CssBundleName("~/RessourcesSpecifiquesCIBundle.css");
            Css("~/Content/FormPanel/style.css",
                 "~/Areas/RessourcesSpecifiquesCI/Content/style.css");
        }
    }
}
