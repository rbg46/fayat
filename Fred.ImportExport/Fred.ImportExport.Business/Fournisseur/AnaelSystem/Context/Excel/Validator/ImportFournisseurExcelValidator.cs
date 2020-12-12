using System.Linq;
using Fred.Entities.RepriseDonnees.Rule;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Excel.Input;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem.Excel.Validator
{
    /// <summary>
    /// Validator pour l'import de ci par fichier excel
    /// </summary>
    public class ImportFournisseurExcelValidator
    {
        /// <summary>
        /// Verifie les regles d'imports
        /// </summary>
        /// <param name="context">le context qui contiens les données necessaire a l'imports</param>
        /// <returns>Le resultat de la validation</returns>
        public ImportResult Verify(ImportFournisseurContext<ImportFournisseursByExcelInputs> context)
        {
            var result = new ImportResult();

            if (context.Input.RepriseImportFournisseurs == null || !context.Input.RepriseImportFournisseurs.Any())
            {
                result.ErrorMessages.Add("Pas de fournisseur renseignés dans la source de données");
            }
            else
            {
                foreach (var repriseImportFournisseur in context.Input.RepriseImportFournisseurs)
                {
                    //Rules pour les champs
                    var fieldsRule = VerifyFieldsRule(repriseImportFournisseur.CodeFournisseur, repriseImportFournisseur.TypeSequence, repriseImportFournisseur.NumeroDeLigne);
                    if (!fieldsRule.IsValid)
                    {
                        result.ErrorMessages.Add(fieldsRule.ErrorMessage);
                    }

                    //Rules pour les fournisseurs venant d'Anael
                    var codeFournisseurRule = VerifyCodeFournisseurRule(context, repriseImportFournisseur.CodeFournisseur, repriseImportFournisseur.TypeSequence, repriseImportFournisseur.NumeroDeLigne);
                    if (!codeFournisseurRule.IsValid)
                    {
                        result.ErrorMessages.Add(codeFournisseurRule.ErrorMessage);
                    }

                    //Rules pour la coherence des types sequences
                    var typeSequenceRule = VerifyTypeSequenceRule(context, repriseImportFournisseur.TypeSequence, repriseImportFournisseur.NumeroDeLigne);
                    if (!typeSequenceRule.IsValid)
                    {
                        result.ErrorMessages.Add(typeSequenceRule.ErrorMessage);
                    }
                }
            }

            result.IsValid = !result.ErrorMessages.Any();

            return result;

        }

        private RuleResult VerifyFieldsRule(string codeFournisseur, string typeSequence, string numeroDeLigne)
        {
            var result = new RuleResult
            {
                ErrorMessage = string.Empty,
                //Le code fournisseur existe dans les fournisseurs recuperes par Anael
                IsValid = !string.IsNullOrEmpty(codeFournisseur) && !string.IsNullOrEmpty(typeSequence)
            };

            //Si non valide, on ajoute le message d'erreur
            if (!result.IsValid)
            {
                result.ErrorMessage = string.Format("Rejet ligne n°{0} : champ(s) obligatoire(s) non renseigné(s).", numeroDeLigne);
            }

            return result;
        }

        private RuleResult VerifyTypeSequenceRule(ImportFournisseurContext<ImportFournisseursByExcelInputs> context, string typeSequence, string numeroDeLigne)
        {
            var result = new RuleResult
            {
                ErrorMessage = string.Empty,
                //Le code fournisseur existe dans les fournisseurs recuperes par Anael
                IsValid = context.TypeSequences.Contains(typeSequence)
            };

            //Si non valide, on ajoute le message d'erreur
            if (!result.IsValid)
            {
                result.ErrorMessage = string.Format("Rejet ligne n°{0} : Type sequence non valide pour cette société.", numeroDeLigne);
            }

            return result;
        }

        private RuleResult VerifyCodeFournisseurRule(ImportFournisseurContext<ImportFournisseursByExcelInputs> context, string codeFournisseur, string typeSequence, string numeroDeLigne)
        {
            var result = new RuleResult
            {
                ErrorMessage = string.Empty,
                //Le code fournisseur existe dans les fournisseurs recuperes par Anael
                IsValid = context.SocietesContexts.Any(s => s.AnaelFournisseurs != null && s.AnaelFournisseurs.Any(f => f.Code == codeFournisseur && f.TypeSequence == typeSequence))
            };

            //Si non valide, on ajoute le message d'erreur
            if (!result.IsValid)
            {
                result.ErrorMessage = string.Format("Rejet ligne n°{0} : fournisseur non trouvé dans Anael.", numeroDeLigne);
            }

            return result;
        }
    }
}
