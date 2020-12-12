using System.Collections.Generic;
using Fred.Entities.Personnel;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Repository des Reprises des données des Personnels
    /// </summary>
    public interface IReprisePersonnelRepository : IMultipleRepository
    {
        /// <summary>
        /// UnitOfWork
        /// </summary>
        IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// Creation du Personnel
        /// </summary>
        /// <param name="personnelEnts">Le Personnel a créer</param>
        void CreatePersonnel(List<PersonnelEnt> personnelEnts);
    }
}
