using CommonServiceLocator;
using Fred.ImportExport.Business.ValidationPointage.Factory.Interfaces;
using Fred.ImportExport.Business.ValidationPointage.Fes;
using Fred.ImportExport.Business.ValidationPointage.Fes.Logs;
using Fred.ImportExport.Business.ValidationPointage.Fon;
using Fred.ImportExport.Business.ValidationPointage.Ftp;
using Fred.ImportExport.Business.ValidationPointage.Rzb;

namespace Fred.ImportExport.Business.ValidationPointage.Factory
{
    public class ValidationPointageFluxFactory : IValidationPointageFluxFactory
    {
        private readonly ValidationPointageFesLogger logger;
        private readonly IValidationPointageSettingsProvider validationPointageSettingsProvider;

        public ValidationPointageFluxFactory(ValidationPointageFesLogger logger,
            IValidationPointageSettingsProvider validationPointageSettingsProvider)
        {
            this.logger = logger;
            this.validationPointageSettingsProvider = validationPointageSettingsProvider;
        }

        /// <summary>
        ///  Permet de resoudre le gestionnaire métier du flux des lots de pointage pour la validation pointage
        /// </summary>
        /// <param name="utilisateurId">utilisateurId</param>
        /// <returns>IValidationPointageFluxManager</returns>
        public IValidationPointageFluxManager GetFluxManager(int utilisateurId)
        {
            IValidationPointageFluxManager result = null;

            ValidationPointageFactorySetting setting = validationPointageSettingsProvider.GetFactorySetting(utilisateurId);

            this.logger.LogSelectedSettings(setting);

            switch (setting.FluxCode)
            {
                case ValidationPointageSettingsProvider.ValidationPointageRzbFluxCode:
                    result = ServiceLocator.Current.GetInstance<IValidationPointageFluxManagerRzb>();
                    break;
                case ValidationPointageSettingsProvider.ValidationPointageFesFluxCode:
                    result = ServiceLocator.Current.GetInstance<IValidationPointageFluxManagerFes>();
                    break;
                case ValidationPointageSettingsProvider.ValidationPointageFonFluxCode:
                    result = ServiceLocator.Current.GetInstance<IValidationPointageFluxManagerFon>();
                    break;
                case ValidationPointageSettingsProvider.ValidationPointageFtpFluxCode:
                    result = ServiceLocator.Current.GetInstance<IValidationPointageFluxManagerFtp>();
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
