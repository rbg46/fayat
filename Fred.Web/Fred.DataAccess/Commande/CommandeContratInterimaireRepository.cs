
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Commande
{
    /// <summary>
    ///   Référentiel de données pour les commandes liés aux contrats intérimaires
    /// </summary>
    public class CommandeContratInterimaireRepository : FredRepository<CommandeContratInterimaireEnt>, ICommandeContratInterimaireRepository
    {

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="CommandeContratInterimaireRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        /// <param name="uow">Unit of work</param>
        public CommandeContratInterimaireRepository(FredDbContext context)
          : base(context)
        { }

        /// <summary>
        /// Permet de savoir s'il existe une commande contrat intérimaire en fonction de la commande Id, du contrat Id et du ci Id
        /// </summary>
        /// <param name="contratId">identifiant unique du contrat intérimaire</param>
        /// <returns>La délégation</returns>
        public CommandeContratInterimaireEnt GetCommandeContratInterimaireExist(int contratId)
        {
            return Query()
              .Get()
              .Where(c => c.ContratId == contratId)
              .FirstOrDefault();
        }

        /// <summary>
        /// Permet de récupérer une commande contrat intérimaire en fonction du contrat Id et du ci Id
        /// </summary>
        /// <param name="contratId">identifiant unique du contrat intérimaire</param>
        /// <param name="ciId">identifiant unique du CI</param>
        /// <returns>La délégation</returns>
        public CommandeContratInterimaireEnt GetCommandeContratInterimaire(int contratId, int ciId)
        {
            return Query()
              .Include(c => c.Commande.Lignes)
              .Include(c => c.Contrat.Ci.Societe.TypeSociete)
              .Include(c => c.Contrat.Societe.TypeSociete)
              .Filter(c => c.ContratId == contratId && c.CiId == ciId)
              .Get()
              .AsNoTracking()
              .FirstOrDefault();
        }

        /// <summary>
        /// Permet de récupérer une commande contrat intérimaire en fonction du commandeId
        /// </summary>
        /// <param name="commandeId">identifiant unique d'une commande</param>
        /// <returns>La CommandeContratInterimaire</returns>
        public CommandeContratInterimaireEnt GetCommandeContratInterimaireByCommandeId(int commandeId)
        {
            return Query()
              .Include(c => c.Interimaire)
              .Filter(c => c.CommandeId == commandeId)
              .Get()
              .AsNoTracking()
              .FirstOrDefault();
        }

        /// <summary>
        /// Permet d'ajouter une commande contrat intérimaire
        /// </summary>
        /// <param name="commandeContratInterimaireEnt">commande contrat intérimaire</param>
        /// <returns>La commande contrat intérimaire enregistrée</returns>
        public CommandeContratInterimaireEnt AddCommandeContratInterimaire(CommandeContratInterimaireEnt commandeContratInterimaireEnt)
        {
            Insert(commandeContratInterimaireEnt);

            return commandeContratInterimaireEnt;
        }
    }
}
