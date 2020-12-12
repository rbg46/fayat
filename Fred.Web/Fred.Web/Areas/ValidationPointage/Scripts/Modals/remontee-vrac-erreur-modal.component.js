(function (angular) {
  'use strict';

  var remonteeVracErreurComponent = {
    templateUrl: '/Areas/ValidationPointage/Scripts/Modals/remontee-vrac-erreur-modal.html',
    bindings: {
      resolve: '<',
      close: '&',
      dismiss: '&'
    },
    controller: remonteeVracErreurController
  };

  angular.module('Fred').component('remonteeVracErreurComponent', remonteeVracErreurComponent);

  angular.module('Fred').controller('remonteeVracController', remonteeVracErreurController);

  remonteeVracErreurController.$inject = ['Notify', '$filter', 'ProgressBar', 'ValidationPointageService'];

  function remonteeVracErreurController(Notify, $filter, ProgressBar, ValidationPointageService) {
    var $ctrl = this;

    angular.extend($ctrl, {
      // Fonctions
      handleExportPdf: handleExportPdf,
      handleClose: handleClose,

      // Variables 
      erreurCount: 0,
      remonteeVracErreurList: [],
      remonteeVrac: null,
      period: null,
      busy: false,
      searchText: "",
      paging: { pageSize: 20, page: 1, hasMorePage: true }
    });

    /*
     * Initilisation du controller de la modal
     */
    $ctrl.$onInit = function () {
      $ctrl.resources = $ctrl.resolve.resources;
      $ctrl.period = $filter('date')($ctrl.resolve.periode, 'MM-yyyy');
      $ctrl.remonteeVrac = $ctrl.resolve.remonteeVrac;

      FredToolBox.bindScrollEnd('#personnel-erreur-list', actionLoadMore);

      actionGetRemonteeVracErreurList(true);
    };

    /**
     * Gestion de l'export PDF des erreurs de remontée vrac
     */
    function handleExportPdf() {
      ValidationPointageService.ExportPdfRemonteeVracErreur($ctrl.remonteeVrac.RemonteeVracId);
    }

    /* 
     * @function handleClose()
     * @description Fermeture modal
     */
    function handleClose() {
      $ctrl.dismiss({ $value: 'cancel' });
    }

    /*
     * @description Récupère la liste des personnels avec chacun ses erreurs
     */
    function actionGetPersonnelErreurList(firstLoad) {
      //$scope.$emit("controlePointageErreurCtrl.TotalErreurCount", 0);
      return actionGetRemonteeVracErreurList(firstLoad);
    }

    /*
     * @description Récupère la liste des personnels avec leurs erreurs de Remontée vrac
     */
    function actionGetRemonteeVracErreurList(firstLoad) {
      if ($ctrl.remonteeVrac) {
        $ctrl.busy = true;

        if (firstLoad) {
          $ctrl.remonteeVracErreurList = [];
          $ctrl.paging.page = 1;
        }

        ProgressBar.start();
        return ValidationPointageService.GetRemonteeVracErreurList({ remonteeVracId: $ctrl.remonteeVrac.RemonteeVracId, page: $ctrl.paging.page, pageSize: $ctrl.paging.pageSize, searchText: $ctrl.searchText }).$promise
          .then(function (value) {
            var data = value;
            angular.forEach(data.Erreurs, function (val) {
              angular.forEach(val.Erreurs, function (e, key) {
                e.DateDebut = $filter('toLocaleDate')(e.DateDebut);
                e.DateFin = $filter('toLocaleDate')(e.DateFin);

                // Formalisation du message d'erreur de la remontée vrac                                
                e.Message = String.format($ctrl.resources.VPWeb_RemonteeVracErreur, e.CodeAbsenceAnael, e.CodeAbsenceFred);
              });
              $ctrl.remonteeVracErreurList.push(val);
            });

            $ctrl.erreurCount = value.TotalErreurCount;            
            $ctrl.busy = false;
            $ctrl.paging.hasMorePage = value.Erreurs.length !== $ctrl.paging.pageSize;
          })
          .catch(Notify.defaultError)
          .finally(ProgressBar.complete);
      }
    }

    /*
     * @function actionLoadMore()
     * @description Action Chargement de données supplémentaires (scroll end)
     */
    function actionLoadMore() {
      if (!$ctrl.busy && !$ctrl.paging.hasMorePage) {
        $ctrl.paging.page++;
        actionGetPersonnelErreurList(false);
      }
    }
    
  }

}(angular));