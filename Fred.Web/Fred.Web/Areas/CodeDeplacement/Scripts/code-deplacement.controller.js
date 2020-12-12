(function (angular) {
    'use strict';

    angular.module('Fred').controller('CodeDeplacementController', CodeDeplacementController);

    CodeDeplacementController.$inject = ['UserService','$q', '$timeout', 'Notify', 'CodeDeplacementService', 'ProgressBar', 'confirmDialog', 'TypeSocieteService'];

    function CodeDeplacementController(UserService,$q, $timeout, Notify, CodeDeplacementService, ProgressBar, confirmDialog, TypeSocieteService) {

        // assignation de la valeur du scope au controller pour les templates non mis à jour
        var $ctrl = this;

        // méthodes exposées
        $ctrl.loadData = loadData;
        $ctrl.handleSelect = handleSelect;
        $ctrl.handleClickCreateNew = handleClickCreateNew;
        $ctrl.handleClickAddOrUpdate = handleClickAddOrUpdate;
        $ctrl.handleClickAddOrUpdateAndNew = handleClickAddOrUpdateAndNew;
        $ctrl.handleClickDelete = handleClickDelete;
        $ctrl.handleClickCancel = handleClickCancel;
        $ctrl.handleSearch = handleSearch;
        $ctrl.handleChangeCode = handleChangeCode;
        $ctrl.typeSocieteCodesParams = TypeSocieteService.GetQueryParamTypeSocieteCodes([TypeSocieteService.TypeSocieteCodes.INTERNE]);
        
        init();

        return $ctrl;


        /**
         * Initialisation du controller.
         * 
         */
        function init() {


            // Instanciation Objet Ressources
            $ctrl.resources = resources;

            // Initialisation des données
            $ctrl.societeId = null;

            // Instanciation de la recherche
            $ctrl.recherche = "";
            // Attribut d'affichage de la liste
            $ctrl.checkDisplayOptions = "close-right-panel";
            $ctrl.checkFormatOptions = "small";

            $ctrl.isBusy = false;
            $ctrl.isAlreadyUsed = false;

            UserService.getCurrentUser().then(function(user) {
                $ctrl.isUserFes = user.Personnel.Societe.Groupe.Code.trim() === 'GFES' ? true : false;
            });
        }

        /**
         * Contrôle si le code déplacement est valide.
         * 
         * @param {any} codeDeplacement modèle 
         * @returns {Boolean} true si le code déplacement est valide, false sinon.
         */
        function validCodeDeplacement(codeDeplacement) {
            return codeDeplacement &&
                codeDeplacement.KmMini < codeDeplacement.KmMaxi &&
                codeDeplacement.KmMini >= 0 &&
                codeDeplacement.KmMaxi >= 0 &&
                codeDeplacement.KmMaxi <= 9999;
        }

        //////////////////////////////////////////////////////////////////
        // Actions                                                      //
        //////////////////////////////////////////////////////////////////

        /**
         * Action click sur les boutons Enregistrer.
         * 
         * @returns {Promise.<any>} promise
         */
        function actionAddOrUpdate() {
            if ($ctrl.formCodeDep.$invalid || !validCodeDeplacement($ctrl.codeDep)) {
                return $q.reject("invalid object");
            }
            var promise;
            if ($ctrl.codeDep.CodeDeplacementId === 0 ||
                $ctrl.codeDep.CodeDeplacementId === undefined) {
                promise = actionCreate();
            } else {
                promise = actionUpdate();
            }
            return promise
                .then(function () {
                    Notify.message(resources.Global_Notification_Enregistrement_Success);
                    return actionLoad();
                })
                .catch(Notify.defaultError);
        }

        /**
         * Enregistrement de l'entité.
         * 
         * @returns {Promise.<any>} a promise
         */
        function actionCreate() {
            return CodeDeplacementService.Create($ctrl.codeDep).$promise;
        }

        /**
         * Action Update
         * 
         * @returns {Promise.<any>} a promise
         */
        function actionUpdate() {
            return CodeDeplacementService.Update($ctrl.codeDep).$promise;
        }

        /**
         * Action Cancel
         */
        function actionCancel() {
            $ctrl.checkDisplayOptions = "close-right-panel";
            $ctrl.formCodeDep.$setPristine();
            $ctrl.formCodeDep.Code.$setValidity('exist', true);
        }

        /**
         * Action Delete
         * 
         * @param {any} item item à supprimer 
         * @returns {Promise.<any>} a promise
         */
        function actionDelete(item) {
            return actionLoad()
                .then(function () {
                    var params = { codeDeplacementId: item.CodeDeplacementId };
                    return CodeDeplacementService.Delete(params).$promise;
                })
                .then(function () {
                    Notify.message(resources.Global_Notification_Suppression_Success);
                    return actionCancel();
                })
                .catch(function () {
                    Notify.error(resources.Global_Notification_Suppression_Error);
                });
        }

        /**
         * Ouverture du formulaire de création de code déplacement.
         * 
         * @returns {Promise.<any>} a promise
         */
        function actionOpenCreateForm() {
            $ctrl.formCodeDep.$setPristine();
            $ctrl.formCodeDep.Code.$setValidity('exist', true);
            $ctrl.isAlreadyUsed = false;
            return actionNewCodeDeplacement().then(function () {
                $ctrl.checkDisplayOptions = "open";
                $ctrl.changeFormModel = false;
                // scroll to top (delayed)
                $timeout(function () { angular.element('.deplacement.container .form').scrollTop(0); }, 0);
                angular.element('.deplacement.container .form').scrollTop(0);
            });
        }

        /**
         * Action initalisation d'une nouveau Code Déplacement
         * 
         * @returns {Promise.<any>} a promise
         */
        function actionNewCodeDeplacement() {
            var params = { societeId: $ctrl.societeId };
            return CodeDeplacementService.New(params)
                .$promise
                .then(function (value) {
                    $ctrl.codeDep = value;
                    if ($ctrl.isUserFes) {
                        $ctrl.codeDep.IsETAM = true;
                        $ctrl.codeDep.IsCadre = true;
                        $ctrl.codeDep.IsOuvrier = true;
                    }
                })
                .catch(Notify.defaultError);
        }

        /**
         * Action load
         *  
         * @returns {Promise.<any>} a promise
         */
        function actionLoad() {
            var params = {
                societeId: $ctrl.societeId,
                searchText: $ctrl.recherche
            };
            return CodeDeplacementService.Search(params, $ctrl.filters)
                .$promise
                .then(function (value) {
                    if (value.length === 0) {
                        Notify.error($ctrl.resources.Global_Notification_AucuneDonnees);
                    }
                    $ctrl.items = value;
                })
                .catch(Notify.defaultError);
        }

        /**
         * Action d'initialisation de la recherche muli-critère des Codes Déplacement
         * 
         */
        function actionInitSearch() {
            $ctrl.filters = { Code: true, Libelle: true };
        }

        /**
         * Action de test d'existence du Code Déplacement
         * 
         * @param {any} idCourant id du code déplacement courant
         * @param {any} code code
         * @returns {Promise.<any>} a promise
         */
        function existCodeCodeDep(idCourant, code) {
            return CodeDeplacementService.GetBySocieteIdAndCode(
                {
                    code: code,
                    societeId: $ctrl.societeId
                })
                .$promise
                .then(function (result) {
                    var valid = result.CodeDeplacementId === undefined || result.CodeDeplacementId === idCourant;
                    $ctrl.formCodeDep.Code.$setValidity('exist', valid);
                })
                .catch(Notify.defaultError);
        }

        /**
         * Selection dans la Picklist société
         * @returns {Promise.<any>} a promise
         */
        function loadData() {
            ProgressBar.start();
            $ctrl.societeId = $ctrl.societe.SocieteId;
            $ctrl.societeLibelle = $ctrl.societe.CodeLibelle;

            var promises = [];
            // Chargement des données
            promises.push(actionInitSearch());
            promises.push(actionLoad());
            promises.push(actionNewCodeDeplacement());

            return $q
                .all(promises)
                .finally(ProgressBar.complete);
        }

        //////////////////////////////////////////////////////////////////
        // Handlers                                                     //
        //////////////////////////////////////////////////////////////////

        /**
         * Handler de sélection d'une ligne dans le repeater Angular
         * 
         * @param {any} item item sélectionné
         */
        function handleSelect(item) {

            $ctrl.codeDep = angular.copy(item);
            $ctrl.checkDisplayOptions = "open";
            $ctrl.changeFormModel = false;
            $ctrl.isAlreadyUsed = false;
            $ctrl.isBusy = true;
            ProgressBar.start();
            CodeDeplacementService.isAlreadyUsed($ctrl.codeDep.CodeDeplacementId)
                .then(function (response) {
                    $ctrl.isAlreadyUsed = response.data.isAlreadyUsed;
                }).catch(function (error) {
                    Notify.defaultError();
                }).finally(function () {
                    $ctrl.isBusy = false;
                    ProgressBar.complete();
                });
        }

        /**
         * Handler de click sur le bouton ajouter
         * 
         * @return {void|Boolean} false si erreur, sinon void
         */
        function handleClickCreateNew() {
            if (!angular.isNumber($ctrl.societeId)) {
                return false;
            }
            $ctrl.isAlreadyUsed = false;
            actionOpenCreateForm();
            return true;
        }

        /**
         * Handler de click sur le bouton Enregistrer
         */
        function handleClickAddOrUpdate() {
            if ($ctrl.isBusy) {
                return;
            }

            if ($ctrl.isAlreadyUsed) {
                confirmDialog.confirm($ctrl.resources, $ctrl.resources.Global_Modal_Item_Already_Used)
                    .then(function () {
                        manageActionAddOrUpdate();
                    });
            } else {
                manageActionAddOrUpdate();
            }

        }

        function manageActionAddOrUpdate() {
            $ctrl.isBusy = true;
            handleDecimalValues();
            actionAddOrUpdate()
                .then(actionCancel)
                .finally(function () {
                    $ctrl.isBusy = false;
                });
        }

        /**
         * Handler de click sur le bouton Enregistrer et Nouveau
         */
        function handleClickAddOrUpdateAndNew() {
            if ($ctrl.isBusy) {
                return;
            }

            if ($ctrl.isAlreadyUsed) {
                confirmDialog.confirm($ctrl.resources, $ctrl.resources.Global_Modal_Item_Already_Used)
                    .then(function () {
                        manageActionAddOrUpdateNew();
                    });
            } else {
                manageActionAddOrUpdateNew();
            }
          }

        function manageActionAddOrUpdateNew() {
            $ctrl.isBusy = true;
            handleDecimalValues();
            actionAddOrUpdate().then(actionOpenCreateForm)
                .finally(function () {
                    $ctrl.isBusy = false;
                });
        }

        /**
         * Suppression d'un code déplacement
         * 
         * @param {any} codeDep code déplacement
         * @returns {Promise.<any>} a promise
         */
        function handleClickDelete(codeDep) {
            $ctrl.checkDisplayOptions = "close-right-panel";
            return confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationSuppression)
                .then(function () {
                    ProgressBar.start();
                    return actionDelete(codeDep);
                })
                .then(function () {
                    return actionLoad();
                })
                .finally(ProgressBar.complete);
        }

        /**
         * Handler de click sur le bouton Cancel
         * 
         */
        function handleClickCancel() {
            actionCancel();
        }

        function handleDecimalValues() {
            $ctrl.codeDep.KmMini = parseFloat($ctrl.codeDep.KmMini);
            $ctrl.codeDep.KmMaxi = parseFloat($ctrl.codeDep.KmMaxi);
        }

        /**
         * Handler de frappe clavier dans le champs recherche
         * 
         * @param {any} recherche valeur du champ recherche
         */
        function handleSearch() {
            ProgressBar.start();
            actionLoad().finally(ProgressBar.complete);
        }

        /**
         * Handler de frappe clavier dans le champs code
         * 
         */
        function handleChangeCode() {
            if ($ctrl.formCodeDep.Code.$error.pattern) {
                return;
            }
            existCodeCodeDep($ctrl.codeDep.CodeDeplacementId, $ctrl.codeDep.Code);
        }
    }


}(angular));