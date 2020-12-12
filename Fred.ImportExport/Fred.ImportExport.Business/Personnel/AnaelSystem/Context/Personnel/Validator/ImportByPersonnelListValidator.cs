using Fred.Business.Societe;
using Fred.Entities.Societe;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Personnel.Input;

namespace Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Societe.Validator
{
    public class ImportByPersonnelListValidator : IImportByPersonnelListValidator
    {
        /// <summary>
        /// Verifie les regles d'imports
        /// </summary>
        /// <param name="context">le context qui contiens les données necessaire a l'import</param>
        /// <returns>Le resultat de la validation</returns>
        public ImportResult Verify(ImportPersonnelContext<ImportByPersonnelListInputs> context)
        {
            var result = new ImportResult
            {
                IsValid = true
            };

            //verifie que les societes stockes dans la liste societesContext existe dans fred
            if (context.SocietesContexts != null)
            {
                foreach (var societeContext in context.SocietesContexts)
                {
                    var societe = societeContext?.Societe;

                    result.IsValid = societe != null;

                    if (result?.IsValid == false)
                    {
                        FillErrorMessage(ref result, societe);
                    }
                }
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
