using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.OperationDiverse;
using Fred.Web.Shared.Models.OperationDiverse;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Repository des FamilleOperationDiverseEnt
    /// </summary>
    public interface IFamilleOperationDiverseRepository : IRepository<FamilleOperationDiverseEnt>
    {
        /// <summary>
        /// Récupère la liste des familles d'OD pour une société
        /// </summary>
        /// <param name="societeId">Identifiant de la société à laquelle les familles sont rattachées</param>
        /// <returns>Liste des familles d'OD de la société</returns>
        IEnumerable<FamilleOperationDiverseEnt> GetFamilyBySociety(int societeId);

        /// <summary>
        /// Récupère la liste des familles d'OD pour une société
        /// </summary>
        /// <param name="societeIds">Identifiant de la société à laquelle les familles sont rattachées</param>
        /// <returns>Liste des familles d'OD de la société</returns>
        IEnumerable<FamilleOperationDiverseEnt> GetFamilyBySociety(List<int> societeIds);

        /// <summary>
        /// Récupère la liste des familles d'OD pour une liste d'indentifiant de famille
        /// </summary>
        /// <param name="familleIds">iste d'identifiant des familles d'OD</param>
        /// <returns>Liste de <see cref="FamilleOperationDiverseEnt"/> </returns>
        IReadOnlyList<FamilleOperationDiverseEnt> GetByIds(List<int> familleIds);

        /// <summary>
        /// Récupère la liste des parametrage d'une famille d'OD pour une société
        /// </summary>
        /// <param name="societeId">Identifiant de la société à laquelle les familles sont rattachées</param>
        /// <returns>Liste des parametrage d'une famille d'OD de la société</returns>
        IReadOnlyList<ParametrageFamilleOperationDiverseModel> GetAllParametrageFamilleOperationDiverseNaturesJournaux(int societeId);

        int GetFamilyTaskId(int familleOperationDiverseId);
    }
}
