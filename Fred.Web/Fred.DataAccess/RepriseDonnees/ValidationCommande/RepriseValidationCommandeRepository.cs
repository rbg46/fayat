using Fred.DataAccess.Common;
using Fred.Entities.Commande;
using Fred.EntityFramework;
using Fred.Framework.Exceptions;
using System;
using Fred.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Fred.DataAccess.RepriseDonnees.ValidationCommande
{
    /// <summary>
    /// Repository des Reprises des données
    /// </summary>
    public class RepriseValidationCommandeRepository : IRepriseValidationCommandeRepository
    {

        private readonly FredDbContext fredDbContext;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="unitOfWork">unitOfWork</param>       
        public RepriseValidationCommandeRepository(IUnitOfWork unitOfWork)
        {
            this.fredDbContext = (unitOfWork as UnitOfWork).Context;
        }

        /// <summary>
        /// Marque les commande comme validées et sauvegarde en base
        /// </summary>
        /// <param name="commandes">Les commande a validées</param>
        public void SetCommandesAsValidatedAndSaveCommandes(List<CommandeEnt> commandes)
        {
            Action<CommandeEnt> updateAction = (commande) => this.fredDbContext.Entry(commande).Property(x => x.StatutCommandeId).IsModified = true;
            BulkSave(commandes, updateAction);
        }

        /// <summary>
        /// Met a jour l'HangfireJobId 
        /// </summary>
        /// <param name="commandes">Les commande a validées</param>
        public void SetHangfireIdsAndSaveCommandes(List<CommandeEnt> commandes)
        {
            Action<CommandeEnt> updateAction = (commande) => this.fredDbContext.Entry(commande).Property(x => x.HangfireJobId).IsModified = true;

            BulkSave(commandes, updateAction);
        }

        private void BulkSave(List<CommandeEnt> commandes, Action<CommandeEnt> updateAction)
        {
            if (!commandes.Any())
            {
                return;
            }
            try
            {
                fredDbContext.ChangeTracker.AutoDetectChangesEnabled = false;
                // disable detection of changes
                int count = 0;
                const int commitCount = 100;

                // Mise à jour des personnels en BDD
                foreach (CommandeEnt commande in commandes)
                {
                    ++count;
                    this.fredDbContext.Commandes.Attach(commande);
                    updateAction(commande);

                    // A chaque 100 opérations, on sauvegarde le contexte.
                    if (count % commitCount == 0)
                    {
                        fredDbContext.SaveChanges();
                    }

                }
                fredDbContext.SaveChanges();

            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
            finally
            {
                // re-enable detection of changes
                this.fredDbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }

    }



}
