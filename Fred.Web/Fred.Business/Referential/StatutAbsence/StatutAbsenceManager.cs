using System.Collections.Generic;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;

namespace Fred.Business.Referential.StatutAbsence
{
    /// <summary>
    /// Classe manager des statuts absences
    /// </summary>
    public class StatutAbsenceManager : Manager<StatutAbsenceEnt, IStatutAbsenceRepository>, IStatutAbsenceManager
    {
        public StatutAbsenceManager(IUnitOfWork uow, IStatutAbsenceRepository statutAbsenceRepository)
            : base(uow, statutAbsenceRepository)
        {
        }

        /// <summary>
        ///   La liste de tous les statuts d'absence.
        /// </summary>
        /// <returns>Renvoie la liste de des statuts d'absence active</returns>
        public IEnumerable<StatutAbsenceEnt> GetStatutAbsList()
        {
            return Repository.GetStatutAbsList();
        }
    }
}
