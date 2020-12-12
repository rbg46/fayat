
(function () {
    'use strict';

    angular.module('Fred').service('PointageHelperService', PointageHelperService);

    PointageHelperService.$inject = ['$log'];

    function PointageHelperService($log) {

        var pointageSelected = {};
        var indexSelected = 0;
        var libPrimeAbregeMaxLength = 2;
        var isUserFes = null;

        // Constantes
        // Statut ETAM
        var StatutsETAM = ["2", "4", "5"];
        // Statut Ouvrier et Cadre
        var StatutsCO = ["1", "3"];

        var service = {
            getPointage: getPointage,
            setPointage: setPointage,
            getPointageIndex: getPointageIndex,
            setPointageIndex: setPointageIndex,
            actionIsUpdated: actionIsUpdated,
            refreshTotalHeure: refreshTotalHeure,
            refreshHeureNormale: refreshHeureNormale,
            setNumSemaineIntemperie: setNumSemaineIntemperie,
            heuresAbsenceMin: heuresAbsenceMin,
            heuresAbsenceMax: heuresAbsenceMax,
            refreshLibelleAbregePrime: refreshLibelleAbregePrime,
            refreshHeuresAbsenceDefaut: refreshHeuresAbsenceDefaut,
            IsPrimeHoraire: IsPrimeHoraire,
            IsPrimeJournaliere: IsPrimeJournaliere,
            getIsUserFes: getIsUserFes,
            setIsUserFes: setIsUserFes,
            initLock: initLock
        };

        return service;

        //////////////////////////////////////////////////////////////////
        // PUBLICS METHODES                                             //
        //////////////////////////////////////////////////////////////////

        // index : l'index du pointag dans la liste
        function setPointageIndex(pointage, index) {
            pointageSelected = pointage;
            indexSelected = index;
        }

        function getPointageIndex() {
            return { pointage: pointageSelected, index: indexSelected };
        }

        function setPointage(pointage) {
            pointageSelected = pointage;
        }

        function getPointage() {
            return pointageSelected;
        }
        function getIsUserFes() {
            return isUserFes;
        }

        function setIsUserFes(value) {
            isUserFes = value;
        }

        function heuresAbsenceMin(pointage) {
            if (getIsUserFes()) {
                return 0;
            } else {
                if (pointage.CodeAbsence) {

                    if (pointage.Personnel.Statut === "3" || pointage.Personnel.Statut === "1") {
                        return pointage.CodeAbsence.NBHeuresMinCO;
                    }
                    else {
                        return pointage.CodeAbsence.NBHeuresMinETAM;
                    }
                }
                else {
                    return 0;
                }

            }
        }

        function heuresAbsenceMax(pointage) {
            if (getIsUserFes() && pointage.Personnel) {
                if (pointage.Personnel.Statut === "3") {
                    return 1;
                }
                else {
                    return 7;
                }
            } else {
                if (pointage.CodeAbsence) {
                    if (pointage.Personnel.Statut === "2" || pointage.Personnel.Statut === "4" || pointage.Personnel.Statut === "5") {
                        return pointage.CodeAbsence.NBHeuresMaxETAM;
                    }
                    if (pointage.Personnel.Statut === "3" || pointage.Personnel.Statut === "1") {
                        return pointage.CodeAbsence.NBHeuresMaxCO;
                    }
                    else {
                        return 12;
                    }
                }
                else {
                    return 12;
                }
            }
        }

        function setNumSemaineIntemperie(pointage) {
            pointage.NumSemaineIntemperieAbsence = null;
            if (pointage.CodeAbsence && pointage.CodeAbsence.Intemperie) {
                pointage.NumSemaineIntemperieAbsence = parseInt(moment().format('W'));
            }
        }

        function refreshTotalHeure(pointage) {
            var sum = 0;
            for (var i = 0; i < pointage.ListRapportLigneTaches.length; i++) {
                if (!pointage.ListRapportLigneTaches[i].IsDeleted) {
                    sum += parseFloat(pointage.ListRapportLigneTaches[i].HeureTache);
                }
            }
            sum = isNaN(parseFloat(Math.trunc(sum * 100) / 100)) ? 0 : parseFloat(Math.trunc(sum * 100) / 100);
            pointage.HeureTotalTravail = sum;
            if (pointage.MaterielId) {
                pointage.MaterielMarche = sum;
            }
        }

        function refreshHeureNormale(pointage) {
            var sum = 0;
            for (var i = 0; i < pointage.ListRapportLigneTaches.length; i++) {
                if (!pointage.ListRapportLigneTaches[i].IsDeleted) {
                    sum += parseFloat(pointage.ListRapportLigneTaches[i].HeureTache);
                }
            }
            sum = isNaN(parseFloat(Math.trunc(sum * 100) / 100)) ? 0 : parseFloat(Math.trunc(sum * 100) / 100);

            pointage.HeureNormale = sum - (isNaN(parseFloat(pointage.HeureMajoration)) ? 0 : parseFloat(pointage.HeureMajoration));
        }

        // Retourne vrai si la prime est de type horaire
        function IsPrimeHoraire(prime) {
            if (prime) {
                return prime.PrimeType === 1;
            }
            else {
                return false;
            }
        }

        // Retourne vrai si la prime est de type journaliere
        function IsPrimeJournaliere(prime) {
            if (prime) {
                return prime.PrimeType === 0;
            }
            else {
                return false;
            }
        }

        function refreshLibelleAbregePrime(pointage) {
            var listPrimes = pointage.ListRapportLignePrimes;
            var libPrimeAbrege = '';
            var count = 0;
            for (var j = 0; j < listPrimes.length && count < 3; j++) {
                if (count < libPrimeAbregeMaxLength) {
                    if (!listPrimes[j].IsDeleted && (IsPrimeJournaliere(listPrimes[j].Prime) && listPrimes[j].IsChecked || IsPrimeHoraire(listPrimes[j].Prime) && listPrimes[j].HeurePrime > 0)) {
                        count++;
                        if (libPrimeAbrege.length > 0) {
                            libPrimeAbrege += ', ';
                        }
                        libPrimeAbrege += listPrimes[j].Prime.Code;
                    }
                }
                else {
                    if (!listPrimes[j].IsDeleted && (IsPrimeJournaliere(listPrimes[j].Prime) && listPrimes[j].IsChecked || IsPrimeHoraire(listPrimes[j].Prime) && listPrimes[j].HeurePrime > 0)) {
                        libPrimeAbrege += ', ...';
                        count++;
                    }
                }
            }
            pointage.LibPrimeAbrege = libPrimeAbrege;
        }

        function refreshHeuresAbsenceDefaut(pointage) {
            if (pointage.CodeAbsence) {
                if (StatutsETAM.indexOf(pointage.Personnel.Statut) !== -1) {
                    pointage.HeureAbsence = pointage.CodeAbsence.NBHeuresDefautETAM;
                } else if (StatutsCO.indexOf(pointage.Personnel.Statut) !== -1) {
                    pointage.HeureAbsence = pointage.CodeAbsence.NBHeuresDefautCO;
                    pointage.HeureAbsenceIac = pointage.HeureAbsence > 0 ? pointage.HeureAbsence / 7 : 0;
                    pointage.HeureNormaleIac = pointage.HeureNormale > 0 ? pointage.HeureNormale / 7 : 0;
                }
                else {
                    pointage.HeureAbsence = 0;
                }
            }
            else {
                pointage.HeureAbsence = 0;
            }
            pointage.HeureAbsenceIac = pointage.HeureAbsence > 0 ? pointage.HeureAbsence / 7 : 0;
            pointage.HeureNormaleIac = pointage.HeureNormale > 0 ? pointage.HeureNormale / 7 : 0;
        }

        function actionIsUpdated(pointage) {
            pointage.IsUpdated = pointage.PointageId > 0;
        }

        function initLock(pointage, personnel) {
            if (pointage.PointageId) {
                pointage.IsLocked = !pointage.IsCreated
                    && (!pointage.MonPerimetre || pointage.Cloture || pointage.IsGenerated);
            }
            if (!personnel.IsInterimaire && (personnel.DateSortie && pointage.Day.Date > personnel.DateSortie || personnel.DateEntree > pointage.Day.Date)) {
                pointage.NewPointageLocked = true;
            }
        }
    }
})();