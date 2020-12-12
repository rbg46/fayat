using Fred.Entities.Budget;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Fred.DataAccess.Budget
{

    /// <summary>
    /// Classe interne 
    /// </summary>
    internal static class BudgetRepoExtension
  {
    /// <summary>
    /// Ajoute à la requete passée en paramètre des appels à la fonction include pour les propriétés de navigations d'un objet budget
    /// </summary>
    /// <param name="query">Query a modifier</param>
    /// <returns>La query avec les includes</returns>
    public static IQueryable<BudgetEnt> WithIncludes(this IQueryable<BudgetEnt> query)
    {
        return query
            .Include(b => b.Ci)
            .Include(b => b.Devise)
            .Include(b => b.BudgetEtat)
            .Include(b => b.Workflows)
            .Include(b => b.Workflows).ThenInclude(w => w.EtatInitial)
            .Include(b => b.Workflows).ThenInclude(w => w.EtatCible)
            .Include(b => b.Workflows).ThenInclude(w => w.Auteur)
            .Include(b => b.Workflows).ThenInclude(w => w.Auteur.Personnel)
            .Include(b => b.BudgetT4s)
            .Include(b => b.BudgetT4s).ThenInclude(t4 => t4.BudgetSousDetails).ThenInclude(sd => sd.Ressource.SousChapitre.Chapitre)
            .Include(b => b.BudgetT4s).ThenInclude(t4 => t4.T4.Parent.Parent.Parent)
            .Include(b => b.Recette);
        }

    /// <summary>
    /// Ajoute à la requete passée en paramètre des appels à la fonction include pour les propriétés de navigations d'un objet budget
    /// </summary>
    /// <param name="query">Query a modifier</param>
    /// <returns>La query avec les includes</returns>
    public static IQueryable<BudgetEnt> AvecT4etRessource(this IQueryable<BudgetEnt> query)
    {
        return query
            .Include(b => b.Ci)
            .Include(b => b.Devise)
            .Include(b => b.BudgetEtat)
            .Include(b => b.Recette)
            .Include(b => b.BudgetT4s).ThenInclude(w => w.T4)
            .Include(b => b.BudgetT4s).ThenInclude(t4 => t4.BudgetSousDetails).ThenInclude(w => w.Ressource);
        }
  }
}
