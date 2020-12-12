using System.Linq;
using Fred.ImportExport.Business.CI.WebApi.Context.Models;
using Fred.ImportExport.Framework.Exceptions;

namespace Fred.ImportExport.Business.CI.WebApi.Validator
{
    /// <summary>
    /// Service qui verfie les regles sue les societes
    /// </summary>
    public class SocieteValidator : ISocieteValidator
    {
        /// <summary>
        /// Verfie que les societes existes
        /// </summary>
        /// <param name="context">le context</param>
        public void VerifyAllSocietesFound(ImportCiByWebApiContext context)
        {
            var firstSocieteContextWithSocieteToNull = context.SocietesContexts.FirstOrDefault(x => x.Societe == null);

            if (firstSocieteContextWithSocieteToNull != null)
            {
                string errorMessage = string.Format(FredImportExportBusinessResources.Error_Import_Web_Api_Societe_Not_Found, firstSocieteContextWithSocieteToNull.CodeSocieteComptable);

                throw new FredIeBusinessException(errorMessage);
            }
        }


        /// <summary>
        /// Verifie que les societes sont actives
        /// </summary>
        /// <param name="context">le context</param>
        public void VerifyAllSocieteAreActives(ImportCiByWebApiContext context)
        {
            var firstSocieteContextWithSocieteToNull = context.SocietesContexts.FirstOrDefault(x => x.Societe != null && !x.Societe.Active);

            if (firstSocieteContextWithSocieteToNull != null)
            {
                string errorMessage = string.Format(FredImportExportBusinessResources.Error_Import_Web_Api_Societe_Not_Active, firstSocieteContextWithSocieteToNull.CodeSocieteComptable);

                throw new FredIeBusinessException(errorMessage);
            }
        }

    }
}
