(function (angular) {
    'use strict';

    angular.module('Fred').controller('BudgetController', BudgetController);

    BudgetController.$inject = ['$q',
        '$timeout',
        '$scope',
        'BudgetDataService',
        'ProgressBar',
        'BudgetCopyPasteService',
        'Notify',
        'DevisesManagerService',
        'CiManagerService'];

    function BudgetController($q,
        $timeout,
        $scope,
        BudgetDataService,
        ProgressBar,
        BudgetCopyPasteService,
        Notify,
        DevisesManagerService,
        CiManagerService) {

        // assignation de la valeur du scope au controller pour les templates non mis à jour
        var $ctrl = this;

        //////////////////////////////////////////////////////////////////
        // Déclaration des propriétés publiques                         //
        //////////////////////////////////////////////////////////////////
        $ctrl.saving = false;
        $ctrl.taches = [];
        $ctrl.view = 'initialView';

        //ci selectionnée dans la picklist
        $ctrl.ciSelected = null;
        //Tache selectionnée
        $ctrl.taskSelected = null;
        $ctrl.selectedTaskIsModified = false;
        $ctrl.deviseSelected = null;
        $ctrl.showBudgetRessources = false;
        $ctrl.showButtonBudgetRessources = false;
        // Instanciation Objet Ressources
        $ctrl.resources = resources;
        $ctrl.taskSearchText = '';
        $ctrl.tasks = [];
        $ctrl.bugetRevisionStatut = null; //pas de statut a la revision du budget
        $ctrl.isSavingTask = false;
        $ctrl.isSavingTasks = false;

        //////////////////////////////////////////////////////////////////
        // Déclaration des fonctions publiques                          //
        //////////////////////////////////////////////////////////////////
        $ctrl.handleClickOpenCloseBudgetRessources = handleClickOpenCloseBudgetRessources;
        $ctrl.handleClickChangedCI = handleClickChangedCI;

        $ctrl.handleDeviseSelectedChanged = handleDeviseSelectedChanged;
        $ctrl.handleTaskSearchText = handleTaskSearchText;
        $ctrl.handleShowBudget = handleShowBudget;

        $ctrl.onBugdetInfoChanged = onBugdetInfoChanged;
        $ctrl.onTasksLoaded = onTasksLoaded;
        $ctrl.onTacksChanged = onTacksChanged;
        init();

        return $ctrl;

        /**
         * Initialisation du controller.
         * 
         */
        function init() {
            //ici je m'abonne a l'evenement qui est envoyé lors d'un clique sur une ligne
            //On passerra a l'etat 'detail'
            $scope.$on('tacheBudgetComponent.openTask', function (event, taskToOpen) {
                $ctrl.taskSelected = taskToOpen;
                changeView('detailView');
                $scope.$broadcast('budgetCrtl.openTask', taskToOpen);
                //$ctrl.canCopy = BudgetCopyPasteService.canCopy(taskToOpen);
            });

            $scope.$on('budgetRessourceComponent.addRessourceToTask', function (event, newRessource) {
                $scope.$broadcast('budgetCrtl.addRessourceToTask', newRessource);
            });

            $scope.$on('tachesBudgetComponent.recetteTotalChanged', function (event, recetteInfo) {
                $scope.$broadcast('budgetCrtl.recetteTotalChanged', recetteInfo);
            });

            //permet de mettre l'overlay sur la liste en mode edition de la tache (detailView..)
            $scope.$on('tacheDetailComponent.selectedTaskIsModified', function (event, info) {
                //$ctrl.canCopy = BudgetCopyPasteService.canCopy(info);
                $ctrl.selectedTaskIsModified = info.isModified;
            });

            //permet de changer de vue sur la reception du message 
            $scope.$on('changeView', function (event, viewName) {
                changeView(viewName);
            });

            $scope.$on('tacheDetailComponent.taskSaved', function (event, info) {
                $ctrl.isSavingTask = false;
                if (info !== null) {
                    $scope.$broadcast('budgetCrtl.taskSaved', info);
                }
            });

            $scope.$on('tachesBudgetComponent.tasksSaved', function (event, info) {
                $ctrl.isSavingTasks = false;
            });

            $scope.$on('tachesBudgetComponent.quantityChanged', function (event, info) {
                $scope.$broadcast('budgetCrtl.quantityChanged', info);
            });

            $scope.$on('budgetRessourceComponent.ressourceModifiedOnRessourceView', function (event, info) {
                $scope.$broadcast('budgetCrtl.ressourceModifiedOnRessourceView', info);
                $scope.$broadcast('budgetCrtl.ressourceModifiedOnRessourceView2', info);
            });

            $scope.$on('tacheDetailComponent.ressourceChangedOnDetail', function (event, info) {
                $scope.$broadcast('budgetCrtl.ressourceChangedOnDetail', info);
            });

        }

        //////////////////////////////////////////////////////////////////
        // Evenements                                                   //
        //////////////////////////////////////////////////////////////////

        function onBugdetInfoChanged(bugdetInfo) {
            var revision = bugdetInfo;
            if (revision.Statut === 0) {
                $ctrl.bugetRevisionStatut = 'Brouillon';
            }
            if (revision.Statut === 1) {
                $ctrl.bugetRevisionStatut = 'AValider';
            }
            if (revision.Statut === 2) {
                $ctrl.bugetRevisionStatut = 'Valide';
            }
            $scope.$broadcast('budgetCrtl.bugdetInfoChanged', bugdetInfo);
        }

        function onTasksLoaded(tasksLoadedInfo) {
            $ctrl.tasks = tasksLoadedInfo;
            $scope.$broadcast('budgetCrtl.tasksLoaded', tasksLoadedInfo);
        }

        /*
         * Methode appelée quand une tache du plan de tache est modifiée, ajoutée et supprimée
         */
        function onTacksChanged(tasksChangedInfo) {
            $scope.$broadcast('budgetCrtl.onTacksChanged', tasksChangedInfo);
        }

        //////////////////////////////////////////////////////////////////
        // Handlers                                                     //
        //////////////////////////////////////////////////////////////////

        function handleClickChangedCI() {
            $ctrl.selectedTaskIsModified = false;
            actionLoad();
        }

        function handleClickOpenCloseBudgetRessources() {
            var view = $ctrl.view === "detailView" ? "detailWithRessourcesView" : "detailView";
            changeView(view);
        }

        function handleDeviseSelectedChanged(devise) {
            $ctrl.deviseSelected = devise;
            $scope.$broadcast('budgetCrtl.deviseChanged', devise);
        }

        function handleTaskSearchText() {
            $scope.$broadcast('budgetCrtl.taskSearchTextChanged', $ctrl.taskSearchText);
        }

        /*
        *  Affiche le listing
        */
        function handleShowBudget() {
            changeView("listingView");
        }

        //////////////////////////////////////////////////////////////////
        // Actions                                                      //
        //////////////////////////////////////////////////////////////////

        function actionLoad() {
            BudgetDataService.GetCi($ctrl.ciSelected.CiId)
                .then(GetCiSuccess)
                .catch(GetCiError);
        }

        function GetCiSuccess(response) {
            if (response.data.Organisation) {
                $ctrl.ciSelected.OrganisationId = response.data.Organisation.OrganisationId;
                CiManagerService.setCi($ctrl.ciSelected);
                LoadDevises();
                changeView("listingView");
            } else {
                Notify.error(resources.Budget_Controller_Notification_CISansOrganisation_Erreur);
            }

        }

        function GetCiError(error) {
            Notify.error(resources.Budget_Controller_Notification_ChargementCI_Erreur);
        }


        function LoadDevises() {
            BudgetDataService
                .GetDeviseRefByCiId($ctrl.ciSelected.CiId)
                .then(GetDeviseRefByCiIdSuccess)
                .catch(GetDeviseRefByCiIdError);

        }

        function GetDeviseRefByCiIdSuccess(response) {
            $ctrl.devises = response.data;
            if ($ctrl.devises && $ctrl.devises.length && $ctrl.devises.length > 0) {
                DevisesManagerService.setDevises($ctrl.devises);
                $ctrl.deviseSelected = $ctrl.devises[0];
                $scope.$broadcast('budgetCrtl.loadTasks', $ctrl.ciSelected);
                $scope.$broadcast('budgetCrtl.ciSelectedChanged', $ctrl.ciSelected);
            } else {
                Notify.error("Il n' y a pas de devise associée au CI.");
            }

        }

        function GetDeviseRefByCiIdError(error) {
            Notify.error(resources.Budget_Controller_Notification_ChargementDeviseDuCI_Erreur);
        }



        //////////////////////////////////////////////////////////////////
        // Action -  gestion des vues                                   //
        //////////////////////////////////////////////////////////////////

        function changeView(view) {
            if (view === "initialView") {
                $ctrl.showDetail = false;
                $ctrl.showBudgetRessources = false;
                $ctrl.showButtonBudgetRessources = false;
                $ctrl.showListing = false;
                $ctrl.showTasksPlan = false;
                $ctrl.view = view;
            }
            if (view === "listingView") {
                $ctrl.showDetail = false;
                $ctrl.showBudgetRessources = false;
                $ctrl.showButtonBudgetRessources = false;
                $ctrl.showListing = true;
                $scope.$broadcast('changedTaskListView', "LargeList");
                $ctrl.showTasksPlan = false;
                $ctrl.view = view;
            }
            if (view === "detailView") {
                $ctrl.showDetail = true;
                $ctrl.showBudgetRessources = false;
                $ctrl.showButtonBudgetRessources = true;
                $ctrl.showListing = true;
                $scope.$broadcast('changedTaskListView', "SmallList");
                $ctrl.showTasksPlan = false;
                $ctrl.view = view;
            }
            if (view === "detailWithRessourcesView") {
                $ctrl.showDetail = true;
                $ctrl.showBudgetRessources = true;
                $ctrl.showButtonBudgetRessources = true;
                $ctrl.showListing = true;
                $scope.$broadcast('changedTaskListView', "SmallList");
                $ctrl.showTasksPlan = false;
                $ctrl.view = view;
            }
            if (view === "taskPlanView") {
                $ctrl.showDetail = false;
                $ctrl.showBudgetRessources = false;
                $ctrl.showButtonBudgetRessources = false;
                $ctrl.showListing = false;
                $ctrl.showTasksPlan = true;
                $ctrl.view = view;
            }
        }

    }
}(angular));