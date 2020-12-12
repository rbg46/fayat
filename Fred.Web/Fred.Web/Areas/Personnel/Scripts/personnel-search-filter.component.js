(function () {
    'use strict';


    angular.module('Fred').component('personnelSearchFilterComponent', {
        templateUrl: '/Areas/Personnel/Scripts/personnel-search-filter.tpl.html',
        bindings: {
            resources: '<',
            filter: '=',
            favoriId: '<'
        },
        require: {
            parentCtrl: '^ngController'
        },
        controller: 'personnelSearchFilterComponentController'
    });

    angular.module('Fred').controller('personnelSearchFilterComponentController', personnelSearchFilterComponentController);

    personnelSearchFilterComponentController.$inject = ['$window', '$q', 'fredSubscribeService', 'favorisService', 'ProgressBar'];

    function personnelSearchFilterComponentController($window, $q, fredSubscribeService, favorisService, ProgressBar) {

        var $ctrl = this;
        var personnelEditUrl = "/Personnel/Personnel/Edit/";

        //////////////////////////////////////////////////////////////////
        // Déclaration des propriétés publiques                         //
        //////////////////////////////////////////////////////////////////

        $ctrl.resources = resources;
        //ici extend est important car on ne change pas l'object,
        //qui est partagé avec le controlleur principal mais on on change juste ses valeurs.
        $ctrl.filter = angular.extend($ctrl.filter, {
            'ValueText': '',
            'ValueTextNom': '',
            'ValueTextPrenom': '',
            'SearchEtab': undefined,
            'EtablissementPaieCode': '',
            'SearchSociete': undefined,
            'SocieteCode': '',
            'Nom': true,
            'Prenom': true,
            'SocieteCodeLibelle': true,
            'Matricule': true,
            'IsInterne': false,
            'IsInterimaire': false,
            'IsUtilisateur': false,
            'IsActif': true,
            'NomPrenomAsc': true,
            'SocieteAsc': true,
            'MatriculeAsc': true,
            'IsPersonnelsNonPointables': false
        });

        $ctrl.originalFilter = angular.copy($ctrl.filter);

        if (sessionStorage.getItem('personnelFilter') !== null) {
            $ctrl.filter = Object.assign($ctrl.filter, JSON.parse(sessionStorage.getItem('personnelFilter')));
            if (sessionStorage.getItem('delegationActive') !== null) {
                sessionStorage.removeItem('delegationActive');
                sessionStorage.setItem('viewDelegationActive', 1);
            }
            else {
                sessionStorage.removeItem('viewDelegationActive');
            }
        }

        //////////////////////////////////////////////////////////////////
        // Déclaration des fonctions publiques                          //
        //////////////////////////////////////////////////////////////////
        $ctrl.applyFilter = applyFilter;
        $ctrl.handleRedirectAddPersonnel = handleRedirectAddPersonnel;
        $ctrl.addFilter2Favoris = addFilter2Favoris;
        $ctrl.resetFilter = resetFilter;
        $ctrl.handleDeletePickList = handleDeletePickList;
        $ctrl.handleSelectPickList = handleSelectPickList;
        $ctrl.displayFilterState = displayFilterState;

        //////////////////////////////////////////////////////////////////
        // Init                                                         //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {
            $ctrl.userOrganizationId = $ctrl.parentCtrl.userOrganisationId;
            ProgressBar.start();
            favorisService.getFilterByFavoriIdOrDefault({
                favoriId: $ctrl.favoriId, defaultFilter: $ctrl.filter
            })
                .then(function (response) {
                    //ici extend est important car on ne change pas l'object,
                    //qui est partagé avec le controlleur principal mais on on change juste ses valeurs.
                    $ctrl.filter = angular.extend($ctrl.filter, response);
                    fredSubscribeService.raiseEvent('personnelSearchComponent.applyFilter', true);
                })
                .catch(function (error) {
                    console.log(error);
                }).finally(function () {
                    $ctrl.busy = false;
                    // Chargement des données du personnel          
                    ProgressBar.complete();
                });

        };

        //////////////////////////////////////////////////////////////////
        // Action sur les filtres                                       //
        //////////////////////////////////////////////////////////////////


        function resetFilter() {
            $ctrl.filter = angular.extend($ctrl.filter, $ctrl.originalFilter);
            fredSubscribeService.raiseEvent('personnelSearchComponent.applyFilter', true);
            displayFilterState();
        }


        function applyFilter() {
            sessionStorage.setItem('personnelFilter', JSON.stringify($ctrl.filter));
            fredSubscribeService.raiseEvent('personnelSearchComponent.applyFilter', true);
            displayFilterState();
        }


        //////////////////////////////////////////////////////////////////
        // Redirection                                                  //
        //////////////////////////////////////////////////////////////////

        /* 
         * @function handleRedirectAddPersonnel(typetypePersonnel)
         * @description Redirection vers la page d'ajout d'un nouveau personnel externe ou interimaire
         *              L'url prend 2 paramètres, l'id du personnel et le type de personnel
         *              Lors d'un ajout d'un personne, l'id est 0
         */
        function handleRedirectAddPersonnel(typePersonnel) {
            $window.location.href = personnelEditUrl + "0/" + typePersonnel;
        }

        //////////////////////////////////////////////////////////////////
        // Gestion des favoris                                          //
        //////////////////////////////////////////////////////////////////

        /*
        * @function addFilter2Favoris()
        * @description Crée un nouveau favori
        */
        function addFilter2Favoris() {
            var url = $window.location.pathname;
            if ($ctrl.favoriId !== 0) {
                url = $window.location.pathname.slice(0, $window.location.pathname.lastIndexOf("/"));
            }

            favorisService.initializeAndOpenModal("Personnel", url, $ctrl.filter);
        }

        /*     
          * @description Handler : Suppression d'un élément sélectionné dans une picklist     
          * @param {any} type Type de la picklist
          */
        function handleDeletePickList(type) {
            switch (type) {
                case "EtablissementPaie":
                    $ctrl.filter.SearchEtab = undefined;
                    $ctrl.filter.EtablissementPaieCode = '';
                    break;
                case "Societe":
                    $ctrl.filter.SearchSociete = undefined;
                    $ctrl.filter.SocieteCode = '';
                    break;
            }
        }


        /*     
          * @description Handler : Sélection d'un élément dans une picklist     
          * @param {any} type Type de la picklist
          */
        function handleSelectPickList(type) {
            switch (type) {
                case "EtablissementPaie":
                    $ctrl.filter.EtablissementPaieCode = $ctrl.filter.SearchEtab.Code;
                    break;
                case "Societe":
                    $ctrl.filter.SocieteCode = $ctrl.filter.SearchSociete.Code;
                    break;
            }
        }

        /*
        * @function displayFilterState()
        * @description diplay filter state
        */
        function displayFilterState() {
            return !$ctrl.filter.IsActif || $ctrl.filter.IsInterne || $ctrl.filter.IsUtilisateur || $ctrl.filter.IsInterimaire || $ctrl.filter.SearchEtab !== undefined || $ctrl.filter.SearchSociete !== undefined || $ctrl.filter.IsPersonnelsNonPointables;
        }
    }
})();