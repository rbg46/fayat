(function (angular) {
    'use strict';

    var associeSepComponent = {
        templateUrl: '/Areas/Societe/Scripts/modals/associe-sep-modal.component.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: AssocieSepModalController
    };

    angular.module('Fred').component('associeSepComponent', associeSepComponent);

    AssocieSepModalController.$inject = ['TypeParticipationSepService', 'Notify', 'ProgressBar', '$q', '$filter', 'SocieteService', 'TypeSocieteService', 'FournisseurService'];

    function AssocieSepModalController(TypeParticipationSepService, Notify, ProgressBar, $q, $filter, SocieteService, TypeSocieteService, FournisseurService) {
        var $ctrl = this;

        angular.extend($ctrl, {
            handleSave: handleSave,
            handleCancel: handleCancel,
            handleLookupSelection: handleLookupSelection,
            handleLookupDeletion: handleLookupDeletion,
            handleAddAssocieSep: handleAddAssocieSep,
            handleAddAssocieSepChild: handleAddAssocieSepChild,
            handleDeleteAssocieSep: handleDeleteAssocieSep,
            handleDeleteAssocieSepChildren: handleDeleteAssocieSepChildren,
            handleOnTypeParticipationSepChange: handleOnTypeParticipationSepChange,
            handleOnQuotePartChange: handleOnQuotePartChange,

            busy: false
        });

        /*
         * Initilisation du controller de la modal
         */
        $ctrl.$onInit = function () {
            init();

            $ctrl.toDelete = [];
            $ctrl.toCreateOrUpdate = [];
            $ctrl.associeSeps = [];
            $ctrl.typeParticipationSeps = [];
            $ctrl.typeParticipationAssocieCode = 'A';

            // Récupération et formattage des type société Interne pour paramétrer l'url de la lookup société            
            $ctrl.typeSocieteCodesParams = TypeSocieteService.GetQueryParamTypeSocieteCodes([TypeSocieteService.TypeSocieteCodes.INTERNE, TypeSocieteService.TypeSocieteCodes.PARTENAIRE]);

            $q.when()
                .then(onBeginRequest)
                .then(getTypeParticipationSeps)
                .then(getAssocieSeps)
                .then(function () {
                    $ctrl.typeParticipationAssocie = $ctrl.typeParticipationSeps.filter(function (x) { return x.Code === $ctrl.typeParticipationAssocieCode; })[0];
                })
                .finally(onFinallyRequest);
        };

        /**
         * Initialisation du component
         */
        function init() {
            $ctrl.resources = $ctrl.resolve.resources;
            $ctrl.societe = $ctrl.resolve.societe;
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            HANDLERS
         * -------------------------------------------------------------------------------------------------------------
         */

        function handleSave() {

            if ($ctrl.associeSeps && $ctrl.associeSeps.length === 0) {
                // Si liste vide, on ne validate pas, on supprime tout
                actionDeleteAll();
            }
            else {
                if (actionValidate() && $ctrl.form.$valid) {
                    $q.when()
                        .then(onBeginRequest)
                        .then(actionDeleteRange)
                        .then(actionCreateOrUpdateRange)
                        .then(getAssocieSeps)
                        .then(actionOnSuccess)
                        .catch(notifyGlobalError)
                        .finally(onFinallyRequest);
                }
                else {
                    Notify.error($ctrl.resources.Modal_AssocieSep_Table_CorrigerErreur);
                }
            }
        }
       
        function handleAddAssocieSep() {
            var newAssocieSep = {
                SocieteId: $ctrl.societe.SocieteId,
                SocieteAssocieeId: null,
                SocieteAssociee: null,
                TypeParticipationSepId: null,
                TypeParticipationSep: null,
                QuotePart: null,
                FournisseurId: null,
                Fournisseur: null,
                AssocieSepChildren: [],
                SocieteAssocieeValid: true,
                TypeParticipationSepValid: true,
                FournisseursValid: true,
                QuotePartValid: true
            };

            $ctrl.toCreateOrUpdate.push(newAssocieSep);
            $ctrl.associeSeps.push(newAssocieSep);
        }

        function handleAddAssocieSepChild(parent) {
            var newAssocieSepChild = {
                SocieteId: $ctrl.societe.SocieteId,
                Societe: null,
                SocieteAssocieeId: null,
                SocieteAssociee: null,
                TypeParticipationSepId: parent.TypeParticipationSepId,
                TypeParticipationSep: null,
                QuotePart: parent.QuotePart,
                FournisseurId: parent.FournisseurId,
                Fournisseur: null,
                SocieteAssocieeValid: true
            };

            if (parent.AssocieSepId) {
                newAssocieSepChild.AssocieSepParentId = parent.AssocieSepId;
            }

            parent.AssocieSepChildren.push(newAssocieSepChild);
            $ctrl.toCreateOrUpdate.push(newAssocieSepChild);
        }

        function handleOnTypeParticipationSepChange(ligne) {
            onUpdateAssocieSep(ligne);
            ligne.TypeParticipationSep = $filter('filter')($ctrl.typeParticipationSeps, { TypeParticipationSepId: ligne.TypeParticipationSepId }, true)[0];
            checkDuplicates($ctrl.associeSeps, 'TypeParticipationSepId', 'TypeParticipationSepValid', [$ctrl.typeParticipationAssocie.TypeParticipationSepId]);
        }

        function handleOnQuotePartChange(ligne) {
            onUpdateAssocieSep(ligne);
            if ($ctrl.form.$submitted) {                
                checkQuoteParts($ctrl.associeSeps);
            }
        }

        function handleDeleteAssocieSep(ligne) {
            if (ligne.AssocieSepId) {
                $ctrl.toDelete.push(ligne.AssocieSepId);
            }

            if (ligne.AssocieSepChildren && ligne.AssocieSepChildren.length > 0) {
                angular.forEach(ligne.AssocieSepChildren, function (child) {
                    if (child.AssocieSepId) {
                        $ctrl.toDelete.push(child.AssocieSepId);
                        onDeleteAssocieSep($ctrl.toCreateOrUpdate, child);
                    }
                });
            }

            onDeleteAssocieSep($ctrl.associeSeps, ligne);
            onDeleteAssocieSep($ctrl.toCreateOrUpdate, ligne);
            actionCheckSocietes();
            checkDuplicates($ctrl.associeSeps, 'FournisseurId', 'FournisseursValid', null);
        }

        function handleDeleteAssocieSepChildren(parent, child) {
            if (child.AssocieSepId) {
                $ctrl.toDelete.push(child.AssocieSepId);
            }

            onDeleteAssocieSep(parent.AssocieSepChildren, child);
            onDeleteAssocieSep($ctrl.toCreateOrUpdate, child);
            actionCheckSocietes();
        }

        function handleLookupSelection(type, item, ligne, parent) {
            onUpdateAssocieSep(ligne);

            switch (type) {
                case 'SocieteAssociee':
                    ligne.SocieteAssocieeId = item.SocieteId;
                    actionGetFournisseur(ligne);

                    if (!actionCheckSocietes()) {
                        Notify.error($ctrl.resources.Modal_AssocieSep_Societe_Doublon_Error);
                    }

                    break;
                case 'SocieteAssocieeChild':
                    ligne.SocieteAssocieeId = item.SocieteId;

                    if (!actionCheckSocietes()) {
                        Notify.error($ctrl.resources.Modal_AssocieSep_Societe_Doublon_Error);
                    }

                    break;
                case 'Fournisseur':
                    ligne.FournisseurId = item.FournisseurId;

                    if (!checkDuplicates($ctrl.associeSeps, 'FournisseurId', 'FournisseursValid', null)) {
                        Notify.error($ctrl.resources.Modal_AssocieSep_Fournisseur_Doublon_Error);
                    }

                    break;
            }
        }

        function handleLookupDeletion(type, ligne, parent) {
            switch (type) {
                case 'SocieteAssociee':
                case 'SocieteAssocieeChild':
                    ligne.SocieteAssociee = null;
                    ligne.SocieteAssocieeId = null;
                    actionCheckSocietes();
                    break;
                case 'Fournisseur':
                    ligne.Fournisseur = null;
                    ligne.FournisseurId = null;
                    checkDuplicates($ctrl.associeSeps, 'FournisseurId', 'FournisseursValid', null);
                    break;
            }
        }

        function handleCancel() {
            $ctrl.dismiss();
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            ACTIONS
         * -------------------------------------------------------------------------------------------------------------
         */

        function actionGetFournisseur(ligne) {
            if (ligne.SocieteAssociee.FournisseurId) { 
                return FournisseurService.GetById({ fournisseurId: ligne.SocieteAssociee.FournisseurId }).$promise
                    .then(function (f) {
                        ligne.FournisseurId = f.FournisseurId;
                        ligne.Fournisseur = f;
                      
                        if (!checkDuplicates($ctrl.associeSeps, 'FournisseurId', 'FournisseursValid', null)) {
                            Notify.error($ctrl.resources.Modal_AssocieSep_Fournisseur_Doublon_Error);
                        }

                    });
            }
            else {
                ligne.Fournisseur = null;
                ligne.FournisseurId = null;
            }
        }

        function actionOnSuccess() {
            $ctrl.form.$setPristine();
            Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
        }

        function onUpdateAssocieSep(ligne) {
            if (ligne.AssocieSepId) {
                var item = $filter('filter')($ctrl.toCreateOrUpdate, { AssocieSepId: ligne.AssocieSepId }, true)[0];
                if (!item) {
                    $ctrl.toCreateOrUpdate.push(ligne);
                }
            }
        }

        function onDeleteAssocieSep(list, item) {
            var i = list.indexOf(item);

            if (i > -1) {
                list.splice(i, 1);
            }
        }

        function onBeginRequest() {
            ProgressBar.start();
            $ctrl.busy = true;
        }

        function onFinallyRequest() {
            ProgressBar.complete();
            $ctrl.busy = false;
        }

        function getAssocieSeps() {
            return SocieteService.GetAllAssocieSep($ctrl.societe.SocieteId).then(function (response) {
                angular.forEach(response.data, function (val) {
                    val.SocieteAssocieeValid = true;
                    val.TypeParticipationSepValid = true;
                    val.QuotePartValid = true;
                    val.FournisseursValid = true;
                    angular.forEach(val.AssocieSepChildren, function (child) { child.SocieteAssocieeValid = true; });
                });

                $ctrl.associeSeps = response.data;

            }).catch(function (err) { console.log(err); })
        }

        function getTypeParticipationSeps() {
            return TypeParticipationSepService.GetAll().then(function (response) { $ctrl.typeParticipationSeps = response.data; }).catch(function (err) { console.log(err); })
        }

        function actionCreateOrUpdateRange() {
            var list = angular.copy($ctrl.toCreateOrUpdate);
            angular.forEach(list, function (val) { val.AssocieSepChildren = []; });
            if (list && list.length > 0) {
                return SocieteService.CreateOrUpdateAssocieSepRange($ctrl.societe.SocieteId, list).then(function () { $ctrl.toCreateOrUpdate = []; });
            }
        }

        function actionDeleteRange() {
            if ($ctrl.toDelete && $ctrl.toDelete.length > 0) {
                var param = $ctrl.toDelete.join('&associeSepIds=');
                return SocieteService.DeleteAssocieSepRange($ctrl.societe.SocieteId, param).then(function () { $ctrl.toDelete = []; });
            }
        }

        function actionDeleteAll() {
            $q.when()
                .then(onBeginRequest)
                .then(actionDeleteRange)
                .then(actionOnSuccess)
                .catch(notifyGlobalError)
                .finally(onFinallyRequest);
        }

        function notifyGlobalError(err) {
            Notify.error($ctrl.resources.Global_Notification_Error);
            console.error(err);
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            VALIDATION
         * -------------------------------------------------------------------------------------------------------------
         */

        /** Validation du formulaire (Recherche de doublons, somme des quotes-parts)
         * @returns {any} vrai si tout est valide, sinon faux
         */
        function actionValidate() {
            return actionCheckSocietes()
                && checkDuplicates($ctrl.associeSeps, 'TypeParticipationSepId', 'TypeParticipationSepValid', [$ctrl.typeParticipationAssocie.TypeParticipationSepId])
                && checkDuplicates($ctrl.associeSeps, 'FournisseurId', 'FournisseursValid', null)
                && checkQuoteParts($ctrl.associeSeps);
        }

        /**
         * Vérification de doublons de société dans la liste (niveau 1) principale et toutes les listes de niveau 2
         * @returns {boolean} vrai si aucun doublon, sinon faux
         */
        function actionCheckSocietes() {
            var allAssocieSeps = [];

            angular.forEach($ctrl.associeSeps, function (val) { allAssocieSeps = allAssocieSeps.concat(val.AssocieSepChildren); });

            allAssocieSeps = allAssocieSeps.concat($ctrl.associeSeps);

            return checkDuplicates(allAssocieSeps, 'SocieteAssocieeId', 'SocieteAssocieeValid');
        }

        function setValidity(list, field, value) {
            angular.forEach(list, function (item) { item[field] = value; });
        }

        /**
         * Règle de gestion Quote Part
         * @param {any} list liste d'éléments à vérifier
         * @returns {boolean} faux si la somme n'est pas égale à 100, sinon faux
         */
        function checkQuoteParts(list) {
            var quotePartSum = 0;
            angular.forEach(list, function (item) { quotePartSum += parseFloat(item.QuotePart); });

            if (quotePartSum !== 100) {
                setValidity(list, 'QuotePartValid', false);
                return false;
            }
            else {
                setValidity(list, 'QuotePartValid', true);
            }

            return true;
        }

        /**
         * Vérification d'existence de duplication
         * @param {any} list liste d'éléments à vérifier
         * @param {any} field champ à vérifier
         * @param {any} fieldValid Détermine si le champ est valide ou non
         * @param {any} exceptions liste d'éléments à exclure
         * @returns {boolean} vrai si aucun doublon, sinon faux
         */
        function checkDuplicates(list, field, fieldValid, exceptions) {
            var allIds = list.filter(function (x) { return x[field] !== null && x[field] !== undefined && x[field] !== 0; }).map(function (x) { return x[field]; });
            var allDuplicatedIds = findDuplicates(allIds, exceptions);

            if (allDuplicatedIds && allDuplicatedIds.length > 0) {

                angular.forEach(list, function (item) {
                    var i = allDuplicatedIds.indexOf(item[field]);
                    item[fieldValid] = !(i > -1);
                });

                return false;
            }
            else {
                setValidity(list, fieldValid, true);
            }

            return true;
        }

        /**
         * Récupération des duplications
         * @param {any} data liste à vérifier
         * @param {any} exceptions liste d'éléments à exclure
         * @returns {any[]} return les duplications dans data
         */
        function findDuplicates(data, exceptions) {
            var result = [];

            //Exclusion des objets dans 'data'
            if (exceptions && exceptions.length > 0) {
                data = data.filter(function (x) { return exceptions.indexOf(x) === -1; });
            }

            angular.forEach(data, function (element, index) {

                // Find if there is a duplicate or not
                if (data.indexOf(element, index + 1) > -1) {

                    // Find if the element is already in the result array or not
                    if (result.indexOf(element) === -1) {
                        result.push(element);
                    }
                }
            });
            return result;
        }
    }
}(angular));