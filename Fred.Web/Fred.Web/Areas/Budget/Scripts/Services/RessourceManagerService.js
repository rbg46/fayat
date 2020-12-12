/*
 * Ce service sert a copier coller des ressourceTache d'une tache a une autre.
 */
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('RessourceManagerService', RessourceManagerService);

  RessourceManagerService.$inject = ['ParametrageReferentielEtenduService', 'CiManagerService'];

  function RessourceManagerService(ParametrageReferentielEtenduService, CiManagerService) {



    var service = {
      createNewRessourceWithRessourceParent: createNewRessourceWithRessourceParent
    };
    return service;

    //////////////////////////////////////////////////////////////////
    // PUBLIC METHODES                                              //
    //////////////////////////////////////////////////////////////////

    function createNewRessourceWithRessourceParent(ressourceParent, deviseSelected) {
      var newRessource = {};
      angular.copy(ressourceParent, newRessource);
      newRessource.RessourceId = 0;
      newRessource.ParentId = ressourceParent.RessourceId;
      newRessource.RessourcesEnfants = [];
      newRessource.ReferentielEtendus['0'].RessourceId = 0;
      newRessource.ReferentielEtendus['0'].ReferentielEtenduId = 0;
      newRessource.ReferentielEtendus['0'].ParametrageReferentielEtendus = [];
      newRessource.Code = ressourceParent.Code + '-' + (ressourceParent.RessourcesEnfants.length + 1);
      newRessource.Libelle = ressourceParent.Libelle;

      var lastParamForDevise = ParametrageReferentielEtenduService.getLastParametrageReferentielEtenduForDeviseByRessource(ressourceParent, deviseSelected);
      var defaultUnite = ParametrageReferentielEtenduService.getDefaultUnite(null, ressourceParent, deviseSelected);
      var ciSelected = CiManagerService.getCi();
      
      var devise = deviseSelected;
      var parametrageReferentielEtendu = {};
      parametrageReferentielEtendu.ParametrageReferentielEtenduId = 0;
      parametrageReferentielEtendu.ReferentielEtenduId = 0;
      parametrageReferentielEtendu.Unite = lastParamForDevise ? lastParamForDevise.Unite : defaultUnite;
      parametrageReferentielEtendu.UniteId = lastParamForDevise ? lastParamForDevise.UniteId : defaultUnite.UniteId;
      parametrageReferentielEtendu.Devise = devise;
      parametrageReferentielEtendu.DeviseId = devise.DeviseId;
      parametrageReferentielEtendu.Montant = 0;
      parametrageReferentielEtendu.OrganisationId = ciSelected.OrganisationId;
      newRessource.ReferentielEtendus['0'].ParametrageReferentielEtendus.push(parametrageReferentielEtendu);

      return newRessource;
    }
  }
})();