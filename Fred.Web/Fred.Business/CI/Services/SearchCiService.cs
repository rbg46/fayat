using System.Collections.Generic;
using System.Linq;
using Fred.Business.CI.Services.Interfaces;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.CI;
using Fred.Entities.Organisation.Tree;

namespace Fred.Business.CI.Services
{
    /// <summary>
    /// Service de recherche des cis
    /// </summary>
    public class SearchCiService : ISearchCiService
    {
        private readonly ICisAccessiblesService cisAccessiblesService;
        private readonly ICIRepository ciRepository;
        private readonly ISocieteManager societeManager;
        private readonly IUtilisateurManager utilisateurManager;

        /// <summary>
        /// ctor
        /// </summary>      
        /// <param name="ciRepository">ciRepository</param>
        /// <param name="societeManager">societeManager</param>       
        /// <param name="cisAccessiblesService">cisAccessiblesService</param>
        /// <param name="utilisateurManager">utilisateurManager</param>
        public SearchCiService(ICisAccessiblesService cisAccessiblesService,
            ICIRepository ciRepository,
            ISocieteManager societeManager,
            IUtilisateurManager utilisateurManager)
        {
            this.cisAccessiblesService = cisAccessiblesService;
            this.ciRepository = ciRepository;
            this.societeManager = societeManager;
            this.utilisateurManager = utilisateurManager;
        }

        /// <summary>
        /// Recupere les cis pour la vue 
        /// </summary>
        /// <param name="filters">Le filtre </param>
        /// <param name="page">La page </param>
        /// <param name="pageSize">La taille de la page</param>
        /// <returns>Liste de cis</returns>
        public List<CIEnt> SearchForCiListView(SearchCIEnt filters, int page, int pageSize)
        {
            var currentUser = utilisateurManager.GetContextUtilisateur();

            List<OrganisationBase> cisVisiblesForUserAndPermission = cisAccessiblesService.GetCisAccessiblesForUserAndPermission(currentUser.UtilisateurId, filters.PermissionKey);

            List<int> ciIds = cisVisiblesForUserAndPermission.Select(c => c.Id).ToList();

            List<int?> typeSocieteIdList = societeManager.GetTypeSocieteId(filters.IsSEP);

            List<CIEnt> result = ciRepository.GetForCiListView(filters, ciIds, typeSocieteIdList, page, pageSize);

            return result;
        }


    }
}
