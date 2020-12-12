(function (angular) {
  'use strict';

  angular.module('Fred').controller('ValorisationController', ValorisationController);

  ValorisationController.$inject = ['$scope', '$window', '$filter', '$timeout', 'ValorisationService', 'Notify', 'ProgressBar', 'fredSubscribeService'];

  function ValorisationController($scope, $window, $filter, $timeout, ValorisationService, Notify, ProgressBar, fredSubscribeService) {
    var $ctrl = this;
    $ctrl.busy = false;


    // méthodes exposées
    $ctrl.changePeriode = changePeriode;
    $ctrl.changeCI = changeCI;

    init();

    /**
     * Initialisation du controller.
     * 
     */
    function init() {
      $($window).resize(function () {
        FredToolBox.manageTableResize($('#tableFixed'), $window);
      });

      // Instanciation Objet Ressources
      $ctrl.resources = resources;
      $ctrl.valorisationList = [];
      $ctrl.filter = {};
      $ctrl.search = "";

    }

    return $ctrl;

    /*
     * -------------------------------------------------------------------------------------------------------------
     *                                            INITIALISATION
     * -------------------------------------------------------------------------------------------------------------
     */
    function actionLoadData() {
      if ($ctrl.ci && $ctrl.periode) {
        if ($ctrl.busy) {
          return;
        }
        $ctrl.busy = true;
        ProgressBar.start();
        ValorisationService.GetList($filter('date')($ctrl.periode, 'MM-dd-yyyy'), $ctrl.ci.CiId)
            .then(LoadListValorisation)
            .catch(function (error) {
              console.log(error);
            })
            .finally(function () {
              $ctrl.busy = false;
              ProgressBar.complete();
            });
      }
    }

    function changePeriode() {
      if ($ctrl.periode) {
        actionLoadData();
      }
    }

    function changeCI() {
      if ($ctrl.ci) {
        actionLoadData();
      }
    }

    function LoadListValorisation(getResult) {
      $ctrl.valorisationList = getResult.data;
    } 


  }
}(angular));