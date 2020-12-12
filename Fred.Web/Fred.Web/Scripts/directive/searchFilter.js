(function () {
    'use strict';

    angular.module('Fred').directive('searchFilter', searchFilterDirective);

    searchFilterDirective.$inject = ['$http', '$document', '$templateCache', '$parse'];

    function searchFilterDirective($http, $document, $templateCache, $parse) {
        return {
            restrict: 'E',
            scope: {
                filterTemplateUrl: '@',
                filterApiController: '@',
                filterPlaceholder: '@',
                filter: '=',
                oldFilters: '=?',
                callbackFnSearch: '&',
                callbackFnFavoris: '&',
                busy: '<'
            },
            templateUrl: function (element, attrs) {
                return attrs.filterTemplateUrl;
            },
            controller: 'searchFilterDirectiveController'
        };
    }

    angular.module('Fred').controller('searchFilterDirectiveController', searchFilterDirectiveController);

    searchFilterDirectiveController.$inject = ['$http', '$scope', '$element', 'RapportService', 'Notify'];

    function searchFilterDirectiveController($http, $scope, $element, RapportService, Notify) {
        // Instanciation Objet Ressources
        $scope.resources = resources;

        $scope.$on('filter-processed-message', function (event, lastFilter) {
            var newFilter = JSON.stringify($scope.filter);
            $scope.filter.isAllEtablissement = true;
            $scope.filter.EtablissementPaieList = [];
            if (lastFilter !== newFilter) {
                $scope.callbackFnSearch({ filter: $scope.filter });
            }
        });

        // Appel la fonction de recherche avec les filtres choisis
        $scope.applyFilter = function (filter) {
            $scope.callbackFnSearch({ filter: filter });
            closeSearchFilterModal();
        };

        // Appel la fonction de création d'un favori
        $scope.addFilter2Favoris = function (filter) {
            $scope.callbackFnFavoris({ filter: filter });
            closeSearchFilterModal();
        };

        // Réinitialise les filtres
        $scope.resetFilter = function () {
            $http.get($scope.filterApiController).then(function (t) {
                $scope.filter = t.data;
                $scope.callbackFnSearch({ filter: $scope.filter });
                closeSearchFilterModal();
            });
        };

        // Appel la fonction de recherche avec les filtres choisis
        $scope.savePreviousFilter = function (filter) {
            $scope.previousFilter = angular.copy(filter);
        };

        $scope.cancelFilter = function() {
            $scope.filter = $scope.previousFilter;
            closeSearchFilterModal();
        }

        /*
         * @description Gestion de la sélection dans les Lookup
         */
        $scope.handleLookupSelection = function (type, item) {
            switch (type) {
                case 'Organisation':
                    $scope.filter.OrganisationId = item.OrganisationId;
                    $scope.filter.EtablissementPaieIdList = [];
                    $scope.filter.EtablissementPaieList = [];
                    $scope.filter.isAllEtablissement = true;
                    break;
                case 'EtablissementPaieList':        
                    if ($scope.filter.EtablissementPaieIdList.indexOf(item.IdRef) > -1)
                    {
                        Notify.error($scope.resources.Rapport_ModalImport_EtablissementPaie_DejaChoisi);
                    }
                    else {
                        $scope.filter.EtablissementPaieIdList.push(item.IdRef);
                        $scope.filter.EtablissementPaieList.push(item);
                    }
                    break;
            }
        };

        $scope.handleShowLookup = function (type) {
            switch (type){
                case 'EtablissementPaie':
                    var url = '/api/EtablissementPaie/SearchLight/?page={0}&societeId={1}';
                    url = String.format(url, 1, $scope.filter.Societe.SocieteId);
                    return url;
            }
          };

        $scope.handleSelectChoseEtablissement = function () {
            $scope.filter.EtablissementPaieIdList = $scope.filter.EtablissementPaieList.map(x => x.EtablissementPaieId);
            RapportService.GetSocieteByOrganisationId($scope.filter.OrganisationId).then(function (response){
                $scope.filter.Societe = response.data;
            });
        };

        
        $scope.handleLookupDeletion = function (type, item) {
            switch (type) {
                case 'Organisation' :
                    $scope.libelleOrganisation = $scope.resources.Global_ReferentielOrganisation_Placeholder;
                    $scope.filter.Organisation = null;
                    $scope.filter.OrganisationId = null;
                    $scope.filter.EtablissementPaieIdList = [];
                    $scope.filter.EtablissementPaieList = [];
                    $scope.filter.isAllEtablissement = true;
                    break;
                case 'EtablissementPaie' : 
                    var i1 = $scope.filter.EtablissementPaieIdList.indexOf(item.IdRef);
                    var i2 = $scope.filter.EtablissementPaieList.indexOf(item);
                    $scope.filter.EtablissementPaieIdList.splice(i1, 1);
                    $scope.filter.EtablissementPaieList.splice(i2, 1);
                    break;
            }
          };

        /*
         * @description Fonction temporaire : gestion des controls "select"
         */
        $scope.handleSelect = function (type, item) {
            var tmp;

            switch (type) {
                case 'Departement':
                    $scope.filter.Departement = item.Code;
                    break;
                case 'CommandeType':
                    if (item === '') {
                        $scope.filter.TypeId = null;
                        $scope.filter.TypeLibelle = undefined;
                    }
                    else {
                        tmp = JSON.parse(item);
                        $scope.filter.TypeId = tmp.CommandeTypeId;
                        $scope.filter.TypeLibelle = tmp.Libelle;
                    }
                    break;
                case 'StatutCommande':
                    if (item === '') {
                        $scope.filter.StatutId = null;
                        $scope.filter.StatutLibelle = undefined;
                    }
                    else {
                        tmp = JSON.parse(item);
                        $scope.filter.StatutId = tmp.StatutCommandeId;
                        $scope.filter.StatutLibelle = tmp.Libelle;
                    }
                    break;
                case 'SystemeExterne':
                    if (item === '') {
                        $scope.filter.SystemeExterneId = null;
                        $scope.filter.SystemeExterneLibelle = undefined;
                    }
                    else {
                        tmp = JSON.parse(item);
                        $scope.filter.SystemeExterneId = tmp.SystemeExterneId;
                        $scope.filter.SystemeExterneLibelle = tmp.LibelleAffiche;
                    }
                    break;
            }
        };

        function closeSearchFilterModal() {
            angular.element($element[0].querySelector('#demo')).collapse("hide");
        }

        /*
         * @description Augmentation du z-index lorsqu'on ouvre la popup de filtre
         */
        angular.element($element[0].querySelector('#demo')).on('show.bs.collapse', function () {
            $element.find(".filter-container").css({ 'z-index': 1101 });
        });

        /*
         * @description Diminution du z-index lorsqu'on ouvre la popup de filtre
         */
        angular.element($element[0].querySelector('#demo')).on('hidden.bs.collapse', function () {
            $element.find(".filter-container").css({ 'z-index': 'initial' });
        });

    }
})();
