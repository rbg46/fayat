
(function (angular) {
  'use strict';

  angular.module('Fred').controller('RapprochementFactureController', RapprochementFactureController);

  RapprochementFactureController.$inject = ['$q', '$timeout', '$scope'];

  function RapprochementFactureController($q, $timeout, $scope) {

    // assignation de la valeur du scope au controller pour les templates non mis à jour
    var $ctrl = this;

    // méthodes exposées
    //angular.extend($ctrl, {
    //    handleClickOpenCloseBudgetRessources: handleClickOpenCloseBudgetRessources,
    //    handleClickChangedCI: handleClickChangedCI,
    //    handleCancelTask: handleCancelTask,
    //    handleCancelTasks: handleCancelTasks,
    //    handleSaveTask: handleSaveTask,
    //    handleSaveTasks: handleSaveTasks
    //});

    init();

    return $ctrl;


    /**
     * Initialisation du controller.
     * 
     */
    function init() {

      //angular.extend($ctrl, {
      //    saving: false,
      //    taches: [],
      //    view: 'initialView',

      //    //ci selectionnée dans la picklist
      //    ciSelected: null,
      //    //Tache selectionnée
      //    taskSelected: null,
      //    showBudgetRessources: false,
      //    showButtonBudgetRessources: false,
      //});

      $ctrl.test = "hello";

      $scope.$broadcast('budgetCrtl.displayList', 'hello');

      var demo, fixedTable;

      fixedTable = function (el) {
        var $body, $header, $sidebar;
        $body = $(el).find('.fixedTable-body');
        $sidebar = $(el).find('.fixedTable-sidebar table');
        $header = $(el).find('.fixedTable-header table');
        return $($body).scroll(function () {
          $($sidebar).css('margin-top', -$($body).scrollTop());
          return $($header).css('margin-left', -$($body).scrollLeft());
        });
      };

      fixedTable($('#popDepTable'));
      fixedTable($('#popDepTable2'));

      // redimmension de la pop in de recherche des dépenses
      $('.popRechDepContent').resizable({
        handles: 'n, s'
      });

      // déplacement de la pop in de recherche des dépenses
      $('.popRechDepDialog').draggable();

      // redimmensionnement automatique du tableau du bandeau
      var x = $(window).height();

      $('.bandeauS').css('height', x - 400);
      $('.bandeauB').css('height', x - 400);

    }



    //////////////////////////////////////////////////////////////////
    // Handlers                                                     //
    //////////////////////////////////////////////////////////////////

    function handleClickChangedCI() {
      actionLoad();
    }

    function handleSaveTasks() {
      $scope.$broadcast('budgetCrtl.SaveTasks');
    }

    function handleCancelTasks() {

    }

    function handleCancelTask() {
      changeView('listingView');
    }

    function handleSaveTask() {
      $scope.$broadcast('budgetCrtl.SaveTask');
    }

    function handleClickOpenCloseBudgetRessources() {
      var view = $ctrl.view === "detailView" ? "detailWithRessourcesView" : "detailView";
      changeView(view);
      $scope.$broadcast('loadBudgetRessources');
    }






    //////////////////////////////////////////////////////////////////
    // Actions                                                      //
    //////////////////////////////////////////////////////////////////


    function actionLoad() {
      $scope.$broadcast('loadTasks', $ctrl.ciSelected);
      changeView("listingView");
    }

    function changeView(view) {
      if (view === "initialView") {
        $ctrl.showDetail = false;
        $ctrl.showBudgetRessources = false;
        $ctrl.showButtonBudgetRessources = false;
        $ctrl.showListing = false;
        $ctrl.view = view;
      }
      if (view === "listingView") {
        $ctrl.showDetail = false;
        $ctrl.showBudgetRessources = false;
        $ctrl.showButtonBudgetRessources = false;
        $ctrl.showListing = true;
        $scope.$broadcast('changedTaskListView', "LargeList");
        $ctrl.view = view;
      }
      if (view === "detailView") {
        $ctrl.showDetail = true;
        $ctrl.showBudgetRessources = false;
        $ctrl.showButtonBudgetRessources = true;
        $ctrl.showListing = true;
        $scope.$broadcast('changedTaskListView', "SmallList");
        $ctrl.view = view;
      }
      if (view === "detailWithRessourcesView") {
        $ctrl.showDetail = true;
        $ctrl.showBudgetRessources = true;
        $ctrl.showButtonBudgetRessources = true;
        $ctrl.showListing = true;
        $scope.$broadcast('changedTaskListView', "SmallList");
        $ctrl.view = view;
      }
    }


  }
}(angular));
