using Fred.ImportExport.Business.CI.WebApi.Context.Models;

namespace Fred.ImportExport.Business.CI.WebApi.Validator
{
    /// <summary>
    /// Valid les etablissement comptables
    /// </summary>
    public interface IEtablissementValidator : IFredIEService
    {
        /// <summary>
        /// Verifie existance de l'etablissement
        /// </summary>
        /// <param name="context">le context</param>
        void VerifyAllEtablissementFoundForAllWebApiCis(ImportCiByWebApiContext context);
    }
}
