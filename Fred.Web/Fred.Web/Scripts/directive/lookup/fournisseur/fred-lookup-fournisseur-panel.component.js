(function () {
    'use strict';
    angular
        .module('Fred')
        .component('fredLookupFournisseurPanelComponent', {
            templateUrl: '/Scripts/directive/lookup/fournisseur/fred-lookup-fournisseur-panel.template.html',
            bindings: {
                selectItemAction: '&',
                groupeId: '@',
                key: '@'
            },
            controller: FredLookupFournisseurPanelController
        });


    FredLookupFournisseurPanelController.$inject = ['$timeout', '$scope', '$element', '$http', 'fredSubscribeService', 'lookupKeyDownService'];

    function FredLookupFournisseurPanelController($timeout, $scope, $element, $http, fredSubscribeService, lookupKeyDownService) {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        var $ctrl = this;

        $ctrl.isOpen = false;

        $ctrl.selectedIndex = 0;
        $ctrl.panelAgenceIsVisible = false;
        $ctrl.scrollName = 'divScroll' + new Date().getTime();
        $ctrl.searchText = '';
        $ctrl.searchText2 = '';
        $ctrl.scrolling = false;
        $ctrl.resources = resources;
        $ctrl.selectedFournisseur = null;
        $ctrl.textsToHighlight = {
            searchTexts: null,
            searchTexts2: null
        };


        $ctrl.$onInit = function () {
            fredSubscribeService.subscribe({ eventName: 'fredLookupOpen' + $ctrl.key, callback: openPanel });
        };

        $ctrl.$onDestroy = function () {
            fredSubscribeService.unsubscribe({ eventName: 'fredLookupOpen' + $ctrl.key, callback: openPanel });
        };

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                  Handlers                                                                      //
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        function openPanel(buttonKey) {

            if ($ctrl.isOpen) {
                return;
            }

            //permet de savoir qu'elle est 
            $ctrl.buttonKey = buttonKey;

            $ctrl.isOpen = true;

            $timeout(function () {
                FredToolBox.bindScrollEnd('.' + $ctrl.scrollName, actionLoadMore);
            });

            // Set height           
            var lookupHeaderHeight = $('.custom-header').height();


            $('.' + $ctrl.scrollName).height(window.innerHeight - lookupHeaderHeight);

            $ctrl.selectedIndex = 0;

            actionInitPaging();

            // Si la barre de recherche contient déjà un mot, on lance une recherche à l'ouverture (ou réouverture) de la picklist          
            //actionWatchSearchText();
            actionLoad(true);

        }

        $ctrl.handleKeyDown = (e) => {
            lookupKeyDownService.onKeyDown(e, $ctrl, $ctrl.handleSelection);
        };

        $ctrl.handleChangeSearchText = function () {
            $ctrl.textsToHighlight.searchTexts = actionGetWords($ctrl.searchText);
            $ctrl.textsToHighlight.searchTexts2 = actionGetWords($ctrl.searchText2);
            actionLoad(true);
        };



        //Fonction éxécutée lors du click sur un élément de la picklist
        $ctrl.handleSelection = function (fournisseur) {
            select(fournisseur, null);
        };

        function select(fournisseur, agence) {

            var result = {
                model: fournisseur,// propriete model necessaire pour le bindind du button
                fournisseur: fournisseur,
                agance: agence
            };

            fredSubscribeService.raiseEvent('fredPickListItemSelected' + $ctrl.buttonKey, result);
            //Ici , c'est tres important de passé par un $timeout, dans l'optique de la rétrocompatibilite avec la picklist.
            //Il faut forcer le changement de valeur du pickListModel avant l'envoi de l'evenement.
            //Ce qui n'est pas le cas ici, meme si la valeur est mis dans le model ($ctrl.pickListModel = ref;)
            //le $timeout force le mecanisme d'angular de mis a jour.
            $timeout(function () {
                //envoie de l'evenement de changement de valeur
                $ctrl.selectItemAction({ item: result });
                //Fermeture de la picklist
                $ctrl.handleCancel();
            });
        }

        //Fonction de fermeture de la picklist
        $ctrl.handleCancel = function () {
            $ctrl.selectedFournisseur = null;
            $ctrl.isOpen = false;
            $ctrl.panelAgenceIsVisible = false;
        };


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                            AGENCE                                                                               //
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        $ctrl.onSelectFournisseurOnAgencePane = function (fournisseur, agence) {
            select(fournisseur, null);
        };

        $ctrl.onSelectAgenceOnAgencePane = function (fournisseur, agence) {
            select(fournisseur, agence);
        };


        $ctrl.onSelectFournisseur = function (fournisseur) {
            select(fournisseur, null);
        };


        $ctrl.onOpenAgence = function (fournisseur) {
            $ctrl.selectedFournisseur = fournisseur;
            $ctrl.panelAgenceIsVisible = true;
            fredSubscribeService.raiseEvent('fred-lookup-agence-tab', openPanel);
        };



        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                             GESTION HTTP REQUEST                                                               //
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /*
         * fonction dinitialisation de pagination
         */
        function actionInitPaging() {
            $ctrl.paging = {
                pageSize: 20,
                currentPage: 1
            };
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
            if (!$ctrl.isOpen) {
                return;
            }

            $ctrl.searchText = !angular.isDefined($ctrl.searchText) ? null : $ctrl.searchText;
            $ctrl.searchText2 = !angular.isDefined($ctrl.searchText2) ? null : $ctrl.searchText2;

            $ctrl.busy = true;
            $ctrl.scrolling = scrolling === true;

            if (firstLoad) {
                $ctrl.error = '';
                $ctrl.referentiels = [];
                $ctrl.paging.currentPage = 1;
            }

            var urlBeforeAddPagginationAndRecherche = "/api/Fournisseur/SearchLight/?groupeId=" + $ctrl.groupeId;

            var url = addQueryParamForSearchAndPaggination(urlBeforeAddPagginationAndRecherche);

            $http.get(url)
                .success(actionOnSuccess)
                .error(actionOnError)
                .finally(actionOnFinally);
        }

        /*
         * Fonction executée lors de la reception des donnees du serveur.
         */
        function actionOnSuccess(data) {
            angular.forEach(data, function (item) {
                $ctrl.referentiels.push(item);
            });
            $ctrl.message = '';

            if ((data === null || data.length === 0) && $ctrl.referentiels.length === 0) {
                $ctrl.message = 'Aucun référentiel trouvé.';
            }
        }

        /*
         * Fonction executée lors d'une erreur lors de la reception des donnees du serveur.
         */
        function actionOnError() {
            $ctrl.error = resources.Global_Notification_Error;
        }

        /*
         * Action finale http   
         */
        function actionOnFinally() {
            $ctrl.busy = false;
            $ctrl.scrolling = false;
        }

        /*
         * Creer l'url du server.
         */
        function addQueryParamForSearchAndPaggination(urlBeforeAddPagginationAndRecherche) {

            var url = urlBeforeAddPagginationAndRecherche + "?&page=" + $ctrl.paging.currentPage +
                "&pageSize=" + $ctrl.paging.pageSize +
                "&recherche=" + $ctrl.searchText +
                "&recherche2=" + $ctrl.searchText2;
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
