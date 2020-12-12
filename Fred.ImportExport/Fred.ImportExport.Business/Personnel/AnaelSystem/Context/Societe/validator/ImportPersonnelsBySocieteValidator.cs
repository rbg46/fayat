using Fred.Business.Societe;
using Fred.Entities.Societe;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Societe.Input;

namespace Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Societe.Validator
{
    public class ImportPersonnelsBySocieteValidator : IImportPersonnelsBySocieteValidator
    {
        /// <summary>
        /// Verifie les regles d'imports
        /// </summary>
        /// <param name="context">le context qui contiens les données necessaire a l'import</param>
        /// <returns>Le resultat de la validation</returns>
        public ImportResult Verify(ImportPersonnelContext<ImportPersonnelsBySocieteInput> context)
        {
            var result = new ImportResult();

            result.IsValid = true;

            //verifie que la societe existe dans fred
            //L'import BySociete prend le postulat qu'une seule societe existe dans le contexte donc on prend la premiere occurence.
            var societe = context.SocietesContexts?[0]?.Societe;
            result.IsValid = societe != null;

            if (result?.IsValid == false)
            {
                FillErrorMessage(ref result, societe);
            }

            return result;

        }

        private void FillErrorMessage(ref ImportResult result, SocieteEnt societe)
        {
            if (societe == null)
            {
                result.ErrorMessages.Add("La societe n'est pas renseigne en entree");
            }
            else
            {
                result.ErrorMessages.Add($"La societe (id : {societe.SocieteId}, libelle : {societe.Libelle}) n'est pas renseigne en entree");
            }
        }
    }
}
