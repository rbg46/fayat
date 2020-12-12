(function (angular) {
  'use strict';

  angular.module('Fred').controller('resetPasswordController', resetPasswordController);

  resetPasswordController.$inject = ['UserService', '$window']

  function resetPasswordController(UserService, $window) {

    var $ctrl = this;
    $ctrl.loginImage = null;
    $ctrl.ResetIdentifiant = ResetIdentifiant;
    $ctrl.ResetEmail = ResetEmail;
    init();

    function init() {
      var loginImagePath = UserService.getSavedImages().login;
      $ctrl.loginImage = loginImagePath;
      ShowResetPasswordForm();
      InitVariable();
    }

    function ShowResetPasswordForm() {
      $(document).ready(function () {
        $('#reset-password').animate({ top: '20%', opacity: '1.0' },
          1500, function () {
            $('#block-reset-password').fadeIn();
            $('#reset-password').toggleClass("withBorder");
          });
      });
    }

    function InitVariable(){
      document.getElementById('Url').value = $window.location.origin;
      $ctrl.username = document.getElementById('UserName').value;
      $ctrl.email = document.getElementById('Email').value;
    }

    function ResetIdentifiant() {
      $ctrl.username = null;
    }

    function ResetEmail() {
      $ctrl.email = null;
    }
  }

}(angular));
