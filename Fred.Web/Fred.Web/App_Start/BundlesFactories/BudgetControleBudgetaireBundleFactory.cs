using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    /// <summary>
    /// Bundle pour BudgetIndex
    /// </summary>
    public class BudgetControleBudgetaireBundleFactory : BundleFactory
    {
        public BudgetControleBudgetaireBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/BudgetControleBudgetaireBundle.js");
            Js("~/Scripts/expr-eval/dist/bundle.min.js",
                 "~/Areas/Budget/Scripts/ControleBudgetaire/budget-controle-budgetaire.controller.js",
                 "~/Areas/Budget/Scripts/Services/ControleBudgetaireCalcul.service.js",
                 "~/Areas/Budget/Scripts/budget.service.js",
                 "~/Areas/Budget/Scripts/ControleBudgetaire/components/budget-controle-budgetaire-arbre-axes.component.js",
                 "~/Areas/Budget/Scripts/ControleBudgetaire/components/budget-ci-modal.component.js",
                 "~/Areas/Budget/Scripts/Services/BudgetMathService.js",
                 "~/Areas/Budget/Scripts/Services/BudgetCopyPasteService.js",
                 "~/Areas/Budget/Scripts/Services/RessourceManagerService.js",
                 "~/Areas/Budget/Scripts/Services/CiManagerService.js",
                 "~/Areas/Budget/Scripts/Services/DevisesManagerService.js",
                 "~/Areas/Budget/Scripts/Services/budgetDate.service.js",
                 "~/Areas/Budget/Directives/SelectOnClick.js",
                 "~/Scripts/Module/Notification/module.js",
                 "~/Scripts/module/DateTimePicker/datetimepickerDirective.js",
                 "~/Scripts/directive/select-on-click.directive.js",
                 "~/Areas/Budget/Scripts/ControleBudgetaire/components/budget-controle-budgetaire-validation-erreur-dialog.component.js",
                 "~/Areas/Budget/Scripts/ControleBudgetaire/components/budget-controle-budgetaire-validation-verrouillage-precedents-dialog.component.js",
                 "~/Areas/CI/Scripts/services/ci.service.js",
                 "~/Scripts/directive/table/fred-table-header.directive.js",
                 "~/Areas/ReferentielTaches/Scripts/referentieltaches.service.js",
                 "~/Areas/ReferentielFixes/Scripts/referentiel-fixe.service.js"
            );

            CssBundleName("~/BudgetControleBudgetaireBundle.css");
            Css("~/Areas/Budget/Content/ControleBudgetaire.css",
                "~/Content/FormPanel/style.css",
                "~/Scripts/directive/table/fred-table-header.css");
        }
    }
}

