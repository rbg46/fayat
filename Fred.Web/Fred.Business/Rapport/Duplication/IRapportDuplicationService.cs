using System;
using Fred.Entities.Rapport;

namespace Fred.Business.Rapport.Duplication
{
    /// <summary>
    /// Service de duplication d'un rapport
    /// </summary>
    public interface IRapportDuplicationService : IService
    {

        /// <summary>
        ///   Duplique un rapport
        /// </summary>
        /// <param name="rapport">Le rapport à dupliquer</param>
        /// <returns>Un rapport</returns>
        RapportEnt DuplicateRapport(RapportEnt rapport);

        /// <summary>
        /// Recupere le rapport en base pour une duplication
        /// </summary>
        /// <param name="rapportId">rapportId</param>
        /// <returns>Le rapport correctement chargé</returns>
        RapportEnt GetRapportForDuplication(int rapportId);

        /// <summary>
        ///   Duplique un rapport sur une periode
        /// </summary>
        /// <param name="rapport">Le rapport à dupliquer</param>
        /// <param name="startDate">date de depart de la duplication</param>
        /// <param name="endDate">date de fin de la duplication</param>
        /// <returns>DuplicateRapportResult</returns>
        DuplicateRapportResult DuplicateRapport(RapportEnt rapport, DateTime startDate, DateTime endDate);
    }
}
