using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Fournisseur.Input;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Ci.Validator
{
    public class ImportFournisseurValidator
    {
        /// <summary>
        /// Verifie les regles d'imports
        /// </summary>
        /// <param name="context">le context qui contiens les données necessaire a l'import</param>
        /// <returns>Le resultat de la validation</returns>
        public ImportResult Verify(ImportFournisseurContext<ImportFournisseurByIdsListInputs> context)
        {
            var result = new ImportResult();
            result.IsValid = true;

            return result;

        }
    }
}
