(function () {
    'use strict';

    var gestionT4DialogComponent = {
        templateUrl: '/Areas/Budget/Scripts/BudgetDetail/components/gestion-t4-dialog.component.html',
        bindings: {
            budget: '<',
            resources: '<'
        },
        controller: gestionT4DialogController
    };
    angular.module('Fred').component('gestionT4DialogComponent', gestionT4DialogComponent);
    angular.module('Fred').controller('gestionT4DialogController', gestionT4DialogController);
    gestionT4DialogController.$inject = ['$scope', 'BudgetService', 'ProgressBar', 'Notify'];

    function gestionT4DialogController($scope, BudgetService, ProgressBar, Notify) {

        //////////////////////////////////////////////////////////////////
        // Membres publiques                                           //
        //////////////////////////////////////////////////////////////////
        var $ctrl = this;
        $ctrl.Tache4 = null;
        $ctrl.Code = "";
        $ctrl.Libelle = "";
        $ctrl.Erreurs = [];
        $ctrl.Mode = {
            AddT4: "AddT4",
            ChangeT4: "ChangeT4",
            DeleteT4: "DeleteT4",
            DeleteBudgetT4: "DeleteBudgetT4"
        };
        $ctrl.CurrentMode = null;
        $ctrl.Busy = false;
        $ctrl.OnValidate = null;
        $ctrl.AjouterModeEnum = {
            Creer: "Créer",
            Existante: "Existante"
        };
        $ctrl.AjouterMode = null;
        $ctrl.Tache4Inutilisees = null;
        $ctrl.Tache4Inutilisee = null;


        //////////////////////////////////////////////////////////////////
        // Fonctions publiques                                          //
        //////////////////////////////////////////////////////////////////
        $ctrl.T4HasChanged = T4HasChanged;
        $ctrl.OnAddT4 = OnAddT4;
        $ctrl.OnChangeT4 = OnChangeT4;
        $ctrl.OnDeleteT4 = OnDeleteT4;
        $ctrl.OnDeleteBudgetT4 = OnDeleteBudgetT4;
        $ctrl.OnAjouterModeChanged = OnAjouterModeChanged;


        //////////////////////////////////////////////////////////////////
        // Initialisation                                               //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {
            $scope.$on(BudgetService.Events.DisplayDialogToAddT4, function (event, arg) { ShowAddT4(arg); });
            $scope.$on(BudgetService.Events.DisplayDialogToChangeT4, function (event, arg) { ShowChangeT4(arg); });
            $scope.$on(BudgetService.Events.DisplayDialogToDeleteT4, function (event, arg) { ShowDeleteT4(arg); });
            $scope.$on(BudgetService.Events.DisplayDialogToDeleteBudgetT4, function (event, arg) { ShowDeleteBudgetT4(arg); });
        };


        //////////////////////////////////////////////////////////////////
        // Evènements                                                   //
        //////////////////////////////////////////////////////////////////
        function ShowAddT4(arg) {
            if ($ctrl.Busy) {
                return;
            }
            $ctrl.Busy = true;

            $ctrl.AjouterMode = $ctrl.AjouterModeEnum.Creer;
            $ctrl.Tache4Inutilisees = null;
            $ctrl.Tache4Inutilisee = null;
            Initialise($ctrl.Mode.AddT4, null, arg.Tache3, "", "", $ctrl.resources.Budget_GestionT4_Titre_Ajouter, arg.OnValidate);

            ProgressBar.start();
            BudgetService.GetNextTacheCode($ctrl.Tache3.TacheId)
                .then(GetNextTaskCodeThen)
                .catch(GetNextTacheCodeCatch);
        }
        function GetNextTaskCodeThen(result) {
            $ctrl.Code = result.data;
            Show();
            $ctrl.Busy = false;
            ProgressBar.complete();
        }
        function GetNextTacheCodeCatch() {
            Notify.error($ctrl.resources.Global_Notification_Error);
            $ctrl.Busy = false;
            ProgressBar.complete();
        }

        function ShowChangeT4(arg) {
            if ($ctrl.Busy) {
                return;
            }
            $ctrl.OriginalTache4 = {
                Code: arg.Tache4.Code,
                Libelle: arg.Tache4.Libelle
            };
            Initialise($ctrl.Mode.ChangeT4, arg.Tache4, arg.Tache4.Tache3, arg.Tache4.Code, arg.Tache4.Libelle, $ctrl.resources.Budget_GestionT4_Titre_Changer, arg.OnValidate);
            Show();
        }

        function ShowDeleteT4(arg) {
            if ($ctrl.Busy) {
                return;
            }
            Initialise($ctrl.Mode.DeleteT4, arg.Tache4, arg.Tache4.Tache3, arg.Tache4.Code, arg.Tache4.Libelle, $ctrl.resources.Budget_GestionT4_Titre_SupprimerT4, arg.OnValidate);
            Show();
        }

        function ShowDeleteBudgetT4(arg) {
            if ($ctrl.Busy) {
                return;
            }
            Initialise($ctrl.Mode.DeleteBudgetT4, arg.Tache4, arg.Tache4.Tache3, arg.Tache4.Code, arg.Tache4.Libelle, $ctrl.resources.Budget_GestionT4_Titre_SupprimerBudgetT4, arg.OnValidate);
            Show();
        }


        //////////////////////////////////////////////////////////////////
        // Evènement : ajout d'une tâche                               //
        //////////////////////////////////////////////////////////////////
        function OnAddT4() {
            var erreurs = [];

            if ($ctrl.AjouterMode === $ctrl.AjouterModeEnum.Existante) {
                if ($ctrl.Tache4Inutilisee === null) {
                    erreurs.push($ctrl.resources.Budget_GestionT4_Erreur_TacheManquante);
                }
                else {
                    $ctrl.Tache4Inutilisee.IsActive = true;
                    Hide();
                    if ($ctrl.OnValidate) {
                        $ctrl.OnValidate($ctrl.Tache4Inutilisee);
                    }
                }
                CreateTache4Finally(erreurs);
            }
            else {
                if (CheckCodeAndLibelle(erreurs)) {
                    ProgressBar.start();
                    $ctrl.Busy = true;
                    BudgetService.CreateTache4($ctrl.budget.CI.CiId, $ctrl.Tache3.TacheId, $ctrl.Code, $ctrl.Libelle)
                        .then((result) => CreateTache4Then(result, erreurs))
                        .catch(() => CreateTache4Catch(erreurs));
                }
                else {
                    CreateTache4Finally(erreurs);
                }
            }
        }
        function CreateTache4Then(result, erreurs) {
            var ret = result.data;
            if (ret.Erreur) {
                erreurs.push(ret.Erreur);
            }
            else if (ret.Tache) {
                Hide();
                if ($ctrl.OnValidate) {
                    $ctrl.OnValidate(ret.Tache);
                }
            }
            CreateTache4Finally(erreurs);
        }
        function CreateTache4Catch(erreurs) {
            erreurs.push($ctrl.resources._Budget_Erreur_Enregistrement);
            CreateTache4Finally(erreurs);
        }
        function CreateTache4Finally(erreurs) {
            $ctrl.Busy = false;
            $ctrl.Erreurs = erreurs;
            ProgressBar.complete();
        }


        //////////////////////////////////////////////////////////////////
        // Evènement : modification d'une tâche                        //
        //////////////////////////////////////////////////////////////////
        function OnChangeT4() {
            var erreurs = [];
            if (CheckCodeAndLibelle(erreurs)) {
                ProgressBar.start();
                $ctrl.Busy = true;
                BudgetService.ChangeTache4($ctrl.budget.CI.CiId, $ctrl.Tache4.TacheId, $ctrl.Code, $ctrl.Libelle)
                    .then((result) => ChangeTache4Then(result, erreurs))
                    .catch(() => ChangeTache4Catch(erreurs));
            }
            else {
                ChangeTache4Finally(erreurs);
            }
        }
        function ChangeTache4Then(result, erreurs) {
            var ret = result.data;
            if (ret.Erreur) {
                erreurs.push(ret.Erreur);
            }
            else {
                Hide();
                if ($ctrl.OnValidate) {
                    $ctrl.OnValidate(ret.Tache.Code, ret.Tache.Libelle);
                }
            }
            ChangeTache4Finally(erreurs);
        }
        function ChangeTache4Catch(erreurs) {
            erreurs.push($ctrl.resources._Budget_Erreur_Enregistrement);
            ChangeTache4Finally(erreurs);
        }
        function ChangeTache4Finally(erreurs) {
            $ctrl.Busy = false;
            $ctrl.Erreurs = erreurs;
            ProgressBar.complete();
        }


        //////////////////////////////////////////////////////////////////
        // Evènement : suppression d'une tâche                          //
        //////////////////////////////////////////////////////////////////
        function OnDeleteT4() {
            var erreurs = [];
            $ctrl.Busy = true;
            ProgressBar.start();
            BudgetService.DeleteTache4($ctrl.budget.CI.CiId, $ctrl.Tache4.TacheId)
                .then((result) => DeleteTache4Then(result, erreurs))
                .catch(() => DeleteTache4Catch(erreurs));
        }
        function DeleteTache4Then(result, erreurs) {
            var ret = result.data;
            if (ret.Erreur) {
                erreurs.push(ret.Erreur);
            }
            else {
                Hide();
                if ($ctrl.OnValidate) {
                    $ctrl.OnValidate();
                }
            }
            DeleteTache4Finally(erreurs);
        }
        function DeleteTache4Catch(erreurs) {
            erreurs.push($ctrl.resources.Budget_GestionT4_Erreur_Suppression);
            DeleteTache4Finally(erreurs);
        }
        function DeleteTache4Finally(erreurs) {
            $ctrl.Busy = false;
            $ctrl.Erreurs = erreurs;
            ProgressBar.complete();
        }


        //////////////////////////////////////////////////////////////////
        // Evènement : suppression d'une ligne de détail                //
        //////////////////////////////////////////////////////////////////
        function OnDeleteBudgetT4() {
            Hide();
            if ($ctrl.OnValidate) {
                $ctrl.OnValidate();
            }
        }


        //////////////////////////////////////////////////////////////////
        // Evènement : mode d'ajout change                              //
        //////////////////////////////////////////////////////////////////
        function OnAjouterModeChanged() {
            $ctrl.Erreurs = [];
            if ($ctrl.AjouterMode !== $ctrl.AjouterModeEnum.Existante || $ctrl.Tache4Inutilisees) {
                return;
            }

            var model = {
                CiId: $ctrl.budget.CI.CiId,
                BudgetId: $ctrl.budget.BudgetId,
                Tache4Ids: []
            };
            for (let tache4 of $ctrl.budget.GetTaches4()) {
                if (!tache4.Deleted) {
                    model.Tache4Ids.push(tache4.TacheId);
                }
            }

            $ctrl.Busy = true;
            ProgressBar.start();
            BudgetService.GetTache4Inutilisees(model)
                .then(GetTache4InutiliseesThen)
                .catch(GetTache4InutiliseesCatch);
        }
        function GetTache4InutiliseesThen(result) {
            $ctrl.Tache4Inutilisees = result.data.Taches4;
            $ctrl.Busy = false;
            ProgressBar.complete();
        }
        function GetTache4InutiliseesCatch() {
            Notify.error($ctrl.resources.Global_Notification_Error);
            $ctrl.Busy = false;
            ProgressBar.complete();
        }


        //////////////////////////////////////////////////////////////////
        // Autre                                                        //
        //////////////////////////////////////////////////////////////////
        function Initialise(mode, tache4, tache3, code, libelle, titre, onValidate) {
            $ctrl.Erreurs = [];
            $ctrl.Titre = titre;
            $ctrl.CurrentMode = mode;
            $ctrl.Code = code;
            $ctrl.Libelle = libelle;
            $ctrl.Tache1 = tache3.Tache2.Tache1;
            $ctrl.Tache2 = tache3.Tache2;
            $ctrl.Tache3 = tache3;
            $ctrl.Tache4 = tache4;
            $ctrl.OnValidate = onValidate;
        }

        function T4HasChanged() {
            return $ctrl.OriginalTache4.Code !== $ctrl.Code || $ctrl.OriginalTache4.Libelle !== $ctrl.Libelle;
        }

        function CheckCodeAndLibelle(erreurs) {
            var ret = true;
            $ctrl.Code = $ctrl.Code.trim();
            $ctrl.Libelle = $ctrl.Libelle.trim();

            if ($ctrl.Code === '') {
                erreurs.push($ctrl.resources.Budget_GestionT4_Erreur_CodeManquant);
                ret = false;
            }
            if ($ctrl.Libelle === '') {
                erreurs.push($ctrl.resources.Budget_GestionT4_Erreur_LibelleManquant);
                ret = false;
            }

            return ret;
        }

        function Show() {
            var modal = $('#GESTION_T4_DIALOG_ID');
            modal.modal('show');

            // Met le focus sur l'input "code"
            // Note : autofocus (html) ne fonctionne pas pour cette modale sous Firefox
            // Ce code met bien le focus sous Chrome et Firefox
            modal.on('shown.bs.modal', function () {
                $('#txtCodeReferentiel').focus();
            });

            modal.on('hide.bs.modal', function () {
                $ctrl.Busy = false;
            });
        }
        function Hide() {
            $('#GESTION_T4_DIALOG_ID').modal('hide');
        }
    }
})();
