using System;
using System.Threading.Tasks;
using Fred.Business.OperationDiverse;
using Fred.Business.Valorisation;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Depense;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Valorisation;

namespace Fred.Business.Depense
{
    public class GroupeRemplacementTacheManager : Manager<GroupeRemplacementTacheEnt, IGroupeRemplacementTacheRepository>, IGroupeRemplacementTacheManager
    {
        private readonly IOperationDiverseManager operationDiverseManager;
        private readonly IValorisationManager valorisationManager;
        private readonly IDepenseManager depenseManager;

        public GroupeRemplacementTacheManager(
            IUnitOfWork uow,
            IGroupeRemplacementTacheRepository groupeRemplacementTacheRepository,
            IOperationDiverseManager operationDiverseManager,
            IValorisationManager valorisationManager,
            IDepenseManager depenseManager)
          : base(uow, groupeRemplacementTacheRepository)
        {
            this.operationDiverseManager = operationDiverseManager;
            this.valorisationManager = valorisationManager;
            this.depenseManager = depenseManager;
        }

        public GroupeRemplacementTacheEnt AddGroupeRemplacementTache(GroupeRemplacementTacheEnt groupe)
        {
            Repository.AddGroupeRemplacementTache(groupe);
            Save();

            return groupe;
        }

        public async Task DeleteGroupeRemplacementTacheByIdAsync(int groupeId)
        {
            // On va vider au préalable le groupe qui est rattachés
            // soit à une dépense soit une od soir une valo
            ValorisationEnt valo = valorisationManager.GetByGroupRemplacementId(groupeId);
            OperationDiverseEnt od = await operationDiverseManager.GetByGroupRemplacementIdAsync(groupeId).ConfigureAwait(false);
            DepenseAchatEnt dep = depenseManager.GetByGroupRemplacementId(groupeId);

            if (valo != null)
            {
                valo.GroupeRemplacementTacheId = null;
                valorisationManager.UpdateValorisationGroupeRemplacement(valo);
            }
            else if (od != null)
            {
                od.GroupeRemplacementTacheId = null;
                await operationDiverseManager.UpdateAsync(od).ConfigureAwait(false);
            }
            else if (dep != null)
            {
                dep.GroupeRemplacementTacheId = null;
                depenseManager.UpdateDepense(dep);
            }

            Repository.DeleteGroupeRemplacementTacheById(groupeId);
            Save();
        }

        public GroupeRemplacementTacheEnt GetGroupeRemplacementTacheById(int groupeId)
        {
            return Repository.GetGroupeRemplacementTacheById(groupeId);
        }

        public async Task<RemplacementTacheEnt> GetRemplacementTacheOrigineAsync(int groupeId)
        {
            ValorisationEnt valo = valorisationManager.GetByGroupRemplacementId(groupeId);
            OperationDiverseEnt od = await operationDiverseManager.GetByGroupRemplacementIdAsync(groupeId).ConfigureAwait(false);
            DepenseAchatEnt dep = depenseManager.GetByGroupRemplacementId(groupeId);
            RemplacementTacheEnt res = new RemplacementTacheEnt();

            if (valo != null)
            {
                res.CiId = valo.CiId;
                res.Tache = valo.Tache;
                res.DateComptableRemplacement = valo.RapportLigne != null ? valo.RapportLigne.DatePointage : DateTime.UtcNow;
            }
            else if (od != null)
            {
                res.CiId = od.CiId;
                res.Tache = od.Tache;
                res.DateComptableRemplacement = od.DateComptable;
            }
            else if (dep != null)
            {
                res.CiId = dep.CiId.Value;
                res.Tache = dep.Tache;
                res.DateComptableRemplacement = dep.DateComptable;
            }
            else
            {
                return null;
            }

            return res;
        }
    }
}
