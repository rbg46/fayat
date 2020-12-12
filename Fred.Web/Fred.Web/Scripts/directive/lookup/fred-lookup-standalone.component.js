(function () {
    'use strict';
    angular
        .module('Fred')
        .component('lookupStandalone', {
            templateUrl: '/Scripts/directive/lookup/lookup-standalone.template.html',
            transclude: true,
            require: {
                ngModel: '^ngModel'
            },
            bindings: {

                // [SEULEMENT SUR LOOKUP-STANDALONE]         
                //texte affiché       
                text: "<",

                // [SEULEMENT SUR LOOKUP-STANDALONE]
                //tooltip sur le bouton d'ouverture
                tooltip: "<",

                // [URL OU ON-CLICK OBLIGATOIRE (SOIT L UN SOIT L AUTRE)] 
                // Action executer au click sur l'element.
                // Doit retourner l'url de l'api sur laquelle la recherche doit etre faite.
                // Exemple :/api/CI/SearchLight/
                // Attention, seule la valeur 'CI' dans l'exemple ci-dessus a de l'importance.
                // L'url SearchLight sera automatiquement appelée.
                // Si l'attribut n'est pas déclaré alors l'url appelé est defini par l'attribut 'url'.
                onClick: '&',

                // Action executée lors de la selection d'un element dans la barre de recherche
                // Exemple de déclaration => onSelect: onSelectCi(item)
                // ici 'item' contient l'élement selectionné.
                onSelect: '&',

                // [SEULEMENT SUR LOOKUP-STANDALONE] 
                // Action executé lors du click sur le bouton supprimé
                onDelete: '&',

                //Active ou desactive le lookup.
                ngDisabled: '=ngDisabled',

                // Ne pas renseigné MAIS il faut fournir un ngModel a la directive.
                pickListModel: '=ngModel',

                //texte affiché comme titre dans le panneaux de recherche.
                searchTitle: "@?",

                //texte affiché comme Placeholder dans le panneaux de recherche.
                searchPlaceholder: "@?",

                //texte affiché comme Placeholder 2 dans le panneaux de recherche.
                searchPlaceholder2: "@?",

                //texte affiché comme Placeholder 3 dans le panneaux de recherche.
                searchPlaceholder3: "@?",

                // Tooltip du champ de recherche
                searchTooltip: "@?",

                // [OPTIONNEL VALEUR PAR DEFAULT FALSE] 
                //Attribut qui est obligatoire a true si on affiche le lookup dans une popup        
                showInPopup: '<',

                styleParent: "<",

                // texte du tooltip affiché lorsque la picklist est désactivée
                disabledTooltip: "@?",

                // [OPTIONNEL VALEUR PAR DEFAULT FALSE] 
                //Permet de creer un item dans le lookup si aucune donnée n'est trouvéz sur le serveur
                //L'item créer contiendra 2 champs :
                // - CodeRef = sera egal au champ de recherche 
                // - LibelleRef = sera egal à : "Valeur inexistante. Voulez-vous saisir " + $ctrl.searchText + "?"
                //valeur par defaut = false
                enableDefaultSelection: '=?',

                // [URL OU ON-CLICK OBLIGATOIRE (SOIT L UN SOIT L AUTRE)] 
                //url du serveur. cette valeur n'est pas pris en compte si l'attribut onClick est defini.
                url: '@',

                // [SEULEMENT SUR LOOKUP-STANDALONE] 
                // [OPTIONNEL VALEUR PAR DEFAULT FALSE] 
                // Affiche le bouton de suppression seulement quand on survole le lookup qui a une valeur.
                showDeleteButtonOnMouseOver: '<',

                // [SEULEMENT SUR LOOKUP-STANDALONE] 
                // [OPTIONNEL VALEUR PAR DEFAULT FALSE] 
                // Empeche la génération du composant de suppression et d'affichage de la valeur selectionné.
                buttonOnly: '<',

                // [SEULEMENT SUR LOOKUP-STANDALONE] 
                // [OPTIONNEL VALEUR PAR DEFAULT 'glyphicon glyphicon-search'] 
                // Permet de surcharger la classe du span qui affiche l'icone de recherche. 
                searchIconClass: '@',

                // [SEULEMENT SUR LOOKUP-STANDALONE] 
                // [OPTIONNEL VALEUR PAR DEFAULT FALSE] 
                // Afficher l'icone de recherche seulement quand aucun element n'a été selectionné.
                showSearchIconOnlyWhenEmpty: '<',

                // [SEULEMENT SUR LOOKUP-STANDALONE] 
                // [OPTIONNEL VALEUR PAR DEFAULT TRUE] 
                // Afficher le boutton de suppression.
                showDeleteButton: '<',


                //Met une longeur maximale pour le champ de recherche de la barre de recherche.
                searchMaxLength: '@?',

                // Permet de specifier si il y a une action de confirmation
                // valeur par defaut = false
                hasConfirm: '=',

                // Action de confirmation
                // Cela peux etre une action qui retourne une promise ou une action qui retourne true ou false
                onConfirm: '&',

                // URL Template custom pour chaque élément de la liste d'une lookup
                // Permet de personnaliser les éléments de la lookup
                // ex: custom-template-url="~/Scripts/directive/Lookup/custom-templates/tache.template.html"
                // ATTENTION: ne pas mettre le '~' lorsque la lookup est dans le html d'un component angularjs
                customTemplateUrl: '@?',
                
                // URL Template custom pour LE HEADER ( en dessous des champs de recherche)
                customHeaderTemplateUrl: '@?',

                // URL context custom pour chaque LE HEADER ( en dessous des champs de recherche)
                // permet par exemple de rajouter des filtre de recherche
                customHeaderContext: '=',

                // Placeholder de la lookup
                placeholder: '@?',

                //Ca permet de passer un tableau d'objets (Id, Libelle) pour spécifier un lookup avec différents onglets
                filters: '=?',

                // Active ou non un deuxième champ de recherche
                activeSecondSearchInput: '=?',

                //Afficher l'icon de sélection du personnel lors de l'ajout d'une surcharge ou d'une exception dont la ressource est typée 'personnel'
                addPersonelIcon: '@',

                // Active ou non un troixieme champ de recherche
                activeThirdSearchInput: '=?',

                // Active ou non pour les picklist spécifiques ressources recommandées
                activeCheckboxRessourcesRecomandee: '=?',

                // Définit l'id de la lookup afin d'incrémenter les id internes à la lookup exemple {{'cbRessourcesRecommandees-' + $ctrl.id}} 
                id: '@?',

                // Définit si oui ou non on rafraichit le model dans la directive lookup
                noRefreshModelInLookup: '=?'
            },
            controller: function () {

                var $ctrl = this;
                $ctrl.deleteButtonIsVisible = true;
                $ctrl.handleDelete = handleDelete;
                $ctrl.getTooltipText = getTooltipText;
                $ctrl.onMouseOver = onMouseOver;
                $ctrl.onMouseLeave = onMouseLeave;
                $ctrl.showSearchIcon = showSearchIcon;
                $ctrl.handleIsModelEmpty = handleIsModelEmpty;
                $ctrl.focused = false;

                // TODO : à revoir
                $ctrl.icon = $ctrl.searchIconClass && $ctrl.searchIconClass === "material-icons" ? "add" : "chevron_right";

                //////////////////////////////////////////////////////////////////
                // Init                                                         //
                //////////////////////////////////////////////////////////////////

                $ctrl.$onInit = function () {

                    if (angular.isUndefined($ctrl.styleParent)) {
                        $ctrl.styleParent = true;
                    }

                    if (!$ctrl.showDeleteButtonOnMouseOver) {
                        $ctrl.deleteButtonIsVisible = true;
                    } else {
                        $ctrl.deleteButtonIsVisible = false;
                    }

                    if ($ctrl.searchIconClass === undefined) {
                        $ctrl.searchIconClass = 'material-icons';
                    }
                };

                function getTooltipText() {
                    return ($ctrl.ngDisabled && $ctrl.disabledTooltip)
                        ? $ctrl.disabledTooltip
                        : $ctrl.tooltip
                            ? $ctrl.tooltip
                            : $ctrl.text;
                }

                //////////////////////////////////////////////////////////////////
                // Handlers                                                     //
                //////////////////////////////////////////////////////////////////

                function showSearchIcon() {
                    if ($ctrl.showSearchIconOnlyWhenEmpty === true) {
                        if (!$ctrl.pickListModel) {
                            return true;
                        } else {
                            return false;
                        }
                    } else {
                        return true;
                    }
                }

                function handleIsModelEmpty() {
                    return Object.keys($ctrl.pickListModel).length === 0;
                }

                function handleDelete($event) {
                    $ctrl.onDelete();
                    $event.stopPropagation();
                }

                function onMouseOver() {
                    if ($ctrl.showDeleteButtonOnMouseOver) {
                        $ctrl.deleteButtonIsVisible = true;
                    }
                }

                function onMouseLeave() {
                    if ($ctrl.showDeleteButtonOnMouseOver) {
                        $ctrl.deleteButtonIsVisible = false;
                    }
                }
            }
        });
})();