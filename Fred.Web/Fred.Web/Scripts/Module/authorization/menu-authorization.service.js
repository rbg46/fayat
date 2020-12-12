(function () {
  'use strict';

  angular.module('Fred').factory('menuAuthorizationService', menuAuthorizationService);

  menuAuthorizationService.$inject = ['authorizationService', 'fredSubscribeService'];

  function menuAuthorizationService(authorizationService, fredSubscribeService) {


    var PERMISSION_TYPES = {
      AffichageMenu: 1
    };

    var service = {
      filterMenus: filterMenus,
      menuAdministationIsVisible: menuAdministationIsVisible
    };

    return service;

    function filterMenus(modules) {
      var habilitation = authorizationService.get();
      var permissionOfTypeMenus = habilitation.Permissions.filter(function (permission) {
        return permission.PermissionType === PERMISSION_TYPES.AffichageMenu
          && permission.Mode !== FONCTIONNALITE_TYPE_MODE.UNAFFECTED;
      });

      for (var i = 0; i < modules.length; i++) {
        var module = modules[i];
        checkModule(habilitation.IsSuperAdmin, module, permissionOfTypeMenus);
      }
      return modules;
    }

    function checkModule(isSuperAdmin, module, permissionOfTypeMenus) {
      var modulefeatures = module.Features;
      for (var j = 0; j < modulefeatures.length; j++) {
        var feature = modulefeatures[j];
        setVisibilityOfFeature(isSuperAdmin, permissionOfTypeMenus, feature);
      }
      setVisibilityOfModule(isSuperAdmin, modulefeatures, module);
    }

    function setVisibilityOfFeature(isSuperAdmin, permissionOfTypeMenus, feature) {
      feature.isVisible = false;
      var featurePermissionKey = feature.permissionKey;
      if (isSuperAdmin === true) {
        feature.isVisible = true;
      } else {
        for (var k = 0; k < permissionOfTypeMenus.length; k++) {
          var permissionKeyOfPermissionFonctionnalite = permissionOfTypeMenus[k].PermissionKey;
          if (featurePermissionKey === permissionKeyOfPermissionFonctionnalite
            && (!feature.ModificationRigthRequired || permissionOfTypeMenus[k].Mode === FONCTIONNALITE_TYPE_MODE.WRITE)) {
            feature.isVisible = true;
            break;
          }
        }
      }
    }

    function setVisibilityOfModule(isSuperAdmin, modulefeatures, module) {
      module.isVisible = false;
      if (isSuperAdmin === true) {
        module.isVisible = true;
      } else {
        for (var k = 0; k < modulefeatures.length; k++) {
          var feature = modulefeatures[k];
          if (feature.isVisible === true) {
            module.isVisible = true;
            break;
          }
        }
      }
    }

    function menuAdministationIsVisible(modules) {
      for (var i = 0; i < modules.length; i++) {
        var module = modules[i];
        if (module.isVisible && module.position === 'right') {
          return true;
        }
      }
      return false;
    }
  }
})();