(function () {
  'use strict';
  /*
   * Ce composant permet de changer de vue, sur l'ecran des taches.
   * Il y a 2 vue arbo et splitted, on peux avoir les 2 vues, une seule mais il y a tjrs une vue visible.  
   */
  angular
    .module('Fred')
    .component('referentielTacheViewSelectorComponent', {
      templateUrl: '/Areas/ReferentielTaches/Scripts/referentiel-tache-view-selector.tpl.html',
      bindings: {     
        arboViewSelected: '<',
        splittedViewSelected: '<'
      },
      controller: 'referentielTacheViewSelectorComponentController'
    });


  angular.module('Fred').controller('referentielTacheViewSelectorComponentController', referentielTacheViewSelectorComponentController);

  referentielTacheViewSelectorComponentController.$inject = ['fredSubscribeService'];

  function referentielTacheViewSelectorComponentController(fredSubscribeService) {


    var $ctrl = this;
    $ctrl.resources = resources;
  
    $ctrl.viewArbo = $ctrl.arboViewSelected;
    $ctrl.viewSplitted = $ctrl.splittedViewSelected;
    $ctrl.handleChangeModeDisplay = handleChangeModeDisplay;


    //////////////////////////////////////////////////////////////////
    // Init                                                         //
    //////////////////////////////////////////////////////////////////

    this.$onInit = function () {
    };

    function handleChangeModeDisplay(mode) {
      /*Vue Arbo*/
      if (mode === 'viewArbo') {
       $ctrl.viewArbo = manageViewArbo($ctrl.viewArbo, $ctrl.viewSplitted);
      }
      /*Vue 3/4 colonnes */
      if (mode ==='viewSplitted') {
        $ctrl.viewSplitted = manageViewSplitted($ctrl.viewArbo, $ctrl.viewSplitted);
      }
      fredSubscribeService.raiseEvent('taskPlan.viewChanged', getStateViews());
    }

    /*
     * Manage la view Arbo (vue de gauche)
     */
    function manageViewArbo(viewArbo, viewSplitted) {
      var result = viewArbo;
      if (viewArbo) {
        if (viewSplitted === true) {
          result = false;        
        }
      } else {
        result = true;      
      }
      return result;
    }

    /*
     *  Manage la view splitted (vue de droite)
     */
    function manageViewSplitted(viewArbo,viewSplitted) {
      var result = viewSplitted;
      if (viewSplitted === true) {
        if (viewArbo === true) {
          result = false;        
        }
      } else {
        result = true;     
      }
      return result;
    }

    /*
     * Retourne l'etat général
     */
    function getStateViews() {
      return {
        viewArbo: $ctrl.viewArbo,
        viewSplitted: $ctrl.viewSplitted
      };
    }


  }



})();