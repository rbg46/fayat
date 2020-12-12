(function () {
    'use strict';

    var bibliothequePrixItemHistoriqueComponent = {
        templateUrl: '/Areas/Budget/Scripts/BibliothequePrix/components/bibliotheque-prix-item-historique.component.html',
        bindings: {
            devise: '<'
        },
        controller: bibliothequePrixItemHistoriqueController
    };

    angular.module('Fred').component('bibliothequePrixItemHistoriqueComponent', bibliothequePrixItemHistoriqueComponent);

    angular.module('Fred').controller('bibliothequePrixItemHistoriqueController', bibliothequePrixItemHistoriqueController);

    bibliothequePrixItemHistoriqueController.$inject = ['$scope', 'BudgetService'];

    function bibliothequePrixItemHistoriqueController($scope, BudgetService) {

        var $ctrl = this;
        $ctrl.resources = resources;
        $ctrl.item = null;
        $ctrl.Visible = false;

        $ctrl.Hide = Hide;
        $ctrl.GetPeriodeOfItemHisto = GetPeriodeOfItemHisto;
        $ctrl.GetPeriodeToStringItemCourant = GetPeriodeToStringItemCourant;
        $ctrl.ItemHasAnyValue = ItemHasAnyValue;

        $scope.$on(BudgetService.Events.DisplayHistoriqueBibliothequePrix, function (event, args) { Show(args); });

        return $ctrl;

        function Show(args) {
            $ctrl.Item = args.Item;
            $ctrl.Histo = args.Histo;
            $ctrl.Visible = true;
        }

        function Hide() {
            $ctrl.Visible = false;
        }


        function GetPeriodeToStringItemCourant() {
            //Si l'item n'a jamais été enregistré, ni la date d'insertion, ni la date de motification n'existent
            let dateToFormat = $ctrl.Item.DateModification ? $ctrl.Item.DateModification : $ctrl.Item.DateCreation;
            if (dateToFormat) {
                let date = new Date(dateToFormat);
                return date.toLocaleString();
            }
        }

        function GetPeriodeOfItemHisto(histoItem, indexOfPreviousItemHisto) {
            let itemHistoDateModification = new Date(histoItem.DateInsertion);

            let newerItemHistoDateModification = null;

            //Si jamais il y a un dans l'historique un item plus récent alors l'item s'applique jusqu'a la date 
            //D'application de l'item plus récent trouvé
            if (indexOfPreviousItemHisto >= 0) {
                newerItemHistoDateModification = $ctrl.Histo[indexOfPreviousItemHisto].DateInsertion;

            } else {
                //Si on est ici c'est que l'item historisé est le dernier a avoir été historisé
                //Il a donc été en application jusqu'a la date de création de l'item courant
                newerItemHistoDateModification = $ctrl.Item.DateModification;
            }

            let newerItemHistoDateTimeModification = new Date(newerItemHistoDateModification);
            return itemHistoDateModification.toLocaleString() + " > " + newerItemHistoDateTimeModification.toLocaleString();
        }

        function ItemHasAnyValue() {
            if ($ctrl.Item && $ctrl.Histo) {
                return $ctrl.Histo.length > 0 || $ctrl.Item.Original.Prix !== null && $ctrl.Item.Original.Unite !== null;
            }
        }
    }
})();
