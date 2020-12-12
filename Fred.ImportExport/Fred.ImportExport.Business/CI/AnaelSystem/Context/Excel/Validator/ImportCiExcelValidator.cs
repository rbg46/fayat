using System.Linq;
using Fred.Entities;
using Fred.Entities.RepriseDonnees.Rule;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Excel.Input;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Context.Excel.Validator
{
    /// <summary>
    /// Validator pour l'import de ci par fichier excel
    /// </summary>
    public class ImportCiExcelValidator
    {
        /// <summary>
        /// Verifie les regles d'imports
        /// </summary>
        /// <param name="context">le context qui contiens les données necessaire a l'imports</param>
        /// <returns>Le resultat de la validation</returns>
        public ImportResult Verify(ImportCiContext<ImportCisByExcelInputs> context)
        {
            var result = new ImportResult();

            foreach (var repriseImportCi in context.Input.RepriseImportCis)
            {
                var codeSocieteRule = VerifyCodeSocieteRule(context, repriseImportCi.CodeSociete, repriseImportCi.NumeroDeLigne);
                if (!codeSocieteRule.IsValid)
                {
                    result.ErrorMessages.Add(codeSocieteRule.ErrorMessage);
                }
                var codeCiRule = VerifyCodeCiRule(context, repriseImportCi.CodeSociete, repriseImportCi.CodeCi, repriseImportCi.NumeroDeLigne);
                if (!codeCiRule.IsValid)
                {
                    result.ErrorMessages.Add(codeCiRule.ErrorMessage);
                }
                var ciOfGroupeRzbRule = VerifyCiOfGroupeRzb(context, repriseImportCi.CodeSociete, repriseImportCi.NumeroDeLigne);
                if (!ciOfGroupeRzbRule.IsValid)
                {
                    result.IsValid = false;
                    result.ErrorMessages.Add(ciOfGroupeRzbRule.ErrorMessage);
                }
            }

            result.IsValid = !result.ErrorMessages.Any();

            return result;

        }

        private RuleResult VerifyCodeSocieteRule(ImportCiContext<ImportCisByExcelInputs> context, string codeSociete, string numeroDeLigne)
        {
            var result = new RuleResult();

            var messageErreur = string.Format("Rejet ligne n°{0} : Code Société Comptable non trouvé dans FRED.", numeroDeLigne);

            result.IsValid = context.SocietesContexts.FirstOrDefault(x => x.Societe?.Code == codeSociete) != null;

            result.ErrorMessage = result.IsValid ? string.Empty : messageErreur;

            return result;

        }

        private RuleResult VerifyCodeCiRule(ImportCiContext<ImportCisByExcelInputs> context, string codeSociete, string codeCi, string numeroDeLigne)
        {
            var result = new RuleResult();

            var messageErreur = string.Format("Rejet ligne n°{0} : CI non trouvé dans ANAEL.", numeroDeLigne);

            result.IsValid = CodeCiIsValid(context, codeSociete, codeCi);

            result.ErrorMessage = result.IsValid ? string.Empty : messageErreur;

            return result;
        }

        private bool CodeCiIsValid(ImportCiContext<ImportCisByExcelInputs> context, string codeSociete, string codeCi)
        {
            var result = false;

            var societeContext = context.SocietesContexts.FirstOrDefault(x => x.Societe?.Code == codeSociete);

            if (societeContext != null)
            {
                var cisOfSociete = societeContext.AnaelCis;

                result = cisOfSociete.Select(x => x.CodeAffaire).ToList().Contains(codeCi);
            }

            return result;
        }


        private RuleResult VerifyCiOfGroupeRzb(ImportCiContext<ImportCisByExcelInputs> context, string codeSociete, string numeroDeLigne)
        {
            var result = new RuleResult();

            var messageErreur = string.Format("Rejet du ligne n°{0} : Seul les Cis du groupe GRZB peuvent utiliser le systeme Anael.", numeroDeLigne);

            var societeContext = context.SocietesContexts.FirstOrDefault(x => x.Societe?.Code == codeSociete);

            if (societeContext != null && societeContext.Societe != null)
            {
                var groupParentOfCid = context.OrganisationTree.GetGroupeParentOfSociete(societeContext.Societe.SocieteId);

                result.IsValid = string.Equals(groupParentOfCid.Code, Constantes.CodeGroupeRZB, System.StringComparison.CurrentCultureIgnoreCase);
            }

            result.ErrorMessage = result.IsValid ? string.Empty : messageErreur;

            return result;

        }

    }
}
