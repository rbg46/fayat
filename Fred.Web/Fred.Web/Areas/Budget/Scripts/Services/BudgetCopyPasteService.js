/*
 * Ce service sert a copier coller des ressourceTache d'une tache a une autre.
 */
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('BudgetCopyPasteService', BudgetCopyPasteService);

  function BudgetCopyPasteService() {

    var savedRessourceTasks = null;

    var service = {
      canCopy:canCopy,
      copy: copy,
      canPaste: canPaste,
      paste: paste,
      clear:clear
    };
    return service;



    /*
     * Copie les ressourcesTache d'une tache.
     */
    function canCopy(task) {
      return task && task.RessourceTaches && task.RessourceTaches.length > 0;
    }


    /*
     * Copie les ressourcesTache d'une tache.
     */
    function copy(task) {    
      savedRessourceTasks = internalCopy(task.RessourceTaches);
    }

    /*
     * Verifie que la fonction coller est executable
     */
    function canPaste() {
      return savedRessourceTasks !== null && savedRessourceTasks.length > 0;
    }

    /*
     * Colle les ressourcesTache sauvegardées.
     */
    function paste(task) {
      if (canPaste()) {
        var newCopy = internalCopy(savedRessourceTasks);
        for (var i = 0; i < newCopy.length; i++) {
          var ressourceTask = newCopy[i];
          ressourceTask.RessourceTacheId = 0;
          ressourceTask.TacheId = task.TacheId;
          task.RessourceTaches.push(ressourceTask);
        }
      }
    }
    function internalCopy(ressourceTaches) {
      var copyRessourceTasks = [];
      angular.copy(ressourceTaches, copyRessourceTasks);
      return copyRessourceTasks;
    }

    /*
     * vide le presse papier.
     */
    function clear() {
      savedRessourceTasks = null;
    }

  }
})();

