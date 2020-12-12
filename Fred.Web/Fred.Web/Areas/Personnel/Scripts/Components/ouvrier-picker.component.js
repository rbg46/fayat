(function () {
  'use strict';

  var ouvrierPickerComponent = {
    templateUrl: '/Areas/Personnel/Scripts/components/ouvrier-picker.component.html',
    bindings: {
      resources: '<',
      ouvrier: '='
    },
    controller: ouvrierPickerController
  };

  angular.module('Fred').component('ouvrierPickerComponent', ouvrierPickerComponent);

  angular.module('Fred').controller('ouvrierPickerController', ouvrierPickerController);

  ouvrierPickerController.$inject = ['$scope', 'OuvrierPickerService'];

  function ouvrierPickerController($scope, OuvrierPickerService) {

    var $ctrl = this;

    //////////////////////////////////////////////////////////////////
    // Handlers                                                     //
    //////////////////////////////////////////////////////////////////
    $ctrl.handleChangeOuvrier = function (item) {
      if ($ctrl.ouvrier) {
        OuvrierPickerService.setOuvrier($ctrl.ouvrier);
        $scope.$emit('changeOuvrier');
      }
    };

  }
})();