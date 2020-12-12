(function(angular) {
    'use strict';

    angular.module('Fred').controller('connectController', connectController);

    connectController.$inject = ['UserService'];

    function connectController(UserService) {
        var $ctrl = this;

        $ctrl.loginImage = null;
        init();

        function init() {
            var loginImagePath = UserService.getSavedImages().login;
            $ctrl.loginImage = loginImagePath;
            ShowConnectForm();

            clearCache();
        }

        function ShowConnectForm() {
            $(document).ready(function() {
                $('#login').animate(
                    { top: '20%', opacity: '1.0' },
                    1500,
                    function() {
                        $("#auth").fadeIn();
                        $("#login").toggleClass("withBorder");
                    }
                );
            });
        }

        function clearCache() {
            sessionStorage.clear();
            localStorage.clear();
        }
    }
}(angular));
