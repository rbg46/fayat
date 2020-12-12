(function () {
    'use strict';

    angular.module('Fred').service('PieceJointeValidatorService', PieceJointeValidatorService);

    PieceJointeValidatorService.$inject = ['Notify'];

    /**
     * Service des Pièces jointes Fred     
     */
    function PieceJointeValidatorService(Notify) {
        var vm = this;

        /**
         * Récupérer la taille d'un fichier
         * @param {any} file
         */
        vm.getFileSize = function (file) {
            return Math.floor(file.size / 1024);
        }

        /**
         * Vérifier si la taille du fichier est valide
         * @param {any} file Fichier à vérifier
         * @param {any} maxFileSize Taille maximale acceptée
         */
        vm.isValidFileSize = function (file, maxFileSize) {
            var fileSize = vm.getFileSize(file) // Conversion en Mo
            var isValid = (fileSize < maxFileSize);
            // Notify
            if (!isValid) {
                Notify.error(resources.PieceJointe_Error_MaxTailleFichier.replace('{0}', file.name));
            }
            return isValid;
        }

        /**
         * Vérifier si le format du fichier est accepté
         * @param {any} file Fichier à vérifier
         * @param {any} acceptedFormats Liste des formats acceptés
         */
        vm.isValidFileFormat = function (file, acceptedFormats) {
            var fileTypes = acceptedFormats.split(',');
            var fileExtension = file.name.match(/\.[0-9a-z]+$/i)[0];
            var isValidFormat = false;
            for (var i = 0; i < fileTypes.length; i++) {
                if (file.type === fileTypes[i].trim() || fileExtension === fileTypes[i].trim()) {
                    isValidFormat = true;
                    break;
                }
            }
            // Notify
            if (!isValidFormat) {
                Notify.error(resources.PieceJointe_Error_FormatFichierInvalide.replace('{0}', file.name));
            }
            return isValidFormat;
        }

        /**
         * Vérifier si une pièce jointe est valide
         * @param {any} attachments Attachement déjà attachés
         * @param {any} fileAdded Nouvelle pièce jointe
         * @param {any} acceptedFormats Liste des formats acceptés
         * @param {any} maxSizeForOneAttachment Taille maximale configurée pour une fichier
         * @param {any} maxSizeForAllAttachments Taille maximale configurée pour tous les fichiers
         * @param {any} nbAttachmentsMax Nombre maximal du nombre de pièces jointes
         */
        vm.isAttachmentValid = function (attachments, fileAdded, acceptedFormats, maxSizeForOneAttachment, maxSizeForAllAttachments, nbAttachmentsMax) {

            // Sommer la taille des fichiers déjà dans les pièces jointes
            let nbFilesAlreadyAttached = 0;
            var currentTotalSize = (attachments || []).reduce(function (acc, attachment) {
                // Compter le nombre de fichiers
                if (!attachment.IsDeleted) {
                    nbFilesAlreadyAttached++;
                    return acc + attachment.SizeInKo;
                }
                return acc + 0;
            }, 0);

            // Vérifier si le nombre de fichiers est valide
            var isValidNbAttachmentsMax = ((nbFilesAlreadyAttached + 1) <= nbAttachmentsMax);

            if (!isValidNbAttachmentsMax) {

                // Notify
                Notify.error(resources.PieceJointe_Error_MaxNbFichiers.replace('{0}', nbAttachmentsMax));
                return false;

            } else {

                // Calculer taille totale
                var fileToAddSize = vm.getFileSize(fileAdded);

                // Check all sizes
                var isValidTotalSize = ((currentTotalSize + fileToAddSize) < maxSizeForAllAttachments);

                if (!isValidTotalSize) {

                    // Notify
                    Notify.error(resources.PieceJointe_Error_MaxTailleTotaleFichiers.replace('{0}', Math.floor(maxSizeForAllAttachments/1000)));
                    return false;

                } else {

                    // Vérifier le format d'un fichier
                    var isValidFileFormat = vm.isValidFileFormat(fileAdded, acceptedFormats);

                    // Ajouter si pas d'erreurs
                    if (!isValidFileFormat) {
                        return false;
                    } else {

                        // Vérifier la taille d'un fichier
                        var isValidFileSize = vm.isValidFileSize(fileAdded, maxSizeForOneAttachment);

                        // File is Valid
                        return isValidFileSize;
                    }
                }
            }
        }
    }
})();

