(function () {
    'use strict';

    angular.module('Fred').service('CloturesPeriodesService', CloturesPeriodesService);

    CloturesPeriodesService.$inject = ['$http'];

    function CloturesPeriodesService($http) {

        var service =
        {
            PlageCisDatesClotureComptableDto: { date: new Date() },
            cloturerAllDepensesExclusDeselectionnes: cloturerAllDepensesExclusDeselectionnes,
            cloturerSeulementDepensesInclusSelectionnes: cloturerSeulementDepensesInclusSelectionnes,
            decloturerSeulementDepensesInclusSelectionnes: decloturerSeulementDepensesInclusSelectionnes,
            getFromList: getFromList,
            OpenAndCloseDatesClotureComptableFromList: OpenAndCloseDatesClotureComptableFromList,
            searchFilter: searchFilter
        };

        function getFromList(list) {
            return $http.post('/api/DatesClotureComptable/GetDatesClotureComptableFromList/', list);
        }

        function OpenAndCloseDatesClotureComptableFromList(list) {
            return $http.post('/api/DatesClotureComptable/OpenAndCloseDatesClotureComptableFromList/', list);
        }

        function cloturerAllDepensesExclusDeselectionnes(isModeBlocDepensesToutSelectionner, filter, dccList, lstFiltreeEtSelectionneeDccPourCloturerDepenses) {
            let date = new Date();
            let identifiantsSelected = lstFiltreeEtSelectionneeDccPourCloturerDepenses.map(function (e) { return e.CiId; });
            let payload = Object.create(service.PlageCisDatesClotureComptableDto);
            payload.Date = date;
            payload.Filter = filter;
            payload.identifiants = identifiantsSelected;
            payload.isModeBlocToutSelectionner = isModeBlocDepensesToutSelectionner;
            payload.isModeDecloturer = false;
            return $http.post(`/api/clotures_periodes/dates_cloture_comptable/depenses`, payload);
        }

        function cloturerSeulementDepensesInclusSelectionnes(isModeBlocDepensesToutSelectionner, filter, lstFiltreeEtSelectionneeDccPourCloturerDepenses) {
            let date = new Date();
            let identifiants = lstFiltreeEtSelectionneeDccPourCloturerDepenses.map(function (e) { return e.CiId; });
            let payload = Object.create(service.PlageCisDatesClotureComptableDto);
            payload.Date = date;
            payload.Filter = filter;
            payload.identifiants = identifiants;
            payload.isModeBlocToutSelectionner = isModeBlocDepensesToutSelectionner;
            payload.isModeDecloturer = false;
            return $http.post(`/api/clotures_periodes/dates_cloture_comptable/depenses`, payload);
        }

        function decloturerSeulementDepensesInclusSelectionnes(isModeBlocDepensesToutSelectionner, filter, lstFiltreeEtSelectionneeDccPourCloturerDepenses) {
            let date = new Date();
            let identifiants = lstFiltreeEtSelectionneeDccPourCloturerDepenses.map(function (e) { return e.CiId; });
            let payload = Object.create(service.PlageCisDatesClotureComptableDto);
            payload.Date = date;
            payload.Filter = filter;
            payload.identifiants = identifiants;
            payload.isModeBlocToutSelectionner = isModeBlocDepensesToutSelectionner;
            payload.isModeDecloturer = true;
            return $http.post(`/api/clotures_periodes/dates_cloture_comptable/depenses`, payload);
        }

        function searchFilter(filter) {
            return $http.post(`/api/clotures_periodes/dates_cloture_comptable`, filter);
        }


        return service; 
    }
})();