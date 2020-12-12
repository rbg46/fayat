(function (angular) {
    'use strict';

    angular.module('Fred').controller('FeatureFlippingController', FeatureFlippingController);

    FeatureFlippingController.$inject = ['Notify', 'FeatureFlippingService', 'ProgressBar', 'confirmDialog', '$q'];

    function FeatureFlippingController(Notify, FeatureFlippingService, ProgressBar, confirmDialog, $q) {

        var $ctrl = this;

        $ctrl.resources = resources;

        init();

        function init() {
            $q.when()
                .then(ProgressBar.start)
                .then(actionLoad)
                .then(ProgressBar.complete);
        }

        // Fonctions publiques
        $ctrl.handleUpdate = handleUpdate;

        return $ctrl;

        /*
         * -------------------------------------------------------------------------------------------------------------
         *                                            HANDLERS
         * -------------------------------------------------------------------------------------------------------------
         */

        /**
         * Update a feature flipping
         * @param {any} item feature
         */
        function handleUpdate(item) {
            var featureRessource = "";
            featureRessource = item.IsActived ? resources.FeatureFlipping_Index_Desactivation_Feature : resources.FeatureFlipping_Index_Activation_Feature;

            confirmDialog.confirm(resources, featureRessource)
                .then(function () {
                    $q.when()
                        .then(ProgressBar.start)
                        .then(function () { return item; })
                        .then(actionUpdate)
                        .then(ProgressBar.complete);
                });
        };

        /*
         * -------------------------------------------------------------------------------------------------------------
         *                                            ACTIONS
         * -------------------------------------------------------------------------------------------------------------
         */

        /**
         * Chargement des données : Liste des Features 
         * @returns {Promise} promesse 
         */
        function actionLoad() {
            return FeatureFlippingService.List()
                .then(function (response) {                    
                    if (response && response.data && response.length === 0) {
                        Notify.error(resources.Global_Notification_AucuneDonnees);
                    }
                    $ctrl.featureList = response.data;
                })
                .catch(function () {
                    Notify.error(resources.Global_Notification_Error);
                });
        }

        /**
         * Mise à jour d'une feature
         * @param {any} item feature
         * @returns {Promise} promesse
         */
        function actionUpdate(item) {
            return FeatureFlippingService.Update(item)
                .then(function (data) {                    
                    item.IsActived = data.data.IsActived;
                    item.DateActivation = data.data.DateActivation;
                    item.UserActivation = data.data.UserActivation;
                    Notify.message(resources.Global_Notification_Enregistrement_Success);
                })
                .catch(function (err) { actionDisplayError(err.data); });
        }

        /**
         * Affichage des erreurs lors d'une mise à jour
         * @param {any} data erreurs back
         */
        function actionDisplayError(data) {
            for (var err in data.ModelState) {
                Notify.error(data.ModelState[err][0]);
            }
        }
    }
})(angular);
