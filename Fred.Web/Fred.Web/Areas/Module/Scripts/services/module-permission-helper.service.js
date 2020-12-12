(function () {
  "use strict";


  angular.module('Fred').service('ModulePermissionHelperService', ModulePermissionHelperService);

  ModulePermissionHelperService.$inject = ['ModuleService', 'Notify','$q'];

  function ModulePermissionHelperService(ModuleService, Notify,$q) {


    var service = {
      // Gestion des Modules
      canAddPermissionFonctionnalite: canAddPermissionFonctionnalite,
      addPermissionFonctionnalite: addPermissionFonctionnalite,
      canDeletePermissionFonctionnalite: canDeletePermissionFonctionnalite,
      deletePermissionFonctionnalite: deletePermissionFonctionnalite,

    };

    return service;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////// AJOUT //////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////


    function canAddPermissionFonctionnalite(permissionFonctionnaliteList, permission, isBusy) {
      var deferred = $q.defer();
      if (isBusy) {
        deferred.resolve({ canAdd: false });      
      }
      var elementWithSameId = permissionFonctionnaliteList.filter(function (pf) {
        return pf.Permission.PermissionId === permission.PermissionId;
      });
      if (elementWithSameId.length >= 1) {
        deferred.resolve({ canAdd: false });
      }

      ModuleService.CanAddPermissionFonctionnalite(permission.PermissionId)
                   .then(function (response) {
                     deferred.resolve({ canAdd: response.data.canAdd });
                   }).catch(function () {
                     deferred.resolve({ canAdd:false });
                   });

      return deferred.promise;
    }

    function addPermissionFonctionnalite(permissionFonctionnaliteList, permission, selectedFeature) {
      return ModuleService.AddPermissionFonctionnalite({ permissionId: permission.PermissionId, fonctionnaliteId: selectedFeature.FonctionnaliteId })
                     .then(manageResponseWithList(permissionFonctionnaliteList, permission))
                     .catch(Notify.defaultError);
    }

    function manageResponseWithList(permissionFonctionnaliteList, permission) {
      return function (response) {
        var newpermissionFonctionnalite = response.data;
        newpermissionFonctionnalite.Permission = permission;
        newpermissionFonctionnalite.Code = permission.Code;
        newpermissionFonctionnalite.Libelle = permission.Libelle;
        permissionFonctionnaliteList.push(response.data);
      }
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////// SUPPRESSION ////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    function canDeletePermissionFonctionnalite(permissionFonctionnaliteToDelete, isBusy) {
      if (isBusy) {
        return false;
      }
      return true;
    }

    function deletePermissionFonctionnalite(permissionFonctionnaliteList, permissionFonctionnaliteToDelete) {
      return ModuleService.deletePermissionFonctionnalite(permissionFonctionnaliteToDelete.PermissionFonctionnaliteId)
                     .then(manageDeleteResponseWithList(permissionFonctionnaliteList, permissionFonctionnaliteToDelete))
                     .catch(Notify.defaultError);
    }

    function manageDeleteResponseWithList(permissionFonctionnaliteList, permissionFonctionnaliteToDelete) {
      return function (response) {
        var index = permissionFonctionnaliteList.indexOf(permissionFonctionnaliteToDelete);
        if (index > -1) {
          permissionFonctionnaliteList.splice(index, 1);
        }
      }
    }
  }

})();