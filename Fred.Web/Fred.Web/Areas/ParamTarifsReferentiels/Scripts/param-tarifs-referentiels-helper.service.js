////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////// HELPERS POUR ParamTarifsReferentiels ///////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
(function () {
  'use strict';

  angular.module('Fred').service('ParamTarifsReferentielsHelperService', ParamTarifsReferentielsHelperService);

  function ParamTarifsReferentielsHelperService() {


    var service = {
      canSave: canSave,
      hasMissingInfoOnParam: hasMissingInfoOnParam,
      setSynthese: setSynthese,
      getListParamToDelete: getListParamToDelete,
      getListParamToUpdate: getListParamToUpdate,
      setWarnings: setWarnings

    };
    return service;

    /*
     * Permet de savoir si un des parametres n'est pas valide, cad, qu 'il manque soit le prix soit l'unité.
     */
    function hasMissingInfoOnParam(listParamRefEtenduModified) {
      var result = false;
      for (var i = 0; i < listParamRefEtenduModified.length; i++) {
        var paramRefEtendu = listParamRefEtenduModified[i];
        if (_isInError(paramRefEtendu)) {
          result = true;
        }
      }
      return result;
    }

    function _isInError(paramRefEtendu) {
      if (paramRefEtendu.Montant !== null && paramRefEtendu.Unite === null && paramRefEtendu.ParametrageReferentielEtenduId === 0) {
        return true;
      }
      return false;
    }


    /*
     * Permet de savoir si l'on peux sauvegarder.
     */
    function canSave(listParamRefEtenduModified) {
      var result = false;
      if (listParamRefEtenduModified.length > 0 && !hasMissingInfoOnParam(listParamRefEtenduModified)) {
        result = true;
      }
      return result;
    }


    /*
     * Calcule la synthese(prix et unité) et l'affecte au ReferentielEtendu
     */
    function setSynthese(tarifsReferentiels) {
      for (var i = 0; i < tarifsReferentiels.length; i++) {
        var chapitre = tarifsReferentiels[i];
        for (var j = 0; j < chapitre.SousChapitres.length; j++) {
          var sousChapitre = chapitre.SousChapitres[j];
          for (var k = 0; k < sousChapitre.Ressources.length; k++) {
            var ressource = sousChapitre.Ressources[k];
            var foundSynthese = null;
            for (var l = 0; l < ressource.ReferentielEtendus[0].ParametrageReferentielEtendus.length; l++) {
              var param = ressource.ReferentielEtendus[0].ParametrageReferentielEtendus[l];
              if (param.Montant !== null)
                foundSynthese = { Montant: param.Montant, Unite: param.Unite };
            }
            ressource.ReferentielEtendus[0].Synthese = foundSynthese;
          }
        }
      }
    }

    function setWarnings(tarifsReferentiels) {
      for (var i = 0; i < tarifsReferentiels.length; i++) {
        var chapitre = tarifsReferentiels[i];
        chapitre.CountParamRefEtenduToBeTreated = 0;
        for (var j = 0; j < chapitre.SousChapitres.length; j++) {
          var sousChapitre = chapitre.SousChapitres[j];
          sousChapitre.CountParamRefEtenduToBeTreated = 0;
          for (var k = 0; k < sousChapitre.Ressources.length; k++) {
            var ressource = sousChapitre.Ressources[k];
            var referentielEtendu = ressource.ReferentielEtendus[0];
            if (referentielEtendu.Synthese === null) {
              sousChapitre.CountParamRefEtenduToBeTreated += 1;
              chapitre.CountParamRefEtenduToBeTreated += 1;
            }
          }
        }
      }
    }

    /*
    * Permet de savoir quelle sont les param a supprimer.
    */
    function getListParamToDelete(listParamRefEtenduModified) {

      var result = [];
      for (var i = 0; i < listParamRefEtenduModified.length; i++) {
        var paramRefEtendu = listParamRefEtenduModified[i];
        if (_isDelete(paramRefEtendu)) {
          result.push(paramRefEtendu);
        }
      }
      return result;
    }


    function _isDelete(paramRefEtendu) {
      if (paramRefEtendu.ParametrageReferentielEtenduId && paramRefEtendu.Montant === null) {
        return true;
      }
      return false;
    }

    /*
    * Permet de savoir quelle sont les param a mettre a jour.
    */
    function getListParamToUpdate(listParamRefEtenduModified) {

      var result = [];
      for (var i = 0; i < listParamRefEtenduModified.length; i++) {
        var paramRefEtendu = listParamRefEtenduModified[i];
        if (_isUpdateOrCreate(paramRefEtendu)) {
          result.push(paramRefEtendu);
        }
      }
      return result;
    }

    function _isUpdateOrCreate(paramRefEtendu) {
      if (paramRefEtendu.Montant !== null &&
        paramRefEtendu.Montant !== undefined &&
        paramRefEtendu.Unite !== null &&
        paramRefEtendu.Unite !== undefined) {
        return true;
      }
      return false;
    }



  }
})();

