(function () {
    'use strict';

    var budgetValidationDialogComponent = {
        templateUrl: '/Areas/Budget/Scripts/BudgetDetail/components/budget-validation-dialog.component.html',
        bindings: {
            ciId: '<',
            budget: '<',
            resources: '<',
            onValidate: '&'
        },
        controller: budgetValidationDialogController
    };
    angular.module('Fred').component('budgetValidationDialogComponent', budgetValidationDialogComponent);
    angular.module('Fred').controller('budgetValidationDialogController', budgetValidationDialogController);
    budgetValidationDialogController.$inject = ['$scope', 'BudgetService', 'ProgressBar', 'Notify'];

    function budgetValidationDialogController($scope, BudgetService, ProgressBar, Notify) {

        //////////////////////////////////////////////////////////////////
        // Membres publiques                                            //
        //////////////////////////////////////////////////////////////////
        var $ctrl = this;
        $ctrl.RetourBrouillon = false;
        $ctrl.ConfirmValidation = false;
        $ctrl.Commentaire = "";
        $ctrl.LibelleCommentaire = "";
        $ctrl.LibelleAvertissement = "";
        $ctrl.CodeBudgetEtat = BudgetService.CodeBudgetEtat;

        //////////////////////////////////////////////////////////////////
        // Fonctions publiques                                          //
        //////////////////////////////////////////////////////////////////
        $ctrl.onClickButtonValidateDetail = onClickButtonValidateDetail;
        $ctrl.IsNotCheckToValidate = IsNotCheckToValidate;

        //////////////////////////////////////////////////////////////////
        // Initialisation                                               //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {
            $scope.$on(BudgetService.Events.DisplayValidationDialog, function (event, arg) { ShowValidate(arg); });
            $scope.$on(BudgetService.Events.DisplayReturnDraftDialog, function (event, arg) { ShowReturnDraft(arg); });
        };

        //////////////////////////////////////////////////////////////////
        // Evènements                                                   //
        //////////////////////////////////////////////////////////////////
        function ShowValidate(arg) {
            $ctrl.ConfirmValidation = false;
            $ctrl.RetourBrouillon = false;
            if ($ctrl.budget.Etat.Code === $ctrl.CodeBudgetEtat.Brouillon) {
                $ctrl.LibelleCommentaire = $ctrl.resources.Budget_Validation_AValider;
                Show();
            }
            if ($ctrl.budget.Etat.Code === $ctrl.CodeBudgetEtat.AValider && !$ctrl.RetourBrouillon) {
                $ctrl.LibelleCommentaire = $ctrl.resources.Budget_Validation_EnApplication;
                BudgetService.GetMessageMiseEnApllication($ctrl.ciId, $ctrl.budget.Version)
                    .then(AffectLibelleAvertissement);
            }
        }

        function ShowReturnDraft(arg) {
            $ctrl.ConfirmValidation = false;
            $ctrl.RetourBrouillon = true;
            if ($ctrl.budget.Etat.Code === $ctrl.CodeBudgetEtat.AValider && $ctrl.RetourBrouillon) {
                $ctrl.LibelleCommentaire = $ctrl.resources.Budget_Validation_RetourBrouillon;
            }
            Show();
        }

        //////////////////////////////////////////////////////////////////
        // Validation du budget                                         //
        //////////////////////////////////////////////////////////////////
        function onClickButtonValidateDetail() {
            Hide();

            if ($ctrl.RetourBrouillon) {
                ProgressBar.start();
                BudgetService.RetourBrouillon($ctrl.budget.BudgetId, $ctrl.Commentaire)
                    .then(ValidateThen)
                    .catch(ValidateCatch)
                    .finally(ValidationFinally);
            }
            else {
                BudgetService.ValidateDetail($ctrl.budget.BudgetId, $ctrl.Commentaire)
                    .then(ValidateThen)
                    .catch(ValidateCatch)
                    .finally(ValidationFinally);
            }
        }

        function ValidateThen(result, tachesChanged) {
            var validationErreurs = result.data;
            $ctrl.onValidate({ validationErreurs: validationErreurs});
            if (validationErreurs.length > 0) {
                Notify.error("Erreur de validation. Veuillez consulter la liste de erreurs.");
            }
            else {
                if ($ctrl.RetourBrouillon) {
                    Notify.message($ctrl.resources._Budget_RetourBrouillon_Effectue);
                }
                else {
                    Notify.message($ctrl.resources._Budget_Validation_Effectue);
                }
                RefreshEtatBudget();
            }
            ProgressBar.complete();
        }

        function ValidateCatch() {
            ProgressBar.complete();
            Notify.error($ctrl.resources._Budget_Erreur_Enregistrement);
        }

        function ValidationFinally() {
            ProgressBar.complete();
        }

        //////////////////////////////////////////////////////////////////
        // Autre                                                        //
        //////////////////////////////////////////////////////////////////
        function AffectLibelleAvertissement(result) {
            if (result) {
                $ctrl.LibelleAvertissement = result.data;
                Show();
            }
        }

        function IsNotCheckToValidate() {
            if ($ctrl.budget.Etat.Code === $ctrl.CodeBudgetEtat.AValider && !$ctrl.RetourBrouillon) {
                return !($ctrl.ConfirmValidation && $ctrl.Commentaire.length > 0);
            }
            else {
                return $ctrl.Commentaire.length === 0;
            }
        }

        function RefreshEtatBudget() {
            $scope.$emit(BudgetService.Events.LoadDetail, { budgetId: $ctrl.budget.BudgetId });
        }

        function Show() {
            $('#BUDGET_VALIDATION_DIALOG_ID').modal();
        }

        function Hide() {
            $('#BUDGET_VALIDATION_DIALOG_ID').modal('hide');
            $('.modal-backdrop').remove();
        }
    }
})();
