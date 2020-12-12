
using System.Collections.Generic;
using Fred.Entities.Budget.Avancement;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Référentiel de données des workflows du avancement.
    /// </summary>
    public interface IAvancementWorkflowRepository : IRepository<AvancementWorkflowEnt>
    {
        /// <summary>
        /// Retourne la liste des workflows concernant un avancement
        /// </summary>
        /// <param name="avancementId">Identifiant du avancement</param>
        /// <returns>La liste des workflows d'un avancement</returns>
        IEnumerable<AvancementWorkflowEnt> GetList(int avancementId);
    }
}
