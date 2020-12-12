(function (angular) {
    'use strict';

    angular.module('Fred').controller('EtablissementPaieController', EtablissementPaieController);

    EtablissementPaieController.$inject = ['$scope', '$timeout', 'Notify', 'NgMap', 'EtablissementPaieService', 'ProgressBar', 'confirmDialog', 'GoogleService', 'PaysService', 'fredDialog', 'UserService'];

    function EtablissementPaieController($scope, $timeout, Notify, NgMap, EtablissementPaieService, ProgressBar, confirmDialog, GoogleService, PaysService, fredDialog, UserService) {

        /* -------------------------------------------------------------------------------------------------------------
         *                                            INIT
         * -------------------------------------------------------------------------------------------------------------
         */

        // Instanciation Objet Ressources
        $scope.resources = resources;
        $scope.societeId = undefined;

        // Instanciation de la recherche
        $scope.recherche = "";
        $scope.carto = {
            latitude: 0,
            longitude: 0
        };

        NgMap.getMap("map").then(function (map) {
            $scope.map = map;
            $scope.map.setOptions({ disableDoubleClickZoom: true });

            GoogleService.addSelectPositionButton($scope.resources, $scope.map, $scope.mapSelector, $scope.handleSelectLocation);

            google.maps.event.addListener($scope.map, 'dblclick', function (e) {
                $scope.map.markers.mapSelectorMap.setMap($scope.map);
                var lat = e.latLng.lat();
                var lng = e.latLng.lng();
                $scope.mapSelector = {
                    Latitude: lat.toFixed(7),
                    Longitude: lng.toFixed(7)
                };

                GoogleService.displaySelectPositionButton($scope.mapSelector || $scope.map.markers.mapSelectorMap.map);

                $timeout(angular.noop);
            });
        }).catch(function (error) {
            console.log(error);
        });

        // Attribut d'affichage de la liste
        $scope.checkDisplayOptions = "close-right-panel";
        $scope.checkFormatOptions = "small";

        UserService.getCurrentUser().then(function(user) {
            $scope.userOrganisationId = user.Personnel.Societe.Organisation.OrganisationId;
        });

        // Selection dans la Picklist société
        $scope.loadData = function () {
            $scope.societeId = $scope.societe.SocieteId;
            $scope.societeLibelle = $scope.societe.CodeLibelle;
            $scope.etablissementComptableMandatory = false;
            if ($scope.societe && $scope.societe.Groupe && $scope.societe.Groupe.Code.trim() === "GFES") {
                $scope.etablissementComptableMandatory = true;
            }
            if ($scope.societe && $scope.societe.Groupe && $scope.societe.Groupe.Code.trim() === "GRZB") {
                $scope.isGRZB = true;
            }
            // Chargement des données
            $scope.actionInitSearch();
            $scope.actionLoad(true);
            $scope.actionNewEtablissementPaie();
        };

        /* -------------------------------------------------------------------------------------------------------------
         *                                            HANDLERS
         * -------------------------------------------------------------------------------------------------------------
         */

        // Handler de sélection d'une ligne de le repeater Angular
        $scope.handleSelect = function (item) {
            $scope.etabPaie = angular.copy(item);
            //$scope.setAgencesRattachement(0);
            $scope.checkDisplayOptions = "open";
            $scope.changeFormModel = false;

            $scope.map.setCenter(new google.maps.LatLng($scope.etabPaie.Latitude, $scope.etabPaie.Longitude));

            // Permet la mise à jour de la map à l'ouverture du paneau latéral
            var center = $scope.map.getCenter();
            $timeout(function () {
                google.maps.event.trigger($scope.map, 'resize');
                // $scope.map.setMapTypeId(google.maps.MapTypeId.HYBRID);
                $scope.map.setCenter(center);
            }, 0);
        };

        // Handler de click sur le bouton ajouter
        $scope.handleClickCreateNew = function () {
            if ($scope.societeId !== undefined) {
                $scope.formEtabPaie.$setPristine();
                $scope.formEtabPaie.Code.$setValidity('exist', true);
                $scope.actionNewEtablissementPaie();
                $scope.checkDisplayOptions = "open";
                $scope.changeFormModel = false;
            }
        };

        // Handler de click sur le bouton Enregistrer
        $scope.handleClickAddOrUpdate = function () {
            if ($scope.isGRZB) {
                handleClickAddOrUpdateRZB(false, true);
            }
            else {
                $scope.actionAddOrUpdate(false, true);
            }
        };

        // Handler de click sur le bouton Enregistrer et Nouveau
        $scope.handleClickAddOrUpdateAndNew = function () {
            if ($scope.isGRZB) {
                handleClickAddOrUpdateRZB(true, true);
            }
            else {
                $scope.actionAddOrUpdate(true, true);
            }
        };

        // Handler de click sur le bouton Cancel
        $scope.handleClickCancel = function () {
            $scope.actionCancel();
        };

        // Handler de frappe clavier dans le champs recherche
        $scope.handleSearch = function (recherche) {
            $scope.recherche = recherche;
            $scope.actionLoad();
        };

        // Handler de frappe clavier dans le champs code
        $scope.handleChangeCode = function () {
            if (!$scope.formEtabPaie.Code.$error.pattern) {
                var idCourant;

                if ($scope.etabPaie.EtablissementPaieId !== undefined)
                    idCourant = $scope.etabPaie.EtablissementPaieId;
                else
                    idCourant = 0;
                if ($scope.societe !== null)
                    $scope.existCodeEtabPaie(idCourant, $scope.etabPaie.Code, $scope.societe.SocieteId);
            }
        };

        // Recherche et résolution d'Adresse via le service Google
        $scope.handleCheckGps = function (m) {
            var a = $scope.etabPaie;
            var pays = a.Pays ? a.Pays.Libelle : "";

            GoogleService.geocode(a.Adresse, a.Adresse2, a.Adresse3, a.CodePostal, a.Ville, pays).then(function (response) {
                var data = response.data;
                if (data && data.length > 0) {
                    $scope.addressList = data;
                    $scope.etabPaie.Latitude = data[0].Geometry.Location.Lat;
                    $scope.etabPaie.Longitude = data[0].Geometry.Location.Lng;
                    $scope.showAddressList = true;
                }
            }).catch(function (error) {
                Notify.error(error);
            });
        };

        // Selection d'une Adresse proposée par Google et ajouter des coordonnées GPS
        $scope.handleChangeSelectAdress = function (item) {
            $scope.etabPaie.Adresse = item.Adresse.Adresse1;
            $scope.etabPaie.Adresse2 = item.Adresse.Adresse2;
            $scope.etabPaie.Adresse3 = item.Adresse.Adresse3;
            $scope.etabPaie.CodePostal = item.Adresse.CodePostal;
            $scope.etabPaie.Ville = item.Adresse.Ville;
            $scope.etabPaie.Latitude = item.Geometry.Location.Lat;
            $scope.etabPaie.Longitude = item.Geometry.Location.Lng;
            $scope.showAddressList = false;
            actionGetPays(item.Adresse.Pays);
        };

        $scope.handleSelectLocation = function () {
            $scope.map.markers.mapSelectorMap.setMap(null);
            $scope.carto.latitude = $scope.mapSelector.Latitude;
            $scope.carto.longitude = $scope.mapSelector.Longitude;
            GoogleService.inverserGeocode($scope.carto.latitude, $scope.carto.longitude).then(function (response) {
                var data = response.data;
                if (data && data.length > 0) {
                    setAddressWithInverseGeocode(data[0], $scope.carto.latitude, $scope.carto.longitude);
                }
            }).catch(function (error) {
                Notify.error(error.data.Message);
            });
        };

        function setAddressWithInverseGeocode(item, lat, lng) {
            $scope.etabPaie.Adresse = item.Adresse.Adresse1;
            $scope.etabPaie.Adresse2 = item.Adresse.Adresse2;
            $scope.etabPaie.Adresse3 = item.Adresse.Adresse3;
            $scope.etabPaie.CodePostal = item.Adresse.CodePostal;
            $scope.etabPaie.Ville = item.Adresse.Ville;
            $scope.etabPaie.Latitude = Number(lat);
            $scope.etabPaie.Longitude = Number(lng);
            $scope.showAddressList = false;
            actionGetPays(item.Adresse.Pays);
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            ACTIONS
         * -------------------------------------------------------------------------------------------------------------
         */

        // Action click sur les boutons Enregistrer
        $scope.actionAddOrUpdate = function (newItem, withNotif) {
            if ($scope.formEtabPaie.$invalid)
                return;
            if ($scope.etabPaie.EtablissementPaieId === 0)
                $scope.actionCreate(newItem, withNotif);
            else
                $scope.actionUpdate(newItem, withNotif);
        };

        // Action Create
        $scope.actionCreate = function (newItem, withNotif) {
            $scope.etabPaie.AgenceRattachement = null;
            EtablissementPaieService.Create($scope.etabPaie).then(function () {
                $scope.actionLoad(false);
                if (newItem) {
                    $scope.handleClickCreateNew();
                }
                else {
                    $scope.actionCancel();
                }
                ProgressBar.complete();
                if (withNotif) Notify.message(resources.Global_Notification_Enregistrement_Success);
            }, function (reason) {
                console.log(reason);
                ProgressBar.complete();
                if (withNotif) Notify.error(resources.Global_Notification_Error);
            });
        };

        // Action Update
        $scope.actionUpdate = function (newItem, withNotif) {
            EtablissementPaieService.Update($scope.etabPaie).then(function () {
                $scope.actionLoad(false);
                if (newItem) {
                    $scope.handleClickCreateNew();
                }
                else {
                    $scope.actionCancel();
                }
                ProgressBar.complete();
                if (withNotif) Notify.message(resources.Global_Notification_Enregistrement_Success);
            }, function (reason) {
                console.log(reason);
                ProgressBar.complete();
                if (withNotif) Notify.error(resources.Global_Notification_Error);
            });
        };

        // Action Cancel
        $scope.actionCancel = function () {
            $scope.checkDisplayOptions = "close-right-panel";
            $scope.formEtabPaie.$setPristine();
            $scope.formEtabPaie.Code.$setValidity('exist', true);
        };

        // Action Delete
        $scope.handleClickDelete = function (etabPaie) {
            $scope.checkDisplayOptions = "close-right-panel";
            confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationSuppression).then(function () {
                EtablissementPaieService.Delete(etabPaie).then(function () {
                    $scope.actionLoad(false);
                    ProgressBar.complete();
                    $scope.actionCancel(true);
                }, function (reason) {
                    console.log(reason);
                    Notify.error(resources.Global_Notification_Suppression_Error);
                });
            });
        };

        // Action initalisation d'un nouvel Etablissement Paie
        $scope.actionNewEtablissementPaie = function () {
            EtablissementPaieService.New($scope.societeId).then(function (value) {
                $scope.etabPaie = value;
            }, function (reason) {
                console.log(reason);
            });
        };

        // Action Load
        $scope.actionLoad = function (withNotif) {
            EtablissementPaieService.Search($scope.filters, $scope.societeId, $scope.recherche).then(function (value) {
                $scope.items = value;
                if (value && value.length === 0) {
                    Notify.error(resources.Global_Notification_AucuneDonnees);
                }
            }, function (reason) {
                console.log(reason);
                if (withNotif) Notify.error(resources.Global_Notification_Error);
            });
            ProgressBar.complete();
        };

        // Action d'initialisation de la recherche muli-critère des Etablissements Paie
        $scope.actionInitSearch = function () {
            $scope.filters = { Code: true, Libelle: true };
        };

        // Action de test d'existence de l'Etablissement Paie
        $scope.existCodeEtabPaie = function (idCourant, code, societeId) {
            EtablissementPaieService.CodeExists(idCourant, code, societeId).then(function (value) {
                if (value) {
                    $scope.formEtabPaie.Code.$setValidity('exist', false);
                } else {
                    $scope.formEtabPaie.Code.$setValidity('exist', true);
                }
            }, function (reason) {
                console.log(reason);
            });
        };

        /**
         * Récupération du pays par son libellé
         * @param {any} libelle libellé du pays récupéré de google
         * @returns {Promise.<any>} a promise
         */
        function actionGetPays(libelle) {
            return PaysService.GetByLibelle(libelle)
                .then(function (result) {
                    $scope.etabPaie.Pays = result.data;
                    $scope.etabPaie.PaysId = result.data.PaysId;
                }).catch(Notify.defaultError);
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            DIVERS
         * -------------------------------------------------------------------------------------------------------------
         */

        // pickListeAgenceRattachement
        $scope.showPickList = function (val) {
            var baseControllerUrl = '/api/' + val + '/SearchLight/?page={0}&societeId={1}&agenceId={2}';
            $scope.apiController = val;
            switch (val) {
                case "EtablissementPaie":
                    baseControllerUrl = String.format(baseControllerUrl, 1, $scope.societeId, $scope.etabPaie.EtablissementPaieId);
                    break;
            }
            return baseControllerUrl;
        };

        // pickListeAgenceRattachement
        $scope.loadAgenceRattachements = function (item) {
            $scope.etabPaie.AgenceRattachement = item;
            $scope.etabPaie.AgenceRattachementId = item.IdRef;
            if (!$.isNumeric($scope.etabPaie.AgenceRattachementId))
                $scope.etabPaie.AgenceRattachementId = null;
        };

        // pickListeAgenceRattachement - delete
        $scope.handleDeletePickListAgenceRattachement = function () {
            $scope.etabPaie.AgenceRattachement = null;
            $scope.etabPaie.AgenceRattachementId = null;
        };

        function handleClickAddOrUpdateRZB(newItem, withNotif) {
            if ($scope.formEtabPaie.$invalid)
                return;

            if ($scope.etabPaie) {
                if ($scope.etabPaie.IsPersonnelsNonPointables === true) {
                    fredDialog.confirmation(resources.EtablissementPaie_PersonnelsNonPointable_Confirmation, resources.Global_Modal_Confirmation, 'flaticon flaticon-warning',
                        resources.Global_Bouton_Valider, resources.Global_Bouton_Annuler, function () { return ConfirmAddUpdate(newItem, withNotif); }, CancelUpdatePointable);
                }
                else {
                    fredDialog.confirmation(resources.EtablissementPaie_PersonnelsPointable_Confirmation, resources.Global_Modal_Confirmation, 'flaticon flaticon-warning',
                        resources.Global_Bouton_Valider, resources.Global_Bouton_Annuler, function () { return ConfirmAddUpdate(newItem, withNotif); }, CancelUpdatePointable);
                }
            }
        }

        function ConfirmAddUpdate(newItem, withNotif) {
            $scope.actionAddOrUpdate(newItem, withNotif);
            EtablissementPaieService.UpdatePersonnelsPointableByEtatPaieId($scope.etabPaie.EtablissementPaieId, $scope.etabPaie.IsPersonnelsNonPointables);
        }

        function CancelUpdatePointable() {
            $scope.etabPaie.IsPersonnelsNonPointables = !$scope.etabPaie.IsPersonnelsNonPointables;
        }
    }
})(angular);