using System.Web.Optimization;

namespace Fred.Web.App_Start.BundlesFactories
{
    public class ClassificationSocietesBundleFactory : BundleFactory
    {
        public ClassificationSocietesBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/ClassificationSocietesIndexBundle.js");
            Js("~/Areas/ClassificationSocietes/Scripts/classification-societes.controller.js",
               "~/Scripts/Angularjs/1.5.8/i18n/angular-locale_fr-fr.js",
               "~/Areas/ClassificationSocietes/Scripts/services/classification-societes.service.js",
               "~/Scripts/filter/code-libelle.filter.js"
               );

            CssBundleName("~/ClassificationSocietesIndexBundle.css");
            Css("~/Content/FormPanel/style.css",
                "~/Content/BarreRecherche/barre-recherche-light.css",
                "~/Content/Referentials/style.css");
        }
    }
}

