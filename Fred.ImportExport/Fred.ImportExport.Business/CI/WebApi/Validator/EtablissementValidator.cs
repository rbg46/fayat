using System.Linq;
using Fred.ImportExport.Business.CI.WebApi.Context.Models;
using Fred.ImportExport.Framework.Exceptions;

namespace Fred.ImportExport.Business.CI.WebApi.Validator
{
    /// <summary>
    /// Valid les etablissement comptables
    /// </summary>
    public class EtablissementValidator : IEtablissementValidator
    {
        /// <summary>
        /// Verifie existance de l'etablissement
        /// </summary>
        /// <param name="context">le context</param>
        public void VerifyAllEtablissementFoundForAllWebApiCis(ImportCiByWebApiContext context)
        {
            foreach (var societeContext in context.SocietesContexts)
            {
                VerifyEtablissementFoundForWebApiCi(societeContext);
            }
        }

        private void VerifyEtablissementFoundForWebApiCi(ImportCiByWebApiSocieteContext societeContext)
        {
            foreach (var webApiCi in societeContext.CisToImport)
            {
                var etablissementComptable = societeContext.EtablissementComptables.FirstOrDefault(x => x.Code == webApiCi.CodeEtablissementComptable);
                if (etablissementComptable == null)
                {
                    string errorMessage = string.Format(FredImportExportBusinessResources.Error_Import_Web_Api_Etablissement_Not_Found, webApiCi.CodeEtablissementComptable);

                    throw new FredIeBusinessException(errorMessage);
                }
            }
        }

    }
}
