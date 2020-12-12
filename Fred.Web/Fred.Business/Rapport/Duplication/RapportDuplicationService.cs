using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Rapport.Common.Duplication;
using Fred.Business.Rapport.Pointage.Duplication;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Rapport;
using Fred.Framework.Extensions;

namespace Fred.Business.Rapport.Duplication
{
    public class RapportDuplicationService : IRapportDuplicationService
    {
        private readonly IRapportRepository rapportRepository;
        private readonly IDatesClotureComptableManager datesClotureComptableManager;
        private readonly IPointageDuplicatorService pointageDuplicatorService;

        public RapportDuplicationService(
            IRapportRepository rapportRepository,
            IDatesClotureComptableManager datesClotureComptableManager,
            IPointageDuplicatorService pointageDuplicatorService)
        {
            this.rapportRepository = rapportRepository;
            this.datesClotureComptableManager = datesClotureComptableManager;
            this.pointageDuplicatorService = pointageDuplicatorService;
        }

        /// <summary>
        /// Recupere le rapport en base pour une duplication
        /// </summary>
        /// <param name="rapportId">rapportId</param>
        /// <returns>Le rapport correctement chargé</returns>
        public RapportEnt GetRapportForDuplication(int rapportId)
        {
            var includes = new List<Expression<Func<RapportEnt, object>>>();

            includes.Include(r => r.ListLignes.Select(x => x.Personnel.Societe))
                .Include(r => r.ListLignes.Select(x => x.CodeAbsence))
                    .Include(r => r.ListLignes.Select(x => x.ListRapportLigneTaches))
                    .Include(r => r.ListLignes.Select(x => x.ListRapportLigneAstreintes))
                    .Include(r => r.ListLignes.Select(x => x.ListRapportLigneMajorations))
                    .Include(r => r.ListLignes.Select(x => x.ListRapportLignePrimes))
                    .Include(r => r.ListLignes.Select(x => x.Ci));

            RapportEnt rapportToDuplicate = rapportRepository.Get(rapportId, includes);

            return rapportToDuplicate;
        }

        /// <summary>
        ///   Duplique un rapport sur une periode
        /// </summary>
        /// <param name="rapport">Le rapport à dupliquer</param>
        /// <param name="startDate">date de depart de la duplication</param>
        /// <param name="endDate">date de fin de la duplication</param>
        /// <returns>DuplicateRapportResult</returns>
        public DuplicateRapportResult DuplicateRapport(RapportEnt rapport, DateTime startDate, DateTime endDate)
        {

            var result = new DuplicateRapportResult();

            var dateClotures = this.datesClotureComptableManager.GetListDatesClotureComptableByCiGreaterThanPeriode(rapport.CiId, startDate.Month, startDate.Year);

            var allDayDuplicationRequested = DuplicationTimeHelper.GetAllDaysInPeriode(startDate, endDate);

            var allDaysToDuplicate = DuplicationTimeHelper.FiltersWeekEnd(allDayDuplicationRequested);

            result.DuplicationOnlyOnWeekend = DuplicationTimeHelper.GetIfPeriodIsOnlyOnWeekendOrWeekEnd(allDayDuplicationRequested);

            if (result.DuplicationOnlyOnWeekend)
            {
                return result;
            }

            result.HasDatesInClosedMonth = datesClotureComptableManager.HasDatesInClosedMonth(rapport.CiId, dateClotures, allDaysToDuplicate);

            if (result.HasDatesInClosedMonth)
            {
                return result;
            }

            var pointages = rapport.ListLignes.Where(p => p.DateSuppression == null).ToList();

            var pointagesDuplicatedOnPeriode = pointageDuplicatorService.DuplicatePointages(pointages, rapport.CiId, startDate, endDate);

            if (HasPartialDuplicationInDifferentZoneDeTravail(pointagesDuplicatedOnPeriode))
            {
                result.HasPartialDuplicationInDifferentZoneDeTravail = true;
            }
            else if (HasAllDuplicationInDifferentZoneDeTravail(pointagesDuplicatedOnPeriode))
            {
                result.HasAllDuplicationInDifferentZoneDeTravail = true;
                return result;
            }

            if (HasInterimaireWithoutContrat(pointagesDuplicatedOnPeriode))
            {
                result.HasInterimaireWithoutContrat = true;
                return result;
            }

            if (HasPersonnelInactifInPeriod(pointagesDuplicatedOnPeriode))
            {
                result.HasPersonnelsInactivesOnPeriode = true;
                return result;
            }

            foreach (var currentDay in allDaysToDuplicate)
            {
                var pointagesOnDay = pointagesDuplicatedOnPeriode.SelectMany(pdr => pdr.DuplicatedRapportLignes).Where(r => r.DatePointage == currentDay).ToList();

                if (pointagesOnDay.Any())
                {
                    var rapportDuplicated = CreateCopyRapport(rapport);

                    rapportDuplicated.ListLignes = pointagesOnDay;

                    rapportDuplicated.DateChantier = currentDay;

                    result.Rapports.Add(rapportDuplicated);
                }
            }

            return result;
        }

        private bool HasInterimaireWithoutContrat(List<DuplicatePointageResult> pointagesDuplicatedOnPeriode)
        {
            return pointagesDuplicatedOnPeriode.Any(pdr => pdr.InterimaireDuplicationState == InterimaireDuplicationState.NothingDayDuplicate || (pdr.InterimaireDuplicationState == InterimaireDuplicationState.PartialDuplicate && !pdr.HasPartialDuplicationInDifferentZoneDeTravail));
        }

        private bool HasAllDuplicationInDifferentZoneDeTravail(List<DuplicatePointageResult> pointagesDuplicatedOnPeriode)
        {
            return pointagesDuplicatedOnPeriode.Any(pdr => pdr.InterimaireDuplicationState == InterimaireDuplicationState.AllDuplicationInDifferentZoneDeTravail);
        }

        private bool HasPartialDuplicationInDifferentZoneDeTravail(List<DuplicatePointageResult> pointagesDuplicatedOnPeriode)
        {
            return pointagesDuplicatedOnPeriode.Any(pdr => pdr.HasPartialDuplicationInDifferentZoneDeTravail);
        }

        private bool HasPersonnelInactifInPeriod(List<DuplicatePointageResult> pointagesDuplicatedOnPeriode)
        {
            return pointagesDuplicatedOnPeriode.Any(pdr => pdr.PersonnelIsInactiveInPeriode);
        }
        private RapportEnt CreateCopyRapport(RapportEnt rapport)
        {
            var pointages = rapport.ListLignes.Where(rl => rl.DateSuppression == null).ToList();
            return new RapportEnt
            {
                RapportId = 0,
                RapportStatutId = 0,
                RapportStatut = null,
                DateChantier = DateTime.UtcNow.Date,
                Meteo = rapport.Meteo,
                Evenements = rapport.Evenements,
                AuteurCreationId = null,
                AuteurCreation = null,
                AuteurModificationId = null,
                AuteurModification = null,
                AuteurSuppressionId = null,
                AuteurSuppression = null,
                HoraireDebutM = rapport.HoraireDebutM,
                HoraireDebutS = rapport.HoraireDebutS,
                HoraireFinM = rapport.HoraireFinM,
                HoraireFinS = rapport.HoraireFinS,
                DateCreation = null,
                DateModification = null,
                DateSuppression = null,
                CiId = rapport.CiId,
                CI = rapport.CI,
                ListLignes = this.pointageDuplicatorService.DuplicateListPointageReel(pointages, true).ToList(),
                ListCommentaires = DuplicateListCommentaires(rapport.ListCommentaires),
                NbMaxPrimes = rapport.NbMaxPrimes,
                Cloture = false,
                NbMaxTaches = rapport.NbMaxTaches
            };
        }

        /// <summary>
        ///   Duplique un rapport
        /// </summary>
        /// <param name="rapport">Le rapport à dupliquer</param>
        /// <returns>Un rapport</returns>
        public RapportEnt DuplicateRapport(RapportEnt rapport)
        {
            return new RapportEnt
            {
                RapportId = 0,
                RapportStatutId = 0,
                RapportStatut = null,
                DateChantier = DateTime.UtcNow.Date,
                Meteo = rapport.Meteo,
                Evenements = rapport.Evenements,
                AuteurCreationId = null,
                AuteurCreation = null,
                AuteurModificationId = null,
                AuteurModification = null,
                AuteurSuppressionId = null,
                AuteurSuppression = null,
                HoraireDebutM = rapport.HoraireDebutM,
                HoraireDebutS = rapport.HoraireDebutS,
                HoraireFinM = rapport.HoraireFinM,
                HoraireFinS = rapport.HoraireFinS,
                DateCreation = null,
                DateModification = null,
                DateSuppression = null,
                CiId = rapport.CiId,
                CI = rapport.CI,
                ListLignes = this.pointageDuplicatorService.DuplicateListPointageReel(rapport.ListLignes, true).ToList(),
                ListCommentaires = DuplicateListCommentaires(rapport.ListCommentaires),
                NbMaxPrimes = rapport.NbMaxPrimes,
                Cloture = false,
                NbMaxTaches = rapport.NbMaxTaches
            };
        }

        private ICollection<RapportTacheEnt> DuplicateListCommentaires(ICollection<RapportTacheEnt> listCommentairesToDuplicate)
        {
            var listCommentaires = new List<RapportTacheEnt>();
            foreach (var commentaireToDuplicate in listCommentairesToDuplicate)
            {
                listCommentaires.Add(DuplicateCommentaires(commentaireToDuplicate));
            }
            return listCommentaires;
        }

        private RapportTacheEnt DuplicateCommentaires(RapportTacheEnt commentaireToDuplicate)
        {
            return new RapportTacheEnt
            {
                Rapport = null,
                RapportId = 0,
                Tache = commentaireToDuplicate.Tache,
                TacheId = commentaireToDuplicate.TacheId
            };
        }


    }
}
