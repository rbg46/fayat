using System.Collections.Generic;
using System.Linq;
using Fred.Business.RepriseDonnees.Rapport.ContextProviders;
using Fred.Entities.CI;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Rapport.Selector
{
    /// <summary>
    /// Service qui recuper le ci de la base en fonction des données contextuelle a l'import
    /// </summary>
    public class CiSelectorService
    {

        /// <summary>
        /// Recupere le CiEnt de fred, en fonction du codeci
        /// </summary>
        /// <param name="repriseExcelRapport"> le ci type excel</param>     
        /// <param name="context">les données contextuelle a l'import</param>
        /// <returns>le ci de la base correspondant au code ci et </returns>
        public CIEnt GetCiOfDatabase(RepriseExcelRapport repriseExcelRapport, ContextForImportRapport context)
        {
            OrganisationBase societe = context.SocietesOfGroupe.FirstOrDefault(s => s.Code == repriseExcelRapport.CodeSocieteCi);

            if (societe == null)
            {
                return null;
            }

            List<OrganisationBase> cisOfSociete = context.OrganisationTree.GetAllCisOfSociete(societe.Id);

            OrganisationBase ciInFredWithCode = cisOfSociete.FirstOrDefault(x => x.Code == repriseExcelRapport.CodeCi && (x.IsCi() || x.IsSousCi()));

            if (ciInFredWithCode == null)
            {
                return null;
            }

            CIEnt ciInFred = context.CisUsedInExcel.FirstOrDefault(x => x.CiId == ciInFredWithCode.Id);

            return ciInFred;
        }
    }
}
