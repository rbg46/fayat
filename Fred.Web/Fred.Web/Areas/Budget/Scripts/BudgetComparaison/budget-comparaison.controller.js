(function (angular) {
    'use strict';

    angular.module('Fred').controller('BudgetComparaisonController', BudgetComparaisonController);
    BudgetComparaisonController.$inject = ['$scope', 'BudgetComparaisonService', 'ProgressBar', 'Notify', 'fredDialog', '$timeout'];

    function BudgetComparaisonController($scope, BudgetComparaisonService, ProgressBar, Notify, fredDialog, $timeout) {

        //////////////////////////////////////////////////////////////////
        // Membres                                                      //
        //////////////////////////////////////////////////////////////////
        var $ctrl = this;
        var viewIdIndex = 0;
        $ctrl.resources = resources;
        $ctrl.BudgetComparaisonService = BudgetComparaisonService;
        $ctrl.Loaded = false;
        $ctrl.Loading = false;
        $ctrl.FirstTimeLoad = true;
        $ctrl.CI = null;
        $ctrl.Revision1 = null;
        $ctrl.Revision2 = null;
        $ctrl.View = {
            CI: null,
            Revision1: null,
            Revision2: null
        };
        $ctrl.Data = null;
        $ctrl.ColWidths = null;
        $ctrl.Identifiers = {
            EnteteColQuantite: "ENTETE_COL_QUANTITE_ID",
            EnteteColUnite: "ENTETE_COL_UNITE_ID",
            EnteteColPrixUnitaire: "ENTETE_COL_PRIX_UNITAIRE_ID",
            EnteteColMontant: "ENTETE_COL_MONTANT_ID"
        };

        $ctrl.OnCiChanged = OnCiChanged;
        $ctrl.OnRevision1Changed = OnRevision1Changed;
        $ctrl.OnRevision1Deleted = OnRevision1Deleted;
        $ctrl.OnRevision2Changed = OnRevision2Changed;
        $ctrl.OnRevision2Deleted = OnRevision2Deleted;
        $ctrl.OnAxeTacheRessourceButtonClick = OnAxeTacheRessourceButtonClick;
        $ctrl.OnAxeRessourceTacheButtonClick = OnAxeRessourceTacheButtonClick;
        $ctrl.OnExpandAllButtonClick = OnExpandAllButtonClick;
        $ctrl.OnCollapseAllButtonClick = OnCollapseAllButtonClick;
        $ctrl.OnPersonnalisationAffichageButtonClick = OnPersonnalisationAffichageButtonClick;
        $ctrl.OnExcelExportButtonClick = OnExcelExportButtonClick;
        $ctrl.OnHideColonneButtonClick = OnHideColonneButtonClick;

        $ctrl.GetRevisionLookupText = GetRevisionLookupText;
        $ctrl.GetRevisionTooltip = GetRevisionTooltip;

        return $ctrl;


        //////////////////////////////////////////////////////////////////
        // Evènements internes                                          //
        //////////////////////////////////////////////////////////////////

        // Appelé au changement de CI
        function OnCiChanged() {
            if ($ctrl.CI && $ctrl.CI.CiId === $ctrl.View.CI.CiId) {
                return;
            }

            if ($ctrl.Loaded) {
                fredDialog.confirmation(
                    $ctrl.resources.BudgetComparaison_ChangementDeCiMessage,
                    $ctrl.resources.BudgetComparaison_ChangementDeCiTitre,
                    "flaticon flaticon-shuffle-1", '', '',
                    function () {       // Valider
                        CiHasChanged();
                    },
                    function () {       // Annuler
                        $ctrl.View.CI = $ctrl.CI;
                    }
                );
            }
            else {
                CiHasChanged();
            }
        }

        // Appelé au changement de la révision 1
        function OnRevision1Changed() {
            RevisionHasChanged(
                () => $ctrl.Revision1 = $ctrl.View.Revision1,   // Si c'est valide
                () => $ctrl.View.Revision1 = $ctrl.Revision1    // Si c'est invalide
            );
        }

        // Appelé à la suppression de la révision 1
        function OnRevision1Deleted() {
            $ctrl.View.Revision1 = null;
            $ctrl.Revision1 = null;
            $ctrl.Loaded = false;
        }

        // Appelé au changement de la révision 2
        function OnRevision2Changed() {
            RevisionHasChanged(
                () => $ctrl.Revision2 = $ctrl.View.Revision2,   // Si c'est valide
                () => $ctrl.View.Revision2 = $ctrl.Revision2    // Si c'est invalide
            );
        }

        // Appelé à la suppression de la révision 2
        function OnRevision2Deleted() {
            $ctrl.View.Revision2 = null;
            $ctrl.Revision2 = null;
            $ctrl.Loaded = false;
        }

        // Appelé sur le clic du bouton tâche / ressource
        function OnAxeTacheRessourceButtonClick() {
            if (BudgetComparaisonService.Filter.AxeAnalytique.Type === BudgetComparaisonService.AxeAnalytique.Type.TacheRessource) {
                return;
            }
            BudgetComparaisonService.Filter.AxeAnalytique.Type = BudgetComparaisonService.AxeAnalytique.Type.TacheRessource;
            BudgetComparaisonService.Filter.AxeAnalytique.Update();
            Compare(false);
        }

        // Appelé sur le clic du bouton ressource / tâche
        function OnAxeRessourceTacheButtonClick() {
            if (BudgetComparaisonService.Filter.AxeAnalytique.Type === BudgetComparaisonService.AxeAnalytique.Type.RessourceTache) {
                return;
            }
            BudgetComparaisonService.Filter.AxeAnalytique.Type = BudgetComparaisonService.AxeAnalytique.Type.RessourceTache;
            BudgetComparaisonService.Filter.AxeAnalytique.Update();
            Compare(false);
        }

        // Appelé sur le clic du bouton de personnalisation de l'affichage
        function OnPersonnalisationAffichageButtonClick() {
            $scope.$broadcast(BudgetComparaisonService.Events.DisplayFilterPanel, {
                OnValidate: function (axeAnalytiqueChanged) {
                    UpdateColWidths();
                    if (axeAnalytiqueChanged) {
                        Compare(false);
                    }
                }
            });
        }

        // Appelé sur le clic du bouton tout déplier
        function OnExpandAllButtonClick() {
            ExpandOrCollapseAll('show');
        }

        // Appelé sur le clic du bouton tout replier
        function OnCollapseAllButtonClick() {
            ExpandOrCollapseAll('hide');
        }

        // Appelé sur le clic du bouton d'export Excel
        function OnExcelExportButtonClick() {
            ExcelExport();
        }

        // Appelé sur le clic du bouton pour cacher une colonne
        function OnHideColonneButtonClick(group, colonneType) {
            switch (colonneType) {
                case $ctrl.BudgetComparaisonService.ColonneType.Quantite:
                    group.HasQuantite = false;
                    break;
                case $ctrl.BudgetComparaisonService.ColonneType.Unite:
                    group.HasUnite = false;
                    break;
                case $ctrl.BudgetComparaisonService.ColonneType.PrixUnitaire:
                    group.HasPrixUnitaire = false;
                    break;
            }
            UpdateColWidths();
        }


        //////////////////////////////////////////////////////////////////
        // Comparaison                                                  //
        //////////////////////////////////////////////////////////////////
        function Compare(displayLoadInfo) {
            $ctrl.Loaded = false;
            $ctrl.Loading = true;
            ProgressBar.start(true);
            let request = {
                BudgetId1: $ctrl.Revision1.BudgetId,
                BudgetId2: $ctrl.Revision2.BudgetId,
                Axes: BudgetComparaisonService.Filter.AxeAnalytique.NodeTypes
            };
            BudgetComparaisonService.Compare(request)
                .then((response) => CompareThen(response, displayLoadInfo))
                .catch(CompareCatch)
                .finally(CompareFinally);
        }
        function CompareThen(response, displayLoadInfo) {
            $ctrl.Data = response.data;
            if ($ctrl.Data.Erreur) {
                Notify.error($ctrl.Data.Erreur);
            }
            else {
                if (displayLoadInfo && $ctrl.Data.Information) {
                    Notify.message($ctrl.Data.Information);
                }
                viewIdIndex = 0;
                for (let node of $ctrl.Data.Tree.Nodes) {
                    InitialyzeNode(node);
                }
                InitialyzeGroup($ctrl.Data.EcartTotal);
                $ctrl.Loaded = true;

                if ($ctrl.FirstTimeLoad) {
                    $ctrl.FirstTimeLoad = false;
                    $timeout(() => {
                        UpdateColWidths();
                    }, 0, false);
                }
            }
        }
        function CompareCatch() {
            Notify.error($ctrl.resources.Global_Notification_Error);
        }
        function CompareFinally() {
            ProgressBar.complete();
            $ctrl.Loading = false;
        }

        // Initialise un noeud après le chargement
        function InitialyzeNode(node) {
            node.ViewId = "NODE_" + viewIdIndex++;
            InitialyzeGroup(node.Budget1);
            InitialyzeGroup(node.Budget2);
            InitialyzeGroup(node.Ecart);
            for (let subNode of node.Nodes) {
                InitialyzeNode(subNode);
            }
        }

        // Initialise un groupe après le chargement
        function InitialyzeGroup(group) {
            group.Unites = [];
            if (group.UniteIds !== null) {
                for (let uniteId of group.UniteIds) {
                    if (uniteId === null) {
                        group.Unites.push(BudgetComparaisonService.SansUnite);
                    }
                    else {
                        for (let unite of $ctrl.Data.Unites) {
                            if (uniteId === unite.UniteId) {
                                group.Unites.push(unite);
                                break;
                            }
                        }
                    }
                }
            }
        }


        //////////////////////////////////////////////////////////////////
        // Excel export                                                 //
        //////////////////////////////////////////////////////////////////
        function ExcelExport() {
            ProgressBar.start(true);
            let request = {
                BudgetId1: $ctrl.Revision1.BudgetId,
                BudgetId2: $ctrl.Revision2.BudgetId,
                Axes: BudgetComparaisonService.Filter.AxeAnalytique.NodeTypes,
                AxePrincipal: BudgetComparaisonService.Filter.AxeAnalytique.Type,
                Colonnes: BudgetComparaisonService.Filter.Colonnes
            };
            BudgetComparaisonService.ExcelExport(request)
                .then(ExcelExportThen)
                .catch(ExcelExportCatch)
                .finally(ExcelExportFinally);
        }
        function ExcelExportThen(response) {
            if (response && response.data) {
                Notify.error(response.data.Erreur);
            }
        }
        function ExcelExportCatch() {
            Notify.error($ctrl.resources.Global_Notification_Error);
        }
        function ExcelExportFinally() {
            ProgressBar.complete();
        }


        //////////////////////////////////////////////////////////////////
        // Vue                                                          //
        //////////////////////////////////////////////////////////////////

        // Retourne le texte à afficher dans un lookup de révision de budget
        // - revision : la révision concernée
        function GetRevisionLookupText(revision) {
            if (!revision) {
                return null;
            }
            return $ctrl.resources.BudgetComparaison_Version + " " + revision.Revision;
        }

        // Retourne le tooltip à afficher pour une révision de budget
        // - revision : la révision concernée
        function GetRevisionTooltip(revision) {
            if (!revision) {
                return null;
            }
            var ret = $ctrl.resources.BudgetComparaison_Version + " " + revision.Revision;
            ret += '\r\n' + revision.Etat;
            if (revision.PeriodeDebut !== null) {
                ret += '\r\n' + revision.PeriodeDebut;
                if (revision.PeriodeFin) {
                    ret += " - " + revision.PeriodeFin;
                }
            }
            return ret;

        }

        // Met à jour la largeur des colonnes
        function UpdateColWidths() {
            if ($ctrl.ColWidths === null) {
                $ctrl.ColWidths = {
                    Quantite: $('#' + $ctrl.Identifiers.EnteteColQuantite)[0].offsetWidth,
                    Unite: $('#' + $ctrl.Identifiers.EnteteColUnite)[0].offsetWidth,
                    PrixUnitaire: $('#' + $ctrl.Identifiers.EnteteColPrixUnitaire)[0].offsetWidth,
                    Montant: $('#' + $ctrl.Identifiers.EnteteColMontant)[0].offsetWidth
                };
            }
            $ctrl.ColWidths.Budget1 = GetSubEnteteAndFooterWidth(BudgetComparaisonService.Filter.Colonnes.Budget1);
            $ctrl.ColWidths.Budget2 = GetSubEnteteAndFooterWidth(BudgetComparaisonService.Filter.Colonnes.Budget2);
            $ctrl.ColWidths.Ecart = GetSubEnteteAndFooterWidth(BudgetComparaisonService.Filter.Colonnes.Ecart);
        }

        // Retourne la largeur des colonnes pour un groupe
        // - group : le groupe concerné
        function GetSubEnteteAndFooterWidth(group) {
            var width = $ctrl.ColWidths.Montant;
            width += group.HasQuantite ? $ctrl.ColWidths.Quantite : 0;
            width += group.HasUnite ? $ctrl.ColWidths.Unite : 0;
            width += group.HasPrixUnitaire ? $ctrl.ColWidths.PrixUnitaire : 0;
            return width;
        }


        //////////////////////////////////////////////////////////////////
        // Autre                                                        //
        //////////////////////////////////////////////////////////////////

        // A appeler lorsque le CI a changé
        function CiHasChanged() {
            $ctrl.CI = $ctrl.View.CI;
            $ctrl.Revision1 = null;
            $ctrl.View.Revision1 = null;
            $ctrl.Revision2 = null;
            $ctrl.View.Revision2 = null;
            $ctrl.Loaded = false;
        }

        // A appeler lorsqu'une révision a changé
        function RevisionHasChanged(valid, invalid) {
            if ($ctrl.View.Revision1 === null || $ctrl.View.Revision2 === null) {
                valid();
                return;
            }
            if ($ctrl.View.Revision1.Revision === $ctrl.View.Revision2.Revision) {
                Notify.error($ctrl.resources.BudgetComparaison_BudgetDejaSelectionne);
                invalid();
                return;
            }
            valid();
            Compare(true);
        }

        // Plie ou déplie le noeud indiqué et ses enfants.
        // - mode : 'show' ou 'hide'
        // - node : le noeud concerné, si undefined alors prend les noeuds de 1er niveau
        function ExpandOrCollapseAll(mode, node) {
            let nodes = null;
            if (!node) {
                nodes = $ctrl.Data.Tree.Nodes;
            }
            else {
                nodes = node.Nodes;
                $("#" + node.ViewId).collapse(mode);
            }
            for (let subNode of nodes) {
                ExpandOrCollapseAll(mode, subNode);
            }
        }
    }
}(angular));
