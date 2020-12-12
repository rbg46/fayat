using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Fred.Business.Utilisateur;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.ValidationPointage.Factory.Interfaces;
using Fred.ImportExport.Business.ValidationPointage.Fes.Logs;

namespace Fred.ImportExport.Business.ValidationPointage.Factory
{
    /// <summary>
    /// Classe qui regroupe les setting pour le choix du manager de validation pointage
    /// </summary>
    public class ValidationPointageSettingsProvider : IValidationPointageSettingsProvider
    {
        public const string ValidationPointageRzbFluxCode = "VALIDATION_POINTAGE_RZB";
        public const string ValidationPointageFesFluxCode = "VALIDATION_POINTAGE_FES";
        public const string ValidationPointageFonFluxCode = "VALIDATION_POINTAGE_FON";
        public const string ValidationPointageFtpFluxCode = "VALIDATION_POINTAGE_FTP";
        private static readonly string AnaelConnectionStringRzb = ConfigurationManager.AppSettings["ANAEL:ConnectionString:Groupe:GRZB"];
        private static readonly string AnaelConnectionStringFes = ConfigurationManager.AppSettings["ANAEL:ConnectionString:Groupe:GFES"];
        private static readonly string AnaelConnectionStringFon = ConfigurationManager.AppSettings["ANAEL:ConnectionString:Groupe:GFON"];
        private static readonly string AnaelConnectionStringFtp = ConfigurationManager.AppSettings["ANAEL:ConnectionString:Groupe:GFTP"];

        private readonly List<ValidationPointageFactorySetting> validationPointageFactorySettingsValues = new List<ValidationPointageFactorySetting>()
        {
            new ValidationPointageFactorySetting()
            {
                 FluxCode = ValidationPointageRzbFluxCode,
                 GroupeCode = "GRZB", // code du groupe de l'utilisateur qui demande le controle ou la remontée vrac
                 ConnexionChaineSource = AnaelConnectionStringRzb
            },
              new ValidationPointageFactorySetting()
            {
                 FluxCode = ValidationPointageFesFluxCode,
                 GroupeCode = "GFES", // code du groupe de l'utilisateur qui demande le controle ou la remontée vrac 
                 ConnexionChaineSource = AnaelConnectionStringFes
            },
              new ValidationPointageFactorySetting()
            {
                 FluxCode = ValidationPointageFonFluxCode,
                 GroupeCode = "GFON", // code du groupe de l'utilisateur qui demande le controle ou la remontée vrac 
                 ConnexionChaineSource = AnaelConnectionStringFon
            },
            new ValidationPointageFactorySetting()
            {
                 FluxCode = ValidationPointageFtpFluxCode,
                 GroupeCode = "GFTP", // code du groupe de l'utilisateur qui demande le controle ou la remontée vrac 
                 ConnexionChaineSource = AnaelConnectionStringFtp
            },
        };

        private readonly IUtilisateurManager utilisateurManager;
        private readonly ValidationPointageFesLogger logger;

        public ValidationPointageSettingsProvider(IUtilisateurManager utilisateurManager, ValidationPointageFesLogger logger)
        {
            this.utilisateurManager = utilisateurManager;
            this.logger = logger;
        }

        /// <summary>
        /// Retourne le ValidationPointageFactorySetting en fonction du code du groupe de l'utilisteur
        /// </summary>
        /// <param name="utilisateurId">utilisateurId</param>
        /// <returns>ValidationPointageFactorySetting</returns>
        public ValidationPointageFactorySetting GetFactorySetting(int utilisateurId)
        {
            UtilisateurEnt utilisateur = utilisateurManager.GetUtilisateurById(utilisateurId);

            if (utilisateur != null)
            {
                var groupeCode = utilisateur.Personnel.Societe.Groupe.Code.Trim();

                var setting = validationPointageFactorySettingsValues.FirstOrDefault(s => s.GroupeCode == groupeCode);
                if (setting == null)
                {
                    var error = this.logger.ErrorNoConfigForGroupe(groupeCode);
                    throw new FredBusinessException(error);
                }
                return setting;
            }
            else
            {
                var error = this.logger.ErrorUserNotFound(utilisateurId);
                throw new FredBusinessException(error);
            }
        }
    }
}
