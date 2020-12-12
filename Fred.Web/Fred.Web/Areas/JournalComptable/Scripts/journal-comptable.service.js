(function () {
    'use strict';

    angular.module('Fred').service('JournalComptableService', JournalComptableService);

    JournalComptableService.$inject = ['$http'];

    function JournalComptableService($http) {
        return {
            GetFilteredJournalList: function (filters) {
                return $http.post('/api/Journal/GetFilteredJournalList', filters, { cache: false });
            },
            isAlreadyUsed: function (id) {
                return $http.get('/api/Journal/IsAlreadyUsed/' + id);
            },
            SaveJournal: function (journal, isNewJournal) {
                if (isNewJournal) { return $http.post('/api/Journal/CreateJournal', journal, { cache: false }); }

                return $http.post('/api/Journal/UpdateJournalCodeLibelleDateCloture', journal, { cache: false });
            },
            DeleteJournal: function (journalId) {
                return $http.delete('/api/Journal/DeleteJournal/' + journalId);
            },
            GetJournaux: function (societeId) {
                return $http.get('/api/Journal/GetJournaux/' + societeId);
            },
            GetJournauxActifs: function (societeId) {
                return $http.get('/api/Journal/GetJournauxActifs/' + societeId);
            },
            UpdateFamilleOperationDiverseJournal: function (journaux) {
                return $http.post('/api/Journal/UpdateJournaux', journaux , { cache: false });
            }
        };
    }
})();