
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('TacheBudgetComparerService', TacheBudgetComparerService);

  TacheBudgetComparerService.$inject = ['TacheBudgetService'];

  function TacheBudgetComparerService(TacheBudgetService) {

    var service = {
      getModifiedTasks: getModifiedTasks,
      clearModifiedTasks: clearModifiedTasks     
    };
    return service;

    function getModifiedTasks(taches) {
      var result = [];
      var tasksFlatten = TacheBudgetService.getAllTasksFlatten(taches);
      var currentConvertedTasks = convertTasksForCompare(tasksFlatten);
      for (var i = 0; i < currentConvertedTasks.length; i++) {
        var task = currentConvertedTasks[i];
        if (task.isModified === true) {
          result.push(task);
        }
      }
      return result;
    }

    function clearModifiedTasks(taches) {   
      var tasksFlatten = TacheBudgetService.getAllTasksFlatten(taches);     
      for (var i = 0; i < tasksFlatten.length; i++) {
        var task = tasksFlatten[i];
        if (task.isModified === true) {
          task.isModified = false;
        }
      }    
    }  

    function convertTasksForCompare(tasks) {
      var convertedTasks = TacheBudgetService.getAllTasksFlatten(tasks);
      convertedTasks = angular.copy(convertedTasks);
      for (var i = 0; i < convertedTasks.length; i++) {
        var task = convertedTasks[i];
        convertTaskForCompare(task);
      }
      return convertedTasks;
    }


    function convertTaskForCompare(task) {
      task.TachesEnfants = [];
      task.RessourceTaches = [];
    }
  }
})();

