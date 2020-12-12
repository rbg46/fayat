(function () {
  "use strict";


  angular.module('Fred').service('moduleArboStoreService', moduleArboStoreService);

  moduleArboStoreService.$inject = [];


  function moduleArboStoreService() {

    var store = {
      moduleSelected: null,
      fonctionnaliteSelected: null,
      page: 0,
      societeId: null,
      organisations: [],
      pole: null,
      societesInactives: [],
      initialRequestStarted: false
    };



    this.isModuleSelected = function () {
      return store.moduleSelected !== null;
    }

    /*
     * Permet de savoir si le context a changer 
     * exemple , la societe, le module selectionné ou la fonctionnalite selecteionnée
     */
    this.contextHasChanged = function (infoContext) {
      if (store.societeId != infoContext.societeId) {
        return true;
      }
      if (store.moduleSelected != infoContext.module) {
        return true;
      }
      if (store.fonctionnaliteSelected != infoContext.feature) {
        return true;
      }
      return false;
    }

    /*
     * Selection d'un module, d'une fonctionnalite et de la societe
     */
    this.changeContext = function (info) {

      store.organisations = [];
      store.pole = null;
      store.page = 0;
      store.societeId = info.societeId;

      if (store.moduleSelected != info.module) {
        store.moduleSelected = info.module;
      }
      if (store.fonctionnaliteSelected != info.feature) {
        store.fonctionnaliteSelected = info.feature;
      }
    }

    /*
     * Retourne une propriete du store
     */
    this.get = function (property) {
      return store[property];
    }


    /*
     * Retourne le prochain numero de page
     */
    this.getNextPage = function () {
      store.page = store.page + 1;
      return store.page;
    }


    this.requestArboStarted = function () {
      store.initialRequestStarted = true;// implementer le comportement
    }

    this.requestArboFinish = function () {
      store.initialRequestStarted = false;
    }

    this.saveOrganisationIdsOfSocietesInactives = function (societesInactives) {
      store.societesInactives = societesInactives;
    }


    /////////////////////////////////////////////////////////////////////////////////////////
    // RAJOUTE DES ORGANISATIONS A L ARBO                                                  //
    /////////////////////////////////////////////////////////////////////////////////////////

    /*
     * rajoute des organisations a l arbo
     */
    this.addOrganisations = function (organisations) {
      if (!store.pole) {
        store.pole = {};
        store.pole = organisations[0];
        store.organisations.push(organisations[0]);
      }

      for (var i = 0; i < organisations.length; i++) {
        var newOrga = organisations[i];
        markSocieteIsEnable(newOrga);
        newOrga.originalIsEnable = newOrga.IsEnable;

        if (newOrga.TypeOrganisationId !== ORGANISATION_TYPE.POLE) {

          var parent = findParent(newOrga, store.organisations);

          if (parent !== null) {
            if (!parent.children) {
              parent.children = [];
            }
            parent.children.push(newOrga);
            store.organisations.push(newOrga);
          } else {
            console.log("parent not found pereId: " + newOrga.PereId);
          }

        }
      }
    }

    /*
     * Trouve le parent dans l'arbre.
     * Pour toutes les organisation inferieure a une societe , le parent est la societe.
     */
    function findParent(organisation, organisations) {
      var pereId = organisation.PereId;
      var result = null;
      var exitDoorCount = 0; //evite la boucle infini si on ne trouve jamais le parent.
      var continuSearch = true;

      while (result === null && continuSearch) {
        var parents = organisations.filter(function (orga) {
          return orga.OrganisationId === pereId;
        });

        if (parents && parents.length > 0) {
          var parent = parents[0];
          if (parent.TypeOrganisationId === ORGANISATION_TYPE.PUO ||
            parent.TypeOrganisationId === ORGANISATION_TYPE.UO ||
            parent.TypeOrganisationId === ORGANISATION_TYPE.PUO ||
            parent.TypeOrganisationId === ORGANISATION_TYPE.ETABLISSEMENT ||
            parent.TypeOrganisationId === ORGANISATION_TYPE.CI) {
            pereId = parent.PereId;
          } else {
            result = parent;
          }
        }
        exitDoorCount += 1;

        if (exitDoorCount > 10) {
          console.log("parent not found pereId: " + pereId);
          continuSearch = false;
        }
      }
      return result;

    }

    /*
     * Marque la societe comme active ou incative pour un module ou un fonctionnalité.
     */
    function markSocieteIsEnable(organisation) {
      if (organisation.TypeOrganisationId === ORGANISATION_TYPE.SOCIETE) {
        var orgnaisationsWithSameId = store.societesInactives.filter(function (organisationInactiveId) {
          return organisation.OrganisationId === organisationInactiveId;
        });
        if (orgnaisationsWithSameId.length > 0) {
          organisation.IsEnable = false;
        } else {
          organisation.IsEnable = true;
        }
      }

    }

    /////////////////////////////////////////////////////////////////////////////////////////
    // SELECTION DES SOCIETES QUI ONT ETE CHANGE PAR L UTILISATEUR - ACTIVES & INACTIVES   //
    /////////////////////////////////////////////////////////////////////////////////////////

    /*
     * Recupere les societe qui sont devenues inactives apres l'initialisation de l'arbo.
     */
    this.getSocietesDisabledAfterInit = function () {
      return getSocietesAfterInit(isDisableSelector);
    }

    /*
   * Recupere les societe qui sont devenues actives apres l'initialisation de l'arbo.
   */
    this.getSocietesEnabledAfterInit = function () {
      return getSocietesAfterInit(isEnableSelector);
    }

    function isEnableSelector(organisation) {
      return organisation.IsEnable;
    }

    function isDisableSelector(organisation) {
      return organisation.IsEnable === false;
    }

    function getSocietesAfterInit(selector) {
      var result = [];
      for (var i = 0; i < store.organisations.length; i++) {
        var organisation = store.organisations[i];
        if (organisation.TypeOrganisationId === ORGANISATION_TYPE.SOCIETE) {
          if (organisation.originalIsEnable !== organisation.IsEnable) {
            if (selector(organisation)) {
              result.push(organisation.OrganisationId);
            }
          }
        }
      }
      return result;
    }

    /////////////////////////////////////////////////////////////////////////////////////////
    // SAUVEGARDE                                                                          //
    /////////////////////////////////////////////////////////////////////////////////////////

    this.arboSaveSuccess = function (response) {

      var societeDisabledWithSuccess = getSocietesAfterInit(isDisableSelector);
      // on inclut les societes inactives dans la liste des societes inactives.
      for (var i = 0; i < societeDisabledWithSuccess.length; i++) {
        var organisationId = societeDisabledWithSuccess[i];
        store.societesInactives.push(organisationId);
      }
      // on considere que l'attribut IsEnable est maintenant l'original.
      for (var j = 0; j < store.organisations.length; j++) {
        var organisation = store.organisations[j];
        organisation.originalIsEnable = organisation.IsEnable;
      }

    }

    /////////////////////////////////////////////////////////////////////////////////////////
    // CANCEL                                                                          //
    /////////////////////////////////////////////////////////////////////////////////////////

    this.cancel = function () {

      // on remet la valeur orignial sur l'attribut IsEnable.
      for (var i = 0; i < store.organisations.length; i++) {
        var organisation = store.organisations[i];
        if (organisation.TypeOrganisationId === ORGANISATION_TYPE.SOCIETE) {
          organisation.IsEnable = organisation.originalIsEnable;
        }
      }
    }

  };


})();


