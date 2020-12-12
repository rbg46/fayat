(function () {
    'use strict';

    angular.module('Fred').directive('appDatePicker', appDatePicker);

    appDatePicker.$inject = ['$timeout'];

    function appDatePicker($timeout) {

        return {
            restrict: 'E',
            require: 'ngModel',
            scope: {
                format: '@',
                lang: '@',
                placeholder: '@',
                initialDate: '@',
                minDate: '@',
                maxDate: '@',
                disabled: '=?ngDisabled',
                onUpdate: '&',
                onChange: '&',
                calendarWeeks: '=?',
                inputHiden: '=?',
                isWeekCalendar: '<'
            },
            templateUrl: '/Scripts/module/DateTimePicker/datetimepicker.html',

            controller: function () { },
            link: function (scope, element, attrs, ngModel) {
                var el = angular.element(element).children().first();
                var options = {
                    locale: scope.lang,
                    format: scope.format,
                    allowInputToggle: true,
                    calendarWeeks: scope.calendarWeeks,
                    widgetPositioning: {
                        vertical: 'bottom'
                    },
                    parseInputDate: dateParser,
                    keyBinds: { 'up': null, 'down': null, 'right': null, 'left': null }
                };

                // DEBUT - Workaround permettant de rester sur le jour d'aujourd'hui en UTC
                var defaultTime = { hour: moment().format('H'), minute: moment().format('m'), second: 0, millisecond: 0 };
                if (scope.format === "MM/YYYY") {
                    defaultTime.date = 15;
                }
                // FIN - Workaround permettant de rester sur le jour d'aujourd'hui en UTC        
                // Instanciation des datetimepicker 
                el.on('dp.change', onChange)
                    .on('dp.show', onShow)
                    .on('dp.update', scope.onUpdate)
                    .datetimepicker(options);

                attrs.$observe('disabled', onDisable);
                attrs.$observe('initialDate', onInitialDateChange);
                scope.$watch(getModelValue, setModelValue);

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

                        var parsedDate = moment(value, scope.format, scope.lang, true).set(defaultTime);

                        if (!moment(parsedDate, scope.format, scope.lang, true).isValid()) {
                            parsedDate = moment(value).set(defaultTime).format(scope.format);
                        }
                        return parsedDate;
                    }
                    return;
                }

                /**
                 * Action au changement de la date par défaut
                 * @param {any} value valeur
                 */
                function onInitialDateChange(value) {
                    if (value !== "") {
                        el.data('DateTimePicker').date(dateParser(value));
                    }
                }

                /**
                * Récupération de la valeur du model
                * @returns {any} model value
                */
                function getModelValue() {
                    return ngModel.$modelValue;
                }

                /**
                 * Définit la valeur du model
                 * @param {any} newValue nouvelle valeur du model
                 * @param {any} oldValue ancienne valeur du model         
                 */
                function setModelValue(newValue, oldValue) {
                    if (!newValue) {
                        if (oldValue) {
                            scope.onUpdate();
                        }
                        el.data('DateTimePicker').date(null);
                    }
                    else {
                        el.data('DateTimePicker').date(dateParser(newValue));
                    }
                }

                /**
                 *  Action a l'ouverture de la datetimepicker
                 * @param {any} e valeur
                 */
                function onShow(e) {
                    var value;
                    var parsedDate;
                    var date;
                    el.data('DateTimePicker').minDate(new Date(1900, 0));
                    el.data('DateTimePicker').maxDate(new Date(2100, 0));

                    if (scope.minDate) {
                        value = scope.minDate;
                        if (value !== "") {
                            parsedDate = dateParser(value);
                            date = moment(parsedDate, scope.format, scope.lang, true);
                            el.data('DateTimePicker').minDate(date._d);
                        }
                    }

                    if (scope.maxDate) {
                        value = scope.maxDate;
                        if (value !== "") {
                            parsedDate = dateParser(value);
                            date = moment(parsedDate, scope.format, scope.lang, true);
                            el.data('DateTimePicker').maxDate(date._d);
                        }
                    }
                }

                /**
                 * Action lors du changement de date
                 * @param {any} e valeur
                 */
                function onChange(e) {
                    $timeout(function () {
                        if (e.date !== null) {
                            ngModel.$viewValue = e.date._d;
                            ngModel.$commitViewValue();

                        }

                        if (moment(ngModel.$viewValue) !== moment(e.date._d)) {
                            scope.onChange({ date: e.date._d });
                        }

                    });
                }

                /**
                 * Action Désactiver le composant Datetimepicker
                 * @param {any} value valeur
                 */
                function onDisable(value) {
                    if (angular.isDefined(scope.disabled)) {
                        if (scope.disabled === true) {
                            el.data('DateTimePicker').disable();
                        }
                        else {
                            el.data('DateTimePicker').enable();
                        }
                    }
                }

            }
        };
    }
}());
