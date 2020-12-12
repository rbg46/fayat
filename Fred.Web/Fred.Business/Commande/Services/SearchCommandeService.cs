using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Business.OrganisationFeature;
using Fred.Business.Personnel;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.Entities.Personnel;
using Fred.Entities.Utilisateur;
using Fred.Framework.Extensions;

namespace Fred.Business.Commande
{
    /// <summary>
    /// Services recherche pour les commandes
    /// </summary>
    public class SearchCommandeService : ISearchCommandeService
    {
        private readonly IUtilisateurManager utilisateurManager;
        private readonly ICommandeRepository commandeRepository;
        private readonly IPersonnelRepository personnelRepository;
        private readonly ICommandeTypeManager commandeTypeManager;
        private readonly IOrganisationRelatedFeatureService organisationRelatedFeatureService;
        private readonly UtilisateurEnt user;


        public SearchCommandeService(IUtilisateurManager utilisateurManager,
            ICommandeRepository commandeRepository,
            IPersonnelRepository personnelRepository,
            ICommandeTypeManager commandeTypeManager,
            IOrganisationRelatedFeatureService organisationRelatedFeatureService)
        {
            this.utilisateurManager = utilisateurManager;
            this.commandeRepository = commandeRepository;
            this.personnelRepository = personnelRepository;
            this.commandeTypeManager = commandeTypeManager;
            this.organisationRelatedFeatureService = organisationRelatedFeatureService;
            user = GetCurrentUser();
        }

        /// <summary>
        ///   Retourner la requête de récupération des commandes
        /// </summary>
        /// <param name="filters">Les filtres.</param>
        /// <param name="orderBy">Les tris</param>
        /// <param name="includeProperties">Les propriétés à inclure.</param>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">Taille de la page.</param>
        /// <param name="asNoTracking">asNoTracking</param>
        /// <returns>Une requête</returns>
        public IEnumerable<CommandeEnt> Search(List<Expression<Func<CommandeEnt, bool>>> filters,
                                              Func<IQueryable<CommandeEnt>, IOrderedQueryable<CommandeEnt>> orderBy = null,
                                              List<Expression<Func<CommandeEnt, object>>> includeProperties = null,
                                              int? page = null,
                                              int? pageSize = null,
                                              bool asNoTracking = false)
        {
            return commandeRepository.Search(filters, orderBy, includeProperties, page, pageSize, asNoTracking);
        }

        /// <summary>
        /// renvoie la liste des Auteurs suivant le filtre
        /// </summary>
        /// <param name="search">model de recherches</param>
        /// <returns>renvoie la liste des Auteurs</returns>
        public List<PersonnelEnt> SearchCommandeAuthors(SearchLightPersonnelModel search)
        {
            List<int> userCIsIds = utilisateurManager.GetAllCIbyUser(user.UtilisateurId).ToList();

            int? groupeId = user.Personnel?.Societe?.GroupeId;
            List<PersonnelEnt> perso = commandeRepository.Query()
                           .Filter(c => !c.DateSuppression.HasValue && userCIsIds.Contains(c.CiId.Value))
                           .Filter(GetFilterByAuthorType(search.AuthorType))
                           .Get().Select(GetSelectByAuthorType(search.AuthorType))
                           .Where(p => !groupeId.HasValue || p.Societe.GroupeId == groupeId)
                           .Distinct().Where(search.GetImprovedSearchedTextPredicat()).ToList();

            List<int> persoids = perso.ConvertAll(x => x.PersonnelId);

            //inculre la socite (impossible de le faire directement depuis commandeRepository)
            return personnelRepository.Query().Include(s => s.Societe)
                .Filter(x => persoids.Contains(x.PersonnelId))
                .OrderBy(list => list.OrderBy(pe => pe.Societe.Code).ThenBy(pe => pe.Matricule))
                .GetPage(search.Page, search.PageSize).ToList();
        }

        public IEnumerable<CommandeEnt> SearchCommandes(string text, int? page, int? pageSize)
        {
            var filter = new SearchCommandeEnt { ValueText = text };
            return SearchCommandeListWithFilter(filter, page, pageSize).Commandes;
        }

        public SearchCommandeListWithFilterResult SearchCommandeListWithFilter(SearchCommandeEnt filter, int? page, int? pageSize)
        {
            var result = new SearchCommandeListWithFilterResult();
            int totalCount = 0;

            filter.TypeCodes = new List<string> { CommandeTypeEnt.CommandeTypeF, CommandeTypeEnt.CommandeTypeL, CommandeTypeEnt.CommandeTypeP };
            filter.CiIds = utilisateurManager.GetAllCIbyUser(user.UtilisateurId).ToList();

            RemoveTimeFromDateForFiltering(filter);

            result.Commandes = commandeRepository.GetList(filter, out totalCount, page, pageSize).ComputeAll().ToList();
            result.TotalCount = totalCount;

            return result;
        }

        /// <summary>
        /// Retire la partie heure des dates du filtre, pour le filtrage par date
        /// </summary>
        /// <param name="filter">le filtre</param>
        private void RemoveTimeFromDateForFiltering(SearchCommandeEnt filter)
        {
            if (filter.DateFrom.HasValue)
            {
                filter.DateFrom = filter.DateFrom.Value.ToStartDate();
            }
            if (filter.DateTo.HasValue)
            {
                filter.DateTo = filter.DateTo.Value.ToEndDate();
            }
        }

        public SearchReceptionnableResult SearchReceivableOrders(SearchReceivableOrdersFilter filter, int page = 1, int pageSize = 20)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            bool featureLockUnlockCommandeLigneIsEnabled = organisationRelatedFeatureService.IsEnabledForCurrentUser(CommandeLigneLockingService.FeatureKey, false);

            filter.CurrentUserHasFeatureLockUnLockCommandeLigne = featureLockUnlockCommandeLigneIsEnabled;

            filter.CiIds = utilisateurManager.GetAllCIbyUser(user.UtilisateurId).ToList();

            (List<CommandeEnt> orders, int total) = this.commandeRepository.SearchReceivableOrders(filter, page, pageSize);

            orders.ComputeAll();

            return new SearchReceptionnableResult
            {
                Commandes = orders,
                TotalCount = total
            };
        }

        private UtilisateurEnt GetCurrentUser()
        {
            return utilisateurManager.GetContextUtilisateur();
        }

        /// <summary>
        /// Renvoi une expression de recherche lamda
        /// </summary>
        /// <param name="authorType">Type Auteur </param>
        /// <returns>Filtre les commande suivants le type auteur</returns>
        private Expression<Func<CommandeEnt, bool>> GetFilterByAuthorType(string authorType)
        {
            switch (authorType)
            {
                case "AuteurCreation":
                    return s => s.AuteurCreation != null;
                case "Valideur":
                    return s => s.Valideur != null;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Renvoi une Expression de selection suivant le type autor
        /// </summary>
        /// <param name="authorType">Type d'auteur recherché</param>
        /// <returns>Expression</returns>
        private Expression<Func<CommandeEnt, PersonnelEnt>> GetSelectByAuthorType(string authorType)
        {
            switch (authorType)
            {
                case "AuteurCreation":
                    return s => s.AuteurCreation.Personnel;
                case "Valideur":
                    return s => s.Valideur.Personnel;
                default:
                    return null;
            }
        }


    }
}
