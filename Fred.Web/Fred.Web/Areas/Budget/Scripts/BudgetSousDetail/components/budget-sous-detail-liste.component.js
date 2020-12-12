(function () {
    'use strict';

    var budgetSousDetailListeComponent = {
        templateUrl: '/Areas/Budget/Scripts/BudgetSousDetail/components/budget-sous-detail-liste.component.html',
        bindings: {
            budget: '<',
            sousDetail: '<',
            resources: '<'
        },
        controller: budgetSousDetailListeController
    };
    angular.module('Fred').component('budgetSousDetailListeComponent', budgetSousDetailListeComponent);
    angular.module('Fred').controller('budgetSousDetailListeController', budgetSousDetailListeController);
    budgetSousDetailListeController.$inject = ['$scope', 'BudgetService', 'BudgetDetailService', 'ProgressBar', 'Notify'];

    function budgetSousDetailListeController($scope, BudgetService, BudgetDetailService, ProgressBar, Notify) {
        var $ctrl = this;

        //////////////////////////////////////////////////////////////////
        // Membres publiques                                            //
        //////////////////////////////////////////////////////////////////
        $ctrl.Budget = $ctrl.budget;
        $ctrl.SousDetail = $ctrl.sousDetail;
        $ctrl.BudgetDetailService = BudgetDetailService;
        $ctrl.VueSD = false;
        $ctrl.VueT4 = false;
        $ctrl.filter = {
            displaySyntheseQB: true,
            displaySyntheseT4: true
        };

        $ctrl.ShowRessourcePanel = ShowRessourcePanel;
        $ctrl.ShowComment = ShowComment;
        $ctrl.DeleteSousDetailItem = DeleteSousDetailItem;
        $ctrl.GetMainClass = GetMainClass;
        $ctrl.GetShowCommentButtonTooltip = GetShowCommentButtonTooltip;
        $ctrl.ChangeDisplaySynthese = ChangeDisplaySynthese;
        $ctrl.OnChangeQuantite = OnChangeQuantite;
        $ctrl.OnChangeQuantiteSD = OnChangeQuantiteSD;
        $ctrl.IsPuDifferentDeBibliothequePrix = IsPuDifferentDeBibliothequePrix;
        $ctrl.IsUniteReadOnly = IsUniteReadOnly;
        $ctrl.GetTooltipPrixUnitaire = GetTooltipPrixUnitaire;

        //////////////////////////////////////////////////////////////////
        // Initialisation                                               //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {
            VueChanged();
            $scope.$on(BudgetService.Events.SaveSousDetail, function (event, SaveDoneCallback) { Save(SaveDoneCallback); });
            $scope.$on(BudgetService.Events.PanelRessourceAdded, function (event, arg) { RessourceAdded(arg); });
            $scope.$on(BudgetService.Events.SousDetailVueChanged, function (event, arg) { VueChanged(); });
            $scope.$on(BudgetService.Events.PanelCommentaireModified, function (event, arg) { CommentaireHasChanged(arg); });

            if (sessionStorage.getItem('filterBudgetSousDetail') !== null) {
                $ctrl.filter = JSON.parse(sessionStorage.getItem('filterBudgetSousDetail'));
            }
        };


        //////////////////////////////////////////////////////////////////
        // Enregistrement                                               //
        //////////////////////////////////////////////////////////////////
        function Save(SaveDoneCallback) {
            var itemsChanged = [];
            var itemsDeletedId = [];
            var sousDetailItemsChanged = [];
            var sousDetailItemsDeletedViewId = [];
            for (let sousDetailItem of $ctrl.SousDetail.Items) {
                if (sousDetailItem.Deleted) {
                    sousDetailItemsDeletedViewId.push(sousDetailItem.ViewId);
                    itemsDeletedId.push(sousDetailItem.BudgetSousDetailId);
                }
                else if (sousDetailItem.HasChanged($ctrl.SousDetail.BudgetT4.View.VueSD)) {
                    sousDetailItemsChanged.push(sousDetailItem);
                    // Crée les [BudgetSousDetailSave.ItemModel]
                    itemsChanged.push(BudgetService.CreateBudgetSousDetailSaveItemModel(sousDetailItem));
                }
            }

            // Vérifie s'il y a des changements
            var budgetT4Changed = $ctrl.SousDetail.BudgetT4.HasChangedInSousDetail();
            if (itemsChanged.length === 0 && itemsDeletedId.length === 0 && !budgetT4Changed) {
                Notify.message($ctrl.resources._Budget_PasDeModificationAEnregistrer);
            }
            else {
                // Crée le [BudgetSousDetailSave.Model]
                var model = BudgetService.CreateBudgetSousDetailSaveModel($ctrl.Budget, $ctrl.SousDetail, itemsChanged, itemsDeletedId);

                // Crée le [BudgetSousDetailSave.Tache4Model] si requis
                if (budgetT4Changed || $ctrl.SousDetail.BudgetT4Id === 0 && itemsChanged.length > 0) {
                    model.BudgetT4 = BudgetService.CreateBudgetSousDetailSaveTache4Model($ctrl.SousDetail.BudgetT4.Tache.TacheId, $ctrl.SousDetail.BudgetT4.View);
                }

                ProgressBar.start();
                BudgetService.SaveSousDetail(model)
                    .then(result => SaveSousDetailThen(result, sousDetailItemsChanged, sousDetailItemsDeletedViewId, SaveDoneCallback))
                    .catch(error => SaveSousDetailCatch(SaveDoneCallback));
            }
        }
        function SaveSousDetailThen(result, sousDetailItemsChanged, sousDetailItemsDeletedViewId, SaveDoneCallback) {
            var data = result.data;
            if (data.Erreur) {
                Notify.error(data.Erreur);
                if (SaveDoneCallback) {
                    SaveDoneCallback(false);
                }

            }
            else {
                // Affecte l'identifiant du budget T4 s'il a été créé
                if (data.BudgetT4CreatedId) {
                    $ctrl.SousDetail.BudgetT4Id = data.BudgetT4CreatedId;
                    $ctrl.SousDetail.BudgetT4.BudgetT4Id = data.BudgetT4CreatedId;
                }
                $ctrl.SousDetail.BudgetT4.SavedForSousDetail();

                // Supprime les éléments de la liste qui ont été supprimés
                for (let sousDetailItemDeletedViewId of sousDetailItemsDeletedViewId) {
                    for (var i = 0; i < $ctrl.SousDetail.Items.length; i++) {
                        if ($ctrl.SousDetail.Items[i].ViewId === sousDetailItemDeletedViewId) {
                            $ctrl.SousDetail.Items.splice(i, 1);
                            break;
                        }
                    }
                }

                // Affecte les nouveaux identifiants et flag les éléments comme enregistrés
                for (let sousDetailItemChanged of sousDetailItemsChanged) {
                    for (var j = 0; j < data.ItemsCreated.length; j++) {
                        var itemCreated = data.ItemsCreated[j];
                        if (sousDetailItemChanged.ViewId === itemCreated.ViewId) {
                            sousDetailItemChanged.BudgetSousDetailId = itemCreated.BudgetSousDetailId;
                            data.ItemsCreated.splice(j, 1);
                            break;
                        }
                    }
                    sousDetailItemChanged.Saved();
                }
                Notify.message($ctrl.resources.Budget_SousDetail_EnregistrementReussi);
                if (SaveDoneCallback) {
                    SaveDoneCallback(true);
                }

            }
            ProgressBar.complete();
        }
        function SaveSousDetailCatch(SaveDoneCallback) {
            ProgressBar.complete();
            Notify.error($ctrl.resources._Budget_Erreur_Enregistrement);
            if (SaveDoneCallback) {
                SaveDoneCallback(false);
            }

        }


        //////////////////////////////////////////////////////////////////
        // Actions                                                      //
        //////////////////////////////////////////////////////////////////

        // Affiche le panneau de sélection des ressources
        function ShowRessourcePanel() {
            $scope.$broadcast(BudgetService.Events.OpenPanelRessource);
        }

        // Ouvre le panneau de gestion des commentaires.
        function ShowComment(sousDetailItem) {
            $scope.$broadcast(BudgetService.Events.OpenPanelCommentaire, { SousDetailItem: sousDetailItem });
        }

        // Marque un élément de sous-détail comme supprimé ou le supprime de la liste s'il n'a jamais été enregistré
        function DeleteSousDetailItem(sousDetailItem) {
            if (sousDetailItem.BudgetSousDetailId === 0) {
                for (var i = 0; i < $ctrl.SousDetail.Items.length; i++) {
                    if ($ctrl.SousDetail.Items[i].ViewId === sousDetailItem.ViewId) {
                        $ctrl.SousDetail.Items.splice(i, 1);
                        break;
                    }
                }
            }
            else {
                sousDetailItem.Deleted = true;
            }
            BudgetDetailService.SousDetailItemDeleted(sousDetailItem);
        }


        //////////////////////////////////////////////////////////////////
        // Evènements                                                   //
        //////////////////////////////////////////////////////////////////

        // Appelé lors du changement de vue.
        function VueChanged() {
            $ctrl.VueSD = $ctrl.SousDetail.BudgetT4.View.VueSD === BudgetDetailService.SousDetailViewModeType.SD;
            $ctrl.VueT4 = $ctrl.SousDetail.BudgetT4.View.VueSD === BudgetDetailService.SousDetailViewModeType.T4;
        }

        // Appelé après l'ajout d'une ressource via le panneau.
        function RessourceAdded(arg) {
            // Crée un [BudgetSousDetailLoad.ItemModel]
            var sousDetailItem = BudgetService.CreateBudgetSousDetailLoadItemModel(0, null, null, null, null, arg.Chapitre, arg.SousChapitre, arg.Ressource);
            $ctrl.SousDetail.Items.push(sousDetailItem);
            var promise = Promise.resolve(BudgetDetailService.SousDetailItemAdded(sousDetailItem, $ctrl.SousDetail, arg.Ressource.BibliothequePrixMontant));
            promise.then(ScrollToBottom);
        }

        // Appelé après la modification d'un commentaire via le panneau.
        function CommentaireHasChanged(arg) {
            if (arg.SousDetailItem) {
                arg.SousDetailItem.View.Commentaire = arg.Commentaire;
            }
        }

        //////////////////////////////////////////////////////////////////
        // Autre                                                        //
        //////////////////////////////////////////////////////////////////

        // Retourne la class css à utiliser en fonction de la vue.
        function GetMainClass() {
            if ($ctrl.VueT4) {
                return "vue-t4";
            }
            if ($ctrl.VueSD) {
                return "vue-sd";
            }
            return "";
        }

        function GetShowCommentButtonTooltip() {
            return $ctrl.Budget.Readonly
                ? $ctrl.resources.Budget_Detail_Tooltip_VoirCommentaire
                : $ctrl.resources.Budget_Detail_Tooltip_ModifierCommentaire;
        }

        function ChangeDisplaySynthese(item) {
            if (item === "T4") {
                $ctrl.filter.displaySyntheseT4 = !$ctrl.filter.displaySyntheseT4;
                sessionStorage.setItem('filterBudgetSousDetail', JSON.stringify($ctrl.filter));
            } else if (item === "QB") {
                $ctrl.filter.displaySyntheseQB = !$ctrl.filter.displaySyntheseQB;
                sessionStorage.setItem('filterBudgetSousDetail', JSON.stringify($ctrl.filter));
            }
        }

        function ScrollToBottom() {
            var scroll = document.getElementById('tbody-sousDetail');
            scroll.scrollTop = scroll.scrollHeight;
        }

        // Gere les changements de formule et de quantité depuis le composant input formula
        function OnChangeQuantite(item, valeur, formule) {
            item.View.Quantite = valeur;
            item.View.QuantiteFormule = formule;
            BudgetDetailService.SousDetailItemQuantiteChanged(item);
            BudgetDetailService.SousDetailItemQuantiteFormuleChanged(item);
        }

        // Gere les changements de formule et de quantité SD depuis le composant input formula
        function OnChangeQuantiteSD(item, value, formule) {
            item.View.QuantiteSD = value;
            item.View.QuantiteSDFormule = formule;
            BudgetDetailService.SousDetailItemQuantiteSdChanged(item);
            BudgetDetailService.SousDetailItemQuantiteSdFormuleChanged(item);
        }

        function IsPuDifferentDeBibliothequePrix(sousDetailItem) {
            return sousDetailItem.Current.PrixUnitaire !== sousDetailItem.PuBibliothequePrix;
        }

        function IsUniteReadOnly(sousDetailItem) {
            //L'unité est considérée comme étant readonly si elle est saisie dans la bibliotheque des prix
            return sousDetailItem.UniteIdBibliothequePrix;
        }

        function GetTooltipPrixUnitaire(sousDetailItem){
            if(IsPuDifferentDeBibliothequePrix(sousDetailItem)){
                var tooltip = $ctrl.resources.Budget_SousDetail_Tooltip_PrixDifferentBibliothequePrix;
                tooltip = String.format(tooltip, BudgetDetailService.GetTooltip(sousDetailItem.PuBibliothequePrix));
                return tooltip;
            }
            return BudgetDetailService.GetTooltip(sousDetailItem.View.PrixUnitaire);
        }
    }
})();
