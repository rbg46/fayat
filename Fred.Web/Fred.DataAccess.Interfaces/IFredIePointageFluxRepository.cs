using System.Collections.Generic;
using Fred.Entities.Rapport;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    ///  Repo FredWeb pour la gestion des exports de pointages de fred ie
    /// </summary>
    public interface IFredIePointageFluxRepository : IMultipleRepository
    {
        /// <summary>
        /// Permet de récupérer toutes les lignes de pointage d'un rapport avec les informations de personnel.
        /// </summary>
        /// <param name="rapportId">rapportId</param>
        /// <returns>List de RapportLigneEnt</returns>
        List<RapportLigneEnt> GetAllPointagesForPersonnelSap(int rapportId);

        /// <summary>
        /// Récupération de liste des rapports en fonction d'une liste d'identifiants de rapport
        /// </summary>
        /// <param name="rapportIds">Liste d'identifiants de rapport</param>
        /// <returns>Liste de rapport</returns>
        List<RapportEnt> GetRapportList(IEnumerable<int> rapportIds);

        /// <summary>
        /// Recupere un rapport par Id
        /// </summary>
        /// <param name="rapportId">rapportId</param>
        /// <returns>Un rapport</returns>
        RapportEnt FindByRapportId(int rapportId);
    }
}
