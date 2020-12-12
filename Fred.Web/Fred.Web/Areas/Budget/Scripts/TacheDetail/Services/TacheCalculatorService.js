
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('TacheCalculatorService', TacheCalculatorService);

  TacheCalculatorService.$inject = ['TacheBudgetService', 'TacheValidatorService', 'BudgetMathService', 'TachePriceManagerService','UniteManagerService'];

  function TacheCalculatorService(TacheBudgetService, TacheValidatorService, BudgetMathService, TachePriceManagerService, UniteManagerService) {
    var parser = new exprEval.Parser();
    var service = {
      initializeQuantityBaseIfUndefined: initializeQuantityBaseIfUndefined,
      initializeQuantityToRealizedIfUndefined: initializeQuantityToRealizedIfUndefined,

      manageTaskQuantityChanged: manageTaskQuantityChanged,
      canCalculQuantiteBaseWithFormule: canCalculQuantiteBaseWithFormule,
      calculQuantiteBaseWithFormule: calculQuantiteBaseWithFormule,

      calculateAllForOneRessourceTask: calculateAllForOneRessourceTask,
      calculateAllForEachRessourceTask: calculateAllForEachRessourceTask,


      calculTotalsForTask: calculTotalsForTask,
      resetRessourceTask: resetRessourceTask,
      calculateTotalsForAllTasks: calculateTotalsForAllTasks,
      calculTotalsForTaskWithoutModify: calculTotalsForTaskWithoutModify

    };

    return service;



    function initializeQuantityBaseIfUndefined(taskSelected) {
      if (taskSelected.QuantiteBase === null || taskSelected.QuantiteBase === undefined) {
        taskSelected.QuantiteBase = 1;
      }
    }

    function initializeQuantityToRealizedIfUndefined(taskSelected) {
      if (taskSelected.QuantiteARealise === null || taskSelected.QuantiteARealise === undefined) {
        taskSelected.QuantiteARealise = 1;
      }
    }

    //////////////////////////////////////////////////////////////////
    // Action sur le changement de quantite (base ou à réalisée)    //
    //////////////////////////////////////////////////////////////////

    function manageTaskQuantityChanged(taskSelected, deviseSelected) {
      if (TacheValidatorService.taskIsInError(taskSelected)) {
        resetAllRessources(taskSelected);
      } else {
        calculQuantiteBaseWithFormuleForAllRessources(taskSelected);
        calculateAllForEachRessourceTask(taskSelected, deviseSelected);
      }
      calculTotalsForTask(taskSelected, deviseSelected);
    }




    //////////////////////////////////////////////////////////////////
    // Calcul sur les ressources                                    //
    //////////////////////////////////////////////////////////////////

    /*
     * Calcule le montant de base pour une ressource
     */
    function calculRessourceMontantBase(ressourceTache, deviseSelected) {
      return BudgetMathService.toFixed(function () { return ressourceTache.QuantiteBase * TachePriceManagerService.getPrixUnitaire(ressourceTache, deviseSelected); });
    }

    /*
     * Calcule le quantité totale pour une ressource
     */
    function calculRessourceQuantiteTotale(taskSelected, ressourceTache) {
      return BudgetMathService.toFixed(function () { return taskSelected.QuantiteARealise / taskSelected.QuantiteBase * ressourceTache.QuantiteBase;});
    }

    /*
    * Calcule le montant totale pour une ressource
    */
    function calculRessourceMontantTotal(ressourceTache, deviseSelected) {
      return BudgetMathService.toFixed(function () { return ressourceTache.Quantite * TachePriceManagerService.getPrixUnitaire(ressourceTache, deviseSelected); });
    }

    /*
     * verifie que l'on peu calculer pour une ressource.
     */
    function canCalculateRessource(taskSelected, ressourceTache, deviseSelected) {
      if (angular.isNumber(ressourceTache.QuantiteBase) && angular.isNumber(TachePriceManagerService.getPrixUnitaire(ressourceTache, deviseSelected))) {
        if (angular.isNumber(taskSelected.QuantiteBase) && angular.isNumber(taskSelected.QuantiteARealise)) {
          return true;
        }
      }
      return false;
    }

    /*
     * verifie si on peux calculer la quantité de base a partir de la formule.
     */
    function canCalculQuantiteBaseWithFormule(taskSelected, ressourceTache) {
      var hasError = true;
      try {
        calculQuantiteBaseWithFormule(taskSelected, ressourceTache);
        ressourceTache.hasFormuleError = false;
        hasError = false;
      } catch (e) {
        hasError = true;
        ressourceTache.hasFormuleError = true;
      }
      return !hasError;
    }

    /*
     * calcule la quantité de base avec la formule mathematique
     */
    function calculQuantiteBaseWithFormule(taskSelected, ressourceTache) {
      var formule = ressourceTache.Formule.replace('Q', 'q');
      var expr = parser.parse(formule);
      var quantiteBase = expr.evaluate({ q: taskSelected.QuantiteBase });
      return quantiteBase;
    }

    function calculQuantiteBaseWithFormuleForAllRessources(taskSelected) {
      for (var i = 0; i < taskSelected.RessourceTaches.length; i++) {
        var ressourceTache = taskSelected.RessourceTaches[i];
        if (canCalculQuantiteBaseWithFormule(taskSelected, ressourceTache)) {

          ressourceTache.QuantiteBase = calculQuantiteBaseWithFormule(taskSelected, ressourceTache);
        }
      }
    }

    /*
     * Calcule le Montant, la quantite et le montant total pour une ressourceTache.
     */
    function calculateAllForOneRessourceTask(taskSelected, ressourceTache, deviseSelected) {

      if (!TacheValidatorService.ressourceIsInError(taskSelected, ressourceTache)
        && canCalculateRessource(taskSelected, ressourceTache, deviseSelected)) {

        ressourceTache.Montant = calculRessourceMontantBase(ressourceTache, deviseSelected);
        ressourceTache.Quantite = calculRessourceQuantiteTotale(taskSelected, ressourceTache);
        ressourceTache.MontantTotal = calculRessourceMontantTotal(ressourceTache, deviseSelected);

      } else {
        resetRessourceTask(ressourceTache);
      }
    }

    /*
     * Calcule le Montant, la quantite et le montant total pour toutes ressourceTaches.
     */
    function calculateAllForEachRessourceTask(taskSelected, deviseSelected) {
      for (var i = 0; i < taskSelected.RessourceTaches.length; i++) {
        var ressourceTache = taskSelected.RessourceTaches[i];
        calculateAllForOneRessourceTask(taskSelected, ressourceTache, deviseSelected);
      }
    }


    //////////////////////////////////////////////////////////////////
    // Calcul des totaux                                            //
    //////////////////////////////////////////////////////////////////

    /*
     * Calcule le prix unitaire QB
     */
    function calculPrixUnitaireQB(task, deviseSelected) {
      return BudgetMathService.toFixed(function () { return calculPrixTotalQB(task, deviseSelected) / task.QuantiteBase; });
    }

    /*
     * Calcule le prix total QB
     */
    function calculPrixTotalQB(task, deviseSelected) {
      var result = 0;
      for (var i = 0; i < task.RessourceTaches.length; i++) {
        var ressourceTache = task.RessourceTaches[i];
        if (!TacheValidatorService.ressourceIsInError(task, ressourceTache)) {
          if (angular.isNumber(ressourceTache.Montant) && !isNaN(ressourceTache.Montant)) {
            if (TachePriceManagerService.hasPriceForDevise(ressourceTache, deviseSelected)) {
              var total = BudgetMathService.add(result, ressourceTache.Montant);
              result = total;
            }
          }
        }
      }
      return result;
    }

    /*
     * Calcule le prix total T4
     */
    function calculPrixTotalT4(task, deviseSelected) {
      var result = 0;
      for (var i = 0; i < task.RessourceTaches.length; i++) {
        var ressourceTache = task.RessourceTaches[i];
        if (!TacheValidatorService.ressourceIsInError(task, ressourceTache)) {
          if (angular.isNumber(ressourceTache.MontantTotal) && !isNaN(ressourceTache.MontantTotal)) {
            if (TachePriceManagerService.hasPriceForDevise(ressourceTache, deviseSelected)) {
              var total = BudgetMathService.add(result, ressourceTache.MontantTotal);
              result = total;
            }
          }

        }
      }
      return result;
    }


    function calculTotalHeureMO(task, deviseSelected) {
      var result = 0;
      for (var i = 0; i < task.RessourceTaches.length; i++) {
        var ressourceTache = task.RessourceTaches[i];

        //todo modifier base pour TypeRessourceId === 3 demande manu
        if (!TacheValidatorService.ressourceIsInError(task, ressourceTache) && ressourceTache.Ressource.TypeRessourceId === 2) {
          if (angular.isNumber(ressourceTache.QuantiteBase) && !isNaN(ressourceTache.QuantiteBase)) {
            if (TachePriceManagerService.hasPriceForDevise(ressourceTache, deviseSelected)) {

              var isOneHour = UniteManagerService.getParamIsHourTime(ressourceTache, deviseSelected);
              if (isOneHour) {
                var total1 = BudgetMathService.add(result, ressourceTache.QuantiteBase);
                result = total1;
              }

              var isOneDay = UniteManagerService.getParamIsDayTime(ressourceTache, deviseSelected);
              if (isOneDay) {
                var nbHours = ressourceTache.QuantiteBase * 8; 
                var total2 = BudgetMathService.add(result, nbHours);
                result = total2;
              }

            }
          }
        }
      }
      return result;
    }

    function calculHeureMOUnite(task, deviseSelected) {
      return BudgetMathService.toFixed(function () { return calculTotalHeureMO(task, deviseSelected) / task.QuantiteBase; });
    }

    function calculTotalHeureMOT4(task, deviseSelected) {
      return BudgetMathService.toFixed(function () { return task.QuantiteARealise / task.QuantiteBase * calculTotalHeureMO(task, deviseSelected);});
    }


    /*
     * Calcule les prix totaux pour une tache
     */
    function calculTotalsForTask(task, deviseSelected) {

      var totalsForTask = calculTotalsForTaskWithoutModify(task, deviseSelected);

      angular.merge(task, totalsForTask);

    }

    function calculTotalsForTaskWithoutModify(task, deviseSelected) {

      var result = {
        PrixTotalQB: null,
        PrixUnitaireQB: null,
        TotalT4: null,
        TotalHeureMO: null,
        HeureMOUnite: null,
        TotalHeureMOT4: null
      };

      if (!TacheValidatorService.taskIsInError(task)) {
        result.PrixTotalQB = calculPrixTotalQB(task, deviseSelected);
        result.PrixUnitaireQB = calculPrixUnitaireQB(task, deviseSelected);
        result.TotalT4 = calculPrixTotalT4(task, deviseSelected);
        result.TotalHeureMO = calculTotalHeureMO(task, deviseSelected);
        result.HeureMOUnite = calculHeureMOUnite(task, deviseSelected);
        result.TotalHeureMOT4 = calculTotalHeureMOT4(task, deviseSelected);
      }
      return result;
    }


    /*
     * calcule tous les totaux pour toutes les taches.
     */
    function calculateTotalsForAllTasks(taches, deviseSelected) {

      var all = TacheBudgetService.getAllTasksFlatten(taches);
      var taskSelected = null;

      var tasksLevelFour = TacheBudgetService.selectAllTasksForLevel(all, 4);
      for (var i = 0; i < tasksLevelFour.length; i++) {
        taskSelected = tasksLevelFour[i];
        initializeQuantityBaseIfUndefined(taskSelected);
        initializeQuantityToRealizedIfUndefined(taskSelected);
        calculateAllForEachRessourceTask(taskSelected, deviseSelected);
        calculTotalsForTask(taskSelected, deviseSelected);
      }

      var tasksLevelthree = TacheBudgetService.selectAllTasksForLevel(all, 3);
      for (var j = 0; j < tasksLevelthree.length; j++) {
        taskSelected = tasksLevelthree[j];
        calculateAllForEachRessourceTask(taskSelected, deviseSelected);
        calculTotalsForTask(taskSelected, deviseSelected);
      }

      var tasksLeveltwo = TacheBudgetService.selectAllTasksForLevel(all, 2);
      for (var k = 0; k < tasksLeveltwo.length; k++) {
        taskSelected = tasksLeveltwo[k];
        calculateAllForEachRessourceTask(taskSelected, deviseSelected);
        calculTotalsForTask(taskSelected, deviseSelected);
      }

      var tasksLevelOne = TacheBudgetService.selectAllTasksForLevel(all, 1);
      for (var l = 0; l < tasksLevelOne.length; l++) {
        taskSelected = tasksLevelOne[l];
        calculateAllForEachRessourceTask(taskSelected, deviseSelected);
        calculTotalsForTask(taskSelected, deviseSelected);
      }

    }


    //////////////////////////////////////////////////////////////////
    // HELPERS - Remise a zero                                      //
    //////////////////////////////////////////////////////////////////

    /*
    * suppression des calculs sur chaque ligne
    */
    function resetAllRessources(taskSelected) {
      for (var i = 0; i < taskSelected.RessourceTaches.length; i++) {
        var ressourceTache = taskSelected.RessourceTaches[i];
        resetRessourceTask(ressourceTache);
      }
    }

    /*
     * suppression des calculs sur une ligne
     */
    function resetRessourceTask(ressourceTache) {
      ressourceTache.Montant = null;
      ressourceTache.MontantTotal = null;
      ressourceTache.Quantite = null;
    }


    //////////////////////////////////////////////////////////////////
    // HELPERS                                                      //
    //////////////////////////////////////////////////////////////////



  }
})();

