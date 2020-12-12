using System.Web.Optimization;

namespace Fred.Web.App_Start.BundlesFactories
{
    /// <summary>
    /// Bundle pour la bibliothèque des prix.
    /// </summary>
    public class BudgetBibliothequePrixBundleFactory : BundleFactory
    {
        public BudgetBibliothequePrixBundleFactory(BundleCollection bundles)
                      : base(bundles)
        {
            JsBundleName("~/BudgetBibliothequePrixBundle.js");
            Js(
                "~/Areas/Budget/Scripts/BibliothequePrix/bibliotheque-prix.classes.js",
                "~/Areas/Budget/Scripts/BibliothequePrix/bibliotheque-prix.controller.js",
                "~/Areas/Budget/Scripts/BibliothequePrix/bibliotheque-prix.service.js",
                "~/Areas/Budget/Scripts/BibliothequePrix/bibliotheque-prix.validator.js",
                "~/Areas/Budget/Scripts/BibliothequePrix/components/bibliotheque-prix-item-historique.component.js",
                "~/Areas/Budget/Scripts/BibliothequePrix/components/bibliotheque-prix-enregistrement-sur-ci.component.js",
                "~/Areas/Budget/Scripts/BibliothequePrix/components/bibliotheque-prix-copier-valeurs.component.js",
                "~/Areas/Budget/Scripts/budget.service.js",
                "~/Areas/Organisation/Scripts/organisation.service.js",
                "~/Areas/CI/Scripts/services/ci.service.js",
                "~/Scripts/filter/code-libelle.filter.js",
                "~/Scripts/directive/fred-one-time-binding-refresher.js"
            );

            CssBundleName("~/BudgetBibliothequePrixBundle.css");
            Css(
                "~/Areas/Budget/Content/bibliotheque-prix.css",
                "~/Areas/Budget/Content/bibliotheque-prix-historique.css",
                "~/Areas/Budget/Content/bibliotheque-prix-enregistrement-ci.css",
                "~/Scripts/directive/UI/fred-flex-table.css",
                 "~/Content/FormPanel/style.css"
            );
        }
    }
}
