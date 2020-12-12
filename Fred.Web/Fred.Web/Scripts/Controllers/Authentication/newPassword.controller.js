(function (angular) {
  'use strict';

  angular.module('Fred').controller('newPasswordController', newPasswordController);

  newPasswordController.$inject = ['UserService', '$window']

  function newPasswordController(UserService, $window) {

    var $ctrl = this;
    $ctrl.loginImage = null;
    init();

    function init() {
      var loginImagePath = UserService.getSavedImages().login;
      $ctrl.loginImage = loginImagePath;
      ShowNewPasswordForm();
      ShowNewPasswordError();
    }

    function ShowNewPasswordForm() {
      $(document).ready(function () {
        $('#new-password').animate({ top: '20%', opacity: '1.0' },
          1500, function () {
            $('#block-new-password').fadeIn();
            $('#new-password').toggleClass("withBorder");
          });
      });
    }

    function ShowNewPasswordError() {
      $(document).ready(function () {
        $('#new-password-error').animate({ top: '20%', opacity: '1.0' }, 1500)});
    }
  }

}(angular));
