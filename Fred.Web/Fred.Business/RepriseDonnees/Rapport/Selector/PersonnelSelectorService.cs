using System.Linq;
using Fred.Business.RepriseDonnees.Rapport.ContextProviders;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Personnel;

namespace Fred.Business.RepriseDonnees.Rapport.Selector
{
    /// <summary>
    /// Service qui recuper le personnel de la base en fonction des données contextuelle a l'import
    /// </summary>
    public class PersonnelSelectorService
    {
        /// <summary>
        ///  Recupere le personnel de fred, en fonction du codeSociete et du matricule
        /// </summary>
        /// <param name="codeSociete">code de la societe</param>
        /// <param name="matricule">matricule du personnel</param>
        /// <param name="context">les données contextuelle a l'import</param>
        /// <returns>le personne lde la base de fred</returns>
        public PersonnelEnt GetPersonnel(string codeSociete, string matricule, ContextForImportRapport context)
        {
            OrganisationBase societe = context.SocietesOfGroupe.FirstOrDefault(x => x.Code == codeSociete);

            if (societe == null)
            {
                return null;
            }

            return context.PersonnelsUsedInExcel.FirstOrDefault(x => x.SocieteId == societe.Id && x.Matricule == matricule);
        }

    }
}
