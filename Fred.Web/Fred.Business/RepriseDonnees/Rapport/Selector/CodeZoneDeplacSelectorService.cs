using System.Linq;
using Fred.Business.RepriseDonnees.Rapport.ContextProviders;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;

namespace Fred.Business.RepriseDonnees.Rapport.Selector
{
    /// <summary>
    /// Service qui recuper le personnel de la base en fonction des données contextuelle a l'import
    /// </summary>
    public class CodeZoneDeplacSelectorService
    {
        /// <summary>
        ///  Recupere le code zone deplacement de fred, en fonction du codeSociete et du code zone deplacement
        /// </summary>
        /// <param name="codeSociete">code de la societe</param>
        /// <param name="codeZoneDeplacement">codeZoneDeplacement</param>
        /// <param name="context">les données contextuelle a l'import</param>
        /// <returns>codeZoneDeplacement de la base de fred</returns>
        public CodeZoneDeplacementEnt GetCodeZoneDeplacement(string codeSociete, string codeZoneDeplacement, ContextForImportRapport context)
        {
            OrganisationBase societe = context.SocietesOfGroupe.FirstOrDefault(x => x.Code == codeSociete);

            if (societe == null)
            {
                return null;
            }

            return context.CodeZoneDeplacementsUsedInExcel.FirstOrDefault(x => x.SocieteId == societe.Id && x.Code == codeZoneDeplacement);
        }

    }
}
