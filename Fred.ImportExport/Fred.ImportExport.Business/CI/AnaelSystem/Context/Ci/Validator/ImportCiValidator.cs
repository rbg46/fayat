using Fred.Business.Organisation.Tree;
using Fred.Entities;
using Fred.Entities.RepriseDonnees.Rule;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Ci.Input;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Common;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Context.Ci.Validator
{
    public class ImportCiValidator
    {
        /// <summary>
        /// Verifie les regles d'imports
        /// </summary>
        /// <param name="context">le context qui contiens les données necessaire a l'import</param>
        /// <returns>Le resultat de la validation</returns>
        public ImportResult Verify(ImportCiContext<ImportCisByCiListInputs> context)
        {
            var result = new ImportResult();
            result.IsValid = true;

            foreach (var ciId in context.Input.CiIds)
            {
                // on verifie ici que j'ai bien un ci qui correspond a l'id en effet quand je charge les ci dans le context 
                // il ce peux qu'aucun ci ne soit trouvé dans fred.(clause sql contains)
                var ruleResult = VerifyCiExistInFred(context, ciId);
                if (!ruleResult.IsValid)
                {
                    result.IsValid = false;
                    result.ErrorMessages.Add(ruleResult.ErrorMessage);
                }
                var ciOfGroupeRzbRule = VerifyCiOfGroupeRzb(context, ciId);
                if (!ciOfGroupeRzbRule.IsValid)
                {
                    result.IsValid = false;
                    result.ErrorMessages.Add(ciOfGroupeRzbRule.ErrorMessage);
                }
            }
            return result;

        }

        private RuleResult VerifyCiExistInFred(ImportCiContext<ImportCisByCiListInputs> context, int ciId)
        {
            var result = new RuleResult();

            var messageErreur = string.Format("Rejet du ciId n°{0} : non trouvé dans FRED.", ciId);

            var fredCi = context.OrganisationTree.GetCi(ciId);

            result.IsValid = fredCi != null;

            result.ErrorMessage = result.IsValid ? string.Empty : messageErreur;

            return result;

        }


        private RuleResult VerifyCiOfGroupeRzb(ImportCiContext<ImportCisByCiListInputs> context, int ciId)
        {
            var result = new RuleResult();

            var messageErreur = string.Format("Rejet du ciId n°{0} : Seul les Cis du groupe GRZB peuvent utiliser le systeme Anael.", ciId);

            var groupParentOfCid = context.OrganisationTree.GetGroupeParentOfCi(ciId);

            result.IsValid = string.Equals(groupParentOfCid.Code, Constantes.CodeGroupeRZB, System.StringComparison.CurrentCultureIgnoreCase);

            result.ErrorMessage = result.IsValid ? string.Empty : messageErreur;

            return result;

        }
    }
}
