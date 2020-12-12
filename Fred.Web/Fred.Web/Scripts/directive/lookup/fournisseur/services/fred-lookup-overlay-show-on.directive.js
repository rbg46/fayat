(function () {
    'use strict';

    //angular.module('Fred').service('lookupOverlayService', lookupOverlayService);

    ///**
    // * @description Service du clavier dans la barre de recherche
    // * @returns {any} Constructeur du service
    // */
    //function lookupOverlayService() {
    //    var bodyRef = angular.element(window.document.body);

    //    var service = {
    //        showOverlay: showOverlay,
    //        hideOverlay: hideOverlay
    //    };

    //    return service;


    //    function showOverlay() {
    //        // Up (38)
    //        if (e.keyCode === 38 && ctrl.selectedIndex - 1 >= 0) {
    //            ctrl.selectedIndex--;
    //        }
    //        // Down (40)
    //        if (e.keyCode === 40 && ctrl.selectedIndex + 1 < ctrl.referentiels.length) {
    //            ctrl.selectedIndex++;
    //        }
    //        // Enter (13)
    //        if (e.keyCode === 13 && ctrl.referentiels.length > 0) {
    //            //on attend que la dernière recherche effctuée soit achevée
    //            while (ctrl.busy) {
    //                setTimeout(function () {
    //                    console.log('waiting for watcher end');
    //                }, 200);
    //            }
    //            //Appel de la fonction de sélection pour l'élément sélectionné dans la liste
    //            onEnterCallback(ctrl.referentiels[ctrl.selectedIndex]);
    //            e.preventDefault();
    //        }

    //        if (e.keyCode === 27) {
    //            ctrl.isOpen = false;
    //        }
    //    }


    //}




    angular.module('Fred').directive('fredLookUpOverlayShowOn', function ($timeout) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {

                var bodyRef = angular.element(window.document.body);

                scope.$watch(attrs.fredLookUpOverlayShowOn, function (value) {
                    var showOverlay = value || null;
                    if (showOverlay) {

                        bodyRef.addClass('ovh');

                        //$timeout(function () {
                        //    if (element.length > 0) {
                        //        element[0].focus();
                        //        element[0].select();
                        //    }
                        //}, 10);
                    }
                }, true);
            }

        };
    });

    angular.module('Fred').directive('fredLookUpOverlayHideOn', function ($timeout) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {

                var bodyRef = angular.element(window.document.body);

                scope.$watch(attrs.fredLookUpOverlayShowOn, function (value) {
                    var hideOverlay = value || null;
                    if (hideOverlay) {
                        bodyRef.removeClass('ovh');
                    }
                }, true);
            }

        };
    });



})();
