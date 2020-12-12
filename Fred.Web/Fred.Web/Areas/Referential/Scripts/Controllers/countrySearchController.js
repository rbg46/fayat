angular.module('Fred', [])
    .controller('countrySearchController', function ($scope, $http) {

      $("#searchGrid").show();

      // Recherche des pays
      $scope.searchCountry = function () {

        $http.get("/api/countries/search/" + this.searchText).success(function (data, status, headers, config) {
          $scope.countries = data;
        })
        .error(function () {
          $scope.error = resources.ErreurLoading_error;
        });
      };
    });