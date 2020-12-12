(function (angular) {
    'use strict';

    angular.module('Fred').controller('PersonnelController', PersonnelController);

    PersonnelController.$inject = ['$window', '$filter', 'PersonnelService', 'Notify', 'ProgressBar', 'confirmDialog', 'fredSubscribeService', 'favorisService', 'UserService'];

    function PersonnelController($window, $filter, PersonnelService, Notify, ProgressBar, confirmDialog, fredSubscribeService, favorisService, UserService) {
        var $ctrl = this;
        var lastFilter = "";
        // méthodes exposées

        $ctrl.loadPage = loadPage;
        $ctrl.handleSearch = handleSearch;
        $ctrl.handleDeletePersonnel = handleDeletePersonnel;
        $ctrl.handleRedirectPersonnelIndemnite = handleRedirectPersonnelIndemnite;
        $ctrl.handleRedirectAddPersonnel = handleRedirectAddPersonnel;
        $ctrl.handleRedirectDetailPersonnel = handleRedirectDetailPersonnel;
        $ctrl.handleAddFilter2Favoris = handleAddFilter2Favoris;
        $ctrl.onExportExcel = onExportExcel;
        $ctrl.exportReceptionInterim = exportReceptionInterim;
        $ctrl.userOrganisationId = null;
        $ctrl.haveHabilitation = false;
        init();

        /**
         * Initialisation du controller.
         */
        function init() {

            // Instanciation Objet Ressources
            $ctrl.resources = resources;
            $ctrl.permissionKeys = PERMISSION_KEYS;
            $ctrl.paging = { pageSize: 25, currentPage: 1 };
            $ctrl.personnelListOpt = [];
            $ctrl.filter = {};
            $ctrl.favoriId = 0;
            $ctrl.hasMorePage = true;
            $ctrl.personnelEditUrl = "/Personnel/Personnel/Edit/";
            $ctrl.indemniteDeplacementUrl = "/IndemniteDeplacement/IndemniteDeplacement/Index/";
            $ctrl.search = "";
            $ctrl.countPersonnel = 0;
            $ctrl.busy = false;
            $ctrl.displayReduce = true;

            UserService.getCurrentUser().then(function(user) {
                $ctrl.userOrganisationId = user.Personnel.Societe.Organisation.OrganisationId;
            });

            // Abonnement de l'élément listPersonnel à l'évènement
            FredToolBox.bindScrollEnd('#listPersonnel', actionLoadMore);
            fredSubscribeService.subscribe({ eventName: 'personnelSearchComponent.applyFilter', callback: actionLoadData });
            $ctrl.societes = [];
        }

        return $ctrl;

        /*
         * -------------------------------------------------------------------------------------------------------------
         *                                            INITIALISATION
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @function loadPage(favoriId)
         * @description Chargement de la page
         */
        function loadPage(favoriId) {
            $ctrl.favoriId = parseInt(favoriId) || 0;
            getFilter();
        }

        /*
         * @function actionLoadData()
         * @description Rafraichissement des données : liste des personnels
         */
        function actionLoadData(firstLoad) {
            if ($ctrl.busy) {
                return;
            }
            $ctrl.busy = true;
            if (firstLoad) {
                $ctrl.paging.currentPage = 1;
                $ctrl.personnelListOpt = [];
                $ctrl.countPersonnel = 0;
            }
            ProgressBar.start();
            lastFilter = JSON.stringify($ctrl.filter);
            PersonnelService.SearchOptimized({ page: $ctrl.paging.currentPage, pageSize: $ctrl.paging.pageSize, recherche: $ctrl.search }, $ctrl.filter).$promise
                .then(searchSucessed)
                .then(reloadIfFilterChangedBeforeReceiveLastResquest)
                .then(loadSocietesForExport)
                .catch(function (error) {
                    console.log(error);
                })
                .finally(function () {
                    $ctrl.busy = false;
                    ProgressBar.complete();
                });
        }

        /*
         * Action lorsque la response du serveur a une recherche a reussie.
         */
        function searchSucessed(response) {
            $ctrl.countPersonnel = response.TotalCount;
            var personnelList = response.PersonnelList;
            $ctrl.hasMorePage = personnelList.length !== $ctrl.paging.pageSize;
            if (response && personnelList && personnelList.length === 0 && $ctrl.personnelListOpt === 0) {
                Notify.error(resources.Global_Notification_AucuneDonnees);
            }
            else {
                angular.forEach(personnelList, function (personnel) {
                    mapPersonnelDateTime(personnel);
                    mapTypePersonnelLibelle(personnel);
                    $ctrl.personnelListOpt.push(personnel);
                });
            }
        }

        /*
         * Relance la recherche si elle ne correspond pas au dernier criteres.
         */
        function reloadIfFilterChangedBeforeReceiveLastResquest() {
            $ctrl.busy = false;
            var actualFilter = JSON.stringify($ctrl.filter);
            if (lastFilter !== actualFilter) {
                actionLoadData(true);
            }
        }

        /*
         * Mappage des données de type date
         */
        function mapPersonnelDateTime(personnel) {
            personnel.DateEntree = $filter('toLocaleDate')(personnel.DateEntree);
            personnel.DateSortie = $filter('toLocaleDate')(personnel.DateSortie);
            personnel.DateCreation = $filter('toLocaleDate')(personnel.DateCreation);
            personnel.DateSuppression = $filter('toLocaleDate')(personnel.DateSuppression);
            personnel.DateModification = $filter('toLocaleDate')(personnel.DateModification);
        }

        /*
         * Mappage de la propriétée TypePersonnelLibelle
         */
        function mapTypePersonnelLibelle(personnel) {
            if (personnel.IsInterimaire) {
                personnel.TypePersonnelLibelle = resources.Global_Interimaire;
            }
            else if (personnel.IsInterne) {
                personnel.TypePersonnelLibelle = resources.Global_Interne;
            }
            else {
                personnel.TypePersonnelLibelle = resources.Global_Externe;
            }
        }


        /* 
         * @function handleSearch(filter)
         * @description Handler de frappe clavier dans le champs recherche
         */
        function handleSearch(filter) {
            actionLoadData(true);
        }

        function onExportExcel() {
            ProgressBar.start();
            PersonnelService.getExportExcel($ctrl.filter, $ctrl.haveHabilitation)
                .then(response => {
                    PersonnelService.downloadExcel(response.data.id);
                })
                .catch(error => {
                    Notify.error("Erreur lors de l'export excel");
                })
                .finally(() => ProgressBar.complete());
        }

        /*
         * @function exportReceptionInterim()
         * @description permet de lancer l'export de receptions interim
         */
        function exportReceptionInterim() {
            ProgressBar.start();
            let societesSelectionnes = [];
            angular.forEach($ctrl.societes, g => angular.forEach(g.Societes, s => { if (s.Selected) societesSelectionnes.push(s); }));
            PersonnelService.exportReceptionInterim(societesSelectionnes)
                .then(() => Notify.message(resources.Personnel_Index_Export_Success))
                .catch(() => Notify.error(resources.Personnel_Index_Export_Fail))
                .finally(() => ProgressBar.complete());
        }

        /*
         * @function loadSocietesForExport()
         * @description permet de charger les societes pour l'export réception intérim
         */
        function loadSocietesForExport() {
            PersonnelService.SearchSocietes()
                .then(r => {
                    if (r.data) {
                        $ctrl.societes = [];
                        angular.forEach(r.data, s => {
                            s.Selected = false;
                            $ctrl.societes.push(s);
                        });
                    }
                })
                .catch(() => Notify.error(resources.Global_Notification_Error));
        }

        /*
         * -------------------------------------------------------------------------------------------------------------
         *                                            GESTION DES REDIRECTIONS
         * -------------------------------------------------------------------------------------------------------------
         */

        /* 
         * @function handleRedirectPersonnelIndemnite(personnel)
         * @description Redirection pour gérer les indemnités de déplacement 
         */
        function handleRedirectPersonnelIndemnite(personnel) {
            $window.location.href = $ctrl.indemniteDeplacementUrl + personnel.PersonnelId;
        }

        /* 
         * @function redirectDetailPersonnelInterne(personnel)
         * @description Redirection vers le détail du personnel externe
         */
        function handleRedirectDetailPersonnel(personnel) {
            $window.location.href = $ctrl.personnelEditUrl + personnel.PersonnelId;
        }


        /* 
       * @function handleRedirectAddPersonnel(typetypePersonnel)
       * @description Redirection vers la page d'ajout d'un nouveau personnel externe ou interimaire
       *              L'url prend 2 paramètres, l'id du personnel et le type de personnel
       *              Lors d'un ajout d'un personne, l'id est 0
       */
        function handleRedirectAddPersonnel(typePersonnel) {
            $window.location.href = $ctrl.personnelEditUrl + "0/" + typePersonnel;
        }


        /*
         * -------------------------------------------------------------------------------------------------------------
         *                                            DIVERS
         * -------------------------------------------------------------------------------------------------------------
         */

        /* 
         * @function actionLoadMore(personnelId)
         * @description Action Chargement de données supplémentaires (scroll end)
         */
        function actionLoadMore() {
            if (!$ctrl.busy && !$ctrl.hasMorePage) {
                $ctrl.paging.currentPage++;
                actionLoadData(false);
            }
        }

        /* 
         * @function handleDeletePersonnel(personnelId)
         * @description Suppression d'un personnel
         */
        function handleDeletePersonnel(personnelId) {
            confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationSuppression).then(function () {

                PersonnelService.Delete({ personnelId: personnelId }).$promise.then(function (value) {
                    actionLoadData(true);
                    Notify.message(resources.Global_Notification_Suppression_Success);
                }).catch(function (error) { Notify.error(resources.Global_Notification_Error); });

            });
        }

        function handleAddFilter2Favoris() {
            addFilter2Favoris();
        }

        function addFilter2Favoris() {
            var filterToSave = $ctrl.filter;
            var url = $window.location.pathname;
            if ($ctrl.favoriId !== 0) {
                url = $window.location.pathname.slice(0, $window.location.pathname.lastIndexOf("/"));
            }
            favorisService.initializeAndOpenModal("Personnel", url, filterToSave);
        }

        function getFilter() {
            if ($ctrl.favoriId !== 0) {
                return favorisService.getFilterByFavoriId($ctrl.favoriId)
                    .then(function (response) {
                        $ctrl.filter = response;
                    })
                    .then(function () {
                        actionLoadData(true);
                    })              
                    .catch(function () {
                        Notify.error($ctrl.resources.Global_Notification_Error);
                    });
            }
        }
    }
}(angular));