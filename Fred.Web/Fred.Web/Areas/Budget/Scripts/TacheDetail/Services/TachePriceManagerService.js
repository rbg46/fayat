
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('TachePriceManagerService', TachePriceManagerService);

  TachePriceManagerService.$inject = ['$log', 'RessourceTacheDeviseManagerService', 'CiManagerService', 'ParametrageReferentielEtenduService'];

  function TachePriceManagerService($log, RessourceTacheDeviseManagerService, CiManagerService, ParametrageReferentielEtenduService) {

    var service = {

      getPriceOfParam: getPriceOfParam,
      getPriceOfRessourceTask: getPriceOfRessourceTask,
      /*
      * met un prix au ParametrageReferentielEtendu.
      * A besoin d'une tache pour selectionner une unite si on créer un ParametrageReferentielEtendu.
      * A besoin d'une RessourceTache pour selectionner le ReferentielEtenduId si on créer un ParametrageReferentielEtendu.
      * A besoin d'un prix.
      * A besoin d'une devise.
      */
      setPriceOfParam: setPriceOfParam,
      setPriceOfRessourceTask: setPriceOfRessourceTask,
      hasPriceForDevise: hasPriceForDevise,
      getPrixUnitaire: getPrixUnitaire
    };

    return service;

    //////////////////////////////////////////////////////////////////
    // PUBLICS METHODES                                             //
    //////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////
    //  PARAMETRAGES REFERENTIEL ETENDU                             //
    //////////////////////////////////////////////////////////////////

    function getPriceOfParam(ressourceTask, deviseSelected) {
      var result = null;
      var parametrageReferentielEtendu = ParametrageReferentielEtenduService.getLastParametrageReferentielEtenduForDevise(ressourceTask, deviseSelected);
      if (parametrageReferentielEtendu !== null) {
        result = parametrageReferentielEtendu.Montant;
      }
      return result;
    }

    /*
     * met un prix au ParametrageReferentielEtendu.
     * A besoin d'une tache pour selectionner une unite si on créer un ParametrageReferentielEtendu.
     * A besoin d'une RessourceTache pour selectionner le ReferentielEtenduId si on créer un ParametrageReferentielEtendu.
     * A besoin d'un prix.
     * A besoin d'une devise.
     */
    function setPriceOfParam(task, ressourceTask, price, deviseSelected) {
      setPriceOfParametrageReferentielEtenduForDevise(task, ressourceTask, price, deviseSelected);
    }

    function hasPriceForDevise(ressourceTask, deviseSelected) {
      var param = ParametrageReferentielEtenduService.getLastParametrageReferentielEtenduForDevise(ressourceTask, deviseSelected);
      if (param) {
        return true;
      }
      var ressourceTacheDevise = RessourceTacheDeviseManagerService.getRessourceTacheDeviseForDevise(ressourceTask, deviseSelected);
      if (ressourceTacheDevise !== null) {
        return true;
      }
      return false;
    }


    //////////////////////////////////////////////////////////////////
    //  RESSOURCES TACHES                                           //
    //////////////////////////////////////////////////////////////////

    function getPriceOfRessourceTask(ressourceTask, deviseSelected) {
      var result = null;
      var ressourceTacheDevise = RessourceTacheDeviseManagerService.getRessourceTacheDeviseForDevise(ressourceTask, deviseSelected);
      if (ressourceTacheDevise !== null) {
        result = ressourceTacheDevise.PrixUnitaire;
      }
      return result;
    }

    function setPriceOfRessourceTask(ressourceTask, price, deviseSelected) {
      RessourceTacheDeviseManagerService.setPriceOfRessourceTacheDevises(ressourceTask, price, deviseSelected);
    }

    //////////////////////////////////////////////////////////////////
    //  PARAMETRAGES REFERENTIEL ETENDU  && RESSOURCES TACHES       //
    //////////////////////////////////////////////////////////////////


    function getPrixUnitaire(ressourceTask, deviseSelected) {
      var ressourceTacheDevise = RessourceTacheDeviseManagerService.getRessourceTacheDeviseForDevise(ressourceTask, deviseSelected);
      if (ressourceTacheDevise) {
        var prix = RessourceTacheDeviseManagerService.getPriceOfRessourceTacheDevise(ressourceTask, deviseSelected);
        $log.log('Prix RT =>' + prix);
        return prix;
      } else {
        var price = getPriceOfParametrageReferentielEtenduForDevise(ressourceTask, deviseSelected);
        return price;
      }
    }


    //////////////////////////////////////////////////////////////////
    // PRIVATES  METHODES                                            //
    //////////////////////////////////////////////////////////////////



    function getPriceOfParametrageReferentielEtenduForDevise(ressourceTask, deviseSelected) {
      var result = 0;
      var parametrageReferentielEtendu = ParametrageReferentielEtenduService.getLastParametrageReferentielEtenduForDevise(ressourceTask, deviseSelected);
      if (parametrageReferentielEtendu !== null) {
        result = parametrageReferentielEtendu.Montant;
      }
      return result;
    }

    function setPriceOfParametrageReferentielEtenduForDevise(task, ressourceTask, price, deviseSelected) {
      var parametrageReferentielEtendu = getParametrageReferentielEtenduForDeviseAndCi(ressourceTask, deviseSelected);
      if (parametrageReferentielEtendu !== null) {
        parametrageReferentielEtendu.Montant = price;
        $log.log('Prix REF Ci <= ' + parametrageReferentielEtendu.Montant);
      } else {
        parametrageReferentielEtendu = ParametrageReferentielEtenduService.createParametrageReferentielEtenduForDevise(task, ressourceTask.Ressource, deviseSelected);
        parametrageReferentielEtendu.Montant = price;
        $log.log('Prix REF Ci (new) <= ' + parametrageReferentielEtendu.Montant);
      }

    }


    function getParametrageReferentielEtenduForDeviseAndCi(ressourceTask, deviseSelected) {

      var result = null;

      var parametrageReferentielEtendus = ressourceTask.Ressource.ReferentielEtendus["0"].ParametrageReferentielEtendus;

      var parametrageReferentielEtendu = parametrageReferentielEtendus.find(function (param) {
        var ciSelected = CiManagerService.getCi();
        return param.DeviseId === deviseSelected.DeviseId && param.OrganisationId === ciSelected.OrganisationId;
      });

      if (parametrageReferentielEtendu) {
        $log.log('il y a un param etendu au niveau du ci ');
        result = parametrageReferentielEtendu;
      } else {
        $log.log('il y a pas de param etendu au niveau du ci ');
        result = null;
      }

      return result;
    }


  }
})();

