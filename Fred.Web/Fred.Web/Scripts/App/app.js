/* -------------------------------------------------
 * FAYAT CONSTRUCTION INFORMATIQUE 
 * -------------------------------------------------
 * FRED / BPE / 20161104
 * Fichier d'instanciation de l'app
 */
(function () {
  'use strict';

  console.info("FAYAT > Instanciation App FRED");

  var app = angular.module('Fred', modules);

  /* Par défaut, le séparateur des milliers est un espace et le séparateur des décimales est un point */
  /* A variabiliser lorsqu'on que l'appli sera traduite */
  app.run(["$locale", function ($locale) {
    $locale.NUMBER_FORMATS.GROUP_SEP = " ";
    $locale.NUMBER_FORMATS.DECIMAL_SEP = ".";
  }]);

  app.config(['$httpProvider', '$locationProvider', function ($httpProvider, $locationProvider) {
    $httpProvider.interceptors.push('unauthorizedInterceptor');
    $locationProvider.html5Mode({ enabled: true, requireBase: false, rewriteLinks: false });
  }]);

})();