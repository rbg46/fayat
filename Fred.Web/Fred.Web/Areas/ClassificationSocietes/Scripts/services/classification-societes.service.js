(function () {
    'use strict';

    angular.module('Fred').service('ClassificationSocietesService', ClassificationSocietesService);

    ClassificationSocietesService.$inject = ['$http'];

    function ClassificationSocietesService($http) {

        return {
            GetClasssificationSocietyByIdGrp: function (GrpId) {
                return $http.get("/api/SocieteClassification/GetGroupeById/" + GrpId);
            },
            CreateOrUpdateListOfClass: function (ListClassSoc) {
                return $http.post("/api/SocieteClassification/CreateOrUpdateRange/Classifications", ListClassSoc);
            },
            DeleteListOfClass: function (ListClassSoc) {
                return $http.post("/api/SocieteClassification/DeleteRange/Classifications", ListClassSoc);
            }
        };
    }
})();