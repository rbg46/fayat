using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Personnel.Interimaire;
using Fred.Business.Rapport.Common.Duplication;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.DatesClotureComptable;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Rapport;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.Rapport.Pointage.Duplication
{
    public class PointageDuplicatorService : IPointageDuplicatorService
    {
        private readonly IPointageRepository pointageRepository;
        private readonly IDatesClotureComptableManager datesClotureComptableManager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IContratInterimaireManager contratInterimaireManager;

        public PointageDuplicatorService(
            IPointageRepository pointageRepository,
            IDatesClotureComptableManager datesClotureComptableManager,
            IUtilisateurManager utilisateurManager,
            IContratInterimaireManager contratInterimaireManager)
        {
            this.pointageRepository = pointageRepository;
            this.datesClotureComptableManager = datesClotureComptableManager;
            this.utilisateurManager = utilisateurManager;
            this.contratInterimaireManager = contratInterimaireManager;
        }

        /// <summary>
        /// Recupere le pointage pour faire une duplication
        /// </summary>
        /// <param name="rapportLigneId">le pointage id a dupliquer</param>
        /// <returns>Le pointage</returns>
        public RapportLigneEnt GetPointageForDuplication(int rapportLigneId)
        {
            var includes = new List<Expression<Func<RapportLigneEnt, object>>>();

            includes.Include(x => x.Personnel.Societe)
                    .Include(x => x.Ci)
                    .Include(x => x.CodeAbsence)
                    .Include(x => x.ListRapportLigneTaches)
                    .Include(x => x.ListRapportLigneAstreintes)
                    .Include(x => x.ListRapportLigneMajorations)
                    .Include(x => x.ListRapportLignePrimes);

            RapportLigneEnt pointageToDuplicate = pointageRepository.Get(rapportLigneId, includes);

            return pointageToDuplicate;
        }

        /// <summary>
        /// Duplique un pointage
        /// </summary>
        /// <param name="pointagesToDuplicate">Les pointages que l'on duplique</param>
        /// <param name="ciId">Le ciId avec lequel on fait la duplication</param>
        /// <param name="startDate">La date de debut de duplication</param>
        /// <param name="endDate">La date de FIN de duplication</param>
        /// <returns>Le resultat d'une duplication de pointage</returns>
        public List<DuplicatePointageResult> DuplicatePointages(List<RapportLigneEnt> pointagesToDuplicate, int ciId, DateTime startDate, DateTime endDate)
        {

            var result = new List<DuplicatePointageResult>();
            var dateClotures = this.datesClotureComptableManager.GetListDatesClotureComptableByCiGreaterThanPeriode(ciId, startDate.Month, startDate.Year);
            var listPersonnelIds = pointagesToDuplicate.Where(p => p != null && p.PersonnelId.HasValue).Select(p => p.PersonnelId.Value).ToList();
            var contratInterimaires = this.contratInterimaireManager.GetListContratInterimaireOpenOnPeriod(listPersonnelIds, startDate, endDate);

            foreach (var pointageToDuplicate in pointagesToDuplicate)
            {
                var duplicatePointageResult = DuplicatePointageInternal(pointageToDuplicate, ciId, startDate, endDate, dateClotures, contratInterimaires);

                result.Add(duplicatePointageResult);
            }

            return result;
        }

        /// <summary>
        /// Duplique un pointage
        /// POUR UNE LISTE DE POINTAGES UTILISER 'DuplicatePointages'
        /// </summary>
        /// <param name="pointageToDuplicate">Le pointage que l'on duplique</param>
        /// <param name="ciId">Le ciId avec lequel on fait la duplication</param>
        /// <param name="startDate">La date de debut de duplication</param>
        /// <param name="endDate">La date de FIN de duplication</param>
        /// <returns>Le resultat d'une duplication de pointage</returns>
        public DuplicatePointageResult DuplicatePointage(RapportLigneEnt pointageToDuplicate, int ciId, DateTime startDate, DateTime endDate)
        {
            var dateClotures = this.datesClotureComptableManager.GetListDatesClotureComptableByCiGreaterThanPeriode(ciId, startDate.Month, startDate.Year);
            var listPersonnelIds = pointageToDuplicate.PersonnelId.HasValue ? new List<int>() { pointageToDuplicate.PersonnelId.Value } : new List<int>();
            var contratInterimaires = this.contratInterimaireManager.GetListContratInterimaireOpenOnPeriod(listPersonnelIds, startDate, endDate);
            var duplicatePointageResult = DuplicatePointageInternal(pointageToDuplicate, ciId, startDate, endDate, dateClotures, contratInterimaires);

            return duplicatePointageResult;
        }

        private DuplicatePointageResult DuplicatePointageInternal(RapportLigneEnt pointageToDuplicate, int ciId, DateTime startDate, DateTime endDate, IEnumerable<DatesClotureComptableEnt> dateClotures, List<ContratInterimaireEnt> contratInterimaires)
        {
            bool isUtilisateurConnectedFes = utilisateurManager.IsUtilisateurOfGroupe(Constantes.CodeGroupeFES);
            var result = new DuplicatePointageResult();

            if (pointageToDuplicate == null)
            {
                throw new FredBusinessNotFoundException(FeatureRapport.Pointage_Duppliquer_Exception);
            }

            result.PointageToDuplicate = pointageToDuplicate;

            if (!pointageToDuplicate.HeureNormale.IsZero() && pointageToDuplicate.CiId != ciId)
            {
                throw new FredBusinessNotFoundException(FeatureRapport.Pointage_Modifier_CI_Exception);
            }

            var daysInPeriod = DuplicationTimeHelper.GetAllDaysInPeriode(startDate, endDate);
            var daysWithoutWeekend = DuplicationTimeHelper.FiltersWeekEnd(daysInPeriod);
            result.DuplicationOnlyOnWeekend = DuplicationTimeHelper.GetIfPeriodIsOnlyOnWeekendOrWeekEnd(daysInPeriod);
            var daysActiveForPersonnel = FilterDaysWithDateSortiePersonnel(daysWithoutWeekend, pointageToDuplicate?.Personnel?.DateSortie);
            result.PersonnelIsInactiveInPeriode = GetIfPersonnelIsInactiveInPeriode(daysWithoutWeekend, pointageToDuplicate?.Personnel?.DateSortie);
            var duplicateDays = FilterDaysForInterimaire(daysActiveForPersonnel, contratInterimaires, pointageToDuplicate);
            result.InterimaireDuplicationState = DuplicationTimeHelper.GetInterimaireDuplicationState(daysActiveForPersonnel, duplicateDays, pointageToDuplicate.Personnel, contratInterimaires);
            result.HasPartialDuplicationInDifferentZoneDeTravail = HasPartialDuplicationOnDifferentZoneDeTravail(duplicateDays, contratInterimaires, pointageToDuplicate, result.InterimaireDuplicationState);
            var hasDatesInClosedMonth = datesClotureComptableManager.HasDatesInClosedMonth(ciId, dateClotures, duplicateDays);

            if (hasDatesInClosedMonth)
            {
                result.HasDatesInClosedMonth = true;
                return result;
            }
            if (isUtilisateurConnectedFes)
            {
                duplicateDays = FilterDaysWithCiInFes(pointageToDuplicate, duplicateDays, ciId, startDate, endDate);
            }
            foreach (var duplicateDay in duplicateDays)
            {
                // On créer une copie du pointage
                var duplicatedPointage = DuplicatePointageReel(pointageToDuplicate, emptyValues: false, isUtilisateurConnectedFes: isUtilisateurConnectedFes);
                duplicatedPointage.DatePointage = duplicateDay.Date;
                duplicatedPointage.CiId = ciId;
                duplicatedPointage.RapportId = 0;
                if (duplicatedPointage.Personnel != null && duplicatedPointage.Personnel.IsInterimaire)
                {
                    duplicatedPointage.ContratId = GetContrat(duplicateDay.Date, contratInterimaires.Where(c => c.InterimaireId == duplicatedPointage.PersonnelId))?.ContratInterimaireId;
                }
                duplicatedPointage.Personnel = null;
                result.DuplicatedRapportLignes.Add(duplicatedPointage);
            }

            return result;
        }

        private ContratInterimaireEnt GetContrat(DateTime date, IEnumerable<ContratInterimaireEnt> contratInterimaires)
        {
            ContratInterimaireEnt contrat = null;
            IEnumerable<ContratInterimaireEnt> normalizedContracts = contratInterimaires.Where(c => c.DateDebut <= date && date <= c.DateFin.AddDays(c.Souplesse));
            if (normalizedContracts.Count() == 1)
            {
                contrat = normalizedContracts.First();
            }
            else if (normalizedContracts.Count() > 1)
            {
                contrat = normalizedContracts.FirstOrDefault(c => c.DateDebut <= date && date <= c.DateFin);
            }

            return contrat;
        }

        private List<DateTime> FilterDaysWithCiInFes(RapportLigneEnt pointageToDuplicate, List<DateTime> allDays, int ciId, DateTime startDate, DateTime endDate)
        {
            Managers managers = new Managers();
            var getpointage = managers.Pointage.GetPointageByCiPersonnelAndDates(ciId, pointageToDuplicate.PersonnelId.Value, startDate, endDate);
            var datesPointage = getpointage.Select(i => i.DatePointage);
            var listDateFes = allDays.Where(day => !datesPointage.Any(dt => dt == day)).ToList();
            return listDateFes;
        }

        private List<DateTime> FilterDaysWithDateSortiePersonnel(List<DateTime> allDays, DateTime? dateSortiePersonnel)
        {
            var result = new List<DateTime>();

            foreach (var day in allDays)
            {
                if (dateSortiePersonnel.HasValue)
                {
                    var normalizedDay = new DateTime(day.Year, day.Month, day.Day);
                    if (normalizedDay <= new DateTime(dateSortiePersonnel.Value.Year, dateSortiePersonnel.Value.Month, dateSortiePersonnel.Value.Day))
                    {
                        result.Add(day);
                    }
                }
                else
                {
                    result.Add(day);
                }
            }
            return result.Distinct().ToList();
        }

        private bool GetIfPersonnelIsInactiveInPeriode(List<DateTime> allDays, DateTime? dateSortiePersonnel)
        {
            foreach (var day in allDays)
            {
                var normalizedDay = new DateTime(day.Year, day.Month, day.Day);

                if (dateSortiePersonnel.HasValue && normalizedDay > new DateTime(dateSortiePersonnel.Value.Year, dateSortiePersonnel.Value.Month, dateSortiePersonnel.Value.Day))
                {
                    return true;
                }
            }
            return false;
        }

        private List<DateTime> FilterDaysForInterimaire(List<DateTime> allDays, List<ContratInterimaireEnt> contratInterimairesForCiAndPeriode, RapportLigneEnt pointage)
        {
            var result = new List<DateTime>();

            if (pointage.Personnel == null)
            {
                return allDays.ToList();
            }

            if (!pointage.Personnel.IsInterimaire)
            {
                return allDays.ToList();
            }

            var contratInterimaires = contratInterimairesForCiAndPeriode.Where(c => c.InterimaireId == pointage.Personnel.PersonnelId);
            foreach (var item in allDays)
            {
                var normalizedDate = new DateTime(item.Year, item.Month, item.Day);
                ContratInterimaireEnt contrat = GetContrat(normalizedDate, contratInterimaires);

                if (contrat != null && contrat.ZonesDeTravail.Any(z => z.EtablissementComptableId == pointage.Ci.EtablissementComptableId))
                {
                    result.Add(item);
                }
            }

            return result.Distinct().ToList();
        }

        /// <summary>
        ///   Duplique une liste de lignes de rapport
        /// </summary>
        /// <param name="listPointage">La liste de lignes de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <returns>Une liste de ligne de rapport</returns>
        public IEnumerable<RapportLigneEnt> DuplicateListPointageReel(IEnumerable<RapportLigneEnt> listPointage, bool emptyValues = false)
        {
            var newListPointage = new List<RapportLigneEnt>();

            foreach (var pointage in listPointage)
            {
                if (!pointage.IsDeleted)
                {
                    newListPointage.Add(DuplicatePointageReel(pointage, emptyValues));
                }
            }
            return newListPointage;
        }

        /// <summary>
        ///   Duplique une ligne de rapport
        /// </summary>
        /// <param name="pointageReel">La ligne de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <param name="isUtilisateurConnectedFes">is Utilisateur Connected Fes</param>
        /// <returns>Une ligne de rapport</returns>
        private RapportLigneEnt DuplicatePointageReel(RapportLigneEnt pointageReel, bool emptyValues = false, bool isUtilisateurConnectedFes = false)
        {
            var duplicatedPointage = pointageReel.Duplicate();

            if (emptyValues)
            {
                duplicatedPointage.Rapport = null;
                duplicatedPointage.CiId = 0;
                duplicatedPointage.Ci = null;
                duplicatedPointage.HeureNormale = 0;
                duplicatedPointage.HeureMajoration = 0;
                duplicatedPointage.CodeMajorationId = null;
                duplicatedPointage.CodeMajoration = null;
                duplicatedPointage.CodeAbsenceId = null;
                duplicatedPointage.CodeAbsence = null;
                duplicatedPointage.HeureAbsence = 0;
                duplicatedPointage.NumSemaineIntemperieAbsence = null;
                duplicatedPointage.MaterielMarche = 0;
                duplicatedPointage.MaterielArret = 0;
                duplicatedPointage.MaterielPanne = 0;
                duplicatedPointage.MaterielIntemperie = 0;
                duplicatedPointage.IsDeleted = false;
            }

            duplicatedPointage.ListRapportLignePrimes = DuplicateListPointagePrimeReel(pointageReel.ListRapportLignePrimes, emptyValues).ToList();
            duplicatedPointage.ListRapportLigneTaches = DuplicateListPointageReelTache(pointageReel.ListRapportLigneTaches, emptyValues).ToList();
            if (isUtilisateurConnectedFes)
            {
                duplicatedPointage.ListRapportLigneMajorations = DuplicateListPointageMajorationReel(pointageReel.ListRapportLigneMajorations, emptyValues).ToList();
            }
            duplicatedPointage.HasAstreinte = pointageReel.HasAstreinte;
            duplicatedPointage.AstreinteId = pointageReel.AstreinteId;

            return duplicatedPointage;
        }

        /// <summary>
        ///   Duplique une liste de ligne de prime de rapport
        /// </summary>
        /// <param name="listPointagePrime">La liste de ligne de prime de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <returns>Une liste de ligne de prime de rapport</returns>
        private IEnumerable<RapportLignePrimeEnt> DuplicateListPointagePrimeReel(IEnumerable<RapportLignePrimeEnt> listPointagePrime, bool emptyValues = false)
        {
            var newListPointagePrime = new List<RapportLignePrimeEnt>();

            foreach (var pointagePrime in listPointagePrime)
            {
                if (!pointagePrime.IsDeleted)
                {
                    newListPointagePrime.Add(DuplicatePointagePrimeReel(pointagePrime, emptyValues));
                }
            }

            return newListPointagePrime;
        }

        /// <summary>
        ///   Duplique une liste de ligne du majoration de rapport
        /// </summary>
        /// <param name="listPointageMajoration">La liste de ligne de prime de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <returns>Une liste de ligne de prime de rapport</returns>
        private IEnumerable<RapportLigneMajorationEnt> DuplicateListPointageMajorationReel(IEnumerable<RapportLigneMajorationEnt> listPointageMajoration, bool emptyValues = false)
        {
            var newListPointageMajoration = new List<RapportLigneMajorationEnt>();

            foreach (var pointageMajoration in listPointageMajoration)
            {
                if (!pointageMajoration.IsDeleted)
                {
                    newListPointageMajoration.Add(DuplicatePointageMajorationReel(pointageMajoration, emptyValues));
                }
            }

            return newListPointageMajoration;
        }

        /// <summary>
        ///   Duplique une ligne de prime de rapport
        /// </summary>
        /// <param name="pointagePrime">La ligne de prime de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <returns>Une ligne de prime de rapport</returns>
        private RapportLignePrimeEnt DuplicatePointagePrimeReel(RapportLignePrimeEnt pointagePrime, bool emptyValues = false)
        {
            var duplicatedPointagePrime = pointagePrime.Duplicate();

            if (emptyValues)
            {
                duplicatedPointagePrime.IsChecked = false;
                duplicatedPointagePrime.IsDeleted = false;
            }

            return duplicatedPointagePrime;
        }

        /// <summary>
        ///   Duplique une ligne de majoration de rapport
        /// </summary>
        /// <param name="pointageMajoration">La ligne de prime de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <returns>Une ligne de prime de rapport</returns>
        private RapportLigneMajorationEnt DuplicatePointageMajorationReel(RapportLigneMajorationEnt pointageMajoration, bool emptyValues = false)
        {
            var duplicatedPointageMajoration = pointageMajoration.Duplicate();

            if (emptyValues)
            {
                duplicatedPointageMajoration.IsDeleted = false;
                duplicatedPointageMajoration.HeureMajoration = 0;
            }

            return duplicatedPointageMajoration;
        }

        /// <summary>
        ///   Duplique une liste de ligne de tache de rapport
        /// </summary>
        /// <param name="listePointageReelTache">La liste de ligne de tache de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <returns>Une liste de ligne de tache de rapport</returns>
        private IEnumerable<RapportLigneTacheEnt> DuplicateListPointageReelTache(IEnumerable<RapportLigneTacheEnt> listePointageReelTache, bool emptyValues = false)
        {
            var newListPointageReelTache = new List<RapportLigneTacheEnt>();

            foreach (var pointageReelTache in listePointageReelTache)
            {
                if (!pointageReelTache.IsDeleted)
                {
                    newListPointageReelTache.Add(DuplicatePointageReelTache(pointageReelTache, emptyValues));
                }
            }

            return newListPointageReelTache;
        }

        /// <summary>
        ///   Duplique une ligne de tache de rapport
        /// </summary>
        /// <param name="pointageReelTache">Une ligne de tache de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <returns>Une ligne de tache de rapport</returns>
        private RapportLigneTacheEnt DuplicatePointageReelTache(RapportLigneTacheEnt pointageReelTache, bool emptyValues = false)
        {
            var duplicatedPointageTache = pointageReelTache.Duplicate();

            if (emptyValues)
            {
                duplicatedPointageTache.HeureTache = 0;
                duplicatedPointageTache.IsDeleted = false;
            }
            return duplicatedPointageTache;
        }

        /// <summary>
        /// Has Partial Duplication On Different Zone De Travail
        /// </summary>
        /// <param name="allDays"></param>
        /// <param name="contratInterimairesForCiAndPeriode"></param>
        /// <param name="pointage"></param>
        /// <param name="interimaireDuplicationState"></param>
        /// <returns></returns>
        private bool HasPartialDuplicationOnDifferentZoneDeTravail(List<DateTime> allDays, List<ContratInterimaireEnt> contratInterimairesForCiAndPeriode, RapportLigneEnt pointage, InterimaireDuplicationState interimaireDuplicationState)
        {
            bool hasDifferentPeriod = interimaireDuplicationState == InterimaireDuplicationState.PartialDuplicationInDifferentZoneDeTravail
                                    && contratInterimairesForCiAndPeriode.Any(contrat => contrat.ZonesDeTravail.Any(z => z.EtablissementComptableId != pointage.Ci.EtablissementComptableId));
            hasDifferentPeriod = hasDifferentPeriod && contratInterimairesForCiAndPeriode.Any(contrat => contrat.InterimaireId == pointage.Personnel.PersonnelId && !allDays.Any(date => contrat.DateDebut <= date && date <= contrat.DateFin.AddDays(contrat.Souplesse)) && contrat.ZonesDeTravail.Any(z => z.EtablissementComptableId != pointage.Ci.EtablissementComptableId));
            return hasDifferentPeriod;
        }
    }
}
