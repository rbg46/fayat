using Fred.ImportExport.Business.CI.WebApi.Context.Models;

namespace Fred.ImportExport.Business.CI.WebApi.Validator
{
    /// <summary>
    /// Service qui Verifie toutes le regles d'import
    /// </summary>
    public interface IImportCiByWebApiValidator : IFredIEService
    {
        /// <summary>
        /// Verifie toutes le regles d'import
        /// </summary>
        /// <param name="context">le context</param>
        void VerifyRulesAndThrowIfNecessary(ImportCiByWebApiContext context);
    }
}
