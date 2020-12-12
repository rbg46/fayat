using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Fred.Business.Action.CommandeLigne;
using Fred.Business.Action.CommandeLigne.Models;
using Fred.Business.Action.Models;
using Fred.Business.Commande.Validators;
using Fred.Business.OrganisationFeature;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Framework.Exceptions;

namespace Fred.Business.Commande
{
    public class CommandeLigneLockingService : ICommandeLigneLockingService
    {
        public const string FeatureKey = "lock.unlock.commandeLigne";
        private readonly ICommandeLignesRepository commandeLignesRepository;
        private readonly ICommandeLigneLockValidator commandeLigneLockValidator;
        private readonly ICommandeLigneUnlockValidator commandeLigneUnlockValidator;
        private readonly IOrganisationRelatedFeatureService organisationRelatedFeatureService;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IActionCommandeLigneService actionCommandeLigneService;

        public CommandeLigneLockingService(
            ICommandeLignesRepository commandeLignesRepository,
            ICommandeLigneLockValidator commandeLigneLockValidator,
            ICommandeLigneUnlockValidator commandeLigneUnlockValidator,
            IOrganisationRelatedFeatureService organisationRelatedFeatureService,
            IUtilisateurManager utilisateurManager,
            IActionCommandeLigneService actionCommandeLigneService)
        {
            this.commandeLignesRepository = commandeLignesRepository;
            this.commandeLigneLockValidator = commandeLigneLockValidator;
            this.commandeLigneUnlockValidator = commandeLigneUnlockValidator;
            this.organisationRelatedFeatureService = organisationRelatedFeatureService;
            this.utilisateurManager = utilisateurManager;
            this.actionCommandeLigneService = actionCommandeLigneService;
        }

        public async Task LockAsync(int commandeLigneId)
        {
            if (IsLockUnlockCommandeLigneEnabledForCurrentUser())
            {
                CommandeLigneEnt commandeLigne = await commandeLignesRepository.FindByIdAsync(commandeLigneId);

                ValidateCommandeLigne(commandeLigne, commandeLigneLockValidator);

                LockCommandeLigne(commandeLigneId);
            }
            else
            {
                throw new FredBusinessException(CommandeResources.FeatureNotEnabledOnOrganisation);
            }
        }

        public async Task UnlockAsync(int commandeLigneId)
        {
            if (IsLockUnlockCommandeLigneEnabledForCurrentUser())
            {
                CommandeLigneEnt commandeLigne = await commandeLignesRepository.FindByIdAsync(commandeLigneId);

                ValidateCommandeLigne(commandeLigne, commandeLigneUnlockValidator);

                UnlockCommandeLigne(commandeLigneId);
            }
            else
            {
                throw new FredBusinessException(CommandeResources.FeatureNotEnabledOnOrganisation);
            }
        }

        /// <summary>
        ///   Retourne l'etat du champ de verrou d'une ligne de commande.
        /// </summary>
        /// <param name="commandeLigneId">l'id de la commande ligne.</param>
        /// <returns>L'etat sous format booleen</returns>
        public async Task<bool> IsCommandeLigneLockedAsync(int commandeLigneId)
        {
            if (!IsLockUnlockCommandeLigneEnabledForCurrentUser())
            {
                return false;
            }

            CommandeLigneEnt commandeLigne = await commandeLignesRepository.FindByIdAsync(commandeLigneId);

            VerifyNullCommandeLigne(commandeLigne);

            return commandeLigne.IsVerrou;
        }

        public void AutomaticLockIfNeededOnDelete(DepenseAchatEnt deletedReception)
        {
            if (!IsLockUnlockCommandeLigneEnabledForCurrentUser())
            {
                return;
            }

            if (deletedReception == null || !deletedReception.CommandeLigneId.HasValue)
            {
                throw new Exception(CommandeResources.CommandeLigneNotFound);
            }

            CommandeLigneWithReceptionQuantiteModel commandeLigne = commandeLignesRepository.GetCommandeLigneWithReceptionQuantiteById(deletedReception.CommandeLigneId.Value);
            if (Math.Abs(commandeLigne.GetQuantiteReceptionee() - deletedReception.Quantite) >= commandeLigne.Quantite)
            {
                CreateActionCommandeLigne(deletedReception.CommandeLigneId.Value, true, ActionStatus.Initiated);
                LockCommandeLigne(commandeLigne);
            }
        }

        public void AutomaticLockIfNeededOnAdd(DepenseAchatEnt addedReception)
        {
            if (!IsLockUnlockCommandeLigneEnabledForCurrentUser())
            {
                return;
            }

            if (addedReception == null || !addedReception.CommandeLigneId.HasValue)
            {
                throw new Exception(CommandeResources.CommandeLigneNotFound);
            }

            CommandeLigneWithReceptionQuantiteModel commandeLigne = commandeLignesRepository.GetCommandeLigneWithReceptionQuantiteById(addedReception.CommandeLigneId.Value);
            if (Math.Abs(commandeLigne.GetQuantiteReceptionee() + addedReception.Quantite) >= commandeLigne.Quantite)
            {
                CreateActionCommandeLigne(addedReception.CommandeLigneId.Value, true, ActionStatus.Initiated);
                LockCommandeLigne(commandeLigne);
            }
        }

        public void AutomaticLockIfNeededOnUpdate(List<DepenseAchatEnt> updatedReceptions)
        {
            if (!IsLockUnlockCommandeLigneEnabledForCurrentUser())
            {
                return;
            }

            if (updatedReceptions != null)
            {
                List<int> commandeLigneIds = updatedReceptions.Where(r => r.CommandeLigneId.HasValue).Select(r => r.CommandeLigneId.Value).ToList();
                List<int> updatedReceptionsIds = updatedReceptions.Select(r => r.DepenseId).ToList();
                List<CommandeLigneWithReceptionQuantiteModel> commandeLignesModel = commandeLignesRepository.GetCommandeLignesWithReceptionQuantiteByIds(commandeLigneIds);

                foreach (CommandeLigneWithReceptionQuantiteModel commandeLigneModel in commandeLignesModel)
                {
                    decimal previousQuantiteOfUpdatedReceptions = commandeLigneModel.Receptions.Where(r => updatedReceptionsIds.Contains(r.ReceptionId)).Sum(r => r.Quantite);
                    decimal newQuantiteOfUpdatedReceptions = updatedReceptions.Where(r => r.CommandeLigneId == commandeLigneModel.CommandeLigneId).Sum(r => r.Quantite);// analyse 12552 not ok 
                    if (Math.Abs(commandeLigneModel.GetQuantiteReceptionee() - previousQuantiteOfUpdatedReceptions + newQuantiteOfUpdatedReceptions) >= commandeLigneModel.Quantite)
                    {
                        CreateActionCommandeLigne(commandeLigneModel.CommandeLigneId, true, ActionStatus.Initiated);
                        LockCommandeLigne(commandeLigneModel);
                    }
                }
            }
        }

        public bool CanAddOrUpdateReceptionsOnCommandeLignes(List<int> commandeLigneIds)
        {
            if (!IsLockUnlockCommandeLigneEnabledForCurrentUser() || commandeLigneIds == null || !commandeLigneIds.Any())
            {
                return true;
            }

            List<Expression<Func<CommandeLigneEnt, bool>>> filters = new List<Expression<Func<CommandeLigneEnt, bool>>>
            {
                x => commandeLigneIds.Contains(x.CommandeLigneId)
            };

            List<CommandeLigneEnt> commandeLignes = commandeLignesRepository.Get(filters);

            return commandeLignes.All(x => !x.IsVerrou);
        }

        private void ValidateCommandeLigne(CommandeLigneEnt commandeLigne, IValidator<CommandeLigneEnt> validator)
        {
            VerifyNullCommandeLigne(commandeLigne);

            if (commandeLigne != null && validator != null)
            {
                ValidationResult validationResult = validator.Validate(commandeLigne);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
            }
        }

        private void VerifyNullCommandeLigne(CommandeLigneEnt commandeLigne)
        {
            if (commandeLigne == null)
            {
                throw new Exception(CommandeResources.CommandeLigneNotFound);
            }
        }

        private void UnlockCommandeLigne(int commandeLigneId)
        {
            int utilisateurId = utilisateurManager.GetContextUtilisateurId();
            commandeLignesRepository.UnlockCommandeLigne(commandeLigneId, utilisateurId);
        }

        private void LockCommandeLigne(CommandeLigneWithReceptionQuantiteModel commandeLigne)
        {
            int utilisateurId = utilisateurManager.GetContextUtilisateurId();
            commandeLignesRepository.LockCommandeLigne(commandeLigne.CommandeLigneId, utilisateurId);
        }

        private void LockCommandeLigne(int commandeLigneId)
        {
            int utilisateurId = utilisateurManager.GetContextUtilisateurId();
            commandeLignesRepository.LockCommandeLigne(commandeLigneId, utilisateurId);
        }

        private bool IsLockUnlockCommandeLigneEnabledForCurrentUser()
        {
            return organisationRelatedFeatureService.IsEnabledForCurrentUser(FeatureKey, false);
        }

        private void CreateActionCommandeLigne(int commandeLigneId, bool isLocked, ActionStatus actionStatus)
        {
            int utilisateurId = utilisateurManager.GetContextUtilisateurId();
            ActionCommandeLigneInputModel actionInputModel = BuildActionLockUnlockInputModel(
                commandeLigneId,
                isLocked,
                actionStatus,
                utilisateurId);
            actionCommandeLigneService.CreateActionCommandeLigne(actionInputModel);
        }

        private ActionCommandeLigneInputModel BuildActionLockUnlockInputModel(int commandeLigneId, bool toLock, ActionStatus? actionStatus, int auteurId)
        {
            return new ActionCommandeLigneInputModel
            {
                ActionInput = new ActionInputModel
                {
                    ActionStatus = actionStatus,
                    ActionType = toLock ? ActionType.Verrouillage : ActionType.Deverrouillage,
                    Message = CommandeResources.SaveLockActionCommandeLigneInFred,
                    AuteurId = auteurId
                },
                CommandeLigneId = commandeLigneId
            };
        }
    }
}
