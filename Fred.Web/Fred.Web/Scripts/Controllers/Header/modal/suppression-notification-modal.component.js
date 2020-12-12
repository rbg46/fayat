(function (angular) {
    'use strict';

    angular.module('Fred').component('deleteNotificationComponent', {
        templateUrl: '/Scripts/Controllers/Header/modal/suppression-notification-modal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: 'DeleteNotificationComponentController'
    });

    DeleteNotificationComponentController.$inject = [];

    function DeleteNotificationComponentController() {
        var $ctrl = this;

        $ctrl.$onInit = $onInit;
        $ctrl.handleValidate = handleValidate;
        $ctrl.handleCancel = handleCancel;

        function $onInit() {
            $ctrl.notification = angular.copy($ctrl.resolve.notification);
            $ctrl.resources = $ctrl.resolve.resources;
        }

        function handleValidate() {
            $ctrl.close({ $value: $ctrl.notification });
        }

        function handleCancel() {
            $ctrl.dismiss({ $value: 'cancel' });
        }
    }


    angular.module('Fred').controller('DeleteNotificationComponentController', DeleteNotificationComponentController);

}(angular));