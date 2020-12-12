(function (angular) {
    'use strict';

    angular.module('Fred').controller('AvancementController', AvancementController);

    AvancementController.$inject = ['$scope', '$q', 'ProgressBar', 'BudgetService', 'Notify', 'AvancementExcelModelBuilderService', 'AvancementCalculService', 'favorisService', 'CIService', '$window', '$uibModal', '$timeout'];

    function AvancementController($scope, $q, ProgressBar, BudgetService, Notify, AvancementExcelModelBuilderService, AvancementCalculService, favorisService, CIService, $window, $uibModal, $timeout) {
        var $ctrl = this;
        var nomMois = ["Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre"];
        $ctrl.showableColumns = [
            { name: 'BudgetQuantite', size: 100 },
            { name: 'BudgetPU', size: 100 },
            { name: 'BudgetMontant', size: 130 },
            { name: 'AcMoisDernier', size: 100 },
            { name: 'AcMoisActuel', size: 100 },
            { name: 'AcEcart', size: 100 },
            { name: 'DadMoisDernier', size: 130 },
            { name: 'DadMoisActuel', size: 130 },
            { name: 'DadEcart', size: 130 },
            { name: 'RadMontant', size: 130 },
            { name: 'RadPourcent', size: 100 },
            { name: 'RadQuantite', size: 130 }
        ];
        $ctrl.showableAnalyticalAxis =
            [
                { Name: "T1", Type: "T", Order: 0 },
                { Name: "T2", Type: "T", Order: 1 },
                { Name: "T3", Type: "T", Order: 2 },
                { Name: "T4", Type: "T", Order: 3 }
            ];
        //////////////////////////////////////////////////////////////////
        // Membres  publiques                                           //
        //////////////////////////////////////////////////////////////////
        $ctrl.resources = resources;
        $ctrl.Budget = null;
        var periodeCourante;
        $ctrl.periodePrecedente = $ctrl.resources.Global_Period;
        $ctrl.periodeCourante = $ctrl.resources.Global_PreviousPeriod;
        $ctrl.libelleBoutonValider = $ctrl.resources.Global_Bouton_Valider;
        $ctrl.busy = false;
        $ctrl.symboleDevise = "€";
        $ctrl.filters = {
            ci: null,
            periode: new Date(),
            searchTache: null,
            shownAnalyticalAxis: $ctrl.showableAnalyticalAxis,
            shownColumns: $ctrl.showableColumns
        };
        $ctrl.favoriId = 0;
        periodeCourante = $ctrl.filters.periode;
        $ctrl.currentMonth = $ctrl.filters.periode;
        $ctrl.currentMonthFront = null;
        $ctrl.previousMonth = angular.copy($ctrl.filters.periode);
        $ctrl.previousMonthFront = null;
        $ctrl.displayBanner = false;
        $ctrl.filterActiveOnAxe = false;
        $ctrl.shownColumns = angular.copy($ctrl.showableColumns);
        $ctrl.shownColumnsBeforeCancel = null;
        $ctrl.shownAnalyticalAxis = angular.copy($ctrl.showableAnalyticalAxis);
        $ctrl.shownAnalyticalAxisBeforeCancel = null;
        $ctrl.analyticalAxisMessage = "";

        $ctrl.ChangePeriode = ChangePeriode;
        $ctrl.ChangePerimetre = ChangePerimetre;
        $ctrl.Load = Load;
        $ctrl.Save = Save;
        $ctrl.Validate = Validate;
        $ctrl.RetourBrouillonAvancement = RetourBrouillonAvancement;
        $ctrl.EcartAvancement = AvancementCalculService.EcartAvancement;
        $ctrl.EcartAvancementT4 = AvancementCalculService.EcartAvancementT4;
        $ctrl.EcartDAD = AvancementCalculService.EcartDAD;
        $ctrl.CalculPourcentageRAD = AvancementCalculService.CalculPourcentageRAD;
        $ctrl.CalculQuantiteRAD = AvancementCalculService.CalculQuantiteRAD;
        $ctrl.CalculAvancementFromEcartPourcent = AvancementCalculService.CalculAvancementFromEcartPourcent;
        $ctrl.RecalculFromT1 = RecalculFromT1;
        $ctrl.RecalculFromT2 = RecalculFromT2;
        $ctrl.RecalculFromT3 = RecalculFromT3;
        $ctrl.RecalculFromT4 = RecalculFromT4;
        $ctrl.ShowComment = ShowComment;
        $ctrl.GetUniteAvancementPrevious = GetUniteAvancementPrevious;
        $ctrl.GetUniteAvancement = GetUniteAvancement;
        $ctrl.GetAvancementPrevious = GetAvancementPrevious;
        $ctrl.onToutPlier = onToutPlier;
        $ctrl.onToutDeplier = onToutDeplier;
        $ctrl.onExportExcel = onExportExcel;
        $ctrl.onClickHideOrShowChildren = onClickHideOrShowChildren;
        $ctrl.updateFilters = updateFilters;
        $ctrl.getTooltipFavoris = getTooltipFavoris;
        $ctrl.addFilter2Favoris = addFilter2Favoris;
        $ctrl.getFilterOrFavoris = getFilterOrFavoris;
        $ctrl.ChangeEcartTache = ChangeEcartTache;
        $ctrl.ChangeAvancementTache = ChangeAvancementTache;
        $ctrl.isAnalyticalAxisSelected = isAnalyticalAxisSelected;
        $ctrl.configurationRightPanel = configurationRightPanel;
        $ctrl.adjustColumnWidth = adjustColumnWidth;
        $ctrl.checkFilterActiveOnAxe = checkFilterActiveOnAxe;
        $ctrl.resetColumnsWidth = resetColumnsWidth;
        $ctrl.adjustColumnsWidth = adjustColumnsWidth;
        $ctrl.showAnalyticalAxis = showAnalyticalAxis;
        $ctrl.isShowableColumn = isShowableColumn;

        $ctrl.handleRightPanelShow = handleRightPanelShow;
        $ctrl.handleShowOrHideColumn = handleShowOrHideColumn;
        $ctrl.handleShowOrHideAxe = handleShowOrHideAxe;
        $ctrl.handleSaveConfigurationAvancement = handleSaveConfigurationAvancement;
        $ctrl.handleCancelConfigurationChangesAvancement = handleCancelConfigurationChangesAvancement;

        // Enumérations
        $ctrl.EditedColumnEnum = {
            Avancement: 1,
            Ecart: 2
        };

        // Valeurs venant du CSS
        $ctrl.enteteBudgetWidth = 330;
        $ctrl.enteteAvancementCumuleWidth = 300;
        $ctrl.enteteDadCumuleWidth = 390;
        $ctrl.enteteRadCumuleWidth = 360;

        //////////////////////////////////////////////////////////////////
        // Initialisation                                               //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {
            $ctrl.CodeEtatAvancement = BudgetService.CodeEtatAvancement;
            $ctrl.TypeAvancementEnum = BudgetService.TypeAvancementEnum;
            $ctrl.GetTwoDecimalValue = BudgetService.GetTwoDecimalValue;
            $ctrl.IsNotNullOrEmpty = BudgetService.IsNotNullOrEmpty;
            $scope.$on(BudgetService.Events.PanelCommentaireModified, function (event, arg) { CommentaireHasChanged(arg); });
        };


        //////////////////////////////////////////////////////////////////
        // Chargement                                                   //
        //////////////////////////////////////////////////////////////////
        function ChangePeriode() {
            $q.when()
                .then(() => {
                    if (!periodeCourante || !IsSamePeriod($ctrl.filters.periode, periodeCourante)) {
                        periodeCourante = $ctrl.filters.periode;
                        ChangePerimetre();
                    }
                });
        }

        // Indique si 2 dates sont sur la même période
        // return : true si les 2 dates sont sur la même période, sinon false
        function IsSamePeriod(date1, date2) {
            return date1.getFullYear() === date2.getFullYear() && date1.getMonth() === date2.getMonth();
        }

        function ChangePerimetre() {
            if ($ctrl.filters.ci && $ctrl.filters.periode) {
                sessionStorage.setItem('avancementFilter', JSON.stringify($ctrl.filters));
                Load();
            }
        }

        function Load() {
            ProgressBar.start();
            $ctrl.busy = true;
            BudgetService.GetAvancement($ctrl.filters.ci.CiId, GetPeriodeFormat($ctrl.filters.periode))
                .then(GetDetailThen)
                .then(InitAvancement)
                .catch(GetDetailCatch)
                .finally(GetDetailFinally);
        }

        function GetDetailThen(result) {
            $ctrl.Budget = result.data;
        }


        function GetDetailCatch() {
            Notify.error($ctrl.Budget.Erreur);
        }

        function GetDetailFinally() {
            $ctrl.busy = false;
            ProgressBar.complete();
        }

        function Save() {
            ProgressBar.start();
            var AvancementSaveModel = InitAvancementSaveModel(true);

            BudgetService.SaveAvancement(AvancementSaveModel)
                .then(SaveAvancementThen)
                .catch(SaveAvancementCatch)
                .finally(SaveAvancementFinally);
        }

        function Validate() {
            var modalInstance = $uibModal.open({
                animation: true,
                component: 'validateAvancementComponent',
                resolve: {
                    resources: function () { return $ctrl.resources; }
                }
            });

            // Traitement des données renvoyées par la modal
            modalInstance.result.then(function () {
                ProgressBar.start();
                var AvancementValidateModel = InitAvancementSaveModel(true);
                if ($ctrl.Budget.CodeEtatAvancement === $ctrl.CodeEtatAvancement.Enregistre) {
                    BudgetService.SaveAvancement(AvancementValidateModel)
                        .then(() => SaveBeforeValidateThen(AvancementValidateModel))
                        .catch(SaveAvancementCatch)
                        .finally(SaveAvancementFinally);
                }
                else {
                    BudgetService.ValidateAvancement(AvancementValidateModel, $ctrl.Budget.CodeEtatAvancement)
                        .then(ValidateAvancementThen)
                        .catch(ValidateAvancementCatch)
                        .finally(ProgressBarComplete);
                }
            });

        }

        function RetourBrouillonAvancement() {
            ProgressBar.start();
            var AvancementValidateModel = InitAvancementSaveModel(true);

            BudgetService.RetourBrouillonAvancement(AvancementValidateModel)
                .then(RetourBrouillonAvancementThen)
                .catch(ValidateAvancementCatch)
                .finally(ProgressBarComplete);
        }

        function InitAvancementSaveModel(saveMode) {
            var AvancementSaveModel = {
                BudgetId: $ctrl.Budget.BudgetId,
                CiId: $ctrl.filters.ci.CiId,
                DeviseId: 48,
                Periode: GetPeriodeFormat($ctrl.filters.periode),
                ListTaches: [],
                ListAvancementT4: []
            };

            for (let tache1 of $ctrl.Budget.TachesNiveau1) {
                PushTacheCommentaireAvancement(AvancementSaveModel.ListTaches, tache1);
                for (let tache2 of tache1.TachesNiveau2) {
                    PushTacheCommentaireAvancement(AvancementSaveModel.ListTaches, tache2);
                    for (let tache3 of tache2.TachesNiveau3) {
                        PushTacheCommentaireAvancement(AvancementSaveModel.ListTaches, tache3);
                        for (let tache4 of tache3.TachesNiveau4) {
                            PushTacheCommentaireAvancement(AvancementSaveModel.ListTaches, tache4);
                            if (saveMode) {
                                if (tache4.BudgetT4Id) {
                                    AvancementSaveModel.ListAvancementT4.push(GetTache4SaveModel(tache4));
                                }
                            }
                            else {
                                AvancementSaveModel.ListAvancementT4.push(GetTache4SaveModel(tache4));
                            }
                        }
                    }
                }
            }
            return AvancementSaveModel;
        }

        function ValidateAvancementThen() {
            Notify.message($ctrl.resources.Avancement_ValidationOK);
            Load();
        }

        function ValidateAvancementCatch() {
            Notify.error($ctrl.resources.Global_Notification_Error);
        }

        function RetourBrouillonAvancementThen() {
            Notify.message($ctrl.resources.Avancement_ValidationOK);
            Load();
        }

        function SaveBeforeValidateThen(AvancementValidateModel) {
            Notify.message($ctrl.resources.Avancement_EnregistrementOK);
            ProgressBar.start();
            BudgetService.ValidateAvancement(AvancementValidateModel, $ctrl.Budget.CodeEtatAvancement)
                .then(ValidateAvancementThen)
                .catch(ValidateAvancementCatch)
                .finally(ProgressBarComplete);
        }

        function SaveAvancementThen() {
            Notify.message($ctrl.resources.Avancement_EnregistrementOK);
        }

        function SaveAvancementCatch() {
            Notify.error($ctrl.resources.Global_Notification_Error);
        }

        function onExportExcel(isPdfConverted) {

            ProgressBar.start();
            var AvancementSaveModel = InitAvancementSaveModel(true);
            BudgetService.SaveAvancement(AvancementSaveModel)
                .then(() => {
                    SaveAvancementThen();
                    continueWithExportExcel(isPdfConverted);
                })
                .catch(SaveAvancementCatch)
                .finally(ProgressBarComplete);
        }


        function continueWithExportExcel(isPdfConverted) {

            ProgressBar.start();
            var model = AvancementExcelModelBuilderService.CreateExcelModel($ctrl.filters.ci.CiId, GetPeriodeFormat($ctrl.filters.periode), $ctrl.Budget.TachesNiveau1, $ctrl.shownAnalyticalAxis, isPdfConverted);
            BudgetService.ExportExcelAvancement(model)
                .then(response => downloadExportExcelFile(response, isPdfConverted))
                .catch(onExportExcelError)
                .finally(ProgressBarComplete);
        }

        function downloadExportExcelFile(response, isPdf) {
            BudgetService.DownloadExportFile(response.data.id, isPdf, 'Avancement');
        }

        function onExportExcelError() {
            Notify.error($ctrl.resources.Global_Notification_Error);
        }

        function SaveAvancementFinally() {
            if ($ctrl.Budget.IsBudgetAvancementEcart)
                UneditAllTaches();

            ProgressBarComplete();
        }

        function ProgressBarComplete() {
            ProgressBar.complete();
        }

        function InitAvancement() {
            InitTotalTacheNiveau1();
            InitPeriode($ctrl.filters.periode);
            InitT1toT4();
            CalculLibelleBoutonValider();
        }

        function InitPeriode(date) {
            var currentDate = new Date(date);
            $ctrl.periodeCourante = nomMois[currentDate.getMonth()] + " " + currentDate.getFullYear();
            currentDate.setMonth(currentDate.getMonth() - 1);
            $ctrl.periodePrecedente = nomMois[currentDate.getMonth()] + " " + currentDate.getFullYear();
        }

        function InitT1toT4() {
            for (let tache1 of $ctrl.Budget.TachesNiveau1) {
                var allTaches2Disabled = true;
                for (let tache2 of tache1.TachesNiveau2) {
                    var allTaches3Disabled = true;
                    for (let tache3 of tache2.TachesNiveau3) {
                        var allTaches4Disabled = true;
                        for (let tache4 of tache3.TachesNiveau4) {
                            tache4.Disabled = true;
                            tache4.Disabled = false;
                            if (allTaches4Disabled) {
                                allTaches4Disabled = tache4.Disabled;
                            }
                            tache4.EcartAvancement = $ctrl.EcartAvancementT4(tache4);
                        }
                        tache3.Disabled = allTaches4Disabled;
                        if (allTaches3Disabled) {
                            allTaches3Disabled = tache3.Disabled;
                        }
                        SommeTache(tache3, tache3.TachesNiveau4, true);
                    }
                    tache2.Disabled = allTaches3Disabled;
                    if (allTaches2Disabled) {
                        allTaches2Disabled = tache2.Disabled;
                    }
                    SommeTache(tache2, tache2.TachesNiveau3, false);
                }
                tache1.Disabled = allTaches2Disabled;
                SommeTache(tache1, tache1.TachesNiveau2, false);
            }
            SommeTotalTachesNiveau1();
        }

        function AggregationT1toT4(tache1) {
            for (let tache2 of tache1.TachesNiveau2) {
                for (let tache3 of tache2.TachesNiveau3) {
                    for (let tache4 of tache3.TachesNiveau4) {
                        RefreshRowValuesT4(tache4);
                    }
                    SommeTache(tache3, tache3.TachesNiveau4, true);
                }
                SommeTache(tache2, tache2.TachesNiveau3, false);
            }
            SommeTache(tache1, tache1.TachesNiveau2, false);
            SommeTotalTachesNiveau1();
        }

        function SommeTache(tacheParent, listTachesEnfants, isTacheParent3) {
            if (!tacheParent.Disabled) {
                var count = 0;
                var countPrevious = 0;
                var sumMontant = 0;
                var sumMontantAvance = 0;
                var sumMontantPreviousAvance = 0;
                var sumQuantite = 0;
                var sumPU = 0;
                var sumDADPrevious = 0;
                var sumDAD = 0;
                var sumRAD = 0;
                for (let tache of listTachesEnfants) {
                    if (!tache.Disabled) {
                        sumMontant += tache.Montant;
                        sumQuantite += tache.Quantite;
                        sumPU += tache.PU;
                        sumDADPrevious += tache.DADPrevious;
                        sumDAD += tache.DAD;
                        sumRAD += tache.RAD;
                        if (isTacheParent3 && tache.AvancementQtePrevious && tache.AvancementQte) {
                            sumMontantPreviousAvance += tache.Montant * tache.AvancementQtePrevious / tache.Quantite;
                            countPrevious++;
                            if (tache.AvancementQte) {
                                sumMontantAvance += tache.Montant * tache.AvancementQte / tache.Quantite;
                                count++;
                            }
                        }
                        else {
                            if (tache.AvancementPourcentPrevious) {
                                sumMontantPreviousAvance += tache.Montant * tache.AvancementPourcentPrevious / 100;
                                countPrevious++;
                            }
                            if (tache.AvancementPourcent) {
                                sumMontantAvance += tache.Montant * tache.AvancementPourcent / 100;
                                count++;
                            }
                        }
                    }
                }
                tacheParent.Montant = sumMontant;
                tacheParent.Quantite = sumQuantite;
                tacheParent.PU = sumPU;
                tacheParent.DADPrevious = sumDADPrevious;
                tacheParent.DAD = sumDAD;
                tacheParent.RAD = sumRAD;
                if (count > 0) {
                    tacheParent.AvancementPourcent = sumMontantAvance / tacheParent.Montant * 100;
                }
                else {
                    tacheParent.AvancementPourcent = 0;
                }
                if (countPrevious > 0) {
                    tacheParent.AvancementPourcentPrevious = sumMontantPreviousAvance / tacheParent.Montant * 100;
                }
                else {
                    tacheParent.AvancementPourcentPrevious = 0;
                }
                if ($ctrl.IsNotNullOrEmpty(tacheParent.AvancementPourcent)) {
                    tacheParent.CurrentAvancementPourcent = tacheParent.AvancementPourcent;
                }
                tacheParent.EcartAvancement = $ctrl.EcartAvancement(tacheParent);
            }
        }

        // flag l'indicateur d'edition et recalcul les autres niveaux d'avancement
        function ChangeAvancementTache(tache1, tache2, tache3, tache4) {
            // récupération la tache éditée (de plus bas niveau)
            var tache = tache4 || tache3 || tache2 || tache1;

            // reset l'indicateur d'edition pour les parents et les enfants de la tache
            if ($ctrl.Budget.IsBudgetAvancementEcart) {
                UneditParentTaches(tache1, tache2, tache3, tache4);
                UneditChildrenTaches(tache);
                tache.EditedColumn = $ctrl.EditedColumnEnum.Avancement;
            }

            // recalcul des avancements en fonction du niveau
            if (tache4)
                $ctrl.RecalculFromT4(tache1, tache2, tache3, tache4);
            else if (tache3)
                $ctrl.RecalculFromT3(tache1, tache2, tache3);
            else if (tache2)
                $ctrl.RecalculFromT2(tache1, tache2);
            else if (tache1)
                $ctrl.RecalculFromT1(tache1);
        }

        // flag l'indicateur d'edition et recalcul les autres niveaux d'avancement en fonction de l'écart saisi
        function ChangeEcartTache(tache1, tache2, tache3, tache4) {
            // récupération la tache éditée (de plus bas niveau)
            var tache = tache4 || tache3 || tache2 || tache1;

            // reset l'indicateur d'edition pour les parents et les enfants de la tache
            if ($ctrl.Budget.IsBudgetAvancementEcart) {
                UneditParentTaches(tache1, tache2, tache3, tache4);
                UneditChildrenTaches(tache);
                tache.EditedColumn = $ctrl.EditedColumnEnum.Ecart;
            }

            var ecartAvancement = parseFloat(tache.EcartAvancement);
            if (Number.isNaN(ecartAvancement)) {
                //restaure l'ecart initial si la valeur saisie est invalide
                tache.EcartAvancement = $ctrl.EcartAvancement(tache);
                return;
            }

            // calcul de l'ecart en %, les calculs sont tous fait avec l'ecart pourcent, pas d'ecart quantité
            var ecartPourcent = tache.EcartAvancement;
            if (tache.TypeAvancement === BudgetService.TypeAvancementEnum.Quantite) {
                ecartPourcent = (tache.EcartAvancement || 0) / (tache.Quantite || 0) * 100;
            }

            tache.EcartAvancement = ecartAvancement.toFixed(2);
            // recherche toutes les taches de niveau 4 
            var taches4 = [];
            if (tache4)
                taches4 = [tache4];
            else {
                taches4 = GetTacheChildrenNiveau4(tache);
            }

            // calcule l'avancement pour tous les T4
            taches4.forEach((t4) => {
                t4.AvancementPourcent = parseFloat($ctrl.CalculAvancementFromEcartPourcent(t4.AvancementPourcentPrevious, ecartPourcent));
                t4.ecartAvancement = $ctrl.EcartAvancement(t4);
                setModifiedValueFlag(t4);
                RefreshRowValuesT4(t4);
            });
            // flag de modification pour les niveaux parents
            setModifiedValueFlag(tache1);
            setModifiedValueFlag(tache2);
            setModifiedValueFlag(tache3);
            // flag de modification pour les niveaux T1 à T3 enfants de la tache
            SetModifiedValueFlagChildrenT1toT3(tache);
            // recalcul des avancements du niveau 1 à 4
            AggregationT1toT4(tache1);
        }

        // supprime le flag d'édition des parents de la tache
        function UneditParentTaches(tache1, tache2, tache3, tache4) {
            if (tache4)
                tache3.EditedColumn = null;
            if (tache3)
                tache2.EditedColumn = null;
            if (tache2)
                tache1.EditedColumn = null;
        }

        // supprime le flag d'édition des enfants de la tache
        function UneditChildrenTaches(tache) {
            for (var childTache of GetTacheChildren(tache)) {
                childTache.EditedColumn = null;
                UneditChildrenTaches(childTache);
            }
        }

        // retourne la liste des enfants de la tache
        function GetTacheChildren(tache) {
            return tache.TachesNiveau2
                || tache.TachesNiveau3
                || tache.TachesNiveau4
                || [];
        }

        // retourne la liste des enfants de niveau 4 de la tache
        function GetTacheChildrenNiveau4(tache) {
            if (tache.TachesNiveau4)
                return tache.TachesNiveau4;

            var tachesNiveau4 = [];
            for (var childTache of GetTacheChildren(tache)) {
                tachesNiveau4 = tachesNiveau4.concat(GetTacheChildrenNiveau4(childTache));
            }
            return tachesNiveau4;
        }

        // Modification du flag IsTacheModified pour les niveaux T1 à T3
        function SetModifiedValueFlagChildrenT1toT3(tache) {
            setModifiedValueFlag(tache);
            if (tache.TachesNiveau4) {
                return;
            }
            for (var childTache of GetTacheChildren(tache)) {
                SetModifiedValueFlagChildrenT1toT3(childTache);
            }
        }

        // supprime le flag d'édition sur toutes les taches
        function UneditAllTaches() {
            for (var t1 of $ctrl.Budget.TachesNiveau1) {
                t1.EditedColumn = null;
                for (var t2 of t1.TachesNiveau2) {
                    t2.EditedColumn = null;
                    for (var t3 of t2.TachesNiveau3) {
                        t3.EditedColumn = null;
                        for (var t4 of t3.TachesNiveau4) {
                            t4.EditedColumn = null;
                        }
                    }
                }
            }
        }

        function GetPeriodeFormat(date) {
            var year = date.getFullYear();
            var month = date.getMonth() + 1;
            return year * 100 + month;
        }


        function GetUniteAvancement(tache4) {
            return DeduceUniteAvancement(tache4, tache4.AvancementQte, tache4.AvancementPourcent);
        }


        function GetUniteAvancementPrevious(tache4) {
            return DeduceUniteAvancement(tache4, tache4.AvancementQtePrevious, tache4.AvancementPourcentPrevious);
        }

        function DeduceUniteAvancement(tache4, tache4Qte, tache4Pourcentage) {

            //Si les deux valeurs valent 0 ou null ou undefined alors dans ce cas on utilise le Type de l'avancement
            if (!tache4Qte && !tache4Pourcentage) {
                if (tache4.TypeAvancement === BudgetService.TypeAvancementEnum.Quantite) {
                    return getNullOrTache4UniteCode(tache4);
                } else if (tache4.TypeAvancement === BudgetService.TypeAvancementEnum.Pourcentage) {
                    return '%';
                }
            }

            if (tache4Qte) {
                return getNullOrTache4UniteCode(tache4);
            }

            return '%';
        }

        function getNullOrTache4UniteCode(tache4) {
            if (tache4.Unite) {
                return tache4.Unite.Code;
            }
            return null;
        }

        /**
         * Retourne true si l'axe est sélectionné. Sinon, false
         * @param {any} axeType Type d'axe
         * @returns {any} Booléen (true, false)
         */
        function isAnalyticalAxisSelected(axeType) {
            if ($ctrl.shownAnalyticalAxis) {
                return $ctrl.shownAnalyticalAxis.findIndex((axe) => axe.Name === axeType.Name) !== -1;
            }
        }

        /**
         * Configure l'affiche de certains éléments du bandeau de droite
         */
        function configurationRightPanel() {
            $ctrl.currentMonthFront = nomMois[$ctrl.currentMonth.getMonth()] + ' ' + $ctrl.currentMonth.getFullYear();
            $ctrl.previousMonth.setMonth($ctrl.previousMonth.getMonth() - 1);
            $ctrl.previousMonthFront = nomMois[$ctrl.previousMonth.getMonth()] + ' ' + $ctrl.previousMonth.getFullYear();
        }

        /**
         * Ajustement des tailles de colonnes
         * @param {any} column Nom de la colonne
         * @param {any} shouldIncreased Détermine si la colonne doit s'agrandir ou non
         */
        function adjustColumnWidth(column, shouldIncreased) {
            var size = !shouldIncreased ? -column.size : column.size;

            if (column.name.startsWith("Budget")) {
                $ctrl.enteteBudgetWidth += size;
            }
            else if (column.name.startsWith("Ac")) {
                $ctrl.enteteAvancementCumuleWidth += size;
            }
            else if (column.name.startsWith("Dad")) {
                $ctrl.enteteDadCumuleWidth += size;
            }
            else if (column.name.startsWith("Rad")) {
                $ctrl.enteteRadCumuleWidth += size;
            }
        }

        /**
         * Contrôles sur les options de personnalisation
         */
        function checkFilterActiveOnAxe() {
            if ($ctrl.shownColumns.length !== $ctrl.showableColumns.length || $ctrl.shownAnalyticalAxis.length !== $ctrl.showableAnalyticalAxis.length) {
                $ctrl.filterActiveOnAxe = true;
            }
            else {
                $ctrl.filterActiveOnAxe = false;
            }
            if ($ctrl.shownAnalyticalAxis.length === 0) {
                $ctrl.analyticalAxisMessage = "Veuillez sélectionner au moins un axe.";
            }
            else {
                $ctrl.analyticalAxisMessage = "";
            }
        }

        /**
         *  Remet à 0 la width des colonnes paramétrables 
         */
        function resetColumnsWidth() {
            $ctrl.enteteAvancementCumuleWidth = 0;
            $ctrl.enteteBudgetWidth = 0;
            $ctrl.enteteDadCumuleWidth = 0;
            $ctrl.enteteRadCumuleWidth = 0;
        }

        /**
         * Ajuste la width selon les colonnes sélectionnées
         */
        function adjustColumnsWidth() {
            resetColumnsWidth();
            $ctrl.shownColumns.forEach(function (element) {
                adjustColumnWidth(element, true);
            });
        }

        function showAnalyticalAxis(axeName) {
            var temp = $ctrl.shownAnalyticalAxis.findIndex(axe => axe.Name === axeName) === -1;
            return temp;
        }

        /**
         * Evènement de l'affichage du bandeau de droite
         */
        function handleRightPanelShow() {
            $ctrl.shownAnalyticalAxisBeforeCancel = angular.copy($ctrl.shownAnalyticalAxis);
            $ctrl.shownColumnsBeforeCancel = angular.copy($ctrl.shownColumns);
            $ctrl.displayBanner = true;
            $ctrl.currentMonth = $ctrl.filters.periode;
            $ctrl.previousMonth = angular.copy($ctrl.filters.periode);
            configurationRightPanel();
        }

        /**
         * Affiche ou cache une colonne
         * @param {any} columnName Colonne
         */
        function handleShowOrHideColumn(columnName) {
            var column = $ctrl.shownColumns.find(col => col.name === columnName);
            var shouldIncreased = false;

            if (column) {
                $ctrl.shownColumns = $ctrl.shownColumns.filter(col => col !== column);
            } else {
                column = $ctrl.showableColumns.find(col => col.name === columnName);
                $ctrl.shownColumns.push(column);
                shouldIncreased = true;
            }

            adjustColumnWidth(column, shouldIncreased);
            checkFilterActiveOnAxe();
        }

        /**
         * Affiche ou cache un axe
         * @param {any} axe Axe (T1, T2, T3, T4)
         */
        function handleShowOrHideAxe(axe) {
            var indexOfAxe = $ctrl.shownAnalyticalAxis.findIndex(a => a.Name === axe.Name);
            if (indexOfAxe !== -1) {
                $ctrl.shownAnalyticalAxis.splice(indexOfAxe, 1);
            } else {
                $ctrl.shownAnalyticalAxis.push(axe);
            }
            checkFilterActiveOnAxe();
        }

        /**
         * Sauvegarde les modifications de l'utilisateur
         */
        function handleSaveConfigurationAvancement() {
            $ctrl.displayBanner = false;

            $ctrl.filters.shownAnalyticalAxis = angular.copy($ctrl.shownAnalyticalAxis);
            $ctrl.filters.shownColumns = angular.copy($ctrl.shownColumns);

            $ctrl.shownAnalyticalAxisBeforeCancel = angular.copy($ctrl.shownAnalyticalAxis);
            $ctrl.shownColumnsBeforeCancel = angular.copy($ctrl.shownColumns);

            if ($ctrl.filters.ci !== null) {
                Load();
            }
        }

        /**
         * Annule les modifications de l'utilisateur
         */
        function handleCancelConfigurationChangesAvancement() {
            $ctrl.shownAnalyticalAxis = angular.copy($ctrl.shownAnalyticalAxisBeforeCancel);
            $ctrl.shownColumns = angular.copy($ctrl.shownColumnsBeforeCancel);
            adjustColumnsWidth();
            checkFilterActiveOnAxe();
        }

        //////////////////////////////////////////////////////////////////
        // Actions                                                      //
        //////////////////////////////////////////////////////////////////


        function RecalculFromT1(tache1) {

            if (TacheValueChanged(tache1)) {
                setModifiedValueFlag(tache1);
                tache1.AvancementPourcent = parseFloat(tache1.AvancementPourcent);
                for (let tache2 of tache1.TachesNiveau2) {
                    if (!tache2.Disabled && $ctrl.IsNotNullOrEmpty(tache1.AvancementPourcent)) {
                        tache2.AvancementPourcent = tache1.AvancementPourcent;
                        setModifiedValueFlag(tache2);
                        for (let tache3 of tache2.TachesNiveau3) {
                            if (!tache3.Disabled && $ctrl.IsNotNullOrEmpty(tache2.AvancementPourcent)) {
                                setModifiedValueFlag(tache3);
                                tache3.AvancementPourcent = tache2.AvancementPourcent;
                                for (let tache4 of tache3.TachesNiveau4) {
                                    if (!tache4.Disabled && $ctrl.IsNotNullOrEmpty(tache3.AvancementPourcent)) {
                                        setModifiedValueFlag(tache4);
                                        tache4.AvancementPourcent = tache3.AvancementPourcent;
                                        RefreshRowValuesT4(tache4);
                                    }
                                }
                                SommeTache(tache3, tache3.TachesNiveau4, true);
                            }
                        }
                        SommeTache(tache2, tache2.TachesNiveau3, false);
                    }
                }
                SommeTache(tache1, tache1.TachesNiveau2, false);
            }
            SommeTotalTachesNiveau1();
        }

        function RecalculFromT2(tache1, tache2) {
            if (TacheValueChanged(tache2)) {
                setModifiedValueFlag(tache1);
                setModifiedValueFlag(tache2);
                tache2.AvancementPourcent = parseFloat(tache2.AvancementPourcent);
                for (let tache3 of tache2.TachesNiveau3) {
                    if (!tache3.Disabled && $ctrl.IsNotNullOrEmpty(tache2.AvancementPourcent)) {
                        setModifiedValueFlag(tache3);
                        tache3.AvancementPourcent = tache2.AvancementPourcent;
                        for (let tache4 of tache3.TachesNiveau4) {
                            if (!tache4.Disabled && $ctrl.IsNotNullOrEmpty(tache3.AvancementPourcent)) {
                                setModifiedValueFlag(tache4);
                                tache4.AvancementPourcent = tache3.AvancementPourcent;
                                RefreshRowValuesT4(tache4);
                            }
                        }
                    }
                }
                AggregationT1toT4(tache1);
            }
        }

        function RecalculFromT3(tache1, tache2, tache3) {
            if (TacheValueChanged(tache3)) {
                setModifiedValueFlag(tache1);
                setModifiedValueFlag(tache2);
                setModifiedValueFlag(tache3);
                tache3.AvancementPourcent = parseFloat(tache3.AvancementPourcent);
                for (let tache4 of tache3.TachesNiveau4) {
                    if (!tache4.Disabled && $ctrl.IsNotNullOrEmpty(tache3.AvancementPourcent)) {
                        setModifiedValueFlag(tache4);
                        tache4.AvancementPourcent = tache3.AvancementPourcent;
                        RefreshRowValuesT4(tache4);
                    }
                }
                AggregationT1toT4(tache1);
            }
        }

        function RecalculFromT4(tache1, tache2, tache3, tache4) {
            var typeAvancementHasChanged = SetTache4RelevantValueAvancementFieldFromUserInput(tache4);

            if (Tache4ValueChanged(tache4) || typeAvancementHasChanged) {
                setModifiedValueFlag(tache1);
                setModifiedValueFlag(tache2);
                setModifiedValueFlag(tache3);
                setModifiedValueFlag(tache4);
                if (tache4.AvancementQte) {
                    tache4.AvancementQte = parseFloat(tache4.AvancementQte);
                    RefreshRowValuesT4(tache4);
                    AggregationT1toT4(tache1);
                } else {
                    tache4.AvancementPourcent = parseFloat(tache4.AvancementPourcent);
                    RefreshRowValuesT4(tache4);
                    AggregationT1toT4(tache1);
                }
            }
        }

        function SetTache4RelevantValueAvancementFieldFromUserInput(tache4) {
            var initialTypeAvancement = tache4.TypeAvancement;
            if (!$ctrl.IsNotNullOrEmpty(tache4.ValeurAvancement)) {
                tache4.ValeurAvancement = '0';
            }

            if ($ctrl.Budget.IsTypeAvancementBudgetDynamique) {
                SetAvancementTypeFromUserInput(tache4);
                tache4.ValeurAvancement = tache4.ValeurAvancement.replace('%', '');
            }
            else if (tache4.ValeurAvancement.includes('%')) {
                Notify.warning($ctrl.resources.Avancement_SaisieDynamiqueAvecDynamismeDesactive);
            }


            tache4.ValeurAvancement = parseFloat(tache4.ValeurAvancement).toFixed(2);
            if (tache4.TypeAvancement === BudgetService.TypeAvancementEnum.Pourcentage) {
                if (tache4.ValeurAvancement > 100) {
                    tache4.ValeurAvancement = 100;
                }

                tache4.AvancementQte = null;
                tache4.AvancementPourcent = tache4.ValeurAvancement;
            }
            else if (tache4.TypeAvancement === BudgetService.TypeAvancementEnum.Quantite) {

                if (tache4.ValeurAvancement > tache4.Quantite) {
                    tache4.ValeurAvancement = tache4.Quantite;
                }

                tache4.AvancementQte = tache4.ValeurAvancement;
            }

            return initialTypeAvancement !== tache4.TypeAvancement;
        }

        function SetAvancementTypeFromUserInput(tache4) {
            if (tache4.ValeurAvancement.includes('%')) {
                //l'utilisateur a saisi une valeur en pourcentage
                tache4.TypeAvancement = BudgetService.TypeAvancementEnum.Pourcentage;
            }
            else {
                //L'utilisateur a saisi une valeur sans unité donc avancement par quantité 
                tache4.TypeAvancement = BudgetService.TypeAvancementEnum.Quantite;
            }
        }

        function RefreshRowValuesT4(tache) {
            if (tache.TypeAvancement === BudgetService.TypeAvancementEnum.Pourcentage) {
                tache.DAD = tache.Montant * tache.AvancementPourcent / 100;
                tache.CurrentAvancementPourcent = tache.AvancementPourcent;
                tache.ValeurAvancement = tache.AvancementPourcent;
            }
            else if (tache.TypeAvancement === BudgetService.TypeAvancementEnum.Quantite) {
                tache.AvancementQte = (tache.AvancementPourcent / 100 * tache.Quantite).toFixed(2);
                tache.DAD = tache.Montant * tache.AvancementQte / tache.Quantite;
                tache.CurrentAvancementQte = tache.AvancementQte;
                tache.ValeurAvancement = tache.AvancementQte;
            }
            tache.RAD = tache.Montant - tache.DAD;
            tache.EcartAvancement = $ctrl.EcartAvancementT4(tache);
        }

        function PushTacheCommentaireAvancement(listeTache, tache) {
            if (tache.CommentaireAvancement) {
                listeTache.push({
                    TacheId: tache.TacheId,
                    CommentaireAvancement: tache.CommentaireAvancement
                });
            }
        }

        function GetTache4SaveModel(tache4) {
            var tache4SaveModel = {
                TacheId: tache4.TacheId,
                BudgetT4Id: tache4.BudgetT4Id,
                TypeAvancement: tache4.TypeAvancement,
                AvancementPourcent: tache4.AvancementPourcent,
                AvancementQte: tache4.AvancementQte,
                Quantite: tache4.Quantite,
                DAD: tache4.DAD
            };
            return tache4SaveModel;
        }

        function setModifiedValueFlag(tache) {
            if (tache) {
                tache.IsTacheModified = true;
            }
        }

        function TacheValueChanged(tache) {
            var viewValue = GetValue(tache.AvancementPourcent);
            if (viewValue !== tache.CurrentAvancementPourcent) {
                tache.CurrentAvancementPourcent = viewValue;
                return true;
            }
            else {
                return false;
            }
        }

        function Tache4ValueChanged(tache4) {
            var viewValue = null;
            if (tache4.TypeAvancement === BudgetService.TypeAvancementEnum.Pourcentage) {
                viewValue = GetValue(tache4.AvancementPourcent);
                if (viewValue !== tache4.CurrentAvancementPourcent) {
                    tache4.CurrentAvancementPourcent = viewValue;
                    return true;
                }
                else {
                    return false;
                }
            }
            else if (tache4.TypeAvancement === BudgetService.TypeAvancementEnum.Quantite) {
                viewValue = GetValue(tache4.AvancementQte);
                if (viewValue !== tache4.CurrentAvancementQte) {
                    tache4.CurrentAvancementQte = viewValue;
                    tache4.AvancementPourcent = tache4.CurrentAvancementQte / tache4.Quantite * 100;
                    tache4.CurrentAvancementPourcent = tache4.AvancementPourcent;
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        function GetValue(viewValue) {
            if (viewValue === 0) {
                return 0;
            }
            if (!viewValue) {
                return null;
            }
            return Number(viewValue);
        }

        function CalculLibelleBoutonValider() {
            if ($ctrl.Budget.CodeEtatAvancement === $ctrl.CodeEtatAvancement.Enregistre) {
                $ctrl.libelleBoutonValider = $ctrl.resources.Avancement_DemandeValidation;
            }
            else if ($ctrl.Budget.CodeEtatAvancement === $ctrl.CodeEtatAvancement.AValider) {
                $ctrl.libelleBoutonValider = $ctrl.resources.Global_Bouton_Valider;
            }
        }

        // Ouvre le panneau de gestion des commentaires.
        function ShowComment(tache1, tache2, tache3, tache4) {
            $scope.$broadcast(BudgetService.Events.OpenPanelCommentaireAvancement, {
                Tache1: tache1,
                Tache2: tache2,
                Tache3: tache3,
                Tache4: tache4
            });
        }

        // Appelé après la modification d'un commentaire via le panneau.
        function CommentaireHasChanged(arg) {
            arg.Tache.CommentaireAvancement = arg.Commentaire;
        }


        // initialisation des totaux de taches de niveau 1
        function InitTotalTacheNiveau1() {
            $ctrl.TotalDADPrevious = null;
            $ctrl.TotalDAD = null;
            $ctrl.TotalEcartDAD = null;
            $ctrl.TotalRAD = null;
            $ctrl.TotalQuantiteRAD = null;
            $ctrl.TotalMontant = null;
            $ctrl.TotalPourcentageRAD = null;
            $ctrl.TotalAvancementPourcent = null;
            $ctrl.TotalAvancementPourcentPrevious = null;
            $ctrl.TotalEcartAvancement = null;
        }

        // calcul des totaux pour toutes les taches de niveau 1 
        function SommeTotalTachesNiveau1() {
            if ($ctrl.Budget === null || $ctrl.Budget.TachesNiveau1 === null)
                return;

            var totalDADPrevious = 0;
            var totalDAD = 0;
            var totalEcartDAD = 0;
            var totalRAD = 0;
            var totalQuantiteRAD = 0;
            var totalMontant = 0;
            var totalMontantAvancement = 0;

            $ctrl.Budget.TachesNiveau1.forEach((t1) => {

                if (t1.DADPrevious)
                    totalDADPrevious += t1.DADPrevious;

                if (t1.DAD)
                    totalDAD += t1.DAD;

                var ecartDAD = $ctrl.EcartDAD(t1);
                if (ecartDAD)
                    totalEcartDAD += ecartDAD;

                if (t1.RAD)
                    totalRAD += t1.RAD;

                var quantiteRAD = $ctrl.CalculQuantiteRAD(t1);
                if (quantiteRAD) {
                    totalQuantiteRAD += quantiteRAD;
                }

                if (t1.Montant)
                    totalMontant += t1.Montant;

                if (t1.Montant && t1.AvancementPourcent)
                    totalMontantAvancement += t1.Montant * t1.AvancementPourcent / 100;
            });

            $ctrl.TotalDADPrevious = totalDADPrevious;
            $ctrl.TotalDAD = totalDAD;
            $ctrl.TotalEcartDAD = totalEcartDAD;
            $ctrl.TotalRAD = totalRAD;
            $ctrl.TotalQuantiteRAD = totalQuantiteRAD;
            $ctrl.TotalMontant = totalMontant;
            $ctrl.TotalPourcentageRAD = totalRAD / totalMontant * 100;
            $ctrl.TotalAvancementPourcent = totalMontantAvancement / totalMontant * 100;
            $ctrl.TotalAvancementPourcentPrevious = totalDADPrevious / totalMontant * 100;
            $ctrl.TotalEcartAvancement = $ctrl.TotalAvancementPourcent - $ctrl.TotalAvancementPourcentPrevious;
        }

        function GetAvancementPrevious(tache4) {
            if (tache4.AvancementQtePrevious) {
                return tache4.AvancementQtePrevious;
            }

            return tache4.AvancementPourcentPrevious;
        }


        //////////////////////////////////////////////////////////////////
        // Evènements                                                   //
        //////////////////////////////////////////////////////////////////

        function onToutPlier() {
            $(".collapse").collapse('hide');
            hideOrShowAll(true);
        }

        function onToutDeplier() {
            $(".collapse").collapse('show');
            hideOrShowAll(false);
        }

        function updateFilters() {
            sessionStorage.setItem('avancementFilter', JSON.stringify($ctrl.filters));
        }


        function onClickHideOrShowChildren(tache) {

            if (tache.childrenHidden === true) {
                tache.childrenHidden = false;
            }
            else {
                tache.childrenHidden = true;
            }
        }

        /**
         * Mets à jour les variables childrenHidden des taches avec la valeur du paramètre
         * @param {any} hideOrShow true si tous les enfants sont cachés, false si ils sont tous affichés
         */
        function hideOrShowAll(hideOrShow) {
            for (let t1 of $ctrl.Budget.TachesNiveau1) {
                t1.childrenHidden = hideOrShow;
                for (let t2 of t1.TachesNiveau2) {
                    t2.childrenHidden = hideOrShow;
                    for (let t3 of t2.TachesNiveau3) {
                        t3.childrenHidden = hideOrShow;
                    }
                }
            }
        }

        function getTooltipFavoris() {
            if ($ctrl.filters.ci && $ctrl.filters.periode) {
                var periode = $ctrl.filters.periode.getMonth() + 1 + "/" + $ctrl.filters.periode.getFullYear();
                return resources.Avancement_Tooltip_Favoris_Part1 + $ctrl.filters.ci.CodeLibelle + resources.Avancement_Tooltip_Favoris_Part2 + periode;
            }
            return "";
        }

        /*
        * @function addFilter2Favoris()
        * @description Crée un nouveau favori
        */
        function addFilter2Favoris() {
            var filter = {
                ValueText: $ctrl.filters.searchTache,
                CI: $ctrl.filters.ci,
                Periode: $ctrl.filters.periode,
                AxesAnalytiquesAffiches: $ctrl.filters.shownAnalyticalAxis.map(axe => axe.Name)
            };
            if ($ctrl.filters.shownColumns[0] !== undefined && $ctrl.filters.shownColumns[0].name !== undefined) {
                filter.ColonnesAffichees = $ctrl.filters.shownColumns.map(axe => axe.name);
            } else {
                filter.ColonnesAffichees = $ctrl.filters.shownColumns;
            }

            var url = $window.location.pathname;
            if ($ctrl.favoriId !== 0) {
                url = $window.location.pathname.slice(0, $window.location.pathname.lastIndexOf("/"));
            }
            favorisService.initializeAndOpenModal("Avancement", url, filter);
        }

        function getFilterOrFavoris(favoriId) {
            $ctrl.favoriId = parseInt(favoriId);
            if ($ctrl.favoriId !== 0) {
                favorisService.getFilterByFavoriIdOrDefault({ favoriId: $ctrl.favoriId, defaultFilter: $ctrl.filter })
                    .then(function (response) {
                        $timeout(function () {
                            $ctrl.filters = {
                                ci: response.CI,
                                searchTache: response.ValueText,
                                periode: new Date(response.Periode),
                                shownColumns: response.ColonnesAffichees
                            };
                            periodeCourante = $ctrl.filters.periode;
                            var tempAxesAnalytiques = [];
                            response.AxesAnalytiquesAffiches.forEach(function (element) {
                                tempAxesAnalytiques.push($ctrl.showableAnalyticalAxis.find(axe => axe.Name === element));
                            });
                            $ctrl.filters.shownAnalyticalAxis = tempAxesAnalytiques;
                            $ctrl.shownAnalyticalAxisBeforeCancel = tempAxesAnalytiques;
                            $ctrl.shownColumnsBeforeCancel = response.ColonnesAffichees;

                            var colonnesAffichees = [];
                            response.ColonnesAffichees.forEach(function (element) {
                                colonnesAffichees.push($ctrl.showableColumns.find(axe => axe.name === element));
                            });
                            $ctrl.shownColumns = colonnesAffichees;
                            $ctrl.shownAnalyticalAxis = tempAxesAnalytiques;

                            adjustColumnsWidth();
                            checkFilterActiveOnAxe();

                            Load();
                        });
                    }).catch(function (error) { console.log(error); });
            }
            else {
                if (sessionStorage.getItem('avancementFilter') !== null) {
                    $ctrl.filters = JSON.parse(sessionStorage.getItem('avancementFilter'));
                    $ctrl.filters.periode = new Date($ctrl.filters.periode);
                    Load();
                }
            }
        }

        function isShowableColumn(columnName) {
            return $ctrl.shownColumns.find(col => col.name === columnName) !== undefined;
        };
    }
}(angular));
