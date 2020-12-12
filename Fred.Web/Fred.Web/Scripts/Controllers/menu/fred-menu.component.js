/// <reference path="../../module/authorization/permissionKeys.js" />

(function () {
    'use strict';

    angular
      .module('Fred')
      .component('fredMenu', {
          templateUrl: '/Scripts/Controllers/menu/fred-menu.html',
          bindings: {
              user: '<',
              headerMobileIsOpen: '=',
              modeMenu: '=',
              modernMenuIsOpen: '=',
              modulesAutorized: '<',
              menuAdminIsVisible: '<',
              backgroundImgUrl: '<'
          },
          controller: 'fredMenuController'
      });

    angular.module('Fred').controller('fredMenuController', fredMenuController);

    fredMenuController.$inject = ['$scope'];

    function fredMenuController($scope) {

        var $ctrl = this;
        $ctrl.resources = resources;
        $ctrl.openCloseModernMenu = openCloseModernMenu;

        $ctrl.$onInit = function () {
        };

        /**
        * Affichage du menu mode Modern
        */
        function openCloseModernMenu() {
            $scope.$emit('open.close.modern.menu');
        }
    }
})();
