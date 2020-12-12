(function () {
  'use strict';

  angular.module('Fred').service('AffectationService', AffectationService);

  AffectationService.$inject = ['$http', '$q', '$resource'];

  function AffectationService($http, $q, $resource) {
    var vm = this;
    var uriBase = "/api/Affectation/";

    var resource = $resource(uriBase + ':cmd',
      {}, //parameters default
      {
        GetAffectationListByCi: {
          method: "GET",
          url: uriBase + "GetAffectationListByCi/:ciId/:dateDebut/:dateFin",
          params: { ciId: 0, dateDebut: '2018-06-25', dateFin: '2018-07-01' },
          isArray: false,
          cache: false
        },
        DeleteAffectations: {
          method: "POST",
          url: uriBase + "DeleteAffectations/:ciId",
          params: {},
          isArray: true,
          cache: false     
        },
        GetEquipePersonnelsByProprietaireId: {
          method: "GET",
          url: "/api/Equipe/GetEquipeByProprietaireId",
          params: {},
          isArray: true,
          cache: false
        },
        AddOrUpdateAffectationList: {
          method: "POST",
          url: uriBase + "AddOrUpdateAffectationList",
          params: {},
          isArray: false,
          cache: false
        },
        CheckPersonnelBeforDelete: {
         method: "Get",
         url: uriBase + "CheckPersonnelBeforDelete/:personnelId/:ciId",
         params: { personnelId: 0, ciId:0},
         isArray: false
        }
      }
    );

    angular.extend(vm, resource);
  }
})();