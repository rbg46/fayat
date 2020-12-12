(function () {
    'use strict';

    angular.module('Fred').constant('Enums', {

        /// Enumération sur les types des entités
        EnumTypeEntite: {
            Commande: {
                Label: 'Commande',
                Value: 1
            },
            Reception: {
                Label: 'Réception',
                Value: 2
            }
        },

        /// Enumération sur les types des évènements d'une commande
        EnumTypeCommandeEvent: {
            Creation: {
                Label: 'Création',
                Value: 1
            },
            ValidationCommande: {
                Label: 'Validation commande',
                Value: 2
            },
            ValidationAvenant: {
                Label: 'Validation avenant',
                Value: 3
            },
            Avis: {
                Label: 'Avis',
                Value: 4
            },
            Impression: {
                Label: 'Impression',
                Value: 5
            }
        },
        /// Enumération sur les type avis
        EnumTypeAvis: {
            Accord: {
                Label: 'Accord',
                Value: 1
            },
            Refus: {
                Label: 'Refus',
                Value: 2
            },
            SansAvis: {
                Label: 'Sans avis',
                Value: 3
            }
        },
        EnumPersonnelStatus: {
            Ouvrier: {
                Label: 'Ouvrier',
                Value: 1
            },
            Etam: {
                Label: 'ETAM',
                Value: 2
            },
            Cadre: {
                Label: 'IAC',
                Value: 3
            }
        }
    });

})();

