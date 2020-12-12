
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('BudgetMathService', BudgetMathService);

  BudgetMathService.$inject = ['TacheBudgetService'];

  function BudgetMathService(TacheBudgetService) {
   

    var service = {
      add: add,
      toFixed: toFixed
    };

    return service;

    //////////////////////////////////////////////////////////////////
    // Publics methodes                                             //
    //////////////////////////////////////////////////////////////////


    /*
     * fait une addition sans les probleme d'arrondi du javascript
     */
    function add(left, right) {
      var addition = left + right;
      var additionFixed = addition.toFixed(2);
      var result = parseFloat(additionFixed);
      return result;
    }

    /*
     * Permet de faire un arrondi sur une operation mathematique.
     */
    function toFixed(mathematicalCallBack) {
      var mathematicalResult = mathematicalCallBack();
      var fixed = mathematicalResult.toFixed(2);
      var result = parseFloat(fixed);
      return result;
    }

  }
})();

