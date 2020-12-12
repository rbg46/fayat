using System.Collections.Generic;
using System.Linq;
using Fred.Business.CI;
using Fred.Business.Rapport;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Rapport;
using Fred.Entities.Rapport.Search;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;

namespace Fred.Business.Referential.Materiel
{
    public class SearchFeatureFes : SearchFeature
    {
        private readonly IUtilitiesFeature utilities;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly ICIManager ciManager;
        private readonly IEtablissementPaieManager etablissementPaieManager;

        public SearchFeatureFes(
            IUnitOfWork uow,
            IRapportRepository repository,
            IUtilitiesFeature utilities,
            IUtilisateurManager utilisateurManager,
            ICIManager ciManager,
            IEtablissementPaieManager etablissementPaieManager)
            : base(uow,
                   repository,
                   utilities,
                   utilisateurManager,
                   ciManager)
        {
            this.utilities = utilities;
            this.utilisateurManager = utilisateurManager;
            this.ciManager = ciManager;
            this.etablissementPaieManager = etablissementPaieManager;
        }

        public override SearchRapportListWithFilterResult SearchRapportWithFilter(SearchRapportEnt filter, int? page = 1, int? pageSize = 20)
        {
            SearchRapportListWithFilterResult result = new SearchRapportListWithFilterResult();
            int totalCount;
            UtilisateurEnt currentUser = utilisateurManager.GetContextUtilisateur();
            int currentUserId = currentUser.UtilisateurId;
            List<RapportEnt> rapports = null;
            filter.DemandeurId = currentUserId;
            filter.DateChantierMin = filter.DateChantierMin.HasValue ? filter.DateChantierMin.Value.Date : filter.DateChantierMin;
            filter.DateChantierMax = filter.DateChantierMax.HasValue ? filter.DateChantierMax.Value.Date : filter.DateChantierMax;


            // Liste des CI associés à l'utilisateur courant
            List<int> userCiIdList = utilisateurManager.GetAllCIbyUser(currentUserId).ToList();

            if (filter.OrganisationId.HasValue)
            {
                // Liste des CI de l'organisation parent (SOCIETE, PUO, UO, ETABLISSEMENT)
                List<int> ciIdListByOrgaParent = this.ciManager.GetCIList(filter.OrganisationId.Value).Select(x => x.CiId).ToList();
                filter.Cis = (filter.OrganisationId != 0) ? userCiIdList.Where(x => ciIdListByOrgaParent.Contains(x)).ToList() : userCiIdList;

                // Récupéré les établissements paie de l'organisation séléctionnée que pour FES
                if (currentUser.Personnel.Societe.Groupe.Code == Constantes.CodeGroupeFES &&
                    (filter.EtablissementPaieIdList == null || !filter.EtablissementPaieIdList.Any()))
                {
                    IEnumerable<EtablissementPaieEnt> etablissementPaieList = etablissementPaieManager.GetEtablissementPaieByOrganisationId(filter.OrganisationId.Value);
                    if (etablissementPaieList != null && etablissementPaieList.Any())
                    {
                        if (filter.EtablissementPaieIdList == null)
                        {
                            filter.EtablissementPaieIdList = new List<int?>();
                        }
                        filter.EtablissementPaieIdList.AddRange(etablissementPaieList.Select(x => (int?)x.EtablissementPaieId).Distinct());
                    }
                }
            }
            else
            {
                filter.Cis = userCiIdList;
            }

            rapports = Repository.SearchRapportWithFilter(filter.GetPredicateWhere(), filter.EtablissementPaieIdList, base.GetSortFilter(filter).ToString(), out totalCount, page, pageSize).ToList();

            rapports?.ForEach(rapport => utilities.SetStatut(rapport));
            result.TotalCount = totalCount;
            result.Rapports = rapports;

            return result;
        }
    }
}
