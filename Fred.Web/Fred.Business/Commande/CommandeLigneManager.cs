using System;
using System.Threading.Tasks;
using Fred.Business.Action.CommandeLigne;
using Fred.Business.Action.CommandeLigne.Models;
using Fred.Business.Action.Models;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Commande;

namespace Fred.Business.Commande
{
    /// <summary>
    /// Gestionnaire des Lignes de Commandes
    /// </summary>
    public class CommandeLigneManager : Manager<CommandeLigneEnt, ICommandeLignesRepository>, ICommandeLigneManager
    {
        private readonly ICommandeLigneLockingService commandeLigneLockingService;
        private readonly IActionCommandeLigneService actionCommandeLigneService;

        public CommandeLigneManager(
            IUnitOfWork uow,
            ICommandeLignesRepository commandeLignesRepository,
            ICommandeLigneLockingService commandeLigneLockingService,
            IActionCommandeLigneService actionCommandeLigneService)
        : base(uow, commandeLignesRepository)
        {
            this.commandeLigneLockingService = commandeLigneLockingService;
            this.actionCommandeLigneService = actionCommandeLigneService;
        }

        public async Task LockAsync(int commandeLigneId)
        {
            try
            {
                await commandeLigneLockingService.LockAsync(commandeLigneId);
                CreateActionLockUnlock(commandeLigneId, ActionType.Verrouillage, ActionStatus.Success);
                Save();
            }
            catch (Exception ex)
            {
                CreateActionLockUnlock(commandeLigneId, ActionType.Verrouillage, ActionStatus.Failed, ex.Message);
                Save();
                throw;
            }
        }

        public async Task UnlockAsync(int commandeLigneId)
        {
            try
            {
                await commandeLigneLockingService.UnlockAsync(commandeLigneId);
                CreateActionLockUnlock(commandeLigneId, ActionType.Deverrouillage, ActionStatus.Success);
                Save();
            }
            catch (Exception ex)
            {
                CreateActionLockUnlock(commandeLigneId, ActionType.Deverrouillage, ActionStatus.Failed, ex.Message);
                Save();
                throw;
            }
        }

        /// <summary>
        ///   Retourne l'etat du champ de verrou d'une ligne de commande.
        /// </summary>
        /// <param name="commandeLigneId">l'id de la commande ligne.</param>
        /// <returns>L'etat sous format booleen</returns>
        public async Task<bool> IsCommandeLigneLockedAsync(int commandeLigneId)
        {
            return await commandeLigneLockingService.IsCommandeLigneLockedAsync(commandeLigneId);
        }

        private void CreateActionLockUnlock(int commandeLigneId, ActionType actionType, ActionStatus actionStatus, string appendMessage = "")
        {
            var createdAction = new ActionCommandeLigneInputModel
            {
                CommandeLigneId = commandeLigneId,
                ActionInput = new ActionInputModel
                {
                    ActionStatus = actionStatus,
                    ActionType = actionType,
                    Message = CommandeResources.SaveLockActionCommandeLigneInFred
                }
            };

            if (!string.IsNullOrWhiteSpace(appendMessage))
            {
                createdAction.ActionInput.Message += createdAction.ActionInput.Options.ConcatSeparator + appendMessage;
            }

            actionCommandeLigneService.CreateActionCommandeLigne(createdAction);
        }
    }
}
