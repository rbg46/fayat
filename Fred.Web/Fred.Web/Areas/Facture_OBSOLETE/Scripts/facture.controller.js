(function (angular) {
  'use strict';

  angular.module('Fred').controller('FactureController', FactureController);

  FactureController.$inject = ['$q', '$filter', 'Notify', 'FactureService', 'ProgressBar', 'favorisService', 'favoriModal'];

  /**
   * Controller des Factures.
   *       
   * @param {any} $q $q
   * @param {any} $filter $filter
   * @param {any} Notify Notify
   * @param {any} FactureService FactureService
   * @param {any} ProgressBar ProgressBar   
   * @param {any} favorisService favorisService   
   * @param {any} favoriModal favoriModal   
   * @returns {FactureController} $ctrl
   */
  function FactureController($q, $filter, Notify, FactureService, ProgressBar, favorisService, favoriModal) {

    // assignation de la valeur du scope au controller pour les templates non mis à jour
    var $ctrl = this;

    // méthodes exposées
    angular.extend($ctrl, {
      handleLoadPage: handleLoadPage,
      handleSelectFacture: handleSelectFacture,
      handleScanFacture: handleScanFacture,
      handleReCacheeFacture: handleReCacheeFacture,
      handleDeCacheeFacture: handleDeCacheeFacture,
      handleSearch: handleSearch,
      handleFavori: handleFavori
    });

    init();

    return $ctrl;

    /**
     * Initialisation du controller.     
     */
    function init() {
      angular.extend($ctrl, {
        // Instanciation Objet Ressources
        resources: resources,

        busy: false,
        paging: { pageSize: 20, page: 1, hasMorePage: true },
        factureRapprochementURL: "/RapprochementFacture/RapprochementFacture/",
        factureScanURL: "",
        favori: [],
        FactureList: [],
        filters: {}
      });
      FredToolBox.bindScrollEnd('#containerTableauFacture', actionLoadMore);
    }


    /* -------------------------------------------------------------------------------------------------------------
     *                                            LISTE DES FACTURES
     * -------------------------------------------------------------------------------------------------------------
     */

    /*
     * @function handleLoadPage(FactureFavoriId)
     * @description Gestion du chargement de la liste des Facture
     */
    function handleLoadPage(FactureFavoriId) {
      var promise = {};

      // Chargement de la liste des Factures à partir d'un filtre vierge
      if (FactureFavoriId === null || FactureFavoriId === "") {
        promise = FactureService.GetFilter().$promise;
      }
        // Chargement de la liste des Factures à partir du filtre du favori
      else {
        promise = favorisService.GetById(FactureFavoriId);
      }      
      promise
        .then(actionGetFilter)
        .then(function (filter) { actionSearch(filter, true); })
        .catch(Notify.defaultError);
    }

    /* -------------------------------------------------------------------------------------------------------------
     *                                            HANDLERS
     * -------------------------------------------------------------------------------------------------------------
     */

    /*
     * @description Handler Redirection vers le rapprochement d'une Facture sélectionnée
     */
    function handleSelectFacture(selectedFacture) {
      actionGoToDetailFacture(selectedFacture);
    }

    /*
     * @description Handler de clic sur le bouton "Ouvrir le scan de la facture"
     */
    function handleScanFacture(selectedFacture) {
      actionGoToScanFacture(selectedFacture);
    }

    /*
     * @description Handler de clic sur le bouton "re-cacher la facture"
     */
    function handleReCacheeFacture(selectedFacture) {
      selectedFacture.Cachee = true;
      actionUpdateFacture(selectedFacture);
    }

    /*
     * @description Handler de clic sur le bouton "dé-cacher la facture"
     */
    function handleDeCacheeFacture(selectedFacture) {
      selectedFacture.Cachee = false;
      actionUpdateFacture(selectedFacture);
    }

    /*
     * @description Handler Bouton Appliquer les filtres
     */
    function handleSearch(filters) {
      actionSearch(filters, true);
    }

    /* -------------------------------------------------------------------------------------------------------------
     *                                            ACTIONS
     * -------------------------------------------------------------------------------------------------------------
     */

    /*
     * @function actionSearch(filters, firstLoad)
     * @description Action de recherche d'une Facture à partir du filtre
     */
    function actionSearch(filters, firstLoad) {
      ProgressBar.start();
      $ctrl.busy = true;
      if (firstLoad) {
        $ctrl.FactureList = [];
        $ctrl.paging.page = 1;
      }
      FactureService.Search({ page: $ctrl.paging.page, pageSize: $ctrl.paging.pageSize }, filters).$promise
       .then(function (value) {
         angular.forEach(value, function (val, key) {           
           val.CiCodeLibellesTooltip = val.CiCodeLibelles.join('\n');
           $ctrl.FactureList.push(val);
         });

         $ctrl.paging.hasMorePage = value.length !== $ctrl.paging.pageSize;
       })
       .catch(function (error) { console.log(error); })
       .finally(function () { $ctrl.busy = false; ProgressBar.complete(); });
    }

    /*
     * @function actionGoToDetailFacture(facture)
     * @description Redirection vers le rapprochement d'une Facture
     */
    function actionGoToDetailFacture(facture) {
      // TSA:  TODO à enlever, temporaire pour la démo du 03/10/2017
      //window.location.href = $ctrl.factureRapprochementURL + facture.FactureId;
      window.location.href = $ctrl.factureRapprochementURL;
    }

    /*
     * @function actionGoToScanFacture(facture)
     * @description Redirection vers le scan d'une Facture
     */
    function actionGoToScanFacture(facture) {
      FactureService.ScanURL(facture.FactureId).then(function (value) {
        $ctrl.Facture = value.data;
        window.open($ctrl.Facture.ScanUrl + facture.NoFMFI);
      });
    }

    /*
     * @function actionUpdateFacture(facture)
     * @description Mise à jour d'une Facture
     */
    function actionUpdateFacture(facture) {
      FactureService.Update(facture).$promise.then(function (value) {
        Notify.message(resources.Global_Notification_Enregistrement_Success);
      }).catch(function (error) {
        Notify.error(resources.Global_Notification_Error);
      });
    }

    /* 
     * @function    actionLoadMore()
     * @description Action Chargement de données supplémentaires (scroll end)
     */
    function actionLoadMore() {
      if (!$ctrl.busy && !$ctrl.paging.hasMorePage) {
        $ctrl.paging.page++;
        actionSearch($ctrl.filters, false);
      }
    }

    /*
     * @function actionGetFilter(filter)
     * @description Enregistre le filtre de recherche
     */
    function actionGetFilter(filters) {
      if (angular.equals($ctrl.filters, {})) {
        $ctrl.filters = filters ? filters : {};
      }
      return $ctrl.filters;
    }

    /* -------------------------------------------------------------------------------------------------------------
     *                                            FAVORI
     * -------------------------------------------------------------------------------------------------------------
     */

    /* 
     * @description Récupération d'un nouveau favori Facture vierge 
     */
    function actionGetNewFavori() {
      return favorisService.GetNew("Facture").then(function (value) {        
        $ctrl.favori = value;
        $ctrl.favori.Filtre = $ctrl.filters;
        return $ctrl.favori;
      });
    }

    /*
     * @description Gestion de l'ajout d'un favori
     */
    function handleFavori() {
      $q.when()
        .then(actionGetNewFavori)
        .then(function (favori) {
          favoriModal.open(resources, favori);          
        });
    }

  }


}(angular));