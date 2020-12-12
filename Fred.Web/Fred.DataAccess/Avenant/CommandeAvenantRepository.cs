using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Avenant
{
    /// <summary>
    /// Représente un référentiel de données pour les avenanta de commande.
    /// </summary>
    public class CommandeAvenantRepository : FredRepository<CommandeAvenantEnt>, ICommandeAvenantRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="CommandeAvenantRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        /// <param name="context">Fred context</param>
        public CommandeAvenantRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        /// Liste des avenants d'une commande
        /// </summary>
        /// <param name="commandeId">commande Id</param>
        /// <returns>Liste des avenants</returns>
        public IEnumerable<CommandeAvenantEnt> GetAvenantByCommandeId(int commandeId)
        {
            return Context.CommandeAvenant
                .Where(x => x.CommandeId == commandeId)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Récupération de l'avenant d'une commande depuis son commandeId et son numeroAvenant
        /// </summary>
        /// <param name="commandeId">commandeId</param>
        /// <param name="numeroAvenant">numeroAvenant</param>
        /// <returns>CommandeAvenantEnt</returns>
        public CommandeAvenantEnt GetAvenantByCommandeIdAndAvenantNumber(int commandeId, int numeroAvenant)
        {
            return Context.CommandeAvenant
               .Where(a => a.CommandeId == commandeId && a.NumeroAvenant == numeroAvenant)
               .AsNoTracking()
               .First();
        }

        /// <summary>
        /// Ajoute un avenant
        /// </summary>
        /// <param name="avenant">avenant</param>
        public void AddAvenant(CommandeAvenantEnt avenant)
        {
            Context.CommandeAvenant.Add(avenant);
        }


        /// <summary>
        /// Update d'un avenant
        /// </summary>
        /// <param name="avenant">avenant</param>
        public void UpdateAvenant(CommandeAvenantEnt avenant)
        {
            Context.CommandeAvenant.Update(avenant);
        }


    }
}
