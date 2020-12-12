(function () {
    'use strict';

    var personnelPickerComponent = {
        templateUrl: '/Areas/Pointage/ListePointagePersonnel/Scripts/Components/personnel-picker.component.html',
        bindings: {
            resources: '<',
            personnel: '=',
            periode: '=',
            pointagesList: '=',
            showInPopup: '=',
            showLabel: '='
        },
        require: {
            parentCtrl: '^ngController'
        },
        controller: personnelPickerController
    };

    angular.module('Fred').component('personnelPickerComponent', personnelPickerComponent);

    angular.module('Fred').controller('personnelPickerController', personnelPickerController);

    personnelPickerController.$inject = ['$scope', 'PersonnelPickerService'];

    function personnelPickerController($scope, PersonnelPickerService) {

        var $ctrl = this;

        //////////////////////////////////////////////////////////////////
        // Init                                                         //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {
            $ctrl.userOrganizationId = $ctrl.parentCtrl.userOrganisationId;
        };

        //////////////////////////////////////////////////////////////////
        // Handlers                                                     //
        //////////////////////////////////////////////////////////////////
        $ctrl.handleChangePersonnel = function () {
            if ($ctrl.personnel) {
                PersonnelPickerService.setPersonnel($ctrl.personnel);
                $scope.$emit('changePersonnel');
            }
        };

        $ctrl.handleDeletePersonnel = function () {
            $ctrl.personnel = null;
            PersonnelPickerService.setPersonnel($ctrl.personnel);
        };

    }
})();