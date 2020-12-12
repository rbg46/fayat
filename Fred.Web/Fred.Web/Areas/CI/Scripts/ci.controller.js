(function (angular) {
    'use strict';

    angular.module('Fred').controller('CIController', CIController);

    CIController.$inject = ['$filter', '$q', 'Notify', 'CIService', 'ProgressBar', 'favorisService', 'favoriModal', '$window'];

    /**
     * Controller des CI.
     *
     * @param {any} $filter $filter       
     * @param {any} $q $q   
     * @param {any} Notify Notify
     * @param {any} CIService CIService
     * @param {any} ProgressBar ProgressBar   
     * @param {any} favorisService favorisService   
     * @param {any} favoriModal favoriModal   
     * @returns {CIController} $ctrl
     */
    function CIController($filter, $q, Notify, CIService, ProgressBar, favorisService, favoriModal, $window) {

        // assignation de la valeur du scope au controller pour les templates non mis à jour
        var $ctrl = this;

        // méthodes exposées
        angular.extend($ctrl, {
            handleLoadPage: handleLoadPage,
            handleSelectCi: handleSelectCi,
            handleSearch: handleSearch,
            handleGetNewFavori: handleGetNewFavori,
            getFilterOrFavoris: getFilterOrFavoris,
            addFilter2Favoris: addFilter2Favoris,
            cancelFilter: cancelFilter,
            displayFilterState: displayFilterState 
        });

        init();

        return $ctrl;

        /**
         * Initialisation du controller.     
         */
        function init() {
            angular.extend($ctrl, {
                // Instanciation Objet Ressources
                resources: resources,
                hasMorePage: true,
                busy: false,
                paging: { pageSize: 25, currentPage: 1 },
                ciDetailURL: "/CI/CI/Detail/",
                favori: [],
                CIList: [],
                filters: {},
                oldFilters: {}
            });
            FredToolBox.bindScrollEnd('#containerTableauCI', actionLoadMore);

        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            LISTE DES CI
         * -------------------------------------------------------------------------------------------------------------
         */

        /**
         * Gestion du chargement de la liste des CI
         * @param {int} ciFavoriId Identifiant du favori
         */
        function handleLoadPage(ciFavoriId) {
            var promise;

            // Chargement de la liste des CI à partir d'un filtre vierge
            if (ciFavoriId === null || ciFavoriId === "") {
                promise = CIService.GetFilter().$promise;
            }
            // Chargement de la liste des CI à partir du filtre du favori
            else {
                promise = favorisService.GetById(ciFavoriId);
            }
            ProgressBar.start();
            promise
                .then(actionGetFilter)
                .then(function (filter) { actionSearch(filter, true); })
                .catch(Notify.defaultError)
                .finally(ProgressBar.complete);
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            HANDLERS
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @description Handler Redirection vers le détail du CI sélectionné
         */
        function handleSelectCi(selectedCi) {
            actionGoToDetailCi(selectedCi);
        }

        /*
         * @description Handler Bouton Appliquer les filtres
         */
        function handleSearch(filters) {
            $ctrl.oldFilters = angular.copy(filters);
            actionSearch(filters, true);
        }

        /*
         * @description Handler Récupère une nouvelle instance de favori
         */
        function handleGetNewFavori() {
            $q.when()
                .then(actionGetNewFavori)
                .then(function (favori) {
                    favoriModal.open(resources, favori);
                });
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            ACTIONS
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @function actionSearch(filters, firstLoad)
         * @description Action de recherche d'un CI à partir de filtre
         */
        function actionSearch(filters, firstLoad) {
            $ctrl.busy = true;
            sessionStorage.setItem('CIFilter', JSON.stringify($ctrl.filters));
            if (firstLoad) {
                $ctrl.CIList = [];
                $ctrl.paging.currentPage = 1;
            }
            ProgressBar.start();
            CIService.Search({ page: $ctrl.paging.currentPage, pageSize: $ctrl.paging.pageSize }, filters).$promise
                .then(function (value) {
                    angular.forEach(value, function (val) {
                        val.DateOuverture = $filter('toLocaleDate')(val.DateOuverture);
                        val.DateFermeture = $filter('toLocaleDate')(val.DateFermeture);
                        $ctrl.CIList.push(val);
                    });
                    $ctrl.busy = false;
                    $ctrl.hasMorePage = value.length !== $ctrl.paging.pageSize;
                })
                .catch(function (error) { console.log(error); })
                .finally(ProgressBar.complete);
        }

        /*
         * @function actionGetNewFavori()
         * @description Récupère un nouvelle objet Favori
         */
        function actionGetNewFavori() {
            return favorisService.GetNew("CI").then(function (value) {
                $ctrl.favori = value;
                $ctrl.favori.Filtre = $ctrl.filters;
                return $ctrl.favori;
            });
        }

        /*
         * @function actionGoToDetailCi(ci)
         * @description Redirection vers le détail d'un CI
         */
        function actionGoToDetailCi(ci) {
            window.location.href = $ctrl.ciDetailURL + ci.CiId;
        }

        /* 
         * @function    actionLoadMore()
         * @description Action Chargement de données supplémentaires (scroll end)
         */
        function actionLoadMore() {
            if (!$ctrl.busy && !$ctrl.hasMorePage) {
                $ctrl.paging.currentPage++;
                actionSearch($ctrl.filters, false);
            }
        }

        /*
         * @function actionGetFilter(filter)
         * @description Enregistre le filtre de recherche
         */
        function actionGetFilter(filters) {
            if (angular.equals($ctrl.filters, {})) {
                $ctrl.filters = filters ? filters : {};
            }
            return $ctrl.filters;
        }

        /*
       * @function cancelFilter()
       * @CAncel Filter
       */
        function cancelFilter() {
            $ctrl.filters = angular.copy($ctrl.oldFilters);
        }

        function displayFilterState() {
            return $ctrl.filters.IsSEP || $ctrl.filters.ClotureOk || $ctrl.filters.DateOuvertureFrom || $ctrl.filters.DateFermetureTo;
        }

        /*
        * @function addFilter2Favoris()
        * @description Crée un nouveau favori
        */
        function addFilter2Favoris(filters) {
            var url = $window.location.pathname;
            if ($ctrl.favoriId !== 0) {
                url = $window.location.pathname.slice(0, $window.location.pathname.lastIndexOf("/"));
            }
            favorisService.initializeAndOpenModal("CI", url, filters);
        }

        function getFilterOrFavoris(favoriId) {
            $ctrl.favoriId = parseInt(favoriId);
            ProgressBar.start();
            if ($ctrl.favoriId !== 0) {
                favorisService.getFilterByFavoriIdOrDefault({ favoriId: $ctrl.favoriId, defaultFilter: $ctrl.filter }).then(function (response) {
                    $ctrl.filters = response;
                    actionSearchWithFilterorFavoris();
                }).catch(function (error) { console.log(error); })
                    .finally(ProgressBar.complete);
            } else {
                if (sessionStorage.getItem('CIFilter') !== null) {
                    $ctrl.filters = JSON.parse(sessionStorage.getItem('CIFilter'));
                    actionSearchWithFilterorFavoris();
                    ProgressBar.complete();
                } else {
                    CIService.GetFilter().$promise.then(function (filter) {
                        actionGetFilter(filter);
                        actionSearch($ctrl.filters);
                    }).catch(Notify.defaultError)
                        .finally(ProgressBar.complete);
                }
            }
        }

        function actionSearchWithFilterorFavoris() {
            $ctrl.filters.DateOuvertureFrom ? new Date($ctrl.filters.DateOuvertureFrom) : null;
            $ctrl.filters.DateFermetureTo ? new Date($ctrl.filters.DateFermetureTo) : null;
            actionSearch($ctrl.filters);
        }


    }

}(angular));