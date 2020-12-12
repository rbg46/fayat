using System.Collections.Generic;
using Fred.Business.RepriseDonnees.IndemniteDeplacement.Models;
using Fred.Entities.IndemniteDeplacement;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.IndemniteDeplacement.Mapper
{
    /// <summary>
    /// Transform les données du fichier excel en Indemnité Personnel
    /// </summary>
    public interface IIndemniteDeplacementDataMapper : IService
    {
        /// <summary>
        /// Creer des IndeMnité Deplacements à partir d'une liste de RepriseExcelIndemniteDeplacement
        /// </summary>
        /// <param name="context">Contexte contenant les data nécessaires à l'import</param>
        /// <param name="listRepriseExcelIndemniteDeplacement">Indemnite Deplacement sous la forme Excel</param>
        /// <returns>Liste d'Indemnite Déplacement</returns>
        List<IndemniteDeplacementEnt> Transform(ContextForImportIndemniteDeplacement context, List<RepriseExcelIndemniteDeplacement> listRepriseExcelIndemniteDeplacement);
    }
}
