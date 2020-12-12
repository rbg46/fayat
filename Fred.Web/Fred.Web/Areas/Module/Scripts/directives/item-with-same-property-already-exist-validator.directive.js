(function () {
  "use strict";

  angular.module('Fred').directive('itemWithSamePropertyAlreadyExistValidator', [function () {
    return {
      require: 'ngModel',
      link: function (scope, elem, attr, ngModel) {
        var listToCheck = scope.$eval(attr.listToCheck);
        var propertyName =  attr.propertyName;

        //For DOM -> model validation
        ngModel.$parsers.unshift(function (value) {        
          var valid = !alreadyExist(value, propertyName, listToCheck);
          ngModel.$setValidity('alreadyExist', valid);
          return valid ? value : undefined;
        });

        //For model -> DOM validation
        ngModel.$formatters.unshift(function (value) {
          var valid = !alreadyExist(value, propertyName, listToCheck);
          ngModel.$setValidity('alreadyExist', valid);
          return value;
        });

        function alreadyExist(model, propertyName, list) {
          if (!model || !propertyName || !list) {
            return false;
          }
          for (var i = 0; i < list.length; i++) {
            var item = list[i];
            if (item && item[propertyName]) {
              var propetyValue = item[propertyName];
              if (propetyValue.toLowerCase() === model.toLowerCase()) {
                return true;
              }
            }           
          }
          return false;         
        }



      }
    };
  }]);

})();