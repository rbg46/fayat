(function () {
    'use strict';

    var inputFormuleComponent = {
        templateUrl: '/Areas/Budget/Scripts/BudgetSousDetail/components/input.formule.component.html',
        bindings: {
            disabled: '<',
            item: '<',
            valeur: '<',
            formule: '<',
            onChangeItem: '&',
            cssClass: '@',
            tooltipFormule: '@',
            decimalPrecision: '@',
            exactValue: '<'
        },
        controller: inputFormuleController
    };

    function inputFormuleController($parse, $scope) {

        // initialise la valeur affichée
        this.$onInit = function () {
            this.displayedText = formatNumber(this.valeur, this.decimalPrecision);
            this.isEdited = false;
        };

        // entre en edition
        this.focusIn = function () {
            if (this.formule && this.formule !== '')
                this.displayedText = this.formule;
            this.isEdited = true;
        };

        // sort de l'edition
        this.focusOut = function () {
            this.displayedText = formatNumber(this.valeur, this.decimalPrecision);
            this.isEdited = false;
        };

        // parse la formule et met à jour la valeur
        this.parseFormule = function () {
            this.displayedText = restrictInput(this.displayedText, this.decimalPrecision);
            // regex replace /g pour faire un replace sur toutes les occurences du caractère
            var valeurInitiale = this.displayedText.replace(/\,/g, '.');

            if (this.displayedText && this.displayedText.startsWith('=')) {
                let formule = valeurInitiale.substring(1);
                try {
                    let valeurCalculee = $parse(formule)();
                    this.valeur = roundNumber(valeurCalculee, this.decimalPrecision);
                    this.formule = this.displayedText;
                }
                catch (e) {
                    // pas de conversion possible la valeur reste à l'état précédent
                    return;
                }
            }
            else {
                this.formule = null;
                var valeurNumerique = parseFloat(valeurInitiale);

                if (isNaN(valeurNumerique))
                    this.valeur = null;
                else
                    this.valeur = roundNumber(valeurNumerique, this.decimalPrecision);
            }

            this.onChangeItem({
                item: this.item,
                valeur: this.valeur,
                formule: this.formule
            });
        };

        // gère les changements exterieurs au composant
        this.$onChanges = function (changesObj) {
            if (!this.isEdited) {
                this.displayedText = formatNumber(this.valeur, this.decimalPrecision);
            }
        };
    }

    // formattage du nombre pour affichage
    function formatNumber(valeur, decimalPrecision) {
        var formattedNumber = roundNumber(valeur, decimalPrecision).toFixed(decimalPrecision);

        // séparateur de milliers
        var parts = formattedNumber.split('.');
        parts[0] = parts[0].replace(/\d{1,3}(?=(\d{3})+(?!\d))/g, "$&" + ' ');
        formattedNumber = parts.join('.');

        // replace le décimal separator
        return formattedNumber.replace('.', ',').toString();
    }

    // arrondi au nombre de décimales donné
    function roundNumber(valeur, decimalPrecision) {
        var roundRatio = Math.pow(10, decimalPrecision);
        return parseFloat(Math.round(valeur * roundRatio) / roundRatio);
    }

    // restriction de l'input avec desregex
    function restrictInput(valeur, decimalPrecision) {
        // replace decimal separator, remove spaces, replace Xx by *
        valeur = valeur.trim().replace(/\./g, ',')
            .replace(/\s/g, '')
            .replace(/x/gi, '*');

        // regex input standard numerique
        var regex = new RegExp("^\\d*\\,?\\d{0," + decimalPrecision + "}");
        // regex input formule formule ex:"=1+2-3*(4/5)" 
        if (valeur.startsWith('='))
            regex = /^\=[\d\/\+\-\*\,()]*/g;

        var result = valeur.match(regex);
        if (!result)
            return '';

        // return all matching strings
        return result.join('');
    }

    angular.module('Fred').component('inputFormuleComponent', inputFormuleComponent);

})();
