(function (angular) {
    'use strict';

    angular.module('Fred').controller('BibliothequePrixController', BibliothequePrixController);
    BibliothequePrixController.$inject = ['$scope', 'BibliothequePrixService', 'BudgetService', 'ProgressBar', 'Notify', 'fredDialog', '$timeout', 'BibliothequePrixValidator', 'StringFormat', 'OrganisationService', 'favorisService', '$window'];

    function BibliothequePrixController($scope, BibliothequePrixService, BudgetService, ProgressBar, Notify, fredDialog, $timeout, BibliothequePrixValidator, StringFormat, OrganisationService, favorisService, $window) {

        //////////////////////////////////////////////////////////////////
        // Membres                                                      //
        //////////////////////////////////////////////////////////////////
        var $ctrl = this;
        $ctrl.resources = resources;
        $ctrl.Loaded = false;
        $ctrl.Loading = false;
        $ctrl.Organisation = null;
        $ctrl.Devise = null;
        $ctrl.Organisations = null;
        $ctrl.SocieteId = null;
        $ctrl.Unites = null;
        $ctrl.Devises = null;
        $ctrl.View = {
            Organisation: null,
            Devise: null,
            Chapitres: null
        };
        $ctrl.Current = {
            Chapitres: null
        };
        $ctrl.Identifiers = {
            BibliothequePrixTable: "BIBLIOTHEQUE_PRIX_TABLE_ID",
            LookupUnite: "BIBLIOTHEQUE_PRIX_LOOKUP_UNITE_ID"
        };

        $ctrl.OnInit = OnInit;
        BudgetService.OnQuitPage = OnQuitPage;
        $ctrl.OnOrganisationChanged = OnOrganisationChanged;
        $ctrl.OnDeviseChanged = OnDeviseChanged;
        $ctrl.OnChapitreClick = OnChapitreClick;
        $ctrl.OnSousChapitreClick = OnSousChapitreClick;
        $ctrl.OnCancelButtonClick = OnCancelButtonClick;
        $ctrl.OnSaveButtonClick = OnSaveButtonClick;
        $ctrl.OnExpandAllButtonClick = OnExpandAllButtonClick;
        $ctrl.OnCollapseAllButtonClick = OnCollapseAllButtonClick;
        $ctrl.OnChapitreToChildrenClick = OnChapitreToChildrenClick;
        $ctrl.OnSousChapitreToChildrenClick = OnSousChapitreToChildrenClick;
        $ctrl.OnUniteClick = OnUniteClick;
        $ctrl.OnUniteReset = OnUniteReset;
        $ctrl.OnAddToFavoriteButtonClick = OnAddToFavoriteButtonClick;
        $ctrl.OnCopyButtonClick = OnCopyButtonClick;
        $ctrl.OnShowHistoriqueButtonClick = OnShowHistoriqueButtonClick;

        $ctrl.IsItemViewValid = IsItemViewValid;
        $ctrl.GetComposantesOriginaleMessage = GetComposantesOriginaleMessage;
        $ctrl.IsReadOnly = IsReadOnly;
        $ctrl.GetOrganisationClass = GetOrganisationClass;
        $ctrl.GetTypeOrganisationCodeToDisplay = GetTypeOrganisationCodeToDisplay;
        $ctrl.GetComposantesClass = GetComposantesClass;
        $ctrl.GetItemErrorMessage = GetItemErrorMessage;
        $ctrl.GetUniteToolTip = GetUniteToolTip;
        $ctrl.OrganisationCouranteEstCi = OrganisationCouranteEstCi;
        $ctrl.GetFavoriTooltip = GetFavoriTooltip;

        return $ctrl;

        //////////////////////////////////////////////////////////////////
        // Evènements internes                                          //
        //////////////////////////////////////////////////////////////////

        // Appelé au chargement de la page
        function OnInit(organisationId, favoriId, deviseId) {
            var load = function (organisationIdToLoad, deviseIdToLoad) {
                OrganisationService.getOrganisationByOrganisationId(organisationIdToLoad)
                    .then((response) => {
                        $ctrl.View.Organisation = response.data;
                        $ctrl.View.Devise = { DeviseId: deviseIdToLoad };
                        Load();
                    });
            };

            if (organisationId.length > 0) {
                organisationId = parseInt(organisationId);
                deviseId = parseInt(deviseId);

                if (!isFinite(deviseId)) {
                    deviseId = null;
                }

                if (isFinite(organisationId)) {
                    $ctrl.Loading = true;
                    load(organisationId, deviseId);
                }
            }
            else if (favoriId.length > 0) {
                favoriId = parseInt(favoriId);
                if (isFinite(favoriId)) {
                    $ctrl.Loading = true;
                    var defaultFilter = BibliothequePrixService.GetFavoriObj(0, 0);
                    favorisService.getFilterByFavoriIdOrDefault({ favoriId: favoriId, defaultFilter: defaultFilter })
                        .then(function (response) {
                            load(response.OrganisationId, response.DeviseId);
                        });
                }
            }
        }

        // Appelé lorsque la page va se fermer
        function OnQuitPage() {
            if (HasChanged()) {
                return true;
            }
        }

        // Appelé lorsque l'organisation a changé
        function OnOrganisationChanged() {
            // On ne fait rien si l'organisation est la même que celle déjà chargée
            if ($ctrl.View.Organisation && $ctrl.Organisation && $ctrl.View.Organisation.OrganisationId === $ctrl.Organisation.OrganisationId) {
                return;
            }

            if (HasChanged()) {
                fredDialog.confirmation($ctrl.resources.Budget_BibliothequePrix_ConfirmerModificationsPerdues, '', '', '', '',
                    function () {       // Valider
                        $ctrl.View.Devise = null;
                        Load();
                    },
                    function () {       // Annuler
                        $ctrl.View.Organisation = $ctrl.Organisation;
                    }
                );
            }
            else {
                $ctrl.View.Devise = null;
                Load();
            }
        }

        // Appelé lorsque la devise a changé
        function OnDeviseChanged() {
            // On ne fait rien si la devise est la même que celle déjà chargée
            if ($ctrl.View.Devise && $ctrl.Devise && $ctrl.View.Devise.DeviseId === $ctrl.Devise.DeviseId) {
                return;
            }

            if (HasChanged()) {
                fredDialog.confirmation($ctrl.resources.Budget_BibliothequePrix_ConfirmerModificationsPerdues, '', '', '', '',
                    function () {       // Valider
                        Load();
                    },
                    function () {       // Annuler
                        $ctrl.View.Devise = $ctrl.Devise;
                    }
                );
            }
            else {
                Load();
            }
        }

        // Appelé sur le clic d'un chapitre
        // - chapitre : le chapitre concerné.
        function OnChapitreClick(chapitre) {
            LoadSousChapitres(chapitre);
        }

        // Appelé sur le clic d'un sous-chapitre
        // - sousChapitre : le sous-chapitre concerné.
        function OnSousChapitreClick(sousChapitre) {
            LoadRessources(sousChapitre);
        }

        // Appelé sur le clic du bouton Annuler
        function OnCancelButtonClick() {
            if (!HasChanged()) {
                Notify.message($ctrl.resources.Budget_BibliothequePrix_Annulation_PasDeModification);
            }
            else {
                fredDialog.confirmation($ctrl.resources.Budget_BibliothequePrix_Annulation_Confirmation, '', '', '', '', function () {
                    for (let item of GetChanges()) {
                        item.Cancelled();
                    }
                    Notify.message($ctrl.resources.Budget_BibliothequePrix_Annulation_Effectue);
                });
            }
        }

        // Appelé sur le clic du bouton Enregistrer
        function OnSaveButtonClick() {
            TrySave();
        }

        // Appelé sur le clic du bouton Tout déplier
        // Permet de déplier tous les chapitres et sous-chapitres dans la vue
        function OnExpandAllButtonClick() {
            // Charge tous les éléments non encore chargés pour les afficher
            for (let chapitre of $ctrl.View.Chapitres) {
                if (!chapitre.SousChapitres) {
                    LoadSousChapitres(chapitre);
                }
                for (let sousChapitre of chapitre.SousChapitres) {
                    if (!sousChapitre.Ressources) {
                        LoadRessources(sousChapitre);
                    }
                }
            }

            // Ouvre les chapitres et sous-chapitres
            // Doit se faire après que tous les éléments soient créés, soit après le digest, d'où le $timeout
            $timeout(() => {
                for (let chapitre of $ctrl.View.Chapitres) {
                    $("#" + chapitre.View.ViewId).collapse('show');
                    for (let sousChapitre of chapitre.SousChapitres) {
                        $("#" + sousChapitre.View.ViewId).collapse('show');
                    }
                }
            }, 0, false);
        }

        // Appelé sur le clic du bouton Tout replier
        // Permet de replier tous les chapitres et sous-chapitres dans la vue
        function OnCollapseAllButtonClick() {
            for (let chapitre of $ctrl.View.Chapitres) {
                $("#" + chapitre.View.ViewId).collapse('hide');
                if (chapitre.SousChapitres) {
                    for (let sousChapitre of chapitre.SousChapitres) {
                        $("#" + sousChapitre.View.ViewId).collapse('hide');
                    }
                }
            }
        }

        // Appelé sur le clic du bouton "Appliquer aux enfants" sur un chapitre
        function OnChapitreToChildrenClick(viewChapitre) {
            var apply = function () {
                ChapitreToChildren(viewChapitre);
                viewChapitre.View.Prix = null;
                viewChapitre.View.Unite = null;
                Notify.message($ctrl.resources.Budget_BibliothequePrix_AppliquerAuxEnfants_Applique);

                // Ouvre les enfants concernés
                $timeout(() => {
                    $("#" + viewChapitre.View.ViewId).collapse('show');
                    for (let sousChapitre of viewChapitre.SousChapitres) {
                        $("#" + sousChapitre.View.ViewId).collapse('show');
                    }
                }, 0, false);
            };

            if (!ChapitreChildrenAreEmpty(viewChapitre)) {
                fredDialog.confirmation(
                    $ctrl.resources.Budget_BibliothequePrix_AppliquerAuxEnfants_NonVide_Confirmation_Message,
                    $ctrl.resources.Budget_BibliothequePrix_AppliquerAuxEnfants_NonVide_Confirmation_Titre,
                    '', '', '', apply
                );
            }
            else {
                apply();
            }
        }

        // Appelé sur le clic du bouton "Appliquer aux enfants" sur un sous-chapitre
        function OnSousChapitreToChildrenClick(viewSousChapitre) {
            var apply = function () {
                SousChapitreToChildren(viewSousChapitre);
                viewSousChapitre.View.Prix = null;
                viewSousChapitre.View.Unite = null;
                Notify.message($ctrl.resources.Budget_BibliothequePrix_AppliquerAuxEnfants_Applique);

                // Ouvre les enfants concernés
                $timeout(() => {
                    $("#" + viewSousChapitre.View.ViewId).collapse('show');
                }, 0, false);
            };

            if (!SousChapitreChildrenAreEmpty(viewSousChapitre)) {
                fredDialog.confirmation(
                    $ctrl.resources.Budget_BibliothequePrix_AppliquerAuxEnfants_NonVide_Confirmation_Message,
                    $ctrl.resources.Budget_BibliothequePrix_AppliquerAuxEnfants_NonVide_Confirmation_Titre,
                    '', '', '', apply
                );
            }
            else {
                apply();
            }
        }

        // Appelé sur le clic d'un "lookup" unité
        function OnUniteClick(item) {
            $ctrl.LookupUnite = item.View.Unite;
            $ctrl.LookupUniteSelected = () => {
                item.View.Unite = $ctrl.LookupUnite;
            };
            $("#" + $ctrl.Identifiers.LookupUnite)[0].Show();
        }

        // Appelé sur le clic du reset d'une unité
        function OnUniteReset(item) {
            item.View.Unite = null;
        }

        // Appelé sur le clic du bouton Ajouter aux favoris
        function OnAddToFavoriteButtonClick() {
            var url = $window.location.pathname;
            var favoriObj = BibliothequePrixService.GetFavoriObj($ctrl.Organisation.OrganisationId, $ctrl.Devise.DeviseId);
            favorisService.initializeAndOpenModal("BudgetBibliothequePrix", url, favoriObj);
        }

        // Appelé sur le clic du bouton Copier les valeurs
        function OnCopyButtonClick() {
            $scope.$broadcast(BudgetService.Events.BibliothequePrixDisplayCopierValeursDialog, {
                OrganisationId: $ctrl.Organisation.OrganisationId,
                DeviseId: $ctrl.Devise.DeviseId,
                EtablissementOrganisationId: $ctrl.Organisations[0].OrganisationId,
                OnValidate: function (data) { Copy(data); }
            });
        }

        // Appelé sur le clic du bouton Afficher l'historique
        function OnShowHistoriqueButtonClick(item) {
            ProgressBar.start();
            BudgetService.LoadBibliothequePrixItemHistorique($ctrl.Organisation.OrganisationId, $ctrl.Devise.DeviseId, item.Ressource.RessourceId)
                .then(function (response) {
                    ProgressBar.complete();
                    $scope.$broadcast(BudgetService.Events.DisplayHistoriqueBibliothequePrix, {
                        Item: item,
                        Histo: response.data.Histo
                    });
                })
                .catch(ProgressBar.complete);
        }


        //////////////////////////////////////////////////////////////////
        // Chargement                                                   //
        //////////////////////////////////////////////////////////////////
        function Load() {
            $ctrl.Loaded = false;
            $ctrl.Loading = true;

            ProgressBar.start(true);
            BudgetService.GetBibliothequePrix($ctrl.View.Organisation.OrganisationId, $ctrl.View.Devise ? $ctrl.View.Devise.DeviseId : null)
                .then(GetBibliothequePrixThen)
                .catch(GetBibliothequePrixCatch);
        }
        function GetBibliothequePrixThen(result) {
            var data = result.data;
            if (data.Erreur) {
                $ctrl.View.Organisation = $ctrl.Organisation;
                // Load()   ?
                $ctrl.View.Devise = $ctrl.Devise;
                GetBibliothequePrixFinally(false);
                Notify.error(data.Erreur);
            }
            else {
                $ctrl.Organisation = $ctrl.View.Organisation;
                $ctrl.Organisations = data.Organisations;
                $ctrl.SocieteId = data.SocieteId;
                $ctrl.Devises = data.Devises;
                $ctrl.Unites = data.Unites;

                // Devises
                for (let devise of $ctrl.Devises) {
                    if (devise.DeviseId === data.DeviseId) {
                        $ctrl.Devise = devise;
                        $ctrl.View.Devise = $ctrl.Devise;
                        break;
                    }
                }

                // Chapitres
                if ($ctrl.Current.Chapitres === null || !BibliothequePrixService.ReferentielsAreSame($ctrl.Current.Chapitres, data.Chapitres)) {
                    $ctrl.Current.Chapitres = data.Chapitres;
                    $ctrl.View.Chapitres = [];
                    for (var i = 0; i < data.Chapitres.length; i++) {
                        $ctrl.View.Chapitres.push(new Chapitre(data.Chapitres[i], i));
                    }
                    $("#" + $ctrl.Identifiers.BibliothequePrixTable)[0].Refresh();
                }
                else {
                    // Le référentiel n'a pas changé
                    // Met à jour les chapitres et sous-chapitres précédemment ouvert
                    for (let chapitre of $ctrl.View.Chapitres) {
                        chapitre.View.Prix = null;
                        chapitre.View.Unite = null;
                        if (chapitre.SousChapitres) {
                            for (let sousChapitre of chapitre.SousChapitres) {
                                sousChapitre.View.Prix = null;
                                sousChapitre.View.Unite = null;
                                if (sousChapitre.Ressources) {
                                    sousChapitre.Ressources = null;
                                    LoadRessources(sousChapitre);
                                }
                            }
                        }
                    }
                }

                GetBibliothequePrixFinally(true);
            }
        }
        function GetBibliothequePrixCatch() {
            Notify.error($ctrl.resources.Global_Notification_Error);
            GetBibliothequePrixFinally(false);
        }
        function GetBibliothequePrixFinally(loaded) {
            $ctrl.Loaded = loaded;
            $ctrl.Loading = false;
            ProgressBar.complete();
        }


        //////////////////////////////////////////////////////////////////
        // Chargement dynamique                                         //
        // Améliore de beaucoup le temps du digest                      //
        //////////////////////////////////////////////////////////////////

        // Charge les sous-chapitres d'un chapitre.
        // - chapitre : le chapitre concerné.
        // - return : true si les sous-chapitres ont été chargés, false s'ils le sont déjà.
        function LoadSousChapitres(viewChapitre) {
            var ret = false;
            if (!viewChapitre.SousChapitres) {
                ret = true;
                viewChapitre.SousChapitres = [];
                var currentChapitre = $ctrl.Current.Chapitres[viewChapitre.Index];
                for (var i = 0; i < currentChapitre.SousChapitres.length; i++) {
                    viewChapitre.SousChapitres.push(new SousChapitre(currentChapitre.SousChapitres[i], viewChapitre, i));
                }
            }
            return ret;
        }

        // Charge les ressources d'un sous-chapitre.
        // - sousChapitre : le sous-chapitre concerné.
        // - return : true si les ressources ont été chargés, false si elles le sont déjà.
        function LoadRessources(viewSousChapitre) {
            var ret = false;
            if (!viewSousChapitre.Ressources) {
                ret = true;
                viewSousChapitre.Ressources = [];
                var currentRessources = $ctrl.Current.Chapitres[viewSousChapitre.Chapitre.Index].SousChapitres[viewSousChapitre.Index].Ressources;
                for (let currentRessource of currentRessources) {
                    var viewRessource = new Ressource(currentRessource);
                    viewSousChapitre.Ressources.push(viewRessource);
                    for (let organisation of $ctrl.Organisations) {
                        var viewItem = new Item(organisation.OrganisationId, viewRessource);
                        for (let item of organisation.Items) {
                            if (item.RessourceId === currentRessource.RessourceId) {
                                viewItem.Original.Prix = item.Prix;
                                viewItem.Original.Unite = GetUnite(item.UniteId);
                                viewItem.DateModification = item.DateModification;
                                viewItem.DateCreation = item.DateCreation;
                                viewItem.View.Prix = viewItem.Original.Prix;
                                viewItem.View.Unite = viewItem.Original.Unite;
                                break;
                            }
                        }
                        viewRessource.Items.push(viewItem);
                    }
                }
            }
            return ret;
        }


        //////////////////////////////////////////////////////////////////
        // Enregistrement                                               //
        //////////////////////////////////////////////////////////////////
        function TrySave() {
            // On ne fait rien s'il n'y a pas eu de changement
            if (!HasChanged()) {
                Notify.message($ctrl.resources.Budget_BibliothequePrix_Enregistrement_PasDeModification);
                return;
            }

            // On ne fait rien s'il existe des erreurs
            // TODO : à revoir du coup : GetChangesArray & GetChangesSaveModel ?
            var changes = GetChangesArray();
            var model = GetChangesSaveModel(changes);
            if (!BibliothequePrixValidator.ValidateSaveModel(model)) {
                Notify.error($ctrl.resources.Budget_BibliothequePrix_ErreurEnregistrement);
                return;
            }

            ProgressBar.start(true);
            if ($ctrl.OrganisationCouranteEstCi()) {
                // L'organisation courante est un CI. Si des budgets brouillons existent pour ce CI
                // alors la boite de propagation va s'ouvrir, sinon on enregistre directement
                var ciId = $ctrl.Organisations[$ctrl.Organisations.length - 1].TargetId;
                BudgetService.GetBudgetsBrouillons(ciId, $ctrl.Devise.DeviseId)
                    .then(function (result) {
                        var budgetsBrouillons = result.data;
                        if (budgetsBrouillons.length === 0) {
                            Save(changes, model, null);
                        }
                        else {
                            ProgressBar.complete();
                            $scope.$broadcast(BudgetService.Events.BibliothequePrixEnregistrementSurCiDialog, {
                                BudgetsBrouillons: budgetsBrouillons,
                                OnContinue: function (arg) {
                                    Save(changes, model, {
                                        CiOrganisationId: $ctrl.Organisation.OrganisationId,
                                        DeviseId: $ctrl.Devise.DeviseId,
                                        BudgetIdAEnregistrer: arg.BudgetIdAEnregistrer,
                                        UpdateValeursSurLignesEnException: arg.UpdateValeursSurLignesEnException
                                    });
                                }
                            });
                        }
                    })
                    .catch(function () {
                        Notify.warning($ctrl.resources.Budget_BibliothequePrix_Save_RecuperationBudgetBrouillonErreurEnregistrementCompromisWarning);
                        // Essaye quand même d'enregistrer sans propager
                        Save(changes, model, null);
                    });
            }
            else {
                // L'organisation courante est un établissement, on enregistre directement
                Save(changes, model, null);
            }
        }

        // Sauvegarde la bibliotheque des prix
        // - changes : les changements
        // - model : le modèle d'enregistrement
        // - bibliothequePrixPropagationModel : contient les informations a transmettre a l'API pour la propagation de la bibliotheque des prix, null sinon
        function Save(changes, model, bibliothequePrixPropagationModel) {
            BudgetService.SaveBibliothequePrix(model)
                .then((response) => SaveBibliothequePrixThen(response, changes, bibliothequePrixPropagationModel))
                .catch(SaveBibliothequePrixCatch);
        }
        function SaveBibliothequePrixThen(response, changes, bibliothequePrixPropagationModel) {
            var currentItems = $ctrl.Organisations[$ctrl.Organisations.length - 1].Items;
            var toProcessItems = currentItems.slice();
            for (let change of changes) {
                var founded = false;
                for (let i = 0; i < toProcessItems.length; i++) {
                    var toProcessItem = toProcessItems[i];
                    if (toProcessItem.RessourceId === change.Ressource.RessourceId) {
                        toProcessItem.Prix = change.View.Prix;
                        toProcessItem.UniteId = change.View.Unite ? change.View.Unite.UniteId : null;
                        toProcessItems.splice(i, 1);
                        founded = true;
                        break;
                    }
                }
                if (!founded) {
                    currentItems.push({
                        RessourceId: change.Ressource.RessourceId,
                        Prix: change.View.Prix,
                        UniteId: change.View.Unite ? change.View.Unite.UniteId : null
                    });
                }

                change.Saved(response.data.DateSave);
            }
            Notify.message($ctrl.resources.Budget_BibliothequePrix_Enregistrement_Effectue);

            if (bibliothequePrixPropagationModel) {
                ApplyBibliothequePrixSurBudget(bibliothequePrixPropagationModel);
            }

            ProgressBar.complete();
        }
        function SaveBibliothequePrixCatch(error) {
            let message = $ctrl.resources.Global_Notification_Error;
            if (error && error.data && error.data.Message) {
                message = error.data.Message;
            }
            Notify.error(message);
            ProgressBar.complete();
        }

        function ApplyBibliothequePrixSurBudget(bibliothequePrixPropagationModel) {
            ProgressBar.start();
            BudgetService.ApplyBibliothequePrixSurBudget(bibliothequePrixPropagationModel)
                .then(ApplyBibliothequePrixSurBudgetThen)
                .catch(ApplyBibliothequePrixSurBudgetCatch)
                .finally(() => ProgressBar.complete());
        }
        function ApplyBibliothequePrixSurBudgetThen() {
            Notify.message($ctrl.resources.Budget_BibliothequePrix_PropagationBudgetBrouillonSuccess);
        }
        function ApplyBibliothequePrixSurBudgetCatch() {
            Notify.error($ctrl.resources.Budget_BibliothequePrix_PropagationBudgetBrouillonError);
        }


        //////////////////////////////////////////////////////////////////
        // Copie                                                        //
        //////////////////////////////////////////////////////////////////

        // Copie
        function Copy(data) {
            // Met à jour la liste des unités au cas où la source n'aurait pas les mêmes unités que la cible
            CopyUpdateUnites(data);

            // Annule tous les changements
            for (var item of GetChanges()) {
                item.Cancelled();
            }

            // Récupère la liste des items à mettre à jour
            var changes = CopyGetChanges(data);

            if (changes.length === 0) {
                // Il n'y a pas de changement
                Notify.message($ctrl.resources.Budget_BibliothequePrix_Copier_DonneedIdentiques);
            }
            else {
                // Applique les changements
                CopyApplyChanges(changes);
                Notify.message($ctrl.resources.Budget_BibliothequePrix_Copier_Effectuee);
            }
        }

        // Met à jour les unités (pour la copie)
        function CopyUpdateUnites(data) {
            for (let newUnite of data.Unites) {
                let newUniteFounded = false;
                for (let existingUnite of $ctrl.Unites) {
                    if (existingUnite.UniteId === newUnite.UniteId) {
                        newUniteFounded = true;
                        break;
                    }
                }
                if (!newUniteFounded) {
                    $ctrl.Unites.push(newUnite);
                }
            }
        }

        // Récupère la liste des items à mettre à jour (pour la copie)
        function CopyGetChanges(data) {
            //    Cas    Cible       Source        remarques
            //     1      5 €         7 FRT
            //     2      5 €         indéfinies
            //     3     indéfinies   7 FRT
            //     4     indéfinies   indéfinies          rien à faire dans ce cas

            let targetItems = $ctrl.Organisations[$ctrl.Organisations.length - 1].Items;
            let sourceItems = data.Items.slice();
            let changes = [];
            for (let targetItem of targetItems) {
                var founded = false;
                for (let sourceItemIndex = 0; sourceItemIndex < sourceItems.length; sourceItemIndex++) {
                    var sourceItem = sourceItems[sourceItemIndex];
                    if (targetItem.RessourceId === sourceItem.RessourceId) {
                        // Cas 1
                        founded = true;
                        if (targetItem.Prix !== sourceItem.Prix || targetItem.UniteId !== sourceItem.UniteId) {
                            changes.push(new RessourceToUpdate(sourceItem.RessourceId, sourceItem.Prix, sourceItem.UniteId));
                        }
                        sourceItems.splice(sourceItemIndex, 1);
                        break;
                    }
                }
                if (!founded) {
                    // Cas 2
                    if (targetItem.Prix !== null || targetItem.UniteId !== null) {
                        changes.push(new RessourceToUpdate(targetItem.RessourceId, null, null));
                    }
                }
            }
            for (let sourceItem of sourceItems) {
                // Cas 3
                if (sourceItem.Prix !== null || sourceItem.UniteId !== null) {
                    changes.push(new RessourceToUpdate(sourceItem.RessourceId, sourceItem.Prix, sourceItem.UniteId));
                }
            }
            return changes;
        }

        // Applique les changements (pour la copie)
        function CopyApplyChanges(changes) {
            for (let change of changes) {
                if (CopyUpdatePosition(change)) {
                    var chapitre = $ctrl.View.Chapitres[change.ChapitreIndex];
                    if (!chapitre.SousChapitres) {
                        LoadSousChapitres(chapitre);
                    }
                    var sousChapitre = chapitre.SousChapitres[change.SousChapitreIndex];
                    if (!sousChapitre.Ressources) {
                        LoadRessources(sousChapitre);
                    }
                    var ressource = sousChapitre.Ressources[change.RessourceIndex];
                    var item = ressource.Items[ressource.Items.length - 1];
                    item.View.Prix = change.Prix;
                    item.View.Unite = GetUnite(change.UniteId);
                }
            }
        }

        // Met à jour la position (chapitre, sous-chapitre et ressource) d'un changement (pour la copie)
        // - return : true si la position a été mise-à-jour, sinon false
        function CopyUpdatePosition(change) {
            for (let chapitreIndex = 0; chapitreIndex < $ctrl.Current.Chapitres.length; chapitreIndex++) {
                var chapitre = $ctrl.Current.Chapitres[chapitreIndex];
                for (let sousChapitreIndex = 0; sousChapitreIndex < chapitre.SousChapitres.length; sousChapitreIndex++) {
                    var sousChapitre = chapitre.SousChapitres[sousChapitreIndex];
                    for (let ressourceIndex = 0; ressourceIndex < sousChapitre.Ressources.length; ressourceIndex++) {
                        var ressource = sousChapitre.Ressources[ressourceIndex];
                        if (ressource.RessourceId === change.RessourceId) {
                            change.ChapitreIndex = chapitreIndex;
                            change.SousChapitreIndex = sousChapitreIndex;
                            change.RessourceIndex = ressourceIndex;
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        //////////////////////////////////////////////////////////////////
        // View                                                         //
        //////////////////////////////////////////////////////////////////

        // Indique si l'élément visuel à l'index indiqué est en lecture seul
        // - index : index concerné
        function IsReadOnly(index) {
            return index !== $ctrl.Organisations.length - 1;
        }

        // Retourne la class css à utiliser pour une organisation
        // - organisationIndex : l'index de l'organisation concernée
        // - return : la class à utiliser
        function GetOrganisationClass(organisationIndex) {
            var ret = organisationIndex % 2 === 0 ? "orange" : "pink";
            ret += IsReadOnly(organisationIndex) ? " readonly" : " editable";
            return ret;
        }

        // Récupère le code de l'organisation pour l'afficher.
        // - organisation : l'organisation concernée
        function GetTypeOrganisationCodeToDisplay(organisation) {
            var ret = "";
            var code = organisation.TypeCode;
            if (code.length > 0) {
                ret = code[0].toUpperCase();
                ret += code.substring(1, code.length).toLowerCase();
            }
            return ret;
        }

        // Retourne la classe css à utiliser pour les composantes
        // - item : l'item concerné
        // - return : la class à utiliser
        function GetComposantesClass(item) {
            return !item.Errors || item.Errors.length === 0 ? "" : " error";
        }

        // Retourne le message d'erreur d'un item de ressource
        // - item : l'item concerné
        // - return : le message
        function GetItemErrorMessage(item) {
            var message = "";
            if (item.Errors) {
                for (var i = 0; i < item.Errors.length; i++) {
                    if (i > 0) {
                        message += '\r\n';
                    }
                    message += item.Errors[i];
                }
            }
            return message;
        }

        // Retourne le tooltip de l'unité d'un élément de la vue
        // - item : l'élément de la vue
        function GetUniteToolTip(item) {
            return item.View.Unite
                ? item.View.Unite.Code + '-' + item.View.Unite.Libelle
                : "";
        }

        // Retourne le tooltip du bouton Ajouter aux favoris
        function GetFavoriTooltip() {
            if ($ctrl.Organisation) {
                var format = OrganisationCouranteEstCi()
                    ? $ctrl.resources.Budget_BibliothequePrix_Favoris_CI_ButtonTooltip
                    : $ctrl.resources.Budget_BibliothequePrix_Favoris_Etablissement_ButtonTooltip;
                return StringFormat.Format(
                    format,
                    $ctrl.Organisation.CodeLibelle,
                    $ctrl.Devise.Symbole,
                    $ctrl.Devise.Libelle
                );
            }
            return '';
        }

        //////////////////////////////////////////////////////////////////
        // Appliquer aux enfants                                        //
        //////////////////////////////////////////////////////////////////
        function ChapitreChildrenAreEmpty(viewChapitre) {
            if (!viewChapitre.SousChapitres) {
                LoadSousChapitres(viewChapitre);
            }
            for (let viewSousChapitre of viewChapitre.SousChapitres) {
                if (!SousChapitreChildrenAreEmpty(viewSousChapitre)) {
                    return false;
                }
            }
            return true;
        }
        function SousChapitreChildrenAreEmpty(viewSousChapitre) {
            if (!viewSousChapitre.Ressources) {
                LoadRessources(viewSousChapitre);
            }
            for (let viewRessource of viewSousChapitre.Ressources) {
                var viewItem = viewRessource.Items[$ctrl.Organisations.length - 1];
                if (viewItem.View.Prix !== null || viewItem.View.Unite !== null) {
                    return false;
                }
            }
            return true;
        }
        function ChapitreToChildren(chapitre) {
            if (!chapitre.SousChapitres) {
                LoadSousChapitres(chapitre);
            }
            for (let sousChapitre of chapitre.SousChapitres) {
                SousChapitreToItems(sousChapitre, chapitre.View);
            }
        }
        function SousChapitreToChildren(sousChapitre) {
            SousChapitreToItems(sousChapitre, sousChapitre.View);
        }
        function SousChapitreToItems(sousChapitre, view) {
            if (!sousChapitre.Ressources) {
                LoadRessources(sousChapitre);
            }
            for (let ressource of sousChapitre.Ressources) {
                var item = ressource.Items[$ctrl.Organisations.length - 1];
                item.View.Prix = view.Prix;
                item.View.Unite = view.Unite;
            }
        }


        //////////////////////////////////////////////////////////////////
        // Autre                                                        //
        //////////////////////////////////////////////////////////////////
        function IsItemViewValid(itemView) {
            return BibliothequePrixValidator.ValidatePrixUniteCoherence({
                Prix: itemView.Prix,
                UniteId: itemView.Unite ? itemView.Unite.UniteId : null
            });
        }

        // Retourne un message contenant les valeurs originales d'un item
        function GetComposantesOriginaleMessage(item) {
            return StringFormat.Format(
                $ctrl.resources.Budget_BibliothequePrix_ComposanteOriginale_Tooltip,
                item.Original.Prix ? item.Original.Prix : $ctrl.resources.Budget_BibliothequePrix_ComposanteOriginale_Tooltip_NonRenseigne,
                item.Original.Unite ? item.Original.Unite.Code + '-' + item.Original.Unite.Libelle : $ctrl.resources.Budget_BibliothequePrix_ComposanteOriginale_Tooltip_NonRenseignee
            );
        }

        // Indique si l'utilisateur a effectué des changements
        function HasChanged() {
            var enumerator = GetChanges();
            var next = enumerator.next();
            return !next.done;
        }

        // Enumère les changements effectués par l'utilisateur
        function* GetChanges() {
            if ($ctrl.View.Chapitres) {
                for (let chapitre of $ctrl.View.Chapitres) {
                    if (chapitre.SousChapitres) {
                        for (let sousChapitre of chapitre.SousChapitres) {
                            if (sousChapitre.Ressources) {
                                for (let ressource of sousChapitre.Ressources) {
                                    var item = ressource.Items[$ctrl.Organisations.length - 1];
                                    if (item.HasChanged()) {
                                        yield item;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // Retourne un tableau contenant les changements effectués par l'utilisateur
        function GetChangesArray() {
            let changes = [];
            for (let item of GetChanges()) {
                changes.push(item);
            }
            return changes;
        }

        // Retourne le modèle d'enregistrement des changements
        // - changes : les changements concernés
        // - return : un [BibliothequePrixSave.Model]
        function GetChangesSaveModel(changes) {
            var model = {
                OrganisationId: $ctrl.Organisation.OrganisationId,
                DeviseId: $ctrl.Devise.DeviseId,
                Items: []
            };

            for (let item of changes) {
                model.Items.push({
                    RessourceId: item.Ressource.RessourceId,
                    Prix: item.View.Prix,
                    UniteId: item.View.Unite ? item.View.Unite.UniteId : null
                });
            }

            return model;
        }

        // Retourne une unité en fonction de son identifiant ou null si l'unité n'existe pas
        function GetUnite(uniteId) {
            if (uniteId !== null && uniteId !== 0) {
                for (let unite of $ctrl.Unites) {
                    if (unite.UniteId === uniteId) {
                        return unite;
                    }
                }
            }
            return null;
        }

        // Indique si l'organisation courante est un CI
        function OrganisationCouranteEstCi() {
            return $ctrl.Organisation && $ctrl.Organisation.TypeOrganisationId === OrganisationService.OrganisationType.Ci && $ctrl.Organisations !== null;
        }
    }
}(angular));
