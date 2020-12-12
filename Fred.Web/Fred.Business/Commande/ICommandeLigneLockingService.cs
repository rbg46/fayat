using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Depense;

namespace Fred.Business.Commande
{
    public interface ICommandeLigneLockingService
    {
        Task LockAsync(int commandeLigneId);

        Task UnlockAsync(int commandeLigneId);

        /// <summary>
        ///   Retourne l'etat du champ de verrou d'une ligne de commande.
        /// </summary>
        /// <param name="commandeLigneId">l'id de la commande ligne.</param>
        /// <returns>L'etat sous format booleen</returns>
        Task<bool> IsCommandeLigneLockedAsync(int commandeLigneId);

        void AutomaticLockIfNeededOnDelete(DepenseAchatEnt deletedReception);

        void AutomaticLockIfNeededOnAdd(DepenseAchatEnt addedReception);

        void AutomaticLockIfNeededOnUpdate(List<DepenseAchatEnt> updatedReceptions);

        bool CanAddOrUpdateReceptionsOnCommandeLignes(List<int> commandeLigneIds);
    }
}
