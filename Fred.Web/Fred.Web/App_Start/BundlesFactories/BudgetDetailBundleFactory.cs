using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    /// <summary>
    /// Bundle pour BudgetIndex
    /// </summary>
    public class BudgetDetailBundleFactory : BundleFactory
    {
        public BudgetDetailBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/BudgetDetailBundle.js");
            Js(
                "~/Areas/Budget/Scripts/budget.service.js",
                "~/Areas/Budget/Scripts/budget.calculator.js",

                "~/Areas/Budget/Scripts/BudgetDetail/budget-detail.controller.js",
                "~/Areas/Budget/Scripts/BudgetDetail/budget-detail.service.js",
                "~/Areas/Budget/Scripts/BudgetDetail/budget-detail.calculator.js",
                "~/Areas/Budget/Scripts/BudgetDetail/components/budget-detail-liste.component.js",
                "~/Areas/Budget/Scripts/BudgetDetail/components/gestion-t4-dialog.component.js",
                "~/Areas/Budget/Scripts/BudgetDetail/components/copier-deplacer-T4-dialog.component.js",
                "~/Areas/Budget/Scripts/BudgetDetail/components/copier-deplacer-T4-confirmation-dialog.component.js",
                "~/Areas/Budget/Scripts/BudgetDetail/components/budget-detail-customize-display-panel.component.js",
                "~/Areas/Budget/Scripts/BudgetDetail/components/budget-detail-export-popup.component.js",
                "~/Areas/Budget/Scripts/BudgetDetail/components/budget-validation-dialog.component.js",

                "~/Areas/Budget/Scripts/BudgetSousDetail/budget-sous-detail.calculator.js",
                "~/Areas/Budget/Scripts/BudgetSousDetail/budget-sous-detail-sd.calculator.js",
                "~/Areas/Budget/Scripts/BudgetSousDetail/budget-sous-detail-t4.calculator.js",
                "~/Areas/Budget/Scripts/BudgetSousDetail/components/budget-sous-detail-liste.component.js",
                "~/Areas/Budget/Scripts/BudgetSousDetail/components/budget-sous-detail-ressource-panel.component.js",
                "~/Areas/Budget/Scripts/BudgetSousDetail/components/budget-sous-detail-navigate-t4.component.js",
                "~/Areas/Budget/Scripts/BudgetSousDetail/components/budget-plan-tache-lateral.component.js",
                "~/Areas/Budget/Scripts/BudgetSousDetail/components/input.formule.component.js",

                "~/Areas/Budget/Scripts/Common/components/budget-commentaire-panel.component.js",




                "~/Areas/Budget/Scripts/components/budget-ressource.component.js",
                "~/Areas/Budget/Scripts/components/budget-ressource-create.component.js",
                "~/Scripts/helpers/throttle.js",
                "~/Scripts/directive/table/fred-table.directive.js",

                "~/Areas/Budget/Scripts/Services/ParametrageReferentielEtenduService.js",
                "~/Areas/Budget/Scripts/Services/CiManagerService.js",
                "~/Areas/Budget/Scripts/Services/RessourceManagerService.js",
                "~/Areas/Budget/Scripts/Services/RessourceUpdaterService.js",
                "~/Areas/Budget/Scripts/TachesBudget/Services/TacheBudgetService.js",
                "~/Areas/Budget/Scripts/TacheDetail/Services/UniteManagerService.js",

                "~/Areas/Budget/Scripts/BudgetDetail/components/copier-budget-dialog.component.js",
                "~/Areas/Budget/Scripts/BudgetDetail/components/copier-budget-confirm-dialog.component.js"
              );

            CssBundleName("~/BudgetDetailBundle.css");
            Css(
              "~/Areas/Budget/Content/budget-detail-main.css",
              "~/Areas/Budget/Content/budget-detail.css",
              "~/Areas/Budget/Content/budget-commentaire-panel.css",
              "~/Areas/Budget/Content/budget-sous-detail.css",
              "~/Areas/Budget/Content/budget-sous-detail-ressource-panel.css",
              "~/Areas/Budget/Content/copier-deplacer-T4-dialog.css",
              "~/Areas/Budget/Content/copier-deplacer-T4-confirmation-dialog.css",
              "~/Areas/Budget/Content/gestion-t4-dialog.css",
              "~/Areas/Budget/Content/copier-budget-dialog.css",
              "~/Areas/Budget/Content/copier-budget-confirm-dialog.css",
              "~/Content/FormPanel/style.css"
              );

            //"~/Content/module/Notification/style.css"
            //"~/Areas/ReferentielTaches/Content/style.css"
            //"~/Content/ReferentialPickList/ReferentialPickList.css"
            //"~/Areas/Budget/Content/Style.css"

        }
    }
}
