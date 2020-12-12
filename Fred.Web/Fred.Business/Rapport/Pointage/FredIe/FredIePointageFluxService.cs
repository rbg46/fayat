using System.Collections.Generic;
using Fred.DataAccess.Rapport.Pointage.FredIe;
using Fred.Entities.Rapport;

namespace Fred.Business.Rapport.Pointage.FredIe
{
    /// <summary>
    /// Service situé dans fredWeb qui gere les flux de fred Ie
    /// </summary>
    public class FredIePointageFluxService : IFredIePointageFluxService
    {
        private readonly FredIePointageFluxRepository fredIePointageFluxRepository;

        public FredIePointageFluxService(FredIePointageFluxRepository fredIePointageFluxRepository)
        {
            this.fredIePointageFluxRepository = fredIePointageFluxRepository;
        }

        /// <summary>
        /// Permet de récupérer toutes les lignes de pointage d'un rapport avec les informations de personnel.
        /// </summary>
        /// <param name="rapportId">rapportId</param>
        /// <returns>List de RapportLigneEnt</returns>
        public List<RapportLigneEnt> GetAllPointagesForPersonnelSap(int rapportId)
        {
            return fredIePointageFluxRepository.GetAllPointagesForPersonnelSap(rapportId);
        }

        /// <summary>
        /// Récupération de liste des rapports en fonction d'une liste d'identifiants de rapport
        /// </summary>
        /// <param name="rapportIds">Liste d'identifiants de rapport</param>
        /// <returns>Liste de rapport</returns>
        public List<RapportEnt> GetRapportList(IEnumerable<int> rapportIds)
        {
            return fredIePointageFluxRepository.GetRapportList(rapportIds);
        }

        /// <summary>
        /// Recupere un rapport par Id
        /// </summary>
        /// <param name="rapportId">rapportId</param>
        /// <returns>Un rapport</returns>
        public RapportEnt FindByRapportId(int rapportId)
        {
            return fredIePointageFluxRepository.FindByRapportId(rapportId);
        }
    }
}
