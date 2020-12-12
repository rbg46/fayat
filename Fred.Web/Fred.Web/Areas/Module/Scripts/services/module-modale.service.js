(function () {
  "use strict";


  angular.module('Fred').service('ModuleModalService', ModuleModalService);

  ModuleModalService.$inject = ['$uibModal', '$filter', 'ModuleService', 'Notify'];


  function ModuleModalService($uibModal, $filter, ModuleService, Notify) {

    return {
      openCreateModuleModal: function (moduleList, societeSelectedId) {
        var modalInstance = $uibModal.open({
          templateUrl: "/Areas/Module/Scripts/modals/module/module-modal.tpl.html",
          backdrop: "static",
          controller: 'openCreateModuleModalController',
          size: "md",
          resolve: {
            moduleList: function () {
              return moduleList;
            },
            societeSelectedId: function () {
              return societeSelectedId;
            }
          }
        });
        return modalInstance.result;
      },

      openEditModuleModal: function (selectedModule, societeSelectedId) {
        var modalInstance = $uibModal.open({
          templateUrl: "/Areas/Module/Scripts/modals/module/module-modal.tpl.html",
          backdrop: "static",
          controller: 'openEditModuleModalController',
          size: "md",
          resolve: {
            selectedModule: function () {
              return selectedModule;
            },
            societeSelectedId: function () {
              return societeSelectedId;
            }
          }
        });
        return modalInstance.result;
      },

      openCreateFeatureModal: function (moduleList, featureList, selectedModule, parentScope) {
        var modalInstance = $uibModal.open({
          templateUrl: "/Areas/Module/Scripts/modals/feature/feature-modal.tpl.html",
          backdrop: "static",
          controller: 'openCreateFeatureModalController',
          size: "md",
          resolve: {
            moduleList: function () {
              return moduleList;
            },
            featureList: function () {
              return featureList;
            },
            selectedModule: function () {
              return selectedModule;
            }
          }
        });
        // Sélectionne la fonctionnalité créee
        modalInstance.result.then(parentScope.handleSelectFeature);
      },

      openEditFeatureModal: function (feature, selectedModule, parentScope) {
        var modalInstance = $uibModal.open({
          templateUrl: "/Areas/Module/Scripts/modals/feature/feature-modal.tpl.html",
          backdrop: "static",
          controller: 'openEditFeatureModalController',
          size: "md",
          resolve: {
            feature: function () {
              return feature;
            },
            selectedModule: function () {
              return selectedModule;
            }
          }
        });
        // Sélectionne la fonctionnalité mise à jour
        modalInstance.result.then(parentScope.handleSelectFeature);
      }
    };
  };


})();