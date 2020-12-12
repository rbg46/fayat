using System.Linq;
using Fred.Entities;
using Fred.Entities.RepriseDonnees.Rule;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Societe.Input;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Context.Ci.Validator
{
    /// <summary>
    /// Validator pour l'import de ci par societe
    /// </summary>
    public class ImportCiBySocieteValidator
    {
        /// <summary>
        /// Verifie les regles d'imports
        /// </summary>
        /// <param name="context">le context qui contiens les données necessaire a l'imports</param>
        /// <returns>Le resultat de la validation</returns>
        public ImportResult VerifyRules(ImportCiContext<ImportCisBySocieteInputs> context)
        {
            var result = new ImportResult();

            result.IsValid = true;

            var ruleSocieteExist = VerifySocieteExistInFred(context);

            if (!ruleSocieteExist.IsValid)
            {
                result.IsValid = false;

                result.ErrorMessages.Add(ruleSocieteExist.ErrorMessage);
            }

            var ciOfGroupeRzbRule = VerifySocieteOfGroupeRzb(context);

            if (!ciOfGroupeRzbRule.IsValid)
            {
                result.IsValid = false;

                result.ErrorMessages.Add(ciOfGroupeRzbRule.ErrorMessage);
            }

            return result;

        }

        private RuleResult VerifySocieteExistInFred(ImportCiContext<ImportCisBySocieteInputs> context)
        {
            var result = new RuleResult();

            var messageErreur = string.Format("Rejet de l'import pour la societe n°{0} : non trouvé dans FRED.", context.Input.CodeSocieteCompatble);

            result.IsValid = context.SocietesNeeded.FirstOrDefault() != null;

            result.ErrorMessage = result.IsValid ? string.Empty : messageErreur;

            return result;

        }


        private RuleResult VerifySocieteOfGroupeRzb(ImportCiContext<ImportCisBySocieteInputs> context)
        {
            var result = new RuleResult();

            var messageErreur = "Rejet du la societe : Seul les Cis du groupe GRZB peuvent utiliser le systeme Anael.";

            var societe = context.SocietesNeeded.FirstOrDefault();

            if (societe != null)
            {
                var groupParentOfCid = context.OrganisationTree.GetGroupeParentOfSociete(societe.SocieteId);

                result.IsValid = string.Equals(groupParentOfCid.Code, Constantes.CodeGroupeRZB, System.StringComparison.CurrentCultureIgnoreCase);
            }

            result.ErrorMessage = result.IsValid ? string.Empty : messageErreur;

            return result;

        }

    }
}
