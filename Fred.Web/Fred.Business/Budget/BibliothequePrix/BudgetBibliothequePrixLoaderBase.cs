using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Entities.Referential;

namespace Fred.Business.Budget.BibliothequePrix
{
    // NPI : certaines fonctions ici devraient se trouver dans leur repository respectifs.

    /// <summary>
    /// Permet de charger une bibliothèque des prix.
    /// </summary>
    public abstract class BudgetBibliothequePrixLoaderBase
    {
        private readonly IUniteRepository uniteRepository;
        private readonly IUniteSocieteRepository uniteSocieteRepository;
        private readonly IBudgetBibliothequePrixRepository budgetBibliothequePrixRepository;

        protected BudgetBibliothequePrixLoaderBase(
            IUniteRepository uniteRepository,
            IUniteSocieteRepository uniteSocieteRepository,
            IBudgetBibliothequePrixRepository budgetBibliothequePrixRepository)
        {
            this.uniteRepository = uniteRepository;
            this.uniteSocieteRepository = uniteSocieteRepository;
            this.budgetBibliothequePrixRepository = budgetBibliothequePrixRepository;
        }

        /// <summary>
        /// Retourne une unité en fonction de son identifiant.
        /// </summary>
        /// <typeparam name="TUnite">Le type de l'unité souhaitée.</typeparam>
        /// <param name="uniteId">L'identifiant de l'unité.</param>
        /// <param name="selector">Selector permettant de construire un TUnite a partir d'une unité.</param>
        /// <returns>L'unité ou null si elle n'existe pas.</returns>
        protected TUnite GetUnite<TUnite>(int uniteId, Expression<Func<UniteEnt, TUnite>> selector)
        {
            return uniteRepository.Get()
                .Where(u => u.UniteId == uniteId)
                .Select(selector)
                .FirstOrDefault();
        }

        /// <summary>
        /// Retourne les unités d'une société.
        /// </summary>
        /// <typeparam name="TUnite">Le type des unités souhaitées.</typeparam>
        /// <param name="societeId">L'identifiant de la société.</param>
        /// <param name="selector">Selector permettant de construire un TUnite a partir d'une unité.</param>
        /// <returns>Les unités de la société.</returns>
        protected List<TUnite> GetUnites<TUnite>(int societeId, Expression<Func<UniteEnt, TUnite>> selector)
        {
            return uniteSocieteRepository.Get()
                .Where(us => us.SocieteId == societeId)
                .Select(us => us.Unite)
                .Select(selector)
                .ToList();
        }

        /// <summary>
        /// Indique si une bibliothèque des prix existe.
        /// </summary>
        /// <param name="organisationId">L'identifiant de l'organisation.</param>
        /// <param name="deviseId">L'identifiant de la devise.</param>
        /// <returns>True si la bibliothèque des prix existe, sinon false.</returns>
        protected bool Exists(int organisationId, int deviseId)
        {
            return budgetBibliothequePrixRepository.Get()
                .Where(bp => bp.OrganisationId == organisationId && bp.DeviseId == deviseId)
                .Count() == 1;
        }

        /// <summary>
        /// Retourne les items d'une bibliothèque des prix.
        /// </summary>
        /// <typeparam name="TItem">Le type des items souhaités.</typeparam>
        /// <param name="organisationId">L'identifiant de l'organisation.</param>
        /// <param name="deviseId">L'identifiant de la devise.</param>
        /// <param name="selector">Selector permettant de constuire un TItem a partir d'un élément d'une bibliothèque des prix.</param>
        /// <returns>Les items de la bibliothèque des prix.</returns>
        protected List<TItem> GetItems<TItem>(int organisationId, int deviseId, Expression<Func<BudgetBibliothequePrixItemEnt, TItem>> selector)
        {
            return budgetBibliothequePrixRepository.Get()
                .Where(bp => bp.OrganisationId == organisationId && bp.DeviseId == deviseId)
                .SelectMany(bp => bp.Items.AsQueryable().Select(selector))
                .ToList();
        }
    }
}
