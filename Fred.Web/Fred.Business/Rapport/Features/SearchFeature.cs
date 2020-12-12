using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fred.Business.CI;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Rapport;
using Fred.Entities.Rapport.Search;
using Fred.Entities.Utilisateur;

namespace Fred.Business.Rapport
{
    public class SearchFeature : ManagerFeature<IRapportRepository>, ISearchFeature
    {
        private IUtilitiesFeature Utilities { get; }

        private readonly IUtilisateurManager utilisateurManager;
        private readonly ICIManager ciManager;

        public SearchFeature(
            IUnitOfWork uow,
            IRapportRepository repository,
            IUtilitiesFeature utilities,
            IUtilisateurManager utilisateurManager,
            ICIManager ciManager)
            : base(uow, repository)
        {
            this.utilisateurManager = utilisateurManager;
            this.ciManager = ciManager;

            Utilities = utilities;
        }

        /// <summary>
        ///  Récupère la liste des rapports correspondant aux critères de recherche
        /// </summary>
        /// <param name="filter">critère de recherche</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>IEnumerable contenant la liste des rapports correspondants aux critères</returns>
        public virtual SearchRapportListWithFilterResult SearchRapportWithFilter(SearchRapportEnt filter, int? page = 1, int? pageSize = 20)
        {
            SearchRapportListWithFilterResult result = new SearchRapportListWithFilterResult();
            int totalCount;
            var currentUserId = utilisateurManager.GetContextUtilisateurId();
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
            }
            else
            {
                filter.Cis = userCiIdList;
            }

            rapports = Repository.SearchRapportWithFilter(filter.GetPredicateWhere(), filter.EtablissementPaieIdList, GetSortFilter(filter).ToString(), out totalCount, page, pageSize).ToList();

            rapports?.ForEach(rapport => Utilities.SetStatut(rapport));
            result.TotalCount = totalCount;
            result.Rapports = rapports;

            return result;
        }

        /// <summary>
        /// Indique si l'utilisateur courant peux supprimer un rapport
        /// </summary>
        /// <param name="rapport">Rapport à supprimer</param>
        /// <returns>true si le rapport peux être supprimer</returns>
        public bool RapportCanBeDeleted(RapportEnt rapport)
        {
            UtilisateurEnt userConnected = this.utilisateurManager.GetById(utilisateurManager.GetContextUtilisateurId());
            return Utilities.GetCanBeDeleted(rapport, userConnected);
        }


        /// <summary>
        /// Récupère la liste des filtre pour la recherche des rapports
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        /// <returns>Retourne un objet représentant les filtres</returns>
        public SearchRapportEnt GetFiltersList(int utilisateurId)
        {
            SearchRapportEnt recherche = new SearchRapportEnt();

            recherche.DemandeurId = utilisateurId;
            DateTime today = DateTime.UtcNow;
            recherche.DateChantierMin = new DateTime(today.Year, today.Month, 1).AddMonths(-1);
            recherche.Organisation = null;
            recherche.DateComptable = null;
            recherche.CiCodeAsc = true;
            recherche.DateChantierAsc = false;
            recherche.NumeroRapportAsc = false;
            recherche.SortFields = new Dictionary<string, bool?>();
            recherche.SortFields.Add("CI.Code", true);
            recherche.SortFields.Add("RapportStatut.Libelle", null);
            recherche.SortFields.Add("RapportId", true);
            recherche.SortFields.Add("DateChantier", false);
            recherche.EtablissementPaieIdList = new List<int?>();

            recherche.IsGSP = false;

            if (utilisateurManager.HasAtLeastThisPaieLevel(Constantes.NiveauPaie.LevelDRC))
            {
                recherche.StatutValide2 = true;
            }
            else if (utilisateurManager.HasAtLeastThisPaieLevel(Constantes.NiveauPaie.LevelCDT))
            {
                recherche.StatutValide1 = true;
            }
            else if (utilisateurManager.HasAtLeastThisPaieLevel(Constantes.NiveauPaie.LevelCDC))
            {
                recherche.StatutEnCours = true;
            }

            return recherche;
        }


        protected StringBuilder GetSortFilter(SearchRapportEnt searchRapport)
        {
            var sortFilter = new StringBuilder();
            const string sortedFieldTemplate = "{0} {1}";

            if (!searchRapport.SortFields["DateChantier"].HasValue)
            {
                sortFilter.Append("DateChantier,");
            }
            else
            {
                sortFilter.Append(string.Format(sortedFieldTemplate, "DateChantier", searchRapport.SortFields["DateChantier"].Value ? "ascending," : "descending,"));
            }
            if (!searchRapport.SortFields["RapportId"].HasValue)
            {
                sortFilter.Append("RapportId,");
            }
            else
            {
                sortFilter.Append(string.Format(sortedFieldTemplate, "RapportId", searchRapport.SortFields["RapportId"].Value ? "ascending," : "descending,"));
            }

            if (!searchRapport.SortFields["CI.Code"].HasValue)
            {
                sortFilter.Append("CI.Code");
            }
            else
            {
                sortFilter.Append(string.Format(sortedFieldTemplate, "CI.Code", searchRapport.SortFields["CI.Code"].Value ? "ascending" : "descending"));
            }
            return sortFilter;
        }
    }
}

