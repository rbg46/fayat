(function (angular) {
    'use strict';

    angular.module('Fred').controller('BudgetListeController', BudgetListeController);

    BudgetListeController.$inject = ['$scope',
        'BudgetService',
        '$window',
        'CIService',
        'ProgressBar',
        'Notify',
        'confirmDialog',
        'favorisService'];

    function BudgetListeController($scope, BudgetService, $window, CIService, ProgressBar, Notify, confirmDialog, favorisService) {

        var $ctrl = this;

        $ctrl.resources = resources;
        $ctrl.displayBanner = false;
        $ctrl.BudgetEnApplicationId = null;
        $ctrl.budgets = new Array();
        $ctrl.copyToSameCi = false;
        $ctrl.useBibliothequePrixPendantCopie = false;

        $ctrl.filters = {
            ciId: null,
            periode: null,
            currentBudgetEtatFilter: [],
            displayBudgetDeleted: false
        };

        //Events déclaration
        $ctrl.initWithCiId = initWithCiId;
        $ctrl.onCiSelected = onCiSelected;
        $ctrl.onPeriodChange = onPeriodChange;
        $ctrl.onShowbudgetInformation = onShowbudgetInformation;
        $ctrl.onCopierbudgetBoutonClicked = onCopierbudgetBoutonClicked;
        $ctrl.onCopierbudgetDansCICourant = onCopierbudgetDansCICourant;
        $ctrl.onBudgetDetailsSave = onBudgetDetailsSave;
        $ctrl.onBoutonCreerBudgetClicked = onBoutonCreerBudgetClicked;
        $ctrl.onFilterBudgetBy = onFilterBudgetBy;
        $ctrl.onPartagerBudget = onPartagerBudget;
        $ctrl.onShowDetailBudget = onShowDetailBudget;
        $ctrl.onDeactivatePeriodeFilter = onDeactivatePeriodeFilter;
        $ctrl.onBudgetDetailsAnnuler = onBudgetDetailsAnnuler;
        $ctrl.onOpenModalPeriodeFin = onOpenModalPeriodeFin;
        $ctrl.onRecetteBoutonClicked = onRecetteBoutonClicked;
        $ctrl.onSupprimerBudget = onSupprimerBudget;
        $ctrl.onClearCiLookup = onClearCiLookup;
        $ctrl.onRestoreTrashCanClicked = onRestoreTrashCanClicked;
        $ctrl.onRestoreDeletedBudget = onRestoreDeletedBudget;

        //Helpers 
        $ctrl.getClassePourEtatbudget = getClassePourEtatbudget;
        $ctrl.isBudgetEnApplication = isBudgetEnApplication;
        $ctrl.isBudgetBrouillon = isBudgetBrouillon;
        $ctrl.isBudgetSupprime = isBudgetSupprime;
        $ctrl.computeSomeRecette = computeSomeRecette;
        $ctrl.getTooltipFavoris = getTooltipFavoris;
        $ctrl.getFilterOrFavoris = getFilterOrFavoris;
        $ctrl.addFilter2Favoris = addFilter2Favoris;

        $scope.$on(BudgetService.Events.RefreshListBudget, function (event, arg) { refreshListBudgetWithBudgetId(arg.budgetId); });


        $scope.versionComparer = versionComparer;

        var budgetArrayBeforeFiltering = new Array();
        var currentPeriode = null;
        var budgetsClones;

        return $ctrl;

        //Events Implementation

        function initWithCiId(id) {

            var preselectedCiId = parseInt(id);
            //Maintenant que le filtre est fait, on peut regarder si on a un CI déjà pré choisi
            //spécifié dans l'URL
            if (!isNaN(preselectedCiId)) {
                CIService.GetById({ ciId: preselectedCiId }).$promise.then((response) => {
                    var preSelectedCi = response;
                    if (preSelectedCi !== null) {
                        $ctrl.ciSelected = preSelectedCi;
                        ProgressBar.start();

                        //Puisqu'on a déjà le CI on peut lancer la recherche
                        getBudget();

                    }
                });
            }
        }

        function onCiSelected() {
            ProgressBar.start();
            getBudget();
        }

        function computeSomeRecette(budget) {
            if (typeof budget === "undefined" || budget === null) {
                return 0;
            }
            if (budget.Recettes === null) {
                budget.Recettes = {
                    MontantMarche: 0,
                    MontantAvenants: 0,
                    SommeAValoir: 0,
                    TravauxSupplementaires: 0,
                    Revision: 0,
                    AutresRecettes: 0,
                    PenalitesEtRetenues: 0
                };
            }

            var montantMarche = parseFloat(budget.Recettes.MontantMarche);
            var montantAvenant = parseFloat(budget.Recettes.MontantAvenants);
            var sommeAValoir = parseFloat(budget.Recettes.SommeAValoir);
            var travauxSupplementaires = parseFloat(budget.Recettes.TravauxSupplementaires);
            var revision = parseFloat(budget.Recettes.Revision);
            var autreRecette = parseFloat(budget.Recettes.AutresRecettes);
            var penaliteEtRetenues = parseFloat(budget.Recettes.PenalitesEtRetenues);

            budget.SommeRecettes = montantMarche + montantAvenant + sommeAValoir + travauxSupplementaires + revision + autreRecette + penaliteEtRetenues;
            return budget.SommeRecettes;
        }

        function onDeactivatePeriodeFilter() {
            currentPeriode = null;
            $ctrl.filters.periode = null;

            filterBudgetsByCodeAndPeriode();
        }


        function onClearCiLookup() {
            $ctrl.ciSelected = null;
            $ctrl.budgets = new Array();
        }

        function onPeriodChange() {

            //Lorsque l'utilisateur désactive le filtre sur les période, cette fonction est appelée
            //alors que la variable periode est perdue (puisqu'on l'a désactivé)
            if ($ctrl.filters.periode === null || typeof $ctrl.filters.periode === "undefined") {
                return;
            }

            //Selon l'API PeriodeDebut et PeriodeFin sont des entiers au format YYYYMM
            //Donc on formatte la période saisie pour qu'elle soit à ce format
            var selectedYear = $ctrl.filters.periode.getFullYear();
            var selectedMonth = $ctrl.filters.periode.getMonth() + 1;
            if (selectedMonth < 10) {
                selectedYear = selectedYear * 10;
            }
            var selectedPeriode = parseInt(selectedYear.toString() + selectedMonth.toString());

            if (selectedPeriode !== currentPeriode) {
                currentPeriode = selectedPeriode;
                filterBudgetsByCodeAndPeriode();
            }
        }


        function onShowDetailBudget(budget) {
            BudgetService.ShowBudgetDetail(budget.BudgetId);
        }

        function onRestoreDeletedBudget(budget) {
            ProgressBar.start();
            BudgetService.RestoreDeletedBudget(budget.BudgetId)
                .then((response) => restoreBudgetSuccess(response, budget))
                .catch(restoreBudgetFailure);
        }

        function restoreBudgetSuccess(response, budget) {
            if (response.data) {
                budget.DateSuppression = null;
                Notify.message(resources.BudgetRestaurerSuccess);
                ProgressBar.complete();
            }
            else {
                restoreBudgetFailure();
            }
        }

        function restoreBudgetFailure() {
            Notify.error(resources.BudgetRestaurerEchec);
            ProgressBar.complete();
        }

        function onSupprimerBudget(budget) {
            confirmDialog.confirm(resources, resources.ModalConfirmationSuppressionBudget, "flaticon flaticon-warning")
                .then(() => suppressionConfirmee(budget));
        }

        function suppressionConfirmee(budget) {
            ProgressBar.start();
            BudgetService.SupprimerBudget(budget.BudgetId)
                .then((response) => onSupprimerBudgetSuccess(response, budget))
                .catch(onSupprimerBudgetError);
        }

        function onSupprimerBudgetSuccess(response, budget) {

            budget.DateSuppression = response.data.DateSuppression;
            Notify.message(resources.BudgetSupprimeSuccess);
            ProgressBar.complete();
            filterBudgetsByCodeAndPeriode();
        }

        function onSupprimerBudgetError(error) {

            if (error && error.data) {
                Notify.error(error.data.Message);
            } else {
                Notify.error(resources.ErreurSuppressionBudget);
            }


            ProgressBar.complete();
        }

        function onPartagerBudget(budget) {

            $ctrl.budgetSelectionne = budget;

            //On adapte le texte en fonction de l'état du budget, demande de confirmation de partage pour un budget privé
            //et demande de confirmation de privatisation pour un budget partagé
            var content = resources.ModalConfirmationPartageMessage;
            if ($ctrl.budgetSelectionne.Partage) {
                content = resources.ModalConfirmationPrivatiseBudget;
            }

            confirmDialog.confirm(resources, content, "flaticon flaticon-warning")
                .then(partageOuPrivativeBudget);

        }

        function partageOuPrivativeBudget() {
            ProgressBar.start();
            BudgetService.PartageOuDepartageBudget($ctrl.budgetSelectionne.BudgetId)
                .then(partageBudgetSuccess)
                .catch(partageBudgetError);
        }

        function partageBudgetSuccess(response) {
            $ctrl.budgetSelectionne.Partage = response.data;

            if ($ctrl.budgetSelectionne.Partage) {
                Notify.message(resources.BudgetPartageSuccess);
            }
            else {
                Notify.message(resources.BudgetPrivatiseSuccess);
            }

            ProgressBar.complete();
        }

        function onFilterBudgetBy(filterBy) {

            if ($ctrl.filters.currentBudgetEtatFilter.includes(filterBy)) {
                //Si on reclique sur un bouton on désactive le filtre
                var indexOfColumn = $ctrl.filters.currentBudgetEtatFilter.indexOf(filterBy);
                $ctrl.filters.currentBudgetEtatFilter.splice(indexOfColumn, 1);

            }
            else {
                $ctrl.filters.currentBudgetEtatFilter.push(filterBy);
            }

            filterBudgetsByCodeAndPeriode();
        }

        function filterBudgetsByCodeAndPeriode() {
            $ctrl.filters.ciId = $ctrl.ciSelected !== null ? $ctrl.ciSelected.CiId : null;
            sessionStorage.setItem('budgetListFilter', JSON.stringify($ctrl.filters));
            $ctrl.budgets = budgetArrayBeforeFiltering;

            if ($ctrl.filters.currentBudgetEtatFilter.length > 0) {
                $ctrl.budgets = $ctrl.budgets.filter(
                    budget =>
                        $ctrl.filters.currentBudgetEtatFilter.includes(getBoutonLibelleFromCodeEtat(budget.BudgetEtat.Code))
                        ||
                        //L'état rejeté est un etat calculé donc il n'existe aucun code associé à cet état
                        isBudgetRejete(budget) && $ctrl.filters.currentBudgetEtatFilter.includes('rejete')

                );
            }

            if (currentPeriode !== null) {
                //On récupère tous les budgets qui sont actifs sur la période donnée
                $ctrl.budgets = $ctrl.budgets.filter(budget =>
                    budget.PeriodeDebut <= currentPeriode &&
                    budget.PeriodeFin === null &&
                    budget.PeriodeDebut !== null ||
                    budget.PeriodeDebut <= currentPeriode &&
                    budget.PeriodeFin >= currentPeriode &&
                    budget.PeriodeDebut !== null &&
                    budget.PeriodeFin !== null
                );
            }

            if (!$ctrl.filters.displayBudgetDeleted) {
                $ctrl.budgets = $ctrl.budgets.filter(budget => budget.DateSuppression === null);
            }
        }

        function onRestoreTrashCanClicked() {
            $ctrl.filters.displayBudgetDeleted = !$ctrl.filters.displayBudgetDeleted;
            filterBudgetsByCodeAndPeriode();
        }

        function getBoutonLibelleFromCodeEtat(codeEtat) {

            switch (codeEtat) {
                case BudgetService.CodeBudgetEtat.Brouillon:
                    return 'brouillon';
                case BudgetService.CodeBudgetEtat.AValider:
                    return 'avalider';
                case BudgetService.CodeBudgetEtat.EnApplication:
                    return 'valide';
                case BudgetService.CodeBudgetEtat.Archive:
                    return 'archive';
            }
        }

        function isBudgetRejete(budget) {
            //Pour savoir si un budget a été rejeté il suffit de vérifier si l'état courant est brouillon
            //et si l'état précédent est a validé
            //Supérieur à 2 car pour etre rejeté un brouillon doit : 
            //Avoir été crée (1 workflow)
            //Avoir été mis à l'état a valider (1 workflow)
            //Avoir été rejeté (1 workflow)
            //Rappel l'API nous garanti que les workflow sont triés par date (le plus récent d'abord)
            return budget.BudgetEtat.Code === 'BR' && budget.Workflows.length > 2 && budget.Workflows[0].EtatInitial.Code === 'AV';
        }


        function isBudgetSupprime(budget) {
            return budget.DateSuppression !== null;
        }

        function onBoutonCreerBudgetClicked() {
            if($ctrl.ciSelected){
                confirmDialog.confirm(resources, resources.ModalConfirmationAjoutBudget, "flaticon flaticon-warning")
                    .then(creeNouveauBudgetVideSurCi);
            }
        }

        function creeNouveauBudgetVideSurCi() {
            if ($ctrl.ciSelected === null) {
                Notify.error(resources.BudgetListeCreateEmptyBudgetNoCiSelectedError);
                return;
            }
            ProgressBar.start();
            BudgetService.CreateEmptyBudgetSurCi($ctrl.ciSelected.CiId)
                .then(addNewBudgetToBudgetList)
                .catch(createError);
        }

        function onShowbudgetInformation(budget) {
            $ctrl.budgetSelectionne = budget;
            $ctrl.displayBanner = !$ctrl.displayBanner;
        }

        //Cette fonction est appelée lorsque l'utilisateur clique sur le bouton de copie d'une des révisions affichées dans la liste
        function onCopierbudgetBoutonClicked(budget) {
            $ctrl.budgetSelectionne = budget;
        }


        //Cette fonction est appelée lorsque l'utilisateur clique sur le bouton de copie dans le CI courant dans la boite de 
        //dialogue modale   
        function onCopierbudgetDansCICourant() {
            $("#dupeBudgetModal").modal('hide');
            ProgressBar.start();
            BudgetService.CopieBudgetDansMemeCi($ctrl.budgetSelectionne.BudgetId, $ctrl.useBibliothequePrixPendantCopie)
                .then(addNewBudgetToBudgetList)
                .catch(copyError);

        }


        function getBudget() {
            BudgetService.GetBudget($ctrl.ciSelected.CiId)
                .then(displayBudgetFromCi)
                .catch(getBudgetError);
        }

        function displayBudgetFromCi(response) {
            clearBudgetScope();
            var budgets = response.data;
            if (!budgets || budgets.length === 0) {
                Notify.error(resources.BudgetListeNoBudgetFoundOnCI);
            }
            addBudgetsToScope(budgets);
        }

        function addNewBudgetToBudgetList(response) {
            var budget = response.data;
            var budgets = [budget];
            addBudgetsToScope(budgets);
        }

        function addBudgetsToScope(budgets) {
            $ctrl.budgets = $ctrl.budgets.concat(budgets);
            budgetArrayBeforeFiltering = $ctrl.budgets;
            budgetsClones = JSON.parse(JSON.stringify($ctrl.budgets));
            filterBudgetsByCodeAndPeriode();
            ProgressBar.complete();

        }

        function clearBudgetScope() {
            //Si le budget n'existe pas
            if (typeof $ctrl.budgets !== "undefined") {
                $ctrl.budgets.length = 0;
            }
        }

        function onBudgetDetailsSave(budgetToSave) {
            BudgetService.SaveBudgetChangeInListView(budgetToSave)
                .then(saveSuccessfull)
                .catch(saveError);
        }

        function onBudgetDetailsAnnuler() {

            var budgetAvantModification = budgetsClones.find(function (element) {
                return element.BudgetId === $ctrl.budgetSelectionne.BudgetId;
            });
            $ctrl.budgetSelectionne = JSON.parse(JSON.stringify(budgetAvantModification));

            var budgetInScopeIndex = $ctrl.budgets.findIndex(function (element) {
                return element.BudgetId === $ctrl.budgetSelectionne.BudgetId;
            });
            $ctrl.budgets[budgetInScopeIndex] = $ctrl.budgetSelectionne;
        }

        function onRecetteBoutonClicked() {
            $(".tab1").parent().removeClass('active');
            $(".tab2").parent().addClass('active');
        }

        function onOpenModalPeriodeFin() {
            ProgressBar.start();
            BudgetService.LoadPeriodeRecalage($ctrl.ciSelected.CiId)
                .then(LoadPeriodeRecalageThen)
                .catch(LoadPeriodeRecalageCatch)
                .finally(ProgressBarComplete);
        }

        function LoadPeriodeRecalageThen(result) {
            var res = result.data;
            if (res.Erreur) {
                Notify.error(res.Erreur);
            }
            else {
                $ctrl.BudgetEnApplicationId = res.BudgetId;
                $ctrl.listPeriodes = res.Periodes;
                DisplayRecalage();
            }
        }

        function refreshListBudgetWithBudgetId(budgetId) {
            BudgetService.GetBudgetByBudgetId(budgetId)
                .then(addNewBudgetToBudgetList)
                .catch(getBudgetError);
        }

        function LoadPeriodeRecalageCatch() {
            Notify.error("Une erreur est survenue dans la récupération des périodes de controle budgétaires clôturés.");
        }

        function DisplayRecalage() {
            $scope.$broadcast(BudgetService.Events.DisplayRecalage);
        }

        function ProgressBarComplete() {
            ProgressBar.complete();
        }

        function getClassePourEtatbudget(budget) {
            switch (budget.BudgetEtat.Code) {
                case "BR":
                    return "draft";
                case "AV":
                    return "tovalidate";
                case "EA":
                    return "application";
                case "AR":
                    return "archived";
                default:
                    return "draft";
            }
        }

        function isBudgetEnApplication(budget) {
            if (typeof budget !== "undefined") {
                return budget.BudgetEtat.Code === "EA";
            }

        }

        function isBudgetBrouillon(budget) {
            if (typeof budget !== "undefined") {
                return budget.BudgetEtat.Code === "BR";
            }

        }


        function saveSuccessfull(response) {
            $ctrl.displayBanner = !$ctrl.displayBanner;

            var budgetAvantModificationIndex = budgetsClones.findIndex(function (element) {
                return element.BudgetId === $ctrl.budgetSelectionne.BudgetId;
            });

            //On récupère les recettes qui ont potentiellement étaient créées
            $ctrl.budgetSelectionne.Recettes = response.data.Recettes;
            budgetsClones[budgetAvantModificationIndex] = JSON.parse(JSON.stringify($ctrl.budgetSelectionne));
        }

        function saveError() {
            Notify.error(resources.BudgetListeSaveResumeDetailError);
        }

        function getBudgetError() {
            Notify.error(resources.BudgetListeGetBudgetError);
        }
        function copyError() {
            Notify.error(resources.BudgetListeCopyBudgetError);
        }
        function createError() {
            Notify.error(resources.BudgetListeCreateEmptyBudgetError);
            ProgressBar.complete();
        }

        function partageBudgetError() {
            Notify.error(resources.BudgetListePartageBudgetError);
            ProgressBar.complete();
        }

        function versionComparer(a, b) {
            let splittedVersion = a.value.Version.split('.');
            let aMajeure = splittedVersion[0];
            let aMineure = splittedVersion[1];

            splittedVersion = b.value.Version.split('.');
            let bMajeure = splittedVersion[0];
            let bMineure = splittedVersion[1];

            if (aMajeure !== bMajeure) {
                return aMajeure - bMajeure;
            }

            return aMineure - bMineure;
        }

        function getTooltipFavoris() {
            if ($ctrl.ciSelected && $ctrl.filters.periode) {
                var periode = $ctrl.filters.periode.getMonth() + 1 + "/" + $ctrl.filters.periode.getFullYear();
                return resources.Budget_Liste_Tooltip_Favoris_Part1 + $ctrl.ciSelected.CodeLibelle + resources.Budget_Liste_Tooltip_Favoris_Part2 + periode;
            }
            else if ($ctrl.ciSelected) {
                return resources.Budget_Liste_Tooltip_Favoris_Part1 + $ctrl.ciSelected.CodeLibelle;
            }
            return "";
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
            favorisService.initializeAndOpenModal("ListeBudget", url, $ctrl.filters);
        }

        function getFilterOrFavoris(ciid, favoriId) {
            $ctrl.favoriId = parseInt(favoriId);
            if ($ctrl.favoriId !== 0) {
                favorisService.getFilterByFavoriIdOrDefault({ favoriId: $ctrl.favoriId, defaultFilter: $ctrl.filters })
                    .then(function (response) {
                        $ctrl.filters = {
                            ciId: response.CiId,
                            periode: response.Periode !== null ? new Date(response.Periode) : null,
                            currentBudgetEtatFilter: response.CurrentBudgetEtatFilter,
                            displayBudgetDeleted: response.DisplayBudgetDeleted
                        };
                        initWithCiId($ctrl.filters.ciId);
                    }).catch(function (error) { console.log(error); });
            }
            else if (sessionStorage.getItem('budgetListFilter') !== null) {
                $ctrl.filters = JSON.parse(sessionStorage.getItem('budgetListFilter'));
                $ctrl.filters.periode = $ctrl.filters.periode !== null ? new Date($ctrl.filters.periode) : null;
                ciid = $ctrl.filters.ciId;
            }
            initWithCiId(ciid);
        }
    }
}(angular));