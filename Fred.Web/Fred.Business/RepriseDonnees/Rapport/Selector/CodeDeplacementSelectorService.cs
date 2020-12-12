using System.Linq;
using Fred.Business.RepriseDonnees.Rapport.ContextProviders;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;

namespace Fred.Business.RepriseDonnees.Rapport.Selector
{
    /// <summary>
    /// Service qui recuper le code deplacement de la base en fonction des données contextuelle a l'import
    /// </summary>
    public class CodeDeplacementSelectorService
    {
        /// <summary>
        ///  Recupere le code deplacement de fred, en fonction du codeSociete et du code deplacement
        /// </summary>
        /// <param name="codeSociete">code de la societe</param>
        /// <param name="codeDeplacement">code deplacement</param>
        /// <param name="context">les données contextuelle a l'import</param>
        /// <returns>codeDeplacement de la base de fred</returns>
        public CodeDeplacementEnt GetCodeDeplacement(string codeSociete, string codeDeplacement, ContextForImportRapport context)
        {
            OrganisationBase societe = context.SocietesOfGroupe.FirstOrDefault(x => x.Code == codeSociete);

            if (societe == null)
            {
                return null;
            }

            return context.CodeDeplacementsUsedInExcel.FirstOrDefault(x => x.SocieteId == societe.Id && x.Code == codeDeplacement);
        }

    }
}
