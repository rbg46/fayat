(function (angular) {
    'use strict';

    angular.module('Fred').controller('FournisseurController', FournisseurController);

    FournisseurController.$inject = ['$timeout', '$q', '$filter', 'Notify', 'ProgressBar', 'NgMap', 'FournisseurService', 'GoogleService', 'favorisService', 'favoriModal'];

    /*
     * @summary : Controlleur Angular Fournisseur
     * @author : FCI-BPE
     * @version : 1.0
     */
    function FournisseurController($timeout, $q, $filter, Notify, ProgressBar, NgMap, FournisseurService, GoogleService, favorisService, favoriModal) {

        var $ctrl = this;

        // méthodes exposées
        angular.extend($ctrl, {
            handleRedirectPersonnelDetail: handleRedirectPersonnelDetail,
            handleOpenDetailFournisseur: handleOpenDetailFournisseur,
            handleClickCancel: handleClickCancel,
            handleAffectedPersonnelList: handleAffectedPersonnelList,
            handleSearch: handleSearch,
            handleGetNewFavori: handleGetNewFavori,
            handleLoadPage: handleLoadPage,
            handleExecuteImportFournisseur: handleExecuteImportFournisseur,
            handleSelect: handleSelect,
            cancelFilter: cancelFilter,
        });

        init();

        /**
         * Initialisation du controller.     
         */
        function init() {
            NgMap.getMap("map").then(function (map) { $ctrl.map = map; }).catch(function (error) { console.log(error); });

            angular.extend($ctrl, {
                // Instanciation Objet Ressources
                resources: resources,
                fournisseurList: [],
                selectedFournisseur: {},
                personnelList: [],
                departements: [],
                permissionKeys: PERMISSION_KEYS,

                checkDisplayFournisseurDetail: "close-right-panel",
                checkDisplayListPersonnel: 'close-right-panel',

                hasMorePage: true,
                busy: false,
                paging: { pageSize: 20, currentPage: 1 },

                searchPersonnel: "",
                filter: null,
                inputSearch: "",
                oldfilter: null
            });

            // Abonnement de l'élément listPersonnel à l'évènement
            FredToolBox.bindScrollEnd('#fournisseurList', actionLoadMore);
        }

        return $ctrl;

        /*
         * -------------------------------------------------------------------------------------------------------------
         *                                            HANDLERS
         * -------------------------------------------------------------------------------------------------------------
         */

        function handleLoadPage(favoriId) {
            var promise;

            // Chargement de la liste des Fournisseurs à partir d'un filtre vierge
            if (favoriId === null || favoriId === "") {
                promise = FournisseurService.GetFilter().$promise;
            }
            // Chargement de la liste des Fournisseurs à partir du filtre du favori
            else {
                promise = favorisService.GetById(favoriId);
            }

            //chargement de liste des départemeent à partir du fichier Datas.js A revoir
            $ctrl.departements = datasMockDepartements;

            ProgressBar.start();
            promise
                .then(actionGetFilter)
                .then(function (filter) { actionSearch(filter, true); })
                .catch(Notify.defaultError);


            ProgressBar.complete();
        }

        /*
         * @summary : Chargement des données d'un fournisseur
         * @version : 1.0
         */
        function handleOpenDetailFournisseur(fournisseur) {
            $ctrl.checkDisplayFournisseurDetail = 'open-right';
            $ctrl.selectedFournisseur = fournisseur;

            // Gestion Google Map
            actionLoadGoogleMap();
            // Permet la mise à jour de la map à l'ouverture du paneau latéral
            var center = $ctrl.map.getCenter();
            $timeout(function () {
                google.maps.event.trigger($ctrl.map, 'resize');
                // $ctrl.map.setMapTypeId(google.maps.MapTypeId.HYBRID);
                $ctrl.map.setCenter(center);
            }, 0);
        }

        /*
         * @summary     Fermeture du panneau
         * @author      FCI-BPE
         * @version     1.0
         */
        function handleClickCancel() {
            $ctrl.checkDisplayFournisseurDetail = 'close-right-panel';
            $ctrl.checkDisplayListPersonnel = 'close-right-panel';
        }

        /*
         * @summary     Ouverture Panneau + Liste du personnel
         * @author      FCI-BPE
         * @version     1.0
         */
        function handleAffectedPersonnelList(fournisseurId) {
            actionLoadPersonnelList(fournisseurId);
        }

        /*
         * @summary     Redirection vers le détail du personnel
         * @author      FCI-BPE
         * @version     1.0
         */
        function handleRedirectPersonnelDetail(personnel) {
            window.location = "/Personnel/Personnel/Edit/" + personnel.PersonnelId;
        }

        /*
         * @description Handler de recherche avec filtre
         */
        function handleSearch(filter) {
            filter.ValueText = $ctrl.inputSearch;
            saveFilter(filter);
            actionSearch(filter, true);
        }

        /*
         * @description Handler Récupère une nouvelle instance de favori
         */
        function handleGetNewFavori() {
            $q.when()
                .then(actionGetNewFavori)
                .then(function (favori) {
                    favoriModal.open(resources, favori);
                });
        }

        /*
        * @description Execution de l'import des fournisseurs ANAEL vers FRED
        */
        function handleExecuteImportFournisseur() {
            FournisseurService.ExecuteImportFournisseur().then(function (ok) {
                Notify.message(resources.Global_Import_Lance_Succes);
            }).catch(function (error) { Notify.error(resources.Global_Notification_Error); });
        }

        /*
         * -------------------------------------------------------------------------------------------------------------
         *                                            ACTIONS
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @description Chargement des la liste du personnel lié au fournisseur sélectionné
         * @version 1.0
         */
        function actionLoadPersonnelList(fournisseurId) {
            FournisseurService.GetPersonnelInterimaire({ fournisseurId: fournisseurId }).$promise.then(function (response) {
                $ctrl.personnelList = response;
                $ctrl.checkDisplayFournisseurDetail = 'close-right-panel';
                $ctrl.checkDisplayListPersonnel = 'open-right';
            }).catch(function (error) { console.log(error); });
        }

        /*
         * @description Action Rechercher un fournisseur (aussi utilisé pour le chargement initial de la liste des fournisseurs)
         */
        function actionSearch(filter, firstLoad) {
            $ctrl.busy = true;
            if (firstLoad) {
                $ctrl.fournisseurList = [];
                $ctrl.paging.currentPage = 1;
            }
            ProgressBar.start();
            FournisseurService.Search({ page: $ctrl.paging.currentPage, pageSize: $ctrl.paging.pageSize }, filter).$promise
                .then(function (value) {

                    if (value && value.length === 0) {
                        Notify.error(resources.Global_Notification_AucuneDonnees);
                    }
                    else {
                        angular.forEach(value, function (val) {
                            $ctrl.fournisseurList.push(val);
                        });
                        $ctrl.hasMorePage = value.length !== $ctrl.paging.pageSize;
                        return $ctrl.fournisseurList;
                    }
                    return null;
                })
                .catch(function (error) { console.log(error); })
                .then(function (value) {
                    angular.forEach(value, function (val) {
                        // Récupère le nombre de personnel intérimaire lié à chaque fournisseur
                        FournisseurService.GetCountPersonnelInterimaire({ fournisseurId: val.FournisseurId }).$promise.then(function (res) {
                            val.CountPersonnelInterimaire = res.value;
                        });
                    });
                })
                .catch(function (error) { console.log(error); })
                .finally(function () { ProgressBar.complete(); $ctrl.busy = false; });
        }

        /*
         * @description Récupère un nouveau filtre
         */
        function actionGetFilter(value) {
            if ($ctrl.filter === null) {
                $ctrl.filter = value;
            }
            // A refactoriser quand on aura de vrais données géographique (départements) en base de données et une entité Departement ?
            if (value.Departement) {
                var dept = $filter('filter')(datasMockDepartements, { Code: parseInt(value.Departement) }, true)[0];
                $ctrl.filter.dept = dept;
            }
            return $ctrl.filter;
        }

        /* 
         * @function    actionLoadMore()
         * @description Action Chargement de données supplémentaires (scroll end)
         */
        function actionLoadMore() {
            if (!$ctrl.busy && !$ctrl.hasMorePage) {
                $ctrl.paging.currentPage++;
                actionSearch($ctrl.filter, false);
            }
        }

        /*
         * @summary     Récupère l'adresse du fournisseur pour la carte Google Map   
         * @version     1.0
         */
        function actionLoadGoogleMap() {
            var adresse = $ctrl.selectedFournisseur.Adresse + "," + $ctrl.selectedFournisseur.CodePostal + "," + $ctrl.selectedFournisseur.Ville;
            adresse = adresse.trim();
            if (adresse != null && adresse != undefined && adresse !== "") {
                GoogleService.geocode($ctrl.selectedFournisseur.Adresse, "", "", $ctrl.selectedFournisseur.CodePostal, $ctrl.selectedFournisseur.Ville, "").then(function (response) {
                    if (response && response.data && response.data.length > 0) {
                        $ctrl.selectedFournisseur.lat = response.data[0].Geometry.Location.Lat;
                        $ctrl.selectedFournisseur.lng = response.data[0].Geometry.Location.Lng;
                        $ctrl.map.setCenter(new google.maps.LatLng($ctrl.selectedFournisseur.lat, $ctrl.selectedFournisseur.lng));
                    }
                    else {
                        Notify.error(resources.Fournisseur_Controller_ErrorMetier_AucunFournisseur);
                        $ctrl.map.setCenter(new google.maps.LatLng(0, 0));
                    }
                });
            }
        }

        /*
         * @function actionGetNewFavori()
         * @description Récupère un nouvelle objet Favori
         */
        function actionGetNewFavori() {
            return favorisService.GetNew("Fournisseur").then(function (value) {
                $ctrl.favori = value;
                $ctrl.favori.Filtre = $ctrl.filter;
                return $ctrl.favori;
            });
        }

        function handleSelect(type, item) {
            var tmp;

            switch (type) {
                case 'Departement':
                    $ctrl.filter.Departement = item.Code;
                    break;
                case 'CommandeType':
                    if (item === '') {
                        $ctrl.filter.TypeId = null;
                        $ctrl.filter.TypeLibelle = undefined;
                    }
                    else {
                        tmp = JSON.parse(item);
                        $ctrl.filter.TypeId = tmp.CommandeTypeId;
                        $ctrl.filter.TypeLibelle = tmp.Libelle;
                    }
                    break;
                case 'StatutCommande':
                    if (item === '') {
                        $ctrl.filter.StatutId = null;
                        $ctrl.filter.StatutLibelle = undefined;
                    }
                    else {
                        tmp = JSON.parse(item);
                        $ctrl.filter.StatutId = tmp.StatutCommandeId;
                        $ctrl.filter.StatutLibelle = tmp.Libelle;
                    }
                    break;
                case 'SystemeExterne':
                    if (item === '') {
                        $ctrl.filter.SystemeExterneId = null;
                        $ctrl.filter.SystemeExterneLibelle = undefined;
                    }
                    else {
                        tmp = JSON.parse(item);
                        $ctrl.filter.SystemeExterneId = tmp.SystemeExterneId;
                        $ctrl.filter.SystemeExterneLibelle = tmp.LibelleAffiche;
                    }
                    break;
            }
        }

        function cancelFilter() {
            $ctrl.filter = angular.copy($ctrl.oldfilter);
        }

        function saveFilter(filter) {
            $ctrl.oldfilter = angular.copy(filter);
        }

    }

    angular.module('Fred').filter('nomPrenomMatriculeFilter', nomPrenomMatriculeFilter);

    /* Filter du personnel */
    function nomPrenomMatriculeFilter() {
        return function (items, searchText) {
            if (searchText !== undefined && searchText !== null) {
                var filtered = [];
                for (var i = 0; i < items.length; i++) {
                    var item = items[i];
                    if (item.Nom.toLowerCase().indexOf(searchText.toLowerCase()) >= 0 || item.Prenom.toLowerCase().indexOf(searchText.toLowerCase()) >= 0 || item.Matricule.toLowerCase().indexOf(searchText.toLowerCase()) >= 0) {
                        filtered.push(item);
                    }
                }
                return filtered;
            }
            return items;
        };
    }
}(angular));