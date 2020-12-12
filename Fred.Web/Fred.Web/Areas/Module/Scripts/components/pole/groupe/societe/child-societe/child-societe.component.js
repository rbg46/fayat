(function () {
  'use strict';

  angular.module('Fred').component('childSocieteComponent', {
    templateUrl: '/Areas/Module/Scripts/components/pole/groupe/societe/child-societe/child-societe.tpl.html',
    bindings: {
      libelle: '<',      
      typeOrganisationId: '<'
    },
    controller: 'childSocieteComponentController'
  });

  angular.module('Fred').controller('childSocieteComponentController', childSocieteComponentController);

  childSocieteComponentController.$inject = ['$scope', 'typeOrganisationConverterService', 'Notify'];

  function childSocieteComponentController($scope, typeOrganisationConverterService, Notify) {

    var $ctrl = this;  
    $ctrl.typeOrganisation = 'Not defined';
   
    //////////////////////////////////////////////////////////////////
    // Init                                                         //
    //////////////////////////////////////////////////////////////////
    $ctrl.$onInit = function () {
      $ctrl.typeOrganisation = typeOrganisationConverterService.convertIntToTypeOrganisation($ctrl.typeOrganisationId);
    };


  }
})();