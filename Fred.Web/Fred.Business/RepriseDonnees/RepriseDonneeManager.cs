using System.Collections.Generic;
using Fred.DataAccess.Interfaces;
using Fred.Entities.CI;
using Fred.Entities.Groupe;

namespace Fred.Business.RepriseDonnees
{
    public class RepriseDonneeManager : IRepriseDonneeManager
    {
        private readonly IRepriseDonneesRepository repriseDonneesRepository;

        public RepriseDonneeManager(IRepriseDonneesRepository repriseDonneesRepository)
        {
            this.repriseDonneesRepository = repriseDonneesRepository;
        }

        /// <summary>
        /// Retourne tous les groupes
        /// </summary>
        /// <returns>Liste de groupes</returns>
        public List<GroupeEnt> GetAllGroupes()
        {
            return repriseDonneesRepository.GetAllGroupes();
        }

        /// <summary>
        /// Retourne les ci dont le code est contenu dans la liste ciCodes
        /// </summary>
        /// <param name="ciCodes">liste de code ci</param>
        /// <returns>Les ci qui ont le code contenu dans ciCodes </returns>
        public List<CIEnt> GetCisByCodes(List<string> ciCodes)
        {
            return repriseDonneesRepository.GetCisByCodes(ciCodes);
        }
    }
}
