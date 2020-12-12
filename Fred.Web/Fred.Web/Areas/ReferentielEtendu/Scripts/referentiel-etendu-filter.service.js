(function () {
    'use strict';

    angular.module('Fred').service('ReferentielEtenduFilterService', ReferentielEtenduFilterService);

    function ReferentielEtenduFilterService() {

        var services = {
            ressourcesFilter: ressourcesFilter,
            sousChapitresFilter: sousChapitresFilter,
            chapitresFilter: chapitresFilter,
            chapitresCascadeFilter: chapitresCascadeFilter,
            sousChapitresCascadeFilter: sousChapitresCascadeFilter,
        };

        return services;

        /*
         * @description Les 3 filtres suivants sont complémentaires:
         *              Ils permettent de filtrer sur le code et le libellé d'une ressource sur 3 niveaux (Chapitre, Sous Chapitre et Ressource)
         */

        /*
         * @function chapitresFilter(searchText)
         * @description Filtre des Chapitres
         */
        function chapitresFilter(searchText) {
            return function (item) {
                for (var i = 0; i < item.SousChapitres.length; i++) {
                    for (var j = 0; j < item.SousChapitres[i].Ressources.length; j++) {
                        if (item.SousChapitres[i].Ressources[j].Code.toLowerCase().indexOf(searchText.toLowerCase()) >= 0 || item.SousChapitres[i].Ressources[j].Libelle.toLowerCase().indexOf(searchText.toLowerCase()) >= 0) {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        /*
         * @function sousChapitresFilter(searchText)
         * @description Filtre des Sous Chapitres
         */
        function sousChapitresFilter(searchText) {
            return function (item) {
                for (var i = 0; i < item.Ressources.length; i++) {
                    if (item.Ressources[i].Code.toLowerCase().indexOf(searchText.toLowerCase()) >= 0 || item.Ressources[i].Libelle.toLowerCase().indexOf(searchText.toLowerCase()) >= 0) {
                        return true;
                    }
                }
                return false;
            }
        }

        /*
         * @function ressourcesFilter(searchText)
         * @description Filtre des Ressources
         */
        function ressourcesFilter(searchText) {
            return function (item) {
                return traitementRessources(item, searchText);
            }
        }

        /*
         * @function chapitresCascadeFilter(searchText)
         * @description Filtre en cascade des Chapitres
         */
        function chapitresCascadeFilter(searchText) {
            return function (item) {
                var traitementSousChapitresEtRessources = function (sousChapitre) {
                    //3- Soit Code Libellé du SousChapitre est trouvé par la recherche
                    //4- Si non, on recherche si une de ses Ressources est trouvée
                    return sousChapitre.CodeLibelle.toLowerCase().indexOf(searchText.toLowerCase()) >= 0
                        || sousChapitre.Ressources.some((r) => traitementRessources(r, searchText));
                };

                //1- Soit Code Libellé du Chapitre est trouvé par la recherche
                //2- Si non, on recherche si un de ses Sous-Chapitre est trouvé
                return item.CodeLibelle.toLowerCase().indexOf(searchText.toLowerCase()) >= 0
                    || item.SousChapitres.some(traitementSousChapitresEtRessources);
            }
        }

        /*
         * @function sousChapitresCascadeFilter(searchText)
         * @description Filtre en cascade des SousChapitres
         */
        function sousChapitresCascadeFilter(searchText) {
            return function (item) {
                //1- Soit Code Libellé du SousChapitre est trouvé par la recherche
                //2- Si non, on recherche si une de ses Ressources est trouvée
                return item.CodeLibelle.toLowerCase().indexOf(searchText.toLowerCase()) >= 0
                    || item.Ressources.some((r) => traitementRessources(r, searchText));
            }
        }

        /*
         * @function traitementRessources(searchText)
         * @description Vérifie si la recherche est trouvée dans la Ressource
         */
        function traitementRessources(ressource, searchText) {
            return ressource.CodeLibelle.toLowerCase().indexOf(searchText.toLowerCase()) >= 0;            
        }
    }
})();