(function () {
    'use strict';

    var rapportMoyenTableComponent = {
        templateUrl: '/Areas/Moyen/Scripts/Components/Tables/rapport-moyen-table.component.html',
        bindings: {
            resources: '<'
        },
        controller: RapportMoyenTableController,
        controllerAs: '$ctrl'
    };

    angular.module('Fred').component('rapportMoyenTableComponent', rapportMoyenTableComponent);

    angular.module('Fred').controller('RapportMoyenTableController', RapportMoyenTableController);

    RapportMoyenTableController.$inject = ['$scope', 'Notify', 'MoyenService', 'ProgressBar', 'confirmDialog'];

    function RapportMoyenTableController($scope, Notify, MoyenService, ProgressBar, confirmDialog) {
        var $ctrl = this;
        $ctrl.resources = resources;

        // méthodes exposées
        angular.extend($ctrl, {
            handleApplyFilter: handleApplyFilter,
            handleResetFilter: handleResetFilter,
        });

        init();

        /**
         * Initialisation du controller.     
         */
        function init() {
            angular.extend($ctrl, {
                // Instanciation Objet Ressources
                resources: resources,
                hasMorePage: true,
                paging: { pageSize: 25, currentPage: 5 },
                rapportMoyenList: [],
                filters: null
            });
            // To do 
            FredToolBox.bindScrollEnd('#containerTableauRapport', actionLoadMore);

            // Filter sur les données rapport
            $scope.$on('event.update.rapport.data', function (event, obj) {
                $ctrl.filters = obj.filters;
                actionSearchWithFilters($ctrl.filters, true);
            });
            // 
            $scope.$on('event.init.rapport.extraction', function (event, obj) {
                $ctrl.filters = obj.filters;
                actionGenerateRapportWithFilters($ctrl.filters);
            });
        }

        /* 
         * @function actionLoadMore()
         * @description Action de chargement de données supplémentaires (scroll end)
         */
        function actionLoadMore() {
            if ($ctrl.hasMorePage) {
                $ctrl.paging.currentPage++;
                actionSearchWithFilters($ctrl.filters, false);
            }
        }

        /*
         * @function handleApplyFilter()
         * @description Applique le filtre séléctionné. 
         */
        function handleApplyFilter() {
            actionSearchWithFilters($ctrl.filters, true);
        }

        /*
         * @function handleResetFilter()
         * @description Réinitialise les filtres.
         */
        function handleResetFilter() {
            if ($ctrl.selectedMoyen != null
                || $ctrl.selectedImmatriculation != null
                || $ctrl.selectedAffectationMoyenType != null
                || $ctrl.selectedTypeMoyen != null
                || $ctrl.selectedSousTypeMoyen != null
                || $ctrl.selectedModelMoyen != null
                || $ctrl.selectedSociete != null
                || $ctrl.selectedEtablissement != null
                || $ctrl.selectedSite != null
                || $ctrl.filters.IsDateFinPredictedOutdated == true
                || $ctrl.filters.IsToBringBack == true
                || $ctrl.filters.IsActive == false
                || $ctrl.filters.DateFrom
                || $ctrl.filters.DateTo
            ) {
                // Numéro parc
                $ctrl.filters.NumParc = '';
                $ctrl.selectedMoyen = null;

                // Numéro d'immatriculation 
                $ctrl.filters.NumImmatriculation = '';
                $ctrl.selectedImmatriculation = null;

                // Type affectation moyen
                $ctrl.filters.AffectationMoyenTypeId = '';
                $ctrl.selectedAffectationMoyenType = null;

                // Type moyen
                $ctrl.filters.TypeMoyen = '';
                $ctrl.selectedTypeMoyen = null;

                // Sous type moyen
                $ctrl.filters.SousTypeMoyen = '';
                $ctrl.selectedSousTypeMoyen = null;

                // Model moyen
                $ctrl.filters.ModelMoyen = '';
                $ctrl.selectedModelMoyen = null;

                // Société
                $ctrl.filters.Societe = '';
                $ctrl.selectedSociete = null;

                // Etablissement
                $ctrl.filters.Etablissement = '';
                $ctrl.selectedEtablissement = null;

                // Site
                $ctrl.filters.SiteActuelId = '';
                $ctrl.selectedSite = null;

                // Date fin prévisionnelle dépassée
                $ctrl.filters.IsDateFinPredictedOutdated = false;

                // Moyen à repatrier
                $ctrl.filters.IsToBringBack = false;

                // Is active
                $ctrl.filters.IsActive = false;

                // Dates
                $ctrl.filters.DateFrom = '';
                $ctrl.filters.DateTo = '';

                actionSearchWithFilters($ctrl.filters, true);
            }

        }

        /*
         * @function actionSearchWithFilters
         * @description Action de recherche d'un rapport de moyen à partir de filtre
         */
        function actionSearchWithFilters(filters, firstLoad) {
            if (firstLoad) {
                $ctrl.rapportMoyenList = [];
                $ctrl.paging.currentPage = 1;
            }
            ProgressBar.start();
            MoyenService.SearchRapportLigneMoyenWithFilters({ page: $ctrl.paging.currentPage, pageSize: $ctrl.paging.pageSize }, filters)
                .$promise
                .then(function (value) {
                    angular.forEach(value, function (val) {
                        $ctrl.rapportMoyenList.push(val);
                    });
                    $ctrl.hasMorePage = value.length === $ctrl.paging.pageSize;
                    $ctrl.SelectedAll = false;
                })
                .catch(function (error) {
                    console.log(error);
                })
                .finally(ProgressBar.complete);
        }

        /*
         * @function actionSearchWithFilters
         * @description Action de recherche d'un rapport de moyen à partir de filtre
         */
        function actionGenerateRapportWithFilters(filters) {
            ProgressBar.start();
            MoyenService.GenerateRapportLigneMoyenWithFilters({}, filters)
                .$promise
                .then(OnGenerateSuccess)
                .catch(function (error) {
                    Notify.error("Problème lors de la génération du document");
                })
                .finally(ProgressBar.complete);
        }

        /*
         * @function Succés de la génération
         * @description succés de la génération du fichier excel .
         */
        function OnGenerateSuccess(value) {
            Notify.message("Génération effectuée avec succès");
            var fileName = "RapportMoyenExtract" + (new Date()).toISOString().slice(0, 10).replace(/-/g, '').substring(0, 6);
            MoyenService.downloadFile(value.id, fileName, false);
        }

    }
})();