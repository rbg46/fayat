using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Business.OperationDiverse.ImportODfromExcel.ContextProvider;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;

namespace Fred.Business.OperationDiverse.ImportODfromExcel.Selectors
{

    /// <summary>
    /// Service qui recupére entités de la base en fonction des données contextuelle a l'import
    /// - get UniteEnt par codeUnite
    /// - get DeviseEnt par codeDevise
    /// - get Ressource 
    /// </summary>
    public class OperationDiverseSelectors
    {
        /// <summary>
        ///  Recupere l'unité de fred, en fonction du code unité
        /// </summary>
        /// <param name="codeUnite">code unite</param>
        /// <param name="context">les données contextuelle a l'import</param>
        /// <returns>unité de la base de fred</returns>
        public UniteEnt GetUnite(string codeUnite, ContextForImportOD context)
        {
            return
            (string.IsNullOrEmpty(codeUnite)) ? context.DefaultUnite : context.UnitsUsedInExcel.FirstOrDefault(x => x.Code == codeUnite);
        }

        /// <summary>
        ///  Recupere le devise, en fonction du code deplacement
        /// </summary>
        /// <param name="codeDevise">code Devise</param>
        /// <param name="context">les données contextuelle a l'import</param>
        /// <returns>devise de la base de fred</returns>
        public DeviseEnt GetDevise(string codeDevise, ContextForImportOD context)
        {
            return
            (string.IsNullOrEmpty(codeDevise)) ? context.DefaultDevise : context.DevisesUsedInExcel.FirstOrDefault(x => x.IsoCode == codeDevise);
        }

        /// <summary>
        ///  Recupere le code deplacement de fred, en fonction du codeSociete et du code deplacement
        /// </summary>
        /// <param name="codeSociete">code de la societe</param>
        /// <param name="codeFamille">code famille</param>
        /// <param name="context">les données contextuelle a l'import</param>
        /// <returns>codeDeplacement de la base de fred</returns>
        public FamilleOperationDiverseEnt GetFamilleOD(string codeSociete,string codeFamille, ContextForImportOD context)
        {
            OrganisationBase societe = context.SocietesOfGroupe.FirstOrDefault(x => x.Code == codeSociete);

            if (societe == null)
            {
                return null;
            }
            return context.FamillesOdUsedInExcel.FirstOrDefault(x => x.SocieteId == societe.Id && x.Code == codeFamille);
        }

        /// <summary>
        ///  Recupere le code deplacement de fred, en fonction du codeSociete et du code deplacement
        /// </summary>
        /// <param name="codeRessource">code deplacement</param>
        /// <param name="context">les données contextuelle a l'import</param>
        /// <returns>codeDeplacement de la base de fred</returns>
        public RessourceEnt GetRessource(string codeRessource, ContextForImportOD context)
        {
            return context.RessourcesUsedInExcel.FirstOrDefault(x => x.Code == codeRessource);
        }
    }
}
