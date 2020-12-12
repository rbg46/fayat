/*
 * Ce service sert a copier coller des ressourceTache d'une tache a une autre.
 */
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('RessourceTacheDeviseManagerService', RessourceTacheDeviseManagerService);

  RessourceTacheDeviseManagerService.$inject = ['$log', 'ParametrageReferentielEtenduService', 'DevisesManagerService'];

  function RessourceTacheDeviseManagerService($log, ParametrageReferentielEtenduService, DevisesManagerService) {



    var service = {
      getRessourceTacheDeviseForDevise: getRessourceTacheDeviseForDevise,
      getPriceOfRessourceTacheDevise: getPriceOfRessourceTacheDevise,
      setPriceOfRessourceTacheDevises: setPriceOfRessourceTacheDevises
    };
    return service;

    //////////////////////////////////////////////////////////////////
    // PUBLIC METHODES                                              //
    //////////////////////////////////////////////////////////////////





    function getRessourceTacheDeviseForDevise(ressourceTask, deviseSelected) {
      var result = null;
      for (var i = 0; i < ressourceTask.RessourceTacheDevises.length; i++) {
        var ressourceTacheDevise = ressourceTask.RessourceTacheDevises[i];
        if (ressourceTacheDevise.DeviseId === deviseSelected.DeviseId) {
          result = ressourceTacheDevise;
        }
      }
      return result;
    }

    function getPriceOfRessourceTacheDevise(ressourceTask, deviseSelected) {
      var result = null;
      var ressourceTacheDevise = getRessourceTacheDeviseForDevise(ressourceTask, deviseSelected);
      if (ressourceTacheDevise !== null) {
        result = ressourceTacheDevise.PrixUnitaire;
        $log.log('Get Price Of ressource Tache Devise :  ' + result);
      }
      return result;
    }




    function setPriceOfRessourceTacheDevises(ressourceTask, price, deviseSelected) {
      if (price === '' || price === null || price === undefined) {
        clearRessourceTacheDevises(ressourceTask);
      } else {
        createRessourceTacheDevises(ressourceTask);
      }
      var ressourceTacheDevise = getRessourceTacheDeviseForDevise(ressourceTask, deviseSelected);
      if (ressourceTacheDevise !== null) {
        ressourceTacheDevise.PrixUnitaire = price;
        $log.log('Set Price Of ressource Tache Devise :  ' + price);
      }
    }


    //////////////////////////////////////////////////////////////////
    // PRIVATES METHODES                                            //
    //////////////////////////////////////////////////////////////////




    function clearRessourceTacheDevises(ressourceTask) {
      ressourceTask.RessourceTacheDevises = [];
    }

    function createRessourceTacheDevises(ressourceTask) {

      //var devicesDistinctOnPararms = ParametrageReferentielEtenduService.getDevisesOfParametrageReferentielEtendus(ressourceTask);
      var devicesDistinct = DevisesManagerService.getDevises();
      //var devicesDistinct = devicesOnCi;
      //if (devicesDistinctOnPararms && devicesDistinctOnPararms.length > 0) {
      //  devicesDistinct = devicesOnCi.concat(devicesDistinctOnPararms);
      //}

      //devicesDistinct = uniqueDevises(devicesDistinct);
         
      for (var i = 0; i < devicesDistinct.length; i++) {
        var devise = devicesDistinct[i];
        var existingRessourceTacheDevise = getRessourceTacheDeviseForDevise(ressourceTask, devise);
        if (existingRessourceTacheDevise === null) {
          var ressourceTacheDevise = {
            RessourceTacheDeviseId: 0,
            PrixUnitaire: 0,
            DeviseId: devise.DeviseId,
            RessourceTacheId: ressourceTask.RessourceTacheId
          };
          ressourceTask.RessourceTacheDevises.push(ressourceTacheDevise);
        }
      }    
    }


    //function uniqueDevises(array) {
    //  var processed = [];
    //  for (var i = array.length - 1; i >= 0; i--) {
    //    if (processed.indexOf(array[i].DeviseId) < 0) {
    //      processed.push(array[i].DeviseId);
    //    } else {
    //      array.splice(i, 1);
    //    }
    //  }
    //  return array;
    //}


  }
})();

