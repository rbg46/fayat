(function() {
    'use strict';

    angular
        .module('Fred')
        .service('ServerNotificationsService', ServerNotificationsService);

    function ServerNotificationsService() {
        function subscribeToUserNotificationCountUpdates(callback) {
            subscribeToApiNotificationCountUpdates(callback);
        }

        function subscribeToApiNotificationCountUpdates(callback) {
            var hub = $.connection.fredHub;
            hub.client.updateNotificationCount = callback;

            $.connection.hub.start();
        }

        return {
            SubscribeToUserNotificationCountUpdates: subscribeToUserNotificationCountUpdates
        };
    }
})();

