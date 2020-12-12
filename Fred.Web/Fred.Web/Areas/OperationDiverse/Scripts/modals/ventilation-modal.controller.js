(function (angular) {
    'use strict';

    angular.module('Fred').controller('VentilationModalController', VentilationModalController);

    VentilationModalController.$inject = ['$scope', '$filter', '$uibModalInstance', 'Notify', 'VentilationService', 'OperationDiverseService', 'UserService', 'ProgressBar'];

    function VentilationModalController($scope, $filter, $uibModalInstance, Notify, VentilationService, OperationDiverseService, UserService, ProgressBar) {
        ProgressBar.start();
        var $ctrl = this;
        $ctrl.resources = resources;
        $scope.loading = true;
        $ctrl.ciSelected = $scope.$resolve.ciSelected;
        $ctrl.societeId = $ctrl.ciSelected.SocieteId;
        $ctrl.currencySymbol = $scope.$resolve.currencySymbol;
        $ctrl.family = $scope.$resolve.family;
        $ctrl.EcartRestant = $ctrl.family.AccountingAmount - $ctrl.family.FredAmount;
        $ctrl.operationDiverses = null;

        $ctrl.periodLabel = $filter('date')($scope.$resolve.dateSelected, 'LLLL yyyy');

        $ctrl.ressourceSelected = null;
        $ctrl.tacheSelected = null;
        $ctrl.montant = 0;
        $ctrl.ventilationMontantComptaLibelle = $ctrl.resources.Libelle_MontantCompta;

        $ctrl.isBusy = false;

        init();

        function init() {
            UserService.getCurrentUser().then(function (user) {
                if (user.Personnel.Societe.Groupe.Code === 'GFTP') { 
                    $ctrl.ventilationMontantComptaLibelle = $ctrl.resources.Libelle_MontantCompta_FTP;
                }
            });

            VentilationService.GetOdListByFamilyAndPeriod($ctrl.ciSelected.CiId, $ctrl.family.FamilyId, $scope.$resolve.dateSelected)
                .success(getOdListSuccess)
                .catch(getOdListFailed);
        }

        function getOdListSuccess(response) {
            $ctrl.operationDiverses = response;
            $ctrl.operationDiverses.forEach(function (od) {
                od.Modified = false;
            });
            $ctrl.FredAmount = $ctrl.family.FredAmount - sum($ctrl.operationDiverses);
            $ctrl.EcartVentille = sum($ctrl.operationDiverses);
            $ctrl.GapAmount = $ctrl.family.GapAmount;
            ProgressBar.complete();
            $scope.loading = false;
        }

        function getOdListFailed() {
            Notify.error($ctrl.resources.Global_Notification_Error);
        }

        /**
         * Fermeture de la fenêtre
         */
        $ctrl.close = function () {
            $uibModalInstance.dismiss("cancel");
        };

        /**
         * Ajout d'une nouvelle OD vide
         */
        $ctrl.Add = function () {
            var newOd = { VentilationId: 0, Libelle: '', ResourceName: '', TaskName: '', Quantity: 1, UnitName: '', PUHT: 1, Amount: 1, Commentaire: '', Modified: true };
            if (!$ctrl.operationDiverses) { $ctrl.operationDiverses = []; }
            $ctrl.operationDiverses.push(newOd);
        };

        /**
        * Supprime une OD             
        * @param {any} ventilation Ventilation à supprimer
        */
        $ctrl.Delete = function (ventilation) {
            var operationDiverse = { OperationDiverseId: ventilation.VentilationId };
            OperationDiverseService.Delete(operationDiverse)
                .success(DeleteSuccess(operationDiverse))
                .error(DeleteError);
        };

        $ctrl.SaveOrUpdate = function (ventilation) {
            if ($scope.formVentilation.$invalid) {
                return;
            }

            if (ventilation.VentilationId !== 0 && ventilation.Modified === false) {
                ventilation.Modified = true;
            }
            else if (ventilation.VentilationId !== 0 && ventilation.Modified === true) {
                ventilation.Modified = false;
                operationDiverse = {
                    ciId: $ctrl.ciSelected.CiId,
                    Date: moment($scope.$resolve.dateSelected).format('YYYY-MM-DD'),
                    RessourceId: ventilation.ResourceId,
                    FamilleOperationDiverseId: $ctrl.family.FamilyId,
                    TacheId: ventilation.TaskId,
                    Montant: ventilation.Amount,
                    Quantite: ventilation.Quantity,
                    PUHT: ventilation.PUHT,
                    Commentaire: ventilation.Commentaire,
                    OperationDiverseId: ventilation.VentilationId,
                    DateComptable: moment($scope.$resolve.dateSelected).format('YYYY-MM-DD'),
                    UniteId: ventilation.UnitId,
                    Libelle: ventilation.Libelle
                };

                OperationDiverseService.Update(operationDiverse)
                    .success(UpdateTotal());
            }
            else {
                ventilation.Modified = false;
                ventilation.FamilleOperationDiverseId = $ctrl.family.FamilyId;

                var operationDiverse = {
                    ciId: $ctrl.ciSelected.CiId,
                    Date: moment($scope.$resolve.dateSelected).format('YYYY-MM-DD'),
                    RessourceId: ventilation.ResourceId,
                    FamilleOperationDiverseId: $ctrl.family.FamilyId,
                    TacheId: ventilation.TaskId,
                    Montant: ventilation.Amount,
                    Quantite: ventilation.Quantity,
                    PUHT: ventilation.PUHT,
                    Commentaire: ventilation.Commentaire,
                    DateComptable: moment($scope.$resolve.dateSelected).format('YYYY-MM-DD'),
                    UniteId: ventilation.UnitId,
                    Libelle: ventilation.Libelle

                };
                $scope.newOd = operationDiverse;
                OperationDiverseService.CreateOD(operationDiverse)
                    .success(function (response) {
                        var updateOd = $ctrl.operationDiverses.map(function (operationDiverse) { return operationDiverse.VentilationId; }).indexOf(0);
                        $ctrl.operationDiverses[updateOd].VentilationId = response.OperationDiverseId;
                        UpdateTotal();
                        $scope.formVentilation.$setPristine();
                    })
                    .error(UpdateError);
            }
        };

        function UpdateError() {
            Notify.error($ctrl.resources.Global_Notification_Error);
        }

        function UpdateTotal() {
            $ctrl.EcartVentille = sum($ctrl.operationDiverses);
            $ctrl.family.GapAmount = $ctrl.family.AccountingAmount - $ctrl.FredAmount - $ctrl.EcartVentille;
            $ctrl.family.FredAmount = $ctrl.EcartVentille + $ctrl.FredAmount;
            $scope.$resolve.consolidationLines.TotalFredAmount = sumFred($scope.$resolve.consolidationLines.FamiliesAmounts);
            $scope.$resolve.consolidationLines.TotalGapAmount = sumGap($scope.$resolve.consolidationLines.FamiliesAmounts);
        }

        function sum(obj) {
            var sum = 0;
            for (var el in obj) {
                if (obj.hasOwnProperty(el)) {
                    sum += parseFloat(obj[el].Amount);
                }
            }
            return sum;
        }

        function sumGap(obj) {
            var sum = 0;
            for (var el in obj) {
                if (obj.hasOwnProperty(el)) {
                    sum += parseFloat(obj[el].GapAmount);
                }
            }
            return sum;
        }

        function sumFred(obj) {
            var sum = 0;
            for (var el in obj) {
                if (obj.hasOwnProperty(el)) {
                    sum += parseFloat(obj[el].FredAmount);
                }
            }
            return sum;
        }

        function DeleteSuccess(operationDiverse) {
            var removeIndex = $ctrl.operationDiverses.map(function (operationDiverse) { return operationDiverse.VentilationId; }).indexOf(operationDiverse.OperationDiverseId);
            $ctrl.operationDiverses.splice(removeIndex, 1);
            var activesOperationsDiverse = $ctrl.operationDiverses.filter(function (operationDiverse) { return !operationDiverse.Modified; });
            $ctrl.EcartVentille = sum(activesOperationsDiverse);
            $ctrl.family.FredAmount = $ctrl.FredAmount - $ctrl.EcartVentille;
            $ctrl.family.GapAmount = $ctrl.family.AccountingAmount - $ctrl.FredAmount - $ctrl.EcartVentille;
            $scope.$resolve.consolidationLines.TotalFredAmount = sumFred($scope.$resolve.consolidationLines.FamiliesAmounts);
            $scope.$resolve.consolidationLines.TotalGapAmount = sumGap($scope.$resolve.consolidationLines.FamiliesAmounts);
        }

        function DeleteError() {
            Notify.error($ctrl.resources.Global_Notification_Error);
        }

        /**
         * Ouvre la lookup des tâches
         * @returns {string} URL de l'API de recherche des tâches
         */
        $ctrl.showLookUpTask = function () {
            return '/api/Tache/SearchLight/?page=1&pageSize=20&societeId=&ciId=' + $ctrl.ciSelected.CiId;
        };

        /**
         * Met à jour la ventilation avec la tâche sélectionnée
         * @param {any} ventilation Ventilation à mettre à jour
         * @param {any} task Tâche sélectionnée
         */
        $ctrl.selectTask = function (ventilation, task) {
            ventilation.TaskId = task.TacheId;
            ventilation.TaskName = task.CodeLibelle;
        };

        /**
         * Ouvre la lookup des ressources
         * @returns {string} URL de l'API de recherche des ressources
         */
        $ctrl.showLookUpResource = function () {
            return '/api/Ressource/SearchLight/?societeId=' + $ctrl.ciSelected.SocieteId;
        };

        /**
         * Met à jour la ventilation avec la ressource sélectionnée
         * @param {any} ventilation Ventilation à mettre à jour
         * @param {any} resource Ressource sélectionnée
         */
        $ctrl.selectResource = function (ventilation, resource) {
            ventilation.ResourceId = resource.RessourceId;
            ventilation.ResourceName = resource.CodeLibelle;
        };

        /**
         * Ouvre la lookup des unités
         * @returns {string} URL de l'api de recherche des unités
         */
        $ctrl.showLookUpUnit = function () {
            return '/api/Unite/SearchLight/';
        };

        /**
         * Met à jour la ventilation avec l'unité sélectionnée
         * @param {any} ventilation Ventilation à mettre à jour
         * @param {any} unit Unité sélectionnée
         */
        $ctrl.selectUnit = function (ventilation, unit) {
            ventilation.UnitId = unit.UniteId;
            ventilation.UnitName = unit.Libelle;
        };

        /**
         * Met à jour le montant lors d'un changement de PUHT ou de Quantité
         * @param {any} ventilation Ventilation à mettre à jour
         */
        $ctrl.onChange = function (ventilation) {
            ventilation.Amount = ventilation.PUHT * ventilation.Quantity;
        };
    }
})(angular);
