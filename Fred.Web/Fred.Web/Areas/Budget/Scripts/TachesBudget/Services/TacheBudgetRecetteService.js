
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('TacheBudgetRecetteService', TacheBudgetRecetteService);

  TacheBudgetRecetteService.$inject = ['TacheBudgetService', 'BudgetMathService'];

  function TacheBudgetRecetteService(TacheBudgetService, BudgetMathService) {


    var service = {
      modifyParentAndChildrenRecette: modifyParentAndChildrenRecette,
      calculRecetteTotal: calculRecetteTotal,
      setMontantRecetteByDevise: setMontantRecetteByDevise,
      getMontantRecetteByDevise: getMontantRecetteByDevise
    };
    return service;


    //calcul de la recette totale a partir des taches de niveau 1
    function calculRecetteTotal(taches, deviseSelected) {

      var result = {
        devise: deviseSelected,
        recette: 0
      };
      for (var j = 0; j < taches.length; j++) {
        var tache = taches[j];     

        var tacheRecette = getTaskRecetteByDevise(tache, deviseSelected);

        if (tacheRecette !== undefined && tacheRecette !== null) {        
          result.recette = BudgetMathService.add(result.recette, tacheRecette.Recette);
        } else {         
          result.recette = BudgetMathService.add(result.recette, 0);
        }
      }
      return result;
    }


    //Calcul la recette pour les taches parents et enfants de la tache passée en parametre
    function modifyParentAndChildrenRecette(taches, tacheRecetteChanged, deviseSelected) {

      var all = TacheBudgetService.getAllTasksFlatten(taches);

      //Remise a zero des recettes enfants de la tache modifiée.
      var children = TacheBudgetService.findChildrens(tacheRecetteChanged);
      for (var j = 0; j < children.length; j++) {
        var child = children[j];
        var childRecette = getOrCreateRecetteByDevise(child, deviseSelected);
        child.Recette = 0;      
        child.isModified = true;                
        childRecette.Recette = 0;
      }

      //Modification des recettes parents tant qu'il y a un parent
      //quand il n'y a plus de parent => niveau 1, alors pas besoin de recalculer, la valeur saisie est la recette
      // et comme nous avons mis a zero les recettes enfants , il n'y a plus rien a faire
      var parentId = tacheRecetteChanged.ParentId;
      while (parentId) {

        var parent = all.find(function (t) {
          return t.TacheId === parentId;
        });

        if (parent) {
          ModifyRecetteParent(parent, deviseSelected);
          parentId = parent.ParentId;
        } else {
          parentId = null;
        }
      }
    }

    //Modification de la recette parent
    function ModifyRecetteParent(parent, deviseSelected) {

      var recetteParent = getOrCreateRecetteByDevise(parent, deviseSelected);
      recetteParent.Recette = 0;
      parent.isModified = true;
      parent.Recette = 0;
           
      var children = TacheBudgetService.findChildrens(parent);
      for (var j = 0; j < children.length; j++) {
        var child = children[j];
        var niveauInferieur = parent.Niveau + 1;
        if (child.Niveau === niveauInferieur) {

          var childRecette = getOrCreateRecetteByDevise(child, deviseSelected);

          var nouvelleRecette = BudgetMathService.add(childRecette.Recette, recetteParent.Recette);
          parent.Recette = nouvelleRecette;        
          recetteParent.Recette = nouvelleRecette;
        }
      }
    }



    function getMontantRecetteByDevise(task, deviseSelected) {
      var result = 0;

      var tacheRecette = getTaskRecetteByDevise(task, deviseSelected);

      if (tacheRecette !== undefined && tacheRecette !== null) {
        result = tacheRecette.Recette;
      }
      return result;
    }


    function setMontantRecetteByDevise(task, deviseSelected, montant) {
      var tacheRecette = getOrCreateRecetteByDevise(task, deviseSelected);
      if (!montant) {
        tacheRecette.Recette = null;
      } else {
        tacheRecette.Recette = montant;
      }

    }


    function getOrCreateRecetteByDevise(task, deviseSelected) {
      var result = null;
      result = getTaskRecetteByDevise(task, deviseSelected);

      if (result === undefined || result === null) {
        result = {
          TacheRecetteId: 0,
          DeviseId: deviseSelected.DeviseId,
          TacheId: task.TacheId,
          Recette: 0
        };
        task.TacheRecettes.push(result);
      }
      return result;
    }

    function getTaskRecetteByDevise(task, deviseSelected) {
      var taskRecette = task.TacheRecettes.find(function (recette) {
        return recette.DeviseId === deviseSelected.DeviseId;
      });
      return taskRecette;
    }

  }
})();

