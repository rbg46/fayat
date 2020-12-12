/*
 * Service qui converti un int en type d'organisation (string)
 */
(function () {
  "use strict";


  angular.module('Fred').service('typeOrganisationConverterService', typeOrganisationConverterService);

  typeOrganisationConverterService.$inject = [];


  function typeOrganisationConverterService() {

    return {
      convertIntToTypeOrganisation: function (intValue) {
        var result = 'TYPE NOT DEFINED';
        switch (intValue) {
          case ORGANISATION_TYPE.PUO: {
            result = 'PUO';
            break;
          }
          case ORGANISATION_TYPE.UO: {
            result = 'UO';
            break;
          }
          case ORGANISATION_TYPE.ETABLISSEMENT: {
            result = 'Etablissement';
            break;
          }
          case ORGANISATION_TYPE.CI: {
            result = 'CI';
            break;
          }
          default:
            break;
        }
        return result;
      },

    };
  };


})();