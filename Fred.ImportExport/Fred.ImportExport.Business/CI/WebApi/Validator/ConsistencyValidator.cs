using Fred.ImportExport.Business.CI.WebApi.Context.Models;
using Fred.ImportExport.Framework.Exceptions;

namespace Fred.ImportExport.Business.CI.WebApi.Validator
{
    /// <summary>
    /// verfie la consitance des données recues
    /// </summary>
    public class ConsistencyValidator : IConsistencyValidator
    {
        /// <summary>
        /// verifie si code et le libelle sont remplit
        /// </summary>
        /// <param name="context">Le context par societe</param>
        public void VerifyCodeAndLibelleForAllWebApiCis(ImportCiByWebApiContext context)
        {
            foreach (var societeContext in context.SocietesContexts)
            {
                VerifyCodeAndLibelle(societeContext);
            }
        }

        private void VerifyCodeAndLibelle(ImportCiByWebApiSocieteContext societeContext)
        {
            foreach (var webApiCi in societeContext.CisToImport)
            {
                if (string.IsNullOrEmpty(webApiCi.Code) || string.IsNullOrEmpty(webApiCi.Libelle))
                {
                    string errorMessage = string.Format(FredImportExportBusinessResources.Error_Import_Web_Api_Code_Or_Libelle_Not_Found, webApiCi.Code, webApiCi.Libelle, webApiCi.Description);

                    throw new FredIeBusinessException(errorMessage);
                }
            }
        }

    }
}
