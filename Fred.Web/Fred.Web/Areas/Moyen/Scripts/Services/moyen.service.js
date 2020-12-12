(function () {
    'use strict';

    angular.module('Fred').service('MoyenService', MoyenService);

    MoyenService.$inject = ['$resource', '$filter'];

    function MoyenService($resource, $filter) {
        var vm = this;
        var uriBase = "/api/AffectationMoyen/";

        // DISP
        vm.DISP = 'DISP';

        // LOC
        vm.LOC = 'LOC';

        // AFFECT
        vm.AFFECT = 'AFFECT';

        // MAINT
        vm.MAINT = 'MAINT';

        vm.affectationTypeNotAffected = 1;

        vm.affectationTypeIdPersonnel = 2;

        vm.affectationTypeIdCi = 3;

        vm.affectationTypeIdParking = 4;

        vm.affectationTypeIdDepot = 5;

        vm.affectationTypeIdStock = 6;

        vm.affectationTypeIdReparation = 7;

        vm.affectationTypeIdEntretien = 8;

        vm.affectationTypeIdControle = 9;

        vm.affectationTypeIdRetourAuLoueur = 10;

        vm.affectationTypeIdResteDisponible = 11;

        vm.EnvoiPointageMoyenResultSuccessCode = 0;

        vm.EnvoiPointageMoyenResultErrorCode = 1;



        // is to update on restitution
        vm.isToUpdateOnRestitution = function (typeId, isRestitutionMode) {
            if (isRestitutionMode === null || isRestitutionMode === undefined) {
                return typeId
                    &&
                    [vm.affectationTypeIdParking,
                    vm.affectationTypeIdDepot,
                    vm.affectationTypeIdStock,
                    vm.affectationTypeIdRetourAuLoueur,
                    vm.affectationTypeIdResteDisponible,
                    vm.affectationTypeIdReparation,
                    vm.affectationTypeIdEntretien,
                    vm.affectationTypeIdControle]
                        .some(function (r) {
                            return r === typeId;
                        });
            }

            if (isRestitutionMode) {
                return vm.isRestitutionType(typeId);
            } else {
                return vm.isMaintenanceType(typeId);
            }
        };

        // Is restitution type id
        vm.isRestitutionType = function (typeId) {
            return typeId && [vm.affectationTypeIdParking, vm.affectationTypeIdDepot,
            vm.affectationTypeIdStock, vm.affectationTypeIdRetourAuLoueur,
            vm.affectationTypeIdResteDisponible].some(function (r) { return r === typeId; });
        };

        // Is maintenance type id
        vm.isMaintenanceType = function (typeId) {
            return typeId && [vm.affectationTypeIdReparation,
            vm.affectationTypeIdEntretien,
            vm.affectationTypeIdControle].some(function (r) { return r === typeId; });
        };

        // Get restitution items to update function
        vm.getRestitutionItemsToUpdateFunction = function (isMaintenance) {
            if (!isMaintenance) {
                return function (d) {
                    return d.AffectationMoyenTypeId === vm.affectationTypeNotAffected
                        || vm.isRestitutionType(d.AffectationMoyenTypeId);
                };
            } else {
                return function (d) {
                    return d.AffectationMoyenTypeId === vm.affectationTypeNotAffected
                        || vm.isMaintenanceType(d.AffectationMoyenTypeId);
                };
            }
        };

        // Get maintenance items to update function
        vm.getRestitutionItemsToAddFunction = function (isMaintenance) {
            if (!isMaintenance) {
                return function (d) {
                    return d.AffectationMoyenTypeId !== vm.affectationTypeNotAffected && !vm.isRestitutionType(d.AffectationMoyenTypeId);
                };
            } else {
                return function (d) {
                    return d.AffectationMoyenTypeId !== vm.affectationTypeNotAffected && !vm.isMaintenanceType(d.AffectationMoyenTypeId);
                };
            }
        };

        //________________________________________________________
        // Générer un id unique négatif pour l'assimiler à l'id de l'affectation
        // Négatif pour le différencier des Ids existants .
        // ________________________________________________
        vm.generateUniqueAffectationId = function (moyenId) {
            return new Date().getTime() * -1 - moyenId;
        };

        /*
         * @function comparingTwoDates
         * @description comparisons de deux dates : par jour, mois et année (Deep comparison)
         */
        vm.comparingTwoDates = function (dateTime1, dateTime2) {
            if (!dateTime1 || !dateTime2) {
                return false;
            }

            let d1 = new Date(dateTime1);
            let d2 = new Date(dateTime2);

            return d1.getFullYear() === d2.getFullYear()
                && d1.getMonth() === d2.getMonth()
                && d1.getDate() === d2.getDate();
        };

        /*
         * @function get affectation moyen famille code
         * @description get affectation moyen famille code
         */
        vm.getAffectationMoyenFamilleCode = function (affectationModel) {
            if (affectationModel && affectationModel.TypeAffectation && affectationModel.TypeAffectation.AffectationMoyenFamille) {
                return affectationModel.TypeAffectation.AffectationMoyenFamille.Code;
            }

            return null;
        };

        /*
         * @function  Méthode d'extraction du document du cache
         * @description  Méthode d'extraction du document du cache
         */
        vm.downloadFile = function (id, filename, pdf) {
            var downloadPath = "/api/EtatPaie/ExtractDocument/" + id + "/" + filename + "/" + pdf;
            window.location.href = downloadPath;
        };

        /*
         * @function Appliquer un ordre aux éléments des afféctations
         * @description Appliquer un ordre aux éléments des afféctations
         */
        vm.sortByNumParcAndDateDebut = function (affectationList) {
            if (!affectationList || affectationList.length === 0) {
                return affectationList;
            }

            // utiliser pour arriver à faire le sort avec la date . 
            var affectationListWithDate = affectationList.map(function (item) {
                let d = new Date(item.DateDebut).getTime();
                item.DateAsNumber = d;

                return item;
            });

            return $filter('orderBy')(affectationListWithDate, ['Moyen.Code', 'DateAsNumber'], false);
        };

        /*
         * @function is date de fin invalid
         * @description Check si la date de fin est valid . Control unifié entre affectation et maintenance .
         */
        vm.IsDateFinInvalid = function (sDate, eDate) {
            let startDate = new Date(sDate);
            let endDate = new Date(eDate);
            startDate.setHours(0, 0, 0, 0);
            endDate.setHours(0, 0, 0, 0);
            let isStartDateSuperiorToEndDate = startDate > endDate;

            return isStartDateSuperiorToEndDate;
        };

        /*
        * @function Vérifier si c'est la date d'un weekend
        * @description Vérifier si c'est la date d'un weekend
        */
        vm.isWeekend = function (dateAffectation) {
            if (!dateAffectation)
                return false;

            let dateObj = new Date(dateAffectation);
            let day = dateObj.getDay();
            let isWeekend = day === 6 || day === 0;

            return isWeekend;
        };

        /*
        * @function Retourne le premier jour ouvré avant une date donnée
        * @description Retourne le premier jour ouvré avant une date donnée
        */
        vm.getFirstPreviousWorkingDay = function (newStartDateAffectation, existingStartDate) {
            newStartDateAffectation = new Date(newStartDateAffectation);
            existingStartDate = new Date(existingStartDate);

            do {
                newStartDateAffectation.setDate(newStartDateAffectation.getDate() - 1);
            }
            while (vm.isWeekend(newStartDateAffectation));

            // Il peut y avoir des weekends dans les bornes c'est pourquoi il faut vérifier 
            // qu'on a pas dépassé la date de début existante
            if (new Date(newStartDateAffectation) < new Date(existingStartDate)) {
                newStartDateAffectation = existingStartDate;
            }

            return newStartDateAffectation;
        };

        /*
        * @function Retourne l'url utilisée par la lookup dans le cas du renvoi de l'établissement comptable Id
        * L'établissement comptable Id pose un probléme si il est renvoyé ayant la valeur null . WebApi n'arrive pas 
        * à le convertir en int .
        * Pour que ça soit compris par WebApi dans un cas de nullité ça ne doit pas étre founri
        */
        vm.getLookupBaseControllerUrl = function (lookupId, filters) {
            if (!lookupId || !filters) {
                return '';
            }

            switch (lookupId) {
                case "NumParc":
                    {
                        if (filters.EtablissementComptableId) {
                            return String.format('/api/Moyen/SearchLight/?typeMoyen={0}&sousTypeMoyen={1}&modelMoyen={2}&societe={3}&etablissementId={4}&',
                                filters.TypeMoyen,
                                filters.SousTypeMoyen,
                                filters.ModelMoyen,
                                filters.Societe,
                                filters.EtablissementComptableId);
                        }
                        return String.format('/api/Moyen/SearchLight/?typeMoyen={0}&sousTypeMoyen={1}&modelMoyen={2}&societe={3}&',
                            filters.TypeMoyen,
                            filters.SousTypeMoyen,
                            filters.ModelMoyen,
                            filters.Societe);
                    }
                case "NumImmatriculation":
                    {
                        if (filters.EtablissementComptableId) {
                            return String.format('/api/Moyen/Immatriculation/SearchLight/?typeMoyen={0}&sousTypeMoyen={1}&modelMoyen={2}&societe={3}&etablissementId={4}&numParc={5}&',
                                filters.TypeMoyen,
                                filters.SousTypeMoyen,
                                filters.ModelMoyen,
                                filters.Societe,
                                filters.EtablissementComptableId,
                                filters.NumParc);
                        }

                        return String.format('/api/Moyen/Immatriculation/SearchLight/?typeMoyen={0}&sousTypeMoyen={1}&modelMoyen={2}&societe={3}&numParc={4}&',
                            filters.TypeMoyen,
                            filters.SousTypeMoyen,
                            filters.ModelMoyen,
                            filters.Societe,
                            filters.NumParc);
                    }
            }
        };

        var resource = $resource(uriBase,
            {},
            {
                SearchWithFilters: {
                    method: "POST",
                    url: uriBase + 'SearchWithFilters/:page/:pageSize',
                    isArray: true
                },
                SearchRapportLigneMoyenWithFilters: {
                    method: "POST",
                    url: "/api/RapportMoyen/SearchRapportLigneMoyenWithFilters/",
                    params: {},
                    isArray: true
                },
                GenerateRapportLigneMoyenWithFilters: {
                    method: "POST",
                    url: "/api/RapportMoyen/GenerateRapportLigneMoyenWithFilters/",
                    params: {},
                    isArray: false
                },
                GetAffectationMoyenFamilleByTypeModel: {
                    method: "GET",
                    url: "/api/AffectationMoyen/GetAffectationMoyenFamilleByTypeModel",
                    params: {},
                    isArray: true
                },
                ValidateAffectationMoyen: {
                    method: "POST",
                    url: "/api/AffectationMoyen/ValidateAffectationMoyen",
                    params: {},
                    isArray: false
                },
                CreateMoyenLocation: {
                    method: "POST",
                    url: "/api/AffectationMoyen/CreateMoyenLocation",
                    params: {},
                    isArray: false
                },
                UpdateMoyenPointage: {
                    method: "GET",
                    url: "/api/Moyen/UpdateMoyenPointage/:dateDebut/:dateFin",
                    params: {},
                    isArray: false
                },
                ExportPointageMoyen: {
                    method: "GET",
                    url: "/api/Moyen/ExportPointageMoyen/:dateDebut/:dateFin",
                    params: {},
                    isArray: false
                },
                GetLocation: {
                    method: "GET",
                    url: "/api/MoyenLocation/GetAllActiveLocation",
                    params: {},
                    isArray: true
                },
                UpdateMoyenLocation: {
                    method: "PUT",
                    url: "/api/AffectationMoyen/UpdateMoyenLocation",
                    params: {},
                    isArray:false
                },
                GetAffectation: {
                    method: "GET",
                    url: "/api/AffectationMoyen/GetAffectationOfMaterielLocation/:materielLocationId",
                    params: {},
                    isArray: true
                },
                DeleteMoyenLocation: {
                method: "Delete",
                    url: "/api/AffectationMoyen/DeleteMoyenLocation/:materielLocationId",
                params: {},
                isArray: false
                }
            }
        );

        angular.extend(vm, resource);
    }
})();