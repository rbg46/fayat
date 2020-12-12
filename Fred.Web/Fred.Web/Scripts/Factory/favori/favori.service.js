(function () {
    'use strict';

    angular
        .module('Fred')
        .factory('favoriModal', favoriModalService);

    favoriModalService.$inject = ['$uibModal'];

    function favoriModalService($uibModal) {

        var service = {
            open: open,
            openDeleteModal: openDeleteModal
        };

        return service;

        function open(resources, favori) {

            var modalInstance = $uibModal.open({
                animation: true,
                size: "md",
                component: 'favoriComponent',
                resolve: {
                    resources: function () { return resources; },
                    favori: function () { return favori; }
                }
            });

            return modalInstance.result;
        }


        function openDeleteModal(resources, favori, home) {

            var modalInstance = $uibModal.open({
                animation: true,
                size: "md",
                component: 'deleteFavoriComponent',
                backdropClass: 'indexBackdrop',
                windowClass: 'indexWindow',
                backdrop: 'static',
        resolve: {
                    resources: function () { return resources; },
                    favori: function () { return favori; }
                }
            });

            return modalInstance.result.then(function (response) {
                return response;
            });
        }
    }

})();