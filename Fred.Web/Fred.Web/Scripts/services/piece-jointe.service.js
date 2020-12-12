(function () {
    'use strict';

    angular.module('Fred').service('PieceJointeService', PieceJointeService);

    PieceJointeService.$inject = ['$http', 'Notify'];

    /**
     * Service des Pièces jointes Fred     
      *@param {any} $http  paramètre http
      *@param {any} Notify notification
     */
    function PieceJointeService($http, Notify) {
        var vm = this;


        /**
         * Supprimer et détacher la pièce jointe et la détacher
         * @param {any} pieceJointeId id pièces jointe
         * @returns {any} succès or fieled
         */
        vm.DeleteAttachment = function (pieceJointeId) {
            return $http.delete(
                `/api/PieceJointe/Delete/${pieceJointeId}`
            );
        };

        /**
         * Ajouter et attacher une pièce jointe à l'entité
         * @param {any} pieceJointeTypeEntie entité pièce jointe
         * @param {any} entiteId id entité
         * @param {any} libelle discription
         * @param {any} file model file
         * @returns {any} liste des picèes jointes
         */
        vm.AddAttachment = function (pieceJointeTypeEntie, entiteId, libelle, file) {

            // Build form
            var form = new FormData();
            form.append('PieceJointeTypeEntite', pieceJointeTypeEntie);
            form.append('EntiteId', entiteId);
            form.append('Libelle', libelle);
            form.append('File', file);

            return $http.post(
                "/api/PieceJointe/Add",
                form,
                {
                    cache: false,
                    headers: { 'Content-Type': undefined }
                }
            );
        };

        /**
         * Télécharger la pièce jointe correspondante à l'ID renseigné
         * @param {any} pieceJointeId L'id de la pièce jointe
         */
        vm.Download = function (pieceJointeId) {

            // Url avec l'ID correspondant
            var url = `/api/PieceJointe/Download/${pieceJointeId}`;

            // Télécharger le fichier
            window.open(url);
        };

        /**
         * Récupérer toutes les pièces jointes attachés à l'entite
         * @param {any} typeEntite Type de l'entité concernée (Commande / Réception)
         * @param {any} entiteId L'ID de l'entité
         * @returns {any} liste des picèes jointes
         */
        vm.GetAttachements = function (typeEntite, entiteId) {
            return $http.get(
                `/api/PieceJointe/GetPiecesJointes/${typeEntite}/${entiteId}`,
                {
                    cache: false
                }
            );
        };

    }
})();

