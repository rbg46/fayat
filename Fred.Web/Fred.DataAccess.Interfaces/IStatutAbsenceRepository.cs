using System.Collections.Generic;
using Fred.Entities.Referential;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Interface du repo Statut Abasence
    /// </summary>
    public interface IStatutAbsenceRepository : IRepository<StatutAbsenceEnt>
    {
        /// <summary>
        ///   La liste de tous les statuts d'absence.
        /// </summary>
        /// <returns>Renvoie la liste de des statuts d'absence active</returns>
        IEnumerable<StatutAbsenceEnt> GetStatutAbsList();
    }
}
