(function (angular) {
    'use strict';

    angular.module('Fred').service('OrganisationService', OrganisationService);

    OrganisationService.$inject = ['$http'];

    function OrganisationService($http) {
        var baseUrl = "/api/Organisation/";

        var service = {
            getOrganisationByOrganisationId: getOrganisationByOrganisationId,
            getOrganisationType: getOrganisationType,
            getUserOrganisationListByType: getUserOrganisationListByType,
            getLightOrganisationModelById: getLightOrganisationModelById,

            getRoleListByOrganisationId: getRoleListByOrganisationId,
            getThresholdByOrganisationId: getThresholdByOrganisationId,

            getValidationThresholdByRoleId: getValidationThresholdByRoleId,
            addThresholdOrganisation: addThresholdOrganisation,
            updateThresholdOrganisation: updateThresholdOrganisation,
            deleteThresholdOrganisation: deleteThresholdOrganisation,

            thresholdValidator: thresholdValidator
        };

        service.OrganisationType = {
            Holding: 1,
            Pole: 2,
            Groupe: 3,
            Societe: 4,
            Puo: 5,
            Uo: 6,
            Etablissement: 7,
            Ci: 8,
            SousCi: 9
        };

        return service;

        function getOrganisationByOrganisationId(organisationId) {
            return $http.get(baseUrl + organisationId);
        }

        function getOrganisationType() {
            return $http.get(baseUrl + "GetTypesOrganisation");
        }

        function getUserOrganisationListByType(OrgaTypeId) {
            return $http.get("/api/Utilisateur/ListOrganisationsByType/" + OrgaTypeId);
        }

        function getLightOrganisationModelById(idOrganisation) {
            return $http.get("/api/Organisation/SearchLightOrganisationById/" + idOrganisation);
        }

        function getRoleListByOrganisationId(organisationId) {
            return $http.get("/api/Role/GetRoleListByOrganisationId/" + organisationId);
        }

        function getValidationThresholdByRoleId(roleId) {
            return $http.get("/api/Role/SeuilValidationListByRoleId/" + roleId);
        }

        function getThresholdByOrganisationId(orgaId, roleId) {
            return $http.get(baseUrl + "/GetThresholdByOrganisationIdAndRoleId/" + orgaId + "/" + roleId);
        }

        function addThresholdOrganisation(threshold) {
            var data = JSON.stringify(threshold);
            return $http.post(baseUrl + "ThresholdOrganisation", data);
        }

        function updateThresholdOrganisation(threshold) {
            var data = JSON.stringify(threshold);
            return $http.put(baseUrl + "ThresholdOrganisation", data);
        }

        function deleteThresholdOrganisation(thresholdId) {
            return $http.delete(baseUrl + "ThresholdOrganisation/" + thresholdId);
        }

        function thresholdValidator() {
            return {
                valid: valid,
                validCurrency: validCurrency,
                validThreshold: validThreshold
            };

            function valid(roleOrgaCurrencyList, formSeuil, threshold, currency, organisation, role) {

                var result = { isValid: false, messages: [], message: '' };

                var currencyValidation = validCurrency(roleOrgaCurrencyList, currency, organisation, role);
                var thresholdValidation = validThreshold(formSeuil, threshold);

                if (currencyValidation.isValid && thresholdValidation.isValid) {
                    result.isValid = true;
                    return result;
                } else {
                    result.isValid = false;
                    var messages = [];

                    for (var i = 0; i < currencyValidation.messages.length; i++) {
                        result.messages.push(currencyValidation.messages[i]);
                    }

                    for (var j = 0; j < thresholdValidation.messages.length; j++) {
                        result.messages.push(thresholdValidation.messages[j]);
                    }

                    for (var k = 0; k < result.messages.length; k++) {
                        result.message += result.messages[k] + '\n';
                    }
                    return result;
                }

            }

            function validCurrency(roleOrgaCurrencyList, currency, organisation, role) {
                var result = { isValid: true, messages: [] };
                if (!currency) {
                    result.isValid = false;
                    result.messages.push(resources.Organisation_Service_Modal_DeviseRequis_Info);
                    return result;
                }

                var alreadyOverloadedCurrency = roleOrgaCurrencyList.filter(function (threshold) {
                    return threshold.DeviseId === currency.DeviseId && threshold.RoleId === role.RoleId && threshold.OrganisationId === organisation.OrganisationId;
                });

                if (alreadyOverloadedCurrency.length > 0) {
                    result.isValid = false;
                    result.messages.push(resources.Organisation_Service_Modal_SeuilDeviseExistant_Info);
                }
                return result;
            }

            function validThreshold(formSeuil, threshold) {
                var result = { isValid: true, messages: [], message: '' };
                if (formSeuil.Seuil.$error.required) {
                    result.isValid = false;
                    result.messages.push(resources.Organisation_Service_Modal_SeuilRequis_Info);
                }
                if (formSeuil.Seuil.$error.min) {
                    result.isValid = false;
                    result.messages.push(resources.Organisation_Service_Modal_SeuilMin_Info);
                }
                if (formSeuil.Seuil.$error.max) {
                    result.isValid = false;
                    result.messages.push(resources.Organisation_Service_Modal_SeuilMax_Info);
                }
                if (formSeuil.Seuil.$error.pattern) {
                    result.isValid = false;
                    result.messages.push(resources.Organisation_Service_Modal_Format_Info);
                }
                if (formSeuil.Seuil.$error.number) {
                    result.isValid = false;
                    result.messages.push(resources.Organisation_Service_Modal_Format_Info);
                }
                for (var k = 0; k < result.messages.length; k++) {
                    result.message += result.messages[k] + '\n';
                }
                return result;
            }
        }
    }

})(angular);