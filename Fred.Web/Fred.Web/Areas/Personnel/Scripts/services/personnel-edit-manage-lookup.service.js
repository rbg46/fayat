/*
 * Ce service sert lors de la modification d'un lookup
 */
(function () {
  'use strict';

  angular.module('Fred').service('PersonnelEditManageLookup', PersonnelEditManageLookupService);

  PersonnelEditManageLookupService.$inject = ['PersonnelEditFieldsCleanerService'];

  function PersonnelEditManageLookupService(PersonnelEditFieldsCleanerService) {

    var service = {
      EtablissementPaieChange: EtablissementPaieChange,
      EtablissementRattachementChange: EtablissementRattachementChange,
      PaysChange: PaysChange,
      SocieteChange: SocieteChange,
      RessourceChange: RessourceChange,
      MaterielChange: MaterielChange
    };
    return service;

    function EtablissementPaieChange(selectedRef, personnel, typeRattachementList) {
      if (selectedRef !== null) {
        personnel.EtablissementPaie = selectedRef;
        personnel.EtablissementPaieId = selectedRef.IdRef;
        personnel.EtablissementPaieLibelle = selectedRef.LibelleRef;
        personnel.TypeRattachementModel = selecTypeRattachement(selectedRef, typeRattachementList);
        if (personnel.TypeRattachementModel.Code !== 'D') {
          personnel.EtablissementRattachement = selectedRef;
          personnel.EtablissementRattachementId = selectedRef.IdRef;
          personnel.EtablissementRattachementLibelle = selectedRef.LibelleRef;
        } else {
          personnel.EtablissementRattachement = null;
          personnel.EtablissementRattachementId = null;
          personnel.EtablissementRattachementLibelle = null;
        }

      }
    }


    function selecTypeRattachement(selectedRef, typeRattachementList) {
      var result = null;
      if (selectedRef.HorsRegion) {
        result = selecTypeRattachementWithCode(typeRattachementList, 'D');
      }
      else {
        result = selecTypeRattachementWithCode(typeRattachementList, 'A');
      }
      return result;
    }


    function selecTypeRattachementWithCode(typeRattachementList, code) {
      var result = null;
      for (var t = 0; t < typeRattachementList.length; t++) {
        var typeRatt = typeRattachementList[t];
        if (typeRatt.Code === code) {
          result = typeRatt;
        }
      }
      return result;

    }


    function EtablissementRattachementChange(selectedRef, personnel) {
      if (selectedRef !== null) {
        personnel.EtablissementRattachement = selectedRef;
        personnel.EtablissementRattachementId = selectedRef.IdRef;
        personnel.EtablissementRattachementLibelle = selectedRef.LibelleRef;
      }
    }

    function PaysChange(selectedRef, personnel) {
      if (selectedRef !== null) {
        personnel.Pays = selectedRef;
        personnel.PaysId = selectedRef.IdRef;
        personnel.PaysLibelle = selectedRef.LibelleRef;
      }
    }

    function SocieteChange(selectedRef, personnel) {
      if (selectedRef !== null) {
        if (personnel.SocieteId !== selectedRef.IdRef) {
          PersonnelEditFieldsCleanerService.removeSociete(personnel);
          personnel.Societe = selectedRef;
          personnel.SocieteId = selectedRef.IdRef;
          personnel.SocieteLibelle = selectedRef.LibelleRef;
          // checkInterimaire();
        }
      }
    }

    function RessourceChange(selectedRef, personnel) {
      if (selectedRef !== null) {
        personnel.Ressource = selectedRef;
        personnel.RessourceId = selectedRef.IdRef;
      }
    }

    function MaterielChange(selectedRef, personnel) {
      if (selectedRef !== null) {
        personnel.Materiel = selectedRef;
        personnel.MaterielId = selectedRef.IdRef;
      }
    }


  }
})();

