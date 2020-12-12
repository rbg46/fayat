using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.OperationDiverse;
using Fred.EntityFramework;
using Fred.Web.Shared.Models.OperationDiverse;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.OperationDiverse
{
    /// <summary>
    ///  Repository des FamilleOperationDiverseEnt
    /// </summary>
    public class FamilleOperationDiverseRepository : FredRepository<FamilleOperationDiverseEnt>, IFamilleOperationDiverseRepository
    {
        public FamilleOperationDiverseRepository(FredDbContext context)
          : base(context)
        { }

        /// <summary>
        /// Récupère la liste des familles d'OD pour une liste d'indentifiant de famille
        /// </summary>
        /// <param name="familleIds">Liste d'identifiant des familles d'OD</param>
        /// <returns>Liste de <see cref="FamilleOperationDiverseEnt" /> </returns>
        public IReadOnlyList<FamilleOperationDiverseEnt> GetByIds(List<int> familleIds)
        {
            return Context.FamilleOperationDiverse.Where(famille => familleIds.Contains(famille.FamilleOperationDiverseId)).ToList();
        }

        /// <summary>
        /// Récupère la liste des familles d'OD pour une société
        /// </summary>
        /// <param name="societeIds">Identifiant de la société à laquelle les familles sont rattachées</param>
        /// <returns>Liste des familles d'OD de la société</returns>
        public IEnumerable<FamilleOperationDiverseEnt> GetFamilyBySociety(List<int> societeIds)
        {
            return Context.FamilleOperationDiverse.Where(f => societeIds.Contains(f.SocieteId));
        }

        /// <summary>
        /// Récupère la liste des familles d'OD pour une société
        /// </summary>
        /// <param name="societeId">Identifiant de la société à laquelle les familles sont rattachées</param>
        /// <returns>Liste des familles d'OD de la société</returns>
        public IEnumerable<FamilleOperationDiverseEnt> GetFamilyBySociety(int societeId)
        {
            return Context.FamilleOperationDiverse.Where(f => f.SocieteId == societeId);
        }

        /// <summary>
        /// Récupère la liste des parametrage d'une famille d'OD pour une société
        /// </summary>
        /// <param name="societeId">Identifiant de la société à laquelle les familles sont rattachées</param>
        /// <returns>Liste des parametrage d'une famille d'OD de la société</returns>
        public IReadOnlyList<ParametrageFamilleOperationDiverseModel> GetAllParametrageFamilleOperationDiverseNaturesJournaux(int societeId)
        {
            var result =
            ((from famille in Context.FamilleOperationDiverse.AsNoTracking()
              join journal in Context.Journals.AsNoTracking() on new { famille.FamilleOperationDiverseId, famille.SocieteId } equals new { FamilleOperationDiverseId = journal.ParentFamilyODWithoutOrder, journal.SocieteId }
              join nature in Context.Natures.AsNoTracking() on new { famille.FamilleOperationDiverseId, famille.SocieteId } equals new { FamilleOperationDiverseId = nature.ParentFamilyODWithoutOrder, nature.SocieteId }
              where famille.SocieteId == societeId
              select new ParametrageFamilleOperationDiverseModel
              {
                  FamilleOperationDiverse = famille,
                  Journal = journal,
                  Nature = nature
              }).Distinct()
            .Union((from famille in Context.FamilleOperationDiverse.AsNoTracking()
                    join journal in Context.Journals.AsNoTracking() on new { famille.FamilleOperationDiverseId, famille.SocieteId } equals new { FamilleOperationDiverseId = journal.ParentFamilyODWithOrder, journal.SocieteId }
                    join nature in Context.Natures.AsNoTracking() on new { famille.FamilleOperationDiverseId, famille.SocieteId } equals new { FamilleOperationDiverseId = nature.ParentFamilyODWithOrder, nature.SocieteId }
                    where famille.SocieteId == societeId
                    select new ParametrageFamilleOperationDiverseModel
                    {
                        FamilleOperationDiverse = famille,
                        Journal = journal,
                        Nature = nature
                    }).Distinct())).ToList();

            return result;
        }

        public int GetFamilyTaskId(int familleOperationDiverseId)
        {
            return Context.FamilleOperationDiverse
                .Where(f => f.FamilleOperationDiverseId == familleOperationDiverseId)
                .Select(s => s.TacheId)
                .FirstOrDefault();
        }
    }
}
