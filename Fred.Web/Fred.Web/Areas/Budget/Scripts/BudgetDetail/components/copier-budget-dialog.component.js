(function () {
    'use strict';

    var copierBudgetDialogComponent = {
        templateUrl: '/Areas/Budget/Scripts/BudgetDetail/components/copier-budget-dialog.component.html',
        bindings: {
            resources: '<'
        },
        controller: copierBudgetDialogController
    };
    angular.module('Fred').component('copierBudgetDialogComponent', copierBudgetDialogComponent);
    angular.module('Fred').controller('copierBudgetDialogController', copierBudgetDialogController);
    copierBudgetDialogController.$inject = ['$scope', 'BudgetService', 'ProgressBar', 'Notify'];

    function copierBudgetDialogController($scope, BudgetService, ProgressBar, Notify) {

        //////////////////////////////////////////////////////////////////
        // Membres publiques                                            //
        //////////////////////////////////////////////////////////////////
        var $ctrl = this;
        $ctrl.Current = {
            Budget: null,
            Revision: null
        };
        $ctrl.View = {
            BudgetSource: {
                CI: null,
                Revision: null,
                Revisions: null
            },
            Bibliotheque: {
                CI: null
            }
        };
        $ctrl.OnValidate = null;
        $ctrl.Identifiers = {
            Dialog: "BUDGET_DETAIL_COPIER_BUDGET_DIALOG_ID"
        };

        $ctrl.OnBudgetSourceCiChanged = OnBudgetSourceCiChanged;
        $ctrl.OnValiderButtonClick = OnValiderButtonClick;


        //////////////////////////////////////////////////////////////////
        // Initialisation                                               //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {
            $scope.$on(BudgetService.Events.DisplayDialogToCopyBudget, function (event, arg) { Show(arg); });
        };


        //////////////////////////////////////////////////////////////////
        // Evènements externes                                          //
        //////////////////////////////////////////////////////////////////
        function Show(arg) {
            $ctrl.OnValidate = arg.OnValidate;
            $ctrl.Current.Budget = arg.Budget;
            $ctrl.View.BudgetSource.CI = angular.copy(arg.Budget.CI);
            $ctrl.View.BudgetSource.CI.CodeLibelle = arg.Budget.CI.Code + " - " + arg.Budget.CI.Libelle;
            $ctrl.View.Bibliotheque.CI = $ctrl.View.BudgetSource.CI;
            $ctrl.View.Bibliotheque.CI.CodeLibelle = $ctrl.View.BudgetSource.CI.CodeLibelle;
            $ctrl.Current.Revision = {
                BudgetId: arg.Budget.BudgetId,
                Revision: arg.Budget.Version
            };
            LoadRevisions();
            ShowModal();
        }


        //////////////////////////////////////////////////////////////////
        // Actions                                                      //
        //////////////////////////////////////////////////////////////////

        // Appelé lorsque l'utilisateur change le CI du budget source
        function OnBudgetSourceCiChanged() {
            // Charge les révisions de budget du CI concerné
            LoadRevisions();
        }

        // Appelé lorsque l'utilisateur clique sur le bouton Valider
        function OnValiderButtonClick() {
            if ($ctrl.Current.Budget.CI.CiId !== $ctrl.View.BudgetSource.CI.CiId) {
                $ctrl.Busy = true;
                ProgressBar.start();
                BudgetService.CheckPlanDeTacheIdentiques($ctrl.Current.Budget.CI.CiId, $ctrl.View.BudgetSource.CI.CiId)
                    .then(function (response) {
                        var identique = response.data;
                        if (!identique) {
                            ValiderFinally($ctrl.resources.CopierBudget_Erreur_PlanDeTacheDifferent);
                        }
                        else {
                            Validate();
                            ValiderFinally();
                        }
                    })
                    .catch(function () {
                        ValiderFinally($ctrl.resources.Global_Notification_Error);
                    });
            }
            else {
                Validate();
            }
        }
        function ValiderFinally(error) {
            $ctrl.Busy = false;
            ProgressBar.complete();
            if (error) {
                Notify.error(error);
            }
        }


        //////////////////////////////////////////////////////////////////
        // Chargement des révisions                                     //
        //////////////////////////////////////////////////////////////////
        function LoadRevisions() {
            $ctrl.View.BudgetSource.Revision = null;
            $ctrl.View.BudgetSource.Revisions = null;
            $ctrl.Busy = true;
            ProgressBar.start();
            BudgetService.GetBudgetRevisions($ctrl.View.BudgetSource.CI.CiId)
                .then(GetBudgetRevisionsThen)
                .catch(GetBudgetRevisionsCatch);
        }
        function GetBudgetRevisionsThen(result) {
            // Charge les révisions pour les faire afficher dans la liste déroulante
            var revisions = [];
            for (var i = 0; i < result.data.length; i++) {
                var revision = result.data[i];

                // ng-options fonctionne par référence, si un budgetId dans la liste est celui de
                //  la révision courante, on doit ajouter $ctrl.Current.Revision dans la liste et
                //  surtout pas result.data[i]
                if (revision.BudgetId === $ctrl.Current.Revision.BudgetId) {
                    revision = $ctrl.Current.Revision;
                    $ctrl.View.BudgetSource.Revision = revision;
                }
                revisions.push(revision);
            }

            // Si une seule révision est présente on la charge directement
            if (revisions.length === 1) {
                $ctrl.View.BudgetSource.Revision = revisions[0];
            }

            $ctrl.View.BudgetSource.Revisions = revisions;
            ProgressBar.complete();
            $ctrl.Busy = false;
        }
        function GetBudgetRevisionsCatch() {
            Notify.error($ctrl.resources.Global_Notification_Chargement_Error);
            ProgressBar.complete();
            $ctrl.Busy = false;
        }


        //////////////////////////////////////////////////////////////////
        // Autre                                                        //
        //////////////////////////////////////////////////////////////////

        // Affiche la modale
        function ShowModal() {
            $('#' + $ctrl.Identifiers.Dialog).modal({
                backdrop: 'static',
                keyboard: false
            });
        }

        // Cache la modale
        function HideModal() {
            // https://www.thephani.com/screen-grayed-out-on-bootstrap-modal-close/
            $('#' + $ctrl.Identifiers.Dialog).modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
        }

        function Validate() {
            HideModal();
            if ($ctrl.OnValidate) {
                $ctrl.OnValidate({
                    BudgetSource: {
                        CI: $ctrl.View.BudgetSource.CI,
                        Revision: $ctrl.View.BudgetSource.Revision
                    },
                    Bibliotheque: {
                        CI: $ctrl.View.Bibliotheque.CI
                    },
                    Show: ShowModal
                });
            }
        }
    }
})();
