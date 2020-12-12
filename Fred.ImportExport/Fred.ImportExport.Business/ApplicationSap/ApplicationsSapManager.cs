using Fred.Business.Organisation;
using Fred.Business.Organisation.Tree;
using Fred.Entities;

namespace Fred.ImportExport.Business.ApplicationSap
{
    /// <summary>
    /// Manager permettant de determiner les infos de connexion pour SAP (Url,login,password).
    /// Le manager determine ces info en fonction de la societe ou du groupe de la societe.
    /// </summary>
    public class ApplicationsSapManager : IApplicationsSapManager
    {
        private readonly GeneratedApplicationSapParameterHelper generatorHelper;
        private readonly ApplicationSapFredConfigHelper applicationSapFredConfigHelper;

        public ApplicationsSapManager(IOrganisationManager organisationManager, IOrganisationTreeService organisationTreeService)
        {
            generatorHelper = new GeneratedApplicationSapParameterHelper(organisationManager, organisationTreeService);
            applicationSapFredConfigHelper = new ApplicationSapFredConfigHelper();
        }

        /// <summary>
        /// Retourne un ApplicationSapParameter pour une societe donnée
        /// Si il n'y a pas de conf pour une societe donnée la propriete IsFound = false
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns> un ApplicationSapParameter</returns>
        public ApplicationSapParameter GetParametersForSociete(int societeId)
        {
            // ici je genere toutes les clé possible a partir des données de l'application
            // et je retourne celle qui correspond a ma societe
            var generatedParameter = generatorHelper.GetGeneratedApplicationSapParameter(societeId);

            //si j'ai une conf pour la societe je la retourne en premier
            var existingParameterForSociete = applicationSapFredConfigHelper.GetApplicationSapParameterOnWebConfig(OrganisationType.Societe, generatedParameter.SocieteCode);
            if (existingParameterForSociete.IsFound)
            {
                return existingParameterForSociete;
            }
            //sinon je retourne celle du groupe
            var existingParameterForGroupe = applicationSapFredConfigHelper.GetApplicationSapParameterOnWebConfig(OrganisationType.Groupe, generatedParameter.GroupeCode);

            //il ce peux ici que la conf ne soit pas trouvée IsFound = false
            return existingParameterForGroupe;
        }

    }
}
