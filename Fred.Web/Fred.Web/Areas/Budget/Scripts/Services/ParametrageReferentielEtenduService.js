﻿
/*
 * 
 */
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('ParametrageReferentielEtenduService', ParametrageReferentielEtenduService);

  ParametrageReferentielEtenduService.$inject = ['CiManagerService'];

  function ParametrageReferentielEtenduService(CiManagerService) {



    var service = {
      /*
      * Determine si la ressource a moins un ParametrageReferentielEtendu(s)
      */
      hasAnyParametrageReferentielEtendu: hasAnyParametrageReferentielEtendu,
      /*
      * Retourne le ParametrageReferentielEtendu du plus bas niveau a partir d'une ressourceTache et pour une devise.
      */
      getLastParametrageReferentielEtenduForDevise: getLastParametrageReferentielEtenduForDevise,
      /*
      * Retourne le ParametrageReferentielEtendu du plus bas niveau a partir d'une ressource et pour une devise.
      */
      getLastParametrageReferentielEtenduForDeviseByRessource: getLastParametrageReferentielEtenduForDeviseByRessource,
      /*
      * Retourne les devises des ParametrageReferentielEtendu(s) a partir d'une RessourceTache
      */
      getDevisesOfParametrageReferentielEtendus: getDevisesOfParametrageReferentielEtendus,
      /*
      * Retourne les devises des ParametrageReferentielEtendu(s) a partir d'une Ressource
      */
      getDevisesOfParametrageReferentielEtendusByRessource: getDevisesOfParametrageReferentielEtendusByRessource,
      /*
       * Creer un ParametrageReferentielEtendu pour une devise donnée.
       */
      createParametrageReferentielEtenduForDevise: createParametrageReferentielEtenduForDevise,
      /*
      * Retourne une unite par default.
      * 
      * Si il y a une unite dans les ParametrageReferentielEtendu(s), on la prend.
      * Sinon on regarde au niveau de la tache,
      * Sinon on renvoie une unité 'Sans Unité'
      */
      getDefaultUnite: getDefaultUnite
    };
    return service;

    //////////////////////////////////////////////////////////////////
    // PUBLIC METHODES                                              //
    //////////////////////////////////////////////////////////////////

    /*
     * Determine si la ressource a moins un ParametrageReferentielEtendu(s) qui est stocké en base.
     */
    function hasAnyParametrageReferentielEtendu(ressource) {

      var params = ressource.ReferentielEtendus["0"].ParametrageReferentielEtendus;
      var hasMinimum = params !== null && params !== undefined && params.length > 0;
      if (!hasMinimum) {
        return false;
      }
      for (var i = 0; i < params.length; i++) {
        var param = params[i];
        if (param.ParametrageReferentielEtenduId !== 0) {
          return true;
        }
      }
      return false;

    }

    /*
    * Retourne le ParametrageReferentielEtendu du plus bas niveau a partir d'une ressourceTache et pour une devise.
    */
    function getLastParametrageReferentielEtenduForDevise(ressourceTask, deviseSelected) {
      var parametrageRefInSameDevise = getLastParametrageReferentielEtenduForDeviseByRessource(ressourceTask.Ressource, deviseSelected);
      return parametrageRefInSameDevise;
    }


    /*
    * Retourne le ParametrageReferentielEtendu du plus bas niveau a partir d'une ressource et pour une devise.
    */
    function getLastParametrageReferentielEtenduForDeviseByRessource(ressource, deviseSelected) {

      var result = null;

      var parametrageReferentielEtendus = ressource.ReferentielEtendus["0"].ParametrageReferentielEtendus;

      var parametragesReferentielEtenduInSameDevise = parametrageReferentielEtendus.filter(function (param) {
        return param.DeviseId === deviseSelected.DeviseId;
      });

      for (var i = 0; i < parametragesReferentielEtenduInSameDevise.length; i++) {
        var parametrageRefInSameDevise = parametragesReferentielEtenduInSameDevise[i];
        var organisation = parametrageRefInSameDevise.Organisation;
        var ciSelected = CiManagerService.getCi();
        if (parametrageRefInSameDevise.OrganisationId === ciSelected.OrganisationId) {
          return parametrageRefInSameDevise;
        }
        if (result === null) {
          result = parametrageRefInSameDevise;
          continue;
        }
        if (result.Organisation.TypeOrganisationId < organisation.TypeOrganisationId) {
          result = parametrageRefInSameDevise;
        }
      }

      return result;
    }

    /*
     * Retourne les devises des ParametrageReferentielEtendu(s) a partir d'une RessourceTache
     */
    function getDevisesOfParametrageReferentielEtendus(ressourceTask) {
      var devicesDistinct = getDevisesOfParametrageReferentielEtendusByRessource(ressourceTask.Ressource);
      return devicesDistinct;
    }


    /*
    * Retourne les devises des ParametrageReferentielEtendu(s) a partir d'une Ressource
    */
    function getDevisesOfParametrageReferentielEtendusByRessource(ressource) {
      var devicesDistinct = [];
      var parametrageReferentielEtendus = ressource.ReferentielEtendus["0"].ParametrageReferentielEtendus;

      var devices = parametrageReferentielEtendus.map(function (param) {
        return param.Devise;
      });

      var devicesIds = devices.map(function (param) {
        return param.DeviseId;
      });


      var devicesIdsDistinct = devicesIds.filter(onlyUnique);

      for (var i = 0; i < devicesIdsDistinct.length; i++) {
        var deviseId = devicesIdsDistinct[i];
        var devise = devices.find(function (d) {
          return d.DeviseId === deviseId;
        });
        devicesDistinct.push(devise);
      }
      return devicesDistinct;
    }


    /*
    * Creer un ParametrageReferentielEtendu pour une devise donnée.
    */
    function createParametrageReferentielEtenduForDevise(task, ressource, deviseSelected) {
      var unite = getDefaultUnite(task, ressource, deviseSelected);
      var ciSelected = CiManagerService.getCi();
      var parametrageReferentielEtendu = {
        ParametrageReferentielEtenduId: 0,
        ReferentielEtenduId: ressource.ReferentielEtendus["0"].ReferentielEtenduId,
        Unite: unite,
        UniteId: unite.UniteId,
        Montant: 0,
        DeviseId: deviseSelected.DeviseId,
        Devise: deviseSelected,
        OrganisationId: ciSelected.OrganisationId
      };
      ressource.ReferentielEtendus["0"].ParametrageReferentielEtendus.push(parametrageReferentielEtendu);

      return parametrageReferentielEtendu;
    }


    /*
  * Retourne une unite par default.
  * 
  * Si il y a une unite dans les ParametrageReferentielEtendu(s), on la prend.
  * Sinon on regarde au niveau de la tache,
  * Sinon on renvoie une unité 'Sans Unité'
  */
    function getDefaultUnite(task, ressource, deviseSelected) {

      var lastParametrageReferentielEtenduForDevise = getLastParametrageReferentielEtenduForDeviseByRessource(ressource, deviseSelected);
      if (lastParametrageReferentielEtenduForDevise !== null && lastParametrageReferentielEtenduForDevise.Unite) {
        return lastParametrageReferentielEtenduForDevise.Unite;
      }

      var devises = getDevisesOfParametrageReferentielEtendusByRessource(ressource);
      for (var i = 0; i < devises.length; i++) {
        var param = getLastParametrageReferentielEtenduForDeviseByRessource(ressource, devises[i]);
        if (param !== null && param.Unite) {
          return param.Unite;
        }
      }

      if (task !== null && task.Unite) {
        return task.Unite;
      }

      return {
        UniteId: 1,
        Code: '/-',
        Libelle: 'Sans Unité'
      };
    }

    //////////////////////////////////////////////////////////////////
    // PRIVATES METHODES                                            //
    //////////////////////////////////////////////////////////////////


    function onlyUnique(value, index, self) {
      return self.indexOf(value) === index;
    }



  }
})();

