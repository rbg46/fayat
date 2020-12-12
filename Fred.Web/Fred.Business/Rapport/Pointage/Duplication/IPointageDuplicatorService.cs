using System;
using System.Collections.Generic;
using Fred.Entities.Rapport;

namespace Fred.Business.Rapport.Pointage.Duplication
{
    /// <summary>
    /// Service de duplication de pointage.
    /// </summary>
    public interface IPointageDuplicatorService : IService
    {
        /// <summary>
        /// Recupere le pointage pour faire une duplication
        /// </summary>
        /// <param name="rapportLigneId">le pointage id a dupliquer</param>
        /// <returns>Le pointage</returns>
        RapportLigneEnt GetPointageForDuplication(int rapportLigneId);


        /// <summary>
        /// Duplique un pointage
        /// </summary>
        /// <param name="pointageToDuplicate">Le pointage que l'on duplique</param>
        /// <param name="ciId">Le ciId avec lequel on fait la duplication</param>
        /// <param name="startDate">La date de debut de duplication</param>
        /// <param name="endDate">La date de FIN de duplication</param>
        /// <returns>Le resultat d'une duplication de pointage</returns>
        DuplicatePointageResult DuplicatePointage(RapportLigneEnt pointageToDuplicate, int ciId, DateTime startDate, DateTime endDate);


        /// <summary>
        ///   Duplique une liste de lignes de rapport
        /// </summary>
        /// <param name="listPointage">La liste de lignes de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <returns>Une liste de ligne de rapport</returns>
        IEnumerable<RapportLigneEnt> DuplicateListPointageReel(IEnumerable<RapportLigneEnt> listPointage, bool emptyValues = false);

        /// <summary>
        /// Duplique un pointage
        /// </summary>
        /// <param name="pointagesToDuplicate">Les pointages que l'on duplique</param>
        /// <param name="ciId">Le ciId avec lequel on fait la duplication</param>
        /// <param name="startDate">La date de debut de duplication</param>
        /// <param name="endDate">La date de FIN de duplication</param>
        /// <returns>Le resultat d'une duplication de pointage</returns>
        List<DuplicatePointageResult> DuplicatePointages(List<RapportLigneEnt> pointagesToDuplicate, int ciId, DateTime startDate, DateTime endDate);
    }
}
