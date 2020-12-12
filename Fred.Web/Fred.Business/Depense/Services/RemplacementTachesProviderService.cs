
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Depense.Models;
using Fred.DataAccess.Interfaces;
using Fred.Entities.DatesClotureComptable;
using Fred.Entities.Depense;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Valorisation;
namespace Fred.Business.Depense
{
    public class RemplacementTachesProviderService : IRemplacementTachesProviderService
    {
        private readonly IRemplacementTacheRepository remplacementTacheRepository;
        private readonly IDatesClotureComptableManager datesClotureComptableManager;

        public RemplacementTachesProviderService(IRemplacementTacheRepository remplacementTacheRepository,
            IDatesClotureComptableManager datesClotureComptableManager)
        {
            this.remplacementTacheRepository = remplacementTacheRepository;
            this.datesClotureComptableManager = datesClotureComptableManager;
        }

        public async Task FillRemplacementTachesOnEntitiesAsync(List<DepenseAchatEnt> depenseAchats,
                                                                List<OperationDiverseEnt> operationDiverses,
                                                                List<ValorisationEnt> valorisations)
        {
            if (depenseAchats == null)
                throw new ArgumentNullException(nameof(depenseAchats));

            if (operationDiverses == null)
                throw new ArgumentNullException(nameof(operationDiverses));

            if (valorisations == null)
                throw new ArgumentNullException(nameof(valorisations));

            List<int> groupeRemplacementTacheIds = GetAllGroupeRemplacementTacheIds(depenseAchats, operationDiverses, valorisations);

            List<GroupeRemplacementTacheHistory> histories = await GetHistoryAsync(groupeRemplacementTacheIds, false).ConfigureAwait(false);

            FillRemplacementTachesWithHistories(depenseAchats, operationDiverses, valorisations, histories);
        }

        private static List<int> GetAllGroupeRemplacementTacheIds(List<DepenseAchatEnt> depenseAchats,
                                                                    List<OperationDiverseEnt> operationDiverses,
                                                                    List<ValorisationEnt> valorisations)
        {
            List<int> depAchatsGroupeRemplacementTacheIds = depenseAchats.Where(x => x.GroupeRemplacementTacheId.HasValue).Select(x => x.GroupeRemplacementTacheId.Value).ToList();
            List<int> odsGroupeRemplacementTacheIds = operationDiverses.Where(x => x.GroupeRemplacementTacheId.HasValue).Select(x => x.GroupeRemplacementTacheId.Value).ToList();
            List<int> valosGroupeRemplacementTacheIds = valorisations.Where(x => x.GroupeRemplacementTacheId.HasValue).Select(x => x.GroupeRemplacementTacheId.Value).ToList();
            List<int> groupeRemplacementTacheIds = depAchatsGroupeRemplacementTacheIds.Concat(odsGroupeRemplacementTacheIds).Concat(valosGroupeRemplacementTacheIds).ToList();
            return groupeRemplacementTacheIds;
        }

        public async Task<List<GroupeRemplacementTacheHistory>> GetHistoryAsync(List<int> groupRemplacementIds, bool avecTacheOrigine = true)
        {
            if (groupRemplacementIds == null)
                throw new ArgumentNullException(nameof(groupRemplacementIds));

            List<GroupeRemplacementTacheHistory> result = new List<GroupeRemplacementTacheHistory>();

            List<RemplacementTacheOriginInformationModel> firstTasks = await this.remplacementTacheRepository.GetRemplacementTacheOrigineAsync(groupRemplacementIds).ConfigureAwait(false);
            List<RemplacementTacheEnt> histories = this.remplacementTacheRepository.GetRemplacementTachesListByGroupeRemplacementTacheIds(groupRemplacementIds).ToList();

            Dictionary<int, List<DatesClotureComptableEnt>> datesClotureComptableEntsDictionnay = GetDateCloturesComptables(firstTasks, histories);

            foreach (int groupRemplacementId in groupRemplacementIds)
            {
                List<RemplacementTacheEnt> completeHistory = new List<RemplacementTacheEnt>();

                RemplacementTacheOriginInformationModel remplacementTacheOriginInformation = firstTasks.Single(x => x.RequestForGroupeRemplacementTacheId == groupRemplacementId);
                RemplacementTacheEnt firstTask = remplacementTacheOriginInformation.ToRemplacementTacheEnt();
                List<RemplacementTacheEnt> history = histories.Where(x => x.GroupeRemplacementTacheId == groupRemplacementId).ToList();

                completeHistory.Add(firstTask);

                FillIsPeriodeClotureeForAllHistories(datesClotureComptableEntsDictionnay, firstTask, history);

                if (avecTacheOrigine)
                {
                    List<RemplacementTacheEnt> completeHistoryConcat = completeHistory.Concat(history).ToList();
                    result.Add(new GroupeRemplacementTacheHistory(groupRemplacementId, completeHistoryConcat));
                }
                else
                {
                    result.Add(new GroupeRemplacementTacheHistory(groupRemplacementId, history));
                }
            }

            return result;
        }

        private void FillIsPeriodeClotureeForAllHistories(Dictionary<int, List<DatesClotureComptableEnt>> datesClotureComptableEntsDictionnay, RemplacementTacheEnt firstTask, List<RemplacementTacheEnt> history)
        {
            List<DatesClotureComptableEnt> datesComptablesForCi = datesClotureComptableEntsDictionnay[firstTask.CiId];

            foreach (RemplacementTacheEnt remplacementTache in history)
            {
                int yearOfHistory = remplacementTache.DateComptableRemplacement.Value.Year;
                int monthOfHistory = remplacementTache.DateComptableRemplacement.Value.Month;
                remplacementTache.IsPeriodeCloturee = datesClotureComptableManager.IsPeriodClosed(datesComptablesForCi, firstTask.CiId, yearOfHistory, monthOfHistory);
            }
        }

        private Dictionary<int, List<DatesClotureComptableEnt>> GetDateCloturesComptables(List<RemplacementTacheOriginInformationModel> firstTasks, List<RemplacementTacheEnt> histories)
        {
            Dictionary<int, List<DatesClotureComptableEnt>> datesClotureComptableEntsDictionnay = new Dictionary<int, List<DatesClotureComptableEnt>>();

            List<DateTime> allDatesOnTacheOrigines = histories.Select(x => x.DateComptableRemplacement.Value).ToList();
            List<DateTime> allDatesInHistories = firstTasks.Select(x => x.DateComptableRemplacement.Value).ToList();

            DateTime oldestDate = GetOldestDate(allDatesOnTacheOrigines, allDatesInHistories);

            List<int> allCiIds = firstTasks.Select(x => x.CiId).Distinct().ToList();

            foreach (int ciId in allCiIds)
            {
                IEnumerable<DatesClotureComptableEnt> datesClotureComptableEnts = datesClotureComptableManager.GetListDatesClotureComptableByCiGreaterThanPeriode(ciId, oldestDate.Month, oldestDate.Year);
                datesClotureComptableEntsDictionnay.Add(ciId, datesClotureComptableEnts.ToList());
            }

            return datesClotureComptableEntsDictionnay;
        }

        private DateTime GetOldestDate(List<DateTime> allDates, List<DateTime> allDates2)
        {
            List<DateTime> allDatesOrdered = allDates.OrderBy(x => x).Distinct().ToList();
            DateTime oldestDate = allDatesOrdered.FirstOrDefault();

            List<DateTime> allDatesOrdered2 = allDates2.OrderBy(x => x).Distinct().ToList();
            DateTime oldestDate2 = allDatesOrdered2.FirstOrDefault();

            DateTime finalOldestDate = oldestDate <= oldestDate2 ? oldestDate : oldestDate2;
            return finalOldestDate;
        }

        private void FillRemplacementTachesWithHistories(List<DepenseAchatEnt> depenseAchats,
                                                         List<OperationDiverseEnt> operationDiverses,
                                                         List<ValorisationEnt> valorisations,
                                                         List<GroupeRemplacementTacheHistory> histories)
        {
            foreach (DepenseAchatEnt depAchat in depenseAchats)
            {
                depAchat.RemplacementTaches = GetHistoriesForGroupeRemplacementTacheId(histories, depAchat.GroupeRemplacementTacheId);
            }

            foreach (OperationDiverseEnt od in operationDiverses)
            {
                od.RemplacementTaches = GetHistoriesForGroupeRemplacementTacheId(histories, od.GroupeRemplacementTacheId);
            }

            foreach (ValorisationEnt valo in valorisations)
            {
                valo.RemplacementTaches = GetHistoriesForGroupeRemplacementTacheId(histories, valo.GroupeRemplacementTacheId);
            }
        }

        private List<RemplacementTacheEnt> GetHistoriesForGroupeRemplacementTacheId(List<GroupeRemplacementTacheHistory> histories, int? groupeRemplacementTacheId)
        {
            if (!groupeRemplacementTacheId.HasValue)
            {
                return null;
            }
            return histories.Where(x => x.GroupeRemplacementTacheId == groupeRemplacementTacheId.Value).First().HistoryOfGroupeRemplacementTache;
        }

    }
}
