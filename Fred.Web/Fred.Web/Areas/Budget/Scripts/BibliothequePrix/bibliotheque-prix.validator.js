
(function () {
    'use strict';

    angular
        .module('Fred')
        .service('BibliothequePrixValidator', BibliothequePrixValidator);


    function BibliothequePrixValidator() {

        var service = this;

        /**
         * Retourne true si le model est valide, false sinon
         * Un model valide contient une organisation, une devise
         * et pour chaque item, soit un prix et une unité ou aucun des deux et une ressource
         * @param {any} model le model a tester
         * @returns {any} Un booleen
         */
        service.ValidateSaveModel = function (model) {

            if (!model.OrganisationId)
                return false;

            if (!model.DeviseId) {
                return false;
            }

            for (let item of model.Items) {
                if (!service.ValidateSaveModelItem(item)) {
                    return false;
                }
            }

            return true;
        };

        /**
         * Vérifie la validité d'un item venant du model de sauvegarde
         * @param {any} item l'item a tester
         * @return {any} un booleen
         */
        service.ValidateSaveModelItem = function (item) {
            if (!item.RessourceId) {
                return false;
            }

            return service.ValidatePrixUniteCoherence(item);
        };

        service.ValidatePrixUniteCoherence = function (item) {
            return item.Prix === null && item.UniteId === null
                || item.Prix !== null && item.UniteId !== null;
        };

    }

})();