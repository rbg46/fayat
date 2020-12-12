(function () {
    'use strict';

    var locationMoyenComponent = {
        templateUrl: '/Areas/Moyen/Scripts/Components/location-moyen.component.html',
        bindings: {
            resources: '<'
        },
        controller: LocationMoyenController,
        controllerAs: '$ctrl'
    };

    angular.module('Fred').component('locationMoyenComponent', locationMoyenComponent);

    angular.module('Fred').controller('LocationMoyenController', LocationMoyenController);

    LocationMoyenController.$inject = ['$scope', 'confirmDialog', 'MoyenService', '$q', 'ProgressBar', 'Notify'];

    function LocationMoyenController($scope, confirmDialog, MoyenService, $q, ProgressBar, Notify) {
        var $ctrl = this;
        $ctrl.resources = resources;

        // méthodes exposées
        angular.extend($ctrl, {
            handleGetLookupUrl: handleGetLookupUrl,
            handleLookupSelection: handleLookupSelection,
            handleLookupDeletion: handleLookupDeletion,
            handleValidateForm: handleValidateForm,
            handleValidateFormFields: handleValidateFormFields,
            ouvreCadre: ouvreCadre,
            handleClickItem: handleClickItem,
            handleUpdateMoyenLocation: handleUpdateMoyenLocation,
            handleDeleteMoyenLocation: handleDeleteMoyenLocation,
            handleDeleteLocation: handleDeleteLocation,
            handleCancel: handleCancel
        });

        // Intialisation du component
        // ___________________________________________

        $scope.$on('event.init.location', function (event, obj) {
            init();
        });

        init();

        // ___________________________________________

        /*
         * @function init
         * @description Initialise le component
         */
        function init() {
            $ctrl.moyenModel = {
                NumImmat: null,
                Libelle: null,
                FicheGeneriqueModel: null,
                ModeleLocation: null
            };

            $ctrl.isCadre = false;
            $ctrl.isUpdate = false;
            GetLocation();
        }

        /*
         * Hanlde lookup url
        */
        function handleGetLookupUrl(val) {
            var baseControllerUrl = "";

            switch (val) {
                case "FicheGenerique":
                    baseControllerUrl = '/api/Moyen/FicheGenerique/SearchLight';
                    break;
                case "NumParc":
                    var resourceCode = $ctrl.moyenModel && $ctrl.moyenModel.FicheGeneriqueModel && $ctrl.moyenModel.FicheGeneriqueModel.Ressource
                        ? $ctrl.moyenModel.FicheGeneriqueModel.Ressource.Code : null;

                    baseControllerUrl = String.format('/api/Moyen/SearchLight/?modelMoyen={0}&isLocationView={1}&', resourceCode, true);
                    break;
            }

            return baseControllerUrl;
        }

        //Fonction de chargement des données de l'item sélectionné dans la picklist
        function handleLookupSelection(type, item) {
            if (item) {
                switch (type) {
                    case "FicheGenerique":
                        $ctrl.moyenModel.FicheGeneriqueModel = item;
                        $ctrl.moyenModel.ModeleLocation = null;
                        break;
                    case "NumParc":
                        $ctrl.moyenModel.ModeleLocation = item;
                        break;

                }
            }
            actionValidateForm();
        }
        /*
         * @function handle delete element from the lookup
        */
        function handleLookupDeletion(type) {
            actionLookupDeletion(type);
            actionValidateForm();
        }

        /*
         * @description Gestion de suppression de l'élément sélectionné dans la lookup
         */
        function actionLookupDeletion(type) {
            if (type) {
                switch (type) {
                    case "FicheGenerique":
                        $ctrl.moyenModel.FicheGeneriqueModel = null;
                        break;
                    case "NumParc":
                        $ctrl.moyenModel.ModeleLocation = null;
                        break;
                }
            }
        }

        /*
         * @description Action validate form
         */
        function actionValidateForm() {
            $ctrl.isInvalidForm = !$ctrl.moyenModel.FicheGeneriqueModel || !$ctrl.moyenModel.ModeleLocation || (!$ctrl.moyenModel.Libelle || 0 === $ctrl.moyenModel.Libelle.length);
        }

        /*
         * @description Action hanlde validate form
         */
        function handleValidateForm() {
            actionValidateForm();
            if ($ctrl.isInvalidForm) {
                return;
            }

            let model = $ctrl.moyenModel.ModeleLocation;
            model.MoyenId = $ctrl.moyenModel.MaterielId;
            model.Libelle = $ctrl.moyenModel.Libelle;
            model.Immatriculation = $ctrl.moyenModel.NumImmat;
            model.DateCreation = new Date();
            model.IsLocation = true;
            model.IsImported = false;
            model.AffectationsList = null;
            model.SiteAppartenance = null;
            model.Societe = null;
            model.Ressource = null;
            model.EtablissementComptable = null;

            $q.when()
                .then(ProgressBar.start)
                .then(function () {
                    MoyenService.CreateMoyenLocation(model)
                        .$promise
                        .then(function () {
                            $ctrl.moyenModel = {
                                NumImmat: null,
                                Libelle: null,
                                FicheGeneriqueModel: null,
                                ModeleLocation: null
                            };

                            confirmDialog.confirm($ctrl.resources, 'Un nouveau moyen a été créé, voulez-vous réinitialiser les filtres ?')
                                .then(function () {
                                    $ctrl.isInvalidForm = true;
                                    $scope.$broadcast('event.init.location');
                                });
                        })
                        .catch(function () {
                            Notify.error(resources.Global_Notification_Error);
                        })
                        .finally(function () {
                            ProgressBar.complete();
                        });
                });
        }

        /*
         *Function To handle the update method 
        */
        function handleUpdateMoyenLocation() {
            let model = $ctrl.moyenModel.ModeleLocation;
            model.Libelle = $ctrl.moyenModel.Libelle;
            model.Immatriculation = $ctrl.moyenModel.NumImmat;
            model.MaterielId = model.MaterielId ? model.MaterielId : $ctrl.moyenModel.MaterielId;
            model.MaterielLocationId = model.MaterielLocationId ? model.MaterielLocationId : $ctrl.moyenModel.MaterielLocationId;
            model.AuteurCreationId = $ctrl.moyenModel.AuteurCreationId;
            model.DateCreation = $ctrl.moyenModel.DateCreation;
            model.DateModification = new Date();

            $q.when()
                .then(ProgressBar.start)
                .then(function () {
                    MoyenService.UpdateMoyenLocation(model)
                        .$promise
                        .then(function () {
                            Notify.message($ctrl.resources.msg_ConfirmUpdate);
                            $scope.$broadcast('event.init.location');
                            $scope.$emit('event.refresh.moyen');
                        })
                        .catch(function (error) {
                            Notify.error(resources.Global_Notification_Error);
                        })
                        .finally(function () {
                            ProgressBar.complete();
                        });
                });
        }

        /*
         * @description Hanlde validate form fields
         */
        function handleValidateFormFields() {
            actionValidateForm();
        }

        function ouvreCadre() {
            $ctrl.moyenModel = {
                NumImmat: null,
                Libelle: null,
                FicheGeneriqueModel: null,
                ModeleLocation: null
            };
            $ctrl.isCadre = true;
            $ctrl.isUpdate = false;
        }
        function handleClickItem(item) {
            $ctrl.isInvalidForm = false;
            $ctrl.isUpdate = true;
            $ctrl.moyenModel.Libelle = item.Libelle;
            $ctrl.moyenModel.NumImmat = item.Immatriculation;
            $ctrl.moyenModel.FicheGeneriqueModel = item.FicheGeneriqueModel;
            $ctrl.moyenModel.ModeleLocation = item.ModeleLocation;
            $ctrl.moyenModel.MaterielLocationId = item.MaterielLocationId;
            $ctrl.moyenModel.DateCreation = item.DateCreation;
            $ctrl.moyenModel.MaterielId = item.MaterielId;
            $ctrl.updateItem = item;
            ProgressBar.start();
            MoyenService.GetAffectation({ materielLocationId: item.MaterielLocationId })
                .$promise
                .then(function (res) {
                    $ctrl.moyenModel.ListAffectation = res;
                })
                .catch(function (error) {
                    Notify.error(error);
                })
                .finally(ProgressBar.complete);
            $ctrl.isCadre = true;

        }
        function handleDeleteMoyenLocation() {
            if ($ctrl.moyenModel.ListAffectation.length === 0 || $ctrl.moyenModel.ListAffectation.length === 1 && $ctrl.moyenModel.ListAffectation['0'].AffectationMoyenTypeId === 1) {
                $q.when()
                    .then(ProgressBar.start)
                    .then(function () {
                        MoyenService.DeleteMoyenLocation({ materielLocationId: $ctrl.moyenModel.MaterielLocationId })
                            .$promise
                            .then(function(){
                                Notify.message($ctrl.resources.msg_DeletedLocation);
                                $scope.$broadcast('event.init.location');
                                $scope.$emit('event.applyfilter.moyen');
                            })
                            .catch(function () {
                                Notify.error(resources.Global_Notification_Error);
                            })
                            .finally(function () {
                                ProgressBar.complete();
                            });
                    });
                $ctrl.isCadre = false;
            }
            else {
                Notify.message($ctrl.resources.msg_warningDeletedLocation);
                return;
            }
        }
        function GetLocation() {
            ProgressBar.start();
            MoyenService.GetLocation()
                .$promise
                .then(function (res) {
                    $ctrl.locations = res;
                    $ctrl.FilterInput = '';
                })
                .catch(function (error) {
                    Notify.error(error);
                })
                .finally(ProgressBar.complete);
        }

        function handleDeleteLocation() {
            confirmDialog.confirm($ctrl.resources, $ctrl.resources.msg_ConfirmDeleted)
                  .then(function () {
                      handleDeleteMoyenLocation();
                  })
                    .then(function () {
                        $ctrl.isCadre = false;
                    });
                    
        }
        function handleCancel() {
            $ctrl.isCadre = false;
        }
    }
})();