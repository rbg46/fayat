using System.Collections.Generic;
using System.Linq;
using Fred.Business.RepriseDonnees.Ci.Models;
using Fred.Entities.CI;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Ci.Selector
{
    /// <summary>
    /// Service qui recuper le ci de la base en fonction des données contextuelle a l'import
    /// </summary>
    public class CiSelectorService : ICiSelectorService
    {

        /// <summary>
        /// Recupere le CiEnt de fred, en fonction du codeci
        /// </summary>
        /// <param name="repriseExcelCi"> le ci type excel</param>     
        /// <param name="context">les données contextuelle a l'import</param>
        /// <returns>le ci de la base correspondant au code ci et </returns>
        public CIEnt GetCiOfDatabase(RepriseExcelCi repriseExcelCi, ContextForImportCi context)
        {
            OrganisationBase societe = context.SocietesOfGroupe.FirstOrDefault(s => s.Code == repriseExcelCi.CodeSociete);

            if (societe == null)
            {
                return null;
            }

            List<OrganisationBase> cisOfSociete = context.OrganisationTree.GetAllCisOfSociete(societe.Id);

            OrganisationBase ciInFredWithCode = cisOfSociete.FirstOrDefault(x => x.Code == repriseExcelCi.CodeCi && (x.IsCi() || x.IsSousCi()));

            if (ciInFredWithCode == null)
            {
                return null;
            }

            CIEnt ciInFred = context.CisUsedInExcel.FirstOrDefault(x => x.CiId == ciInFredWithCode.Id);

            return ciInFred;
        }
    }
}
