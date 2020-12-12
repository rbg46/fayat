(function () {
  'use strict';

  angular
      .module('Fred')
      .service('TacheBudgetCodeLibelleFilterService', TacheBudgetCodeLibelleFilterService);

  TacheBudgetCodeLibelleFilterService.$inject = ['TacheBudgetService'];

  function TacheBudgetCodeLibelleFilterService(TacheBudgetService) {

    var tasksIdsWhoMatchesWithFilter = [];

    var service = {
      setFilterForTasks: setFilterForTasks,
      taskMachWithFilter: taskMachWithFilter
    };
    return service;

    ////////////////


    function taskMachWithFilter(taskId) {
      return tasksIdsWhoMatchesWithFilter.indexOf(taskId) !== -1;
    }

    /*
     * Met l'id des taches dans un tableau lorque la tache match avec la recherche.
     * Ce qui permettra d'afficher ou non la tache.Si l'id est contenu dans le tableau.
     * L'ago commence par le niveau le plus bas(T4) et remonte jusqu'au niveau T1.       
     */
    function setFilterForTasks(tasks, searchText) {
      tasksIdsWhoMatchesWithFilter = [];
      var all = TacheBudgetService.getAllTasksFlatten(tasks);


      //je met l'id des taches du niveau T4 dans le tasksIdsWhoMatchesWithFilter.
      var tasksLevelFour = TacheBudgetService.selectAllTasksForLevel(all, 4);
      for (var i = 0; i < tasksLevelFour.length; i++) {
        var taskSelected = tasksLevelFour[i];
        if (match(taskSelected, searchText)) {
          tasksIdsWhoMatchesWithFilter.push(taskSelected.TacheId);
        }
      }
    
      var levelThree = getTaskWhoMathes(all, searchText, 3, tasksIdsWhoMatchesWithFilter);
      tasksIdsWhoMatchesWithFilter = tasksIdsWhoMatchesWithFilter.concat(levelThree);
      var levelTwo = getTaskWhoMathes(all, searchText, 2, tasksIdsWhoMatchesWithFilter);
      tasksIdsWhoMatchesWithFilter = tasksIdsWhoMatchesWithFilter.concat(levelTwo);
      var levelOne = getTaskWhoMathes(all, searchText, 1, tasksIdsWhoMatchesWithFilter);
      tasksIdsWhoMatchesWithFilter = tasksIdsWhoMatchesWithFilter.concat(levelOne);

    }

    function getTaskWhoMathes(all, searchText, level, tasksIdsWhoMatchesWithFilter) {
      var result = [];
      var tasksLevel = TacheBudgetService.selectAllTasksForLevel(all, level);
      for (var l = 0; l < tasksLevel.length; l++) {
        var taskSelected = tasksLevel[l];
        if (match(taskSelected, searchText) || hasChildrenWhoMath(taskSelected, tasksIdsWhoMatchesWithFilter)) {
          result.push(taskSelected.TacheId);
        }
      }
      return result;
    }


    /*
     * Dertermine si le code et le libellé match avec le filtre searchText.
     */
    function match(task, searchText) {
      if (task.Code.toLowerCase().indexOf(searchText.toLowerCase()) >= 0 || task.Libelle.toLowerCase().indexOf(searchText.toLowerCase()) >= 0) {
        return true;
      }
      return false;
    }

    /*
     *  Dertermine si le code et le libellé des taches enfants match avec le filtre searchText.   
     */
    function hasChildrenWhoMath(task, tasksIdsWhoMatchesWithFilter) {
      var firstChildrenWithTag = task.TachesEnfants.find(function (t) {
        return tasksIdsWhoMatchesWithFilter.indexOf(t.TacheId) !== -1;
      });
      if (!firstChildrenWithTag) {
        return false;
      }
      return true;
    }
       
  }

})();

