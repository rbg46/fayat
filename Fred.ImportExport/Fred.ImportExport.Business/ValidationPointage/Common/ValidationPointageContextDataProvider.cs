using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Entities.Utilisateur;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.ValidationPointage.Factory.Interfaces;

namespace Fred.ImportExport.Business.ValidationPointage.Common
{
    /// <summary>
    /// Provider qui founie les données contextuelle
    /// </summary>
    public class ValidationPointageContextDataProvider : IValidationPointageContextDataProvider
    {
        private readonly IUtilisateurManager utilisateurManager;
        private readonly ISocieteManager societeManager;
        private readonly IValidationPointageSettingsProvider validationPointageSettingsProvider;
        private readonly FluxManager fluxManager;

        public ValidationPointageContextDataProvider(IUtilisateurManager utilisateurManager, ISocieteManager societeManager, IValidationPointageSettingsProvider validationPointageSettingsProvider, FluxManager fluxManager)
        {
            this.utilisateurManager = utilisateurManager;
            this.societeManager = societeManager;
            this.validationPointageSettingsProvider = validationPointageSettingsProvider;
            this.fluxManager = fluxManager;
        }

        /// <summary>
        /// Recupere les données contextuelles pour un controle et une remontée vrac
        /// </summary>
        /// <param name="utilisateurId">utilisateurId</param>
        /// <param name="societeId">societeId</param>
        /// <param name="jobId">jobId</param>
        /// <returns>ControleVracContextData</returns>
        public ValidationPointageContextData GetGlobalData(int utilisateurId, int societeId, string jobId)
        {
            ValidationPointageContextData controleVracGlobalData = new ValidationPointageContextData();
            var setting = validationPointageSettingsProvider.GetFactorySetting(utilisateurId);
            var flux = fluxManager.GetByCode(setting.FluxCode);
            var societe = societeManager.GetSocieteById(societeId);
            UtilisateurEnt utilisateur = this.utilisateurManager.GetById(utilisateurId);
            string nomUtilisateur = UsernameFormatter(utilisateur?.Personnel?.Nom);

            controleVracGlobalData.FluxSetting = setting;
            controleVracGlobalData.Flux = flux;
            controleVracGlobalData.ConnexionChaineSource = setting.ConnexionChaineSource;
            controleVracGlobalData.CodeSocietePaye = societe.CodeSocietePaye;
            controleVracGlobalData.CodeSocieteComptable = societe.CodeSocieteComptable;
            controleVracGlobalData.Utilisateur = utilisateur;
            controleVracGlobalData.NomUtilisateur = nomUtilisateur;
            controleVracGlobalData.JobId = jobId;

            return controleVracGlobalData;
        }

        private string UsernameFormatter(string name)
        {
            return name?.ToUpper().Replace(" ", string.Empty).Replace("'", string.Empty).Replace("-", string.Empty).ToUpper();
        }
    }
}
