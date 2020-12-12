using System;
using Fred.Business.Params;
using Fred.Business.Utilisateur;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Helpers;

namespace Fred.Business.OrganisationFeature
{
    /// <summary>
    /// Service de récuperation de l'état (activé ou désactivé) d'une feature par rapport à une organisation
    /// </summary>
    public class OrganisationRelatedFeatureService : IOrganisationRelatedFeatureService
    {
        private readonly IParamsManager paramsManager;
        private readonly IUtilisateurManager userManager;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="paramsManager">Le paramsManager</param>
        /// <param name="userManager">Le UtilisateurManager</param>
        public OrganisationRelatedFeatureService(IParamsManager paramsManager, IUtilisateurManager userManager)
        {
            this.paramsManager = paramsManager;
            this.userManager = userManager;
        }

        /// <summary>
        /// récuperation de l'état (activé ou désactivé) d'une feature par rapport à l'organisation de l'utilisateur courant
        /// </summary>
        /// <param name="featureKey">la clé de la feature</param>
        /// <param name="defaultValue">la valeur par défaut à retourner si aucune clé n'est présente</param>
        /// <returns>Activé ou désactivée</returns>
        public bool IsEnabledForCurrentUser(string featureKey, bool defaultValue)
        {
            UtilisateurEnt utilisateur = userManager.GetContextUtilisateur();
            int? organisationId = utilisateur?.Personnel?.Societe?.Organisation?.OrganisationId;
            if (!organisationId.HasValue)
            {
                throw new FredBusinessException(FredResource.CantFindUserOrganisation);
            }

            string paramValue = paramsManager.GetParamValue(organisationId.Value, featureKey);
            if (!string.IsNullOrEmpty(paramValue))
            {
                return ConvertParamValueToBool(paramValue, defaultValue);
            }

            return defaultValue;
        }

        private bool ConvertParamValueToBool(string paramValue, bool defaultValue)
        {
            try
            {
                BoolParsingHelper boolParsingHelper = new BoolParsingHelper();
                return boolParsingHelper.LooseParse(paramValue);
            } catch(ArgumentException e)
            {
                return defaultValue;
            }
        }
    }
}
