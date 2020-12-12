(function () {
    'use strict';

    angular.module('Fred').directive('lookup', lookupDirective);

    lookupDirective.$inject = ['$timeout', '$q'];

    function lookupDirective($timeout, $q) {

        var directive = {
            bindToController: true,
            scope: {
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

                //Attribut qui est obligatoire a true si on affiche le lookup dans une popup        
                showInPopup: '<',

                styleParent: "<",

                //Permet de creer un item dans le lookup si aucune donnée n'est trouvéz sur le serveur
                //L'item créer contiendra 2 champs :
                // - CodeRef = sera egal au champ de recherche 
                // - LibelleRef = sera egal à : "Valeur inexistante. Voulez-vous saisir " + $ctrl.searchText + "?"
                //valeur par defaut = false
                enableDefaultSelection: '=?',

                //url du serveur. cette valeur n'est pas pris en compte si l'attribut onClick est defini.
                url: '@',

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

                //Ca permet de passer un tableau d'objets (Id, Libelle) pour spécifier un lookup avec différents onglets
                filters: '=?',

                // Active ou non un deuxième champ de recherche
                activeSecondSearchInput: '=?',

                // Active ou non un troixième champ de recherche
                activeThirdSearchInput: '=?',

                // Active ou non pour les picklist spécifiques ressources recommandées
                activeCheckboxRessourcesRecomandee: '=?',

                // Définit l'id de la lookup afin d'incrémenter les id internes à la lookup exemple {{'cbRessourcesRecommandees-' + $ctrl.id}} 
                id: '@?',

                // Définit si oui ou non on rafraichit le model dans la directive lookup
                noRefreshModelInLookup: '=?'
            },
            controllerAs: '$ctrl',
            link: link,
            require: 'ngModel',
            templateUrl: '/Scripts/directive/lookup/lookup.template.html',
            transclude: true,
            controller: 'LookupDirectiveController'
        };

        return directive;

        function link(scope, element, attrs) {
            var content = element.find('.lookup-content');

            // Ajoute la fonction Show au lookup
            if (element && element.length && element.length > 0 && !element[0].Show) {
                element[0].Show = function () {
                    clickOnLookup();
                };
            }

            //sur le click de l'element contenue dans le lookup, j'ouvre la poupup
            content.bind('click', function (e) {

                // Verifie que je n'ai pas de fonction de confirmation
                var hasConfirmAction = scope.$ctrl.hasConfirm;

                if (hasConfirmAction === true) {
                    //ici j'ai le choix d'un result de type boolean ou promise
                    var onConfirmeResult = scope.$ctrl.onConfirm();
                    if (onConfirmeResult === true) {
                        clickOnLookup();
                        return;
                    }
                    //ici j'ai un retour negatif de l'action de confirmation
                    if (!onConfirmeResult) {
                        return;
                    }

                    //je traite l'action comme une promise.
                    var responsePromise = $q.when(onConfirmeResult);
                    responsePromise
                        .then(function () {
                            clickOnLookup();
                        });
                } else {
                    clickOnLookup();
                }
            });

            function clickOnLookup() {
                if (!attrs.disabled) {
                    //$timout => permet de mettre a jour l'UI,
                    //etant donné que l'abonnement au click est en dehors du systeme d'angular;
                    $timeout(function () {
                        scope.$ctrl.open();

                        $timeout(function () {
                            var selector = '.searchLookupPanelRight:visible';
                            var search = element.find(selector);
                            if (search.length > 0) {
                                search[0].focus();
                                search[0].select();
                            }
                        }, 100);
                    });
                }
            }



            window.onresize = scope.$ctrl.resize;

        }
    }


    (function () {
        'use strict';

        angular
            .module('Fred')
            .controller('LookupDirectiveController', LookupDirectiveController);

        LookupDirectiveController.$inject = ['$scope', '$timeout', '$http', '$document', '$element'];

        function LookupDirectiveController(scope, $timeout, $http, $document, $element) {
            var searchWidth = 330;

            var bodyRef = angular.element($document[0].body);
            var $ctrl = this;
            $ctrl.isOpen = false;
            $ctrl.currentSelection = {};
            $ctrl.selectedIndex = 0;
            $ctrl.filterParam = "statut=";
            $ctrl.RessouresRecommandeeParam = "ressourcesRecommandeesOnly=";
            $ctrl.IsRessourcesRecommandees = true;
            $ctrl.letterLimit = 30;
            activate();

            function activate() {
                $ctrl.scrollName = 'divScroll' + new Date().getTime();
                $ctrl.searchText = '';
                $ctrl.searchText2 = '';
                $ctrl.searchText3 = '';
                $ctrl.scrolling = false;
                $ctrl.resources = resources;
                $ctrl.enableDefaultSelection = angular.isDefined($ctrl.enableDefaultSelection) ? $ctrl.enableDefaultSelection : false;
                $ctrl.activeSecondSearchInput = angular.isDefined($ctrl.activeSecondSearchInput) ? $ctrl.activeSecondSearchInput : false;
                $ctrl.activeThirdSearchInput = angular.isDefined($ctrl.activeThirdSearchInput) ? $ctrl.activeThirdSearchInput : false;
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                  Handlers                                                                      //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // gestion de la sélection d'un élément de la picklist au clavier
            $ctrl.handleKeyDown = function (e) {
                // Up (38)
                if (e.keyCode === 38 && $ctrl.selectedIndex - 1 >= 0) {
                    $ctrl.selectedIndex--;
                }
                // Down (40)
                if (e.keyCode === 40 && $ctrl.selectedIndex + 1 < $ctrl.referentiels.length) {
                    $ctrl.selectedIndex++;
                }
                // Enter (13)
                if (e.keyCode === 13 && $ctrl.referentiels.length > 0) {
                    //on attend que la dernière recherche effctuée soit achevée
                    while ($ctrl.busy) {
                        setTimeout(function () {
                            console.log('waiting for watcher end');
                        }, 200);
                    }
                    //Appel de la fonction de sélection pour l'élément sélectionné dans la liste
                    $ctrl.handleSelection($ctrl.referentiels[$ctrl.selectedIndex]);
                    e.preventDefault();
                }

                if (e.keyCode === 27) {
                    $ctrl.isOpen = false;
                }
            };

            $ctrl.handleChangeSearchText = function () {
                $ctrl.searchTexts = actionGetWords($ctrl.searchText);
                $ctrl.searchTexts2 = actionGetWords($ctrl.searchText2);
                $ctrl.searchTexts3 = actionGetWords($ctrl.searchText3);
                actionLoad(true);
            };

            $ctrl.open = function () {
                if (!$ctrl.isOpen) {
                    $ctrl.isOpen = true;

                    // A l'ouverture de la Lookup, on cache l'overflow du body
                    bodyRef.addClass('ovh');

                    // Set height and bind scroll to load more
                    $timeout(function () {
                        var lookupHeaderHeight = document.getElementById('lookupHeader').scrollHeight;
                        if ($ctrl.filters) {
                            $('.' + $ctrl.scrollName).height(window.innerHeight - lookupHeaderHeight - 65);
                        } else {
                            $('.' + $ctrl.scrollName).height(window.innerHeight - lookupHeaderHeight);
                        }           
                        //on repositionne uniquement si on est dans une popup.
                        if (scope.$ctrl.showInPopup) {
                            var newPosition = getNewPosition();
                            var newSize = getNewSize();
                            moveSearchBar(newSize, newPosition);
                        }
                        FredToolBox.bindScrollEnd('.' + $ctrl.scrollName, actionLoadMore);
                    }, 0, false);

                    $ctrl.selectedIndex = 0;

                    if (angular.isDefined($ctrl.onClick())) {
                        var messageUrl = $ctrl.onClick();
                        $ctrl.url = messageUrl;
                    }

                    actionInitPaging();

                    // Initialisation du filter au premier élément si il est définit
                    if ($ctrl.filters) {
                        $ctrl.selectedFilter = $ctrl.filters[0];
                        updateUrlWithFilter($ctrl.selectedFilter.Id);
                    }

                    if ($ctrl.activeCheckboxRessourcesRecomandee) {
                        var index = $ctrl.url.indexOf($ctrl.RessouresRecommandeeParam);
                        var ressourcesRecommandees = parseInt($ctrl.url.substr(index + $ctrl.RessouresRecommandeeParam.length, index + $ctrl.RessouresRecommandeeParam.length + 1));
                        $ctrl.IsRessourcesRecommandees = ressourcesRecommandees === 1 ? true : false;
                        updateUrlWithRessourceRecommandee();
                    }

                    // Si la barre de recherche contient déjà un mot, on lance une recherche à l'ouverture (ou réouverture) de la picklist          
                    //actionWatchSearchText();
                    actionLoad(true);
                }
            };

            $ctrl.resize =  function(){
                $timeout(function () {
                    var lookupHeader = document.getElementById('lookupHeader');
                    if(lookupHeader){
                        var lookupHeaderHeight = lookupHeader.scrollHeight;
                        if (document.getElementById('listButtonFilterLookup')) {
                            document.getElementById('divScroll').style.height = (window.innerHeight - lookupHeaderHeight - 65) + "px";
                        } else {
                            document.getElementById('divScroll').style.height = (window.innerHeight - lookupHeaderHeight) + "px";
                        }          
                    } 
                    //on repositionne uniquement si on est dans une popup.
                   /* if (scope.$ctrl.showInPopup) {
                        var newPosition = getNewPosition();
                        var newSize = getNewSize();
                        moveSearchBar(newSize, newPosition);
                    }*/
                    //FredToolBox.bindScrollEnd('.' + $ctrl.scrollName, actionLoadMore);
                }, 0, false);
            }

            /*
             * Calcule la nouvelle position pour placer l'element en Haut a gauche de l'ecran.
             */
            function getNewPosition() {
                var leftPos = $element[0].getBoundingClientRect().left;
                var topPos = $element[0].getBoundingClientRect().top;

                //var elementHeight = element.find(".lookup-content").children().height();
                var position = $element.position();
                var left = -leftPos + $(window).width() - searchWidth + position.left;
                var top = -1 * topPos + position.top + "px";
                return { top: top, left: left };
            }

            /*
             * Calcule la nouvelle taille de la barre de recherche.
             */
            function getNewSize() {
                var windowHeight = $(window).height();
                return {
                    width: searchWidth + "px",
                    height: windowHeight + "px"
                };
            }

            /*
             * Deplace la barre de recherche en Haut a gauche de l'ecran. 
             */
            function moveSearchBar(newSize, newPosition) {
                $element.find(".lookupPane").css({
                    'width': newSize.width,
                    'height': newSize.height,
                    'position': "absolute",
                    'top': newPosition.top,
                    'left': newPosition.left
                });
            }

            //Fonction éxécutée lors du click sur un élément de la picklist
            $ctrl.handleSelection = function (ref) {
                if (!$ctrl.noRefreshModelInLookup) {
                    $ctrl.pickListModel = ref;
                }

                //Ici , c'est tres important de passé par un $timeout, dans l'optique de la rétrocompatibilite avec la picklist.
                //Il faut forcer le changement de valeur du pickListModel avant l'envoi de l'evenement.
                //Ce qui n'est pas le cas ici, meme si la valeur est mis dans le model ($ctrl.pickListModel = ref;)
                //le $timeout force le mecanisme d'angular de mis a jour.
                $timeout(function () {
                    //envoie de l'evenement de changement de valeur
                    $ctrl.onSelect({ item: ref });
                    //Fermeture de la picklist
                    $ctrl.handleCancel();
                });
            };

            //Fonction de fermeture de la picklist
            $ctrl.handleCancel = function () {
                $ctrl.isOpen = false;
                // A la fermeture de la Lookup, on remet l'overflow du body
                bodyRef.removeClass('ovh');
            };

            //Fonction éxécutée lors du click sur un onglet en cas d'utilisation des filtres pour les données de la lookup
            $ctrl.handleSelectFilter = function (filter) {
                $ctrl.selectedFilter = filter;
                updateUrlWithFilter(filter.Id);
                actionLoad(true);
            };

            //Fonction éxécutée lors du click sur un onglet en cas d'utilisation des filtres pour les données de la lookup
            $ctrl.handleSelectRessourcesRecommandees = function () {
                updateUrlWithRessourceRecommandee();
                actionLoad(true);
            };

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                  Actions                                                                       //
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            /*
             * fonction dinitialisation de pagination
             */
            function actionInitPaging() {
                $ctrl.paging = {
                    pageSize: 20,
                    currentPage: 1,
                    societeId: actionGetParam('societeId'),
                    ciId: actionGetParam('ciId'),
                    groupeId: actionGetParam('groupeId'),
                    agenceId: actionGetParam('agenceId'),
                    organisationId: actionGetParam('organisationId')
                };
            }

            /*
             * Fonction de recuperation des parametres passé dans la variable $ctrl.url
             */
            function actionGetParam(name) {
                var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec($ctrl.url);
                if (results === null) {
                    return null;
                }
                else {
                    return results[1] || 0;
                }
            }

            /*
             * Fonction éxécuter quand la scrollbar de la pick list arrive à la fin du control
             */
            function actionLoadMore() {
                if (!$ctrl.busy) {
                    $ctrl.paging.currentPage++;
                    actionLoad(false, true);
                }
            }

            /*
             *  Action chargement données 
             */
            function actionLoad(firstLoad, scrolling) {
                //si le panneau de recherche n'est pas ouvert je ne lance pas de recherche
                //evite les bugs car la fonction du watch peu etre execute a l'init 
                if (!$ctrl.isOpen) { return; }

                $ctrl.searchText = !angular.isDefined($ctrl.searchText) ? null : $ctrl.searchText;
                $ctrl.searchText2 = !angular.isDefined($ctrl.searchText2) ? null : $ctrl.searchText2;
                $ctrl.searchText3 = !angular.isDefined($ctrl.searchText3) ? null : $ctrl.searchText3;
                $ctrl.busy = true;
                $ctrl.scrolling = scrolling === true;

                if (firstLoad) {
                    $ctrl.error = '';
                    $ctrl.referentiels = [];
                    $ctrl.paging.currentPage = 1;
                }

                $http.get(actionCreateSearchLightUrl())
                    .success(actionOnSuccess)
                    .error(actionOnError)
                    .finally(actionOnFinally);
            }

            /*
             * Fonction executée lors de la reception des donnees du serveur.
             */
            function actionOnSuccess(data) {

                angular.forEach(data, function (item) { $ctrl.referentiels.push(item); });

                $ctrl.message = '';

                if ((data === null || data.length === 0) && $ctrl.referentiels.length === 0) {

                    if ($ctrl.enableDefaultSelection && $ctrl.searchText !== "" && data) {
                        $ctrl.referentiels.push({ CodeRef: $ctrl.searchText, LibelleRef: "Valeur inexistante. Voulez-vous saisir " + $ctrl.searchText + "?" });
                    }
                    else {
                        $ctrl.message = 'Aucun référentiel trouvé.';
                    }
                }
            }

            /*
             * Fonction executée lors d'une erreur lors de la reception des donnees du serveur.
             */
            function actionOnError() {
                $ctrl.error = resources.Global_Notification_Error;
                var ciid = actionGetParam('ciId');
                if (ciid === 0) {
                    $ctrl.error = resources.Global_ReferentielCI_SelectCI;
                }
            }

            /*
             * Action finale http   
             */
            function actionOnFinally() {
                $ctrl.busy = false;
                $ctrl.scrolling = false;
            }

            /*
             * Fonction executée pour actualiser la valeur de l'url en considérant le filtre choisi.
             */
            function updateUrlWithFilter(newFilter) {
                if ($ctrl.url) {
                    var index = $ctrl.url.indexOf($ctrl.filterParam);
                    $ctrl.url = $ctrl.url.substr(0, index + $ctrl.filterParam.length) + newFilter + $ctrl.url.substr(index + $ctrl.filterParam.length + 1);
                }
            }

            function updateUrlWithRessourceRecommandee() {
                if ($ctrl.url) {
                    var ressourceRecommandee = $ctrl.IsRessourcesRecommandees === true ? 1 : 0;
                    var index = $ctrl.url.indexOf($ctrl.RessouresRecommandeeParam);
                    $ctrl.url = $ctrl.url.substr(0, index + $ctrl.RessouresRecommandeeParam.length) + ressourceRecommandee + $ctrl.url.substr(index + $ctrl.RessouresRecommandeeParam.length + 1);
                }
            }

            /*
             * Creer l'url du server.
             */
            function actionCreateSearchLightUrl() {
                var url = "";

                // Si la méthode OnClick est définie, on utilise l'URL par défaut
                if (angular.isDefined($ctrl.onClick())) {
                    var root = $ctrl.url.split("/");
                    url = "/api/" + root[2] + "/SearchLight/?page=" + $ctrl.paging.currentPage +
                        "&pageSize=" + $ctrl.paging.pageSize +
                        "&recherche=" + encodeURIComponent($ctrl.searchText) +
                        "&recherche2=" + encodeURIComponent($ctrl.searchText2) +
                        "&societeId=" + $ctrl.paging.societeId +
                        "&ciId=" + $ctrl.paging.ciId +
                        "&groupeId=" + $ctrl.paging.groupeId +
                        "&agenceId=" + $ctrl.paging.agenceId +
                        "&organisationId=" + $ctrl.paging.organisationId;
                }
                // Sinon on utilise l'URL custom; c'est à dire une URL fixe (avec paramètres personnalisés) issue de l'appelant de la Lookup et on rajoute de les paramètres de pagination et recherche
                else {
                    url = $ctrl.url + "?&page=" + $ctrl.paging.currentPage +
                        "&pageSize=" + $ctrl.paging.pageSize +
                        "&recherche=" + encodeURIComponent($ctrl.searchText) +
                        "&recherche2=" + encodeURIComponent($ctrl.searchText2) +
                        "&recherche3=" + encodeURIComponent($ctrl.searchText3) +
                        "&societeId=" + $ctrl.paging.societeId +
                        "&ciId=" + $ctrl.paging.ciId +
                        "&groupeId=" + $ctrl.paging.groupeId +
                        "&agenceId=" + $ctrl.paging.agenceId +
                        "&organisationId=" + $ctrl.paging.organisationId;
                }
                return url;
            }

            /**
             * Récupération de tous les mots séparés par '*' dans les textbox
             * @param {any} text text dans l'input
             * @returns {any} liste de mots
             */
            function actionGetWords(text) {
                var words = [];

                text = text.trim();

                if (text.indexOf('*') >= 0) {
                    words = text.split('*');
                    for (var i = 0; i < words.length; i++) {
                        words[i] = words[i].trim();
                    }
                }
                else {
                    words.push(text);
                }
                return words;
            }
        }
    })();
})();