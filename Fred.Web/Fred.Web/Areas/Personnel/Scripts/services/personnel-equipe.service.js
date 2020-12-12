(function () {
  'use strict';

  angular.module('Fred').service('PersonnelEquipeService', PersonnelEquipeService);

  PersonnelEquipeService.$inject = ['$http', '$q', '$resource'];

  function PersonnelEquipeService($http, $q, $resource) {
    var vm = this;
    var uriBase = "api/Equipe/";

    var resource = $resource(uriBase + ':cmd/:ouvrierId',
      {}, //parameters default
      {
        // Retourner la liste des ouvriers appartenant à la liste
        GetEquipePersonnelsByProprietaireId: {
          method: "GET",
          url: "/api/Equipe/GetEquipeByProprietaireId",
          params: {},
          isArray: true,
          cache: false
        },
        // Ajout ou suppression des ouvriers d'une équipe
        ManageEquipePersonnels: {
          method: "POST",
          url: "/api/Equipe/ManageEquipePersonnels",
          params: {},
          isArray: false,
        }
      }
    );

    angular.extend(vm, resource);
  }
})();