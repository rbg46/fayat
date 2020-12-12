(function (angular) {
    'use strict';

    angular.module('Fred').controller('ValidationPointageController', ValidationPointageController);

    ValidationPointageController.$inject = ['$q', '$filter', '$scope', 'UserService', '$uibModal', 'ValidationPointageService', 'ProgressBar', 'Notify'];

    function ValidationPointageController($q, $filter, $scope, UserService, $uibModal, ValidationPointageService, ProgressBar, Notify) {

        /* -------------------------------------------------------------------------------------------------------------
        *                                            INIT
        * -------------------------------------------------------------------------------------------------------------
        */
        var $ctrl = this;

        // Méthodes exposées
        angular.extend($ctrl, {
            handleChangePeriod: handleChangePeriod,
            handleRemonteeVrac: handleRemonteeVrac,
            handleShowLookup: handleShowLookup,
            actionGetFilter: actionGetFilter,
            handleExportPdf: handleExportPdf,
            handleShowRemonteeVracErreur: handleShowRemonteeVracErreur,
            handleExecuteRemonteePrimes: handleExecuteRemonteePrimes
        });

        init();

        /*
         * Initialisation du controller.
         */
        function init() {

            angular.extend($ctrl, {
                // Instanciation Objet Ressources
                resources: resources,
                permissionKeys: PERMISSION_KEYS,
                lotPointageList: [],
                periode: new Date(),
                currentUser: null,
                filter: {},
                typeControle: {
                    ControleChantier: 1,
                    ControleVrac: 2,
                    RemonteeVrac: 3
                },
                status: {
                    None: 0,
                    InProgress: 1,
                    Done: 2,
                    Refused: 3,
                    Failed: 4
                },
                selectedLotPointage: null,
                selectedControlePointageId: null,
                remonteeVrac: null,
                currentTotalErreurCount: 0,
                isCurrentUserFes: false,
                userOrganizationId: 0
            });

            $q.when()
                .then(ProgressBar.start)
                .then(actionLoadData)
                .then(ProgressBar.complete);

            $scope.$on("lotPointageCtrl.SelectedLotPointageId", function (event, lotPointageId) {
                $ctrl.selectedLotPointage = $filter('filter')($ctrl.lotPointageList, { LotPointageId: lotPointageId }, true)[0];
                $scope.$broadcast('validationPointageCtrl.SelectedLotPointage', $ctrl.selectedLotPointage);
            });

            // Si on sélectionne un ControlePointage (Vrac ou Chantier)
            $scope.$on("controlePointageErreurCtrl.SelectedControlePointageId", function (event, controlePointageId) {
                $ctrl.selectedControlePointageId = controlePointageId;
            });

            $scope.$on("controlePointageErreurCtrl.TotalErreurCount", function (event, totalErreurCount) {
                $ctrl.currentTotalErreurCount = totalErreurCount;
            });

            UserService.getCurrentUser().then(function(user) {
                $ctrl.currentUser = user.Personnel;
                $ctrl.userOrganizationId = $ctrl.currentUser.Societe.Organisation.OrganisationId;
                if ($ctrl.currentUser.Societe.Groupe.Code.trim() === "GFES") {
                    $ctrl.isCurrentUserFes = true;
                }
            });
        }

        return $ctrl;

        /* -------------------------------------------------------------------------------------------------------------
        *                                            HANDLERS
        * -------------------------------------------------------------------------------------------------------------
        */

        function handleChangePeriod() {
            $q.when()
                .then(ProgressBar.start)
                .then(function () { $scope.$broadcast('validationPointageCtrl.SelectedPeriod', $ctrl.periode); })
                .then(actionLoadData)
                .then(ProgressBar.complete);
        }

        /*
         * @description Exécution de la remontée vrac
         */
        function handleRemonteeVrac() {
            $q.when().then(function () { return $ctrl.typeControle.RemonteeVrac; }).then(actionGetFilter).then(actionShowPopUpRemonteeVrac);
        }

        /*
         * @description Gestion de l'URL pour la Lookup
         */
        function handleShowLookup(val, param) {
            var url = '/api/' + val + '/SearchLight/?page=1&pageSize=20&societeId={0}&ciId={1}';
            switch (val) {
                case "Societe":
                    url = String.format(url, null, null);
                    break;
                case "EtablissementComptable":
                    url = String.format(url, null, null);
                    break;
                case "EtablissementPaie":
                    if ($ctrl.isCurrentUserFes) {
                        url = String.format('/api/EtablissementPaie/GetEtabPaieListForValidationPointageVracFesAsync/?page=1&pageSize=20&societeId={0}', param);
                    }
                    else {
                        url = '/api/EtablissementPaie/SearchLight/?page={0}&societeId={1}';
                        url = String.format(url, 1, param);
                    }
                    break;
                default:
                    url = '/api/' + val + '/SearchLight/';
                    break;
            }
            return url;
        }

        /*
         * @description Gestion de l'export PDF des erreurs de validation
         */
        function handleExportPdf() {
            if ($ctrl.selectedControlePointageId) {
                ValidationPointageService.ExportPdfControlePointageErreur($ctrl.selectedControlePointageId);
            }
        }

        /**
         * Ouverture de la modal d'affichage des erreurs de remontée vrac
         */
        function handleShowRemonteeVracErreur() {
            $uibModal.open({
                animation: true,
                component: 'remonteeVracErreurComponent',
                size: 'lg',
                windowClass: 'modal-lotpointage',
                resolve: {
                    resources: function () { return $ctrl.resources; },
                    remonteeVrac: function () { return $ctrl.remonteeVrac; },
                    periode: function () { return $ctrl.periode; }
                }
            });
        }

        /*
         * Exécute la remontée de primes vers Anael
         */
        function handleExecuteRemonteePrimes() {
            ProgressBar.start();

            ValidationPointageService.ExecuteRemonteePrimes({ periode: $filter('date')($ctrl.periode, 'MM-dd-yyyy') }, null).$promise
                .then(Notify.message)
                .finally(ProgressBar.complete);
        }


        /* -------------------------------------------------------------------------------------------------------------
        *                                            ACTIONS
        * -------------------------------------------------------------------------------------------------------------
        */

        /*
         * @description Récupération des Lots de pointages de l'utilisateur connecté
         */
        function actionGetLotPointageList() {
            return ValidationPointageService.Get({ periode: $filter('date')($ctrl.periode, 'MM-dd-yyyy') }).$promise
                .then(function (value) {
                    angular.forEach(value, actionOnSuccess);

                    $ctrl.lotPointageList = value;
                    return value;
                })
                .catch(function (error) { });
        }

        /*
         * @description Renvoie les données aux composants enfant
         */
        function actionBroadcast(data) {
            $scope.$broadcast("validationPointageCtrl.Data", data);
        }

        /*
         * @description Chargement des données
         */
        function actionLoadData() {
            return $q.when()
                .then(actionGetLotPointageList)
                .then(function (data) {
                    return { "lotPointageList": data, "aucunVerrouillageCount": 0 };
                })
                .then(actionBroadcast)
                .then(actionGetRemonteeVrac);
        }

        /*
         * @description Formatte la dates en dates locales
         */
        function actionOnSuccess(lp, key) {
            if (lp.AuteurCreation.PersonnelId === $ctrl.currentUser.PersonnelId) {
                lp.AuteurCreation.PrenomNom = String.format($ctrl.resources.VPWeb_Vous, lp.AuteurCreation.PrenomNom);
                lp.CurrentUser = true;
            }
            else {
                lp.CurrentUser = false;
            }

            lp.DateCreation = $filter('toLocaleDate')(lp.DateCreation);
            lp.DateModification = $filter('toLocaleDate')(lp.DateModification);
            lp.DateVisa = $filter('toLocaleDate')(lp.DateVisa);
            lp.Periode = $filter('toLocaleDate')(lp.Periode);

            angular.forEach(lp.ControlePointages, function (cp, key1) {
                cp.DateDebut = $filter('toLocaleDate')(cp.DateDebut);
                cp.DateFin = $filter('toLocaleDate')(cp.DateFin);
            });
        }

        /*
         * @description Action Récupération d'un nouveau filtre pour le contrôle et la remontée vrac
         */
        function actionGetFilter(typeControle) {
            return ValidationPointageService.GetFilter({ typeControle: typeControle }).$promise.then(function (value) { $ctrl.filter = value; return $ctrl.filter; }).catch(Notify.defaultError);
        }

        /*
         * @description Action exécution de la remontée vrac
         */
        function actionShowPopUpRemonteeVrac() {
            var modalInstance = $uibModal.open({
                animation: true,
                component: 'remonteeVracComponent',
                size: 'lg',
                windowClass: 'modal-lotpointage',
                resolve: {
                    resources: function () { return $ctrl.resources; },
                    handleShowLookup: function () { return handleShowLookup; },
                    filter: function () { return $ctrl.filter; },
                    isCurrentUserFes: function () { return $ctrl.isCurrentUserFes; },
                    periode: function () { return $filter('date')($ctrl.periode, 'MM-dd-yyyy'); }
                }
            });

            modalInstance.result.then(function (filter) {

                ProgressBar.start();
                verificationCiSep(filter);
            });
        }

        function verificationCiSep(filter) {
            ValidationPointageService.VerificationCiSepRemonteeVrac($filter('date')($ctrl.periode, 'MM-dd-yyyy'), filter)
                .then(function (response) {
                    if (response.data.length > 0) {
                        $uibModal.open({
                            animation: true,
                            backdrop: 'static',
                            component: 'VerificationCiSepComponent',
                            windowClass: 'modal-lotpointage',
                            resolve: {
                                resources: function () { return $ctrl.resources; },
                                ciSepList: function () { return response.data; },
                                controleVrac: function () { return false; },
                                remonteeVrac: function () { return true; }
                            }
                        });

                        ProgressBar.complete();
                        return null;
                    } else {
                        actionRemonteeVrac(filter);
                    }
                })
                .catch(function (error) { Notify.defaultError(); });
        }

        function actionRemonteeVrac(filter) {
            ValidationPointageService.ExecuteRemonteeVrac({ periode: $filter('date')($ctrl.periode, 'MM-dd-yyyy') }, filter).$promise
                .then(function (value) {

                    if (value.RemonteeVracId) {
                        value.DateDebut = $filter('toLocaleDate')(value.DateDebut);
                        value.DateFin = $filter('toLocaleDate')(value.DateFin);
                        value.Periode = $filter('toLocaleDate')(value.Periode);
                        if (value.AuteurCreationPrenomNom) {
                            value.ExecutionTooltip = String.format($ctrl.resources.VPWeb_ExecutionTooltip, $filter('date')(value.DateDebut, 'dd/MM/yyyy HH:mm:ss'), value.AuteurCreationPrenomNom);
                        }
                    }
                    $ctrl.remonteeVrac = value;

                    return $ctrl.resources.VPWeb_RemonteeVracNotif;
                })
                .then(Notify.message)
                .catch(function (error) { Notify.defaultError(); })
                .finally(ProgressBar.complete);

        }

        /*
         * @description Récupération de la dernière remontée vrac
         */
        function actionGetRemonteeVrac() {
            return ValidationPointageService.GetRemonteeVrac({ periode: $filter('date')($ctrl.periode, 'MM-dd-yyyy'), utilisateurId: $ctrl.currentUser.UtilisateurId }).$promise
                .then(function (value) {
                    if (value.RemonteeVracId) {
                        value.DateDebut = $filter('toLocaleDate')(value.DateDebut);
                        value.DateFin = $filter('toLocaleDate')(value.DateFin);
                        value.Periode = $filter('toLocaleDate')(value.Periode);
                        if (value.AuteurCreation) {
                            var date = $filter('date')(value.DateDebut, 'dd/MM/yyyy HH:mm:ss');
                            var auteur = value.AuteurCreation.PrenomNom;

                            value.ErreurTooltip = value.Erreurs.length > 0 ? `${value.Erreurs.length} erreurs` : null;

                            if (value.Statut === $ctrl.status.Done) {
                                value.ExecutionTooltip = String.format($ctrl.resources.VPWeb_ExecutionTooltip, date, auteur);
                                value.ExecutionTooltip += value.Erreurs.length > 0 ? " - " + $ctrl.resources.VPWeb_ConsulterRapportErreur : "";
                            }
                            else if (value.Statut === $ctrl.status.Failed) {
                                value.ExecutionTooltip = String.format($ctrl.resources.VPManager_RemonteVracJobFailed, date);
                            }
                            else {
                                value.ExecutionTooltip = String.format($ctrl.resources.VPWeb_ExecutionTooltip, date, auteur);
                            }
                        }
                    }
                    $ctrl.remonteeVrac = value;
                })
                .catch(Notify.defaultError);
        }
    }
}(angular));