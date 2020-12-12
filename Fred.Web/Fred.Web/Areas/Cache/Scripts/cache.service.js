(function () {
	'use strict';

	angular.module('Fred').service('CacheService', CacheService);

	CacheService.$inject = ['$http'];

	function CacheService($http) {
		return {

			List: function () {
				return $http.get('/api/Cache/List');
			},

			Delete: function (model) {

				return $http.post('/api/Cache/DeleteCache', model);
			}
		};
	}
})();