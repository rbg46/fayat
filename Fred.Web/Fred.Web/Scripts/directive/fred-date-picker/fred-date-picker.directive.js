(function () {
  'use strict';

  angular.module('Fred').directive('fredDatePicker', fredDatePicker);

  fredDatePicker.$inject = ['$timeout'];

  function fredDatePicker($timeout) {

    return {
      restrict: 'E',
      require: 'ngModel',
      scope: {
        format: '@',
        locale: '@',
        placeholder: '@',
        defaultDate: '@',
        min: '@',
        max: '@',
        stepping: '@',
        disabled: '=?ngDisabled'
      },
      templateUrl: '/Scripts/directive/fred-date-picker/fred-date-picker.html',
      controller: function () { },
      link: function (scope, element, attrs, ngModelCtrl) {
        var el = angular.element(element).children().first();

        // options : http://eonasdan.github.io/bootstrap-datetimepicker/Options
        var options = {
          locale: scope.locale,
          format: scope.format,
          allowInputToggle: true,
          useCurrent: false,
          minDate: dateParser(scope.min),
          maxDate: dateParser(scope.max),
          stepping: scope.stepping,
          tooltips: {
                today: 'Go to today',
                clear: 'Clear selection',
                close: 'Close the picker',
                selectMonth: 'Sélection du Mois',
                prevMonth: 'Mois Précédent',
                nextMonth: 'Mois Suivant',
                selectYear: 'Sélection de l\'Année',
                prevYear: 'Année Précédente',
                nextYear: 'Année Suivante',
                selectDecade: 'Select Decade',
                prevDecade: 'Previous Decade',
                nextDecade: 'Next Decade',
                prevCentury: 'Previous Century',
                nextCentury: 'Next Century'
            },

          parseInputDate: dateParser
        };

        // Instanciation des datetimepicker Bootstrap
        el.on('dp.change', onChange)
          .on('dp.show', onShow)
          .datetimepicker(options);

        // Gestion des évènements
        attrs.$observe('defaultDate', onDefaultDateChange);
        attrs.$observe('disabled', onDisable);
        scope.$watch(getModelValue, setModelValue);

        /**
         * Action lors du changement de date
         * @param {any} e valeur
         */
        function onChange(e) {
          if (ngModelCtrl) {
            $timeout(function () {
              ngModelCtrl.$setViewValue(dateParser(e.target.firstElementChild.value));
              ngModelCtrl.$commitViewValue();
            });
          }
        }

        /**
         *  Action a l'ouverture de la datetimepicker
         * @param {any} e valeur
         */
        function onShow(e) {
          if (scope.min && scope.min !== "") {
            el.data('DateTimePicker').minDate(dateParser(scope.min));
          }

          if (scope.max && scope.max !== "") {
            el.data('DateTimePicker').maxDate(dateParser(scope.max));
          }
        }

        /**
         * Action au changement de la date par défaut
         * @param {any} value valeur
         */
        function onDefaultDateChange(value) {
          if (value !== "") {
            el.data('DateTimePicker').date(dateParser(value));
          }
        }

        /**
         * Action Désactiver le composant Datetimepicker
         * @param {any} value valeur
         */
        function onDisable(value) {
          if (scope.disabled === true) {
            el.data('DateTimePicker').disable();
          }
          else {
            el.data('DateTimePicker').enable();
          }
        }

        /**
         * Analyse une date et conversion en date valide
         * @param {any} value date
         * @returns {any} parsed date
         */
        function dateParser(value) {
          if (value) {

            if (!(value instanceof Date) && isNaN(value.valueOf())) {
              value = value.replace(/\"/g, '');
            }

              var parsedDate = moment(value, scope.format, scope.locale, true).utc(true);
            if (!moment(parsedDate, scope.format, scope.locale, true).isValid()) {
                parsedDate = moment(value).utc(true).format(scope.format);
            }
            return parsedDate;
          }
          return;
        }

        /**
         * Récupération de la valeur du model
         * @returns {any} model value
         */
        function getModelValue() {
          return ngModelCtrl.$modelValue;
        }

        /**
         * Définit la valeur du model
         * @param {any} newValue nouvelle valeur du model
         */
        function setModelValue(newValue) {          
          if (!newValue) {
            el.data('DateTimePicker').date(null);
          }
          else if (newValue instanceof Date && !isNaN(newValue.valueOf())) {
            el.data('DateTimePicker').date(newValue);
          }
          else {
            el.data('DateTimePicker').date(dateParser(newValue));
          }
        }
      }
    };
  }
}());