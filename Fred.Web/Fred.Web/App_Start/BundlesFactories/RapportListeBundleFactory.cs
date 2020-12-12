using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    /// <summary>
    /// Bundle pour AreasPointageViewsRapportIndex
    /// </summary>
    public class RapportListeBundleFactory : BundleFactory
    {
        public RapportListeBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/RapportListeBundle.js");
            Js(
                "~/Areas/Pointage/RapportListe/Scripts/rapport.controller.js",

                "~/Areas/Pointage/RapportListe/Scripts/Modals/edition-paie-modal.component.js",

                "~/Areas/Pointage/RapportListe/Scripts/Services/rapport.service.js",
                "~/Areas/Pointage/RapportListe/Scripts/Services/etat-paie.service.js",

                "~/Scripts/module/DateTimePicker/datetimepickerDirective.js",
                "~/Areas/Organisation/Scripts/organisation.service.js");

            CssBundleName("~/RapportListeBundle.css");
            Css("~/Content/FormPanel/style.css",
                "~/Areas/Pointage/RapportListe/Content/RapportListe.css",
                "~/Content/BarreRecherche/rightSearch.css",
                "~/Content/BarreRecherche/barre-recherche.css");
        }
    }
}

