using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Budget;
using Fred.EntityFramework;

namespace Fred.DataAccess.Budget.BibliothequePrix
{
    /// <summary>
    /// Référentiel de données pour les éléments des bibliothèques des prix.
    /// </summary>
    public class BudgetBibliothequePrixItemRepository : FredRepository<BudgetBibliothequePrixItemEnt>, IBudgetBibliothequePrixItemRepository
    {
        private readonly IOrganisationRepository organisationRepository;
        private readonly ICIRepository ciRepository;

        public BudgetBibliothequePrixItemRepository(FredDbContext context, IOrganisationRepository organisationRepository, ICIRepository ciRepository)
          : base(context)
        {
            this.organisationRepository = organisationRepository;
            this.ciRepository = ciRepository;
        }

        /// <inheritdoc/>
        public List<TBibliothequePrix> GetAllBibliothequePrixItemForCi<TBibliothequePrix>(int ciId, int deviseId, Expression<Func<BudgetBibliothequePrixItemEnt, TBibliothequePrix>> selector)
        {
            var ciOrganisationId = ciRepository.GetOrganisationIdByCiId(ciId).Value;
            return GetAllBibliothequePrixItemForOrgaCi(ciOrganisationId, deviseId, selector);
        }

        /// <inheritdoc/>
        public List<TBibliothequePrix> GetAllBibliothequePrixItemForOrgaCi<TBibliothequePrix>(int ciOrganisationId,
            int deviseId, Expression<Func<BudgetBibliothequePrixItemEnt, TBibliothequePrix>> selector)
        {
            var organisationIdEtablissementCi =
                organisationRepository.GetParentId(ciOrganisationId, OrganisationType.Etablissement);

            //D'abord on récupère tous les items valable sur le CI
            var itemsForCi = Context.BibliothequePrixItem
                .Where(i =>
                    i.BudgetBibliothequePrix.OrganisationId == ciOrganisationId &&
                    i.BudgetBibliothequePrix.DeviseId == deviseId)
                .Select(i => new
                {
                    i.RessourceId,
                    i.BudgetBibliothequePrixItemId
                })
                .ToList();

            //Puis on récupère tous les items valable sur l'établissement MAIS qui ne sont pas sur une ressource déjà géré par le CI
            var itemsIdForEtablissement = Context.BibliothequePrixItem
                .Where(i =>
                    i.BudgetBibliothequePrix.OrganisationId == organisationIdEtablissementCi &&
                    i.BudgetBibliothequePrix.DeviseId == deviseId &&
                    !itemsForCi.Any(itemForCi => itemForCi.RessourceId == i.RessourceId))
                .Select(i => i.BudgetBibliothequePrixItemId)
                .ToList();

            var allItemsId =
                itemsIdForEtablissement.Concat(itemsForCi.Select(itemForCi => itemForCi.BudgetBibliothequePrixItemId));

            return Context.BibliothequePrixItem
                .Where(i => allItemsId.Any(id => i.BudgetBibliothequePrixItemId == id))
                .Select(selector)
                .ToList();
        }
    }
}
