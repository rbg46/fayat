(function () {
    'use strict';

    angular.module('Fred').service('BudgetService', BudgetService);
    BudgetService.$inject = ['$http', '$window'];

    function BudgetService($http, $window) {
        var service = this;
        var flexTableScrollbarPos = 0;

        $window.onbeforeunload = function (e) {
            if (service.OnQuitPage) {
                return service.OnQuitPage(e);
            }
        };


        //////////////////////////////////////////////////////////////////
        // Enumération                                                  //
        //////////////////////////////////////////////////////////////////
        // Provient de Fred.Entities.TypeAvancementBudget
        service.TypeAvancementEnum = {
            Aucun: 0,
            Quantite: 1,
            Pourcentage: 2
        };

        service.CodeEtatAvancement = {
            Enregistre: "ER",
            AValider: "AV",
            Valide: "VA"
        };

        service.CodeBudgetEtat = {
            Brouillon: "BR",
            AValider: "AV",
            EnApplication: "EA",
            Archive: "AR"
        };

        service.DetailColumnEnum = {
            Unite: "Unite",
            Quantite: "Quantite",
            PU: "PU"
        };

        //////////////////////////////////////////////////////////////////
        // Evènements                                                   //
        //////////////////////////////////////////////////////////////////
        service.Events = {
            SaveDetail: "SaveDetail",                                           // arg : { continueWithValidation: booléen : True entrainera une demande de validation après, false ne fera rien}
            LoadDetail: "LoadDetail",                                           // arg : { budgetId: $ctrl.budgetId}
            LoadSousDetail: "LoadSousDetail",                                   // arg : { Tache4 : BudgetDetailLoad.Tache4Model }
            SaveSousDetail: "SaveSousDetail",                                   // arg : { Facultatif : SaveDoneCallback(true | false) : Callback a appelé une fois que la sauvegarde est terminée,
            //                                                                         si l'argument de la callback est true : la sauvegarde s'set bien passée, sinon l'argument est à false  
            //                                                                         si la sauvegarde n'est pas lancé car aucun changement n'est en attente la fonction n'est pas appelée}
            ValidateDetail: "ValidateDetail",                                   // Pas d'argument
            DisplayDetail: "DisplayDetail",

            DisplayDialogToAddT4: "DisplayDialogToAddT4",                       // arg : { Tache3 : BudgetDetailLoad.Tache3Model, OnValidate : function(ManageT4Create.ResultModel) }
            DisplayDialogToChangeT4: "DisplayDialogToChangeT4",                 // arg : { Tache4 : BudgetDetailLoad.Tache4Model, OnValidate : function(string code, string libelle) }
            DisplayDialogToDeleteT4: "DisplayDialogToDeleteT4",                 // arg : { Tache4 : BudgetDetailLoad.Tache4Model, OnValidate : function() }
            DisplayDialogToDeleteBudgetT4: "DisplayDialogToDeleteBudgetT4",     // arg : { Tache4 : BudgetDetailLoad.Tache4Model, OnValidate : function() }

            DisplayValidationDialog: "DisplayValidationDialog",
            DisplayReturnDraftDialog: "DisplayReturnDraftDialog",
            DisplayRecalage: "DisplayRecalage",
            RefreshListBudget: "RefreshListBudget",

            OpenPanelCommentaire: "OpenPanelCommentaire",                       // arg : { Tache : BudgetDetailLoad.Tache[x]Model || SousDetailItem: BudgetSousDetailLoad.ItemModel }
            OpenPanelCommentaireAvancement: "OpenPanelCommentaireAvancement",   // arg : { Tache1:=??, Tache2:=??, Tache3:=??, Tache4=?? }
            PanelCommentaireModified: "PanelCommentaireModified",               // arg : { Tache : BudgetDetailLoad.Tache[x]Model, SousDetailItem: BudgetSousDetailLoad.ItemModel, Commentaire : string }

            OpenPanelRessource: "OpenPanelRessource",                           // pas d'argument
            PanelRessourceAdded: "PanelRessourceAdded",                         // arg : { Chapitre : ChapitreLightModel, SousChapitre : SousChapitreLightModel, Ressource : RessourceLightModel }
            OpenPanelCustomizeDisplay: "OpenPanelCustomizeDisplay",
            PanelCustomizeDisplayModified: "PanelCustomizeDisplayModified",

            SousDetailVueChanged: "SousDetailVueChanged",                       // pas d'argument

            // arg : { Periode : string, AvancementValide : bool, PeriodeComptableCloturee : bool }
            DisplayControleBudgetaireValidationErreurDialog: "DisplayControleBudgetaireValidationErreurDialog",

            // arg : { Periodes : string[] }
            DisplayControleBudgetaireValidationVerrouillagePrecedentsDialog: "DisplayControleBudgetaireValidationVerrouillagePrecedentsDialog",

            // pas d'argument
            DisplayControleBudgetaireValidationVerrouillagePrecedentsDialogValidated: "DisplayControleBudgetaireValidationVerrouillagePrecedentsDialogValidated",

            // Affiche la fenêtre de copie / déplacement de T4
            //  arg : {
            //      Tache : BudgetDetailLoad.Tache[x]Model : la tache cible, de niveau 3 ou 4
            //      Budget : le budget courant
            //      OnAction : Appelé lorsque l'utilisateur valide : function (
            //          Action : BudgetDetailService.CopierDeplacerT4ActionEnum : l'action sélectionnée
            //          Budget : Le budget parent des tâches 4 sélectionnées
            //          Taches4 : BudgetDetailLoad.Tache4Model : les tâches 4 sélectionnées
            //      )
            //  }
            DisplayDialogToCopyMoveT4: "DisplayDialogToCopyMoveT4",

            // Affiche la fenêtre de confirmation copie / déplacement de T4
            //  arg : {
            //      Action : BudgetDetailService.CopierDeplacerT4ActionEnum : l'action sélectionnée
            //      Budget : BudgetDetailLoad.Model : le budget courant
            //      Taches4 : BudgetDetailLoad.Tache4Model[] : liste des tâches 4 concernées
            //      OnConfirm : Appelé lorsque l'utilisateur valide :
            //          - pour la copie : function(
            //              CopierMode : BudgetDetailService.CopierT4Mode : une valeur de l'énumération en mode copier
            //              Taches4: liste de { Code, Libelle }
            //          - pour le déplacement / remplacement : function()
            //      )
            //  }
            DisplayDialogToConfirmCopyMoveT4: "DisplayDialogToConfirmCopyMoveT4",

            // Demande l'affichage du bandeau lateral contenant le plan de tache
            DisplayPlanTacheLateral: "DisplayPlanTacheLateral",

            DisplayHistoriqueBibliothequePrix: "DisplayHistoriqueBibliothequePrix",
            HideHistoriqueBibliothequePrix: "HideHistoriqueBibliothequePrix",

            // Affiche la fenêtre d'enregistrement sur CI d'une bibliothèque des prix
            // arg : {
            //      BudgetsBrouillons : [BudgetVersionAuteurModel][]
            //      OnContinue(arg) {
            //          BudgetIdAEnregistrer : int[] - id des budgets sélectionnés
            //          UpdateValeursSurLignesEnException : true si la propagation implique les lignes en exception, sinon false
            //      }
            // }
            BibliothequePrixEnregistrementSurCiDialog: "BibliothequePrixEnregistrementSurCiDialog",

            // Affiche la fenêtre de copie d'une bibliothèque des prix
            // arg : {
            //      OrganisationId : identifiant de l'organisation cible (du CI)
            //      DeviseId : identifiant de la devise courante
            //      EtablissementOrganisationId : identifiant de l'organisation de l'établissement (parent du CI)
            //      OnValidate(arg) {
            //          [Fred.Web.Shared.Models.Budget.BibliothequePrix.BibliothequePrixForCopyLoad.ResultModel]
            //      }
            // }
            BibliothequePrixDisplayCopierValeursDialog: "BibliothequePrixDisplayCopierValeursDialog",

            DisplayDialogToCopyBudget: "DisplayDialogToCopyBudget",
            DisplayDialogToConfirmCopyBudget: "DisplayDialogToConfirmCopyBudget"
        };


        //////////////////////////////////////////////////////////////////
        // Modèle front -> back                                         //
        //////////////////////////////////////////////////////////////////

        // Crée un [BudgetDetailSave.Model].
        // - budgetId : identifiant du budget.
        service.CreateBudgetDetailSaveModel = function (budgetId) {
            return {
                BudgetId: budgetId,
                Tache1To3s: [],
                Taches4: [],
                BudgetT4sDeleted: []
            };
        };

        // Crée un [BudgetDetailSave.Tache4Model].
        // - tacheId : identifiant de la tâche.
        // - typeAvancement : le type d'avancement, provient de l'énumération TypeAvancementEnum.
        // - commentaire : le commentaire.
        // - tache3Id : la tâche de niveau 3 parente .
        service.CreateBudgetDetailSaveTache4Model = function (tacheId, typeAvancement, commentaire, tache3Id) {
            return {
                TacheId: tacheId,
                TypeAvancement: typeAvancement,
                Commentaire: commentaire ? commentaire.trim() : "",
                Tache3Id: tache3Id
            };
        };

        // Crée un [BudgetDetailSave.Tache1To3Model].
        // - tacheId : identifiant de la tâche.
        // - commentaire : le commentaire.
        service.CreateBudgetDetailSaveTache1To3Model = function (tacheId, commentaire) {
            return {
                TacheId: tacheId,
                Commentaire: commentaire ? commentaire.trim() : ""
            };
        };

        // Crée un [BudgetDetailLoad.Tache4Model].
        // - tacheId : identifiant de la tâche.
        // - code : le code de la tâche.
        // - libelle: le libellé de la tâche.
        // - IsActive: l'état d'activation de la tache
        service.CreateBudgetDetailLoadTache4Model = function (tacheId, code, libelle, isActive) {
            return {
                TacheId: tacheId,
                Code: code,
                Libelle: libelle,
                IsActive: isActive
            };
        };

        // Crée un [BudgetDetailLoad.BudgetT4Model].
        // - vueSD : la vue sous-detail.
        service.CreateBudgetDetailLoadBudgetT4Model = function (vueSD) {
            return {
                BudgetT4Id: 0,
                TypeAvancement: null,
                Commentaire: "",
                QuantiteDeBase: null,
                QuantiteARealiser: null,
                Unite: null,
                PU: null,
                MontantSD: null,
                MontantT4: null,
                VueSD: vueSD,
                Tache3Id: 0
            };
        };

        // Crée un [BudgetDetailLoad.Tache1To3InfoModel].
        service.CreateBudgetDetailLoadTache1To3InfoModel = function () {
            return {
                BudgetTacheId: 0,
                Commentaire: ""
            };
        };

        // Crée un [BudgetSousDetailSave.Model].
        // - budget : le budget
        // - sousDetail : le sous-détail
        // - itemsChanged : les éléments changés ou ajoutés
        // - itemsDeletedId : les identifiants des éléments supprimés
        service.CreateBudgetSousDetailSaveModel = function (budget, sousDetail, itemsChanged, itemsDeletedId) {
            return {
                BudgetId: budget.BudgetId,
                BudgetT4Id: sousDetail.BudgetT4Id,
                ItemsChanged: itemsChanged,
                ItemsDeletedId: itemsDeletedId
            };
        };

        // Crée un [BudgetSousDetailSave.Tache4Model].
        // - tacheId : identifiant de la tâche.
        // - typeAvancement : le type d'avancement, provient de l'énumération TypeAvancementEnum.
        // - commentaire : le commentaire.
        service.CreateBudgetSousDetailSaveTache4Model = function (tacheId, budgetT4) {
            return {
                TacheId: tacheId,
                MontantT4: budgetT4.MontantT4,
                PU: budgetT4.PU,
                QuantiteDeBase: budgetT4.QuantiteDeBase,
                QuantiteARealiser: budgetT4.QuantiteARealiser,
                UniteId: budgetT4.Unite ? budgetT4.Unite.UniteId : null,
                VueSD: budgetT4.VueSD,
                TypeAvancement: budgetT4.TypeAvancement
            };
        };

        // Crée un [BudgetSousDetailSave.ItemModel].
        // - sousDetailItem : l'élément de sous-détail concerné.
        service.CreateBudgetSousDetailSaveItemModel = function (sousDetailItem) {
            return {
                ViewId: sousDetailItem.ViewId,
                BudgetSousDetailId: sousDetailItem.BudgetSousDetailId,
                RessourceId: sousDetailItem.Ressource.RessourceId,
                QuantiteSD: sousDetailItem.View.QuantiteSD,
                QuantiteSDFormule: sousDetailItem.View.QuantiteSDFormule,
                Quantite: sousDetailItem.View.Quantite,
                QuantiteFormule: sousDetailItem.View.QuantiteFormule,
                PrixUnitaire: sousDetailItem.View.PrixUnitaire,
                Montant: sousDetailItem.View.Montant,
                Commentaire: sousDetailItem.View.Commentaire.trim(),
                UniteId: sousDetailItem.View.Unite ? sousDetailItem.View.Unite.UniteId : null
            };
        };

        // Crée un [BudgetSousDetailLoad.ItemModel].
        // - budgetSousDetailId : identifiant du sous-détail.
        // - prixUnitaire : le prix unitaire.
        // - quantiteSD : la quantité SD.
        // - quantite : la quantité.
        // - montant : le montant.
        // - chapitre : le chapitre concerné.
        // - sousChapitre : le sous-chapitre concerné.
        // - ressource : la ressource concernée.
        service.CreateBudgetSousDetailLoadItemModel = function (budgetSousDetailId, prixUnitaire, quantiteSD, quantite, montant, chapitre, sousChapitre, ressource) {
            var ret = {
                BudgetSousDetailId: budgetSousDetailId,
                PrixUnitaire: prixUnitaire,
                QuantiteSD: quantiteSD,
                Quantite: quantite,
                Montant: montant,
                Commentaire: "",
                Unite: ressource.Unite ? ressource.Unite : null,
                Chapitre: {
                    ChapitreId: chapitre.ChapitreId,
                    Code: chapitre.ChapitreCode,
                    Libelle: chapitre.ChapitreLibelle
                },
                SousChapitre: {
                    SousChapitreId: sousChapitre.SousChapitreId,
                    Code: sousChapitre.SousChapitreCode,
                    Libelle: sousChapitre.SousChapitreLibelle
                },
                Ressource: {
                    RessourceId: ressource.RessourceId,
                    Code: ressource.RessourceCode,
                    Libelle: ressource.RessourceLibelle,
                    UniteId: ressource.Unite ? ressource.Unite.UniteId : null,
                    TypeRessourceId: ressource.TypeRessourceId
                }
            };

            return ret;
        };

        // Crée un [BudgetSousDetailCopier.Model].
        service.CreateBudgetSousDetailCopierModel = function (budgetCibleId, ciSourceId) {
            return {
                BudgetCibleId: budgetCibleId,
                CiSourceId: ciSourceId,
                Items: []
            };
        };

        // Crée un [BudgetSousDetailCopier.ItemModel].
        service.CreateBudgetSousDetailCopierItemModel = function (budgetT4SourceId, budgetT4Cible) {
            return {
                BudgetT4SourceId: budgetT4SourceId,
                BudgetT4Cible: budgetT4Cible
            };
        };


        //////////////////////////////////////////////////////////////////
        // Utilitaire                                                   //
        //////////////////////////////////////////////////////////////////
        service.GetTwoDecimalValue = function (value) {
            return parseFloat(value).toFixed(2);
        };

        service.IsNotNullOrEmpty = function (value) {
            return value !== undefined && value !== null && value !== '';
        };
        //////////////////////////////////////////////////////////////////
        // Redirections                                                 //
        //////////////////////////////////////////////////////////////////
        service.ShowBudgetDetail = function (budgetId) {
            $window.location.href = "/Budget/Budget/Detail/" + budgetId;
        };

        /**
         * Affiche la page de la liste des budgets
         * @param {any} ciId Id du CI qui sera préchargé, si ce paramètre n'est pas renseigné, aucun CI n'est préchargé
         */
        service.ShowBudgetListe = function (ciId) {

            var budgetListUrl = "/Budget/Budget/Liste/";
            if (ciId) {
                budgetListUrl += ciId;
            }

            $window.location.href = budgetListUrl;
        };

        service.ShowOngletDepenses = function (filter, labelfilters) {
            sessionStorage.setItem('explorateurDepenseFilter', JSON.stringify({ filterValue: filter, filterLabel: labelfilters }));
            $window.open("/Depense/Depense/Explorateur/" + filter.CiId);
        };

        //////////////////////////////////////////////////////////////////
        // HTTP                                                         //
        //////////////////////////////////////////////////////////////////

        service.GetCi = function (ciId) {
            return $http.get("/api/CI/" + ciId);
        };

        service.GetBudgetByBudgetId = function (budgetId) {
            return $http.get("/api/Budget/Id/" + budgetId);
        };

        service.GetBudget = function (ciId) {
            return $http.get("/api/Budget/" + ciId);
        };

        service.GetBudgetBrouillonDuBudgetEnApplication = function (ciId, deviseId) {
            return $http.get("/api/Budget/Application/Brouillons/" + ciId + "/" + deviseId);
        };

        service.GetBudgetsBrouillons = function (ciId, deviseId) {
            return $http.get("/api/Budget/GetBudgetsBrouillons/" + ciId + "/" + deviseId);
        };

        service.CopieBudgetDansMemeCi = function (budgetId, useBibliothequeDesPrix) {
            return $http.post("/api/Budget/" + budgetId + '/' + useBibliothequeDesPrix);
        };

        service.CreateEmptyBudgetSurCi = function (ciId) {
            return $http.post("/api/Budget/" + ciId);
        };

        service.PartageOuDepartageBudget = function (budgetId) {
            return $http.post("/api/Budget/Partage/" + budgetId);
        };

        service.SupprimerBudget = function (budgetId) {
            return $http.delete("/api/Budget/Suppression/" + budgetId);
        };

        service.RestoreDeletedBudget = function (budgetId) {
            return $http.put('/api/Budget/Suppression/' + budgetId);
        };

        service.SaveBudgetChangeInListView = function (budgetToSave) {
            return $http.put("/api/Budget/Liste", budgetToSave);
        };

        service.BudgetDetailsExportExcel = function (budgetId, templateName, isPdfConverted, disabledTasksDisplayed, niveauxVisible) {
            let model = {
                BudgetId: budgetId,
                TemplateName: templateName,
                IsPdfConverted: isPdfConverted,
                DisabledTasksDisplayed: disabledTasksDisplayed,
                NiveauxVisibles: niveauxVisible
            };
            return $http.put("/api/Budget/GetDetail/Excel", model);
        };


        service.GetDetail = function (budgetId) {
            return $http.get("/api/Budget/GetDetail?budgetId=" + budgetId);
        };

        service.GetMessageMiseEnApllication = function (ciId, version) {
            return $http.get("/api/Budget/GetMessageMiseEnApllication?ciId=" + ciId + "&version=" + version);
        };

        // Enregistre le détail d'un budget.
        // - model : [BudgetDetailSave.Model].
        service.SaveDetail = function (model) {
            return $http.post("/api/Budget/SaveDetail", model);
        };

        // Valide le détail d'un budget.
        service.ValidateDetail = function (budgetId, commentaire) {
            return $http.post("/api/Budget/ValidateDetailBudget?budgetId=" + budgetId + "&commentaire=" + commentaire);
        };

        // Remet le budget à l'état brouillon
        service.RetourBrouillon = function (budgetId, commentaire) {
            return $http.post("/api/Budget/RetourBrouillon?budgetId=" + budgetId + "&commentaire=" + commentaire);
        };

        service.GetControleBudgetaire = function (filter) {
            return $http.post("/api/Budget/ControleBudgetaire/", filter);
        };

        service.GetPeriodeControleBudgetaireBrouillon = function (budgetId, periode) {
            return $http.get("/api/Budget/ControleBudgetaire/Brouillon/Periode/" + budgetId + "/" + periode);
        };

        service.EnregistreControleBudgetaire = function (controleBudgetaireModel) {
            return $http.put("/api/Budget/ControleBudgetaire/", controleBudgetaireModel);
        };

        service.ControleBudgetaireExportExcel = function (model) {
            return $http.put("/api/Budget/ControleBudgetaire/Excel/", model);
        };

        /**
         * Retourne les montants de l'ajustement saisie pour la période passée dans filtre
         * @param {any} budgetId l'id du budget dont on veut les valeurs d'ajustements
         * @param {any} periode periode dont on veut les valeurs d'ajustements
         * @returns {any} Cette fonction retourne un tableau d'objet liant une ressource, une tache et une valeur d'ajustement
         */
        service.GetAjustement = function (budgetId, periode) {
            return $http.get("/api/Budget/ControleBudgetaire/Ajustement/" + budgetId + "/" + periode);
        };

        /**
         * Passe l'état du controle budgétaire donné à l'état A Valider
         * @param {any} budgetId id du budget auquel est rattaché le controle budgétaire
         * @param {any} periode periode d'application du controle budétaire dont on modifie l'état
         * @return {any} un objet Promise tel que decrit dans la doc angular pour les fonctions de $http
         */
        service.DemandeValidationControleBudgetaire = function (budgetId, periode) {
            return $http.put("/api/Budget/ControleBudgetaire/Etat/" + budgetId + "/" + periode + "/" + "AV");
        };

        /**
         * Passe l'état du controle budgétaire donné à l'état Valié
         * @param {any} budgetId Un objet contenant toutes les informations sur le contole budgétaire voir l'API BudgetController
         * @param {any} periode periode d'application du controle budétaire dont on modifie l'état
         * @return {any} un objet Promise tel que decrit dans la doc angular pour les fonctions de $http
         */
        service.ValideControleBudgetaire = function (budgetId, periode) {
            return $http.put("/api/Budget/ControleBudgetaire/Etat/" + budgetId + "/" + periode + "/" + "EA");
        };

        /**
         * Repasse le controle budgétaire rattaché à l'id passé en paramètre à l'état brouillon
         * @param {any} budgetId id du budget auquel est rattaché le controle budgétaire
         * @param {any} periode periode d'application du controle budétaire dont on modifie l'état
         * @return {any} un objet Promise tel que decrit dans la doc angular pour les fonctions de $http
         */
        service.RepasseControleBudgetaireEtatBrouillon = function (budgetId, periode) {
            return $http.put("/api/Budget/ControleBudgetaire/Etat/" + budgetId + "/" + periode + "/" + "BR");
        };

        service.GetNextTacheCode = function (tacheParenteId, count) {
            return $http.get("/api/Budget/GetNextTacheCode?tacheParenteId=" + tacheParenteId);
        };

        service.GetNextTacheCodes = function (tacheParenteId, count) {
            return $http.get("/api/Budget/GetNextTacheCodes?tacheParenteId=" + tacheParenteId + "&count=" + count);
        };

        service.CreateTache4 = function (ciId, tache3Id, code, libelle) {
            var model = {
                CiId: ciId,
                Tache3Id: tache3Id,
                Code: code,
                Libelle: libelle
            };
            return $http.post("/api/Budget/CreateTache4", model);
        };

        service.CreateTaches4 = function (ciId, tache3Id, taches4) {
            var model = {
                CiId: ciId,
                Tache3Id: tache3Id,
                Taches4: taches4
            };
            return $http.post("/api/Budget/CreateTaches4", model);
        };

        service.ChangeTache4 = function (ciId, tache4Id, code, libelle) {
            var model = {
                CiId: ciId,
                TacheId: tache4Id,
                Code: code,
                Libelle: libelle
            };
            return $http.post("/api/Budget/ChangeTache4", model);
        };

        service.DeleteTache4 = function (ciId, tache4Id) {
            var model = {
                TacheId: tache4Id,
                CiId: ciId
            };
            return $http.post("/api/Budget/DeleteTache4", model);
        };

        service.GetSousDetail = function (ciId, budgetT4Id) {
            return $http.get("/api/Budget/GetSousDetail?ciId=" + ciId + "&budgetT4Id=" + budgetT4Id);
        };

        service.SaveSousDetail = function (model) {
            return $http.post("/api/Budget/SaveSousDetail", model);
        };

        service.GereRessourcesRecommandees = function (ciId) {
            return $http.get("/api/Budget/GereRessourcesRecommandees?ciId=" + ciId);
        };

        service.GetChapitres = function (ciId, deviseId, searchText, page, pageSize) {
            return $http.get("/api/Budget/GetChapitres?ciId=" + ciId + "&deviseId=" + deviseId + "&filter=" + searchText + "&page=" + page + "&pageSize=" + pageSize);
        };

        service.GetAvancement = function (ciId, periode) {
            return $http.get("/api/Budget/Avancement?ciId=" + ciId + "&periode=" + periode);
        };

        service.GetAvancementRecette = function (ciId, periode) {
            return $http.get("/api/Budget/AvancementRecette?ciId=" + ciId + "&periode=" + periode);
        };

        service.SaveAvancementRecette = function (model) {
            return $http.post("/api/Budget/SaveAvancementRecette", model);
        };

        service.SaveAvancement = function (model) {
            return $http.post("/api/Budget/SaveAvancement", model);
        };

        service.ValidateAvancement = function (model, etatAvancement) {
            return $http.post("/api/Budget/ValidateAvancement/" + etatAvancement, model);
        };

        service.RetourBrouillonAvancement = function (model) {
            return $http.post("/api/Budget/RetourBrouillonAvancement", model);
        };

        /**
         * Réalise l'export excel de l'avancement
         * @param {any} model Model décrivant : le CiId contenant le budget en application à exporter, la période à exporter
         * ainsi que le détail des lignes à afficher (l'export excel ne doit afficher que les lignes déroulées et visibles à l'écran)
         * @returns {any} une promesse telle que décrite dans la documentation de l'objet $Http, voir l'objet HttpPromise d'AngularJS
         */
        service.ExportExcelAvancement = function (model) {
            return $http.put("/api/Budget/Avancement/Excel", model);
        };

        service.LoadPeriodeRecalage = function (ciId) {
            return $http.get("/api/Budget/LoadPeriodeRecalage/" + ciId);
        };

        service.RecalageBudgetaire = function (budgetId, periodeFin) {
            return $http.post("/api/Budget/RecalageBudgetaire/" + budgetId + "/" + periodeFin);
        };

        service.CreateRessource = function (ressource, ciId) {
            return $http.post("/api/Budget/CreateRessource?ciId=" + ciId, ressource);
        };

        service.UpdateRessource = function (ressource, ciId) {
            return $http.put("/api/Budget/UpdateRessource?ciId=" + ciId, ressource);
        };

        service.GetBudgetRevisions = function (ciId) {
            return $http.get("/api/Budget/GetBudgetRevisions/" + ciId);
        };

        service.UpdateDateDeleteNotificationNewTask = function (budgetId) {
            return $http.post("/api/Budget/UpdateDateDeleteNotificationNewTask/" + budgetId);
        };

        service.CopySousDetails = function (model) {
            return $http.post("/api/Budget/CopySousDetails", model);
        };

        service.GetTache4Inutilisees = function (model) {
            return $http.post("/api/Budget/GetTache4Inutilisees", model);
        };


        //////////////////////////////////////////////////////////////////
        // Expor

        /**
         * Télécharge l'export à partir de son id
         * cette fonction doit être appelée après génération d'un export 
         * @param {any} guidIdExport, GUID de l'export, cette valeur est fournie par ExportExcelAvancement
         * @param {any} isPdf, flag de conversion en pdf
         * @param {any} fileName, nom du fichier à générer
         * */
        service.DownloadExportFile = function (guidIdExport, isPdf, fileName) {
            window.location.href = '/api/Budget/Export/' + guidIdExport + '/' + isPdf + '/' + fileName;
        };

        //////////////////////////////////////////////////////////////////
        // Bibliothèque de prix

        service.GetBibliothequePrix = function (organisationId, deviseId) {
            deviseId = deviseId === null ? 0 : deviseId;
            return $http.get("/api/Budget/GetBibliothequePrix/" + organisationId + "/" + deviseId);
        };

        service.GetBibliothequePrixForCopy = function (organisationId, deviseId) {
            return $http.get("/api/Budget/GetBibliothequePrixForCopy/" + organisationId + "/" + deviseId);
        };

        // Enregistre une bibliothèque des prix
        // - model : [BibliothequePrixSave.Model]
        service.SaveBibliothequePrix = function (model) {
            return $http.post("/api/Budget/SaveBibliothequePrix", model);
        };

        service.ApplyBibliothequePrixSurBudget = function (model) {
            return $http.post("/api/Budget/Brouilllon/BibliothequePrix", model);
        };

        service.OpenBibliothequePrixTab = function (organisationId, deviseId) {
            $window.open("/Budget/Budget/BibliothequePrix?organisationId=" + organisationId + "&deviseId=" + deviseId);
        };

        service.LoadBibliothequePrixItemHistorique = function (organisationId, deviseId, ressourceId) {
            return $http.get("/api/Budget/LoadBibliothequePrixItemHistorique/" + organisationId + "/" + deviseId + "/" + ressourceId);
        };

        service.BibliothequePrixExists = function (organisationId, deviseId) {
            return $http.get("/api/Budget/BibliothequePrixExists/" + organisationId + "/" + deviseId);
        };

        // Copie un budget source dans un budget cible
        // - model : modele de copie de budget [CopierBudget.Model]
        service.CopierBudget = function (model) {
            return $http.post("/api/Budget/Copier", model);
        };

        service.CheckPlanDeTacheIdentiques = function (ciId1, ciId2) {
            return $http.get("/api/Budget/CheckPlanDeTacheIdentiques/" + ciId1 + "/" + ciId2);
        };

        //////////////////////////////////////////////////////////////////

        /**
         * Sauvegarde la position de la scrollbar de la flex table avec l'id donné
         * @param {any} flexTableId l'id de la flex table sans le caractère #
         */
        service.SaveFlexTableScrollBarPos = function (flexTableId) {

            let fullQualifiedId = '#' + flexTableId;
            flexTableScrollbarPos = $(fullQualifiedId).scrollTop();
        };

        /**
         * Reset la position de la scrollbar de la flextable avec l'id donné avec la position sauvegardée par la fonction SaveFlexTableScrollBarPos
         * @param {any} flexTableId l'id de la flex table sans le caractère #
         */
        service.ResetFlexTableScrollBarPos = function (flexTableId) {

            let fullQualifiedId = '#' + flexTableId;
            $(fullQualifiedId).scrollTop(flexTableScrollbarPos);
        };

        return service;
    }
})();
