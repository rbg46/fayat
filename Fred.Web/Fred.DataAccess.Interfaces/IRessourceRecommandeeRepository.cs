using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Entities.RessourcesRecommandees;
using Fred.Web.Shared.Models.RessourceRecommandee;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// IRessourceRecommandeeRepository
    /// </summary>
    public interface IRessourceRecommandeeRepository : IRepository<RessourceRecommandeeEnt>
    {
        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>RessourceRecommandeeEnt</returns>
        RessourceRecommandeeEnt Add(RessourceRecommandeeEnt entity);

        /// <summary>
        /// Adds all.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>List of RessourceRecommandeeEnt</returns>
        List<RessourceRecommandeeEnt> AddAll(List<RessourceRecommandeeEnt> items);

        /// <summary>
        /// Finds the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>RessourceRecommandeeEnt</returns>
        RessourceRecommandeeEnt Find(Expression<Func<RessourceRecommandeeEnt, bool>> expression);

        /// <summary>
        /// Includes the specified include expressions.
        /// </summary>
        /// <param name="includeExpressions">The include expressions.</param>
        /// <returns>IQueryable of RessourceRecommandeeEnt</returns>
        IQueryable<RessourceRecommandeeEnt> Include(params Expression<Func<RessourceRecommandeeEnt, object>>[] includeExpressions);

        /// <summary>
        /// Wheres the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>IQueryable of RessourceRecommandeeEnt</returns>
        IQueryable<RessourceRecommandeeEnt> Where(Expression<Func<RessourceRecommandeeEnt, bool>> predicate);

        /// <summary>
        /// Anies the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>Bool</returns>
        bool Any(Expression<Func<RessourceRecommandeeEnt, bool>> expression);

        /// <summary>
        /// Deletes all.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns>List of RessourceRecommandeeEnt</returns>
        List<RessourceRecommandeeEnt> DeleteAll(List<RessourceRecommandeeEnt> entities);

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns>List of RessourceRecommandeeEnt</returns>
        List<RessourceRecommandeeEnt> FindAll();

        /// <summary>
        /// Adds the or update.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns>List Of RessourceRecommandee</returns>
        List<RessourceRecommandeeEnt> AddOrUpdate(List<RessourceRecommandeeEnt> entities);

        /// <summary>
        /// Récupère la liste des ressources recommandées correspondant aux référentiels étendus
        /// </summary>
        /// <param name="etablissementCIOrganisationId">Identifiant de l'organisation à laquelle l'établissement comptable du CI courant appartient</param>
        /// <returns>Une liste de ressources recommandées</returns>
        List<RessourceRecommandeeFromEtablissementCIOrganisationModel> GetRessourceRecommandeeList(int etablissementCIOrganisationId);

    }
}
