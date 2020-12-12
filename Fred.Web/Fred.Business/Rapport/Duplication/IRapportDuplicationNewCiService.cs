using System;
using Fred.Entities.Rapport;

namespace Fred.Business.Rapport.Duplication
{
    /// <summary>
    /// Service de duplication d'un rapport lors d'un changement de ci 
    /// </summary>
    public interface IRapportDuplicationNewCiService : IService
    {
        /// <summary>
        ///   Duplique un rapport
        /// </summary>
        /// <param name="rapport">Le rapport à dupliquer</param>
        /// <returns>Un rapport</returns>
        RapportEnt DuplicateRapport(RapportEnt rapport);
    }
}
