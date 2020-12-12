using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    /// <summary>
    /// Bundle pour Avancement
    /// </summary>
    public class AvancementBundleFactory : BundleFactory
    {
        public AvancementBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/AvancementBundle.js");
            Js(
                "~/Scripts/expr-eval/dist/bundle.min.js",
                "~/Areas/Budget/Scripts/Avancement/avancement.controller.js",
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
                "~/Areas/Budget/Scripts/Services/DevisesManagerService.js",
                "~/Areas/Budget/Scripts/Services/Avancement/AvancementCalcul.service.js",
                "~/Areas/Budget/Scripts/Services/Avancement/Excel/AvancementExcelModelBuilder.service.js",
                "~/Areas/Budget/Scripts/TachesBudget/Services/TacheBudgetAvancementService.js",
                "~/Areas/Budget/Scripts/TachesBudget/Services/TacheBudgetService.js",
                "~/Areas/Budget/Scripts/TachesBudget/Services/TacheBudgetComparerService.js",
                "~/Areas/Budget/Scripts/TachesBudget/Services/TacheBudgetRecetteService.js",
                "~/Areas/Budget/Scripts/TachesBudget/Services/TacheBudgetMontantService.js",
                "~/Areas/Budget/Scripts/TachesBudget/Services/TacheBudgetCodeLibelleFilterService.js",
                "~/Areas/Budget/Scripts/TachesBudget/Services/TacheBudgetManagerService.js",
                "~/Areas/Budget/Scripts/Common/components/budget-commentaire-panel.component.js",
                "~/Areas/Budget/Scripts/filterTaches.filter.js",
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
                "~/Scripts/module/DateTimePicker/datetimepickerDirective.js",
                "~/Areas/CI/Scripts/services/ci.service.js",

                "~/Areas/Budget/Scripts/Avancement/components/validation-avancement-modal.component.js"
            );

            CssBundleName("~/AvancementBundle.css");
            Css("~/Content/module/Notification/style.css",
                 "~/Areas/ReferentielTaches/Content/style.css",
                 "~/Content/ReferentialPickList/ReferentialPickList.css",
                 "~/Areas/Budget/Content/Style.css",
                 "~/Content/FormPanel/style.css",
                 "~/Areas/Budget/Content/budget-commentaire-panel.css",
                 "~/Areas/Budget/Content/Avancement.css");
        }
    }
}

