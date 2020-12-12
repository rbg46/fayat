/*
 * Ce service sert recuperer les erreurs de validation.
 * Lors d'un appel http s'il y a une erreur, le service recupere toutes les erreurs du model.
 * Les erreurs du model sont remontées par un 'Validator' coté serveur.
 */
(function () {
    'use strict';

    angular
        .module('Fred')
        .factory('ModelStateErrorManager', ModelStateErrorManager);

    function ModelStateErrorManager() {
        var service = {
            getErrors: getErrors,
            hasError: hasError,
            getError: getError
        };
        return service;

        /*
         * Contatene les erreurs du model lors d'une erreur de validation coté serveur.
         */
        function getErrors(error) {
            var result = '';

            if (error.data && error.data.ModelState) {
                for (var key in error.data.ModelState) {
                    var value = error.data.ModelState[key];
                    if (angular.isArray(value)) {
                        for (var i = 0; i < value.length; i++) {
                            result += value[i] + '\n';
                        }
                    }
                }
            }
            return result;
        }
        /*
         * Determine s'il y a eu erreur specifique de validation coté serveur .
         */
        function hasError(error, errorKey) {
            var result = false;

            if (error.data && error.data.ModelState) {
                for (var key in error.data.ModelState) {
                    if (key === errorKey) {
                        return true;
                    }
                }
            }
            return result;
        }

        /*
       * Retourne l'erreur specifique de validation coté serveur .
       */
        function getError(error, errorKey) {
            var result = {
                errorMessagesList: [],
                concatenatedErrors: '',
                firstError: '',
                hasThisError: false
            };

            if (error.data && error.data.ModelState) {
                for (var key in error.data.ModelState) {
                    if (key === errorKey) {
                        var value = error.data.ModelState[key];
                        var concatenatedErrors = '';
                        for (var i = 0; i < value.length; i++) {
                            concatenatedErrors += value[i] + '\n';
                        }
                        return {
                            errorMessagesList: value,
                            concatenatedErrors: concatenatedErrors,
                            firstError: value[0],
                            hasThisError: value[0] !== ''
                        };
                    }

                }
            }
            return result;
        }
    }

})();
