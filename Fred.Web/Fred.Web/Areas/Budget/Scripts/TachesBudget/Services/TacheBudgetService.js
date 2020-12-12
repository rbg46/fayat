﻿
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('TacheBudgetService', TacheBudgetService);

  TacheBudgetService.$inject = ['$log'];

  function TacheBudgetService($log) {

    var service = {
      getAllTasksFlatten: getAllTasksFlatten,
      findParents: findParents,
      findChildrens: findChildrens,
      selectAllTasksForLevel: selectAllTasksForLevel,
      replaceTaskT4InParent: replaceTaskT4InParent
    };
    return service;


    function replaceTaskT4InParent(tasks, task) {
      var all = getAllTasksFlatten(tasks);
      var parent = all.find(function (t) {
        return t.TacheId === task.ParentId;
      });

      var childrenOfParent = parent.TachesEnfants;
      var index = childrenOfParent.map(function (e) { return e.TacheId; }).indexOf(task.TacheId);
      if (index !== -1) {
        parent.TachesEnfants[index] = task;
      } else {
        $log.warn('Cas normalement impossible(TacheBudgetService.replaceTaskT4InParent)');
      }
    }

   

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

    /* todo a refacto car doublon
     * Selectionne toutes les taches pour un niveau donné.
     */
    function selectAllTasksForLevel(all, level) {
      var tasksLevel = all.filter(function (t) {
        return t.Niveau === level;
      });
      return tasksLevel;
    }

  }
})();

