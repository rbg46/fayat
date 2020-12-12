using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fred.Business.DatesClotureComptable;
using Fred.Business.OperationDiverse;
using Fred.Business.Utilisateur;
using Fred.Business.Valorisation;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Depense;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Valorisation;
using Fred.Web.Models.Depense;
namespace Fred.Business.Depense
{
    public class RemplacementTacheManager : Manager<RemplacementTacheEnt, IRemplacementTacheRepository>, IRemplacementTacheManager
    {
        private readonly IMapper mapper;
        private readonly IDepenseManager depenseManager;
        private readonly IGroupeRemplacementTacheManager groupeRemplacementTacheManager;
        private readonly IOperationDiverseManager operationDiverseManager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IValorisationManager valorisationManager;
        private readonly IDatesClotureComptableManager datesClotureComptableManager;
        public RemplacementTacheManager(
            IUnitOfWork uow,
            IRemplacementTacheRepository remplacementTacheRepository,
            IMapper mapper,
            IDepenseManager depenseManager,
            IGroupeRemplacementTacheManager groupeRemplacementTacheManager,
            IOperationDiverseManager operationDiverseManager,
            IUtilisateurManager utilisateurManager,
            IValorisationManager valorisationManager,
            IDatesClotureComptableManager datesClotureComptableManager
           )
        : base(uow, remplacementTacheRepository)
        {
            this.mapper = mapper;
            this.depenseManager = depenseManager;
            this.groupeRemplacementTacheManager = groupeRemplacementTacheManager;
            this.operationDiverseManager = operationDiverseManager;
            this.utilisateurManager = utilisateurManager;
            this.valorisationManager = valorisationManager;
            this.datesClotureComptableManager = datesClotureComptableManager;
        }

        public async Task AddAsync(RemplacementTacheModel tache)
        {
            int utilisateurId = utilisateurManager.GetContextUtilisateurId();
            foreach (var d in tache.DepensesLiees)
            {
                RemplacementTacheEnt taskToSave = this.mapper.Map<RemplacementTacheEnt>(tache);
                taskToSave.DateRemplacement = DateTime.UtcNow;
                taskToSave.AuteurCreationId = utilisateurId;
                taskToSave.Annulable = true;
                taskToSave.RangRemplacement = 1;

                if (d.TypeDepense.Equals(Constantes.DepenseType.Valorisation))
                {
                    ManageValorisationUpdate(d.DepenseId, d.Periode, ref taskToSave);
                }
                else if (d.TypeDepense.Equals(Constantes.DepenseType.OD))
                {
                    await ManageODUpdateAsync(d.DepenseId, d.Periode, taskToSave).ConfigureAwait(false);
                }
                else
                {
                    ManageDepenseUpdate(d.DepenseId, d.Periode, ref taskToSave);
                }

                Repository.AddRemplacementTache(taskToSave);
                Save();
            }
        }

        private void ManageValorisationUpdate(int depId, DateTime periodeOrigine, ref RemplacementTacheEnt taskToSave)
        {
            ValorisationEnt completeValo = valorisationManager.GetValorisationById(depId);
            taskToSave.DateComptableRemplacement = GetDateComptableRemplacement(periodeOrigine, completeValo.CiId);

            if (completeValo.GroupeRemplacementTacheId != null)
            {
                SetRankAndGroupId(taskToSave, completeValo.GroupeRemplacementTacheId.Value);
            }
            else
            {
                int newGroupeRemplacementId = GetNewGroupeRemplacementTacheId();
                taskToSave.GroupeRemplacementTacheId = newGroupeRemplacementId;
                completeValo.GroupeRemplacementTacheId = newGroupeRemplacementId;

                valorisationManager.UpdateValorisationGroupeRemplacement(completeValo);
            }
        }

        private async Task ManageODUpdateAsync(int depId, DateTime periodeOrigine, RemplacementTacheEnt taskToSave)
        {
            OperationDiverseEnt completeOD = operationDiverseManager.GetById(depId);
            taskToSave.DateComptableRemplacement = GetDateComptableRemplacement(periodeOrigine, completeOD.CiId);

            if (completeOD.GroupeRemplacementTacheId != null)
            {
                SetRankAndGroupId(taskToSave, completeOD.GroupeRemplacementTacheId.Value);
            }
            else
            {
                int newGroupeRemplacementId = GetNewGroupeRemplacementTacheId();
                taskToSave.GroupeRemplacementTacheId = newGroupeRemplacementId;
                completeOD.GroupeRemplacementTacheId = newGroupeRemplacementId;

                await operationDiverseManager.UpdateAsync(completeOD).ConfigureAwait(false);
            }
        }

        private void ManageDepenseUpdate(int depId, DateTime periodeOrigine, ref RemplacementTacheEnt taskToSave)
        {
            DepenseAchatEnt completeDep = depenseManager.GetDepenseById(depId);
            taskToSave.DateComptableRemplacement = GetDateComptableRemplacement(periodeOrigine, completeDep.CiId.Value);

            if (completeDep.GroupeRemplacementTacheId != null)
            {
                SetRankAndGroupId(taskToSave, completeDep.GroupeRemplacementTacheId.Value);
            }
            else
            {
                int newGroupeRemplacementId = GetNewGroupeRemplacementTacheId();
                taskToSave.GroupeRemplacementTacheId = newGroupeRemplacementId;
                completeDep.GroupeRemplacementTacheId = newGroupeRemplacementId;

                depenseManager.UpdateDepense(completeDep);
            }
        }

        private void SetRankAndGroupId(RemplacementTacheEnt taskToSave, int groupId)
        {
            IEnumerable<RemplacementTacheEnt> listRemplacementT = GetListByGroupId(groupId);

            taskToSave.RangRemplacement = listRemplacementT.First().RangRemplacement + 1;
            taskToSave.GroupeRemplacementTacheId = groupId;
        }

        private int GetNewGroupeRemplacementTacheId()
        {
            GroupeRemplacementTacheEnt group = new GroupeRemplacementTacheEnt();
            group = groupeRemplacementTacheManager.AddGroupeRemplacementTache(group);
            return group.GroupeRemplacementTacheId;
        }

        private DateTime GetDateComptableRemplacement(DateTime periodeOrigine, int ciId)
        {
            bool isPeriodeCloturee = datesClotureComptableManager.IsPeriodClosed(ciId, periodeOrigine.Year, periodeOrigine.Month);

            if (!isPeriodeCloturee)
            {
                if (DateTime.Now.Month == periodeOrigine.Month && DateTime.Now.Year == periodeOrigine.Year)
                {
                    return DateTime.UtcNow.Date;
                }
                else if ((DateTime.Now.Year * 100) + DateTime.Now.Month > (periodeOrigine.Year * 100) + periodeOrigine.Month)
                {
                    return new DateTime(periodeOrigine.Year, periodeOrigine.Month, DateTime.DaysInMonth(periodeOrigine.Year, periodeOrigine.Month));
                }
            }
            else
            {
                periodeOrigine = periodeOrigine.AddMonths(1);
                bool isNextMonthClosed = datesClotureComptableManager.IsPeriodClosed(ciId, periodeOrigine.Year, periodeOrigine.Month);

                while (isNextMonthClosed)
                {
                    periodeOrigine = periodeOrigine.AddMonths(1);
                    isNextMonthClosed = datesClotureComptableManager.IsPeriodClosed(ciId, periodeOrigine.Year, periodeOrigine.Month);
                }

                return new DateTime(periodeOrigine.Year, periodeOrigine.Month, 1);
            }

            return DateTime.UtcNow.Date;
        }

        public async Task DeleteByIdAsync(int remplacementTacheId)
        {
            int groupId = Repository.DeleteRemplacementTacheById(remplacementTacheId);
            Save();

            bool groupHasElements = Repository.Query().Filter(x => x.GroupeRemplacementTacheId == groupId).Get().Any();

            if (!groupHasElements)
            {
                await groupeRemplacementTacheManager.DeleteGroupeRemplacementTacheByIdAsync(groupId).ConfigureAwait(false);
            }
        }

        public RemplacementTacheEnt GetById(int remplacementTacheId)
        {
            return Repository.GetRemplacementTacheById(remplacementTacheId);
        }

        public async Task<IEnumerable<RemplacementTacheEnt>> GetHistoryAsync(int groupeTacheId, bool avecTacheOrigine = true)
        {
            List<RemplacementTacheEnt> completeHistory = new List<RemplacementTacheEnt>();

            RemplacementTacheEnt firstTask = await groupeRemplacementTacheManager.GetRemplacementTacheOrigineAsync(groupeTacheId).ConfigureAwait(false);
            List<RemplacementTacheEnt> history = Repository.GetRemplacementTachesListByGroupId(groupeTacheId).ToList();

            if (firstTask != null)
            {
                completeHistory.Add(firstTask);
            }

            history.ForEach(x => x.IsPeriodeCloturee = datesClotureComptableManager.IsPeriodClosed(firstTask.CiId, x.DateComptableRemplacement.Value.Year, x.DateComptableRemplacement.Value.Month));

            if (avecTacheOrigine)
            {
                return completeHistory.Concat(history).ToList();
            }

            return history.ToList();
        }

        public IEnumerable<RemplacementTacheEnt> GetListByGroupId(int groupId)
        {
            return Repository.GetRemplacementTachesListByGroupId(groupId);
        }

        public RemplacementTacheEnt GetLast(int groupeRemplacementTacheId, DateTime? periodeFin = null)
        {
            return Repository.GetLast(groupeRemplacementTacheId, periodeFin);
        }

        public async Task<IReadOnlyList<RemplacementTacheEnt>> GetLastAsync(IEnumerable<int> groupeRemplacementTacheIds, DateTime? periodeFin = null)
        {
            return await Repository.GetLastAsync(groupeRemplacementTacheIds, periodeFin).ConfigureAwait(false);
        }

        public RemplacementTacheEnt Update(RemplacementTacheEnt rt)
        {
            Repository.UpdateRemplacementTache(rt);
            Save();

            return rt;
        }
    }
}
