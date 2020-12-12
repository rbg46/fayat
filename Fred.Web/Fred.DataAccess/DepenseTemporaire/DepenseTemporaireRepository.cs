using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Depense;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.DepenseTemporaire
{
    /// <summary>
    ///   Référentiel de données pour les dépenses temporaire temporaire.
    /// </summary>
    public class DepenseTemporaireRepository : FredRepository<DepenseTemporaireEnt>, IDepenseTemporaireRepository
    {
        private readonly ILogManager logManager;

        public DepenseTemporaireRepository(FredDbContext context, ILogManager logManager)
          : base(context)
        {
            this.logManager = logManager;
        }

        /// <summary>
        ///   Permet de détacher les entités dépendantes des dépenses temporaire pour éviter de les prendre en compte dans la
        ///   sauvegarde du contexte.
        /// </summary>
        /// <param name="depense">dépense dont les dépendances sont à détachées</param>
        private void AttachDependancies(DepenseTemporaireEnt depense)
        {
            depense.CommandeLigne = null;
            depense.Fournisseur = null;
            depense.Ressource = null;
            depense.Tache = null;
            depense.AuteurCreation = null;
            depense.CI = null;
            depense.Devise = null;
            depense.CommandeLigne = null;
        }

        /// <summary>
        ///   Requête de filtrage des depenses par défaut
        /// </summary>
        /// <returns>Retourne le prédicat de filtrage des depenses par défaut</returns>
        private IQueryable<DepenseTemporaireEnt> GetDefaultListQuery()
        {
            return Context.DepensesTemporaires.Include(d => d.CI)
                          .Include(d => d.CommandeLigne)
                          .Include(d => d.CommandeLigne.Commande)
                          .Include(d => d.Fournisseur)
                          .Include(d => d.Ressource)
                          .Include(d => d.Tache)
                          .Include(d => d.AuteurCreation)
                          .Include(d => d.AuteurCreation.Personnel)
                          .Include(d => d.Devise);
        }

        /// <summary>
        ///   Ajoute une nouvelle dépense.
        /// </summary>
        /// <param name="depense">dépense à ajouter</param>
        /// <returns> L'identifiant de la dépense ajoutée</returns>
        public int AddDepense(DepenseTemporaireEnt depense)
        {
            try
            {
                if (Context.Entry(depense).State == EntityState.Detached)
                {
                    AttachDependancies(depense);
                }

                Context.DepensesTemporaires.Add(depense);
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }

            return depense.DepenseId;
        }

        /// <summary>
        ///   Sauvegarde les modifications d'une dépense.
        /// </summary>
        /// <param name="depense">dépense à modifier</param>
        public void UpdateDepense(DepenseTemporaireEnt depense)
        {
            try
            {
                if (Context.Entry(depense).State == EntityState.Detached)
                {
                    AttachDependancies(depense);
                }

                Context.Entry(depense).State = EntityState.Modified;

                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        ///   Supprime une dépense
        /// </summary>
        /// <param name="id">L'identifiant du dépense à supprimer</param>
        public void DeleteDepenseById(int id)
        {
            DepenseTemporaireEnt depense = Context.DepensesTemporaires.Find(id);
            if (depense == null)
            {
                System.Data.DataException objectNotFoundException = new System.Data.DataException();
                this.logManager.TraceException(objectNotFoundException.Message, objectNotFoundException);
                throw objectNotFoundException;
            }

            Context.DepensesTemporaires.Remove(depense);

            try
            {
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        ///   Retourne la dépense portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="depenseId">Identifiant de la dépense à retrouver.</param>
        /// <returns>La dépense retrouvée, sinon nulle.</returns>
        public DepenseTemporaireEnt GetDepenseById(int depenseId)
        {
            return Context.DepensesTemporaires.Include(t => t.Tache)
                          .Include(d => d.Ressource)
                          .Include(a => a.CI)
                          .Include(f => f.Fournisseur)
                          .Include(cl => cl.CommandeLigne)
                          .Include(d => d.CommandeLigne.Commande)
                          .Include(d => d.Devise)
                          .SingleOrDefault(r => r.DepenseId.Equals(depenseId));
        }

        /// <summary>
        ///   Retourne la liste des dépenses temporaire.
        /// </summary>
        /// <returns>La liste des dépenses temporaire.</returns>
        public IEnumerable<DepenseTemporaireEnt> GetDepenseList()
        {
            foreach (DepenseTemporaireEnt depense in Context.DepensesTemporaires
                                                            .Include(t => t.Tache)
                                                            .Include(d => d.Ressource)
                                                            .Include(a => a.CI)
                                                            .Include(f => f.Fournisseur)
                                                            .Include(cl => cl.CommandeLigne)
                                                            .Include(d => d.CommandeLigne.Commande)
                                                            .Include(d => d.Devise))
            {
                yield return depense;
            }
        }

        /// <summary>
        ///   Retourne une liste de dépenses temporaire en fonction d'un identifiant de CI et d'une date comptable
        ///   et qui n'ont pas été supprimées
        /// </summary>
        /// <param name="ciId">Identifiant du CI associé</param>
        /// <param name="dateComptable">Date comptable de la dépense</param>
        /// <returns>Une liste de dépenses temporaire</returns>
        public IEnumerable<DepenseTemporaireEnt> GetListDepenseByCiIdAndDateComptable(int ciId, DateTime dateComptable)
        {
            foreach (DepenseTemporaireEnt depense in Context.DepensesTemporaires
                                                            .Include(t => t.Tache)
                                                            .Include(d => d.Ressource)
                                                            .Include(a => a.CI)
                                                            .Include(f => f.Fournisseur)
                                                            .Include(cl => cl.CommandeLigne)
                                                            .Include(d => d.CommandeLigne.Commande)
                                                            .Include(d => d.Devise)
                                                            .Where(r => r.Date == dateComptable && r.CiId == ciId))
            {
                yield return depense;
            }
        }

        /// <summary>
        ///   Retourne la liste complète des dépenses temporaire en fonction d'un identifiant de CI et d'une date comptable
        /// </summary>
        /// <param name="ciId">Identifiant du CI associé</param>
        /// <param name="dateComptable">Date comptable de la dépense</param>
        /// <returns>Une liste de dépenses temporaire</returns>
        public IEnumerable<DepenseTemporaireEnt> GetTotalDepenseByCiIdAndDateComptable(int ciId, DateTime dateComptable)
        {
            foreach (DepenseTemporaireEnt depense in Context.DepensesTemporaires
                                                            .Include(t => t.Tache)
                                                            .Include(d => d.Ressource)
                                                            .Include(a => a.CI)
                                                            .Include(f => f.Fournisseur)
                                                            .Include(cl => cl.CommandeLigne)
                                                            .Include(d => d.CommandeLigne.Commande)
                                                            .Include(d => d.Devise)
                                                            .Where(r => r.Date == dateComptable && r.CiId == ciId))
            {
                yield return depense;
            }
        }

        /// <summary>
        ///   Retourne la liste complète des dépenses temporaires en fonction d'un identifiant de dépense parente
        /// </summary>
        /// <param name="depenseParentId">Identifiant de la dépense parente associée</param>
        /// <returns>Une liste de dépenses temporaire</returns>
        public IEnumerable<DepenseTemporaireEnt> GetDepenseListByDepenseParentId(int depenseParentId)
        {
            foreach (DepenseTemporaireEnt depense in Context.DepensesTemporaires
                                                            .Include(t => t.Tache)
                                                            .Include(d => d.Ressource)
                                                            .Include(a => a.CI)
                                                            .Include(f => f.Fournisseur)
                                                            .Include(cl => cl.CommandeLigne)
                                                            .Include(d => d.CommandeLigne.Commande)
                                                            .Include(d => d.Devise)
                                                            .Where(r => r.DepenseParentId == depenseParentId))
            {
                yield return depense;
            }
        }

        /// <summary>
        ///   Retourne la liste des depenses filtrées selon les critères de recherche.
        /// </summary>
        /// <param name="filter">Objet de recherche et de tri des dépense temporaire</param>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>La liste des depenses filtrées selon les critères de recherche et ordonnées selon les critères de tri</returns>
        public IEnumerable<DepenseTemporaireEnt> SearchDepenseListWithFilter(
          SearchDepenseTemporaireEnt filter,
          int page,
          int pageSize)
        {
            var query =
              GetDefaultListQuery()
                .Where(filter.GetPredicateWhere());

            var orderedQuery = filter.ApplyOrderBy(query);
            return orderedQuery.Skip((page - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        ///   Retourne le montant total de la liste des depenses filtrée selon les critères de recherche.
        /// </summary>
        /// <param name="predicateWhere">Prédicat de filtrage des depenses</param>
        /// <returns>
        ///   le montant total de La liste des depenses filtrées selon les critères de recherche et ordonnées selon les
        ///   critères de tri
        /// </returns>
        public double GetMontantTotalDepenseWithFilter(Expression<Func<DepenseTemporaireEnt, bool>> predicateWhere)
        {
            return GetDefaultListQuery().Where(predicateWhere).Sum(d => (double)d.MontantHT);
        }
    }
}