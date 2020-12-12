using System.Web.Optimization;

namespace Fred.Web.App_Start.BundlesFactories
{
    public class RessourcesRecommandeesBundleFactory : BundleFactory
    {
        public RessourcesRecommandeesBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/RessourcesRecommandeesBundle.js");
                 Js("~/Areas/RessourcesRecommandees/Scripts/ressources-recommandees.controller.js",
                 "~/Areas/ReferentielEtendu/Scripts/referentiel-etendu-filter.service.js",
                 "~/Areas/Nature/Scripts/nature.service.js",
                 "~/Areas/RessourcesRecommandees/Scripts/ressources-recommandees.service.js",
                 "~/Scripts/filter/code-libelle.filter.js");

            CssBundleName("~/RessourcesRecommandeesBundle.css");
            Css("~/Areas/RessourcesRecommandees/Content/style.css",
                 "~/Content/FormPanel/style.css",
                 "~/Content/ReferentialPickList/ReferentialPickList.css");
        }
    }
}

