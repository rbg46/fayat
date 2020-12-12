using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    /// <summary>
    /// Bundle pour BudgetIndex
    /// </summary>
    public class BudgetIndexBundleFactory : BundleFactory
    {
        public BudgetIndexBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/BudgetIndexBundle.js");
            Js("~/Scripts/expr-eval/dist/bundle.min.js",
                 "~/Areas/Budget/Scripts/budget.controller.js",
                 "~/Areas/Budget/Scripts/Liste/budget-liste.controller.js",
                 "~/Areas/Budget/Scripts/TachesBudget/TachesBudgetComponent.js",
                 "~/Areas/Budget/Scripts/BudgetDetail/BudgetDetailComponent.js",
                 "~/Areas/Budget/Scripts/TachesBudget/TacheBudget/TacheBudgetComponent.js",
                 "~/Areas/Budget/Scripts/TacheDetail/TacheDetailComponent.js",
                 "~/Areas/Budget/Scripts/TacheDetail/Services/TacheCalculatorService.js",
                 "~/Areas/Budget/Scripts/TacheDetail/Services/TachePriceManagerService.js",
                 "~/Areas/Budget/Scripts/TacheDetail/Services/TacheValidatorService.js",
                 "~/Areas/Budget/Scripts/TacheDetail/Services/UniteManagerService.js",
                 "~/Areas/Budget/Scripts/Ressources/BudgetRessourcesComponent.js",
                 "~/Areas/Budget/Scripts/Ressources/Ressource/BudgetRessourceComponent.js",
                 "~/Areas/Budget/Scripts/Ressources/Ressource/CreateBudgetRessourceComponent.js",
                 "~/Areas/Budget/Scripts/budget.service.js",
                 "~/Areas/Budget/Scripts/Services/BudgetMathService.js",
                 "~/Areas/Budget/Scripts/Services/BudgetCopyPasteService.js",
                 "~/Areas/Budget/Scripts/Services/RessourceUpdaterService.js",
                 "~/Areas/Budget/Scripts/Services/ParametrageReferentielEtenduService.js",
                 "~/Areas/Budget/Scripts/Services/RessourceManagerService.js",
                 "~/Areas/Budget/Scripts/Services/RessourceTacheDeviseManagerService.js",
                 "~/Areas/Budget/Scripts/Services/CiManagerService.js",
                 "~/Areas/Budget/Scripts/Services/budgetDate.service.js",
                 "~/Areas/CI/Scripts/services/ci.service.js",
                 "~/Areas/Budget/Scripts/Services/DevisesManagerService.js",
                 "~/Areas/Budget/Scripts/TachesBudget/Services/TacheBudgetAvancementService.js",
                 "~/Areas/Budget/Scripts/TachesBudget/Services/TacheBudgetService.js",
                 "~/Areas/Budget/Scripts/TachesBudget/Services/TacheBudgetComparerService.js",
                 "~/Areas/Budget/Scripts/TachesBudget/Services/TacheBudgetRecetteService.js",
                 "~/Areas/Budget/Scripts/TachesBudget/Services/TacheBudgetMontantService.js",
                 "~/Areas/Budget/Scripts/TachesBudget/Services/TacheBudgetCodeLibelleFilterService.js",
                 "~/Areas/Budget/Scripts/TachesBudget/Services/TacheBudgetManagerService.js",
                 "~/Areas/Budget/Scripts/Liste/components/budget-liste-recalage.component.js",
                 "~/Scripts/Module/Notification/module.js",
                 "~/Areas/ReferentielTaches/Scripts/referentieltaches.service.js",
                 "~/Areas/ReferentielTaches/Scripts/ReferentielTachesComponent.js",
                 "~/Areas/ReferentielTaches/Scripts/filterCodeAndLibelle.filter.js",
                 "~/Areas/ReferentielTaches/Scripts/filterCodeAndLabelAllLevel.filter.js",
                 "~/Areas/ReferentielTaches/Scripts/Dialogs/task-one-create-modal-controller.js",
                 "~/Areas/ReferentielTaches/Scripts/Dialogs/task-one-edit-modal-controller.js",
                 "~/Areas/ReferentielTaches/Scripts/Dialogs/task-two-create-modal-controller.js",
                 "~/Areas/ReferentielTaches/Scripts/Dialogs/task-two-edit-modal-controller.js",
                 "~/Areas/ReferentielTaches/Scripts/Dialogs/task-three-create-modal-controller.js",
                 "~/Areas/ReferentielTaches/Scripts/Dialogs/task-three-edit-modal-controller.js",
                 "~/Areas/ReferentielTaches/Scripts/Dialogs/task-four-create-modal-controller.js",
                 "~/Areas/ReferentielTaches/Scripts/Dialogs/task-four-edit-modal-controller.js",
                 "~/Areas/ReferentielTaches/Scripts/referentiel-tache-view-selector.component.js",
                 "~/Areas/ReferentielTaches/Scripts/TaskManagerService.js",
                 "~/Areas/ReferentielTaches/Scripts/filterCodeAndLibelle.filter.js",
                 "~/Areas/ReferentielTaches/Scripts/filterCodeAndLabelAllLevel.filter.js",
                 "~/Scripts/module/DateTimePicker/datetimepickerDirective.js"
                 );

            CssBundleName("~/BudgetIndexBundle.css");
            Css("~/Content/module/Notification/style.css",
                 "~/Areas/ReferentielTaches/Content/style.css",
                 "~/Content/ReferentialPickList/ReferentialPickList.css",
                 "~/Areas/Budget/Content/Style.css",
                 "~/Content/FormPanel/style.css",

                 "~/Areas/Budget/Content/Detail.css");
        }
    }
}

