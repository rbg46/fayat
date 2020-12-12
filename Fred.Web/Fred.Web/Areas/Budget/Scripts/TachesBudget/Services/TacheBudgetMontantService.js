
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('TacheBudgetMontantService', TacheBudgetMontantService);

  TacheBudgetMontantService.$inject = ['TacheBudgetService', 'TacheCalculatorService', 'BudgetMathService'];

  function TacheBudgetMontantService(TacheBudgetService, TacheCalculatorService,BudgetMathService) {

    var service = {
      calculateAndModifyTasks: calculateAndModifyTasks,
      calculateTotalsForBudget: calculateTotalsForBudget
    };
    return service;


    /*
     * Calcule et modifie le montant total(TotalT4) pour chaque tache de niveau T1,T2,T3 et T4.
     */
    function calculateAndModifyTasks(taches, deviseSelected) {

      var all = TacheBudgetService.getAllTasksFlatten(taches);
      var taskSelected = null;

      var tasksLevelFour = TacheBudgetService.selectAllTasksForLevel(all, 4);
      for (var i = 0; i < tasksLevelFour.length; i++) {
        taskSelected = tasksLevelFour[i];
        //TacheCalculatorService.initializeQuantityBaseIfUndefined(taskSelected);
        //TacheCalculatorService.initializeQuantityToRealizedIfUndefined(taskSelected);
        TacheCalculatorService.calculateAllForEachRessourceTask(taskSelected, deviseSelected);
        TacheCalculatorService.calculTotalsForTask(taskSelected, deviseSelected);
      }

      var tasksLevelthree = TacheBudgetService.selectAllTasksForLevel(all, 3);
      for (var j = 0; j < tasksLevelthree.length; j++) {
        taskSelected = tasksLevelthree[j];
        ModifyTotalT4ForTask(taskSelected);
      }

      var tasksLeveltwo = TacheBudgetService.selectAllTasksForLevel(all, 2);
      for (var k = 0; k < tasksLeveltwo.length; k++) {
        taskSelected = tasksLeveltwo[k];
        ModifyTotalT4ForTask(taskSelected);
      }

      var tasksLevelOne = TacheBudgetService.selectAllTasksForLevel(all, 1);
      for (var l = 0; l < tasksLevelOne.length; l++) {
        taskSelected = tasksLevelOne[l];
        ModifyTotalT4ForTask(taskSelected);
      }

    }


    //Modification de la total T4 de la tache.
    function ModifyTotalT4ForTask(taskSelected) {
      taskSelected.TotalT4 = 0;
      for (var j = 0; j < taskSelected.TachesEnfants.length; j++) {
        var child = taskSelected.TachesEnfants[j];
        var total = BudgetMathService.add(taskSelected.TotalT4, child.TotalT4);
        taskSelected.TotalT4 = total;
      }
    }


    /*
     * Calcule le montant total pour chaque devises.
     */
    function calculateTotalsForBudget(taches, devises) {
      var results = [];

      //preparation du resultat
      for (var j = 0; j < devises.length; j++) {
        var devise = devises[j];
        var result = {
          DeviseId: devise.DeviseId,
          devise: devise,
          total: null
        };
        results.push(result);
      }

      var all = TacheBudgetService.getAllTasksFlatten(taches);
      var taskSelected = null;
      var tasksLevelFour = TacheBudgetService.selectAllTasksForLevel(all, 4);
      for (var k = 0; k < devises.length; k++) {
        var deviseSelected = devises[k];

        for (var i = 0; i < tasksLevelFour.length; i++) {

          taskSelected = tasksLevelFour[i];
          //TacheCalculatorService.initializeQuantityBaseIfUndefined(taskSelected);
          //TacheCalculatorService.initializeQuantityToRealizedIfUndefined(taskSelected);
          TacheCalculatorService.calculateAllForEachRessourceTask(taskSelected,deviseSelected);


          var totalsForTask = TacheCalculatorService.calculTotalsForTaskWithoutModify(taskSelected, deviseSelected);
          //selection du bon element pour mise a jour du total.
          var resultat = results.find(function (r) {
            return deviseSelected.DeviseId === r.DeviseId;
          });
          resultat.total = BudgetMathService.add(resultat.total, totalsForTask.TotalT4);
        }
      }
      return results;
    }

  }
})();

