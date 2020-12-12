/*
 * Ce service sert a persiter les données partielles mis a jour sur une partie de l'ecran au autre partie de l'ecran.
 * Comme la sauvegarde s'effectue sur l'entité entiere Personnel, il faut donc mettre a jour 
 */
(function () {
    'use strict';

    angular.module('Fred').service('PersonnelEditPersisteStateService', PersonnelEditPersisteStateService);


    function PersonnelEditPersisteStateService() {

        var service = {
            persisteState: persisteState
        };
        return service;

        function persisteState(partialModified, category, partialsPersonnel) {
            switch (category) {
                case "general":
                    persisteStateGeneral(partialModified, partialsPersonnel);
                    break;
                case "contact":
                    persisteStateContact(partialModified, partialsPersonnel);
                    break;
                case "utilisateur":
                    persisteStateUtilisateur(partialModified, partialsPersonnel);
                    break;
            }
        }

        function persisteStateContact(partialModified, partialsPersonnel) {
            partialsPersonnel.forEach(function (partial) {
                partial.Adresse1 = angular.copy(partialModified.Adresse1);
                partial.Adresse2 = angular.copy(partialModified.Adresse2);
                partial.Adresse3 = angular.copy(partialModified.Adresse3);
                partial.CodePostal = angular.copy(partialModified.CodePostal);
                partial.Ville = angular.copy(partialModified.Ville);
                partial.PaysLabel = angular.copy(partialModified.PaysLabel);
                partial.Telephone1 = angular.copy(partialModified.Telephone1);
                partial.Telephone2 = angular.copy(partialModified.Telephone2);
                partial.Email = angular.copy(partialModified.Email);
                partial.LatitudeDomicile = angular.copy(partialModified.LatitudeDomicile);
                partial.LongitudeDomicile = angular.copy(partialModified.LongitudeDomicile);
                partial.Pays = angular.copy(partialModified.Pays);
            });
        }

        function persisteStateGeneral(partialModified, partialsPersonnel) {
            partialsPersonnel.forEach(function (partial) {
                partial.Societe = angular.copy(partialModified.Societe);
                partial.Matricule = angular.copy(partialModified.Matricule);
                partial.Nom = angular.copy(partialModified.Nom);
                partial.Prenom = angular.copy(partialModified.Prenom);
                partial.DateEntree = angular.copy(partialModified.DateEntree);
                partial.DateSortie = angular.copy(partialModified.DateSortie);
                partial.IsInterimaire = angular.copy(partialModified.IsInterimaire);
                partial.CategoriePerso = angular.copy(partialModified.CategoriePerso);
                partial.Statut = angular.copy(partialModified.Statut);
                partial.Ressource = angular.copy(partialModified.Ressource);
                partial.Materiel = angular.copy(partialModified.Materiel);
                partial.EtablissementPaie = angular.copy(partialModified.EtablissementPaie);
                partial.TypeRattachementModel = angular.copy(partialModified.TypeRattachementModel);
                partial.EtablissementRattachement = angular.copy(partialModified.EtablissementRattachement);
            });
        }

        function persisteStateUtilisateur(partialModified, partialsPersonnel) {
            partialsPersonnel.forEach(function (partial) {
                partial.Utilisateur = partialModified.Utilisateur;
                partial.hasSubscribeToEmailSummary = partialModified.hasSubscribeToEmailSummary;
            });

        }

    }
})();
