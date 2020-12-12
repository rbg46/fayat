(function () {
  "use strict";


  angular.module('Fred').service("RoleModalService", RoleModalService);

  RoleModalService.$inject = ['$uibModal', '$timeout', 'Notification', 'RoleService', 'thresholdValidator'];

  function RoleModalService($uibModal, $timeout, Notification, RoleService, thresholdValidator) {
    return {
      openEditModal: function (item, societeId) {
        var modalInstance = $uibModal.open({
          templateUrl: "/Areas/Role/Scripts/modals/role/role-modal.tpl.html",
          backdrop: "static",
          controller: 'EditRoleModalController',
          size: "md",
          resolve: {
            societeId: function () {
              return societeId;
            },
            item: function () {
              return item;
            }
          }
        });
        return modalInstance.result;
      },

      openCreateModal: function (items, societeId) {
        $uibModal.open({
          templateUrl: "/Areas/Role/Scripts/modals/role/role-modal.tpl.html",
          backdrop: "static",
          controller: 'CreateRoleModalController',
          size: "md",
          resolve: {
            societeId: function () {
              return societeId;
            },
            items: function () {
              return items;
            }
          }
        });
      },

      openDuplicateRoleModal: function (role, roles, societeId) {
        $uibModal.open({
          templateUrl: "/Areas/Role/Scripts/modals/role-duplicate/duplicate-role-modal.tpl.html",
          backdrop: "static",
          controller: 'DuplicateRoleModalController',
          size: "md",
          resolve: {
            societeId: function () {
              return societeId;
            },
            roles: function () {
              return roles;
            },
            role: function () {
              return role;
            }
          }
        });
      },


      openAddRoleFeatureModal: function (selectedRoleId, societeId) {
        var modalInstance = $uibModal.open({
          templateUrl: "/Areas/Role/Scripts/modals/role-fonctionnalite/create-role-fonctionnalite-modal.tpl.html",
          backdrop: "static",
          controller: 'CreateRoleFonctionnaliteModalController',
          controllerAs: 'ctrl',
          size: "md",
          resolve: {
            selectedRoleId: function () {
              return selectedRoleId;
            },
            societeId: function () {
              return societeId;
            }
          }
        });
        return modalInstance.result;
      },    

      openCreateSeuilValidationModal: function (items, roleId, moduleId, parentScope, selectedRole) {
        $uibModal.open({
          templateUrl: "/Areas/Role/Scripts/modals/role-seuil-validation/create-role-seuil-validation-modal.tpl.html",
          windowClass: 'ontop',
          backdrop: "static",
          controller: 'CreateRoleSeuilValidationModalController',
          size: "md",
          resolve: {
            items: function () {
              return items;
            },
            roleId: function () {
              return roleId;
            },
            selectedRole: function () {
              return selectedRole;
            },
            moduleId: function () {
              return moduleId;
            },
            parentScope: function () {
              return parentScope;
            }
          }
        });
      },

      openEditSeuilValidationModal: function (item, parentScope, selectedRole) {
        $uibModal.open({
          templateUrl: "/Areas/Role/Scripts/modals/role-seuil-validation/create-role-seuil-validation-modal.tpl.html",
          backdrop: "static",
          windowClass: 'ontop',
          controller: 'EditRoleSeuilValidationModalController',
          size: "md",
          resolve: {
            item: function () {
              return item;
            },
            selectedRole: function () {
              return selectedRole;
            },
            roleId: function () {
              return item.RoleId;
            },
            parentScope: function () {
              return parentScope;
            }
          }
        });
      },

      openRoleFonctionnaliteDetail: function (roleFonctionnaliteId) {
        $uibModal.open({
          templateUrl: "/Areas/Role/Scripts/modals/role-fonctionnalite/role-fonctionnalite-detail.tpl.html",
          backdrop: "static",
          windowClass: 'ontop',
          controllerAs: 'ctrl',
          controller: 'roleFonctionnaliteDetailController',
          size: "md",
          resolve: {
            roleFonctionnaliteId: function () {
              return roleFonctionnaliteId;
            }
             
          }
        });
      }
    };
  };


})();