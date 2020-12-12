(function () {
    'use strict';

    var copierBudgetConfirmDialogComponent = {
        templateUrl: '/Areas/Budget/Scripts/BudgetDetail/components/copier-budget-confirm-dialog.component.html',
        bindings: {
            resources: '<'
        },
        controller: copierBudgetConfirmDialogController
    };
    angular.module('Fred').component('copierBudgetConfirmDialogComponent', copierBudgetConfirmDialogComponent);
    angular.module('Fred').controller('copierBudgetConfirmDialogController', copierBudgetConfirmDialogController);
    copierBudgetConfirmDialogController.$inject = ['$scope', 'BudgetService', 'ProgressBar', 'Notify'];

    function copierBudgetConfirmDialogController($scope, BudgetService, ProgressBar, Notify) {

        //////////////////////////////////////////////////////////////////
        // Membres publiques                                            //
        //////////////////////////////////////////////////////////////////
        var $ctrl = this;
        $ctrl.BudgetCible = null;
        $ctrl.OnValidate = null;
        $ctrl.BudgetSource = {
            CI: null,
            Revision: null
        };
        $ctrl.Bibliotheque = {
            CI: null
        };

        $ctrl.View = {
            InitialiserComposantes: null,
            CopierMode: null,
            InclureRessourceSpecifiques: true
        };

        $ctrl.Identifiers = {
            Dialog: "BUDGET_DETAIL_CONFIRM_COPIER_BUDGET_DIALOG_ID"
        };

        $ctrl.OnValiderButtonClick = OnValiderButtonClick;
        $ctrl.OnAnnulerButtonClick = OnAnnulerButtonClick;
        $ctrl.IsValiderButtonEnable = IsValiderButtonEnable;

        $ctrl.InitialiserComposantesEnum = {
            BudgetSource: '1',
            Vides: '2'
        };
        $ctrl.CopierModeEnum = {
            LignesVides: '1',
            Integrale: '2'
        };


        //////////////////////////////////////////////////////////////////
        // Initialisation                                               //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {
            $scope.$on(BudgetService.Events.DisplayDialogToConfirmCopyBudget, function (event, arg) { Show(arg); });
        };


        //////////////////////////////////////////////////////////////////
        // Evènements externes                                          //
        //////////////////////////////////////////////////////////////////
        function Show(arg) {
            $ctrl.View.InitialiserComposantes = null;
            $ctrl.View.CopierMode = null;
            $ctrl.View.InclureRessourceSpecifiques = true;
            $ctrl.BudgetCible = arg.BudgetCible;
            $ctrl.BudgetSource.CI = arg.BudgetSource.CI;
            $ctrl.BudgetSource.Revision = arg.BudgetSource.Revision;
            $ctrl.Bibliotheque = arg.Bibliotheque;
            $ctrl.Bibliotheque.Exists = false;
            $ctrl.OnValidate = arg.OnValidate;
            $ctrl.OnCancel = arg.OnCancel;

            if ($ctrl.Bibliotheque.CI !== null) {
                // Récupère l'organisation du CI de la bibliothèque
                // La méthode de récupération dépend s'il s'agit du CI original chargé depuis le détail budget ou s'il
                //  provient du lookup
                $ctrl.Bibliotheque.OrganisationId = $ctrl.Bibliotheque.CI.Organisation
                    ? $ctrl.Bibliotheque.CI.Organisation.OrganisationId
                    : $ctrl.Bibliotheque.CI.OrganisationId;

                // Vérifie que le CI indiqué pour la bibliothèque des prix possède bien une bibliothèque des prix et
                //  de même devise que le budget cible
                $ctrl.Busy = true;
                ProgressBar.start();
                BudgetService.BibliothequePrixExists($ctrl.Bibliotheque.OrganisationId, $ctrl.BudgetCible.DeviseId)
                    .then(function (response) {
                        $ctrl.Bibliotheque.Exists = response.data;
                        HttpActionFinally();
                        ShowModal();
                    })
                    .catch(function () {
                        HttpActionFinally();
                        Notify.error($ctrl.resources.Global_Notification_Error);
                    });
            }
            else {
                ShowModal();
            }
        }


        //////////////////////////////////////////////////////////////////
        // Actions                                                      //
        //////////////////////////////////////////////////////////////////

        // Appelé lorsque l'utilisateur clique sur le bouton Valider
        function OnValiderButtonClick() {
            HideModal();
            if ($ctrl.OnValidate) {
                $ctrl.OnValidate({
                    ComposantesDuBudgetSource: $ctrl.View.InitialiserComposantes === null
                        ? null
                        : $ctrl.View.InitialiserComposantes === $ctrl.InitialiserComposantesEnum.BudgetSource,
                    OnlyLignesVides: $ctrl.View.CopierMode === $ctrl.CopierModeEnum.LignesVides,
                    IncludeRessourceSpecifiques: $ctrl.BudgetSource.CI.CiId !== $ctrl.BudgetCible.CI.CiId ? $ctrl.View.InclureRessourceSpecifiques : false
                });
            }
        }

        // Appelé lorsque l'utilisateur clique sur un bouton pour annuler
        function OnAnnulerButtonClick() {
            HideModal();
            if ($ctrl.OnCancel) {
                $ctrl.OnCancel();
            }
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

        // Indique si le bouton Valider est actif ou pas
        // Ne tient pas compte de $ctrl.Busy
        function IsValiderButtonEnable() {
            // Si le CI de la bibliotèque n'est pas indiqué ou si elle n'existe pas, l'utilisateur doit sélectionner
            //  comment initialiser les composantes
            if (($ctrl.Bibliotheque.CI === null || !$ctrl.Bibliotheque.Exists) && $ctrl.View.InitialiserComposantes === null) {
                return false;
            }
            // Le mode de copy est obligatoire
            if ($ctrl.View.CopierMode === null) {
                return false;
            }
            return true;
        }

        function HttpActionFinally() {
            ProgressBar.complete();
            $ctrl.Busy = false;
        }
    }
})();
