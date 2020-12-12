(function () {
  'use strict';

  angular
    .module('Fred')
    .component('fredApplicationHeaderMobile', {
      templateUrl: '/Scripts/Controllers/Header/fred-application-header-mobile.html',
      bindings: {
        resources: '<',
        user: '<',
        headerMobileIsOpen: '='
      },
      controller: 'fredApplicationHeaderMobileController' 
    });

  angular.module('Fred').controller('fredApplicationHeaderMobileController', fredApplicationHeaderMobileController);

  fredApplicationHeaderMobileController.$inject = ['$scope'];

  function fredApplicationHeaderMobileController($scope) {

    var $ctrl = this;
                
    $ctrl.logoSrc = null;
       

    //////////////////////////////////////////////////////////////////
    // Déclaration des fonctions publiques                          //
    //////////////////////////////////////////////////////////////////
    $ctrl.toogleMobileMenu = toogleMobileMenu;

    //////////////////////////////////////////////////////////////////
    // Init                                                         //
    //////////////////////////////////////////////////////////////////

    $ctrl.$onInit = function () {       
    };

    //////////////////////////////////////////////////////////////////
    // Evenements                                                   //
    //////////////////////////////////////////////////////////////////

        

    //////////////////////////////////////////////////////////////////
    // Handlers                                                     //
    //////////////////////////////////////////////////////////////////
    function toogleMobileMenu() {
      $ctrl.headerMobileIsOpen = !$ctrl.headerMobileIsOpen;
    }

    //////////////////////////////////////////////////////////////////
    // Actions                                                      //
    //////////////////////////////////////////////////////////////////

     

  }
})();