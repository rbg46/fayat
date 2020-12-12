(function () {
    'use strict';

    var copierDeplacerT4ConfirmationDialogComponent = {
        templateUrl: '/Areas/Budget/Scripts/BudgetDetail/components/copier-deplacer-T4-confirmation-dialog.component.html',
        bindings: {
            resources: '<'
        },
        controller: copierDeplacerT4ConfirmationDialogController
    };
    angular.module('Fred').component('copierDeplacerT4ConfirmationDialogComponent', copierDeplacerT4ConfirmationDialogComponent);
    angular.module('Fred').controller('copierDeplacerT4ConfirmationDialogController', copierDeplacerT4ConfirmationDialogController);
    copierDeplacerT4ConfirmationDialogController.$inject = ['$scope', 'BudgetService', 'BudgetDetailService', 'ProgressBar', 'Notify'];

    function copierDeplacerT4ConfirmationDialogController($scope, BudgetService, BudgetDetailService, ProgressBar, Notify) {

        //////////////////////////////////////////////////////////////////
        // Membres publiques                                            //
        //////////////////////////////////////////////////////////////////
        var $ctrl = this;
        $ctrl.Action = null;
        $ctrl.Budget = null;
        $ctrl.CI = null;
        $ctrl.OnConfirm = null;
        $ctrl.NePlusDemanderPourCopier = false;
        $ctrl.NePlusDemanderPourDeplacer = false;
        $ctrl.NePlusDemanderPourRemplacer = false;
        $ctrl.CopierMode = null;
        $ctrl.CopierT4Mode = BudgetDetailService.CopierT4Mode;
        $ctrl.ActionEnum = BudgetDetailService.CopierDeplacerT4ActionEnum;

        $ctrl.OnConfirmButtonCLick = OnConfirmButtonCLick;
        $ctrl.GetConfirmButtonEnabled = GetConfirmButtonEnabled;


        //////////////////////////////////////////////////////////////////
        // Initialisation                                               //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {
            $scope.$on(BudgetService.Events.DisplayDialogToConfirmCopyMoveT4, function (event, arg) { Load(arg); });
        };


        //////////////////////////////////////////////////////////////////
        // Evènements externes                                          //
        //////////////////////////////////////////////////////////////////

        // Charge la modale et l'affiche
        // - arg : voir BudgetService.Events.DisplayDialogToConfirmCopyMoveT4
        function Load(arg) {
            $ctrl.Busy = false;
            $ctrl.OnConfirm = arg.OnConfirm;
            $ctrl.Action = arg.Action;
            $ctrl.Budget = arg.Budget;
            $ctrl.Tache = arg.Tache;
            $ctrl.NePlusDemander = false;

            switch (arg.Action) {
                case BudgetDetailService.CopierDeplacerT4ActionEnum.Copier:
                    $ctrl.Taches4 = [];
                    for (let tache4 of arg.Taches4) {
                        $ctrl.Taches4.push({
                            Code: tache4.Code,
                            Libelle: tache4.Libelle,
                            Erreur: null,
                            TacheId: 0
                        });
                    }
                    $ctrl.Titre = $ctrl.resources.CopierDeplacerT4Confirmation_Title_Copier;
                    $ctrl.Message = $ctrl.resources.CopierDeplacerT4Confirmation_Message_Copier;
                    $ctrl.NePlusDemander = $ctrl.NePlusDemanderPourCopier;
                    if (!$ctrl.NePlusDemanderPourCopier) {
                        $ctrl.CopierMode = null;
                    }

                    $ctrl.Busy = true;
                    ProgressBar.start();
                    BudgetService.GetNextTacheCodes(arg.Tache.TacheId, arg.Taches4.length)
                        .then(GetNextTacheCodeThen)
                        .catch(GetNextTacheCodeCatch);
                    break;

                case BudgetDetailService.CopierDeplacerT4ActionEnum.Deplacer:
                    $ctrl.Titre = $ctrl.resources.CopierDeplacerT4Confirmation_Title_Deplacer;
                    $ctrl.Message = $ctrl.resources.CopierDeplacerT4Confirmation_Message_Deplacer;
                    if ($ctrl.NePlusDemanderPourDeplacer) {
                        Confirmation();
                        return;
                    }
                    else {
                        ShowModal();
                    }
                    break;

                case BudgetDetailService.CopierDeplacerT4ActionEnum.Remplacer:
                    $ctrl.Titre = $ctrl.resources.CopierDeplacerT4Confirmation_Title_Remplacer;
                    $ctrl.Message = $ctrl.resources.CopierDeplacerT4Confirmation_Message_Remplacer;
                    if ($ctrl.NePlusDemanderPourRemplacer) {
                        Confirmation();
                        return;
                    }
                    else {
                        ShowModal();
                    }
                    break;

                default:
                    return;
            }
        }
        function GetNextTacheCodeThen(result) {
            for (var i = 0; i < result.data.length; i++) {
                var tache4 = $ctrl.Taches4[i];
                tache4.Code = result.data[i];
                tache4.Erreur = null;
            }

            // Affiche la modale
            ShowModal();

            ProgressBar.complete();
            $ctrl.Busy = false;
        }
        function GetNextTacheCodeCatch() {
            ProgressBar.complete();
            $ctrl.Busy = false;
            Notify.error($ctrl.resources.Global_Notification_Error);
            HideModal();
        }


        //////////////////////////////////////////////////////////////////
        // Evènements internes                                          //
        //////////////////////////////////////////////////////////////////
        function OnConfirmButtonCLick() {
            switch ($ctrl.Action) {
                case BudgetDetailService.CopierDeplacerT4ActionEnum.Copier:
                    CreateTaches4();
                    break;

                case BudgetDetailService.CopierDeplacerT4ActionEnum.Deplacer:
                    $ctrl.NePlusDemanderPourDeplacer = $ctrl.NePlusDemander;
                    Confirmation();
                    HideModal();
                    break;

                case BudgetDetailService.CopierDeplacerT4ActionEnum.Remplacer:
                    $ctrl.NePlusDemanderPourRemplacer = $ctrl.NePlusDemander;
                    Confirmation();
                    HideModal();
                    break;

                default:
                    return;
            }
        }


        //////////////////////////////////////////////////////////////////
        // Création de tâches 4                                         //
        //////////////////////////////////////////////////////////////////
        function CreateTaches4() {
            $ctrl.Busy = true;
            ProgressBar.start();
            BudgetService.CreateTaches4($ctrl.Budget.CI.CiId, $ctrl.Tache.TacheId, $ctrl.Taches4)
                .then(CreateTaches4Then)
                .catch(CreateTaches4Catch);
        }

        function CreateTaches4Then(result) {
            ProgressBar.complete();
            $ctrl.Busy = false;
            var data = result.data;
            if (data.Erreurs) {
                for (var i = 0; i < data.Erreurs.length; i++) {
                    $ctrl.Taches4[i].Erreur = data.Erreurs[i];
                }
                Notify.error($ctrl.resources.CopierDeplacerT4Confirmation_ImpossibleCreerTache);
            }
            else if (data.Taches4CreatedIds) {
                if (data.Taches4CreatedIds.length === $ctrl.Taches4.length) {
                    for (var j = 0; j < data.Taches4CreatedIds.length; j++) {
                        $ctrl.Taches4[j].TacheId = data.Taches4CreatedIds[j];
                    }
                    $ctrl.NePlusDemanderPourCopier = $ctrl.NePlusDemander;
                    HideModal();
                    Confirmation();
                }
                else {
                    CreateTaches4Catch();
                }
            }
        }
        function CreateTaches4Catch() {
            ProgressBar.complete();
            $ctrl.Busy = false;
            Notify.error($ctrl.resources.Global_Notification_Error);
            HideModal();
        }



        //////////////////////////////////////////////////////////////////
        // Autre                                                        //
        //////////////////////////////////////////////////////////////////

        // Affiche la modale
        function ShowModal() {
            $('#COPIER_DEPLACER_T4_CONFIRMATION_DIALOG_ID').modal('show');
        }

        // Cache la modale
        function HideModal() {
            $('#COPIER_DEPLACER_T4_CONFIRMATION_DIALOG_ID').modal('hide');
        }

        // Confirmation
        function Confirmation() {
            if ($ctrl.OnConfirm) {
                if ($ctrl.Action === BudgetDetailService.CopierDeplacerT4ActionEnum.Copier) {
                    $ctrl.OnConfirm({
                        CopierMode: $ctrl.Action === BudgetDetailService.CopierDeplacerT4ActionEnum.Copier ? $ctrl.CopierMode : null,
                        Taches4: $ctrl.Taches4
                    });
                }
                else {
                    $ctrl.OnConfirm();
                }
            }
        }

        // Indique si le bouton de confirmation est actif ou non
        function GetConfirmButtonEnabled() {
            return $ctrl.Action !== BudgetDetailService.CopierDeplacerT4ActionEnum.Copier || $ctrl.CopierMode !== null;
        }
    }
})();
