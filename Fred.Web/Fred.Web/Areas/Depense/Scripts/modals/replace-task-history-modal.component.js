(function (angular) {
  'use strict';

  angular.module('Fred').component('replaceTaskHistoryComponent', {
    templateUrl: '/Areas/Depense/Scripts/modals/replace-task-history-modal.html',
    bindings: {
      resolve: '<',
      close: '&',
      dismiss: '&'
    },
    controller: 'ReplaceTaskHistoryComponentController'
  });

  angular.module('Fred')
    .controller('ReplaceTaskHistoryComponentController', ReplaceTaskHistoryComponentController);


  ReplaceTaskHistoryComponentController.$inject = ['RemplacementTacheService', 'ProgressBar', 'confirmDialog', '$q'];

  function ReplaceTaskHistoryComponentController(RemplacementTacheService, ProgressBar, confirmDialog, $q) {
    var $ctrl = this;

    angular.extend($ctrl, {
      // Fonctions            
      handleCancel: handleCancel,
      handleValidate: handleValidate,
      handleDelete: handleDelete
    });

    /*
     * Initilisation du controller de la modal
     */
    $ctrl.$onInit = function () {
      $ctrl.resources = $ctrl.resolve.resources;
      $ctrl.selectedDepense = $ctrl.resolve.selectedDepense;
      $ctrl.parent = $ctrl.resolve.scope;

      $q.when()
        .then(ProgressBar.start)
        .then(function () { return $ctrl.selectedDepense.GroupeRemplacementTacheId; })
        .then(actionGetHistory)
        .finally(ProgressBar.complete);
    };


    /*
     * @function Récupère l'historique des remplacements
     * @param groupeRemplacementTacheId L'id du groupe de remplacement
     */
    function actionGetHistory(groupeRemplacementTacheId) {
      return RemplacementTacheService.GetRemplacementTacheHistory(groupeRemplacementTacheId)
        .then(function (response) {
          $ctrl.history = response.data;
          $ctrl.history.forEach(function (t) { t.canBeDeleted = false; });

          // trie de la liste par RangRemplacement
          $ctrl.history.sort(function (a, b) { return a.RangRemplacement - b.RangRemplacement; });

          // Seulement le dernier remplacement peut être supprimé si nb > 1
          if ($ctrl.history.length > 1) {
            var lastElement = $ctrl.history[$ctrl.history.length - 1];
            lastElement.canBeDeleted = !lastElement.IsPeriodeCloturee && lastElement.Annulable;
          }
        })
        .catch(function (error) { console.log(error); });
    }

    /**
     * validation d'export
     */
    function handleValidate() {
      $ctrl.dismiss({ $value: 'ok' });
      if ($ctrl.remplacementDeleted) {
        $ctrl.parent.handleSearch();
      }
    }

    /*
     * validation d'export
     * @function handleDelete ()
     * @param remplacementTacheId L'id de la tache de remplacement
     */
    function handleDelete(remplacementTacheId) {
      confirmDialog.confirm($ctrl.resources, $ctrl.resources.RT_DeleteTache_Confirm)
        .then(function () {
         
          $q.when()
            .then(ProgressBar.start)
            .then(function () { return remplacementTacheId; })
            .then(actionDelete)
            .finally(ProgressBar.complete);

        });
    }

    /* 
     * @function handleCancel ()
     * @description Quitte la modal
     */
    function handleCancel() {
      $ctrl.dismiss({ $value: 'cancel' });
      if ($ctrl.remplacementDeleted) {
        $ctrl.parent.handleSearch();
      }
    }
    
    function actionDelete(remplacementTacheId) {
      var firstElement = $ctrl.history[0];

      return RemplacementTacheService.DeleteRemplacementTache(remplacementTacheId)
        .then(function () {
          $ctrl.remplacementDeleted = true;
          return $ctrl.selectedDepense.GroupeRemplacementTacheId;
        })
        .then(actionGetHistory)
        .then(function () {
          if ($ctrl.history.length === 0) {
            $ctrl.history.push(firstElement);
          }
        });
    }
  }
}(angular));