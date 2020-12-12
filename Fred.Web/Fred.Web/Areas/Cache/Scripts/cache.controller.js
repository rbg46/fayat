(function (angular) {
	'use strict';

	angular.module('Fred').controller('CacheController', CacheController);

	CacheController.$inject = ['Notify', 'CacheService', 'ProgressBar', 'confirmDialog', '$q'];

	function CacheController(Notify, CacheService, ProgressBar, confirmDialog, $q) {

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
		$ctrl.handleDelete = handleDelete;

		return $ctrl;

        /*
         * -------------------------------------------------------------------------------------------------------------
         *                                            HANDLERS
         * -------------------------------------------------------------------------------------------------------------
         */

        /**
         * Delete Cache Item
         * @param {any} key cache key
         */
		function handleDelete(key) {
		
            confirmDialog.confirm(resources, $ctrl.resources.Global_Modal_ConfirmationSuppression)
				.then(function () {
					$q.when()
						.then(ProgressBar.start)
						.then(actionDelete(key))						
						.then(ProgressBar.complete);
				});

		}

        /*
         * -------------------------------------------------------------------------------------------------------------
         *                                            ACTIONS
         * -------------------------------------------------------------------------------------------------------------
         */

        /**
         * Chargement des données : Liste des Caches 
         * @returns {Promise} promesse 
         */
		function actionLoad() {
			return CacheService.List()
				.then(function (response) {
					if (response && response.data && response.length === 0) {
						Notify.error(resources.Global_Notification_AucuneDonnees);
					}
					$ctrl.cacheList = response.data;
				})
				.catch(function () {
					Notify.error(resources.Global_Notification_Error);
				});
		}

        /**
         * suppression d'un cache
         * @param {any} item cache item
         * @returns {Promise} promesse
         */
		function actionDelete(item) {
			if (item) {
				$ctrl.cacheList.splice($ctrl.cacheList.indexOf(item), 1);
			}
			return CacheService.Delete(item)
				.then(function () { Notify.message(resources.Global_Notification_Suppression_Success); })
                .catch(function (err) { Notify.error(resources.Global_Notification_Error); });

		}
    
	}
})(angular);
