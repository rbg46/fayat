(function (angular) {
    'use strict';

    angular.module('Fred').controller('SocieteController', SocieteController);

    SocieteController.$inject = ['$q', '$filter', 'Notify', 'SocieteService', '$uibModal', 'TypeSocieteService', 'UserService'];

    /*
     * @description Controller des Sociétés
     */
    function SocieteController($q, $filter, Notify, SocieteService, $uibModal, TypeSocieteService, UserService) {
        /*
         * -------------------------------------------------------------------------------------------------------------
         *                                            INIT
         * -------------------------------------------------------------------------------------------------------------
         */

        var $ctrl = this;

        // méthodes exposées
        angular.extend($ctrl, {
            handleSave: handleSave,
            handleSaveAndNew: handleSaveAndNew,
            handleCancel: handleCancel,
            handleDelete: handleDelete,
            handleNew: handleNew,
            handleSelect: handleSelect,
            handleSelectImageScreenLogin: handleSelectImageScreenLogin,
            handleSelectImageLogo: handleSelectImageLogo,
            handleCancelImageScreenLogin: handleCancelImageScreenLogin,
            handleCancelImageLogo: handleCancelImageLogo,
            handleSearch: handleSearch,
            handleChangeCode: handleChangeCode,
            handleLookupSelection: handleLookupSelection,
            handleLookupDeletion: handleLookupDeletion,
            handleShowLookup: handleShowLookup,

            handleAddJournalAchat: handleAddJournalAchat,
            handleDeleteJournalAchat: handleDeleteJournalAchat,

            handleDeleteDeviseSec: handleDeleteDeviseSec,
            handleDeleteUnite: handleDeleteUnite,
            GetSocieteInterimExistInGroupe: GetSocieteInterimExistInGroupe,
            handleOpenAssocieSepModal: actionOpenAssocieSepModal,
            handleOnSelectTypeSociete: handleOnSelectTypeSociete
        });

        init();

        return $ctrl;

        /**
         * Initialisation du controller.     
         */
        function init() {
            angular.extend($ctrl, {
                // Instanciation Objet Ressources
                resources: resources,

                societe: {},
                societeList: [],
                filter: { ValueText: "" },

                devise: { reference: null, secondaires: [], societeDeviseList: [] },
                unites: [],
                indeminiteDeplacementCalculTypes: [],
                journal: {
                    journalAchatList: [],
                    newJournalAchat: null,
                    journalFAR: { Code: null, Libelle: null, JournalId: 0 },
                    type: { achat: "ACHAT", far: "FAR" }
                },

                months: [
                    { id: null, name: '' },
                    { id: 1, name: '1' },
                    { id: 2, name: '2' },
                    { id: 3, name: '3' },
                    { id: 4, name: '4' },
                    { id: 5, name: '5' },
                    { id: 6, name: '6' },
                    { id: 7, name: '7' },
                    { id: 8, name: '8' },
                    { id: 9, name: '9' },
                    { id: 10, name: '10' },
                    { id: 11, name: '11' },
                    { id: 12, name: '12' }
                ],
                checkDisplayOptions: "close-right-panel",
                checkDisplayImageScreenLogin: "close",
                checkFormatOptions: "small",
                formSociete: {},
                erreurSocieteInterim: false,
                SANS_INDEMNITE_DEPLACEMENT_CALCUL_TYPE: { IndemniteDeplacementCalculTypeId: 0, Libelle: '' },
                typeSocieteCodes: TypeSocieteService.TypeSocieteCodes
            });

            UserService.getCurrentUser().then(function(user) {
                $ctrl.currentUser = user.Personnel;
            });

            // Chargement de la liste des Sociétés
            $q.when()
                .then(actionGetFilter)
                .then(actionLoad)
                .then(actionLoadIndemniteDeplacementCalculTypes)
                .then(actionGetTypeSocietes);
        }

        /*
         * -------------------------------------------------------------------------------------------------------------
         *                                            HANDLERS
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @function handleNew()
         * @description
         */
        function handleNew() {
            $ctrl.btnAssocieSepTitle = $ctrl.resources.Societe_Index_PanelRight_AssocieSep_Title_1;

            $q.when()
                .then(actionInitPanelSociete)
                .then(actionNewSociete);

            $ctrl.checkDisplayOptions = "open";
        }

        /*
         * @function handleSave()
         * @description
         */
        function handleSave() {
            if (actionCheckFormValidity()) {
                $q.when()
                    .then(actionAddOrUpdate)
                    .then(actionManageSocieteDevise)
                    .then(actionManageSocieteUnite)
                    .then(actionManageJournal)
                    .then(actionLoad);
            }
        }

        /*
         * @function handleSaveAndNew()
         * @description
         */
        function handleSaveAndNew() {
            if (actionCheckFormValidity()) {
                $q.when()
                    .then(actionAddOrUpdate)
                    .then(actionManageSocieteDevise)
                    .then(actionManageSocieteUnite)
                    .then(actionManageJournal)
                    .then(actionLoad)
                    .then(actionInitPanelSociete)
                    .then(actionNewSociete);
            }
        }

        /*
         * @function handleCancel()
         * @description
         */
        function handleCancel() {
            actionCancel();
        }

        /*
         * @function handleDelete()
         * @description
         */
        function handleDelete(societe) {
            actionDeleteSociete(societe);
        }

        /*
         * @function handleLookupSelection()
         * @description
         */
        function handleLookupSelection(type, item) {
            switch (type) {
                case "DeviseRef": {
                    if (actionIsDeviseSecondaire(item)) {
                        $ctrl.devise.reference.DeviseId = null;
                        $ctrl.devise.reference.Devise = null;
                        Notify.error($ctrl.resources.Societe_Controller_Notification_DeviseRefDejaSecondaire);
                        return;
                    }
                    else {
                        $ctrl.devise.reference.DeviseId = item.DeviseId;
                        $ctrl.devise.reference.Devise = item;
                        var deviseRef = $filter('filter')($ctrl.devise.societeDeviseList, { DeviseDeReference: true }, true)[0];
                        if (!deviseRef) {
                            var societeDeviseModel = { SocieteId: $ctrl.societe.SocieteId, DeviseId: item.DeviseId, Devise: item, DeviseDeReference: true, typeDevise: $ctrl.resources.Societe_Controller_TypeDevise_Reference };
                            $ctrl.devise.societeDeviseList.push(societeDeviseModel);
                        }
                    }
                    break;
                }
                case "DeviseSec":
                    {
                        // Check devise not already exist in list
                        var societeDeviseModelSec = { SocieteId: $ctrl.societe.SocieteId, DeviseId: item.DeviseId, Devise: item, DeviseDeReference: false, typeDevise: $ctrl.resources.Societe_Controller_TypeDevise_Secondaire };
                        if (actionCheckExistDeviseSec(societeDeviseModelSec)) { Notify.error($ctrl.resources.Societe_Controller_DoubleSelectionDevise_Erreur); return; }
                        else if ($ctrl.devise.secondaires.length >= 4) { Notify.error($ctrl.resources.Societe_Controller_NombreMaxDeviseAtteint_Erreur); return; }
                        else if (actionIsDeviseReference(societeDeviseModelSec)) { Notify.error($ctrl.resources.Societe_Controller_DeviseDejaReference_Erreur); return; }
                        else {
                            $ctrl.devise.secondaires.push(societeDeviseModelSec);
                            $ctrl.devise.societeDeviseList.push(societeDeviseModelSec);
                            break;
                        }
                    }
                case "Unite":
                    {
                        // Check unite not already exist in list
                        if (actionCheckExistUnite(item)) { Notify.error($ctrl.resources.Societe_Controller_DoubleSelectionUnite_Erreur); return; }
                        else {
                            var uniteSociete = { Unite: item, UniteId: item.UniteId, SocieteId: $ctrl.societe.SocieteId, Type: $ctrl.societe.TypesUnite[0].TypeUniteId };
                            $ctrl.unites.push(uniteSociete);
                            break;
                        }
                    }
                case 'Fournisseur': {
                    $ctrl.societe.FournisseurId = item.FournisseurId;
                    break;
                }
                case 'Classification': {
                    $ctrl.societe.SocieteClassificationId = item.SocieteClassificationId;
                    $ctrl.societe.Classification = item;
                    break;
                }
                default: break;
            }
        }

        /*
         * @function handleLookupDeletion()
         * @description
         */
        function handleLookupDeletion(type) {
            switch (type) {
                case "DeviseRef": {
                    var found = $filter('filter')($ctrl.devise.societeDeviseList, { DeviseId: $ctrl.devise.reference.DeviseId }, true)[0];
                    if (found) {
                        $ctrl.devise.societeDeviseList.splice($ctrl.devise.societeDeviseList.indexOf(found), 1);
                    }
                    $ctrl.devise.reference.Devise = null;
                    $ctrl.devise.reference.DeviseId = null;
                    break;
                }
                case 'Fournisseur': {
                    $ctrl.societe.FournisseurId = null;
                    $ctrl.societe.Fournisseur = null;
                    break;
                }
                case 'Classification': {
                    $ctrl.societe.SocieteClassificationId = null;
                    $ctrl.societe.Classification = null;
                    break;
                }
                default: break;
            }
        }

        /*
         * @function handleShowLookup()
         * @description
         */
        function handleShowLookup(type) {
            switch (type) {
                case "Devise": {
                    return '/api/Devise/SearchLight/';
                }
                case "Unite": {
                    return '/api/Unite/SearchLight/';
                }
                default: break;
            }

        }

        /*
         * @function handleAddJournalAchat()
         * @description
         */
        function handleAddJournalAchat() {
            actionAddJournalAchat();
        }

        /*
         * @function handleDeleteJournalAchat()
         * @description
         */
        function handleDeleteJournalAchat(journal) {
            actionDeleteJournalAchat(journal);
        }

        /*
         * @description Gestion de la sélection d'une société
         */
        function handleSelect(item) {
            $ctrl.societe = angular.copy(item);

            $ctrl.selectedTypeSociete = $ctrl.societe.TypeSociete;

            $ctrl.btnAssocieSepTitle = $ctrl.resources.Societe_Index_PanelRight_AssocieSep_Title_2;

            $q.when()
                .then(actionInitPanelSociete)
                .then(actionGetSocieteById)
                .then(actionLoadDevise)
                .then(actionLoadUnite)
                .then(actionLoadJournal);

            $ctrl.checkDisplayOptions = "open";
        }

        /*
         * @description Ouverture du panneau pour sélectionner une image en background sur l'écran d'authentification
         */
        function handleSelectImageScreenLogin(item) {
            $ctrl.societe = angular.copy(item);
            $ctrl.checkDisplayImageScreenLogin = "open";
            $ctrl.openImageSelector = true;
            $ctrl.isLogo = false;
        }

        /*
         * @description Fermeture du panneau pour sélectionner une image en background sur l'écran d'authentification
         */
        function handleCancelImageScreenLogin() {
            $ctrl.checkDisplayImageScreenLogin = "close";
        }

        /*
         * @description Ouverture du panneau pour sélectionner le logo de la société
         */
        function handleSelectImageLogo(item) {
            $ctrl.societe = angular.copy(item);
            $ctrl.checkDisplayImageLogo = "open";
            $ctrl.openImageSelector = true;
            $ctrl.isLogo = true;

        }

        /*
         * @description Fermeture du panneau pour sélectionner le logo de la société
         */
        function handleCancelImageLogo() {
            $ctrl.checkDisplayImageLogo = "close";
        }

        /*
         * @description Gestion de la recherche
         */
        function handleSearch() {
            actionLoad();
        }

        // Handler de frappe clavier dans le champs code
        function handleChangeCode() {
            if (!$ctrl.formSociete.Code.$error.pattern) {
                actionIsCodeSocieteExist($ctrl.societe.Code, $ctrl.societe.Libelle, $ctrl.societe.SocieteId);
            }
        }

        function handleOnSelectTypeSociete(typeSociete) {
            $ctrl.selectedTypeSociete = typeSociete;

            if ($ctrl.selectedTypeSociete.Code === $ctrl.typeSocieteCodes.SEP) {
                $ctrl.societe.FournisseurId = null;
                $ctrl.societe.Fournisseur = null;
                $ctrl.societe.Classification = null;
                $ctrl.societe.SocieteClassificationId = null;
            }
        }

        /*
         * -------------------------------------------------------------------------------------------------------------
         *                                            ACTIONS
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @description Récupère une société par son identifiant
         */
        function actionGetSocieteById() {
            return SocieteService.GetById({ societeId: $ctrl.societe.SocieteId }).$promise
                .then(function (value) {
                    $ctrl.societe = value;
                    UpdateIndemniteDeplacementCalculType();
                })
                .catch(Notify.defaultError);
        }

        /*
         * @description Vérification de code société déjà existant en Base de Données
         */
        function actionIsCodeSocieteExist(codeSociete, libelle, societeId) {

            SocieteService.IsCodeSocieteExist({ codeSociete: codeSociete, libelle: libelle, societeId: societeId }).$promise
                .then(function (response) {
                    $ctrl.formSociete.Code.$setValidity('codeIdentique', !response.value.CodeIdentique);
                    $ctrl.formSociete.Libelle.$setValidity('libelleIdentique', !response.value.LibelleIdentique);
                })
                .catch(Notify.defaultError);
        }

        /*
         * @description Gestion de l'annulation
         */
        function actionCancel() {
            $ctrl.checkDisplayOptions = "close-right-panel";
            actionInitPanelSociete();
        }

        /*
         * @description Init du panel latéral Société
         */
        function actionInitPanelSociete() {
            $ctrl.erreurSocieteInterim = false;
            $ctrl.devise = { reference: null, secondaires: [], societeDeviseList: [] };
            $ctrl.journal = {
                journalAchatList: [],
                newJournalAchat: null,
                journalFAR: { Code: null, Libelle: null, JournalId: 0 },
                type: { achat: "ACHAT", far: "FAR" }
            };
            $ctrl.formSociete.$setPristine();
            $ctrl.formSociete.$setUntouched();
            $ctrl.formSociete.Code.$setValidity('codeIdentique', true);
            $ctrl.formSociete.Libelle.$setValidity('libelleIdentique', true);
        }

        /*
         * @description Action nouvelle société
         */
        function actionNewSociete() {
            return SocieteService.New().$promise
                .then(function (value) {
                    $ctrl.societe = value;

                    $ctrl.selectedTypeSociete = $filter('filter')($ctrl.typeSocietes, { Code: $ctrl.typeSocieteCodes.SEP }, true)[0];
                    $ctrl.societe.TypeSocieteId = $ctrl.selectedTypeSociete.TypeSocieteId;

                    UpdateIndemniteDeplacementCalculType();
                })
                .catch(Notify.defaultError);
        }

        /*
         * @description Gestion de la suppression d'une société
         */
        function actionDeleteSociete(societe) {

            var modalInstance = $uibModal.open({
                animation: true,
                component: 'deleteSocieteComponent',
                windowClass: 'modal-suppressionSociete',
                resolve: {
                    resources: function () { return $ctrl.resources; },
                }
            });

            modalInstance.result.then(function () {
                SocieteService.Delete({ societeId: $ctrl.societe.SocieteId }).$promise
                    .then(function () {
                        var soc = $filter('filter')($ctrl.societeList, { SocieteId: societe.SocieteId }, true)[0];
                        var index = $ctrl.societeList.indexOf(soc);
                        $ctrl.societeList.splice(index, 1);

                        $ctrl.checkDisplayOptions = "close-right-panel";
                        Notify.message($ctrl.resources.Global_Notification_Suppression_Success);
                    }).catch(function (error) {
                        if (error.status === 400) {
                            Notify.error($ctrl.resources.Global_Notification_Suppression_Error);
                        } else {
                            Notify.error(error.data.Message);
                        }
                    });
            });
        }

        function GetSocieteInterimExistInGroupe() {
            if ($ctrl.societe.IsInterimaire) {
                SocieteService.GetDefaultSocieteInterim().then(function (response) {
                    if (response.data) {
                        $ctrl.erreurSocieteInterim = true;
                    }
                });
            }
            else {
                $ctrl.erreurSocieteInterim = false;
            }
        }

        /*
         * @description Ajout ou Mise à jour d'une société
         */
        function actionAddOrUpdate() {
            $ctrl.societe.SocieteDevises = [];
            // Met à jour l'identifiant du type de calcul des indemnités de déplacement avant l'enregistrement
            $ctrl.devise.societeDeviseList.forEach(DeviseModel => {
                $ctrl.societe.SocieteDevises.push(DeviseModel)
            })
            $ctrl.societe.IndemniteDeplacementCalculTypeId =
                $ctrl.societe.IndemniteDeplacementCalculType.IndemniteDeplacementCalculTypeId === $ctrl.SANS_INDEMNITE_DEPLACEMENT_CALCUL_TYPE.IndemniteDeplacementCalculTypeId
                    ? null
                    : $ctrl.societe.IndemniteDeplacementCalculType.IndemniteDeplacementCalculTypeId;

            if ($ctrl.societe.SocieteId === 0) {
                return SocieteService.Create($ctrl.societe).$promise
                    .then(function (value) {
                        $ctrl.societe = value;
                        UpdateIndemniteDeplacementCalculType();
                        $ctrl.societeList.push(value);
                        Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
                    })
                    .catch(function (reason) {
                        handleErrors(reason.data.ModelState);
                    });
            }
            else {
                return SocieteService.Update($ctrl.societe).$promise
                    .then(function (value) {
                        $ctrl.societe = value;
                        UpdateIndemniteDeplacementCalculType();
                        Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
                    })
                    .catch(function (reason) {
                        handleErrors(reason.data.ModelState);
                    });
            }
        }

        /*
         * @description Action Chargement de la liste des Sociétés
         */
        function actionLoad() {
            return SocieteService.Search($ctrl.filter).$promise
                .then(function (value) {
                    $ctrl.societeList = value;
                })
                .catch(Notify.defaultError);
        }

        /*
         * @description Chargement du filtre par défaut
         */
        function actionGetFilter() {
            return SocieteService.GetFilter().$promise
                .then(function (value) {
                    $ctrl.filter = value;
                })
                .catch(Notify.defaultError);
        }

        function actionCheckFormValidity() {
            if ($ctrl.formSociete.$error.codeIdentique || $ctrl.formSociete.$error.libelleIdentique) {
                return false;
            }
            return true;
        }

        function actionOpenAssocieSepModal() {
            $uibModal.open({
                animation: true,
                size: 'lg',
                component: 'associeSepComponent',
                windowClass: 'modal-associesep',
                resolve: {
                    resources: function () { return $ctrl.resources; },
                    societe: function () { return $ctrl.societe; }
                }
            });
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            GESTION DES DEVISES
         * -------------------------------------------------------------------------------------------------------------
         */

        function actionLoadDevise() {
            return SocieteService.GetSocieteDeviseList({ societeId: $ctrl.societe.SocieteId }).$promise
                .then(function (response) {
                    angular.forEach(response, function (val, key) {
                        if (val.DeviseDeReference) {
                            val.typeDevise = $ctrl.resources.Societe_Controller_TypeDevise_Reference;
                            $ctrl.devise.reference = val;
                        }
                        else {
                            val.typeDevise = $ctrl.resources.Societe_Controller_TypeDevise_Secondaire;
                            $ctrl.devise.secondaires.push(val);
                        }
                    });
                    $ctrl.devise.societeDeviseList = response;
                })
                .catch(function (error) {
                    Notify.error($ctrl.resources.Global_Notification_Error);
                });
        }

        /*
         * @description Vérifie si la devise secondaire est déjà dans la liste ou pas
         */
        function actionCheckExistDeviseSec(societeDevise) {
            var isAlreadyDeviseSec = $filter('filter')($ctrl.devise.secondaires, { DeviseId: societeDevise.DeviseId }, true);
            return societeDevise && isAlreadyDeviseSec && isAlreadyDeviseSec.length > 0;
        }

        /*
         * @description Vérifie si la devise sélectionnée n'est pas déjà la devise de référence
         */
        function actionIsDeviseReference(devise) {
            return devise && $ctrl.devise.reference && devise.DeviseId === $ctrl.devise.reference.DeviseId;
        }

        /*
         * @description Vérifie si la devise sélectionnée n'est pas déjà une devise secondaire
         */
        function actionIsDeviseSecondaire(devise) {
            var isDeviseSec = $filter('filter')($ctrl.devise.secondaires, { DeviseId: devise.DeviseId }, true);
            return devise && isDeviseSec && isDeviseSec.length > 0;
        }

        /*
         * @description Gère la suppression d'une devise secondaire
         */
        function handleDeleteDeviseSec(societeDevise) {
            if (societeDevise) {
                var i = $ctrl.devise.secondaires.indexOf(societeDevise);
                $ctrl.devise.secondaires.splice(i, 1);

                i = $ctrl.devise.societeDeviseList.indexOf(societeDevise);
                $ctrl.devise.societeDeviseList.splice(i, 1);
            }
        }

        function actionManageSocieteDevise() {
            // Permet d'ajouter l'identifiant de la société si c'est une nouvelle société
            angular.forEach($ctrl.devise.societeDeviseList, function (val, key) {
                if (val.SocieteId === null || val.SocieteId === 0) {
                    val.SocieteId = $ctrl.societe.SocieteId;
                }
            });

            return SocieteService.ManageSocieteDeviseList({ societeId: $ctrl.societe.SocieteId }, $ctrl.devise.societeDeviseList).$promise
                .then(function (response) {
                    $ctrl.devise.societeDeviseList = response;
                })
                .catch(function (response) {
                    Notify.error(response.data.Message);
                });
        }



        /* -------------------------------------------------------------------------------------------------------------
         *                                            GESTION DES UNITES
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @description charge la liste des unités
         */
        function actionLoadUnite() {
            $ctrl.unites = [];
            return SocieteService.GetSocieteUniteList({ societeId: $ctrl.societe.SocieteId }).$promise
                .then(function (response) {
                    angular.forEach(response, function (val, key) {
                        $ctrl.unites.push(val);
                    });
                })
                .catch(function (error) {
                    Notify.error($ctrl.resources.Global_Notification_Error);
                });
        }

        /*
         * @description Vérifie si l'unité est déjà dans la liste
         */
        function actionCheckExistUnite(unite) {
            var isAlreadyUnite = $filter('filter')($ctrl.unites, { UniteId: unite.UniteId }, true);
            return isAlreadyUnite && isAlreadyUnite.length > 0;
        }

        /*
         * @description Gère la suppression d'une unité
         */
        function handleDeleteUnite(unite) {
            if (unite) {
                var i = $ctrl.unites.indexOf(unite);
                $ctrl.unites.splice(i, 1);
            }
        }

        /*
         * @description Gère l'enregistrement d'une unité
         */
        function actionManageSocieteUnite() {
            // Permet d'ajouter l'identifiant de la société si c'est une nouvelle société
            angular.forEach($ctrl.unites, function (val, key) {
                if (val.SocieteId === null || val.SocieteId === 0) {
                    val.SocieteId = $ctrl.societe.SocieteId;
                }
            });

            return SocieteService.ManageSocieteUniteList({ societeId: $ctrl.societe.SocieteId }, $ctrl.unites).$promise
                .then(function (response) {
                    $ctrl.unites = response;
                })
                .catch(function (response) {
                    Notify.error(response.data.Message);
                });
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            GESTION DES JOURNAUX
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @description Ajout d'un journal
         */
        function actionAddJournalAchat() {
            if ($ctrl.journal.newJournalAchat.Code) {
                var journalModel = { Code: $ctrl.journal.newJournalAchat.Code, SocieteId: $ctrl.societe.SocieteId, JournalId: 0, Libelle: $ctrl.journal.newJournalAchat.Code, TypeJournal: $ctrl.journal.type.achat };
                var ok = true;

                angular.forEach($ctrl.journal.journalAchatList, function (val, key) {
                    if (val.Code.toLowerCase() === journalModel.Code.toLowerCase()) {
                        ok = false;
                        return;
                    }
                });

                if (ok) {
                    $ctrl.journal.journalAchatList.push(journalModel);
                    $ctrl.journal.newJournalAchat.Code = null;
                }
            }
        }

        /*
         * @description Suppression d'un journal
         */
        function actionDeleteJournalAchat(journal) {
            if (journal) {
                $ctrl.journal.journalAchatList.splice($ctrl.journal.journalAchatList.indexOf(journal), 1);
            }
        }

        /*
         * @description Gestion des journaux : Ajout/Modification/Suppression
         */
        function actionManageJournal() {
            var allJournalList = [];
            if ($ctrl.journal.journalAchatList.length > 0) {
                // Si c'est une nouvelle société, il faut remettre l'identifiant de la société pour chaque journal.
                angular.forEach($ctrl.journal.journalAchatList, function (val, key) {
                    if (val.SocieteId === 0 || val.SocieteId === null || val.SocieteId === undefined) {
                        val.SocieteId = $ctrl.societe.SocieteId;
                    }
                    allJournalList.push(val);
                });
            }

            // Si le code n'est pas renseigné, soit le journal des FAR sera supprimé soit simplement pas enregistré.
            if ($ctrl.journal.journalFAR.Code) {
                // Si le journal n'existe pas en base, l'id est à 0
                if ($ctrl.journal.journalFAR.JournalId === 0) {
                    var journalModel = { Code: $ctrl.journal.journalFAR.Code, SocieteId: $ctrl.societe.SocieteId, JournalId: 0, Libelle: $ctrl.journal.journalFAR.Code, TypeJournal: $ctrl.journal.type.far };
                    allJournalList.push(journalModel);
                }
                else {
                    $ctrl.journal.journalFAR.Libelle = $ctrl.journal.journalFAR.Code; // [TSG] TODO : le libellé = code pour l'instant. A voir si cela changera ?
                    allJournalList.push($ctrl.journal.journalFAR);
                }
            }

            return SocieteService.ManageJournalList({ societeId: $ctrl.societe.SocieteId }, allJournalList).$promise
                .then(actionProcessingJournalData)
                .catch(function (response) { Notify.error(response.data.Message); });
        }

        /*
         * @description Chargement des journaux;
         */
        function actionLoadJournal() {
            return SocieteService.GetJournalList({ societeId: $ctrl.societe.SocieteId }).$promise
                .then(actionProcessingJournalData)
                .catch(Notify.defaultError);
        }

        /*
         * @description Gestion des Journaux FAR et Achat
         */
        function actionProcessingJournalData(value) {
            $ctrl.journal.journalAchatList = [];
            $ctrl.journal.journalFAR = { Code: null, Libelle: null, JournalId: 0, TypeJournal: $ctrl.journal.type.far };
            angular.forEach(value, function (val, key) {
                val.DateCreation = $filter('toLocaleDate')(val.DateCreation);
                val.DateModification = $filter('toLocaleDate')(val.DateModification);
                val.DateCloture = $filter('toLocaleDate')(val.DateCloture);

                if (val.TypeJournal === $ctrl.journal.type.far) {
                    $ctrl.journal.journalFAR = val;
                }
                else if (val.TypeJournal === $ctrl.journal.type.achat) {
                    $ctrl.journal.journalAchatList.push(val);
                }
            });
        }

        function actionGetTypeSocietes() {
            return TypeSocieteService.GetAll().then(function (response) { $ctrl.typeSocietes = response.data; }).catch(function (err) { console.log(err); })
        }

        /* -------------------------------------------------------------------------------------------------------------
         * Type de calcul des indemnités de déplacement
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @description Charge tous les types de calculs des indemnités de déplacement.
         */
        function actionLoadIndemniteDeplacementCalculTypes() {
            return SocieteService.GetIndemniteDeplacementCalculTypes().$promise
                .then(function (response) {
                    $ctrl.indeminiteDeplacementCalculTypes = [];
                    $ctrl.indeminiteDeplacementCalculTypes.push($ctrl.SANS_INDEMNITE_DEPLACEMENT_CALCUL_TYPE);
                    angular.forEach(response, function (val) {
                        $ctrl.indeminiteDeplacementCalculTypes.push(val);
                    });
                })
                .catch(function (response) {
                    Notify.error(response.data.Message);
                });
        }

        /*
         * @description Met à jour le type de calcul des indemnités de déplacement
         */
        function UpdateIndemniteDeplacementCalculType() {
            $ctrl.societe.IndemniteDeplacementCalculType = $ctrl.SANS_INDEMNITE_DEPLACEMENT_CALCUL_TYPE;
            if ($ctrl.societe.IndemniteDeplacementCalculTypeId !== null) {
                for (var i = $ctrl.indeminiteDeplacementCalculTypes.length; i-- > 1;) {
                    var indeminiteDeplacementCalculType = $ctrl.indeminiteDeplacementCalculTypes[i];
                    if ($ctrl.societe.IndemniteDeplacementCalculTypeId === indeminiteDeplacementCalculType.IndemniteDeplacementCalculTypeId) {
                        $ctrl.societe.IndemniteDeplacementCalculType = indeminiteDeplacementCalculType;
                        break;
                    }
                }
            }
        }

        /*
         * @description Fonction de récupération des erreurs métier
         */
        function handleErrors(modelState) {
            if (modelState) {
                for (var key in modelState) {
                    var value = modelState[key][0];
                    Notify.error(value);
                }
            }
            else {
                Notify.error($ctrl.resources.Global_Notification_Error);
            }
        }
    }

})(angular);