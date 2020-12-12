using Fred.Entities.Commande;
using System.Threading.Tasks;

namespace Fred.Business.Commande
{
    /// <summary>
    /// Interface Gestionnaire des Lignes de Commandes
    /// </summary>
    public interface ICommandeLigneManager : IManager<CommandeLigneEnt>
    {
        /// <summary>
        /// Verouille la commande ligne
        /// </summary>
        /// <param name="commandeLigneId">id de la commande ligne</param>
        /// <returns></returns>
        Task LockAsync(int commandeLigneId);

        /// <summary>
        /// Deverouille la commande ligne
        /// </summary>
        /// <param name="commandeLigneId">id de la commande ligne</param>
        Task UnlockAsync(int commandeLigneId);

        /// <summary>
        ///   Retourne l'etat du champ de verrou d'une ligne de commande.
        /// </summary>
        /// <param name="commandeLigneId">l'id de la commande ligne.</param>
        /// <returns>L'etat sous format booleen</returns>
        Task<bool> IsCommandeLigneLockedAsync(int commandeLigneId);
    }
}
