(function () {
    'use strict';

    angular
      .module('Fred')
      .component('fredHorloge', {
          templateUrl: '/Scripts/Controllers/Horloge/fred-horloge.html',
          bindings: {
          },
          controller: 'fredHorlogeController'
      });


    angular
     .module('Fred')
     .controller('fredHorlogeController', fredHorlogeController);

    fredHorlogeController.$inject = ['$scope'];

    function fredHorlogeController($scope) {

        var $ctrl = this;
        $ctrl.date = "";
        $ctrl.$onInit = function () {
            var date = new Date();
            $ctrl.title = "Attention : Information calculée à partir de votre horloge";
           
           $ctrl.day = (date.getDate() < 10 ? '0' : '') + date.getDate();
            $ctrl.day += '/' + ((date.getMonth() + 1) < 10 ? '0' : '') + (date.getMonth() + 1);
            $ctrl.day += '/' + date.getFullYear();
            $ctrl.hour = date.getHours();
            $ctrl.hour += ':' + (date.getMinutes() < 10 ? '0' : '') + date.getMinutes();

        };
        

        


    }

})();

