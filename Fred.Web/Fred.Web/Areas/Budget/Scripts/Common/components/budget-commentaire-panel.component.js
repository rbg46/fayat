// composant utilisé dans 2 écrans : budget-detail et budget-avancement
(function () {
    'use strict';

    var budgetCommentairePanelComponent = {
        templateUrl: '/Areas/Budget/Scripts/Common/components/budget-commentaire-panel.component.html',
        bindings: {
            readonly: '<',
            resources: '<'
        },
        controller: budgetCommentairePanelController
    };
    angular.module('Fred').component('budgetCommentairePanelComponent', budgetCommentairePanelComponent);
    angular.module('Fred').controller('budgetCommentairePanelController', budgetCommentairePanelController);
    budgetCommentairePanelController.$inject = ['$scope', 'BudgetService', 'StringFormat', '$timeout'];

    function budgetCommentairePanelController($scope, BudgetService, StringFormat, $timeout) {
        var $ctrl = this;
        $ctrl.Readonly = $ctrl.readonly;


        //////////////////////////////////////////////////////////////////
        // Déclaration des membres publiques                            //
        //////////////////////////////////////////////////////////////////
        $ctrl.CommentaireErreur = null;
        $ctrl.View = {};

        $ctrl.Validate = Validate;
        $ctrl.Hide = Hide;
        $ctrl.CommentaireChanged = CommentaireChanged;


        //////////////////////////////////////////////////////////////////
        // Initialisation                                               //
        //////////////////////////////////////////////////////////////////
        Hide();

        $ctrl.$onInit = function () {
            $scope.$on(BudgetService.Events.OpenPanelCommentaireAvancement, function (event, arg) {
                $ctrl.Tache1 = arg.Tache1;
                $ctrl.Tache2 = arg.Tache2;
                $ctrl.Tache3 = arg.Tache3;
                if (arg.Tache4) {
                    $ctrl.Tache = arg.Tache4;
                    $ctrl.Niveau = 4;
                }
                else if (arg.Tache3) {
                    $ctrl.Tache = arg.Tache3;
                    $ctrl.Niveau = 3;
                }
                else if (arg.Tache2) {
                    $ctrl.Tache = arg.Tache2;
                    $ctrl.Niveau = 2;
                }
                else {
                    $ctrl.Tache = arg.Tache1;
                    $ctrl.Niveau = 1;
                }
                $ctrl.Commentaire = $ctrl.Tache.CommentaireAvancement;
                $ctrl.View.Commentaire = $ctrl.Tache.CommentaireAvancement;
                if ($ctrl.Niveau === 4)
                    $ctrl.CommentaireMaxLength = 500;
                else
                    $ctrl.CommentaireMaxLength = 150;
                Show();
                $timeout(() => $("#TEXTAREA").focus());
            });

            $scope.$on(BudgetService.Events.OpenPanelCommentaire, function (event, arg) {

                console.log(arg.Tache);
                $ctrl.CommentaireErreur = null;

                if (arg.Tache) {
                    $ctrl.Tache = arg.Tache;
                    $ctrl.Niveau = arg.Tache.Niveau;
                    $ctrl.Tache1 = null;
                    $ctrl.Tache2 = null;
                    $ctrl.Tache3 = null;

                    if  ($ctrl.Niveau === 4) {
                        $ctrl.CommentaireMaxLength = 500;
                        $ctrl.Tache3 = $ctrl.Tache.Tache3;
                        $ctrl.Tache2 = $ctrl.Tache3.Tache2;
                        $ctrl.Tache1 = $ctrl.Tache2.Tache1;
                        $ctrl.Commentaire = $ctrl.Tache.BudgetT4.View.Commentaire;
                    }
                    else {
                        $ctrl.CommentaireMaxLength = 150;
                        if ($ctrl.Niveau === 3) {
                            $ctrl.Tache2 = $ctrl.Tache.Tache2;
                            $ctrl.Tache1 = $ctrl.Tache2.Tache1;
                        }
                        else if ($ctrl.Niveau === 2) {
                            $ctrl.Tache1 = $ctrl.Tache.Tache1;
                        }
                        $ctrl.Commentaire = $ctrl.Tache.Info.View.Commentaire;
                    }
                }
                else if (arg.SousDetailItem) {
                    $ctrl.SousDetailItem = arg.SousDetailItem;
                    $ctrl.CommentaireMaxLength = 200;
                    $ctrl.Commentaire = $ctrl.SousDetailItem.View.Commentaire;
                }
                else {
                    return;
                }

                $ctrl.View.Commentaire = $ctrl.Commentaire;
                Show();
                $timeout(() => $("#TEXTAREA").focus());
            });
        };


        //////////////////////////////////////////////////////////////////
        // Actions                                                      //
        //////////////////////////////////////////////////////////////////

        // Appelé lorsque le commentaire change.
        // Il semble qu'il n'est pas possible de mettre un maxlength sur un TextArea qui limite vraiment le nombre de caractères saisis.
        // Cette fonction permet d'afficher un message si le commentaire est trop long
        function CommentaireChanged() {
            if ($ctrl.View.Commentaire.length > $ctrl.CommentaireMaxLength) {
                $ctrl.CommentaireErreur = StringFormat.Format($ctrl.resources.Budget_Detail_CommentairePanel_Erreur_TropLong, $ctrl.View.Commentaire.length, $ctrl.CommentaireMaxLength);
            }
            else {
                $ctrl.CommentaireErreur = null;
            }
        }

        // Valide la modification du commentaire
        function Validate() {
            if ($ctrl.View.Commentaire !== $ctrl.Commentaire) {
                $scope.$emit(BudgetService.Events.PanelCommentaireModified, {
                    Tache: $ctrl.Tache,
                    SousDetailItem: $ctrl.SousDetailItem,
                    Commentaire: $ctrl.View.Commentaire
                });
            }
            Hide();
        }


        //////////////////////////////////////////////////////////////////
        // Autre                                                        //
        //////////////////////////////////////////////////////////////////

        // Ouvre le panneau
        function Show() {
            $ctrl.PanelClass = "open";
        }

        // Ferme le panneau
        function Hide() {
            $ctrl.PanelClass = "close-right-panel";
        }
    }
})();
