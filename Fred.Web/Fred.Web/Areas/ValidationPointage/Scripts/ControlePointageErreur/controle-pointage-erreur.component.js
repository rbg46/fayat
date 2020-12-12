(function (angular) {
    'use strict';

    var controlePointageErreurComponent = {
        templateUrl: '/Areas/ValidationPointage/Scripts/ControlePointageErreur/controle-pointage-erreur.html',
        bindings: {},
        require: {
            parentCtrl: '^ngController'
        },
        controller: controlePointageErreurController
    };

    angular.module('Fred').component('controlePointageErreurComponent', controlePointageErreurComponent);

    angular.module('Fred').controller('controlePointageErreurController', controlePointageErreurController);

    controlePointageErreurController.$inject = ['$scope', '$filter', '$q', 'ValidationPointageService', 'ProgressBar', 'Notify'];

    function controlePointageErreurController($scope, $filter, $q, ValidationPointageService, ProgressBar, Notify) {

        var $ctrl = this;

        /* -------------------------------------------------------------------------------------------------------------
         *                                            INIT
         * -------------------------------------------------------------------------------------------------------------
         */

        $ctrl.$onInit = function () {
            init();
            angular.extend($ctrl, {
                // Fonctions exposées
                handleControlePointageSelection: handleControlePointageSelection,
                handleSearch: handleSearch,

                // Variables
                filter: {},
                erreurCount: 0,
                selectedLotPointage: null,
                typeControleLabel: resources.VPWeb_ControleChantier,
                typeControle: {
                    ControleChantier: 1,
                    ControleVrac: 2
                },
                busy: false,
                paging: { pageSize: 20, page: 1, hasMorePage: true },
                searchText: "",
                selectedControle: null,
                selectedTypeControle: 0,
                period: $filter('date')($ctrl.parentCtrl.periode, 'MM-yyyy')
            });

            FredToolBox.bindScrollEnd('#personnel-erreur-list', actionLoadMore);
        };

        function init() {
            $ctrl.resources = resources;
            $ctrl.permissionKeys = PERMISSION_KEYS;
            $scope.$on("validationPointageCtrl.SelectedPeriod", actionOnChangePeriod);
            $scope.$on("validationPointageCtrl.SelectedLotPointage", actionOnChangeLotPointage);
            $scope.$on("validationPointageCtrl.Data", actionGetData);
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            HANDLERS
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @description Gestion de la sélection du Contrôle pointage     
         */
        function handleControlePointageSelection(typeControle) {
            $ctrl.controlePointageErreurList = [];
            $ctrl.erreurCount = 0;
            switch (typeControle) {
                case $ctrl.typeControle.ControleChantier:
                    $ctrl.selectedControle = $ctrl.selectedLotPointage.ControleChantier ? $ctrl.selectedLotPointage.ControleChantier : null;
                    $scope.$emit("controlePointageErreurCtrl.SelectedControlePointageId", $ctrl.selectedControle ? $ctrl.selectedControle.ControlePointageId : null);
                    $ctrl.typeControleLabel = $ctrl.resources.VPWeb_ControleChantier;
                    $ctrl.selectedTypeControle = $ctrl.typeControle.ControleChantier;
                    break;
                case $ctrl.typeControle.ControleVrac:
                    $ctrl.selectedControle = $ctrl.selectedLotPointage.ControleVrac ? $ctrl.selectedLotPointage.ControleVrac : null;
                    $scope.$emit("controlePointageErreurCtrl.SelectedControlePointageId", $ctrl.selectedControle ? $ctrl.selectedControle.ControlePointageId : null);
                    $ctrl.typeControleLabel = $ctrl.resources.VPWeb_ControleVrac;
                    $ctrl.selectedTypeControle = $ctrl.typeControle.ControleVrac;
                    break;
                default: break;
            }

            actionSearch(true);
        }

        /*
         * @description Gestion de la recherche
         */
        function handleSearch() {
            actionSearch(true);
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            ACTIONS
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @description Action au changement de la période
         */
        function actionOnChangePeriod(e, period) {
            $ctrl.period = $filter('date')(period, 'MM-yyyy');
        }

        /*
         * @description Action lors de la sélection d'un lot de pointage
         */
        function actionOnChangeLotPointage(event, lotPointage) {
            $ctrl.selectedLotPointage = lotPointage;

            if (!$ctrl.selectedTypeControle) {
                $ctrl.selectedTypeControle = $ctrl.typeControle.ControleChantier;
                $ctrl.selectedControle = lotPointage.ControleChantier;
            }

            handleControlePointageSelection($ctrl.selectedTypeControle);
        }

        /*
         * @description action Lance une recherche
         */
        function actionSearch(firstLoad) {
            $q.when()
                .then(function () { return firstLoad; })
                .then(actionGetPersonnelErreurList);
        }

        /*
         * @description Récupère la liste des personnels avec chacun ses erreurs
         */
        function actionGetPersonnelErreurList(firstLoad) {
            $scope.$emit("controlePointageErreurCtrl.TotalErreurCount", 0);

            if ($ctrl.selectedControle) {
                return actionGetControlePointageErreurList(firstLoad);
            }
        }

        /*
         * @description Récupération des erreurs de contrôle du personnel
         */
        function actionGetControlePointageErreurList(firstLoad) {
            $ctrl.busy = true;

            if (firstLoad) {
                $ctrl.controlePointageErreurList = [];
                $ctrl.paging.page = 1;
            }

            ProgressBar.start();
            return ValidationPointageService.GetControlePointageErreurList({ controlePointageId: $ctrl.selectedControle.ControlePointageId, page: $ctrl.paging.page, pageSize: $ctrl.paging.pageSize, searchText: $ctrl.searchText }).$promise
                .then(function (value) {
                    var data = value;
                    angular.forEach(data.Erreurs, function (val) {
                        angular.forEach(val.Erreurs, function (e, key) {
                            if (e.DateRapport) {
                                e.DateRapport = $filter('toLocaleDate')(e.DateRapport);
                                e.DateRapportPeriode = null;
                            }
                            else {
                                e.DateRapportPeriode = $ctrl.selectedLotPointage.Periode;
                            }

                        });
                        $ctrl.controlePointageErreurList.push(val);
                    });
                    $ctrl.erreurCount = value.TotalErreurCount;
                    $scope.$emit("controlePointageErreurCtrl.TotalErreurCount", $ctrl.erreurCount);
                    $ctrl.busy = false;
                    $ctrl.paging.hasMorePage = $ctrl.controlePointageErreurList.length < value.TotalPersonnelCount;
                })
                .catch(Notify.defaultError)
                .finally(ProgressBar.complete);
        }

        /* 
         * @function actionLoadMore()
         * @description Action Chargement de données supplémentaires (scroll end)
         */
        function actionLoadMore() {
            if (!$ctrl.busy && $ctrl.paging.hasMorePage) {
                $ctrl.paging.page++;
                actionGetPersonnelErreurList(false);
            }
        }

        /*
         * @function actionGetData(event, data)
         * @description Action Récupération des données issues du controller principal (validation-pointage.controller.js)
         */
        function actionGetData(event, data) {
            $ctrl.selectedLotPointage = null;
            $ctrl.selectedControle = null;
            $ctrl.typeControleLabel = null;
            $ctrl.selectedTypeControle = null;
            $ctrl.erreurCount = null;
            $ctrl.controlePointageErreurList = [];
        }
    }
})(angular);