using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    /// <summary>
    /// Bundle pour FournisseurIndex
    /// </summary>
    public class FournisseurIndexBundleFactory : BundleFactory
    {
        public FournisseurIndexBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/FournisseurIndexBundle.js");
            Js("~/Areas/Fournisseur/Scripts/fournisseur.controller.js",
                 "~/Areas/Fournisseur/Scripts/fournisseur.service.js",
                 "~/Scripts/directive/searchFilter.js",
                 "~/Scripts/filter/check-empty.filter.js",
                 "~/Scripts/module/Google/google.service.js",
                 "~/Scripts/module/Google/adresse.model.js",
                 "~/Areas/Pointage/RapportListe/Scripts/Services/rapport.service.js",
                 "~/Scripts/Factory/favori/favori.component.js",
                 "~/Scripts/Factory/favori/favori.service.js",
                 "~/Scripts/module/Favoris/Service.js",
                 "~/Areas/Fournisseur/Mocks/datas.js",
                 "~/Scripts/module/BarreRecherche/barre-recherche.js");

            CssBundleName("~/FournisseurIndexBundle.css");
            Css("~/Content/FormPanel/style.css",
                 "~/Areas/Fournisseur/Content/style.css",
                 "~/Content/BarreRecherche/barre-recherche.css",
                 "~/Content/BarreRecherche/rightSearch.css");
        }
    }
}

