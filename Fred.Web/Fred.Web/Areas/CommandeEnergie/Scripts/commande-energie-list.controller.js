(function (angular) {
    'use strict';

    angular.module('Fred').controller('CommandeEnergieListController', CommandeEnergieListController);

    CommandeEnergieListController.$inject = ['$q', 'Notify', 'ProgressBar', 'CommandeEnergieService', 'favorisService'];

    /**
     * Controller de la liste des Commandes Energies
     * 
     * @param {any} $q $q     
     * @param {any} Notify Notify     
     * @param {any} ProgressBar ProgressBar     
     * @param {any} CommandeEnergieService CommandeEnergieService
     * @param {any} favorisService les favoris
     * @returns {CommandeEnergieListController} $ctrl
     */
    function CommandeEnergieListController($q, Notify, ProgressBar, CommandeEnergieService, favorisService) {

        // assignation de la valeur du scope au controller pour les templates non mis à jour
        var $ctrl = this;

        // méthodes exposées
        $ctrl.handleSearch = handleSearch;
        $ctrl.handleLookupSelection = handleLookupSelection;
        $ctrl.handleLookupDeletion = handleLookupDeletion;
        $ctrl.handleRedirectToDetail = handleRedirectToDetail;
        $ctrl.handleNewCommande = handleNewCommande;

        // variables
        $ctrl.resources = resources;
        $ctrl.filter = {};
        $ctrl.favoriId = null;
        $ctrl.commandeEnergies = [];
        $ctrl.hasMorePage = false;
        $ctrl.busy = false;
        $ctrl.typeEnergies =
            {
                P: { class: 'pers'},
                M: { class: 'mat'},
                I: { class: 'int'},
                D: { class: 'dvr'}
            };

        $ctrl.statutCommandes = {
            PREP: { class: 'prep' },
            CL: { class: 'clot' },
            VA: { class: 'val' }
        };

        init();

        return $ctrl;

        function init(favoriId) {
            $ctrl.favoriId = parseInt(favoriId) || 0;
            $q.when()
                .then(onBeginRequest)
                .then(getFilter)
                .then(onSearchCommandeEnergies)
                .then(onFinallyRequest);

            FredToolBox.bindScrollEnd('#commande-energie-table', onLoadMore);

        }


        //////////////////////////////////////////////////////////////////
        //                      PUBLIC METHODS                          //
        //////////////////////////////////////////////////////////////////


        // #region Favoris
        ////// Favoris non encore exploiter////////////
        //$ctrl.handleAddFilter2Favoris = function handleAddFilter2Favoris() {
        //    $ctrl.addFilter2Favoris();
        //};

        //$ctrl.addFilter2Favoris = function addFilter2Favoris() {
        //    var filterToSave = $ctrl.filter;
        //    var url = $window.location.pathname;
        //    if ($ctrl.favoriId !== 0) {
        //        url = $window.location.pathname.slice(0, $window.location.pathname.lastIndexOf("/"));
        //    }
        //    favorisService.initializeAndOpenModal("CommandeEnergie", url, filterToSave);
        //};
        /*-----------------------------------------------------*/
        //////////////////////////////////////////
        //#region

         function getFilter() {
            if ($ctrl.favoriId !== 0) {
                return favorisService.getFilterByFavoriId($ctrl.favoriId)
                    .then(function (response) {
                        $ctrl.filter = {
                            Periode: response.Periode,
                            Ci: response.CI,
                            Fournisseur: response.Fournisseur
                        };
                    })
                    .catch(function () { Notify.error($ctrl.resources.Global_Notification_Error); });
            }
            else {
                $ctrl.filter.Periode = new Date();
                var localStorage = JSON.parse(sessionStorage.getItem('commandeEnergieFilter'));
                    
                if (localStorage!== null) {
                    $ctrl.filter.Periode = localStorage.Periode || new Date();
                    $ctrl.filter.Ci = localStorage.Ci || undefined;
                    $ctrl.filter.Fournisseur = localStorage.Fournisseur || undefined;

                }
            }
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            HANDLERS
         * -------------------------------------------------------------------------------------------------------------
         */


        function handleSearch() {
            $q.when()
                .then(onBeginRequest)
                .then(function () { return true; })
                .then(onSearchCommandeEnergies)
                .then(onFinallyRequest);
        }

        function handleLookupSelection(type, item) {
            switch (type) {
                case 'CI':
                    $ctrl.filter.Ci = item;                   
                    break;
                case 'Fournisseur':
                    $ctrl.filter.Fournisseur = item;
                    break;
                default: break;
            }    
            handleSearch();
        }


        function handleLookupDeletion(type) {
            switch (type) {
                case 'CI':
                    $ctrl.filter.Ci = null;                 
                    break;
                case 'Fournisseur':
                    $ctrl.filter.Fournisseur = null;
                    break;
                default: break;
            }
            handleSearch();
        }


        function handleRedirectToDetail(commandeId) {
            location.href = `/CommandeEnergie/CommandeEnergie/Detail/${commandeId}`;
        }
        
        function handleNewCommande() {
            location.href = '/CommandeEnergie/CommandeEnergie/Detail';
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            ACTIONS
         * -------------------------------------------------------------------------------------------------------------
         */


        /**
         * Appel de recherche des commandes énergies
         * @param {any} firstLoad Premier chargement
         * @returns {any} promise
         */
        function onSearchCommandeEnergies(firstLoad) {

            if (firstLoad) {
                $ctrl.commandeEnergies = [];
                $ctrl.filter.page = 1;
            }

            return CommandeEnergieService.Search($ctrl.filter)
                .then(onSuccessSearchCommandeEnergies)
                .catch(onErrorSearchCommandeEnergies);
        }

        /** Sauvegarder les filtres dans le sessionStorage */
        function setItemToSessionStorage() {
            sessionStorage.setItem('commandeEnergieFilter', JSON.stringify({ Ci: $ctrl.filter.Ci, Periode: $ctrl.filter.Periode, Fournisseur: $ctrl.filter.Fournisseur }));
        }


        /**
         * Traitement des données reçues : chargement des commandes énergies
         * @param {any} response réponse de la promise : données reçues
         */
        function onSuccessSearchCommandeEnergies(response) {
            if (response) {                
                for (var i = 0; i < response.data.length; i++) {
                    $ctrl.commandeEnergies.push(response.data[i]);
                }
                setItemToSessionStorage();
            }

            $ctrl.hasMorePage = response.data.length === $ctrl.filter.pageSize;
        }


        /**
         * Traitement en cas d'erreur sur la promise
         * @param {any} error Erreur de la promise
         */
        function onErrorSearchCommandeEnergies(error) {
            console.error(error);
            Notify.error($ctrl.resources.Global_Notification_Error);
        }


        /** Chargement supplémentaire des données */
        function onLoadMore() {
            if (!$ctrl.busy && $ctrl.hasMorePage) {
                $ctrl.filter.page++;
                onSearchCommandeEnergies(false);
            }
        }


        /** Init d'une requête Http */
        function onBeginRequest() {
            $ctrl.busy = true;
            ProgressBar.start();
        }


        /** Finally d'une requête Http */
        function onFinallyRequest() {
            $ctrl.busy = false;
            ProgressBar.complete();
        }

      

    }
}(angular));