(function () {
  'use strict';

  angular.module('Fred').service('PermissionsService', PermissionsService);

  PermissionsService.$inject = ['$http'];

  /**
   * Service des permissions Fred
   * @param {any} $http http module
   */
  function PermissionsService($http) {
    /**
     * Get a contextual authorization for CI
     * @param {any} ciId CI identifier
     * @param {any} permissionKey Key of the authorization
     * @return {any} The contextual permission
     */
    this.getContextualAuthorization = function (ciId, permissionKey) {
      return $http.get('/api/CI/GetContextualAuthorization/' + ciId + '/' + permissionKey);
    };

    /**
     * Write in the DOM the contextual permission
     * @param {any} permissionKey Key of the permission
     * @param {any} modeAuthorization Datas of the permission
     * @param {any} elementSelector CSS selector ; if specified, the script will be located as a child of the element
     */
    this.registerContextualPermission = function (permissionKey, modeAuthorization, elementSelector = 'div.content') {
      if (modeAuthorization !== null) {

        // Find the existing permission in the DOM
        var permissionScript = document.querySelector("script[name='fredHabilitationPermissionsContextuelles']");
        const scriptValueStart = " fredHabilitation.PermissionsContextuelles=";

        // Initialize the new permission
        var permission = {
          PermissionId: modeAuthorization.PermissionId,
          PermissionKey: permissionKey,
          PermissionType: modeAuthorization.PermissionType,
          Mode: modeAuthorization.Mode
        };

        var permissionsContextuelles = null;
        if (permissionScript === null) {
          permissionsContextuelles = [permission];

          var contentDiv = document.querySelector(elementSelector);
          permissionScript = document.createElement("script");
          permissionScript.setAttribute('name', 'fredHabilitationPermissionsContextuelles');
          contentDiv.appendChild(permissionScript);
        } else {
          // If there's already a script, we search the permission ; if it exists, we first delete it. Then we add it.
          permissionsContextuelles = JSON.parse(permissionScript.innerHTML.substring(scriptValueStart.length));
          var existingPermission = permissionsContextuelles.find(function (p) {
            return p.PermissionId === permission.PermissionId
          });

          if (existingPermission !== null) {
            permissionsContextuelles.splice(permissionsContextuelles.indexOf(existingPermission));
          }
          permissionsContextuelles.push(permission);
        }

        permissionScript.innerHTML = scriptValueStart + JSON.stringify(permissionsContextuelles);
      }
    };
  }
})();