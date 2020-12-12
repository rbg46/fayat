
(function () {
    'use strict';

    angular.module('Fred').service('CommandeLigneIsReceptionnableService', CommandeLigneIsReceptionnableService);

    CommandeLigneIsReceptionnableService.$inject = [
        'GroupeFeatureService',
        'CommandeLigneCommonService'
    ];

    function CommandeLigneIsReceptionnableService(GroupeFeatureService,
        CommandeLigneCommonService) {

        this.isReceptionnable = function (commandeLigne) {

            if (CommandeLigneCommonService.isClassicCommandeLigne(commandeLigne)) {
                return true;// c'est une ligne de commande normale
            }

            if (CommandeLigneCommonService.isAvenantLigneNonValide(commandeLigne)) {
                return false;//c'est une ligne d'Avenant qui n'est pas valide'
            }

            if (GroupeFeatureService.getFeaturesForGroupe().IsPossibleToCreateReceptionWithQuantityNegative) {
                return true;//le groupe authorize les lignes d'avenant negtives a etre receptionnables
            } else {
                //seule les lignes d'avenant positives sont receptionnables
                return !commandeLigne.AvenantLigne.IsDiminution;
            }
        };
    }
})();
