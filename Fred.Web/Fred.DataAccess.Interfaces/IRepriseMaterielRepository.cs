using System.Collections.Generic;
using Fred.Entities.Referential;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Repository des Reprises des données des Materiels
    /// </summary>
    public interface IRepriseMaterielRepository : IMultipleRepository
    {
        /// <summary>
        /// UnitOfWork
        /// </summary>
        IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// Creation des Materiels
        /// </summary>
        /// <param name="materielEnts">Le Matériel a créer</param>
        void CreateMateriel(List<MaterielEnt> materielEnts);
    }
}
