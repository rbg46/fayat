using System.Web.Optimization;

namespace Fred.Web.App_Start.BundlesFactories
{
    /// <summary>
    /// Bundle pour la comparaison de budget.
    /// </summary>
    public class BudgetComparaisonBundleFactory : BundleFactory
    {
        public BudgetComparaisonBundleFactory(BundleCollection bundles)
                      : base(bundles)
        {
            JsBundleName("~/BudgetComparaisonBundle.js");
            Js(
                "~/Areas/Budget/Scripts/BudgetComparaison/budget-comparaison.controller.js",
                "~/Areas/Budget/Scripts/BudgetComparaison/budget-comparaison.service.js",
                "~/Areas/Budget/Scripts/BudgetComparaison/components/budget-comparaison-node.component.js",
                "~/Areas/Budget/Scripts/BudgetComparaison/components/budget-comparaison-filter.component.js",
                "~/Scripts/directive/fred-one-time-binding-refresher.js"
            );

            CssBundleName("~/BudgetComparaisonBundle.css");
            Css(
                "~/Areas/Budget/Content/budget-comparaison.css",
                "~/Scripts/directive/UI/fred-flex-table.css",
                "~/Content/FormPanel/style.css"
            );
        }
    }
}
