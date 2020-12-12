(function (angular) {
    'use strict';

    angular.module('Fred').controller('BudgetDetailController', BudgetDetailController);

    BudgetDetailController.$inject = ['$scope', '$uibModal', 'ProgressBar', 'BudgetService', 'BudgetDetailService', 'BudgetDetailCalculator', 'Notify', '$timeout', 'confirmDialog', 'fredSubscribeService', 'CiManagerService', '$window', 'favorisService', 'fredDialog'];

    function BudgetDetailController($scope, $uibModal, ProgressBar, BudgetService, BudgetDetailService, BudgetDetailCalculator, Notify, $timeout, confirmDialog, fredSubscribeService, CiManagerService, $window, favorisService, fredDialog) {
        var $ctrl = this;
        var FRED_MENU_GO_BACK_EVENT_NAME = 'goBack';

        //////////////////////////////////////////////////////////////////
        // Membres publiques                                            //
        //////////////////////////////////////////////////////////////////
        $ctrl.DEBUG = DEBUG;
        $ctrl.resources = resources;
        $ctrl.BudgetService = BudgetService;
        $ctrl.BudgetDetailService = BudgetDetailService;
        $ctrl.Budget = null;
        $ctrl.SousDetail = null;
        $ctrl.Erreurs = "";
        $ctrl.libelleBoutonValider = "Valider";
        $ctrl.Titre = $ctrl.resources.Budget_Detail_Titre;
        $ctrl.CodeBudgetEtat = BudgetService.CodeBudgetEtat;
        $ctrl.LoadDetail = LoadDetail;
        $ctrl.LoadSousDetail = LoadSousDetail;
        $ctrl.CancelDetail = CancelDetail;
        $ctrl.SaveDetail = SaveDetail;
        $ctrl.DisplayValidationDialog = DisplayValidationDialog;
        $ctrl.DisplayReturnDraftDialog = DisplayReturnDraftDialog;
        $ctrl.CancelSousDetail = CancelSousDetail;
        $ctrl.SaveSousDetail = SaveSousDetail;
        $ctrl.OpenBibliothequePrix = OpenBibliothequePrix;
        $ctrl.GetDisplayButtonClass = GetDisplayButtonClass;
        $ctrl.GetClassePourEtatbudget = GetClassePourEtatbudget;
        $ctrl.ShowSousDetailVueSD = ShowSousDetailVueSD;
        $ctrl.ShowSousDetailVueT4 = ShowSousDetailVueT4;
        $ctrl.DeleteNotificationNewTask = DeleteNotificationNewTask;
        $ctrl.DisplayExportDialog = DisplayExportDialog;
        $ctrl.onExportExcel = onExportExcel;
        $ctrl.onDisabledTasksButtonClicked = onDisabledTasksButtonClicked;
        $ctrl.ShowPanelDisplayCustomization = ShowPanelDisplayCustomization;
        $ctrl.OnValidate = OnValidate;
        $ctrl.OnCopyBudgetButtonClick = OnCopyBudgetButtonClick;
        $ctrl.CanCopyBudget = CanCopyBudget;
        $ctrl.getFilterOrFavoris = getFilterOrFavoris;
        $ctrl.addFilter2Favoris = addFilter2Favoris;
        $ctrl.PermissionKeys = PERMISSION_KEYS;

        $ctrl.filters = {
            NiveauxVisible: [1, 2, 3, 4],
            ColumnsVisible: [$ctrl.BudgetService.DetailColumnEnum.Unite, $ctrl.BudgetService.DetailColumnEnum.Quantite, $ctrl.BudgetService.DetailColumnEnum.PU],
            IsDisplayCustomized: false,
            DisabledTasksDisplayed: false
        };

        //Surcharge de la fonction a appeler lorsque la page est déchargée
        BudgetService.OnQuitPage = onQuitDetailOrSousDetail;

        //////////////////////////////////////////////////////////////////
        // Initialisation                                               //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {
            // Ajoute le menu "go back" au menu de FRED
            fredSubscribeService.subscribe({ eventName: FRED_MENU_GO_BACK_EVENT_NAME, callback: OnFredMenuGoBack, tooltip: $ctrl.resources.Budget_Detail_Tooltip_GoBackButton });

            $scope.$on(BudgetService.Events.LoadDetail, function (event, arg) { LoadDetail(arg.budgetId); });
            $scope.$on(BudgetService.Events.LoadSousDetail, function (event, arg) { LoadSousDetailCheck(arg.Tache4); });
            $scope.$on(BudgetService.Events.ValidateDetail, function (event, arg) { onValidateDetail(); });
            $scope.$on(BudgetService.Events.PanelCustomizeDisplayModified, function (event, args) { onCustomizeDisplay(args); });
        };


        //////////////////////////////////////////////////////////////////
        // Menu "go back" de FRED                                       //
        //////////////////////////////////////////////////////////////////

        // Met à jour le menu "go back" de FRED en fonction de la vue active (détail ou sous-détail)
        function UpdateFredMenuGoBack() {
            if (BudgetDetailService.ViewMode === BudgetDetailService.ViewModeType.Detail) {
                $('#FRED_MENU_GO_BACK').prop('title', $ctrl.resources.Budget_Detail_Tooltip_GoBackButton);
                $scope.$broadcast(BudgetService.Events.DisplayDetail);
            }
            else if (BudgetDetailService.ViewMode === BudgetDetailService.ViewModeType.SousDetail) {
                $('#FRED_MENU_GO_BACK').prop('title', $ctrl.resources.Budget_SousDetail_Tooltip_GoBackButton);
            }
        }

        // Appelé après un clic sur le bouton "go back" du menu de FRED
        function OnFredMenuGoBack() {

            //Si le budget T4 est readonly alors pas besoin de vérifier si il y a un changement puisqu'il ne peut pas y en avoir
            if (BudgetDetailService.ViewMode === BudgetDetailService.ViewModeType.SousDetail) {
                if ($ctrl.SousDetail.HasChanged() && !($ctrl.SousDetail.BudgetT4.Tache.Code.indexOf('T4REV') > -1)) {
                    confirmDialog.confirmWithOption(resources,
                        $ctrl.resources.Budget_SousDetail_RevenirDetailSansEnregistrer,
                        $ctrl.resources.Global_Continuer_Sans_Enregistrer,
                        $ctrl.resources.Global_Bouton_Enregistrer,
                        $ctrl.resources.Global_Bouton_Rester)
                        .then((arg) => {

                            if (arg === true) {
                                //Cela signifie qu'on a cliqué sur le bouton Enregistrer, l'utilisateur veut enregistrer son travail avant de quitter
                                $scope.$broadcast(BudgetService.Events.SaveSousDetail, function (saveSuccessful) {
                                    if (saveSuccessful === true) {
                                        GoBackToDetail();
                                    }
                                });
                            }
                            else if (arg.option === true) {
                                //cela veut dire qu'on a cliqué sur le bouton Continuer Sans enregistrer
                                $ctrl.SousDetail.BudgetT4.ResetForSousDetail();
                                GoBackToDetail();
                            }

                        });
                }
                else {
                    GoBackToDetail();
                }
            }
            else if (BudgetDetailService.ViewMode === BudgetDetailService.ViewModeType.Detail) {

                //Pas besoin de vérifier ici, la vérification se fera au déchargement de la page HTML par la fonction du BudgetService onQuitPage
                ShowBudgetListe();
            }
        }

        function ShowBudgetListe() {
            if ($ctrl.Budget && $ctrl.Budget.CI && $ctrl.Budget.CI.CiId) {
                BudgetService.ShowBudgetListe($ctrl.Budget.CI.CiId);
            }
            else {
                // Peut se produire si l'utilisateur demande un sous-détail d'un budget qui n'existe pas ou plus sans passer par la liste des budgets
                // Ex : un favori dans son navigateur sur un budget supprimé
                BudgetService.ShowBudgetListe();
            }

        }

        function onQuitDetailOrSousDetail(/*onQuitPageEventArg*/) {

            //Si le budget T4 est readonly alors pas besoin de vérifier si il y a un changement puisqu'il ne peut pas y en avoir
            if (BudgetDetailService.ViewMode === BudgetDetailService.ViewModeType.SousDetail) {

                //Ici on est dans un cas particulier, on PEUT PAS être ici après un clic sur le bouton GoBack de Fred car l'écran du détail et du SousDétail 
                //Sont sur la même page de passer de l'un à l'autre n'entraine pas de déchargement de l'HTML et donc d'appel à la fonction onQuitPage.
                if ($ctrl.SousDetail.HasChanged() && !($ctrl.SousDetail.BudgetT4.Tache.Code.indexOf('T4REV') > -1)) {
                    return true;
                }
            }
            else if (BudgetDetailService.ViewMode === BudgetDetailService.ViewModeType.Detail) {
                if ($ctrl.Budget.DetailHasChanged()) {
                    return true;
                }

            }
        }

        // Retourne au détail
        function GoBackToDetail() {
            $ctrl.SousDetail = null;
            BudgetDetailService.ViewMode = BudgetDetailService.ViewModeType.Detail;
            $ctrl.Titre = $ctrl.resources.Budget_Detail_Titre;
            BudgetDetailCalculator.Calculate($ctrl.Budget);
            $timeout(UpdateFredMenuGoBack);
        }


        //////////////////////////////////////////////////////////////////
        // Chargement du détail                                         //
        //////////////////////////////////////////////////////////////////
        function LoadDetail(budgetId, copy) {
            $ctrl.Budget = null;
            BudgetDetailService.ViewMode = BudgetDetailService.ViewModeType.None;
            ProgressBar.start();
            BudgetService.GetDetail(budgetId)
                .then(function (response) { GetDetailThen(response, copy); })
                .catch(GetDetailCatch);
        }
        function GetDetailThen(result, copy) {
            BudgetDetailService.ViewMode = BudgetDetailService.ViewModeType.Detail;
            $ctrl.Budget = result.data;

            // Le budget doit être initialisé dans tous les cas
            BudgetDetailService.InitialiseBudget($ctrl.Budget);

            if ($ctrl.Budget.Erreur) {
                Notify.error($ctrl.Budget.Erreur);
            }
            else {
                CiManagerService.setCi($ctrl.Budget.CI);

                // Avertissement
                if ($ctrl.Budget.Avertissement) {
                    Notify.error($ctrl.Budget.Avertissement);
                }

                // Devises
                for (let devise of $ctrl.Budget.CI.Devises) {
                    if (devise.DeviseId === $ctrl.Budget.DeviseId) {
                        devise.Active = true;
                        $ctrl.Budget.Devise = devise;
                    }
                    else {
                        devise.Active = false;
                    }
                }

                // Lecture seule
                $ctrl.Budget.Readonly = $ctrl.Budget.Etat.Code !== BudgetService.CodeBudgetEtat.Brouillon;

                // Initialise les tâches
                BudgetDetailService.InitialiseTaches($ctrl.Budget, false);

                BudgetDetailCalculator.Calculate($ctrl.Budget);
                CalculLibelleBoutonValider();

                // Marqueur jaune sur les tâches copiées
                for (let tache4 of $ctrl.Budget.GetTaches4()) {
                    tache4.Copied = false;
                }
                if (copy) {
                    for (let i = copy.Tache4IdCopies.length; i-- > 0;) {
                        var tache4Id = copy.Tache4IdCopies[i];
                        for (let tache4 of $ctrl.Budget.GetTaches4()) {
                            if (tache4.TacheId === tache4Id) {
                                copy.Tache4IdCopies.splice(i, 1);
                                tache4.Copied = true;
                                break;
                            }
                        }
                    }
                }
            }

            ProgressBar.complete();
        }
        function GetDetailCatch() {
            Notify.error($ctrl.resources._Budget_Erreur_Chargement);
            ProgressBar.complete();
        }


        //////////////////////////////////////////////////////////////////
        // Chargement du sous-détail                                    //
        //////////////////////////////////////////////////////////////////
        function LoadSousDetailCheck(tache4) {

            if (!BudgetService.OnQuitPage() || BudgetDetailService.ViewMode === BudgetDetailService.ViewModeType.Detail) {
                LoadSousDetail(tache4);
                return;
            }

            confirmDialog.confirmWithOption(resources,
                $ctrl.resources.Budget_SousDetail_RevenirDetailSansEnregistrer,
                $ctrl.resources.Global_Continuer_Sans_Enregistrer,
                $ctrl.resources.Global_Bouton_Enregistrer,
                $ctrl.resources.Global_Bouton_Rester)
                .then((arg) => {
                    if (arg === true) {
                        //Cela signifie qu'on a cliqué sur le bouton Enregistrer, l'utilisateur veut enregistrer son travail avant de quitter
                        $scope.$broadcast(BudgetService.Events.SaveSousDetail, function (saveSuccessful) {
                            if (saveSuccessful === true) {
                                LoadSousDetail(tache4);
                            }
                        });
                    }
                    else if (arg.option === true) {
                        //cela veut dire qu'on a cliqué sur le bouton Continuer Sans enregistrer
                        $ctrl.SousDetail.BudgetT4.ResetForSousDetail();
                        LoadSousDetail(tache4);
                    }
                });
        }

        function LoadSousDetail(tache4) {
            ProgressBar.start();

            BudgetDetailService.ViewMode = BudgetDetailService.ViewModeType.None;
            $ctrl.SousDetail = null;

            // Pas besoin de faire un appel serveur si le budget T4 n'existe pas
            if (tache4.BudgetT4.BudgetT4Id === 0) {
                $timeout(() => {
                    // Simule un [BudgetSousDetailLoad.Model]
                    var sousDetail = {
                        Erreur: null,
                        BudgetT4Id: 0,
                        Items: []
                    };

                    LoadSousDetailThen(sousDetail, tache4);
                });
            }
            else {
                BudgetService.GetSousDetail($ctrl.Budget.CI.CiId, tache4.BudgetT4.BudgetT4Id)
                    .then((result) => LoadSousDetailThen(result.data, tache4))
                    .catch(LoadSousDetailCatch);
            }
        }

        function LoadSousDetailThen(sousDetail, tache4) {
            BudgetDetailService.ViewMode = BudgetDetailService.ViewModeType.SousDetail;

            // Recalcul de la quantité et du montant en vue SD
            if (tache4.BudgetT4.VueSD === 1) {
                for (let sousDetailItem of sousDetail.Items) {
                    if (tache4.BudgetT4.QuantiteDeBase === null) {
                        sousDetailItem.Quantite = 0;
                    } else {
                    sousDetailItem.Quantite = sousDetailItem.QuantiteSD *
                        tache4.BudgetT4.QuantiteARealiser /
                        tache4.BudgetT4.QuantiteDeBase;
                    sousDetailItem.Montant = sousDetailItem.Quantite * sousDetailItem.PrixUnitaire;
                    }
                }
            }

            if (sousDetail.Erreur) {
                Notify.error(sousDetail.Erreur);
            }
            else {
                BudgetDetailService.ResetSousDetailItemViewIdCounter();
                for (let sousDetailItem of sousDetail.Items)
                    BudgetDetailService.InitialiseSousDetailItem(sousDetailItem, sousDetail);
                BudgetDetailService.InitialiseSousDetail(sousDetail, tache4.BudgetT4, $ctrl.Budget);

                $ctrl.SousDetail = sousDetail;

                $ctrl.Titre = $ctrl.resources.Budget_SousDetail_Titre;
                UpdateFredMenuGoBack();

                BudgetDetailService.ProcessSousDetail(sousDetail, false);
            }
            // flag la tache en edition
            $ctrl.Budget.EditTache(tache4);
            ProgressBar.complete();
        }

        function LoadSousDetailCatch() {
            Notify.error($ctrl.resources._Budget_Erreur_Chargement);
            ProgressBar.complete();
        }


        //////////////////////////////////////////////////////////////////
        // Actions                                                      //
        //////////////////////////////////////////////////////////////////
        function CancelDetail() {
            if ($ctrl.Budget.DetailHasChanged()) {
                confirmDialog.confirm(resources, $ctrl.resources._Budget_ConfirmationAnnulation, "flaticon flaticon-warning").then(function () {
                    // Recharge le détail du budget
                    LoadDetail($ctrl.Budget.BudgetId);
                });
            }
            else {
                Notify.message($ctrl.resources._Budget_PasDeModificationAAnnuler);
            }
        }

        function onDisabledTasksButtonClicked() {
            $ctrl.filters.DisabledTasksDisplayed = !$ctrl.filters.DisabledTasksDisplayed;
        }

        function DisplayExportDialog() {
            var modalInstance = $uibModal.open({
                animation: true,
                component: 'budgetDetailExportPopupComponent',
                resolve: {
                    resources: function () { return $ctrl.resources; }
                }
            });

            modalInstance.result.then(function (templateName) {
                onExportExcel(templateName, false);
            });
        }

        function onExportExcel(templateName, isPdfConverted) {
            ProgressBar.start();
            BudgetService.BudgetDetailsExportExcel($ctrl.Budget.BudgetId, templateName, isPdfConverted, $ctrl.filters.DisabledTasksDisplayed, $ctrl.filters.NiveauxVisible)
                .then(response => ExportExcelThen(response, isPdfConverted))
                .finally(ExportExcelFinally);
        }

        function ExportExcelThen(response, isPdf) {
            BudgetService.DownloadExportFile(response.data.id, isPdf, 'DetailsBudgetaire');
        }

        function ExportExcelFinally() {
            ProgressBar.complete();
        }

        function SaveDetail() {
            $scope.$broadcast(BudgetService.Events.SaveDetail, { continueWithValidation: false });
        }

        function DisplayValidationDialog() {
            //LA fonction de sauvegarde appellera elle même l'evenement de validation si la sauvegarde réussi
            $scope.$broadcast(BudgetService.Events.SaveDetail, { continueWithValidation: true });
        }

        function DisplayReturnDraftDialog() {
            $scope.$broadcast(BudgetService.Events.DisplayReturnDraftDialog);
        }

        function CancelSousDetail() {
            if ($ctrl.SousDetail.HasChanged()) {
                confirmDialog.confirm(resources, $ctrl.resources._Budget_ConfirmationAnnulation, "flaticon flaticon-warning")
                    .then(() => {
                        // Recharge le sous-détail du budget
                        $ctrl.SousDetail.BudgetT4.ResetForSousDetail();
                        $ctrl.Budget.Montant = $ctrl.SousDetail.MontantBudgetOriginal;
                        LoadSousDetail($ctrl.SousDetail.BudgetT4.Tache);
                    });
            }
            else {
                Notify.message($ctrl.resources._Budget_PasDeModificationAAnnuler);
            }
        }

        function SaveSousDetail() {
            $scope.$broadcast(BudgetService.Events.SaveSousDetail);
        }

        function OpenBibliothequePrix() {
            BudgetService.OpenBibliothequePrixTab($ctrl.Budget.CI.OrganisationId, $ctrl.Budget.DeviseId);
        }

        function DeleteNotificationNewTask() {
            ProgressBar.start();
            BudgetService.UpdateDateDeleteNotificationNewTask($ctrl.Budget.BudgetId)
                .then(() => DeleteNewTaskWarnings())
                .finally(ProgressBar.complete());
        }

        function OnCopyBudgetButtonClick() {
            // Il ne doit pas exister de modification non enregistrées
            if ($ctrl.Budget.DetailHasChanged()) {
                fredDialog.information($ctrl.resources.CopierBudgetConfirm_ModificationsNonEnregistrees, $ctrl.resources.CopierBudget_Titre, "flaticon flaticon-shuffle-1", $ctrl.resources.Global_Bouton_Ok);
                return;
            }

            // Ouvre la fenêtre de copie
            $scope.$broadcast(BudgetService.Events.DisplayDialogToCopyBudget, {
                Budget: $ctrl.Budget,
                OnValidate: function (argCopier) {
                    // Ouvre la fenêtre de confirmation de copie
                    $scope.$broadcast(BudgetService.Events.DisplayDialogToConfirmCopyBudget, {
                        BudgetCible: $ctrl.Budget,
                        BudgetSource: argCopier.BudgetSource,
                        Bibliotheque: argCopier.Bibliotheque,
                        OnCancel: function () {
                            argCopier.Show();
                        },
                        OnValidate: function (argConfirm) {
                            // Lance la copie
                            ProgressBar.start(true);
                            var model = {
                                DeviseId: $ctrl.Budget.DeviseId,
                                BudgetCibleId: $ctrl.Budget.BudgetId,
                                BudgetCibleCiId: $ctrl.Budget.CI.CiId,
                                BudgetSourceCiId: argCopier.BudgetSource.CI.CiId,
                                BudgetSourceRevision: argCopier.BudgetSource.Revision.Revision,
                                BibliothequePrixOrganisationId: argCopier.Bibliotheque.OrganisationId,
                                ComposantesDuBudgetSource: argConfirm.ComposantesDuBudgetSource,
                                OnlyLignesVides: argConfirm.OnlyLignesVides,
                                IncludeRessourceSpecifiques: argConfirm.IncludeRessourceSpecifiques
                            };
                            BudgetService.CopierBudget(model)
                                .then(function (response) {
                                    ProgressBar.complete();
                                    if (response.data.Tache4IdCopies.length === 0) {
                                        Notify.message($ctrl.resources.CopierBudget_CopieEffectuee_SansChangement);
                                    }
                                    else {
                                        Notify.message($ctrl.resources.CopierBudget_CopieEffectuee);
                                    }
                                    $timeout(function () { LoadDetail($ctrl.Budget.BudgetId, response.data); });
                                })
                                .catch(function (response) {
                                    ProgressBar.complete();
                                    if (response && response.data) {
                                        Notify.error(response.data);
                                    }
                                    else {
                                        Notify.error($ctrl.resources.Global_Notification_Error);
                                    }
                                });
                        }
                    });
                }
            });
        }


        //////////////////////////////////////////////////////////////////
        // Autre                                                        //
        //////////////////////////////////////////////////////////////////
        function GetDisplayButtonClass(sousDetailViewModeType) {
            var ret = $ctrl.Budget && $ctrl.Budget.Readonly ? "item-disabled" : "";
            ret += sousDetailViewModeType === $ctrl.SousDetail.BudgetT4.View.VueSD ? " selected" : "";
            return ret;
        }

        function GetClassePourEtatbudget() {
            switch ($ctrl.Budget.Etat.Code) {
                case $ctrl.CodeBudgetEtat.Brouillon:
                    return "draft";
                case $ctrl.CodeBudgetEtat.EnApplication:
                    return "application";
                case $ctrl.CodeBudgetEtat.Archive:
                    return "archived";
                default:
                    return "draft";
            }
        }

        function CanCopyBudget() {
            return $ctrl.Budget && $ctrl.Budget.Etat.Code === $ctrl.CodeBudgetEtat.Brouillon;
        }

        function ShowSousDetailVueSD() {
            if ($ctrl.Budget && $ctrl.Budget.Readonly) {
                return;
            }

            $ctrl.SousDetail.BudgetT4.View.VueSD = BudgetDetailService.SousDetailViewModeType.SD;
            BudgetDetailService.ProcessSousDetail($ctrl.SousDetail, true);
            $scope.$broadcast(BudgetService.Events.SousDetailVueChanged);
        }

        function ShowSousDetailVueT4() {
            if ($ctrl.Budget && $ctrl.Budget.Readonly) {
                return;
            }

            $ctrl.SousDetail.BudgetT4.View.VueSD = BudgetDetailService.SousDetailViewModeType.T4;
            BudgetDetailService.ProcessSousDetail($ctrl.SousDetail, true);
            $scope.$broadcast(BudgetService.Events.SousDetailVueChanged);
        }

        function CalculLibelleBoutonValider() {
            if ($ctrl.Budget.Etat.Code === $ctrl.CodeBudgetEtat.Brouillon) {
                $ctrl.libelleBoutonValider = "Demander une validation";
            }
            else if ($ctrl.Budget.Etat.Code === $ctrl.CodeBudgetEtat.AValider) {
                $ctrl.libelleBoutonValider = $ctrl.resources.Budget_Detail_ToolBar_Action_Valider;
            }
        }

        function ShowPanelDisplayCustomization() {
            $scope.$broadcast(BudgetService.Events.OpenPanelCustomizeDisplay);
        }

        function onCustomizeDisplay(args) {
            $ctrl.filters.IsDisplayCustomized = args.IsDisplayCustomized;
            if (args.SaveSession) {
                $ctrl.filters.ColumnsVisible = args.ColumnsVisible;
                $ctrl.filters.NiveauxVisible = args.NiveauxVisible;
                sessionStorage.setItem('budgetDetailFilter', JSON.stringify($ctrl.filters));
            }
        }

        function DeleteNewTaskWarnings() {
            $ctrl.Budget.IsNewTaskNotification = false;
            for (let tache1 of $ctrl.Budget.Taches1) {
                tache1.Warnings = null;
                for (let tache2 of tache1.Taches2) {
                    tache2.Warnings = null;
                    for (let tache3 of tache2.Taches3) {
                        tache3.Warnings = null;
                    }
                }
            }
        }

        // mise à jour des erreurs après validation
        function OnValidate(validationErreurs) {
            $ctrl.Erreurs = validationErreurs;
        }


        /*
        * @function addFilter2Favoris()
        * @description Crée un nouveau favori
        */
        function addFilter2Favoris() {
            var url = $window.location.pathname;
            if ($ctrl.favoriId !== 0) {
                url = $window.location.pathname.slice(0, $window.location.pathname.lastIndexOf("/"));
            }
            favorisService.initializeAndOpenModal("DetailBudget", url, $ctrl.filters);
        }

        function getFilterOrFavoris(budgetId, favoriId) {
            $ctrl.favoriId = parseInt(favoriId);
            if ($ctrl.favoriId !== 0) {
                favorisService.getFilterByFavoriIdOrDefault({ favoriId: $ctrl.favoriId, defaultFilter: $ctrl.filters })
                    .then(function (response) {
                        $ctrl.filters = response;
                        LoadDetail(budgetId);
                    }).catch(function (error) {
                        console.log(error);
                        LoadDetail(budgetId);
                    });
            }
            else if (sessionStorage.getItem('budgetDetailFilter') !== null) {
                $ctrl.filters = JSON.parse(sessionStorage.getItem('budgetDetailFilter'));
                LoadDetail(budgetId);
            }
            else {
                LoadDetail(budgetId);
            }
        }
    }
}(angular));
