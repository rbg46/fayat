(function () {
    'use strict';

    angular.module('Fred').service('RessourcesSpecifiquesCIService', RessourcesSpecifiquesCIService);

    RessourcesSpecifiquesCIService.$inject = ['$http'];

    function RessourcesSpecifiquesCIService($http) {
        var baseUrl = '/api/RessourcesSpecifiquesCI';

        var service = {
            getResources: getResources,
            getNextCodeRessource: getNextCodeRessource,
            addRessource: addRessource,
            updateRessource: updateRessource,
            deleteRessource: deleteRessource,
            generateExcel: generateExcel,
            extractExcel: extractExcel,
            cloneRessource: cloneRessource,
            insertInList: insertInList,
            updateFromList: updateFromList,
            removeFromList: removeFromList
        };

        return service;

        //Appel à l'API pour obtenir la liste des chapitres et sous chapitres avec leurs ressources
        function getResources(ciId) {
            return $http.get(baseUrl + '/Ressources/' + ciId);
        }

        //Appel à l'API pour mettre à jour une ressource
        function updateRessource(ressource) {
            var postData = JSON.stringify(ressource);
            return $http.put(baseUrl + '/UpdateRessource/', postData);
        }

        //Appel à l'API pour pour ajouter une ressource
        function addRessource(ressource) {
            var postData = JSON.stringify(ressource);
            return $http.post(baseUrl + '/AddRessource/', postData);
        }

        //Appel à l'API pour Désactiver une ressource et son lien spécifique ci
        function deleteRessource(item) {
            var ressourceId = item.RessourceId;
            return $http.put(baseUrl + '/DeleteRessource/' + ressourceId);
        }
        //Appel à l'API pour creer un fichier excel
        function generateExcel(ciId) {
            return $http.post(baseUrl + '/GenerateExcel/' + ciId);
        }
        //Appel à l'API pour afficher un fichier excel
        function extractExcel(reponseId) {
            return window.location.href = baseUrl + '/ExtractExcel/' + reponseId;
        }

        //Appel à L'api pour obtenir un nouveau code ressource incrémenté
        function getNextCodeRessource(ressourceRattachementId) {
            return $http.get(baseUrl + '/GetNextRessourceCode/' + ressourceRattachementId);
        }

        //Permet de cloner une ressource
        function cloneRessource(ressource, ciid) {
            var newRessource = angular.copy(ressource);
            newRessource.RessourceId = 0;
            newRessource.IsRessourceSpecifiqueCi = true;
            newRessource.RessourceRattachement = ressource;
            newRessource.RessourceRattachementId = ressource.RessourceId;
            newRessource.RessourcesEnfants = [];
            newRessource.ReferentielEtendus = [];
            newRessource.Parent = null;
            newRessource.ParentId = null;
            newRessource.Active = true;
            newRessource.DateCreation = null;
            newRessource.AuteurCreationId = null;
            newRessource.DateModification = null;
            newRessource.AuteurModificationId = null;
            newRessource.DateSuppression = null;
            newRessource.AuteurSuppressionId = null;
            newRessource.SpecifiqueCiId = ciid;
            return newRessource;
        }

        //Permet d'ajouter une ressource à la liste des chapitres
        function insertInList(chapitresList, ressource) {
            angular.forEach(chapitresList, (c, key) =>
                angular.forEach(c.SousChapitres, (sc, key2) => {
                    if (sc.SousChapitreId === ressource.SousChapitreId) {
                        c.CountRessourceSpecifiquesCI++;
                        sc.CountRessourceSpecifiquesCI++;
                        sc.Ressources.push(ressource);
                    }
                })
            );
            return chapitresList;
        }

        //Permet d'ajouter une ressource à la liste des chapitres
        function updateFromList(chapitresList, ressource) {
            angular.forEach(chapitresList, (c, key) =>
                angular.forEach(c.SousChapitres, (sc, key2) =>
                    angular.forEach(sc.Ressources, (r, key3) => {
                        if (r.RessourceId === ressource.RessourceId) {
                            sc.Ressources.splice(key3, 1, ressource);
                        }
                    })
                )
            );
            return chapitresList;
        }
        

        //Permet de supprimer une ressource d'un sous-chapitre
        function removeFromList(chapitresList, ressource) {
            angular.forEach(chapitresList, (c, key) =>
                angular.forEach(c.SousChapitres, (sc, key2) =>
                    angular.forEach(sc.Ressources, (r, key3) => {
                        if (r.RessourceId === ressource.RessourceId) {
                            c.CountRessourceSpecifiquesCI--;
                            sc.CountRessourceSpecifiquesCI--;
                            sc.Ressources.splice(key3, 1);
                        }
                    })
                )
            );
            return chapitresList;
        }

    }
})();