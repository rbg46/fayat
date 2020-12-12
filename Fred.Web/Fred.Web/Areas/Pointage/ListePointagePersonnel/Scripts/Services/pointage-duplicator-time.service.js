
(function () {
    'use strict';

    angular.module('Fred').service('PointageDuplicatorTimeService', PointageDuplicatorTimeService);

    PointageDuplicatorTimeService.$inject = ['$log'];

    function PointageDuplicatorTimeService($log) {


        var service = {
            monthIsImpactedByDuplication: monthIsImpactedByDuplication
        };

        return service;

        //////////////////////////////////////////////////////////////////
        // PUBLICS METHODES                                             //
        //////////////////////////////////////////////////////////////////

        function monthIsImpactedByDuplication(resultDialog) {
            var result = false;
            var firstDayOfMonth = new Date(resultDialog.DatePointage.getFullYear(), resultDialog.DatePointage.getMonth(), 1);
            var lastDayOfMonth = new Date(resultDialog.DatePointage.getFullYear(), resultDialog.DatePointage.getMonth() + 1, 0);
            var listOfDaysOfMonth = enumerateDaysBetweenDates(firstDayOfMonth, lastDayOfMonth);
            for (var i = 0; i < listOfDaysOfMonth.length; i++) {

                var monthDay = listOfDaysOfMonth[i];
                var isInMonthInclusive = moment(monthDay).isBetween(resultDialog.startDate, resultDialog.endDate, 'days', '[]');
                if (isInMonthInclusive) {
                    result = true;
                    break;
                }
            }
            return result;

        }

        function enumerateDaysBetweenDates(startDate, endDate) {
            var dates = [];

            var currDate = moment(startDate).startOf('day');
            var lastDate = moment(endDate).startOf('day');
            dates.push(currDate.clone().toDate());
            while (currDate.add(1, 'days').diff(lastDate) < 0) {
                dates.push(currDate.clone().toDate());
            }
            dates.push(lastDate.clone().toDate());
            return dates;
        }

    }
})();
