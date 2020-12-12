(function () {
    'use strict';

    angular.module('Fred').service('BibliothequePrixService', BibliothequePrixService);
    BibliothequePrixService.$inject = [];

    function BibliothequePrixService() {

        var service = {

            // Enumérations
            CopierValeursActionEnum: {
                Etablissement: "Etablissement",
                CI: "CI"
            },

            // Functions
            ReferentielsAreSame: ReferentielsAreSame,
            GetFavoriObj: GetFavoriObj
        };

        return service;

        //////////////////////////////////////////////////////////////////
        // Comparaison de référentiels                                  //
        //////////////////////////////////////////////////////////////////

        // Indique si des référentiels sont identiques (chapitre, sous-chapitre et ressources)
        // - chapitres1 : un référentiel à comparer
        // - chapitres2 : l'autre référentiel de chapitre à comparer
        function ReferentielsAreSame(chapitres1, chapitres2) {
            if (chapitres1.length !== chapitres2.length) {
                return false;
            }
            for (var i = 0; i < chapitres1.length; i++) {
                if (!ChapitresAreSame(chapitres1[i], chapitres2[i])) {
                    return false;
                }
            }
            return true;
        }

        // Indique si des chapitres sont identiques (sous-chapitre et ressources)
        // - chapitre1 : un chapitre à comparer
        // - chapitre1 : l'autre chapitre à comparer
        function ChapitresAreSame(chapitre1, chapitre2) {
            if (chapitre1.Code !== chapitre2.Code
                || chapitre1.Libelle !== chapitre2.Libelle
                || chapitre1.SousChapitres.length !== chapitre2.SousChapitres.length) {
                return false;
            }
            for (var i = 0; i < chapitre1.SousChapitres.length; i++) {
                if (!SousChapitresAreSame(chapitre1.SousChapitres[i], chapitre2.SousChapitres[i])) {
                    return false;
                }
            }
            return true;
        }

        // Indique si des sous-chapitres sont identiques (ressources)
        // - currentSousChapitre : le sous-chapitre courant
        // - loadedSousChapitre : le sous-chapitre en cours de chargement
        function SousChapitresAreSame(sousChapitre1, sousChapitre2) {
            if (sousChapitre1.Code !== sousChapitre2.Code
                || sousChapitre1.Libelle !== sousChapitre2.Libelle
                || sousChapitre1.Ressources.length !== sousChapitre2.Ressources.length) {
                return false;
            }
            for (var i = 0; i < sousChapitre1.Ressources.length; i++) {
                if (!RessourcesAreSame(sousChapitre1.Ressources[i], sousChapitre2.Ressources[i])) {
                    return false;
                }
            }
            return true;
        }

        // Indique si des ressources sont identiques
        // - currentRessource : la ressource courante
        // - loadedRessource : le ressource en cours de chargement
        function RessourcesAreSame(ressource1, ressource2) {
            return ressource1.Code === ressource2.Code
                && ressource1.Libelle === ressource2.Libelle
                && ressource1.RessourceId === ressource2.RessourceId;
        }


        //////////////////////////////////////////////////////////////////
        // Autre                                                        //
        //////////////////////////////////////////////////////////////////

        // Retourne un objet qui représente un favori
        function GetFavoriObj(organisationId, deviseId) {
            return {
                OrganisationId: organisationId,
                DeviseId: deviseId
            };
        }
    }
})();
