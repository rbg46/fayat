(function (angular) {
    'use strict';

    var lotPointageComponent = {
        templateUrl: '/Areas/ValidationPointage/Scripts/LotPointage/lot-pointage.html',
        bindings: {},
        require: {
            parentCtrl: '^ngController'
        },
        controller: lotPointageController
    };

    angular.module('Fred').component('lotPointageComponent', lotPointageComponent);

    angular.module('Fred').controller('lotPointageController', lotPointageController);

    lotPointageController.$inject = ['$q', '$uibModal', 'confirmDialog', '$scope', '$filter', 'ValidationPointageService', 'Notify', 'ProgressBar', 'authorizationService'];

    function lotPointageController($q, $uibModal, confirmDialog, $scope, $filter, ValidationPointageService, Notify, ProgressBar, authorizationService) {

        var $ctrl = this;

        /* -------------------------------------------------------------------------------------------------------------
         *                                            INIT
         * -------------------------------------------------------------------------------------------------------------
         */
        angular.extend($ctrl, {
            // Fonctions exposées
            handleControleChantier: handleControleChantier,
            handleControleVrac: handleControleVrac,
            handleVisa: handleVisa,
            handleSelectLotPointage: handleSelectLotPointage,

            // Variables
            filter: {},
            selectedLotPointageId: null
        });

        $ctrl.$onInit = function () {
            init();
        };

        function init() {
            $ctrl.resources = resources;
            $ctrl.status = $ctrl.parentCtrl.status;
            $ctrl.isCurrentUserFes = $ctrl.parentCtrl.isCurrentUserFes;
            $ctrl.userOrganizationId = $ctrl.parentCtrl.userOrganizationId;
            $ctrl.rights = authorizationService.getRights(PERMISSION_KEYS.AffichageRemonteeVracValidationPointageIndex);
            $scope.$on("validationPointageCtrl.Data", actionGetData);
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            HANDLERS
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @description Exécution du contrôle chantier
         */
        function handleControleChantier(lotPointageId) {
            $q.when().then(function () { return lotPointageId; })
                .then(actionControleChantier);
        }

        /*
         * @description Exécution du contrôle vrac
         */
        function handleControleVrac(lotPointageId) {
            $q.when().then(function () { return $ctrl.parentCtrl.typeControle.ControleVrac; })
                .then($ctrl.parentCtrl.actionGetFilter)
                .then(function (filter) { $ctrl.filter = filter; return lotPointageId; })
                .then(showPopUpControleVrac);
        }

        /*
         * @description Signature du lot
         */
        function handleVisa(lotPointageId) {
            actionExecuteVisa(lotPointageId);
        }

        /*
         * @description Handle du click sur une ligne du tableau
         */
        function handleSelectLotPointage(lotPointageId) {
            $ctrl.selectedLotPointageId = lotPointageId;
            $scope.$emit("lotPointageCtrl.SelectedLotPointageId", $ctrl.selectedLotPointageId);
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            ACTIONS
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @description Action exécution du contrôle chantier
         */
        function actionControleChantier(lotPointageId) {

            confirmDialog.confirm($ctrl.resources, $ctrl.resources.VPWeb_ExecuterControleChantierConfirmation, "flaticon flaticon-warning")
                .then(function (value) {

                    ProgressBar.start();
                    ValidationPointageService.ExecuteControleChantier({ lotPointageId: lotPointageId }, null).$promise
                        .then(actionManageControle)
                        .then(Notify.message)
                        .catch(function (error) { Notify.defaultError(); })
                        .finally(ProgressBar.complete);
                });
        }

        /*
         * @description Action signature du lot
         */
        function actionExecuteVisa(lotPointageId) {
            confirmDialog.confirm($ctrl.resources, $ctrl.resources.VPWeb_ExecuterSignatureConfirmation, "flaticon flaticon-warning")
                .then(function (value) {

                    ProgressBar.start();
                    ValidationPointageService.ExecuteVisa({ lotPointageId: lotPointageId }, null).$promise
                        .then(function (value) {
                            var lotPointage = $filter('filter')($ctrl.lotPointageList, { LotPointageId: value.LotPointageId }, true)[0];
                            lotPointage.AuteurVisa = value.AuteurVisa;
                            lotPointage.AuteurVisaId = value.AuteurVisaId;
                            lotPointage.DateVisa = $filter('toLocaleDate')(value.DateVisa);
                            return $ctrl.resources.Global_Notification_Enregistrement_Success;
                        })
                        .then(Notify.message)
                        .catch(Notify.defaultError)
                        .finally(ProgressBar.complete);

                });
        }

        /*
         * @description Action exécution du contrôle vrac
         */
        function showPopUpControleVrac(lotPointageId) {

            var modalInstance = $uibModal.open({
                animation: true,
                component: 'controleVracComponent',
                resolve: {
                    resources: function () { return $ctrl.resources; },
                    handleShowLookup: function () { return $ctrl.parentCtrl.handleShowLookup; },
                    filter: function () { return $ctrl.filter; },
                    isCurrentUserFes : function() {return $ctrl.isCurrentUserFes;}
                }
            });

            modalInstance.result.then(function (filter) {

                ProgressBar.start();
                verificationCiSep(lotPointageId, filter);
            });
        }

        function verificationCiSep(lotPointageId, filter){
            ValidationPointageService.VerificationCiSepControleVrac(lotPointageId, filter)
                .then(function (response) {
                    if (response.data.length > 0) {
                        $uibModal.open({
                            animation: true,
                            backdrop: 'static',
                            component: 'VerificationCiSepComponent',
                            resolve: {
                                resources: function () { return $ctrl.resources; },
                                ciSepList: function () { return response.data; },
                                controleVrac: function () { return true; },
                                remonteeVrac: function () { return false; }
                            }
                        });

                        //modalInstance2.result.then(function(){return null});
                        ProgressBar.complete();
                        return null;
                    } else {
                        actionControleVrac(lotPointageId, filter);
                    }
                })
                .catch(function (error) { Notify.defaultError(); });
        }

        function actionControleVrac(lotPointageId, filter){
            ValidationPointageService.ExecuteControleVrac({ lotPointageId: lotPointageId }, filter).$promise
            .then(actionManageControle)
            .then(Notify.message)
            .catch(function (error) { Notify.defaultError(); })
            .finally(ProgressBar.complete);
        }

        /*
         * @description Action gestion de la récupération du contrôle pointage
         */
        function actionManageControle(value) {
            var notif = "";
            var lotPointage = $filter('filter')($ctrl.lotPointageList, { LotPointageId: value.LotPointageId }, true)[0];
            value.DateDebut = $filter('toLocaleDate')(value.DateDebut);
            value.DateFin = $filter('toLocaleDate')(value.DateFin);

            switch (value.TypeControle) {
                case $ctrl.parentCtrl.typeControle.ControleChantier:
                    lotPointage.ControleChantier = value;
                    lotPointage.ControleChantier = setControleChantierTooltip(lotPointage.ControleChantier);
                    notif = $ctrl.resources.VPWeb_ControleChantierNotif;
                    break;
                case $ctrl.parentCtrl.typeControle.ControleVrac:
                    lotPointage.ControleVrac = value;
                    lotPointage.ControleVrac = setControleVracTooltip(lotPointage.ControleVrac);
                    notif = $ctrl.resources.VPWeb_ControleVracNotif;
                    break;

                default: console.error("TypeControle undefined"); break;
            }
            return notif;
        }

        /*
         * @description Gestion des données issues du controller principal (validation-pointage.controller.js)
         */
        function actionGetData(event, data) {
            if (data) {
                $ctrl.lotPointageList = data.lotPointageList;
                $ctrl.aucunVerrouillageCount = data.aucunVerrouillageCount;

                angular.forEach($ctrl.lotPointageList, function (val, key) {

                    if (val.ControleChantier) {
                        val.ControleChantier = setControleChantierTooltip(val.ControleChantier);
                    }
                    if (val.AuteurVisaId) {
                        val.ExecutionTooltip = String.format($ctrl.resources.VPWeb_ExecutionTooltip, $filter('date')(val.DateVisa, 'dd/MM/yy \'à\' HH\'h\'mm'), val.AuteurVisa.PrenomNom);

                    }
                    if (val.ControleVrac) {

                        val.ControleVrac = setControleVracTooltip(val.ControleVrac);
                    }
                });
            }
        }

        /*
         * Met en forme et set le tooltip pour le Controle Chantier
         */
        function setControleChantierTooltip(controleChantier) {

            var date = $filter('date')(controleChantier.DateDebut, 'dd/MM/yy \'à\' HH\'h\'mm');
            switch (controleChantier.Statut) {
                case $ctrl.status.Done:
                    controleChantier.ExecutionTooltip = String.format($ctrl.resources.VPWeb_ExecutionTooltip, date, controleChantier.AuteurCreationPrenomNom) + " : veuillez consulter la liste des anormalies détectées dans l'onglet 'Chantier'";
                    break;
                case $ctrl.status.InProgress:
                    controleChantier.ExecutionTooltip = String.format($ctrl.resources.VPWeb_ExecutionTooltip2, $ctrl.resources.VPWeb_En_Cours_Execution, date, controleChantier.AuteurCreationPrenomNom);
                    break;
                default:
                    break;
            }
            return controleChantier;
        }

        /*
         * Met en forme et set le tooltip pour le Controle Vrac
         */
        function setControleVracTooltip(controleVrac) {
            var date = $filter('date')(controleVrac.DateDebut, 'dd/MM/yy \'à\' HH\'h\'mm');
            switch (controleVrac.Statut) {
                case $ctrl.status.Done:
                    controleVrac.ExecutionTooltip = String.format($ctrl.resources.VPWeb_ExecutionTooltip, date, controleVrac.AuteurCreationPrenomNom) + " : veuillez consulter la liste des anormalies détectées dans l'onglet 'Vrac'";
                    break;
                case $ctrl.status.InProgress:
                    controleVrac.ExecutionTooltip = String.format($ctrl.resources.VPWeb_ExecutionTooltip2, $ctrl.resources.VPWeb_En_Cours_Execution, date, controleVrac.AuteurCreationPrenomNom);
                    break;
                case $ctrl.status.Failed:
                    controleVrac.ExecutionTooltip = String.format($ctrl.resources.VPWeb_ExecutionTooltip2, $ctrl.resources.VPWeb_ExecutionControleVracFailed, date, controleVrac.AuteurCreationPrenomNom) + " : veuillez contacter le support technique.";
                    break;
                default:
                    break;
            }
            return controleVrac;
        }
    }
})(angular);