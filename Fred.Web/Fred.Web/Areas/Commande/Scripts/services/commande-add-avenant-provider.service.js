(function () {
    'use strict';

    angular
        .module('Fred')
        .service('CommandeAddAvenantProviderService', CommandeAddAvenantProviderService);

    CommandeAddAvenantProviderService.$inject = ['authorizationService'];

    function CommandeAddAvenantProviderService(authorizationService) {

        var service = {
            canAddAvenant: canAddAvenant
        };
        return service;

        /*
          * @function canAddAvenant(commande)
          * @description Permet de savoir si on peux rajouter un avenant.
          * @param {commandeValidee} commandeValidee
          * @param {commande} commande
          * @param {prestationCommandeTypeCode} prestationCommandeTypeCode
          */
        function canAddAvenant(commandeValidee, commande, prestationCommandeTypeCode) {


            var hasAuthorizationToManageCommande = authorizationService.getPermission(PERMISSION_KEYS.AffichageMenuCommandeIndex);
            if (!hasAuthorizationToManageCommande) {
                return false;
            }

            var isValidForAddAvenant = commandeIsValidForAddAvenant(commandeValidee, commande);

            if (!isValidForAddAvenant) {
                return false;
            }

            var hasAuthorizationToAddAvenantOnAllTypes = authorizationService.getPermission(PERMISSION_KEYS.AffichageBoutonAvenantAlways);

            if (hasAuthorizationToAddAvenantOnAllTypes) {
                return true;
            } else {

                if (isPresetationCommande(commande, prestationCommandeTypeCode)) {
                    return true;
                }
            }
            return false;
        }


        function commandeIsValidForAddAvenant(commandeValidee, commande) {
            if (commandeValidee && !commande.IsStatutCloturee) {
                return true;
            }
            return false;
        }

        function isPresetationCommande(commande, prestationCommandeTypeCode) {
            if (commande.Type && commande.Type.Code === prestationCommandeTypeCode) {
                return true;
            }
            return false;
        }


    }
})();
