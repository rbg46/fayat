(function (angular) {
    'use strict';

    angular.module('Fred').controller('AvancementRecetteController', AvancementRecetteController);

    AvancementRecetteController.$inject = ['$q', 'ProgressBar', 'BudgetService', 'Notify'];

    function AvancementRecetteController($q, ProgressBar, BudgetService, Notify) {
        var $ctrl = this;

        //////////////////////////////////////////////////////////////////
        // Membres  publiques                                           //
        //////////////////////////////////////////////////////////////////
        $ctrl.busy = false;
        $ctrl.PFAEdit = false;
        $ctrl.loaded = false;
        var periodeCourante;
        $ctrl.resources = resources;
        var PFAOrigin = {
            MontantAvenantsPFA: null, MontantMarchePFA: null, SommeAValoirPFA: null, TauxFraisGenerauxPFA: null,
            RevisionPFA: null, AjustementFraisGenerauxPFA: null, PenalitesEtRetenuesPFA: null
        };

        $ctrl.ChangePeriode = ChangePeriode;
        $ctrl.ChangePerimetre = ChangePerimetre;
        $ctrl.Load = Load;
        $ctrl.Save = Save;
        $ctrl.TotalAvancement = TotalAvancement;
        $ctrl.TotalPFA = TotalPFA;
        $ctrl.TotalFacture = TotalFacture;
        $ctrl.PFAHasChanged = PFAHasChanged;
        $ctrl.TotalAvancementAjustementFraisGeneraux = TotalAvancementAjustementFraisGeneraux;
        $ctrl.TotalAvancementTauxFraisGeneraux = TotalAvancementTauxFraisGeneraux;
        $ctrl.RoundValue = RoundValue;

        //////////////////////////////////////////////////////////////////
        // Initialisation                                               //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {
            if (sessionStorage.getItem('avancementRecetteFilter') !== null) {
                $ctrl.filters = JSON.parse(sessionStorage.getItem('avancementRecetteFilter'));
                $ctrl.periode = new Date($ctrl.filters.periode);
                periodeCourante = $ctrl.periode;
                $ctrl.filters.ci = $ctrl.filters.ci;
                Load();
            }
            else {
                $ctrl.periode = new Date();
                periodeCourante = $ctrl.periode;
            }
        };

        //////////////////////////////////////////////////////////////////
        // Chargement                                                   //
        //////////////////////////////////////////////////////////////////
        function ChangePeriode() {
            $q.when()
                .then(() => {
                    if ($ctrl.periode && (!periodeCourante || !IsSamePeriod($ctrl.periode, periodeCourante))) {
                        periodeCourante = $ctrl.periode;
                        ChangePerimetre();
                    }
                });
        }

        function ChangePerimetre() {
            if ($ctrl.filters.ci && $ctrl.periode) {
                $ctrl.filters.periode = $ctrl.periode;
                sessionStorage.setItem('avancementRecetteFilter', JSON.stringify($ctrl.filters));
                Load();
            }
        }

        function GetPeriodeFormat(date) {
            var year = date.getFullYear();
            var month = date.getMonth() + 1;
            return year * 100 + month;
        }

        // Indique si 2 dates sont sur la même période
        // return : true si les 2 dates sont sur la même période, sinon false
        function IsSamePeriod(date1, date2) {
            if (date1 && date2) {
                return date1.getFullYear() === date2.getFullYear() && date1.getMonth() === date2.getMonth();
            }
            else {
                return false;
            }            
        }

        function ConvertDateToPeriod(date) {
            return date.getFullYear() * 100 + date.getMonth() + 1;
        }

        function SavePFAOrigin() {
            PFAOrigin.MontantAvenantsPFA = $ctrl.AvancementRecette.MontantAvenantsPFA;
            PFAOrigin.MontantMarchePFA = $ctrl.AvancementRecette.MontantMarchePFA;
            PFAOrigin.SommeAValoirPFA = $ctrl.AvancementRecette.SommeAValoirPFA;
            PFAOrigin.TauxFraisGenerauxPFA = $ctrl.AvancementRecette.TauxFraisGenerauxPFA;
            PFAOrigin.RevisionPFA = $ctrl.AvancementRecette.RevisionPFA;
            PFAOrigin.AjustementFraisGenerauxPFA = $ctrl.AvancementRecette.AjustementFraisGenerauxPFA;
            PFAOrigin.PenalitesEtRetenuesPFA = $ctrl.AvancementRecette.PenalitesEtRetenuesPFA;
        }

        function PFAHasChanged() {
            return PFAOrigin.MontantAvenantsPFA !== $ctrl.AvancementRecette.MontantAvenantsPFA ||
                PFAOrigin.MontantMarchePFA !== $ctrl.AvancementRecette.MontantMarchePFA ||
                PFAOrigin.SommeAValoirPFA !== $ctrl.AvancementRecette.SommeAValoirPFA ||
                PFAOrigin.TauxFraisGenerauxPFA !== $ctrl.AvancementRecette.TauxFraisGenerauxPFA ||
                PFAOrigin.RevisionPFA !== $ctrl.AvancementRecette.RevisionPFA ||
                PFAOrigin.AjustementFraisGenerauxPFA !== $ctrl.AvancementRecette.AjustementFraisGenerauxPFA ||
                PFAOrigin.PenalitesEtRetenuesPFA !== $ctrl.AvancementRecette.PenalitesEtRetenuesPFA;
        }

        function Load() {
            ProgressBar.start();
            if (!$ctrl.periode && periodeCourante) {
                $ctrl.periode = periodeCourante;
            }

            $ctrl.busy = true;

            BudgetService.GetAvancementRecette($ctrl.filters.ci.CiId, GetPeriodeFormat(periodeCourante))
                .then(GetDetailThen)
                .catch(GetDetailCatch)
                .finally(Finally);
        }

        function GetDetailThen(result) {
            $ctrl.AvancementRecette = result.data;
            $ctrl.loaded = true;
            if ($ctrl.AvancementRecette.Erreurs.length > 0) {
                Notify.error($ctrl.AvancementRecette.Erreurs);
            }
            SavePFAOrigin();
        }

        function GetDetailCatch() {
            Notify.error($ctrl.resources.Global_Notification_Chargement_Error);
        }

        function Finally() {
            LockPFA();
            $ctrl.busy = false;
            ProgressBar.complete();
        }

        //////////////////////////////////////////////////////////////////
        // Save                                                         //
        //////////////////////////////////////////////////////////////////
        function Save() {
            ProgressBar.start();
            $ctrl.busy = true;
            var saveModel = {
                AvancementRecetteId: $ctrl.AvancementRecette.AvancementRecetteId, BudgetRecetteId: $ctrl.AvancementRecette.BudgetRecette.BudgetRecetteId,
                Periode: ConvertDateToPeriod($ctrl.periode), MontantMarche: $ctrl.AvancementRecette.MontantMarche,
                MontantAvenants: $ctrl.AvancementRecette.MontantAvenants, SommeAValoir: $ctrl.AvancementRecette.SommeAValoir,
                TravauxSupplementaires: $ctrl.AvancementRecette.TravauxSupplementaires, Revision: $ctrl.AvancementRecette.Revision,
                AutresRecettes: $ctrl.AvancementRecette.AutresRecettes, PenalitesEtRetenues: $ctrl.AvancementRecette.PenalitesEtRetenues,
                MontantMarchePFA: $ctrl.AvancementRecette.MontantMarchePFA, MontantAvenantsPFA: $ctrl.AvancementRecette.MontantAvenantsPFA,
                SommeAValoirPFA: $ctrl.AvancementRecette.SommeAValoirPFA, TravauxSupplementairesPFA: $ctrl.AvancementRecette.TravauxSupplementairesPFA,
                RevisionPFA: $ctrl.AvancementRecette.RevisionPFA, AutresRecettesPFA: $ctrl.AvancementRecette.AutresRecettesPFA,
                PenalitesEtRetenuesPFA: $ctrl.AvancementRecette.PenalitesEtRetenuesPFA, Correctif: $ctrl.AvancementRecette.Correctif,
                TauxFraisGeneraux: $ctrl.AvancementRecette.TauxFraisGeneraux, AjustementFraisGeneraux: $ctrl.AvancementRecette.AjustementFraisGeneraux,
                TauxFraisGenerauxPFA: $ctrl.AvancementRecette.TauxFraisGenerauxPFA, AjustementFraisGenerauxPFA: $ctrl.AvancementRecette.AjustementFraisGenerauxPFA,                    
                AvancementTauxFraisGeneraux: $ctrl.AvancementRecette.AvancementTauxFraisGeneraux, AvancementAjustementFraisGeneraux: $ctrl.AvancementRecette.AvancementAjustementFraisGeneraux};
            BudgetService.SaveAvancementRecette(saveModel)
                .then(SaveThen)
                .catch(SaveCatch)
                .finally(Finally);                
        }

        function SaveThen(result) {
            $ctrl.AvancementRecette.AvancementRecetteId = result.data;
            Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
        }

        function SaveCatch() {
            Notify.error($ctrl.resources.Global_Notification_Error);
        }

        //////////////////////////////////////////////////////////////////
        // Calcul                                                       //
        //////////////////////////////////////////////////////////////////
        function TotalAvancement() {
            if ($ctrl.AvancementRecette) {
                return (parseFloat($ctrl.AvancementRecette.MontantMarche) + parseFloat($ctrl.AvancementRecette.MontantAvenants) +
                    parseFloat($ctrl.AvancementRecette.SommeAValoir) + parseFloat($ctrl.AvancementRecette.TravauxSupplementaires) +
                    parseFloat($ctrl.AvancementRecette.Revision) + parseFloat($ctrl.AvancementRecette.AutresRecettes) +
                    parseFloat($ctrl.AvancementRecette.PenalitesEtRetenues)).toFixed(2);
            }
            else {
                return 0;
            }
        }

        function TotalPFA() {
            if ($ctrl.AvancementRecette) {
                return (parseFloat($ctrl.AvancementRecette.MontantMarchePFA) + parseFloat($ctrl.AvancementRecette.MontantAvenantsPFA) +
                    parseFloat($ctrl.AvancementRecette.SommeAValoirPFA) + parseFloat($ctrl.AvancementRecette.TravauxSupplementairesPFA) +
                    parseFloat($ctrl.AvancementRecette.RevisionPFA) + parseFloat($ctrl.AvancementRecette.AutresRecettesPFA) +
                    parseFloat($ctrl.AvancementRecette.PenalitesEtRetenuesPFA)).toFixed(2);
            }
            else {
                return 0;
            }
        }

        function TotalFacture() {
            if ($ctrl.AvancementRecette) {
                return (parseFloat($ctrl.TotalAvancement()) + parseFloat($ctrl.AvancementRecette.Correctif)).toFixed(2);
            }
            else {
                return 0;
            }
        }

        function TotalAvancementTauxFraisGeneraux() {
            if ($ctrl.AvancementRecette.AvancementTauxFraisGeneraux !== null && $ctrl.AvancementRecette.AvancementTauxFraisGenerauxPrevious !== null) {
                return ($ctrl.AvancementRecette.AvancementTauxFraisGeneraux - $ctrl.AvancementRecette.AvancementTauxFraisGenerauxPrevious).toFixed(2);
            }
            else {
                return 0;
            }
        }

        function TotalAvancementAjustementFraisGeneraux() {
            if ($ctrl.AvancementRecette.AvancementAjustementFraisGeneraux && $ctrl.AvancementRecette.AvancementAjustementFraisGenerauxPrevious) {
                return ($ctrl.AvancementRecette.AvancementAjustementFraisGeneraux - $ctrl.AvancementRecette.AvancementAjustementFraisGenerauxPrevious).toFixed(2);
            }
            else {
                return 0;
            }
        }

        function LockPFA() {
            $ctrl.PFAEdit = false;
        }

        function RoundValue(value) {
            return value.toFixed(2);
        }
    }
}(angular));
