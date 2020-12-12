using Fred.ImportExport.Business.CI.WebApi.Context.Models;

namespace Fred.ImportExport.Business.CI.WebApi.Validator
{
    /// <summary>
    /// verfie la consitance des données recues
    /// </summary>
    public interface IConsistencyValidator : IFredIEService
    {
        /// <summary>
        /// verifie si code et le libelle sont remplit
        /// </summary>
        /// <param name="context">Le context par societe</param>
        void VerifyCodeAndLibelleForAllWebApiCis(ImportCiByWebApiContext context);


    }
}
