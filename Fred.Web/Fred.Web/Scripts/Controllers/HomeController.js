(function (angular) {
  'use strict';


  angular.module('Fred').controller('HomeController', HomeController);

  HomeController.$inject = ['$q', '$timeout', '$filter', 'Notify', 'ProgressBar', 'favorisService', 'favoriModal', '$rootScope'];

  /**
   * Controller des Codes Deplacement.
   * 
   * @param {any} $q $q
   * @param {any} $timeout $timeout
   * @param {any} $filter $filter
   * @param {any} Notify Notify
   * @param {any} ProgressBar ProgressBar      
   * @param {any} favorisService favorisService
   * @param {any} favoriModal favoriModal
   * @param {any} $rootScope $rootScope
   * @returns {HomeController} $ctrl
   */
  function HomeController($q, $timeout, $filter, Notify, ProgressBar, favorisService, favoriModal, $rootScope) {
    /*
     * -------------------------------------------------------------------------------------------------------------
     *                                            INITIALISATION
     * -------------------------------------------------------------------------------------------------------------
     */
    // assignation de la valeur du scope au controller pour les templates non mis à jour
    var $ctrl = this;

    // méthodes exposées
    angular.extend($ctrl, {
      handleOpenFavoriDetail: handleOpenFavoriDetail,
      handleSaveFavori: handleSaveFavori,
      handleDeleteFavori: handleDeleteFavori,
      handleOpenFavori: handleOpenFavori,
      handleCancelFavori: handleCancelFavori
    });

    init();

    return $ctrl;


    /**
     * Initialisation du controller.
     * 
     */
    function init() {
      ProgressBar.start();

      angular.extend($ctrl, {
        // Instanciation Objet Ressources
        resources: resources,
        favoris: {},
        isFavoriDetailOpened: false
      });

      $('#favoricolorselector').colorselector();

      actionLoadFavoriList();

      $rootScope.$on('login.changed', function (event, img) {
        $ctrl.backgroundImgUrl = img.Path;
      });

      ProgressBar.complete();

    }




    /*
     * -------------------------------------------------------------------------------------------------------------
     *                                            HANDLERS
     * -------------------------------------------------------------------------------------------------------------
     */

    /*
     * @description Ouverture panneau favori
     */
    function handleOpenFavoriDetail(favori) {
      $ctrl.isFavoriDetailOpened = true;
      $ctrl.formFavoris = favori;

      // TSA : Timeout car il y a déjà un apply en cours (cf. angular lifecycle). colorselector étant asynchrone, il demande un apply également
      // Il y a donc deux actions en même temps, l'ouverture du menu latéral et l'action de peupler la couleur. Il faut améliorer l'ouverture du ménu latéral
      // actuellement fait en CSS.
      $timeout(function () { $("#favoricolorselector").colorselector("setColor", favori.Couleur); }, 0, false);
    }

    /*
     * @description Annulation favori
     */
    function handleCancelFavori() {
      actionLoadFavoriList();
      $ctrl.isFavoriDetailOpened = false;
    }

    /*
     * @description Enregistrement du favori
     */
    function handleSaveFavori() {

      favorisService.Update($ctrl.formFavoris).then(function (value) {
        $ctrl.isFavoriDetailOpened = false;
        Notify.message(resources.Global_Notification_Enregistrement_Success);
      })
        .catch(function (reason) { console.log(reason); });
    }

    /*
     * @description Suppression d'un favori
     */
    function handleDeleteFavori() {

      favorisService.Delete($ctrl.formFavoris.FavoriId).then(function () {

        var fav = $filter('filter')($ctrl.favoris, { FavoriId: $ctrl.formFavoris.FavoriId }, true)[0];
        $ctrl.favoris.splice($ctrl.favoris.indexOf(fav), 1);
        $ctrl.isFavoriDetailOpened = false;
        Notify.message(resources.Global_Notification_Suppression_Success);

      })
        .catch(function (reason) { console.log(reason); });
    }


    /*
     * @description Redirection vers la page du favori
     */
    function handleOpenFavori(url, idFavori) {
      window.location.href = url + idFavori;
    }


    /*
     * -------------------------------------------------------------------------------------------------------------
     *                                            ACTIONS
     * -------------------------------------------------------------------------------------------------------------
     */

    /*
     * @description Chargement de la liste des favoris de l'utilisateur courant
     */
    function actionLoadFavoriList() {
      favorisService.GetList().then(function (value) {
        $ctrl.favoris = value;
      })
        .catch(function (reason) {
          console.log(reason);
        });
    }

  }



}(angular));

