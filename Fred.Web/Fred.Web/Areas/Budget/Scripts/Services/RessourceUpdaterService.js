/*
 * Ce service met a jour toutes les ressources avec le meme Id.
 * Ce service est utilisé lors de la modification d'un prix sur l'ecran de détail d'une tache.
 */
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('RessourceUpdaterService', RessourceUpdaterService);

  RessourceUpdaterService.$inject = ['TacheBudgetService', '$log'];

  function RessourceUpdaterService(TacheBudgetService, $log) {

    var tasks = null;
    var chapitres = null;

    var modifiedRessourcesOnRessources = [];
    var savedOriginalRessourcesOnTasks = [];
    var savedOriginalRessourcesOnRessources = [];

    var service = {



      setTasks: setTasks,
      setChapitres: setChapitres,
      saveRessourceForRollback: saveRessourceForRollback,
      modifyRessourceInRessourcesView: modifyRessourceInRessourcesView,
      modifyRessourceInTasksView: modifyRessourceInTasksView,
      getRessourcesModified: getRessourcesModified,
      rollbackRessourcesModification: rollbackRessourcesModification,
      clear: clear

    };
    return service;

    function setTasks(taches) {
      tasks = taches;
    }

    function setChapitres(chaps) {
      chapitres = chaps;
    }

    function modifyRessourceInRessourcesView(updatedRessource) {
      modifyRessourceInRessources(chapitres, updatedRessource);
    }

    function modifyRessourceInTasksView(updatedRessource) {
      modifyRessourceInTasks(tasks, updatedRessource);
    }


    /////////////////////////////////////////////////////////////////////
    // RESSOURCES   
    /////////////////////////////////////////////////////////////////////

    /*
    * Recherche dans toutes les Ressources des Chapitres - SousChapitre 
    * et met a jour la ressource.
    */
    function modifyRessourceInRessources(chapitres, updatedRessource) {
      for (var i = 0; i < chapitres.length; i++) {
        var chapitre = chapitres[i];
        for (var j = 0; j < chapitre.SousChapitres.length; j++) {
          var sousChapitre = chapitre.SousChapitres[j];
          for (var k = 0; k < sousChapitre.Ressources.length; k++) {
            var ressource = sousChapitre.Ressources[k];
            if (ressource.RessourceId === updatedRessource.RessourceId) {
              saveRessourceOnRessources(ressource, updatedRessource);
              $log.log('Modification de la ressource dans les taches RessourceId = ' + updatedRessource.RessourceId);
              replaceRessource(sousChapitre, updatedRessource, ressource);
            }

            //verification dans les ressources enfants.
            for (var l = 0; l < ressource.RessourcesEnfants.length; l++) {
              var childRessource = ressource.RessourcesEnfants[l];
              if (childRessource.RessourceId === updatedRessource.RessourceId) {
                saveRessourceOnRessources(childRessource, updatedRessource);
                $log.log('Modification de la ressource dans les ressources' + updatedRessource.Code);
                replaceRessourceInRessourcesEnfants(ressource.RessourcesEnfants, updatedRessource);

              }
            }
          }
        }
      }
    }


    /*
    *Remplace une ressource dans un chapitre.
    */
    function replaceRessource(sousChapitre, updatedRessource, oldRessource) {
      var index = sousChapitre.Ressources
                                     .map(function (e) { return e.RessourceId; })
                                     .indexOf(updatedRessource.RessourceId);
      if (index !== -1) {


        var updatedRessourceCopy = {};
        angular.copy(updatedRessource, updatedRessourceCopy);
        var childs = oldRessource.RessourcesEnfants;
        sousChapitre.Ressources[index] = updatedRessourceCopy;
        sousChapitre.Ressources[index].RessourcesEnfants = childs;

      }
    }

    /*
    *remplace la ressource dans une ressource parent.
    */
    function replaceRessourceInRessourcesEnfants(ressourcesEnfants, updatedRessource) {
      var index = ressourcesEnfants
                                     .map(function (e) { return e.RessourceId; })
                                     .indexOf(updatedRessource.RessourceId);
      if (index !== -1) {

        var updatedRessourceCopy = {};
        angular.copy(updatedRessource, updatedRessourceCopy);
        ressourcesEnfants[index] = updatedRessourceCopy;
      }
    }

    /////////////////////////////////////////////////////////////////////
    // TACHES   
    /////////////////////////////////////////////////////////////////////


    /*
    * Recherche dans toutes les Ressources des taches 
    * et met a jour la ressource.
    */
    function modifyRessourceInTasks(taches, updatedRessource) {
      var all = TacheBudgetService.getAllTasksFlatten(taches);
      for (var i = 0; i < all.length; i++) {
        var task = all[i];
        for (var j = 0; j < task.RessourceTaches.length; j++) {
          var ressourceTache = task.RessourceTaches[j];
          var ressource = ressourceTache.Ressource;
          if (ressource.RessourceId === updatedRessource.RessourceId) {
            //saveRessourceOnTasks(updatedRessource);
            modifyRessource(ressourceTache, updatedRessource);
          }
        }
      }
    }



    function modifyRessource(ressourceTache, updatedRessource, savedModification) {
      ressourceTache.RessourceId = updatedRessource.RessourceId;

      var updatedRessourceCopy = {};
      angular.copy(updatedRessource, updatedRessourceCopy);
      ressourceTache.Ressource = updatedRessourceCopy;

    }



    function rollbackRessourcesModification() {

      for (var i = 0; i < savedOriginalRessourcesOnTasks.length; i++) {
        var savedRessourceOnTasks = savedOriginalRessourcesOnTasks[i];
        modifyRessourceInTasks(tasks, savedRessourceOnTasks);
      }


      for (var j = 0; j < savedOriginalRessourcesOnRessources.length; j++) {
        var savedRessourceOnRessource = savedOriginalRessourcesOnRessources[j];
        modifyRessourceInRessources(chapitres, savedRessourceOnRessource);
      }

      clear();

    }

    function clear() {
      modifiedRessourcesOnRessources = [];
      savedOriginalRessourcesOnTasks = [];
      savedOriginalRessourcesOnRessources = [];
    }


    function saveRessourceForRollback(ressource) {
      var index = savedOriginalRessourcesOnTasks
                                .map(function (e) { return e.RessourceId; })
                                .indexOf(ressource.RessourceId);

      var original = {};
      angular.copy(ressource, original);
      if (index === -1) {
        savedOriginalRessourcesOnTasks.push(original);
      }
    }


    function saveRessourceOnRessources(initialRessource, modifiedRessource) {
      var index = modifiedRessourcesOnRessources
                                  .map(function (e) { return e.RessourceId; })
                                  .indexOf(initialRessource.RessourceId);
      var modified = {};
      angular.copy(modifiedRessource, modified);
      var original = {};
      angular.copy(initialRessource, original);
      if (index === -1) {
        modifiedRessourcesOnRessources.push(modified);
        savedOriginalRessourcesOnRessources.push(original);
      }
      else {
        modifiedRessourcesOnRessources[index] = modified;
      }

    }

    function getRessourcesModified(task) {
      var result = [];

      for (var j = 0; j < task.RessourceTaches.length; j++) {
        var ressourceTache = task.RessourceTaches[j];
        var ressource = ressourceTache.Ressource;
        var index = savedOriginalRessourcesOnTasks.map(function (e) { return e.RessourceId; })
                         .indexOf(ressource.RessourceId);
        var index2 = result.map(function (e) { return e.RessourceId; })
                       .indexOf(ressource.RessourceId);
        if (index !== -1 && index2 === -1) {

          result.push(ressource);
        }

      }
      return result;
    }


  }
})();

