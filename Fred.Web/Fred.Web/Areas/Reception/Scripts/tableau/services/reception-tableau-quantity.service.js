/*
 * Ce service sert a gérer les erreurs de quantite d'une reception
 */
(function () {
    'use strict';

    angular.module('Fred').service('ReceptionTableauQuantityService', ReceptionTableauQuantityService);

    ReceptionTableauQuantityService.$inject = ['ModelStateErrorManager'];

    function ReceptionTableauQuantityService(ModelStateErrorManager) {

        this.hasReceptionQuantityNegativeError = function (error, receptions) {
            var result = false;
            angular.forEach(receptions, function (reception) {
                var receptionQuantiteNegativeErrorName = 'Quantite_' + reception.DepenseId;
                var receptionQuantiteNegativeError = ModelStateErrorManager.getError(error, receptionQuantiteNegativeErrorName);
                if (receptionQuantiteNegativeError.hasThisError) {
                    result = true;
                }
            });
            return result;
        };

        this.markReceptionQuantityValidationErrors = function (error, receptions, resources) {
            angular.forEach(receptions, function (reception) {
                var receptionQuantiteNegativeErrorName = 'Quantite_' + reception.DepenseId;
                var receptionQuantiteNegativeError = ModelStateErrorManager.getError(error, receptionQuantiteNegativeErrorName);
                if (receptionQuantiteNegativeError.hasThisError) {
                    reception.quantite_negative_error = receptionQuantiteNegativeError.errorMessagesList[1];                  
                }
            });
        };

        this.cleanReceptionQuantityValidationErrors = function (receptions) {
            angular.forEach(receptions, function (reception) {
                reception.quantite_negative_error = '';
            });
        };


    }
})();
