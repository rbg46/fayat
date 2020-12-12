(function () {
  'use strict';

  angular.module('Fred').factory('authorizationService', authorizationService);
  
  function authorizationService() {

    var service = {
      get: get,
      getPermission: getPermission,
      currentUserIsSuperAdmin: currentUserIsSuperAdmin,
      currentUserSocieteId: currentUserSocieteId,
      getRights: getRights
    };

    return service;

    function get() {
      return fredHabilitation;
    }

    /**
     * Permet de savoir si nous avons la permission passée en parametre.
     * @param {any} permissionKey Identifiant de la permission
     * @returns {any} Permission de l'utilisateur pour la clef indiquée
     */
    function getPermission(permissionKey) {

      var result = fredHabilitation.PermissionsContextuelles.filter(function (availablePermission) {
        return availablePermission.PermissionKey === permissionKey;
      });

      if (result.length > 0) {
        return result[0];
      }

      result = fredHabilitation.Permissions.filter(function (availablePermission) {
        return availablePermission.PermissionKey === permissionKey;
      });

      if (result.length > 0) {
        return result[0];
      }

      return null;
    }

    /**
     * Récupération des booléens isVisible et isReadOnly pour une fonctionnalité
     * @param {any} key clé permission
     * @returns {any} Permission de l'utilisateur pour la clef indiquée
     */
    function getRights(key) {
      var result = { isVisible: false, isReadOnly: true, isSuperAdmin: false };
      if (currentUserIsSuperAdmin()) {
        result.isVisible = true;
        result.isReadOnly = false;
        result.isSuperAdmin = true;
      } else {
        var permission = getPermission(key);

        if (permission !== null) {
          switch (permission.Mode) {
            case FONCTIONNALITE_TYPE_MODE.UNAFFECTED:
              result.isVisible = false;
              result.isReadOnly = true;
              break;
            case FONCTIONNALITE_TYPE_MODE.READ:
              result.isVisible = true;
              result.isReadOnly = true;
              break;
            case FONCTIONNALITE_TYPE_MODE.WRITE:
              result.isVisible = true;
              result.isReadOnly = false;
              break;
          }
        }
      }

      return result;
    }

    /*
     * Determine si l'utilisateur est 'super admin'
     */
    function currentUserIsSuperAdmin() {
      return fredHabilitation.IsSuperAdmin;
    }

    /*
     * retourne l' ide de la societe de l'utilisateur courant.
     */
    function currentUserSocieteId() {
      return fredHabilitation.SocieteId;
    }
  }
})();