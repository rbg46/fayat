/// <reference path="../models/code-role.model.js" />
(function () {
  "use strict";


  angular.module('Fred').service("roleProviderService", roleProviderService);

  roleProviderService.$inject = [];

  function roleProviderService() {

    this.getRoles = function () {
      return [CODE_ROLE.ADM,
              CODE_ROLE.ARH,
              CODE_ROLE.CDC,
              CODE_ROLE.CDS,
              CODE_ROLE.CDT,
              CODE_ROLE.CSP,
              CODE_ROLE.DRA,
              CODE_ROLE.DRC,
              CODE_ROLE.GSP
      ];
    }


  };


})();