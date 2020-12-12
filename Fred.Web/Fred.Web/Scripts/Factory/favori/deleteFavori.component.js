(function () {
    'use strict';
  
    angular.module('Fred').component('deleteFavoriComponent', {
        templateUrl: '/Scripts/Factory/favori/deleteFavori.html',
        bindings: {
          resolve: '<',
          close: '&',
          dismiss: '&'
        },
        controller: 'deleteFavoriComponentController'
    });
  
    angular.module('Fred').controller('deleteFavoriComponentController', deleteFavoriComponentController);
  
    deleteFavoriComponentController.$inject = ['favorisService','$rootScope', 'Notify'];
  
    function deleteFavoriComponentController(favorisService, $rootScope, Notify) {
      var $ctrl = this;

        $ctrl.handleDelete = handleDelete;
        $ctrl.handleCancel = handleCancel;
  
      $ctrl.$onInit = function () {
        $ctrl.resources = $ctrl.resolve.resources;
        $ctrl.favori = $ctrl.resolve.favori;
      };

      function handleDelete(){
        favorisService.Delete($ctrl.favori.FavoriId).then(function (value) {
            $rootScope.$broadcast('delete.favori', {favori : $ctrl.favori});
            Notify.message(resources.Global_Notification_Suppression_Success);
            $ctrl.close({ $value: value });
        })
        .catch(function (reason) { 
            Notify.error(resources.Favoris_Notify_Suppression_Error);
            $ctrl.close({ $value: reason }); 
        });
      }

      function handleCancel(){
        $ctrl.dismiss({ $value: "Dismiss" });
      }
    }

  })();