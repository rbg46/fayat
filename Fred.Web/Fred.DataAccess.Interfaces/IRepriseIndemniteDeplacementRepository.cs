using System.Collections.Generic;
using Fred.Entities.IndemniteDeplacement;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Repository des Reprises des données des Personnels
    /// </summary>
    public interface IRepriseIndemniteDeplacementRepository : IMultipleRepository
    {
        /// <summary>
        /// UnitOfWork
        /// </summary>
        IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// Creation de Indemnite Deplacement
        /// </summary>
        /// <param name="indemniteDeplacementEnts">Les Indemnite Deplacement a créer</param>
        void CreateIndemniteDeplacement(List<IndemniteDeplacementEnt> indemniteDeplacementEnts);
    }
}
