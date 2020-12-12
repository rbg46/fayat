/* -------------------------------------------------
 * FAYAT CONSTRUCTION INFORMATIQUE 
 * -------------------------------------------------
 * FRED / BPE / 20161104
 * Configuration des modules de l'application.
 * 
 * Si des modules sont déjà configurés, ajoute les dépendances à la liste.
 */

if (angular.isDefined(modules) && !angular.isArray(modules)) {
  throw ("'modules' global variable is defined but not an array.")
}
if (!angular.isDefined(modules)) {
  var modules = [];
}

modules = modules.concat([
  'ngStorage',
  'ui-notification',
  'ngProgress.provider',
  'ui.bootstrap',
  'angularModalService',
  'ngResource',
  'ngMap',
  'ngSanitize',
  'ui.mask'
]);