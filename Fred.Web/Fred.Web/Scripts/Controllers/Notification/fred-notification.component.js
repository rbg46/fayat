(function () {
  'use strict';

  angular
    .module('Fred')
    .component('fredNotification', {
      templateUrl: '/Scripts/Controllers/Notification/fred-notification.html',
      bindings: {
        resources: '<'
      },
      controller: 'fredNotificationController'
    });

  angular
    .module('Fred')
    .controller('fredNotificationController', fredNotificationController);

  fredNotificationController.$inject = ['$http'];

  function fredNotificationController($http) {
    var $ctrl = this;
    $ctrl.resources = resources;

    $ctrl.message = "";
    $ctrl.notificationId;
    $ctrl.typeNotification = 0;

    var notifications = null;
    var currentNotificationIndex = 0;

    $ctrl.onClose = function () {
      resetNotificationController();
    };

    $ctrl.canShowNotification = function () {
      return notifications !== null && currentNotificationIndex < notifications.length;
    };

    $ctrl.$onInit = function () {
      $http.get("/api/Notification")
        .success(initNotificationDisplay)
        .error(function () {
          // Nothing to do here
        })
        .finally(function () {
          // Nothing to do here
        });
    };

    function initNotificationDisplay(notificationsUser) {
      if (notificationsUser !== null && notificationsUser.length > 0) {
        notifications = notificationsUser;
        setNotificationDisplayed();
      }
    };

    $ctrl.markAsRead = function () {
      $http.put("/api/Notification/MarkAsRead/" + $ctrl.notificationId)
        .success(displayNextNotification)
        .error(function () {
          // Nothing to do here
        })
        .finally(function () {
          // Nothing to do here
        });
    };

    function displayNextNotification() {
      currentNotificationIndex++;
      if (currentNotificationIndex < notifications.length) {
        setNotificationDisplayed();
      } else {
        resetNotificationController();
      }
    };

    function setNotificationDisplayed() {
      $ctrl.notificationId = notifications[currentNotificationIndex].NotificationId;
      $ctrl.message = notifications[currentNotificationIndex].Message;
      $ctrl.typeNotification = notifications[currentNotificationIndex].TypeNotification;
    };

    function resetNotificationController() {
      $ctrl.message = "";
      $ctrl.notificationId = 0;
      $ctrl.typeNotification = 0;
      notifications = null;
      currentNotificationIndex = 0;
    };
  }
})();