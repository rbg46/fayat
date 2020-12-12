(function () {
    'use strict';
    angular
        .module('Fred')
        .component('lookupPanelFournisseurAgence', {
            templateUrl: '/Scripts/Components/LookupPanel/Custom/lookup-panel-fournisseur-agence.template.html',
            bindings: {

                showEventName: '@',

                onAddItem: '&',

                onCancel: '&',

                onItemSelected: '&',

                defaultSearch: '<',

                currentFournisseur: '<',

                currentAgence: '<',

                currentCi: '<',

                withCommandeValide: '<',

                canAdd: '<'
            },
            controller: ['$scope', 'FournisseurService', 'focus', 'UserService', LookupPanelFournisseurAgence]
        });


    function LookupPanelFournisseurAgence($scope, FournisseurService, focus, UserService) {

        var $ctrl = this;

        $ctrl.$onInit = function () {

            // Constantes
            $ctrl.itemsPerPage = 20;

            // Init
            $ctrl.resources = resources;

            UserService.getCurrentUser().then(function(user) {
                $ctrl.currentUser = user.Personnel;
            });

            $ctrl.isComponentVisible = false;
            $ctrl.isPanelFournisseursVisible = false;
            $ctrl.isPanelAgencesVisible = false;
            $ctrl.busy = false;
            $ctrl.canLoadMore = true;

            $ctrl.fournisseursItems = [];
            $ctrl.agencesItems = [];
            $ctrl.selectedFournisseur = null;
            $ctrl.selectedAgence = null;

            // Filter
            $ctrl.currentFilter = {
                recherche1: null,
                recherche2: null,
                page: 1
            };

            if (!$ctrl.showEventName) {
                $ctrl.showEventName = "showDualPicklist";
            }

            $scope.$on($ctrl.showEventName, function () {

                // Si l'utilisateur n'a fait aucune recherche
                // Alors initialiser le libellé avec une valeur par défaut
                if (!$ctrl.currentFilter.recherche1) {
                    $ctrl.currentFilter.recherche1 = $ctrl.defaultSearch;
                }

                // Show Panel
                $ctrl.handleShowPanel();

            });
        };


        // ################# EVENTS FROM PARENT ###################


        // ################# HANDLERS ###################

        $ctrl.handleFilterChange = function (filter) {

            // Init indicateur
            $ctrl.canLoadMore = true;

            // Init liste
            $ctrl.fournisseursItems = [];

            // Retour à la page 1
            $ctrl.currentFilter.page = 1;

            // Charger les fournisseurs
            loadFournisseursPromise();
        };

        $ctrl.handleFilterAgencesChange = function (text) {

            // Charger les agences filtrées
            $ctrl.agencesItems = $ctrl.selectedFournisseur.Agences.filter(function (agence) {

                if (text.length === 0) {
                    return true;
                }

                let textToSearch = text.toLowerCase();
                let textAgence = ((agence.Libelle || '') + ' ' + (agence.Code || '') + ' ' + (agence.SIRET || '')).toLowerCase();

                if (textAgence.indexOf(textToSearch) > -1) {
                    return true;
                }

                return false;
            });
        };

        $ctrl.handleAddItem = function (itemToAdd) {

            // Notifier le parent
            $ctrl.onAddItem({ itemToAdd: itemToAdd });

            // Fermer le composant
            hidePanel();
        };

        $ctrl.handleLoadMore = function () {

            // Incrémenter la page
            $ctrl.currentFilter.page++;

            // Charger les fournisseurs
            loadFournisseursPromise();
        };


        $ctrl.handleShowPanel = function () {

            // Init indicateur
            $ctrl.canLoadMore = true;

            // Init nb pages
            $ctrl.currentFilter.page = 1;

            // Charger les fournisseurs lors de l'ouverture du panel
            loadFournisseursPromise();

            // Ouvrir le composant
            showPanel();
        };

        $ctrl.handleHidePanel = function () {

            // Fermer le composant
            hidePanel();

        };

        $ctrl.handleSelectFournisseur = function (item) {

            // Init sélection
            $ctrl.selectedFournisseur = item;
            $ctrl.selectedAgence = null;

            // Si le fournisseur possède des agences
            if (item.Agences && (item.Agences.length > 0)) {

                //Afficher les agences
                $ctrl.isPanelAgencesVisible = true;

                // Clone list
                let listClone = Array.from(item.Agences);

                listClone = listClone.map(function (item) {
                    item.IsAgencePrincipale = false;
                    return item;
                });

                // Ajouter agence principale (agence fictive)
                listClone.splice(0, 0, {
                    AgenceId: -99,
                    Code: $ctrl.selectedFournisseur.Code,
                    Libelle: $ctrl.selectedFournisseur.Libelle,
                    Siret: $ctrl.selectedFournisseur.Siret,
                    SIREN: $ctrl.selectedFournisseur.SIREN,
                    TypeSequence: $ctrl.selectedFournisseur.TypeSequence,
                    Adresse: {
                        Ligne: $ctrl.selectedFournisseur.Adresse,
                        Ville: $ctrl.selectedFournisseur.Ville,
                        CodePostal: $ctrl.selectedFournisseur.CodePostal,
                        PaysId: $ctrl.selectedFournisseur.PaysId,
                        Pays: $ctrl.selectedFournisseur.Pays
                    },
                    IsAgencePrincipale: true
                });

                // Affecter les items
                $ctrl.agencesItems = listClone;

                // Init sélection par défaut si définie
                if ($ctrl.currentAgence) {
                    for (var i = 0; i < $ctrl.agencesItems.length; i++) {
                        if ($ctrl.agencesItems[i].Code === $ctrl.currentAgence.Code) {
                            $ctrl.selectedAgence = $ctrl.agencesItems[i];
                        }
                    }
                }

            } else {

                // Notifier parent
                $ctrl.onItemSelected({
                    fournisseur: $ctrl.selectedFournisseur,
                    agence: $ctrl.selectedAgence
                });

                // Fermer le composant
                hidePanel();
            }
        };

        $ctrl.handleSelectAgence = function (item) {

            // Init sélection
            $ctrl.selectedAgence = item;

            // Notifier parent
            $ctrl.onItemSelected({
                fournisseur: $ctrl.selectedFournisseur,
                agence: $ctrl.selectedAgence
            });

            //Fermer le composant
            hidePanel();
        };

        // ################# ACTIONS ###################

        function showPanel() {

            $ctrl.isComponentVisible = true;
            $ctrl.isPanelFournisseursVisible = true;
            $ctrl.isPanelAgencesVisible = false;

            // Demander le focus sur le input-text
            focus('focus-input-text');
        }

        function hidePanel() {
            $ctrl.isComponentVisible = false;
            $ctrl.isPanelFournisseursVisible = false;
            $ctrl.isPanelAgencesVisible = false;
        }

        function loadFournisseursPromise() {

            // Chargement des données
            $ctrl.busy = true;

            // Si chargement de données en plus possible
            if (!$ctrl.canLoadMore)
                return;

            // Rafraîchir les données
            return FournisseurService.GetFournisseursPromise(
                $ctrl.currentUser.Societe.GroupeId,
                $ctrl.currentFilter.page,
                $ctrl.itemsPerPage,
                $ctrl.currentFilter.recherche1,
                $ctrl.currentFilter.recherche2,
                $ctrl.currentCi,
                $ctrl.withCommandeValide
            )
                .then(function (result) {

                    // Indicateur de chargement de données en plus
                    if (result && result.data && (result.data.length < $ctrl.itemsPerPage))
                    {
                        $ctrl.canLoadMore = false;
                    }

                    // Récupération des fournisseurs
                    if ($ctrl.currentFilter.page === 1) {

                        // Ajouter une nouvelle liste
                        $ctrl.fournisseursItems = result.data;

                        // Init sélection par défaut si définie
                        if ($ctrl.currentFournisseur) {
                            for (var i = 0; i < $ctrl.fournisseursItems.length; i++) {
                                if ($ctrl.fournisseursItems[i].Code === $ctrl.currentFournisseur.Code) {
                                    $ctrl.selectedFournisseur = $ctrl.fournisseursItems[i];
                                }
                            }
                        }
                    } else {

                        // Ajouter des éléments en plus
                        for (var j = 0; j < result.data.length; j++) {
                            $ctrl.fournisseursItems.push(result.data[j]);

                            // Init sélection par défaut si définie
                            if ($ctrl.currentFournisseur) {
                                if (result.data[j].Code === $ctrl.currentFournisseur.Code) {
                                    $ctrl.selectedFournisseur = result.data[j];
                                }
                            }
                        }
                    }

                    // Chainer les resultats
                    return result;

                }).then(function (result) {

                    // Chargement des données
                    $ctrl.busy = false;

                    // Chainer les resultats
                    return result;
                });
        }
    }
})();