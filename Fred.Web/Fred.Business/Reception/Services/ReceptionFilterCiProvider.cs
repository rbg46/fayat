using System.Collections.Generic;
using System.Linq;
using Fred.Business.CI;
using Fred.Business.Utilisateur;
using Fred.Entities.Depense;
using Fred.Entities.Utilisateur;

namespace Fred.Business.Reception.Services
{
    public class ReceptionFilterCiProvider : IReceptionFilterCiProvider
    {
        private readonly IUtilisateurManager utilisateurManager;
        private readonly ICIManager ciManager;

        public ReceptionFilterCiProvider(
            IUtilisateurManager utilisateurManager,
            ICIManager ciManager)
        {
            this.utilisateurManager = utilisateurManager;
            this.ciManager = ciManager;
        }

        /// <summary>
        /// Initialise les cis dans le filtre
        /// </summary>
        /// <param name="filter">Le filtre</param>
        /// <param name="byPassCurrentUser">byPassCurrentUser</param>
        public void InitializeCisOnFilter(SearchDepenseEnt filter, bool byPassCurrentUser)
        {
            UtilisateurEnt utilisateur = utilisateurManager.GetContextUtilisateur();
            if (!byPassCurrentUser && utilisateur != null)
            {
                List<int> userCiIdList = utilisateurManager.GetAllCIbyUser(utilisateur.UtilisateurId).ToList();
                if (filter.OrganisationId.HasValue)
                {
                    // Liste des CI de l'organisation parent (ETABLISSEMENT)
                    List<int> ciIdListByOrgaParent = this.ciManager.GetCIList(filter.OrganisationId.Value).Select(x => x.CiId).ToList();
                    filter.Cis = (filter.OrganisationId != 0) ? userCiIdList.Where(x => ciIdListByOrgaParent.Contains(x)).ToList() : userCiIdList;
                }
                else
                {
                    filter.Cis = userCiIdList;
                }
            }
            else
            {
                if (filter.OrganisationId.HasValue && filter.Cis.Count == 0)
                {
                    filter.Cis = this.ciManager.GetCIList(filter.OrganisationId.Value).Select(x => x.CiId).ToList();
                }
            }
        }

    }
}
