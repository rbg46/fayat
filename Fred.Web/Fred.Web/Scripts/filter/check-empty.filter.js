(function (angular) {
  'use strict';

  angular
      .module('Fred')
      .filter('checkEmpty', function () {
         return function(input){
            if(angular.isString(input) && !(angular.equals(input,null) || angular.equals(input,'')))
                return input;
            else
                return '-';
        };
      });
})(angular);