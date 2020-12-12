using Fred.ImportExport.Business.CI.WebApi.Context.Models;

namespace Fred.ImportExport.Business.CI.WebApi.Validator
{
    /// <summary>
    /// Service qui Verifie toutes le regles d'import
    /// </summary>
    public class ImportCiByWebApiValidator : IImportCiByWebApiValidator
    {
        private readonly IConsistencyValidator consistencyValidator;
        private readonly IEtablissementValidator etablissementValidator;
        private readonly ISocieteValidator societeValidator;

        public ImportCiByWebApiValidator(IConsistencyValidator consistencyValidator, IEtablissementValidator etablissementValidator, ISocieteValidator societeValidator)
        {
            this.consistencyValidator = consistencyValidator;
            this.etablissementValidator = etablissementValidator;
            this.societeValidator = societeValidator;
        }

        /// <summary>
        /// Verifie les regles d'imports
        /// </summary>
        /// <param name="context">le context qui contiens les données necessaire a l'imports</param>       
        public void VerifyRulesAndThrowIfNecessary(ImportCiByWebApiContext context)
        {
            societeValidator.VerifyAllSocietesFound(context);

            societeValidator.VerifyAllSocieteAreActives(context);

            etablissementValidator.VerifyAllEtablissementFoundForAllWebApiCis(context);

            consistencyValidator.VerifyCodeAndLibelleForAllWebApiCis(context);

        }
    }
}
