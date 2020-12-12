using System.Collections.Generic;
using Fred.Entities.Rapport;
using Fred.Web.Shared.Models.Moyen;

namespace Fred.Business.Moyen
{
    /// <summary>
    /// Fonctionnalité pour assurer l'interaction avec rapport de pointage : Rapport et RapportLigne
    /// </summary>
    public interface IRapportMoyenExtractionService : IService
    {
        /// <summary>
        /// maping RapportLigne To RapportMoyenLigne
        /// </summary>
        /// <param name="listAffectationMoyen">list Affectation Moyen</param>
        /// <returns>List RapportMoyenLigneExcelModel</returns>
        List<RapportMoyenLigneExcelModel> GetRapportMoyenLigneExcel(List<RapportLigneEnt> listAffectationMoyen);
    }
}
