using Fred.ImportExport.Business.CI.WebApi.Context.Models;

namespace Fred.ImportExport.Business.CI.WebApi.Validator
{
    /// <summary>
    /// Service qui verfie les regles sue les societes
    /// </summary>
    public interface ISocieteValidator : IFredIEService
    {
        /// <summary>
        /// Verfie que les societes existes
        /// </summary>
        /// <param name="context">le context</param>
        void VerifyAllSocietesFound(ImportCiByWebApiContext context);
        /// <summary>
        /// Verifie que les societes sont actives
        /// </summary>
        /// <param name="context">le context</param>
        void VerifyAllSocieteAreActives(ImportCiByWebApiContext context);
    }
}
