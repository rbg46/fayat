
(function () {
    'use strict';

    angular.module('Fred').service('PointageListChangeDetectorService', PointageListChangeDetectorService);

    PointageListChangeDetectorService.$inject = ['$log'];

    function PointageListChangeDetectorService($log) {


        var service = {
            listHasChanged: listHasChanged
        };

        return service;

        //////////////////////////////////////////////////////////////////
        // PUBLICS METHODES                                             //
        //////////////////////////////////////////////////////////////////


        function listHasChanged(listToCompare) {
            var hasCreatedElement = listToCompare.filter(function (elem) {
                return elem.IsCreated !== undefined && elem.PointageId !== undefined && elem.IsCreated === true;
            });
            var hasIsUpdatedElement = listToCompare.filter(function (elem) {
                return elem.IsUpdated !== undefined && elem.IsUpdated === true;
            });
            var hasIsDeletedElement = listToCompare.filter(function (elem) {
                return elem.IsDeleted !== undefined && elem.IsDeleted === true;
            });

            var result = hasCreatedElement.length > 0 || hasIsUpdatedElement.length > 0 || hasIsDeletedElement.length > 0;

            return result;
        }
    }
})();
