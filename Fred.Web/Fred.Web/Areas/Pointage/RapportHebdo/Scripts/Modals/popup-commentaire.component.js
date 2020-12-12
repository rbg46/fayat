(function () {
    'use strict';

    var popupCommentaireComponent = {
        templateUrl: '/Areas/Pointage/RapportHebdo/Scripts/Modals/popup-commentaire.component.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: popupCommentaireComponentController
    };
    angular.module('Fred').component('popupCommentaireComponent', popupCommentaireComponent);
    angular.module('Fred').controller('popupCommentaireComponentController', popupCommentaireComponentController);
    popupCommentaireComponentController.$inject = ['$scope'];

    function popupCommentaireComponentController($scope) {

        //////////////////////////////////////////////////////////////////
        // Membres publiques                                            //
        //////////////////////////////////////////////////////////////////
        var $ctrl = this;
        $ctrl.CommentaireDisplay = "";
        $ctrl.handleClickValidate = handleClickValidate;
        $ctrl.handleCancel = handleCancel;
        $ctrl.handleDelete = handleDelete;

        //////////////////////////////////////////////////////////////////
        // Fonctions publiques                                          //
        //////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////
        // Initialisation                                               //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {
            $ctrl.itemWithCommentaireToDisplay = $ctrl.resolve.itemWithCommentaireToDisplay;
            $ctrl.CommentaireDisplay = $ctrl.itemWithCommentaireToDisplay.Commentaire;
            $ctrl.resources = $ctrl.resolve.resources;
            $('#txtCommentaire').focus();
        };

        function handleClickValidate() {
            $ctrl.itemWithCommentaireToDisplay.Commentaire = $ctrl.CommentaireDisplay;
            $ctrl.close({ $value: $ctrl.itemWithCommentaireToDisplay });
        }

        function handleCancel() {
            $ctrl.dismiss({ $value: 'cancel' });
        }

        function handleDelete() {
            $ctrl.CommentaireDisplay = null;
            handleClickValidate();
        }
    }
})();
