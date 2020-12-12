(function () {
    'use strict';

    angular.module('Fred').directive('fredTableHeader', fredTableHeader);

    /**
     * @description Entête d'un tableau : Element à l'intérieur d'un <th></th>   
     * @returns {any} directive
     */
    function fredTableHeader() {
        return {
            restrict: 'E',
            transclude: true,
            require: 'ngModel',
            scope: {
                headerStyle: '@',
                model: '=ngModel',
                headerText: '@'
            },
            templateUrl: '/Scripts/directive/table/fred-table-header.html',
            controller: function () { },
            link: link
        };

        function link(scope, element, attrs, modelCtrl) {
            var knownStyles = ['white', 'blue'];                        
            var headerText = scope.headerText;
            if (!headerText) {
                headerText = element.find('ng-transclude').children().first().text();
            }
            var tooltipHelper = "\n" + resources.Global_Tri_Helper_Tooltip;

            // Init
            scope.handleOnHeaderClick = handleOnHeaderClick;
            scope.tooltipAsc = resources.Global_Tri_Asc_Tooltip + headerText + tooltipHelper;
            scope.tooltipDesc = resources.Global_Tri_Desc_Tooltip + headerText + tooltipHelper;
            scope.tooltipCancel = resources.Global_Tri_Annuler_Tooltip;            
            scope.style = !scope.headerStyle || knownStyles.indexOf(scope.headerStyle) < 0 ? '' : scope.headerStyle;           

            /* -------------------------------------------------------------------------------------------------------------
             *                                         Functions   
             * -------------------------------------------------------------------------------------------------------------
             */

            /**
             * Evenement au click sur l'entête
             * @param {any} value valeur forcé du filtre
             */
            function handleOnHeaderClick(value) {
                scope.model = actionGetNextState(value, scope.model);
                modelCtrl.$setViewValue(scope.model);                
            }

            /**
             * Gestion des 3 états de tri : Ascendant, Descendant, null
             * Récupération du prochain état du tri
             * @param {any} value valeur forcée du tri
             * @param {any} model model
             * @returns {any} valeur du model
             */
            function actionGetNextState(value, model) {
                if (value !== undefined) {
                    return value;
                }

                if (model === true) {
                    return false;
                }
                else if (model === false) {
                    return null;
                }
                return true;
            }
        }
    }
})();