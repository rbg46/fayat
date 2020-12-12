
(function () {
    'use strict';

    angular.module('Fred').service('DuplicatorTimeService', DuplicatorTimeService);

    DuplicatorTimeService.$inject = [];

    function DuplicatorTimeService() {


        var service = {
            getNextWorkingDay: getNextWorkingDay
        };

        return service;

        //////////////////////////////////////////////////////////////////
        // PUBLICS METHODES                                             //
        //////////////////////////////////////////////////////////////////

        function getNextWorkingDay(currentDay) {
            var nextDay = new Date(currentDay);
            nextDay.setDate(nextDay.getDate() + 1); // tomorrow
            if (nextDay.getDay() === 0) {
                //si dimanche => alors on dit que c'est lundi
                nextDay.setDate(currentDay.getDate() + 1);
            }
            else if (nextDay.getDay() === 6) {
                //si dimanche => alors on dit que c'est lundi
                nextDay.setDate(nextDay.getDate() + 2);
            }

            return nextDay;
        }
    }
})();
