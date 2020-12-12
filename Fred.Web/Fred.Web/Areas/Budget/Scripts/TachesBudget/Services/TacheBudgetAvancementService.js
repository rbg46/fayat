
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('TacheBudgetAvancementService', TacheBudgetAvancementService);

  TacheBudgetAvancementService.$inject = ['TacheBudgetService'];

  function TacheBudgetAvancementService(TacheBudgetService) {

    var service = {
      modifyParentAndChildrenAvancement: modifyParentAndChildrenAvancement       
    };
    return service;

    // Decoche l'avancement pour les taches parents et enfants de la tache passée en parametre
    function modifyParentAndChildrenAvancement(taches, tacheAvancementChanged) {
      if (tacheAvancementChanged !== null) {
        var all = TacheBudgetService.getAllTasksFlatten(taches);

        var parents = TacheBudgetService.findParents(all, tacheAvancementChanged);
        for (var i = 0; i < parents.length; i++) {
          var parent = parents[i];
          if (parent.Avancement) {
            parent.isModified = true;
            parent.Avancement = false;
          }         
        }

        var children = TacheBudgetService.findChildrens(tacheAvancementChanged);
        for (var j = 0; j < children.length; j++) {
          var child = children[j];
          if (child.Avancement) {
            child.isModified = true;
            child.Avancement = false;
          }         
        }     
      }
    }  
    
  }
})();

