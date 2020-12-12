(function () {
    'use strict';

    angular.module('Fred').service('PersonnelPickerService', PersonnelPickerService);

    PersonnelPickerService.$inject = ['$log'];

    function PersonnelPickerService($log) {

        var personnelSelected = null;

        var service = {
            setPersonnel: setPersonnel,
            getPersonnel: getPersonnel,
            getIsIac: getIsIac,
            IsEtamIac: IsEtamIac,
            IsOuvrier: IsOuvrier,
            IsCadre: IsCadre,
            IsETAM: IsETAM
        };

        return service;

        //////////////////////////////////////////////////////////////////
        // PUBLICS METHODES                                             //
        //////////////////////////////////////////////////////////////////

        function setPersonnel(personnel) {
            personnelSelected = personnel;
        }


        function getPersonnel() {
            return personnelSelected;
        }

        function getIsIac() {
            return getPersonnel() && getPersonnel().Statut === "3";
        }

        function IsEtamIac() {
            return getPersonnel() && getPersonnel().Statut !== undefined && getPersonnel().Statut.length > 0 && getPersonnel().Statut !== "1";
        }

        function IsOuvrier() {
            return getPersonnel() && getPersonnel().Statut !== undefined && getPersonnel().Statut.length > 0 && getPersonnel().Statut === "1";
        }

        function IsCadre() {
            return getPersonnel() && getPersonnel().Statut !== undefined && getPersonnel().Statut.length > 0 && getPersonnel().Statut === "3";
        }

        function IsETAM() {
            return getPersonnel() && getPersonnel().Statut !== undefined && getPersonnel().Statut.length > 0 && getPersonnel().Statut === "2";
        }

    }
})();