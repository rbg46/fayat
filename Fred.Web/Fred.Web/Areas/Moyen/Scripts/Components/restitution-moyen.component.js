(function () {
    'use strict';

    var restitutionMoyenComponent = {
        templateUrl: '/Areas/Moyen/Scripts/Components/restitution-moyen.component.html',
        bindings: {
            resources: '<',
            isMaintenance: '<'
        },
        controller: RestitutionMoyenController,
        controllerAs: '$ctrl'
    };

    angular.module('Fred').component('restitutionMoyenComponent', restitutionMoyenComponent);

    angular.module('Fred').controller('RestitutionMoyenController', RestitutionMoyenController);

    RestitutionMoyenController.$inject = ['$scope', 'MoyenService', 'confirmDialog'];

    function RestitutionMoyenController($scope, MoyenService, confirmDialog) {
        var $ctrl = this;
        $ctrl.resources = resources;

        // méthodes exposées
        angular.extend($ctrl, {
            affectationTypeNotAffected: 1,
            affectationTypeCi: 3,
            affectationTypePersonnel: 2,
            affectationTypeParking: 4,
            affectationTypeDepot: 5,
            affectationTypeStock: 6,
            affectationTypeRetourLoueur: 10,
            affectationTypeResteDisponible: 11,
            handleChangeSelection: handleChangeSelection,
            handleFormValidation: handleFormValidation,
            isFormDataInValid: true,
            handleGetLookupUrl: handleGetLookupUrl,
            handleRemoveSiteFilter: handleRemoveSiteFilter,
            handleLookupSelection: handleLookupSelection,
            handleDateValidation: handleDateValidation,
            isSiteSelectionError: false,
            ValidateForm: ValidateForm
        });

        // Intialisation du component
        // ___________________________________________

        $scope.$on('event.init.restitution.mode', function (event, obj) {
            $ctrl.selectedMoyenList = obj.selectedMoyenList;
            actionInitialiseAffectationTypeList(obj);
            init();
        });

        // ___________________________________________

        /*
         * @function init
         * @description Initialise le component
         */
        function init() {
            $ctrl.affectationModel = actionGetFormModel();
            if (actionIsNoItemSelected()) {
                handleChangeSelection();
            }

            $ctrl.isSiteSelectionDisabled = actionGetSiteSelectionStatus();
            $ctrl.messageErroExistingStartDate = $ctrl.isMaintenance
                ? 'La date de maintenance est inférieure à au moins une date de début existante de/des affectation(s) choisie(s) !'
                : 'La date de restitution est inférieure à au moins une date de début existante de/des affectation(s) choisie(s) !';
            $ctrl.dateDebutName = $ctrl.isMaintenance ? 'Date de début' : 'Date de restitution';
            $ctrl.title = $ctrl.isMaintenance ? 'Maintenance' : 'Restituer à';

            $ctrl.isErrorExistingStartDate = false;
            $ctrl.dateDebuInvalid = false;
            $ctrl.dateFinInvalid = false;
            $ctrl.isSiteSelectionError = false;

            actionValidateForm(true);
        }

        /*
         * @description action get form model
         */
        function actionGetFormModel() {
            $ctrl.isCheckDateDebutActif = false;
            $ctrl.isCheckErrorExistingStartDates = true;

            var intialModel = {
                Ci: null,
                Personnel: null,
                CiId: null,
                PersonnelId: null,
                DateDebut: new Date(),
                DateFin: null,
                Conducteur: null,
                ConducteurId: null,
                Commentaire: null,
                AffectedTo: null,
                AffectationMoyenTypeId: $ctrl.affectationTypeNotAffected,
                Site: null,
                SiteId: null,
                Libelle: null,
                Immatriculation: null
            };

            if ($ctrl.selectedMoyenList && $ctrl.selectedMoyenList.length === 1) {
                $ctrl.isCheckDateDebutActif = true;
                $ctrl.isCheckErrorExistingStartDates = false;

                var firstElement = $ctrl.selectedMoyenList[0];
                $ctrl.existingStartDate = new Date(firstElement.DateDebut);

                // Initialiser le type de restitution
                angular.forEach($ctrl.AffecationTypeList, function (item) {
                    item.Selected = item && item.AffectationMoyenTypeId === firstElement.AffectationMoyenTypeId;
                });

                if (firstElement.AffectationMoyenTypeId === $ctrl.affectationTypePersonnel ||
                    firstElement.AffectationMoyenTypeId === $ctrl.affectationTypeCi ||
                    firstElement.AffectationMoyenTypeId === $ctrl.affectationTypeNotAffected) {
                    $ctrl.AffecationTypeList[0].Selected = true;

                    return intialModel;
                }

                return {
                    Ci: firstElement.Ci,
                    Personnel: firstElement.Personnel,
                    DateDebut: firstElement.DateDebut,
                    DateFin: firstElement.DateFin,
                    Conducteur: firstElement.Conducteur,
                    ConducteurId: firstElement.ConducteurId,
                    Commentaire: firstElement.Commentaire,
                    AffectedTo: firstElement.AffectedTo,
                    AffectationMoyenTypeId: firstElement.AffectationMoyenTypeId,
                    Site: firstElement.Site,
                    SiteId: firstElement.SiteId,
                    Libelle: firstElement.Libelle,
                    Immatriculation: firstElement.Immatriculation
                };
            } else {
                return intialModel;
            }
        }

        /*
         * @description Hanlde change selection
         */
        function handleChangeSelection(typeId) {
            let appliedTypeId = typeId;

            // La séléction par défault est le premier élement
            if (actionIsNoItemSelected()) {
                $ctrl.AffecationTypeList[0].Selected = true;
                appliedTypeId = $ctrl.AffecationTypeList[0].AffectationMoyenTypeId;
            }

            $ctrl.affectationModel.AffectationMoyenTypeId = appliedTypeId;
            let items = $ctrl.AffecationTypeList.filter(function (e) {
                return e.AffectationMoyenTypeId !== appliedTypeId;
            });

            if (items && items.length > 0) {
                angular.forEach(items, function (a) {
                    a.Selected = false;
                });
            }

            $ctrl.isSiteSelectionDisabled = appliedTypeId === MoyenService.affectationTypeIdRetourAuLoueur;
            if ($ctrl.isSiteSelectionDisabled) {
                $ctrl.affectationModel.Site = null;
                $ctrl.affectationModel.SiteId = null;
            }

            actionValidateForm();
        }


        /*
        * @description Handle form validation
        */
        function ValidateForm() {
            let itemsToReaffect = $ctrl.selectedMoyenList.filter(function (s) {
                return s.AffectationMoyenTypeId !== $ctrl.affectationTypeNotAffected;
            });
            let isStartDateWarning = itemsToReaffect && itemsToReaffect.some(function (s) {
                return MoyenService.comparingTwoDates($ctrl.affectationModel.DateDebut, s.DateDebut);
            });

            if (isStartDateWarning) {
                confirmDialog.confirm($ctrl.resources, $ctrl.resources.Moyen_Warning_Modification_Date)
                    .then(function () {
                        handleFormValidation(itemsToReaffect);
                    });
            }
            else {
                handleFormValidation(itemsToReaffect);
            }
        }


        /*
         * @description Hanlde form validation
         */
        function handleFormValidation(itemsToReaffect) {
            let affectationMoyenList = [];

            let itemsToAffect = $ctrl.selectedMoyenList.filter(function (s) {
                return s.AffectationMoyenTypeId === $ctrl.affectationTypeNotAffected;
            });

            actionUpdateAffectationMoyenList(itemsToAffect, affectationMoyenList);

            let itemsToUpdate = itemsToReaffect.filter(function (s) {
                return MoyenService.comparingTwoDates($ctrl.affectationModel.DateDebut, s.DateDebut);
            });
            actionUpdateAffectationMoyenList(itemsToUpdate, affectationMoyenList);

            let itemsToRestituer = $ctrl.selectedMoyenList.filter(function (s) {
                return new Date($ctrl.affectationModel.DateDebut) > new Date(s.DateDebut);
            });

            var selectedItem = $ctrl.AffecationTypeList.filter(function (e) {
                return e.Selected;
            });

            var element = selectedItem[0];

            if (itemsToRestituer && itemsToRestituer.length > 0) {
                angular.forEach(itemsToRestituer, function (v) {
                    // US-6132 - gestion de la date de fin lors d'une réaffectation
                    v.DateFin = MoyenService.getFirstPreviousWorkingDay($ctrl.affectationModel.DateDebut, v.DateDebut);
                    // Fin traitement de l'US-6132
                    v.IsActive = false;

                    var clonedItem = {};
                    cloneAll($ctrl.affectationModel, clonedItem);
                    clonedItem.Moyen = v.Moyen;
                    clonedItem.MoyenId = v.MoyenId;
                    clonedItem.MaterielLocationId = v.MaterielLocationId;
                    clonedItem.AffectationMoyenId = MoyenService.generateUniqueAffectationId(v.MoyenId);
                    clonedItem.AffectedTo = element.Libelle;
                    clonedItem.AffectationMoyenTypeId = element.AffectationMoyenTypeId;
                    clonedItem.TypeAffectation = element.TypeAffectation;
                    clonedItem.SiteId = clonedItem.Site ? clonedItem.Site.SiteId : null;
                    clonedItem.IsActive = !(element.AffectationMoyenTypeId === MoyenService.affectationTypeIdRetourAuLoueur);
                    clonedItem.IsToAdd = true;
                    clonedItem.Libelle = v.Libelle;
                    clonedItem.Immatriculation = v.Immatriculation;
                    clonedItem.ConducteurId = clonedItem.Conducteur ? clonedItem.Conducteur.PersonnelId : null;

                    if (!$ctrl.isMaintenance && element.AffectationMoyenTypeId !== MoyenService.affectationTypeIdRetourAuLoueur) {
                        clonedItem.DateFin = null;
                    } else if (element.AffectationMoyenTypeId === MoyenService.affectationTypeIdRetourAuLoueur) {
                        // [BUG_8406] Si c'est un retour loueur alors DateFin = DateDebut
                        clonedItem.DateFin = clonedItem.DateDebut;
                    }

                    affectationMoyenList.push(clonedItem);
                    affectationMoyenList.push(v);
                });
            }

            $scope.$emit('event.operation.moyen', { affectationMoyenList: affectationMoyenList });
        }

        /*
         * @function actionUpdateAffectationMoyenList
         * @description Update de liste des affectations des moyens
         */
        function actionUpdateAffectationMoyenList(items, affectationMoyenList) {
            if (items && items.length > 0) {
                angular.forEach(items, function (a) {
                    actionUpdateMoyenItem(a, $ctrl.affectationModel);
                    affectationMoyenList.push(a);
                });
            }
        }

        /*
         * @description action update moyen item
         */
        function actionUpdateMoyenItem(a, affectationMoyen) {
            if (!a || !affectationMoyen) {
                return;
            }

            var selectedItem = $ctrl.AffecationTypeList.filter(function (e) {
                return e.Selected;
            });

            var element = selectedItem[0];

            a.CiId = affectationMoyen.Ci ? affectationMoyen.Ci.CiId : null;
            a.PersonnelId = affectationMoyen.Personnel ? affectationMoyen.Personnel.PersonnelId : null;
            a.Ci = affectationMoyen.Ci;
            a.Personnel = affectationMoyen.Personnel;
            a.ConducteurId = affectationMoyen.Conducteur ? affectationMoyen.Conducteur.PersonnelId : null;
            a.Conducteur = affectationMoyen.Conducteur;
            a.DateDebut = affectationMoyen.DateDebut;

            if (element.AffectationMoyenTypeId === MoyenService.affectationTypeIdRetourAuLoueur) {
                // [BUG_8406] Si c'est un retour loueur alors DateFin = DateDebut
                a.DateFin = a.DateDebut;
            } else {
                a.DateFin = affectationMoyen.DateFin;
            }

            a.Commentaire = affectationMoyen.Commentaire;
            a.AffectedTo = element.Libelle;
            a.AffectationMoyenTypeId = element.AffectationMoyenTypeId;
            a.TypeAffectation = element.TypeAffectation;
            a.Site = affectationMoyen.Site;
            a.SiteId = affectationMoyen.Site ? affectationMoyen.Site.SiteId : null;
            a.TypeAffectation = affectationMoyen.TypeAffectation;
            a.IsActive = !(element.AffectationMoyenTypeId === MoyenService.affectationTypeIdRetourAuLoueur);
        }

        /*
         * Recopie d'un objet vers un autre 
        */
        function cloneAll(source, target) {
            for (var property in source) {
                if (source.hasOwnProperty(property)) {
                    target[property] = angular.copy(source[property]);
                }
            }
        }

        /*
         * Hanlde lookup url
        */
        function handleGetLookupUrl(val) {
            var baseControllerUrl = "";

            switch (val) {
                case "Site":
                    baseControllerUrl = '/api/Site/SearchLight/';
                    break;
            }

            return baseControllerUrl;
        }

        //Fonction de chargement des données de l'item sélectionné dans la picklist
        function handleLookupSelection(type, item) {
            if (item) {
                switch (type) {
                    case "Site":
                        $ctrl.affectationModel.Site = item;
                        break;

                }
            }

            actionValidateForm();
        }

        //Fonction pour la supression du site de la lookup
        function handleRemoveSiteFilter() {
            $ctrl.affectationModel.Site = null;
            actionValidateForm();
        }

        //Fonction pour le check de la date de validation
        function handleDateValidation() {
            if (!$ctrl.affectationModel.DateDebut) {
                $ctrl.dateDebutNull = true;
            }
            
            if ($ctrl.isMaintenance && $ctrl.affectationModel.DateFin) {
                $ctrl.dateFinInvalid = MoyenService.IsDateFinInvalid($ctrl.affectationModel.DateDebut, $ctrl.affectationModel.DateFin);
            }

            if ($ctrl.isCheckDateDebutActif && $ctrl.affectationModel.DateDebut) {
                $ctrl.dateDebuInvalid = $ctrl.existingStartDate > new Date($ctrl.affectationModel.DateDebut)
                    && !MoyenService.comparingTwoDates($ctrl.existingStartDate, $ctrl.affectationModel.DateDebut);
                $ctrl.dateDebutNull = false;
            }

            if ($ctrl.isCheckErrorExistingStartDates && $ctrl.affectationModel.DateDebut) {
                $ctrl.isErrorExistingStartDate = $ctrl.selectedMoyenList && $ctrl.selectedMoyenList.some(function (s) {
                    let sDtCondition = actionGetStartDateCondition(new Date(s.DateDebut), s.AffectationMoyenTypeId);
                    let isCheck = sDtCondition && s.AffectationMoyenTypeId !== $ctrl.affectationTypeNotAffected;
                    $ctrl.dateDebutNull = false;
                    return isCheck;
                });
            }

            actionValidateForm();
        }

        /*
         * @function Action get start date condition
         * @description get start date condition
         */
        function actionGetStartDateCondition(existingStartDate, typeId) {
            let isRestitutionOrMaintenance = MoyenService.isToUpdateOnRestitution(typeId);
            let startDateCondition = isRestitutionOrMaintenance
                ? existingStartDate > new Date($ctrl.affectationModel.DateDebut)
                : existingStartDate > new Date($ctrl.affectationModel.DateDebut) && !MoyenService.comparingTwoDates(existingStartDate, $ctrl.affectationModel.DateDebut);

            return startDateCondition;
        }

        /*
         * @description Action pour vérifier la validité des données du formulaire d'affectation
         */
        function actionValidateForm(isInit) {
            actionValidateSiteSelection();
            if (isInit) {
                handleDateValidation();
            } else {
                let isAffectationDatesOnWeekend = actionCheckAffectationDatesOnWorkingDays();
                $ctrl.isFormDataInValid =
                    $ctrl.dateFinInvalid ||
                    $ctrl.dateDebuInvalid ||
                    $ctrl.isErrorExistingStartDate ||
                    $ctrl.isSiteSelectionError ||
                    $ctrl.dateDebutNull ||
                    isAffectationDatesOnWeekend;
            }

            $scope.$apply();
        }

        /*
         * @description Méthode pour vérifier que les dates de début et de fin des affectations sont des dates de jours ouvrés
         */
        function actionCheckAffectationDatesOnWorkingDays() {
            $ctrl.isDateDebutWeekend = MoyenService.isWeekend($ctrl.affectationModel.DateDebut);
            $ctrl.isDateFinWeekend = MoyenService.isWeekend($ctrl.affectationModel.DateFin);

            // Pour rendre le controle non bloquant : return false;
            return $ctrl.isDateDebutWeekend || $ctrl.isDateFinWeekend;
        }

        /*
         * @description Action de validation de la séléction du site
         */
        function actionValidateSiteSelection() {
            let isSiteUnDefined = $ctrl.affectationModel.Site === null;
            $ctrl.isSiteSelectionError = !$ctrl.isMaintenance && !$ctrl.isSiteSelectionDisabled && isSiteUnDefined;
        }

        /*
         * @description Action pour avoir le status du site 
         */
        function actionGetSiteSelectionStatus() {
            return $ctrl.AffecationTypeList
                && $ctrl.AffecationTypeList.length > 0
                && $ctrl.AffecationTypeList[0].AffectationMoyenTypeId === MoyenService.affectationTypeIdRetourAuLoueur
                && $ctrl.AffecationTypeList[0].Selected;
        }

        /*
         * @description Action pour initialiser la liste des affectations types 
         */
        function actionInitialiseAffectationTypeList(obj) {
            if (obj.affectationMoyenFamilleByTypeMoyenList
                && obj.affectationMoyenFamilleByTypeMoyenList.length > 0
                && obj.affectationMoyenFamilleByTypeMoyenList[0].AffecationTypeList
                && obj.affectationMoyenFamilleByTypeMoyenList[0].AffecationTypeList.length > 0) {

                $ctrl.AffecationTypeList = obj.affectationMoyenFamilleByTypeMoyenList[0].AffecationTypeList.filter(function (e) {
                    return e.AffectationMoyenTypeId !== $ctrl.affectationTypeNotAffected;
                });


                if ($ctrl.AffecationTypeList && $ctrl.AffecationTypeList.length > 0) {
                    angular.forEach($ctrl.AffecationTypeList, function (a) {
                        a.Selected = false;
                    });

                    $ctrl.AffecationTypeList[0].Selected = true;
                    $ctrl.isSiteSelectionDisabled = $ctrl.AffecationTypeList[0].AffectationMoyenTypeId === MoyenService.affectationTypeIdRetourAuLoueur;
                }
            }
        }

        /*
         * @description Action pour voir si aucun élément n'est séléctionné 
         */
        function actionIsNoItemSelected() {
            return $ctrl.AffecationTypeList
                && $ctrl.AffecationTypeList.length > 0
                && $ctrl.AffecationTypeList.every(function (e) { return !e.Selected; });
        }
    }
})();