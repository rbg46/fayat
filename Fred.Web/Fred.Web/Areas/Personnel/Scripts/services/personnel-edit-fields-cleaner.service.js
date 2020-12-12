/*
 * Ce service sert au nettoyage des champs sur la fiche personnel
 */
(function () {
  'use strict';

  angular.module('Fred').service('PersonnelEditFieldsCleanerService', PersonnelEditFieldsCleanerService);

  function PersonnelEditFieldsCleanerService() {

    var service = {
      removeSociete: removeSociete,
      removeRessource: removeRessource,
      removeMateriel: removeMateriel,
      removeEtablissementPaie: removeEtablissementPaie,
      removeEtablissementRattachement: removeEtablissementRattachement

    };
    return service;



    /*
     * Supprime la societe, la ressource, l'etablissement de paye, le materiel et l'etablissement rattachement
     */
    function removeSociete(personnel) {
      personnel.Societe = null;
      personnel.SocieteId = null;
      personnel.SocieteLibelle = null;
      removeRessource(personnel);
      removeEtablissementPaie(personnel);
      removeEtablissementRattachement(personnel);
      removeMateriel(personnel);
      removeTypeRattachement(personnel);
    }

    /*
     * Supprime la ressource
     */
    function removeRessource(personnel) {
      personnel.Ressource = null;
      personnel.RessourceId = null;
    }

    /*
     * Supprime le materiel
     */
    function removeMateriel(personnel) {
      personnel.Materiel = null;
      personnel.MaterielId = null;
    }

    /*
     * Supprime  l'etablissement de paye
     */
    function removeEtablissementPaie(personnel) {
      personnel.EtablissementPaie = null;
      personnel.EtablissementPaieId = null;
      personnel.EtablissementPaieLibelle = null;
    }

    /*
     * Supprime l'etablissement rattachement
     */
    function removeEtablissementRattachement(personnel) {
      personnel.EtablissementRattachement = null;
      personnel.EtablissementRattachementId = null;
      personnel.EtablissementRattachementLibelle = null;
    }

    /*
     * Supprime le type de rattachement
     */
    function removeTypeRattachement(personnel) {
      personnel.TypeRattachementModel = null;
    }



  }
})();

