(function (angular) {
  'use strict';

  angular
      .module('Fred')
      .filter('toLocaleDate', function () {
         return function (input) {
          return (input !== null && input !== undefined) ? new Date(moment.utc(input).local()) : "";
        }
      });
}(angular));