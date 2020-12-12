/// <reference path="template/dockModalDetail.html" />
(function () {
    'use strict';

    angular
      .module('Fred', ['dockModal']);

    angular
      .module('Fred')
      .controller('demoCtrl', demoCtrl)
      .controller('demoDockCtrl', demoDockCtrl);

    demoCtrl.$inject = [
      '$scope',
      '$dockModal'
    ];

    function demoCtrl($scope, $dockModal) {




        $scope.openDockModal = function (id, datas, template) {

            /*variable temp pour la définition des templates*/
            var _templateUrl;
            if (template == "list") {
                _templateUrl = "/vendor/other/dockModal/template/dockModalList.html";
            }

            if (template == "detail") {
                _templateUrl = "/vendor/other/dockModal/template/dockModalDetail.html";
            }


            if (!id) id = Math.floor(Math.random() * 10) + 1;
            $dockModal.show({
                id: id,
                width: 350,
                minimizedWidth: 300,
                // templateUrl: '_demoDock.html',
                templateUrl: _templateUrl,
               // templateUrl: '_demoDock.html',
                controller: 'demoDockCtrl',
                locals: { id: id, datas: datas }
            }).then(function (data) {
                console.log('Closed and resolved dock modal with id ' + id);
            }, function () {
                console.log('Closed and rejected dock modal with id ' + id);
            });
        }



    }

    demoDockCtrl.$inject = [
      '$scope',
      '$dockModal',
      'id',
      'datas'
    ]

    function demoDockCtrl($scope, $dockModal, id, datas) {
        $scope.isMinimized = false;
        $dockModal.when(id).then(function (instance) {
            $scope.dockModal = instance;
        });
        $scope.id = id;
        console.log(datas);

        $scope.restore = function (dockModal) {
            $scope.dockModal.restore();
            $scope.isMinimized = false;
        }

        $scope.minimize = function (dockModal) {
            $scope.dockModal.minimize();
            $scope.isMinimized = true;
        }

        $scope.toggle = function () {
            if ($scope.dockModal.isMinimized()) {
                $scope.restore();
            } else {
                $scope.minimize();
            }
        }

        $scope.close = function (data) {
            if (data)
                $scope.dockModal.remove(data, false);
            else
                $scope.dockModal.remove(undefined, true);
        }
    }
})();