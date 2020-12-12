using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    /// <summary>
    /// Bundle pour SocieteIndex
    /// </summary>
    public class SocieteIndexBundleFactory : BundleFactory
    {
        public SocieteIndexBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/SocieteIndexBundle.js");
            Js("~/Areas/Societe/Scripts/services/societe.service.js",
                 "~/Areas/Societe/Scripts/societe.controller.js",
                 "~/Areas/Societe/Scripts/societe-image-selector.component.js",
                 "~/Areas/Societe/Scripts/modals/associe-sep-modal.component.js",
                 "~/Areas/Societe/Scripts/modals/suppression-societe-modal.component.js",
                 "~/Areas/Societe/Scripts/services/type-participation-sep.service.js",
                 "~/Areas/Fournisseur/Scripts/fournisseur.service.js");

            CssBundleName("~/SocieteIndexBundle.css");
            Css("~/Content/FormPanel/style.css",
                 "~/Content/BarreRecherche/barre-recherche-light.css",
                 "~/Content/Referentials/style.css",
                 "~/Areas/Societe/Content/style.css",
                 "~/Areas/Societe/Content/associe-sep-modal.component.css");
        }
    }
}

