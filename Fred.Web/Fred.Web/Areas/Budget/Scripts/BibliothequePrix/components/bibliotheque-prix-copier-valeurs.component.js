(function () {
    'use strict';

    var bibliothequePrixCopierValeursComponent = {
        templateUrl: '/Areas/Budget/Scripts/BibliothequePrix/components/bibliotheque-prix-copier-valeurs.component.html',
        bindings: {},
        controller: bibliothequePrixCopierValeursController
    };
    angular.module('Fred').component('bibliothequePrixCopierValeursComponent', bibliothequePrixCopierValeursComponent);
    angular.module('Fred').controller('bibliothequePrixCopierValeursController', bibliothequePrixCopierValeursController);
    bibliothequePrixCopierValeursController.$inject = ['$scope', 'BibliothequePrixService', 'BudgetService', 'ProgressBar', 'Notify'];

    function bibliothequePrixCopierValeursController($scope, BibliothequePrixService, BudgetService, ProgressBar, Notify) {

        //////////////////////////////////////////////////////////////////
        // Membres                                                      //
        //////////////////////////////////////////////////////////////////
        var $ctrl = this;
        $ctrl.ActionEnum = BibliothequePrixService.CopierValeursActionEnum;
        $ctrl.resources = resources;
        $ctrl.Busy = false;
        $ctrl.Action = null;
        $ctrl.OnValidate = null;
        $ctrl.Identifiers = {
            Dialog: "BIBLIOTHEQUE_PRIX_COPIER_VALEURS_DIALOG_ID"
        };
        $ctrl.OnValidateButtonClick = OnValidateButtonClick;
        $ctrl.GetValidateButtonEnabled = GetValidateButtonEnabled;
        $ctrl.CI = null;


        //////////////////////////////////////////////////////////////////
        // Initialisation                                               //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {
            $scope.$on(BudgetService.Events.BibliothequePrixDisplayCopierValeursDialog, function (event, arg) { Show(arg); });
        };


        //////////////////////////////////////////////////////////////////
        // Evènements externes                                          //
        //////////////////////////////////////////////////////////////////
        function Show(arg) {
            $ctrl.Action = null;
            $ctrl.CI = null;
            $ctrl.OrganisationId = arg.OrganisationId;
            $ctrl.DeviseId = arg.DeviseId;
            $ctrl.EtablissementOrganisationId = arg.EtablissementOrganisationId;
            $ctrl.OnValidate = arg.OnValidate;
            ShowDialog();
        }


        //////////////////////////////////////////////////////////////////
        // Evènements internes                                          //
        //////////////////////////////////////////////////////////////////
        function OnValidateButtonClick() {
            let organisationId;
            if ($ctrl.Action === BibliothequePrixService.CopierValeursActionEnum.CI) {
                if ($ctrl.CI.Organisation.OrganisationId === $ctrl.OrganisationId) {
                    Notify.error($ctrl.resources.Budget_BibliothequePrix_CopierDialog_Erreur_CiIdentique);
                    return;
                }
                organisationId = $ctrl.CI.Organisation.OrganisationId;

            }
            else if ($ctrl.Action === BibliothequePrixService.CopierValeursActionEnum.Etablissement) {
                organisationId = $ctrl.EtablissementOrganisationId;
            }
            else {
                Notify.error($ctrl.resources.Global_Notification_Error);
                return;
            }

            $ctrl.Busy = true;
            ProgressBar.start();
            BudgetService.GetBibliothequePrixForCopy(organisationId, $ctrl.DeviseId)
                .then(GetBibliothequePrixForCopyThen)
                .catch(GetBibliothequePrixForCopyCacth);
        }
        function GetBibliothequePrixForCopyThen(result) {
            var data = result.data;

            if (data.Erreur) {
                Notify.error(data.Erreur);
            }
            else if ($ctrl.OnValidate) {
                $ctrl.OnValidate(data);
                HideDialog();
            }

            GetBibliothequePrixForCopyFinally();
        }
        function GetBibliothequePrixForCopyCacth() {
            Notify.error($ctrl.resources.Global_Notification_Error);
            GetBibliothequePrixForCopyFinally();
        }
        function GetBibliothequePrixForCopyFinally() {
            $ctrl.Busy = false;
            ProgressBar.complete();
        }


        //////////////////////////////////////////////////////////////////
        // Autre                                                        //
        //////////////////////////////////////////////////////////////////

        function GetValidateButtonEnabled() {
            if ($ctrl.Action === null) {
                return false;
            }
            else if ($ctrl.Action === BibliothequePrixService.CopierValeursActionEnum.Etablissement) {
                return true;
            }
            else if ($ctrl.Action === BibliothequePrixService.CopierValeursActionEnum.CI && $ctrl.CI !== null) {
                return true;
            }
            return false;
        }

        function ShowDialog() {
            $('#' + $ctrl.Identifiers.Dialog).modal('show');
        }
        function HideDialog() {
            $('#' + $ctrl.Identifiers.Dialog).modal('hide');
        }
    }
})();
