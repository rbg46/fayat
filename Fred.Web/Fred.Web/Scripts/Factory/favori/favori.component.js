(function () {
  'use strict';

  angular.module('Fred')
    .component('favoriComponent', {
      templateUrl: '/Scripts/Factory/favori/favori.html',
      bindings: {
        resolve: '<',
        close: '&',
        dismiss: '&'
      },
      controller: 'favoriComponentController'
    });

  angular.module('Fred').controller('favoriComponentController', favoriComponentController);

  favoriComponentController.$inject = ['$document', '$timeout', 'favorisService', 'Notify', '$rootScope'];

  function favoriComponentController($document, $timeout, favorisService, Notify, $rootScope) {
    var $ctrl = this;

    // Variables
    angular.extend($ctrl, {
      defaultColor: "#1F55A0",
      favoriForm: {},
      favori: null
    });

    $ctrl.$onInit = function () {
      angular.element($document[0].querySelector('#favoricolorselector')).colorselector();
      $ctrl.resources = $ctrl.resolve.resources;
      $ctrl.favori = $ctrl.resolve.favori;
    };

    /*
     * @description Ajout d'un nouveau favori
     */
    $ctrl.handleSave = function () {
      if ($ctrl.favori !== null) {
        $ctrl.favori.Libelle = $ctrl.Libelle;
        $ctrl.favori.Couleur = !$ctrl.Couleur ? $ctrl.defaultColor : $ctrl.Couleur;
      }

      favorisService.Add($ctrl.favori).then(function (value) {
        $rootScope.$broadcast('add.favori');
        Notify.message(resources.Global_Notification_Enregistrement_Success);
        $ctrl.close({ $value: value });
      })
      .catch(function (reason) { $ctrl.close({ $value: reason }); });
    };

    /*
     * @description Annulation d'ajout en favori
     */
    $ctrl.handleCancel = function () {
      actionInitFormFavori();
      $ctrl.dismiss({ $value: "Dismiss" });
    };

    /*
     * @description Initialisation du formulaire
     */
    function actionInitFormFavori() {
      $ctrl.Libelle = null;
      $ctrl.Couleur = $ctrl.defaultColor;
      $ctrl.favoriForm.$setPristine();
    }
  };

})();