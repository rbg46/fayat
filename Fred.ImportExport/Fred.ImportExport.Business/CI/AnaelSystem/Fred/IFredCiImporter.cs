using System.Collections.Generic;
using Fred.Business;
using Fred.Entities.CI;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Societe;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Fred
{
    /// <summary>
    /// Service qui gere l'import des ci dans FRED
    /// </summary>
    public interface IFredCiImporter : IService
    {
        /// <summary>
        /// Importe les ci dans fred depuis des données Anael
        /// </summary>
        /// <param name="organisationTree">L'abre</param>
        /// <param name="societe">La societe</param>
        /// <param name="fredAnaelCIs">Les cis de type CIENT avec des données Anael</param>
        /// <returns>Liste des cis importés</returns>
        List<CIEnt> ImportCIsFromAnael(OrganisationTree organisationTree, SocieteEnt societe, List<CIEnt> fredAnaelCIs);
    }
}
