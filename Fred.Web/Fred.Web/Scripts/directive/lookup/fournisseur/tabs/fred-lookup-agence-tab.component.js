(function () {
    'use strict';
    angular
        .module('Fred')
        .component('fredLookupAgenceTabComponent', {
            templateUrl: '/Scripts/directive/lookup/fournisseur/tabs/fred-lookup-agence-tab.template.html',
            require: {
                fredLookupFournisseurPanelComponent: '^fredLookupFournisseurPanelComponent'
            },
            bindings: {
                selectedFournisseur: '=',
                groupeId: '@',
                key: '@'
            },
            controller: FredLookupAgenceTabController
        });


    FredLookupAgenceTabController.$inject = ['$timeout', '$http', 'fredSubscribeService', 'lookupKeyDownService'];

    function FredLookupAgenceTabController($timeout, $http, fredSubscribeService, lookupKeyDownService) {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        var $ctrl = this;

        $ctrl.isOpen = false;

        $ctrl.selectedIndex = 0;
        $ctrl.scrollName = 'divScroll' + new Date().getTime();
        $ctrl.searchText = '';

        $ctrl.scrolling = false;
        $ctrl.resources = resources;
        $ctrl.selectedAgence = null;
        $ctrl.textsToHighlight = {
            searchTexts: null,
            searchTexts2: null
        };

        $ctrl.$onInit = function () {
            fredSubscribeService.subscribe({ eventName: 'fred-lookup-agence-tab', callback: openPanel });
        };

        $ctrl.$onDestroy = function () {
            fredSubscribeService.unsubscribe({ eventName: 'fred-lookup-agence-tab', callback: openPanel });
        };

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                  Handlers                                                                      //
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        function openPanel() {

            if ($ctrl.isOpen) {
                return;
            }


            $ctrl.isOpen = true;

            $timeout(function () {
                FredToolBox.bindScrollEnd('.' + $ctrl.scrollName, actionLoadMore);
            });

            // A l'ouverture de la Lookup, on cache l'overflow du body
            //bodyRef.addClass('ovh');
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

        $ctrl.handleChangeSearchText = function (item) {
            //necessaire pour que $ctrl.searchText se mette a jour, l'evenement est declenche avant la mise a jour du binding
            $timeout(function () {
                $ctrl.searchText = $ctrl.searchText;
                $ctrl.textsToHighlight.searchTexts = actionGetWords(item.searchText);
                actionLoad(true);
            });
        };



        //Fonction éxécutée lors du click sur un élément de la picklist
        $ctrl.selectFournisseur = function () {
            $ctrl.fredLookupFournisseurPanelComponent.onSelectFournisseurOnAgencePane($ctrl.selectedFournisseur, null);
        };

        $ctrl.onSelectAgenceOnItem = function (agence) {
            $ctrl.close();
            $ctrl.fredLookupFournisseurPanelComponent.onSelectAgenceOnAgencePane($ctrl.selectedFournisseur, agence);

        };

        //Fonction de fermeture de la picklist
        $ctrl.close = function () {
            $ctrl.isOpen = false;
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

            $ctrl.busy = true;
            $ctrl.scrolling = scrolling === true;

            if (firstLoad) {
                $ctrl.error = '';
                $ctrl.referentiels = [];
                $ctrl.paging.currentPage = 1;
            }

            var urlBeforeAddPagginationAndRecherche = "/api/Fournisseur/SearchLight/?groupeId=1"; //todo changer le groupe 

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
                "&recherche=" + $ctrl.searchText;
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
