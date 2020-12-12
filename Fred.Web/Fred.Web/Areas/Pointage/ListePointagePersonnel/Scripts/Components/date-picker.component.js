(function () {
  'use strict';

  var datePickerComponent = {
    templateUrl: '/Areas/Pointage/ListePointagePersonnel/Scripts/Components/date-picker.component.html',
    bindings: {
      resources: '<',
      periode: '<'
    },
    controller: datePickerController

  };

  angular.module('Fred').component('datePickerComponent', datePickerComponent);

  angular.module('Fred').controller('datePickerController', datePickerController);

  datePickerController.$inject = ['$q', '$scope', 'DatePickerService'];

  function datePickerController($q, $scope, DatePickerService) {

    var $ctrl = this;

    //////////////////////////////////////////////////////////////////
    // Déclaration des fonctions publiques                          //
    //////////////////////////////////////////////////////////////////
    $ctrl.changePeriode = changePeriode;

    //////////////////////////////////////////////////////////////////
    // Init                                                         //
    //////////////////////////////////////////////////////////////////
    $ctrl.$onInit = function () {
    };

    //////////////////////////////////////////////////////////////////
    // Actions                                                      //
    //////////////////////////////////////////////////////////////////
    function changePeriode() {
      var result = DatePickerService.setPeriode($ctrl.periode);
      if(result){
        $scope.$emit('changePeriode');
      }
    }    
  }
})();