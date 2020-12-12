(function () {
  'use strict';

  angular
    .module('Fred')
    .component('societeImageSelector', {
      templateUrl: '/Areas/Societe/Scripts/societe-image-selector.html',
      bindings: {
        resources: '<',
        isOpen: '=',
        isLogo: '<',
        societe: '<'
      },
      controller: 'ImageSelectorController'
    });

  angular.module('Fred').controller('ImageSelectorController', ImageSelectorController);

  ImageSelectorController.$inject = ['ProgressBar', 'SocieteService', 'Notify', 'UserService'];

  function ImageSelectorController(ProgressBar, SocieteService, Notify, UserService) {

    var $ctrl = this;
    var savedImage = null;
    $ctrl.isBusy = false;
    $ctrl.imageSelected = null;
    $ctrl.images = [];

    //////////////////////////////////////////////////////////////////
    // Déclaration des fonctions publiques                          //
    //////////////////////////////////////////////////////////////////


    $ctrl.canSave = canSave;
    $ctrl.save = save;
    $ctrl.selectImage = selectImage;
    $ctrl.closePanel = closePanel;
    $ctrl.cancel = cancel;


    //////////////////////////////////////////////////////////////////
    // Init                                                         //
    //////////////////////////////////////////////////////////////////

    $ctrl.$onInit = function () {

    };

    $ctrl.$onChanges = function (changesObj) {
      if ((changesObj.isLogo || changesObj.societe) && $ctrl.isOpen === true) {
        onLoad();
      }
    };


    //////////////////////////////////////////////////////////////////
    // Evenements                                                   //
    //////////////////////////////////////////////////////////////////


    //////////////////////////////////////////////////////////////////
    // Handlers                                                     //
    //////////////////////////////////////////////////////////////////
    function canSave() {
      if ($ctrl.isBusy) {
        return false;
      }
      if (!$ctrl.imageSelected) {
        return false;
      }
      if ($ctrl.imageSelected.ImageId) {
        return true;
      }
      return false;
    }

    function save() {
      if ($ctrl.societe && $ctrl.societe.SocieteId !== 0) {
        if ($ctrl.isLogo === false) {
          actionUpdateLoginImage().finally(setNotBusy);
        }
        if ($ctrl.isLogo === true) {
          actionUpdateLogoImage().finally(setNotBusy);
        }
      }
    }

    /*
     * Change l'image selectionnée
     */
    function selectImage(image) {
      $ctrl.imageSelected = image;
    }

    /*
     * Ferme le panel du choix des images
     */
    function closePanel() {
      $ctrl.isOpen = false;
    }

    /*
     * annule la selection 
     */
    function cancel() {
      $ctrl.imageSelected = savedImage;
    }

    //////////////////////////////////////////////////////////////////
    // Actions                                                      //
    //////////////////////////////////////////////////////////////////

    /*
     * ouvre une popup pour creer ou mettre à jour une ressource
     */
    function onLoad() {
      $ctrl.images = [];
      $ctrl.imageSelected = null;
      if ($ctrl.societe && $ctrl.societe.SocieteId !== 0) {

        if ($ctrl.isLogo === true) {
          $ctrl.isBusy = true;
          actionGetLogoImage()
            .then(actionGetLogoImages)
            .finally(setNotBusy);
        }
        if ($ctrl.isLogo === false) {
          $ctrl.isBusy = true;
          actionGetLoginImage()
            .then(actionGetLoginImages)
            .finally(setNotBusy);
        }
      }

    }

    /*
     * Met isBusy a false
     */
    function setNotBusy() {
      $ctrl.isBusy = false;
    }

    /*
     * @description Récupère l'image de login de la societe
     */
    function actionGetLoginImage() {
      return SocieteService.GetLoginImage({ societeId: $ctrl.societe.SocieteId }).$promise
        .then(function (value) {
          $ctrl.imageSelected = value;
          savedImage = angular.copy($ctrl.imageSelected);
        })
      .catch(Notify.defaultError);
    }

    /*
  * @description Récupère le logo de la societe
  */
    function actionGetLogoImage() {
      return SocieteService.GetLogoImage({ societeId: $ctrl.societe.SocieteId }).$promise
        .then(function (value) {
          $ctrl.imageSelected = value;
          savedImage = angular.copy($ctrl.imageSelected);

        })
      .catch(Notify.defaultError);
    }

    /*
    * @description Récupère une les logos des societes
    */
    function actionGetLogoImages() {
      return SocieteService.GetLogoImages().$promise
        .then(function (value) {
          $ctrl.images = value;
        })
      .catch(Notify.defaultError);
    }

    /*
   * @description Récupère une les logos des societes
   */
    function actionGetLoginImages() {
      return SocieteService.GetLoginImages().$promise
        .then(function (value) {
          $ctrl.images = value;
        })
      .catch(Notify.defaultError);
    }

    /*
   * @description met a jour le login pour une societe
   */
    function actionUpdateLoginImage() {
      $ctrl.isBusy = true;
      return SocieteService.UpdateLoginImage({ societeId: $ctrl.societe.SocieteId, imageId: $ctrl.imageSelected.ImageId }, null).$promise
        .then(function (value) {
          Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
          savedImage = angular.copy($ctrl.imageSelected);
          UserService.setLoginImage($ctrl.societe.SocieteId, $ctrl.imageSelected);
        })
      .catch(Notify.defaultError);
    }

    /*
  * @description met a jour le logo pour une societe
  */
    function actionUpdateLogoImage() {
      $ctrl.isBusy = true;
      return SocieteService.UpdateLogoImage({ societeId: $ctrl.societe.SocieteId, imageId: $ctrl.imageSelected.ImageId }, null).$promise
        .then(function (value) {
          Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
          savedImage = angular.copy($ctrl.imageSelected);
          UserService.setLogoImage($ctrl.societe.SocieteId, $ctrl.imageSelected);
        })
      .catch(Notify.defaultError);
    }

  }
})();