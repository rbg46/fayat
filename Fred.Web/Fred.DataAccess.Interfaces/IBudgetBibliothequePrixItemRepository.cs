using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Entities.Budget;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Référentiel de données pour les éléments des bibliothèques des prix.
    /// </summary>
    public interface IBudgetBibliothequePrixItemRepository : IRepository<BudgetBibliothequePrixItemEnt>
    {
        /// <summary>
        /// Retourne tous les items de la bibliotheque des prix renseignés et valable pour ce CI.
        /// Une ressource ne peut être présente qu'une seule fois (Soit pour le CI, soit pour l'etablissement)
        /// </summary>
        /// <param name="ciId">Id du CI</param>
        /// <param name="deviseId">deviseId</param>
        /// <param name="selector">Selector permettant de faire la passerelle entre un bibliothequePrixItemEnt et le TResultModel</param>
        /// <typeparam name="TResultModel">Type de retour du model</typeparam>
        /// <returns>Une liste de TResultModel, jamais null potentiellement vide</returns>
        List<TBibliothequePrix> GetAllBibliothequePrixItemForCi<TBibliothequePrix>(int ciId, int deviseId, Expression<Func<BudgetBibliothequePrixItemEnt, TBibliothequePrix>> selector);

        /// <summary>
        /// Retourne tous les items de la bibliotheque des prix renseignés et valable pour l'organisation d'un CI.
        /// Une ressource ne peut être présente qu'une seule fois (soit pour le CI, soit pour l'etablissement)
        /// </summary>
        /// <typeparam name="TBibliothequePrix">Type de bibliothèque des prix souhaité</typeparam>
        /// <param name="ciOrganisationId">Identifiant de l'organisation du CI</param>
        /// <param name="deviseId">Identifiant de la devise</param>
        /// <param name="selector">Selector permettant de construire un TBibliothequePrix à partir d'un BibliothequePrixItemEnt</param>
        /// <returns>Une liste de TResultModel, jamais null potentiellement vide</returns>
        List<TBibliothequePrix> GetAllBibliothequePrixItemForOrgaCi<TBibliothequePrix>(int ciOrganisationId, int deviseId, Expression<Func<BudgetBibliothequePrixItemEnt, TBibliothequePrix>> selector);
    }
}
