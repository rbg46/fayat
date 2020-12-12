using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.DataAccess.Achat.Calculation.Common;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.Entities.CommandeLigne.QuantiteNegative;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;
namespace Fred.DataAccess.Commande
{
    /// <summary>
    /// Repository de lignes de commande
    /// </summary>
    public class CommandeLignesRepository : FredRepository<CommandeLigneEnt>, ICommandeLignesRepository
    {
        private readonly AchatCommonExpressionsFiltersHelper achatCommonExpressionsFiltersHelper;

        public CommandeLignesRepository(FredDbContext context)
            : base(context)
        {
            achatCommonExpressionsFiltersHelper = new AchatCommonExpressionsFiltersHelper();
        }

        /// <summary>
        ///   Retourne la ligne de commande portant l'identifiant unique du materiel externe.
        /// </summary>
        /// <param name="materielId">Identifiant du materiel externe.</param>
        /// <returns>La ligne de commande retrouvée, sinon nulle.</returns>
        public CommandeLigneEnt GetCommandeLigneByMaterielId(int materielId)
        {
            return Context.CommandeLigne
                        .Include(c => c.Unite)
                        .Include(c => c.Commande)
                        .Where(c => c.MaterielId == materielId)
                        .FirstOrDefault();
        }


        /// <summary>
        /// retourne une commande suivant l'id de la ligne de commande
        /// </summary>
        /// <param name="commandeLigneId">Id de la ligne de commande</param>
        /// <returns>une commande</returns>
        public async Task<CommandeLigneEnt> GetCommandeLigneByIdAsync(int commandeLigneId)
        {
            return await Context.CommandeLigne
                    .Where(c => c.CommandeLigneId.Equals(commandeLigneId))
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
        }

        /// <summary>
        /// retourne la commande ligne avec les dépenses qui lui sont liés
        /// </summary>
        /// <param name="commandeLigneId"></param>
        /// <returns></returns>
        public CommandeLigneWithReceptionQuantiteModel GetCommandeLigneWithReceptionQuantiteById(int commandeLigneId)
        {
            CommandeLigneEnt commandeLigneEnt = Context.CommandeLigne
                .Include(c => c.AllDepenses)
                .ThenInclude(d => d.DepenseType)
                .Where(c => c.CommandeLigneId == commandeLigneId)
                .AsNoTracking()
                .FirstOrDefault();

            return new CommandeLigneWithReceptionQuantiteModel
            {
                CommandeLigneId = commandeLigneEnt.CommandeLigneId,
                Quantite = commandeLigneEnt.Quantite,
                Receptions = commandeLigneEnt.AllDepenses
                        .Where(x => x.CommandeLigneId.HasValue && !x.DateSuppression.HasValue).Select(x => new Fred.Entities.Commande.ReceptionQuantiteModel
                        {
                            Quantite = x.Quantite
                        }).ToList()
            };
        }

        public void LockCommandeLigne(int commandeLigneId, int auteurModificationId)
        {
            var commandeLigne = Context.Entry(
                new CommandeLigneEnt
                {
                    IsVerrou = true,
                    CommandeLigneId = commandeLigneId,
                    AuteurModificationId = auteurModificationId,
                    DateModification = DateTime.UtcNow
                });
            commandeLigne.Property(x => x.IsVerrou).IsModified = true;
            commandeLigne.Property(x => x.AuteurModificationId).IsModified = true;
            commandeLigne.Property(x => x.DateModification).IsModified = true;
        }

        public void UnlockCommandeLigne(int commandeLigneId, int auteurModificationId)
        {
            var commandeLigne = Context.Entry(
                new CommandeLigneEnt
                {
                    IsVerrou = false,
                    CommandeLigneId = commandeLigneId,
                    AuteurModificationId = auteurModificationId,
                    DateModification = DateTime.UtcNow
                });
            commandeLigne.Property(x => x.IsVerrou).IsModified = true;
            commandeLigne.Property(x => x.AuteurModificationId).IsModified = true;
            commandeLigne.Property(x => x.DateModification).IsModified = true;
        }

        public List<CommandeLigneWithReceptionQuantiteModel> GetCommandeLignesWithReceptionQuantiteByIds(List<int> commandeLigneIds)
        {
            return Context.CommandeLigne
                .Include(c => c.AllDepenses)
                .ThenInclude(d => d.DepenseType)
                .Where(c => commandeLigneIds.Contains(c.CommandeLigneId))
                .AsNoTracking()
                .Select(cl => new CommandeLigneWithReceptionQuantiteModel
                {
                    CommandeLigneId = cl.CommandeLigneId,
                    Quantite = cl.Quantite,
                    Receptions = cl.AllDepenses.Where(d => d.CommandeLigneId.HasValue && !d.DateSuppression.HasValue).Select(d => new Fred.Entities.Commande.ReceptionQuantiteModel
                    {
                        Quantite = d.Quantite,
                        ReceptionId = d.DepenseId
                    }).ToList()
                })
                .ToList();
        }

        public List<CommandeLigneEnt> Get(List<Expression<Func<CommandeLigneEnt, bool>>> filters,
                                                Func<IQueryable<CommandeLigneEnt>, IOrderedQueryable<CommandeLigneEnt>> orderBy = null,
                                                List<Expression<Func<CommandeLigneEnt, object>>> includeProperties = null,
                                                int? page = null,
                                                int? pageSize = null,
                                                bool asNoTracking = true)
        {
            if (asNoTracking)
            {
                return base.Get(filters, orderBy, includeProperties, page, pageSize).AsNoTracking().ToList();
            }

            return base.Get(filters, orderBy, includeProperties, page, pageSize).ToList();
        }

        /// <summary>
        /// Ajout d'une ligne de commande
        /// </summary>
        /// <param name="ligne">ligne de la commande</param>
        public void AddCommandeLigne(CommandeLigneEnt ligne)
        {
            Context.CommandeLigne.Add(ligne);
        }

        /// <summary>
        /// Update d'une ligne de commande
        /// </summary>
        /// <param name="ligne">ligne de la commande</param>
        public void UpdateCommandeLigne(CommandeLigneEnt ligne)
        {
            Context.CommandeLigne.Update(ligne);
        }

        /// <summary>
        /// Suppression d'une ligne de commande
        /// </summary>
        /// <param name="ligne">ligne de la commande</param>
        public void DeleteCommandeLigne(CommandeLigneEnt ligne)
        {
            Context.CommandeLigne.Remove(ligne);
        }

        public CommandeLigneQuantiteNegativeModel GetCommandeLigneWithReceptionsQuantities(int commandeLigneId)
        {
            return GetCommandeLignesWithReceptionsQuantities(new List<int>() { commandeLigneId }).First();
        }

        public List<CommandeLigneQuantiteNegativeModel> GetCommandeLignesWithReceptionsQuantities(List<int> commandesLigneIds)
        {
            if (commandesLigneIds == null)
                throw new ArgumentNullException(nameof(commandesLigneIds));

            var depenseAchatNotDeletedFilter = achatCommonExpressionsFiltersHelper.GetIsDepenseAchatNotDeletedFilter();

            var result = (from commandeLigne in Context.CommandeLigne.Where(x => commandesLigneIds.Contains(x.CommandeLigneId))
                          let depenses = commandeLigne.AllDepenses.AsQueryable().Where(depenseAchatNotDeletedFilter)// analyse 12552 ok
                          let quantiteReceptionnee = depenses != null && depenses.Count() > 0 ? depenses.Sum(r => r.Quantite) : 0
                          select new CommandeLigneQuantiteNegativeModel
                          {
                              CommandeLigneId = commandeLigne.CommandeLigneId,
                              QuantiteReceptionnee = quantiteReceptionnee,
                              Quantite = commandeLigne.Quantite,
                              LigneDeCommandeNegative = commandeLigne.AvenantLigne != null ? commandeLigne.AvenantLigne.IsDiminution : false,
                              ReceptionQuantites = depenses.Select(x => new Entities.Reception.QuantiteNegative.ReceptionQuantiteModel
                              {
                                  ReceptionId = x.DepenseId,
                                  Quantity = x.Quantite,
                              }).ToList()
                          }).ToList();
            return result;
        }

    }
}
