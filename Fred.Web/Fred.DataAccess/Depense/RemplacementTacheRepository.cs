using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Depense;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Depense
{
    /// <summary>
    /// Référentiel de données pour les dépenses.
    /// </summary>
    public class RemplacementTacheRepository : FredRepository<RemplacementTacheEnt>, IRemplacementTacheRepository
    {
        public RemplacementTacheRepository(FredDbContext context)
          : base(context)
        {
        }

        public RemplacementTacheEnt AddRemplacementTache(RemplacementTacheEnt remplacementT)
        {
            if (Context.Entry(remplacementT).State == EntityState.Detached)
            {
                CleanDependencies(remplacementT);
            }

            Insert(remplacementT);

            return remplacementT;
        }

        public int DeleteRemplacementTacheById(int remplacementTacheId)
        {
            RemplacementTacheEnt remplacementT = Context.RemplacementTache.Find(remplacementTacheId);

            int groupId = remplacementT.GroupeRemplacementTacheId;

            Delete(remplacementT);

            return groupId;
        }

        public RemplacementTacheEnt GetLast(int groupeRemplacementTacheId, DateTime? periodeFin)
        {
            return Context.RemplacementTache
                .Include(x => x.Tache.Parent.Parent)
                .Where(x => x.GroupeRemplacementTacheId == groupeRemplacementTacheId
                && (!periodeFin.HasValue || ((x.DateComptableRemplacement.Value.Year * 100) + x.DateComptableRemplacement.Value.Month <= (periodeFin.Value.Year * 100) + periodeFin.Value.Month)))
                .OrderByDescending(d => d.RangRemplacement)
                .FirstOrDefault();
        }

        public async Task<IReadOnlyList<RemplacementTacheEnt>> GetLastAsync(IEnumerable<int> groupeRemplacementTacheIds, DateTime? periodeFin)
        {
            return await Context.RemplacementTache
                .Include(x => x.Tache.Parent.Parent)
                .Where(x => groupeRemplacementTacheIds.Contains(x.GroupeRemplacementTacheId)
                              && (!periodeFin.HasValue || ((x.DateComptableRemplacement.Value.Year * 100) + x.DateComptableRemplacement.Value.Month <= (periodeFin.Value.Year * 100) + periodeFin.Value.Month)))
                 .OrderByDescending(d => d.RangRemplacement).ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Retourne la tache portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="remplacementTacheId">Identifiant de la tache à retrouver.</param>
        /// <returns>La dépense retrouvée, sinon nulle.</returns>
        public RemplacementTacheEnt GetRemplacementTacheById(int remplacementTacheId)
        {
            return Context.RemplacementTache.Include(t => t.Tache)
                          .AsNoTracking()
                          .SingleOrDefault(r => r.RemplacementTacheId.Equals(remplacementTacheId));
        }

        public IEnumerable<RemplacementTacheEnt> GetRemplacementTacheList()
        {
            foreach (RemplacementTacheEnt remplacementT in Context.RemplacementTache
                                                  .Include(t => t.Tache)
                                                  .AsNoTracking())
            {
                yield return remplacementT;
            }
        }

        public IEnumerable<RemplacementTacheEnt> GetRemplacementTachesListByGroupId(int groupId)
        {
            return Query()
                    .Include(c => c.AuteurCreation.Personnel)
                    .Include(c => c.Tache.Parent.Parent)
                    .Filter(c => !c.DateSuppression.HasValue && c.GroupeRemplacementTacheId == groupId)
                    .OrderBy(d => d.OrderBy(x => x.RangRemplacement))
                    .Get()
                    .AsNoTracking();
        }

        public RemplacementTacheEnt UpdateRemplacementTache(RemplacementTacheEnt remplacementT)
        {
            if (Context.Entry(remplacementT).State == EntityState.Detached)
            {
                CleanDependencies(remplacementT);
            }

            Update(remplacementT);

            return remplacementT;
        }

        /// <summary>
        /// Permet de détacher les entités dépendantes des dépenses pour éviter de les prendre en compte dans la sauvegarde du
        /// contexte.
        /// </summary>
        /// <param name="remplacementT">dépense dont les dépendances sont à détachées</param>
        private void CleanDependencies(RemplacementTacheEnt remplacementT)
        {
            remplacementT.Tache = null;
            remplacementT.GroupeRemplacementTache = null;
        }

        public List<RemplacementTacheEnt> GetRemplacementTachesListByGroupeRemplacementTacheIds(List<int> groupRemplacementIds)
        {
            return Context.RemplacementTache
                    .Include(c => c.AuteurCreation.Personnel)
                    .Include(c => c.Tache.Parent.Parent)
                    .Where(c => !c.DateSuppression.HasValue && groupRemplacementIds.Contains(c.GroupeRemplacementTacheId))
                    .OrderBy(d => d.RangRemplacement)
                    .AsNoTracking()
                    .ToList();
        }

        public async Task<List<RemplacementTacheOriginInformationModel>> GetRemplacementTacheOrigineAsync(List<int> groupRemplacementIds)
        {
            List<RemplacementTacheOriginInformationModel> valorisationsInfos = await GetRemplacementTacheOrigineForValorisations(groupRemplacementIds).ConfigureAwait(false);
            List<RemplacementTacheOriginInformationModel> operationDiversesInfos = await GetRemplacementTacheOrigineForOperationDiverses(groupRemplacementIds).ConfigureAwait(false);
            List<RemplacementTacheOriginInformationModel> depensesInfos = await OperationDiversesForDepenseAchats(groupRemplacementIds).ConfigureAwait(false);
            var result = valorisationsInfos.Concat(operationDiversesInfos).Concat(depensesInfos).ToList();
            return result;
        }

        private async Task<List<RemplacementTacheOriginInformationModel>> GetRemplacementTacheOrigineForValorisations(List<int> groupRemplacementIds)
        {
            return await Context.Valorisations
              .Where(v => v.GroupeRemplacementTacheId.HasValue)
              .Where(v => groupRemplacementIds.Contains(v.GroupeRemplacementTacheId.Value))
              .Include(x => x.Tache)
              .Include(x => x.RapportLigne)
                .Select(x => new RemplacementTacheOriginInformationModel()
                {
                    CiId = x.CiId,
                    Tache = x.Tache,
                    DateComptableRemplacement = x.RapportLigne != null ? x.RapportLigne.DatePointage : DateTime.UtcNow,
                    RequestForGroupeRemplacementTacheId = x.GroupeRemplacementTacheId.Value
                })
               .ToListAsync()
               .ConfigureAwait(false);
        }

        private async Task<List<RemplacementTacheOriginInformationModel>> GetRemplacementTacheOrigineForOperationDiverses(List<int> groupRemplacementIds)
        {
            return await Context.OperationDiverses
                .Where(v => v.GroupeRemplacementTacheId.HasValue)
                .Where(v => groupRemplacementIds.Contains(v.GroupeRemplacementTacheId.Value))
                .Include(x => x.Tache)
                .Select(x => new RemplacementTacheOriginInformationModel()
                {
                    CiId = x.CiId,
                    Tache = x.Tache,
                    DateComptableRemplacement = x.DateComptable,
                    RequestForGroupeRemplacementTacheId = x.GroupeRemplacementTacheId.Value
                })
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        private async Task<List<RemplacementTacheOriginInformationModel>> OperationDiversesForDepenseAchats(List<int> groupRemplacementIds)
        {
            return await Context.DepenseAchats
                .Where(v => v.GroupeRemplacementTacheId.HasValue)
                .Where(v => groupRemplacementIds.Contains(v.GroupeRemplacementTacheId.Value))
                .Include(x => x.Tache)
                .Include(d => d.DepenseType)
                .Select(x => new RemplacementTacheOriginInformationModel()
                {
                    CiId = x.CiId.Value,
                    Tache = x.Tache,
                    DateComptableRemplacement = x.DateComptable,
                    RequestForGroupeRemplacementTacheId = x.GroupeRemplacementTacheId.Value
                })
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }
    }
}
