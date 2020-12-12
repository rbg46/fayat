(function () {
    'use strict';

    var dateSelectionMoyenComponent = {
        templateUrl: '/Areas/Moyen/Scripts/Components/date-selection-moyen.component.html',
        bindings: {
            resources: '<',
            isUpdatePointage: '<',
            isExportPointage: '<'
        },
        controller: DateSelectionMoyenController,
        controllerAs: '$ctrl'
    };

    angular.module('Fred').component('dateSelectionMoyenComponent', dateSelectionMoyenComponent);

    angular.module('Fred').controller('DateSelectionMoyenController', DateSelectionMoyenController);

    DateSelectionMoyenController.$inject = ['$scope', 'MoyenService'];
    
    function DateSelectionMoyenController($scope, MoyenService) {
        var $ctrl = this;
        $ctrl.resources = resources;
        $ctrl.handleDateChanges = handleDateChanges;
        $ctrl.Validate = Validate;

        initDates();

        /*
         * @description Initialisation des dates : Début et fin.
         */
        function initDates() {
            $ctrl.dateDebut = new Date();
            let dateFin = new Date();
            dateFin.setMonth(dateFin.getMonth() + 1);
            $ctrl.dateFin = dateFin;
        }

        /*
         * @description Fonction de vérification de la cohérence des dates saisies (début avant fin)
         */
        function handleDateChanges() {
            $ctrl.datesInvalid = MoyenService.IsDateFinInvalid($ctrl.dateDebut, $ctrl.dateFin);
        }

         /*
         * @description Validation de la séléction des dates
         */
        function Validate() {
            let dates = { dateDebut: $ctrl.dateDebut, dateFin: $ctrl.dateFin };
            if ($ctrl.isUpdatePointage) {
                $scope.$emit('event.update.pointage.materiel', dates);
            }

            if ($ctrl.isExportPointage) {
                $scope.$emit('event.envoi.pointage.moyen', dates);
            }
        }
    }
})();