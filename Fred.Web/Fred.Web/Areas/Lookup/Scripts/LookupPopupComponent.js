(function () {
  'use strict';
 
  angular
    .module('Fred')
    .component('lookupPopupComponent', {
      templateUrl: '/Areas/Lookup/Scripts/LookupPopupTemplate.html',
      bindings: {
        resources: '<',
        resolve: '<',
        close: '&',
        dismiss: '&'
      },
      controller: function () {

        var $ctrl = this;

       
        $ctrl.initLookup3 = initLookup3;
        $ctrl.onSelectLookup3 = onSelectLookup3;
        $ctrl.onDeleteLookup3 = onDeleteLookup3;
        
        $ctrl.$onInit = function () {
          $ctrl.searchTitle = 'Recherchez quelque chose';
          $ctrl.Placeholder = 'Tapez quelque chose';
          $ctrl.CILookup3 = null;
        };

      
      


        function initLookup3() {
          return '/api/Ressource/SearchLight?societeId=1';

        }



        function onSelectLookup3(lookupName, item) {
          console.log('onSelectLookup3 call with');
          console.log(lookupName);
          console.log(item);
        }



        function onDeleteLookup3() {
          $ctrl.CILookup3 = null;
        }



      }
    });

})();