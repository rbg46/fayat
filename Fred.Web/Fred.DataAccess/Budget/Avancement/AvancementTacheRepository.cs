using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget.Avancement;
using Fred.Framework;
using System.Collections.Generic;
using System.Linq;
using Fred.EntityFramework;

namespace Fred.DataAccess.Budget.Avancement
{
    /// <summary>
    /// Implémentation de l'interface IAvancementRepository permettant de manipuler des taches d'avancement
    /// </summary>
    public class AvancementTacheRepository : FredRepository<AvancementTacheEnt>, IAvancementTacheRepository
    {
        /// <summary>
        /// Constructeur auto généré
        /// </summary>
        /// <param name="logMgr">Log manager</param>
        public AvancementTacheRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        ///   Obtient la liste des taches d'avancement par budget et periode
        /// </summary>
        /// <param name="budgetId">ientifiant de budget</param>
        /// <param name="periode">période numérique</param>
        /// <returns>Liste des taches d'avancement</returns>
        public IEnumerable<AvancementTacheEnt> GetAvancementTaches(int budgetId, int periode)
        {
            return Context.AvancementTaches.Where(t => t.BudgetId == budgetId && t.Periode == periode).ToList();
        }

        /// <summary>
        /// Met a jour une liste de taches d'avancement pour un budget/periode
        /// </summary>
        /// <param name="budgetId">ientifiant de budget</param>
        /// <param name="periode">période numérique</param>>
        /// <param name="avancementTacheEnts">Liste des taches d'avancement</param>
        public void UpdateListe(int budgetId, int periode, IEnumerable<AvancementTacheEnt> avancementTacheEnts)
        {
            var existingAvancementTaches = Context.AvancementTaches.Where(x => x.BudgetId == budgetId && x.Periode == periode).ToList();

            var newTaches = avancementTacheEnts.Where(x => !existingAvancementTaches.Any(t => t.TacheId == x.TacheId));
            var deletedTaches = existingAvancementTaches.Where(x => !avancementTacheEnts.Any(t => t.TacheId == x.TacheId));
            var updatedTaches = existingAvancementTaches.Where(x => avancementTacheEnts.Any(t => t.TacheId == x.TacheId));
            this.Context.AvancementTaches.AddRange(newTaches);
            this.Context.AvancementTaches.RemoveRange(deletedTaches);
            foreach (var updatedTache in updatedTaches)
            {
                updatedTache.Commentaire = avancementTacheEnts.SingleOrDefault(x => x.TacheId == updatedTache.TacheId).Commentaire;
            }
        }
    }
}
