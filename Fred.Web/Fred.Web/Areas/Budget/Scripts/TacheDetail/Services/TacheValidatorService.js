
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('TacheValidatorService', TacheValidatorService);

  TacheValidatorService.$inject = ['TacheBudgetService'];

  function TacheValidatorService(TacheBudgetService) {
    var tasksErrors = [];
   

    var service = {
      taskIsInError: taskIsInError,
      ressourceIsInError: ressourceIsInError,
      getTasksErrorInfo: getTasksErrorInfo,
      calculateTasksErrors: calculateTasksErrors,
      hasAnyFormuleErrorInTask: hasAnyFormuleErrorInTask
    };

    return service;



    //////////////////////////////////////////////////////////////////
    // Publics methodes                                             //
    //////////////////////////////////////////////////////////////////

    /*
     * Determine si une tache a au moins une erreur de formule sur une ressource tache.
     * @param {task} Tache sur laquelle on cherche les erreurs de formulek.
     * @returns true si il y a au moins une erreur.
     */
    function hasAnyFormuleErrorInTask(task) {
      var result = false;
      for (var i = 0; i < task.RessourceTaches.length; i++) {
        var ressourceTache = task.RessourceTaches[i];
        if (ressourceTache.hasFormuleError) {
          return true;
        }
      }

      return result;
    }


    /*
     * Determine si une tache est en erreur.
     */
    function taskIsInError(task) {
      if (!isValidQuantity(task.QuantiteARealise)) {
        return false;
      }
      if (!isValidQuantity(task.QuantiteBase)) {
        return false;
      }

      if (task.QuantiteBase === null) {
        return true;
      }
      if (task.QuantiteARealise === null || task.QuantiteARealise === undefined) {
        return true;
      }
      if (task.QuantiteBase === '' || task.QuantiteARealise === '') {
        return true;
      }
      return false;
    }


    /*
     * Determine si une ressource tache est en erreur.
     */
    function ressourceIsInError(task, ressourceTache) {
      if (taskIsInError(task)) {
        return true;
      }
      if (ressourceTache.QuantiteBase === null && ressourceTache.Formule === null) {
        return true;
      }
      if (ressourceTache.hasFormuleError && ressourceTache.Quantite === null) {
        return true;
      }
      return false;
    }

    /*
     * calcule les erreurs (nb de ressource manquantes) pour toutes les taches.
     */
    function calculateTasksErrors(tasks) {
      tasksErrors = [];
      var all = TacheBudgetService.getAllTasksFlatten(tasks);
      var tasksLevelFour = TacheBudgetService.selectAllTasksForLevel(all, 4);
      for (var i = 0; i < tasksLevelFour.length; i++) {
        var task = tasksLevelFour[i];
        var taskErrorInfo = calculateErrorT4(task);
        tasksErrors.push(taskErrorInfo);
      }

      var TasksLevelTree = calculateErrorForLevel(all, tasksErrors, 3);
      tasksErrors = tasksErrors.concat(TasksLevelTree);
      var TasksLevelTwo = calculateErrorForLevel(all, tasksErrors, 2);
      tasksErrors = tasksErrors.concat(TasksLevelTwo);
      var TasksLevelOne = calculateErrorForLevel(all, tasksErrors, 1);
      tasksErrors = tasksErrors.concat(TasksLevelOne);

    }

    /*
     * permet de recuperer des infos sur les erreurs d'une tache. 
     */
    function getTasksErrorInfo(task) {
      var errorInfo = tasksErrors.find(function (e) {
        return e.TacheId === task.TacheId;
      });
      return errorInfo;
    }

    //////////////////////////////////////////////////////////////////
    // Privates methodes                                            //
    //////////////////////////////////////////////////////////////////

    /*
     * Verifie qu'une quantité est valide.
     */
    function isValidQuantity(quantity) {
      if (quantity === undefined || quantity === null || quantity === '') {
        return false;
      }
      if (!angular.isNumber(quantity)) {
        return false;
      }
      if (quantity > 999999999999.99) {
        return false;
      }
      var pattern = /^\d*\.?\d{0,2}$/;
      var success = pattern.test(quantity);
      if (!success) {
        return false;
      }
      return true;
    }




    //////////////////////////////////////////////////////////////////
    // calcul du nombre d'erreurs                                   //
    //////////////////////////////////////////////////////////////////

    /*
     * Calcule les erreurs sur une tache de niveau 4
     */
    function calculateErrorT4(task) {
      var result = getDefaultErrorInfo(task);

      if (taskIsInError(task)) {
        result.taskIsInError = true;
        result.ressourcesWithoutParameters = task.RessourceTaches.length;
      }
      else {
        for (var i = 0; i < task.RessourceTaches.length; i++) {
          var ressourceTache = task.RessourceTaches[i];
          if (ressourceIsInError(task, ressourceTache)) {
            result.taskIsInError = true;
            result.ressourcesWithoutParameters += 1;
          }
        }
      }
      return result;
    }

    /*
     *  Calcule les erreurs pour un niveau. 
     */
    function calculateErrorForLevel(all, tasksErrors, level) {
      var errorsForLevel = [];
      var TasksForLevel = TacheBudgetService.selectAllTasksForLevel(all, level);
      for (var i = 0; i < TasksForLevel.length; i++) {
        var task = TasksForLevel[i];
        var taskErrorInfo = calculateError(task, tasksErrors);
        errorsForLevel.push(taskErrorInfo);
      }
      return errorsForLevel;
    }


    /*
    *  Calcule les erreurs pour une tache avec un niveau autre que 4. 
    */
    function calculateError(task, tasksErrors) {
      var errorsForTask = [];
      for (var i = 0; i < task.TachesEnfants.length; i++) {
        var childTask = task.TachesEnfants[i];      
        var taskErrorInfo = tasksErrors.find(function (e) {
          return e.TacheId === childTask.TacheId;
        });
        if (taskErrorInfo) {
          errorsForTask.push(taskErrorInfo);
        }
      }

      var result = getDefaultErrorInfo(task);

      for (var j = 0; j < errorsForTask.length; j++) {
        var errorForTask = errorsForTask[j];
        if (errorForTask.taskIsInError) {
          result.taskIsInError = true;
          result.ressourcesWithoutParameters += errorForTask.ressourcesWithoutParameters;
        }
      }
      return result;
    }
  

    function getDefaultErrorInfo(task) {
      return {
        TacheId: task.TacheId,
        taskIsInError: false,
        ressourcesWithoutParameters: 0
      };
    }

  }
})();

