using System.Collections.Generic;
using Fred.Entities.Referential;

namespace Fred.Business.Referential.StatutAbsence
{
    /// <summary>
    /// Interface de Statut Manager
    /// </summary>
    public interface IStatutAbsenceManager : IManager<StatutAbsenceEnt>
    {
        /// <summary>
        ///   La liste de tous les statuts d'absence.
        /// </summary>
        /// <returns>Renvoie la liste de des statuts d'absence active</returns>
        IEnumerable<StatutAbsenceEnt> GetStatutAbsList();
    }
}
