/*
 * Ce service permet de mettre a jour la liste des taches (ajout , suppression et modification)
 * lorsque le plan de taches est modifié.
 */
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('TacheBudgetManagerService', TacheBudgetMontantService);

  TacheBudgetMontantService.$inject = ['TacheBudgetService', '$log'];

  function TacheBudgetMontantService(TacheBudgetService, $log) {

    var service = {
      manage: manage
    };
    return service;

    function manage(tasks, eventInfo) {

      var eventName = eventInfo.event;

      switch (eventName) {
        case "add":
          add(tasks, eventInfo.task);
          break;
        case "edit":
          modify(tasks, eventInfo.task);
          break;
        case "delete":
          del(tasks, eventInfo.task);
          break;
        default:
          $log.warn('Evenement non géré dans TacheBudgetManagerService.manage');
      }

    }

    /*
    * Gere le cas ou on rajoute une tache dans le plan de taches
    */
    function add(tasks, task) {

      if (task.Niveau === 1) {
        tasks.push(task);
      } else {
        var all = TacheBudgetService.getAllTasksFlatten(tasks);
        var parent = all.find(function (t) {
          return t.TacheId === task.ParentId;
        });
        if (parent) {

          var newTask = angular.copy(task);
          newTask.Parent = null;
          newTask.BudgetRevision = null;
          parent.TachesEnfants.push(newTask);

        } else {
          $log.warn('Evenement non géré dans TacheBudgetManagerService.add : tache parente non existante dans le budget.');
        }

      }

    }


    /*
     * Gere le cas ou on modifie une tache dans le plan de taches
     */
    function modify(tasks, updatedtask) {

      var all = TacheBudgetService.getAllTasksFlatten(tasks);

      var task = all.find(function (t) {
        return t.TacheId === updatedtask.TacheId;
      });
      if (task) {
        task.Code = updatedtask.Code;
        task.Libelle = updatedtask.Libelle;
        task.Active = updatedtask.Active;
      } else {
        $log.warn('Evenement non géré dans TacheBudgetManagerService.modify : tache non existante dans le budget.');
      }

    }



    /*
     * Gere le cas ou on supprime une tache dans le plan de taches
     */
    function del(tasks, task) {

      if (task.Niveau === 1) {
        var index = getIndex(tasks, task.TacheId);
        tasks.splice(index, 1);
      } else {
        var all = TacheBudgetService.getAllTasksFlatten(tasks);

        var parent = all.find(function (t) {
          return t.TacheId === task.ParentId;
        });
        if (parent) {
          var indexx = getIndex(parent.TachesEnfants, task.TacheId);
          if (indexx !== -1) {
            parent.TachesEnfants.splice(indexx, 1);
          }
        } else {
          $log.warn('Evenement non géré dans TacheBudgetManagerService.del : tache parente non existante dans le budget.');
        }

      }

    }


    function getIndex(tasksList, tacheId) {
      var index = tasksList.map(function (e) { return e.TacheId; }).indexOf(tacheId);
      return index;
    }

  }
})();

