(function () {
  'use strict';

  angular.module('Fred').service('TaskManagerService', TaskManagerService);

  TaskManagerService.$inject = ['$log'];

  function TaskManagerService($log) {

    var tasks = [];

    var service = {
      init: init,
      addTask: addTask,
      removeTask: removeTask,
      selectTask: selectTask,
      getOptions: getOptions,
      getList1: getList1,
      getDefaultTask: getDefaultTask
    };
    return service;

    function init(taches) {
      tasks = selectAllTasksForLevel(taches, 1);
    }

    //////////////////////////////////////////////////////////////////
    // PUBLICS METHODES                                             //
    //////////////////////////////////////////////////////////////////

    function getList1() {
      return tasks;
    }

    function addTask(task) {

      var copyTask = angular.copy(task);
      var flattenTasks = getAllTasksFlatten(tasks);
      var parent = flattenTasks.find(function (t) {
        return t.TacheId === copyTask.ParentId;
      });
      if (parent) {
        parent.TachesEnfants.push(copyTask);
      } else {
        tasks.push(copyTask);
      }

    }

    function removeTask(task) {
      var flattenTasks = getAllTasksFlatten(tasks);
      var parent = flattenTasks.find(function (t) {
        return t.TacheId === task.ParentId;
      });
      if (parent) {
        var index = parent.TachesEnfants.indexOf(task);
        parent.TachesEnfants.splice(index, 1);
      } else {
        var index2 = tasks.indexOf(task);
        tasks.splice(index2, 1);
      }

    }

    function selectTask(task) {
      var result = {
        TaskLevelOne: null,
        TaskLevelTwo: null,
        TaskLevelTwree: null,
        TaskLevelFoor: null,
        TasksLevelOne: tasks,
        TasksLevelTwo: [],
        TasksLevelTwree: [],
        TasksLevelFoor: []
      };
      var flattenTasks = getAllTasksFlatten(tasks);
      var parents = findParents(flattenTasks, task);
      if (task.Niveau === 1) {
        result.TaskLevelOne = task;
        result.TasksLevelTwo = task.TachesEnfants;
      }
      if (task.Niveau === 2) {
        result.TaskLevelTwo = task;
        result.TasksLevelTwree = task.TachesEnfants;
      }
      if (task.Niveau === 3) {
        result.TaskLevelTwree = task;
        result.TasksLevelFoor = task.TachesEnfants;
      }
      if (task.Niveau === 4) {
        result.TaskLevelFoor = task;
      }

      for (var i = 0; i < parents.length; i++) {

        var parent = parents[i];
        if (parent.Niveau === 3) {
          result.TaskLevelTwree = parent;
          result.TasksLevelFoor = parent.TachesEnfants;
        }
        if (parent.Niveau === 2) {
          result.TaskLevelTwo = parent;
          result.TasksLevelTwree = parent.TachesEnfants;
        }
        if (parent.Niveau === 1) {
          result.TaskLevelOne = parent;
          result.TasksLevelTwo = parent.TachesEnfants;
        }
      }
      return result;
    }

    function getOptions(task) {
      var result = {
        levelOne: {
          availableOptions: [],
          selected: null
        },
        levelTwo: {
          availableOptions: [],
          selected: null
        },
        levelThree: {
          availableOptions: [],
          selected: null
        }
      };
      var flattenTasks = getAllTasksFlatten(tasks);
      if (task.Niveau === 1) {
        result.levelOne.availableOptions = selectAllTasksForLevel(flattenTasks, 1);
        result.levelOne.selected = task;

        result.levelTwo.availableOptions = task.TachesEnfants;
        result.levelTwo.selected = result.levelTwo.availableOptions.length > 0 ? result.levelTwo.availableOptions[0] : null;

        result.levelThree.availableOptions = result.levelTwo.selected !== null ? result.levelTwo.selected.TachesEnfants : [];
        result.levelThree.selected = result.levelThree.availableOptions.length > 0 ? result.levelThree.availableOptions[0] : null;

      }
      if (task.Niveau === 2) {
        result.levelOne.availableOptions = selectAllTasksForLevel(flattenTasks, 1);
        result.levelOne.selected = getParent(task);

        result.levelTwo.availableOptions = result.levelOne.selected.TachesEnfants;
        result.levelTwo.selected = task;

        result.levelThree.availableOptions = result.levelTwo.selected !== null ? result.levelTwo.selected.TachesEnfants : [];
        result.levelThree.selected = result.levelThree.availableOptions.length > 0 ? result.levelThree.availableOptions[0] : null;

      }

      if (task.Niveau === 3) {

        var parentNiveau2 = getParent(task);
        var parentNiveau1 = getParent(parentNiveau2);

        result.levelOne.availableOptions = selectAllTasksForLevel(flattenTasks, 1);
        result.levelOne.selected = parentNiveau1;

        result.levelTwo.availableOptions = result.levelOne.selected.TachesEnfants;
        result.levelTwo.selected = parentNiveau2;

        result.levelThree.availableOptions = result.levelTwo.selected !== null ? result.levelTwo.selected.TachesEnfants : [];
        result.levelThree.selected = task;

      }

      if (task.Niveau === 4) {
        var parentLevel3 = getParent(task);
        var parentLevel2 = getParent(parentLevel3);
        var parentLevel1 = getParent(parentLevel2);

        result.levelOne.availableOptions = selectAllTasksForLevel(flattenTasks, 1);
        result.levelOne.selected = parentLevel1;

        result.levelTwo.availableOptions = result.levelOne.selected.TachesEnfants;
        result.levelTwo.selected = parentLevel2;

        result.levelThree.availableOptions = result.levelTwo.selected.TachesEnfants;
        result.levelThree.selected = parentLevel3;

      }

      return result;
    }

    // Retourne la tâche par défaut
    // - task : tâche parente où rechercher ou null pour rechercher partout
    function getDefaultTask(task) {
      if (task === null) {
        for (var i = 0; i < tasks.length; i++) {
          var founded = this.getDefaultTask(tasks[i]);
          if (founded !== null)
            return founded;
        }
      }
      else {
        for (var j = 0; j < task.TachesEnfants.length; j++) {
          var child = task.TachesEnfants[j];
          if (child.TacheParDefaut) {
            return child;
          }

          founded = this.getDefaultTask(child);
          if (founded !== null)
            return founded;
        }
      }
      return null;
    }


    //////////////////////////////////////////////////////////////////
    // PRIVATES METHODES                                            //
    //////////////////////////////////////////////////////////////////

    /*
     *  recupere les taches à plat
     */
    function getAllTasksFlatten(taches) {
      var result = taches;
      for (var i = 0; i < taches.length; i++) {
        var tache = taches[i];
        result = result.concat(findChildrens(tache));
      }

      var flags = [], output = [], l = result.length;
      for (var k = 0; k < l; k++) {
        if (flags[result[k].TacheId]) {
          continue;
        }
        flags[result[k].TacheId] = true;
        output.push(result[k]);
      }

      return output;
    }

    /*
     * trouve les parents de la tache
     */
    function findParents(taches, tache) {
      var result = [];
      var parentId = tache.ParentId;
      while (parentId) {
        var parent = taches.find(function (t) {
          return t.TacheId === parentId;
        });
        if (parent) {
          result.push(parent);
          parentId = parent.ParentId;
        } else {
          parentId = null;
        }
      }
      return result;
    }

    // trouve les enfants de la tache
    function findChildrens(tache) {
      var result = [];
      var children = tache.TachesEnfants;

      if (children.length > 0) {
        result = result.concat(children);
        for (var i = 0; i < children.length; i++) {
          result = result.concat(findChildrens(children[i]));
        }
      }
      return result;
    }

    /* 
     * Selectionne toutes les taches pour un niveau donné.
     */
    function selectAllTasksForLevel(all, level) {
      var tasksLevel = all.filter(function (t) {
        return t.Niveau === level;
      });
      return tasksLevel;
    }

    function getParent(task) {
      var flattenTasks = getAllTasksFlatten(tasks);
      var parent = flattenTasks.find(function (t) {
        return t.TacheId === task.ParentId;
      });
      return parent;
    }

  }
})();