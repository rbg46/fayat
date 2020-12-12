using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Organisation;
using Fred.Business.Organisation.Tree;
using Fred.Entities;

namespace Fred.ImportExport.Business.ApplicationSap
{
    /// <summary>
    /// Cette classe genere toutes les conf possibles a partie des données de la bases.
    /// </summary>
    public class GeneratedApplicationSapParameterHelper
    {
        private readonly IOrganisationManager organisationManager;
        private List<GeneratedApplicationSapParameter> generateApplicationSapParameters;
        private readonly IOrganisationTreeService organisationTreeService;

        public GeneratedApplicationSapParameterHelper(IOrganisationManager organisationManager,
            IOrganisationTreeService organisationTreeService)
        {
            if (organisationTreeService == null)
            {
                throw new ArgumentNullException(nameof(organisationTreeService));
            }

            if (organisationManager == null)
            {
                throw new ArgumentNullException(nameof(organisationManager));
            }

            this.organisationManager = organisationManager;
            this.organisationTreeService = organisationTreeService;
        }

        /// <summary>
        ///  Ici je genere toutes les clés possibles a partir des données de l'application
        ///  et je retourne celle qui correspond a ma societe
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>GeneratedApplicationSapParameter</returns>
        public GeneratedApplicationSapParameter GetGeneratedApplicationSapParameter(int societeId)
        {
            if (generateApplicationSapParameters == null)
            {
                generateApplicationSapParameters = CreateAllGeneratedApplicationSapParameters();
            }

            var result = this.generateApplicationSapParameters.FirstOrDefault(gasp => gasp.SocieteId == societeId);
            return result;
        }

        /// <summary>
        /// Generation de toutes les conf possibles
        /// </summary>
        /// <returns>List<GeneratedApplicationSapParameter></returns>
        private List<GeneratedApplicationSapParameter> CreateAllGeneratedApplicationSapParameters()
        {
            var result = new List<GeneratedApplicationSapParameter>();

            int typeGroupe = this.organisationManager.GetTypeOrganisationIdByCode(Constantes.OrganisationType.CodeGroupe);
            int typeSociete = this.organisationManager.GetTypeOrganisationIdByCode(Constantes.OrganisationType.CodeSociete);

            var groupeOrganisations = this.organisationManager.GetOrganisationsAvailable(string.Empty, new List<int> { typeGroupe }, null, null).ToList();

            foreach (var groupeOrganisation in groupeOrganisations)
            {
                var societeOrganisations = this.organisationManager.GetOrganisationsAvailable(string.Empty, new List<int> { typeSociete }, null, groupeOrganisation.OrganisationId).ToList();

                foreach (var societeOrganisation in societeOrganisations)
                {
                    result.Add(new GeneratedApplicationSapParameter(groupeOrganisation.OrganisationId, groupeOrganisation.Code, societeOrganisation.OrganisationId, societeOrganisation.Code));
                }
            }

            var organisationTree = this.organisationTreeService.GetOrganisationTree();

            var societes = organisationTree.GetAllSocietes();

            foreach (var generateApplicationSapParameter in result)
            {
                var societe = societes.Find(o => o.OrganisationId == generateApplicationSapParameter.SocieteOrganisationId);
                if (societe != null)
                {
                    generateApplicationSapParameter.SocieteId = societe.Id;
                }
            }
            return result;
        }


    }
}
