(function () {
  'use strict';

  angular.module('Fred').service('ParamTarifsReferentielsService', ParamTarifsReferentielsService);

  ParamTarifsReferentielsService.$inject = ['$http', '$q'];

  function ParamTarifsReferentielsService($http, $q) {
    this.getListOrganisations = function () {
      return $http.get('/api/Organisation/GetList');
    }

    this.getParentOrganisations = function (id) {
      return $http.get('/api/Organisation/GetParents/' + id);
    }

    this.getTarifsReferentiels = function (id, deviseId, filter) {
      return $http.get('/api/ParamTarifsReferentiels?id=' + id + '&deviseId=' + deviseId + '&filter=' + filter);
    }

    this.getListDevises = function (orgaId) {
      return $http.get('/api/ParamTarifsReferentiels/ListDevises/' + orgaId);
    }

    this.delete = function (param) {    
      var deferred = $q.defer();
      $http.delete('/api/ParamTarifsReferentiels/DeleteParam/' + param.ParametrageReferentielEtenduId)
        .then(function (response) {
          deferred.resolve({
            row: param,
            data: response.data
          });
        }).catch(function (error) {
          deferred.reject(error);
        });
      return deferred.promise;
    }

    this.save = function (param) {      
      var deferred = $q.defer();
      $http.post('/api/UpdateParametrages', param)
        .then(function (response) {
          deferred.resolve({
            row: param,
            data: response.data
          });
        }).catch(function (error) {
          deferred.reject(error);
        });
      return deferred.promise;
    }   
  }

})();