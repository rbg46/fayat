(function () {
    'use strict';

    var copierDeplacerT4DialogComponent = {
        templateUrl: '/Areas/Budget/Scripts/BudgetDetail/components/copier-deplacer-T4-dialog.component.html',
        bindings: {
            resources: '<'
        },
        controller: copierDeplacerT4DialogController
    };
    angular.module('Fred').component('copierDeplacerT4DialogComponent', copierDeplacerT4DialogComponent);
    angular.module('Fred').controller('copierDeplacerT4DialogController', copierDeplacerT4DialogController);
    copierDeplacerT4DialogController.$inject = ['$scope', 'BudgetService', 'BudgetDetailService', 'ProgressBar', 'Notify'];

    function copierDeplacerT4DialogController($scope, BudgetService, BudgetDetailService, ProgressBar, Notify) {

        //////////////////////////////////////////////////////////////////
        // Membres publiques                                            //
        //////////////////////////////////////////////////////////////////
        var $ctrl = this;
        $ctrl.ActionEnum = BudgetDetailService.CopierDeplacerT4ActionEnum;
        $ctrl.OnAction = null;
        var allTache3 = {
            Tache3: null,
            Text: $ctrl.resources.CopierDeplacerT4_ToutesLesTaches3
        };

        $ctrl.TargetEnum = {
            Tache3: 1,
            Tache4: 2
        };

        $ctrl.OnActionChanged = OnActionChanged;
        $ctrl.OnCiChanged = OnCiChanged;
        $ctrl.OnRevisionChanged = OnRevisionChanged;
        $ctrl.OnTache3Changed = OnTache3Changed;
        $ctrl.OnSelectTache4 = OnSelectTache4;
        $ctrl.OnActionButtonCLick = OnActionButtonCLick;
        $ctrl.GetActionButtonText = GetActionButtonText;
        $ctrl.GetActionButtonEnabled = GetActionButtonEnabled;


        //////////////////////////////////////////////////////////////////
        // Initialisation                                               //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {
            $scope.$on(BudgetService.Events.DisplayDialogToCopyMoveT4, function (event, arg) { Load(arg); });
        };


        //////////////////////////////////////////////////////////////////
        // Evènements externes                                          //
        //////////////////////////////////////////////////////////////////

        // Charge la modale et l'affiche
        // - arg : voir BudgetService.Events.DisplayDialogToCopyMoveT4
        function Load(arg) {
            if (arg.Tache.Niveau === 3) {
                $ctrl.Titre = $ctrl.resources.CopierDeplacerT4_Title_T3;
                $ctrl.Target = $ctrl.TargetEnum.Tache3;
                $ctrl.Tache4 = null;
                $ctrl.Tache3 = arg.Tache;
                $ctrl.Action = null;
                $ctrl.MultiSelectTache4 = true;
            }
            else if (arg.Tache.Niveau === 4) {
                $ctrl.Titre = $ctrl.resources.CopierDeplacerT4_Title_T4;
                $ctrl.Target = $ctrl.TargetEnum.Tache4;
                $ctrl.Tache4 = arg.Tache;
                $ctrl.Tache3 = $ctrl.Tache4.Tache3;
                $ctrl.Action = $ctrl.ActionEnum.Remplacer;
                $ctrl.MultiSelectTache4 = false;
            }
            else {
                // Ne concerne que les tâches 3 et 4
                return;
            }

            $ctrl.OnAction = arg.OnAction;
            $ctrl.Busy = false;
            $ctrl.Tache2 = $ctrl.Tache3.Tache2;
            $ctrl.Tache1 = $ctrl.Tache2.Tache1;

            // Eléments courant
            $ctrl.Current = {
                Budget: arg.Budget,
                CI: arg.Budget.CI,
                Revision: {
                    BudgetId: arg.Budget.BudgetId,
                    Revision: arg.Budget.Version
                },
                Revisions: []
            };
            $ctrl.Current.CI.CodeLibelle = arg.Budget.CI.Code + " - " + arg.Budget.CI.Libelle;

            // Eléments de la vue
            $ctrl.View = {
                Budget: $ctrl.Current.Budget,
                CI: $ctrl.Current.CI,
                Revision: null,
                Revisions: [],
                Tache3: null,
                Taches3: [],
                Taches4: []
            };
            $ctrl.View.CI.CodeLibelle = $ctrl.Current.CI.CodeLibelle;

            // Pour une tache 4 cible, affiche directement la liste des taches disponibles
            if (arg.Tache.Niveau === 4) {
                $ctrl.View.Revision = $ctrl.Current.Revision;
                OnRevisionChanged();
            }

            // Affiche la modale
            ShowModal();
        }

        //////////////////////////////////////////////////////////////////
        // Actions                                                      //
        //////////////////////////////////////////////////////////////////

        // Appelé lorsque l'utilisateur change l'action (copier / déplacer)
        function OnActionChanged() {
            // Remet les valeurs courantes
            $ctrl.View.CI = $ctrl.Current.CI;
            $ctrl.View.Revision = $ctrl.Current.Revision;

            // Les révisions courantes ne sont chargées qu'en cas de copie
            if ($ctrl.Action === $ctrl.ActionEnum.Copier && $ctrl.Current.Revisions.length === 0) {
                LoadRevisions();
            }
            else {
                $ctrl.View.Revisions = $ctrl.Current.Revisions;
                OnRevisionChanged();
            }
        }

        // Appelé lorsque l'utilisateur change le CI
        function OnCiChanged() {
            $ctrl.View.Revision = null;
            $ctrl.View.Tache3 = null;
            $ctrl.View.Taches3 = [];
            LoadRevisions();
        }

        // Appelé lorsque l'utilisateur change la révision
        function OnRevisionChanged() {
            LoadTaches4();
        }

        // Appelé lorsque l'utilisateur change la tâche 3
        function OnTache3Changed() {
            UpdateTaches4FromTaches3();
        }

        // Appelé lorsque l'utilisateur sélectionne une tâche 4 dans la liste
        function OnSelectTache4(tache4) {
            if ($ctrl.Busy) {
                return;
            }
            if ($ctrl.MultiSelectTache4) {
                tache4.Selected = !tache4.Selected;
            }
            else {
                angular.forEach($ctrl.View.Taches4, function (t4) { t4.Selected = false; });
                tache4.Selected = true;
            }
        }

        // Appelé lorsque l'utilisateur clique sur le bouton d'action
        function OnActionButtonCLick() {
            if ($ctrl.OnAction) {
                var taches4 = [];
                for (let tache4 of $ctrl.View.Taches4) {
                    if (tache4.Selected) {
                        taches4.push(tache4.Tache4);
                    }
                }

                $ctrl.OnAction({
                    Action: $ctrl.Action,
                    Budget: $ctrl.View.Budget,
                    Taches4: taches4
                });
            }
            HideModal();
        }


        //////////////////////////////////////////////////////////////////
        // Chargement des révisions                                     //
        //////////////////////////////////////////////////////////////////
        function LoadRevisions() {
            $ctrl.Busy = true;
            ProgressBar.start();
            BudgetService.GetBudgetRevisions($ctrl.View.CI.CiId)
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
                }
                revisions.push(revision);
            }

            // Si une seule révision est présente on la charge directement
            if (revisions.length === 1) {
                $ctrl.View.Revision = revisions[0];
            }

            $ctrl.View.Revisions = revisions;
            if ($ctrl.Current.Revisions.length === 0 && $ctrl.Current.Revision === $ctrl.View.Revision) {
                $ctrl.Current.Revisions = revisions;
            }

            OnRevisionChanged();
            ProgressBar.complete();
            $ctrl.Busy = false;
        }
        function GetBudgetRevisionsCatch() {
            Notify.error($ctrl.resources.Global_Notification_Chargement_Error);
            ProgressBar.complete();
            $ctrl.Busy = false;
        }


        //////////////////////////////////////////////////////////////////
        // Chargement des tâches 4                                      //
        //////////////////////////////////////////////////////////////////
        function LoadTaches4() {
            if ($ctrl.View.Revision === $ctrl.Current.Revision) {
                // Charge les tâches 4 du détail budget courant
                UpdateFromBudget($ctrl.Current.Budget);
            }
            else if ($ctrl.View.Revision !== null) {
                // Charge les tâches 4 d'un autre détail budget
                $ctrl.Busy = true;
                ProgressBar.start();
                $ctrl.View.Tache3 = null;

                // Cette fonction charge le budget, mais dans le context de ce formulaire, il n'est
                // pas nécessaire de tout charger comme ça...
                BudgetService.GetDetail($ctrl.View.Revision.BudgetId)
                    .then(GetDetailThen)
                    .catch(GetDetailCatch);
            }
            else {
                // Vide la liste des tâches 3 et 4
                $ctrl.View.Tache3 = null;
                $ctrl.View.Taches3 = [];
                $ctrl.View.Taches4 = [];
            }
        }
        function GetDetailThen(result) {
            var budget = result.data;
            BudgetDetailService.InitialiseTaches(budget, true); // Initialise les tâches, requis pour avoir les parents
            UpdateFromBudget(budget);
            ProgressBar.complete();
            $ctrl.Busy = false;
        }
        function GetDetailCatch() {
            Notify.error($ctrl.resources._Budget_Erreur_Chargement);
            ProgressBar.complete();
            $ctrl.Busy = false;
        }


        //////////////////////////////////////////////////////////////////
        // Autre                                                        //
        //////////////////////////////////////////////////////////////////

        // Affiche la modale
        function ShowModal() {
            $('#COPIER_DEPLACER_T4_DIALOG_ID').modal('show');
        }

        // Cache la modale
        function HideModal() {
            $('#COPIER_DEPLACER_T4_DIALOG_ID').modal('hide');
        }

        // Met à jour l'écran en fonction d'un budget
        // - budget : le budget concerné
        function UpdateFromBudget(budget) {
            $ctrl.View.Budget = budget;
            UpdateTaches3();
            UpdateTaches4FromTaches3();
        }

        // Met à jour les tâches 3
        function UpdateTaches3() {
            var taches3 = [];
            taches3.push(allTache3);

            if ($ctrl.View.Budget && $ctrl.View.Budget.Taches1) {
                for (let tache1 of $ctrl.View.Budget.Taches1) {
                    var textTache1 = tache1.Code + ' - ' + tache1.Libelle;
                    for (let tache2 of tache1.Taches2) {
                        var textTache2 = tache2.Code + ' - ' + tache2.Libelle;
                        for (let tache3 of tache2.Taches3) {
                            // Ignore la tâche 3 en cas de déplacement si la cible est la tache 3 courante de l'énumération
                            if ($ctrl.Target === $ctrl.TargetEnum.Tache3 && $ctrl.Action === $ctrl.ActionEnum.Deplacer && $ctrl.Tache3 === tache3) {
                                continue;
                            }
                            var textTache3 = tache3.Code + ' - ' + tache3.Libelle;
                            taches3.push({
                                Tache3: tache3,
                                TextTache1: textTache1,
                                TextTache2: textTache2,
                                TextTache3: textTache3,
                                Text: textTache1 + ' > ' + textTache2 + ' > ' + textTache3
                            });
                        }
                    }
                }
            }

            var setAllTache3 = true;
            if ($ctrl.View.Tache3 !== null && $ctrl.View.Tache3 !== allTache3) {
                // Remet la bonne référence dans la liste déroulante des tâches 3
                for (let tache3 of taches3) {
                    if (tache3 !== allTache3) {
                        if (tache3.Tache3.TacheId === $ctrl.View.Tache3.Tache3.TacheId) {
                            $ctrl.View.Tache3 = tache3;
                            setAllTache3 = false;
                            break;
                        }
                    }
                }
            }
            if (setAllTache3) {
                $ctrl.View.Tache3 = allTache3;
            }

            $ctrl.View.Taches3 = taches3;
        }

        // Met à jour les tâches 4 en fonction des tâches 3
        function UpdateTaches4FromTaches3() {
            var taches4 = [];
            var id = 0;
            for (let tache3 of $ctrl.View.Taches3) {
                if (tache3 === allTache3) {
                    continue;
                }
                for (let tache4 of tache3.Tache3.Taches4) {
                    // Ignore la tâche 4 si la cible est la tache 4 courante de l'énumération
                    if ($ctrl.Target === $ctrl.TargetEnum.Tache4 && $ctrl.Tache4 === tache4) {
                        continue;
                    }

                    // Ignore la tâche 4 si c'est une T4 rev
                    if (tache4.IsT4Rev) {
                        continue;
                    }

                    // Ignore la tâche 4 si elle a été supprimée
                    if (tache4.Deleted) {
                        continue;
                    }

                    // Ignore la tâche 4 si une tâche 3 a été selectionnée mais que la tâche 4 n'est pas son enfant
                    if ($ctrl.View.Tache3 !== null && $ctrl.View.Tache3 !== allTache3 && tache4.Tache3 !== $ctrl.View.Tache3.Tache3) {
                        continue;
                    }

                    // Ajoute la tâche 4
                    taches4.push({
                        Id: "tache4_" + id++,
                        Selected: false,
                        TextTache1: tache3.TextTache1,
                        TextTache2: tache3.TextTache2,
                        TextTache3: tache3.TextTache3,
                        TextTache4: tache4.Code + ' - ' + tache4.Libelle,
                        Tache1: tache3.Tache3.Tache2.Tache1,
                        Tache2: tache3.Tache3.Tache2,
                        Tache3: tache3.Tache3,
                        Tache4: tache4
                    });
                }
            }
            $ctrl.View.Taches4 = taches4;
        }

        // Retourne le text du bouton d'action
        function GetActionButtonText() {
            switch ($ctrl.Action) {
                case $ctrl.ActionEnum.Copier:
                    return $ctrl.resources.CopierDeplacerT4_Bouton_Copier;
                case $ctrl.ActionEnum.Deplacer:
                    return $ctrl.resources.CopierDeplacerT4_Bouton_Déplacer;
                case $ctrl.ActionEnum.Remplacer:
                    return $ctrl.resources.CopierDeplacerT4_Bouton_Remplacer;
            }
        }

        // Indique si le bouton d'action est actif ou non
        function GetActionButtonEnabled() {
            if (!$ctrl.View || !$ctrl.View.Taches4) {
                return false;
            }
            for (let tache4 of $ctrl.View.Taches4) {
                if (tache4.Selected) {
                    return true;
                }
            }
            return false;
        }
    }
})();
