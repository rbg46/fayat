using System.Collections.Generic;
using Fred.Entities.Rapport;

namespace Fred.Business.Rapport.Pointage.FredIe
{
    /// <summary>
    /// Service situé dans fredWeb qui gere les flux de pointages de fred Ie
    /// </summary>
    public interface IFredIePointageFluxService : IService
    {
        /// <summary>
        /// Permet de récupérer toutes les lignes de pointage d'un rapport
        /// associétés à un personnel .
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
