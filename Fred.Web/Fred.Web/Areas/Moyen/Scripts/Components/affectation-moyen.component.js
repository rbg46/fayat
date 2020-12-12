(function () {
    'use strict';

    var affectationMoyenComponent = {
        templateUrl: '/Areas/Moyen/Scripts/Components/affectation-moyen.component.html',
        bindings: {
            resources: '<'
        },
        controller: AffectationMoyenController,
        controllerAs: '$ctrl'
    };

    angular.module('Fred').component('affectationMoyenComponent', affectationMoyenComponent);

    angular.module('Fred').controller('AffectationMoyenController', AffectationMoyenController);

    AffectationMoyenController.$inject = ['$scope', 'MoyenService', 'confirmDialog'];

    function AffectationMoyenController($scope, MoyenService, confirmDialog) {
        var $ctrl = this;
        $ctrl.resources = resources;

        // méthodes exposées
        angular.extend($ctrl, {
            handleLookupSelection: handleLookupSelection,
            handleToggleCiPersonnelSelection: handleToggleCiPersonnelSelection,
            handleLookupDeletion: handleLookupDeletion,
            handleDateValidation: handleDateValidation,
            handleFormValidation: handleFormValidation,
            handleClearDateFin: handleClearDateFin
        });

        // Intialisation du component
        // ___________________________________________

        $scope.$on('event.init.affectation.mode', function (event, obj) {
            $ctrl.selectedMoyenList = obj.selectedMoyenList;
            init();
        });
        init();

        // ___________________________________________

        /*
         * @function init
         * @description Initialise le component
         */
        function init() {
            $ctrl.isPersonnelSelection = false;
            $ctrl.defaultDate = new Date();
            $ctrl.affectationModel = actionGetFormModel();
            $ctrl.affectationTypePersonnel = 2;
            $ctrl.affectationTypeCi = 3;
            $ctrl.affectationTypeNotAffected = 1;

            if (!$ctrl.affectationModel.Personnel) {
                $ctrl.isCiSelection = true;
                $ctrl.isPersonnelSelection = false;
            } else {
                $ctrl.isPersonnelSelection = true;
                $ctrl.isCiSelection = false;
            }

            actionValidateForm(true);
        }

        /*
         * @function handleLookupSelection
         * @description Hanlde lookup selection
         */
        function handleLookupSelection(type, item) {
            if (item) {
                switch (type) {
                    case "CI":
                        $ctrl.affectationModel.Ci = item;
                        if (item.ResponsableAdministratif) {
                            $ctrl.affectationModel.Conducteur = item.ResponsableAdministratif;
                        }

                        break;
                    case "Personnel":
                        $ctrl.affectationModel.Personnel = item;
                        if (item) {
                            $ctrl.affectationModel.Conducteur = item;
                        }
                        break;
                    case "Conducteur":
                        $ctrl.affectationModel.Conducteur = item;
                        break;
                }
            }

            actionValidateForm();
        }

        /*
         * @function handle toggle ci personnel selection
        */
        function handleToggleCiPersonnelSelection(isCi) {
            if (isCi) {
                $ctrl.isPersonnelSelection = !$ctrl.isCiSelection;
                $ctrl.affectationModel.Personnel = null;
                $ctrl.affectationModel.PersonnelId = null;
            }
            else {
                $ctrl.isCiSelection = !$ctrl.isPersonnelSelection;
                $ctrl.affectationModel.Ci = null;
                $ctrl.affectationModel.CiId = null;
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
                    case 'CI':
                        $ctrl.affectationModel.Ci = null;
                        $ctrl.affectationModel.Conducteur = null;
                        break;
                    case 'Personnel':
                        $ctrl.affectationModel.Personnel = null;
                        $ctrl.affectationModel.Conducteur = null;
                        break;
                    case 'Conducteur':
                        $ctrl.affectationModel.Conducteur = null;
                        break;
                }
            }

            actionValidateForm();
        }

        /*
         * @description Fonction de vérification de la cohérence des dates saisies (début avant fin)
         * @ Si le type d'entrée est une restitution ou maintenance on exige une date superieure strictement .
         */
        function handleDateValidation() {
            if (!$ctrl.affectationModel.DateDebut) {
                $ctrl.isDateDebut = true;
            }
            else {
                $ctrl.isDateDebut = false;
            }
            if (!$ctrl.affectationModel.DateFin)
            {
                $ctrl.affectationModel.DateFin = null;
                $ctrl.dateFinInvalid = false;
            }

            if ($ctrl.affectationModel.DateFin && $ctrl.affectationModel.DateDebut) {
                $ctrl.dateFinInvalid = MoyenService.IsDateFinInvalid($ctrl.affectationModel.DateDebut, $ctrl.affectationModel.DateFin);
            }

            if ($ctrl.isCheckDateDebutActif) {
                let startDateCondition = actionGetStartDateCondition($ctrl.existingStartDate, $ctrl.affectationModel.AffectationMoyenTypeId);
                $ctrl.dateDebuInvalid = startDateCondition && $ctrl.affectationModel.AffectationMoyenTypeId !== $ctrl.affectationTypeNotAffected;
               
            }

            if ($ctrl.isCheckErrorExistingStartDates && $ctrl.affectationModel.DateDebut) {
                $ctrl.isErrorExistingStartDate = $ctrl.selectedMoyenList && $ctrl.selectedMoyenList.some(function (s) {
                    let sDtCondition = actionGetStartDateCondition(new Date(s.DateDebut), s.AffectationMoyenTypeId);
                    let isCheck = sDtCondition && s.AffectationMoyenTypeId !== $ctrl.affectationTypeNotAffected;
                    return isCheck;
                });
            }

            actionValidateForm();
        }

        /*
         * @description Action pour vérifier la validité des données du formulaire d'affectation
         */
        function actionValidateForm(isOnInit) {
            if (isOnInit) {
                handleDateValidation();
            }
            else {
                let invalidCi = $ctrl.isCiSelection && !$ctrl.affectationModel.Ci;
                let invalidPersonnel = $ctrl.isPersonnelSelection && !$ctrl.affectationModel.Personnel;

                let isAffectationDatesOnWeekend = actionCheckAffectationDatesOnWorkingDays();
                $ctrl.isFormDataInValid = invalidCi
                    || invalidPersonnel
                    || $ctrl.dateFinInvalid
                    || $ctrl.dateDebuInvalid
                    || $ctrl.isErrorExistingStartDate
                    || isAffectationDatesOnWeekend
                    || $ctrl.isDateDebut;
            }
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
         * @description Handle form validation
         */
        function handleFormValidation() {
            let itemsToReaffect = $ctrl.selectedMoyenList.filter(function (s) {
                return s.AffectationMoyenTypeId !== $ctrl.affectationTypeNotAffected;
            });
            let isStartDateWarning = itemsToReaffect && itemsToReaffect.some(function (s) {
                return comparingTwoDates($ctrl.affectationModel.DateDebut, s.DateDebut);
            });

            if (isStartDateWarning) {
                confirmDialog.confirm($ctrl.resources, $ctrl.resources.Moyen_Warning_Modification_Date)
                    .then(function () {
                        actionPerformValidation(itemsToReaffect);
                    });
            }
            else {
                actionPerformValidation(itemsToReaffect);
            }
        }

        /*
         * @description action performed after confirmation
         */
        function actionPerformValidation(itemsToReaffect) {
            let affectationMoyenList = [];
            let itemsToAffect = $ctrl.selectedMoyenList.filter(function (s) {
                return s.AffectationMoyenTypeId === $ctrl.affectationTypeNotAffected;
            });

            actionUpdateAffectationMoyenList(itemsToAffect, affectationMoyenList);

            let itemsToUpdate = itemsToReaffect.filter(function (s) {
                return comparingTwoDates($ctrl.affectationModel.DateDebut, s.DateDebut);
            });
            actionUpdateAffectationMoyenList(itemsToUpdate, affectationMoyenList);

            let itemsToClose = $ctrl.selectedMoyenList.filter(function (s) {
                return new Date($ctrl.affectationModel.DateDebut) > new Date(s.DateDebut);
            });

            if (itemsToClose && itemsToClose.length > 0) {
                angular.forEach(itemsToClose, function (v) {
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
                    clonedItem.AffectedTo = actionGetAffectedToLibelle($ctrl.affectationModel.Ci, $ctrl.affectationModel.Personnel, $ctrl.affectationModel.AffectedTo);
                    clonedItem.IsActive = true;
                    clonedItem.IsToAdd = true;
                    clonedItem.MoyenCode = v.MoyenCode;
                    clonedItem.Libelle = v.Libelle;
                    clonedItem.Immatriculation = v.Immatriculation;
                    clonedItem.PersonnelId = clonedItem.Personnel ? clonedItem.Personnel.PersonnelId : null;
                    clonedItem.CiId = clonedItem.Ci ? clonedItem.Ci.CiId : null;
                    clonedItem.ConducteurId = clonedItem.Conducteur ? clonedItem.Conducteur.PersonnelId : null;
                    clonedItem.AffectationMoyenTypeId = $ctrl.affectationModel.Personnel ? $ctrl.affectationTypePersonnel :
                        ($ctrl.affectationModel.Ci ? $ctrl.affectationTypeCi : v.AffectationMoyenTypeId);

                    affectationMoyenList.push(clonedItem);
                    affectationMoyenList.push(v);
                });
            }

            $scope.$emit('event.operation.moyen', { affectationMoyenList: affectationMoyenList });
        }

        /*
         * @description action get form model
         */
        function actionGetFormModel() {
            $ctrl.isCheckDateDebutActif = false;
            $ctrl.isCheckErrorExistingStartDates = true;
            if ($ctrl.selectedMoyenList && $ctrl.selectedMoyenList.length === 1) {
                $ctrl.isCheckDateDebutActif = true;
                $ctrl.isCheckErrorExistingStartDates = false;
                var firstElement = $ctrl.selectedMoyenList[0];
                $ctrl.existingStartDate = new Date(firstElement.DateDebut);
                return {
                    Ci: firstElement.Ci,
                    Personnel: firstElement.Personnel,
                    CiId: firstElement.CiId,
                    PersonnelId: firstElement.PersonnelId,
                    DateDebut: firstElement.AffectationMoyenTypeId === $ctrl.affectationTypeNotAffected ? new Date() : firstElement.DateDebut,
                    DateFin: firstElement.DateFin,
                    Conducteur: firstElement.Conducteur,
                    ConducteurId: firstElement.ConducteurId,
                    Commentaire: firstElement.Commentaire,
                    AffectedTo: actionGetAffectedToLibelle(firstElement.Ci, firstElement.Personnel, firstElement.AffectedTo),
                    AffectationMoyenTypeId: firstElement.AffectationMoyenTypeId,
                    MoyenCode: firstElement.MoyenCode,
                    Site: firstElement.Site,
                    SiteId: firstElement.SiteId,
                    Libelle: firstElement.Libelle,
                    Immatriculation: firstElement.Immatriculation
                };
            }

            return {
                Ci: null,
                Personnel: null,
                CiId: null,
                PersonnelId: null,
                DateDebut: new Date(),
                DateFin: null,
                Conducteur: null,
                Commentaire: null,
                AffectedTo: null,
                AffectationMoyenTypeId: $ctrl.affectationTypeNotAffected,
                Site: null,
                SiteId: null,
                Libelle: null,
                Immatriculation: null
            };
        }

        /*
         * @description action update moyen item
         */
        function actionUpdateMoyenItem(a, affectationMoyen) {
            if (!a || !affectationMoyen) {
                return;
            }

            a.CiId = affectationMoyen.Ci ? affectationMoyen.Ci.CiId : null;
            a.PersonnelId = affectationMoyen.Personnel ? affectationMoyen.Personnel.PersonnelId : null;
            a.Ci = affectationMoyen.Ci;
            a.Personnel = affectationMoyen.Personnel;
            a.ConducteurId = affectationMoyen.Conducteur ? affectationMoyen.Conducteur.PersonnelId : null;
            a.Conducteur = affectationMoyen.Conducteur;
            a.DateDebut = affectationMoyen.DateDebut;
            a.DateFin = affectationMoyen.DateFin;
            a.DateDebut = affectationMoyen.DateDebut;
            a.MoyenCode = affectationMoyen.MoyenCode;
            a.Commentaire = affectationMoyen.Commentaire;
            a.AffectedTo = actionGetAffectedToLibelle(affectationMoyen.Ci, affectationMoyen.Personnel, a.AffectedTo);
            a.AffectationMoyenTypeId = affectationMoyen.Personnel ? $ctrl.affectationTypePersonnel :
                (affectationMoyen.Ci ? $ctrl.affectationTypeCi : a.AffectationMoyenTypeId);
            a.TypeAffectation = affectationMoyen.TypeAffectation;
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
         * @function getAffectedToValue
         * @description Remplissage coté front de la valeur affected to
         */
        function actionGetAffectedToLibelle(ci, personnel, typeAffectationLibelle) {
            if (personnel) {
                return personnel.Nom + " " + personnel.Prenom;
            }
            else if (ci) {
                return ci.Libelle;
            }
            else {
                return typeAffectationLibelle;
            }
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
         * @function comparingTwoDates
         * @description comparisons de deux dates : par jour, mois et année (Deep comparison)
         */
        function comparingTwoDates(dateTime1, dateTime2) {
            if (!dateTime1 || !dateTime2) {
                return false;
            }

            let d1 = new Date(dateTime1);
            let d2 = new Date(dateTime2);

            return d1.getFullYear() === d2.getFullYear()
                && d1.getMonth() === d2.getMonth()
                && d1.getDate() === d2.getDate();
        }

        /*
         * @function hanlde clear date fin
         * @description Handle clear date de fin
         */
        function handleClearDateFin() {
            $ctrl.affectationModel.DateFin = null;
            $ctrl.dateFinInvalid = false;
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
    }
})();