using Fred.Business.Action.CommandeLigne;
using Fred.Business.Action.CommandeLigne.Models;
using Fred.Business.Action.Models;
using Fred.Business.Commande;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Action;
using Fred.Entities.Commande;
using Fred.Entities.Utilisateur;
using NLog;
using System;
using System.Threading.Tasks;

namespace Fred.Business.Action
{
    public class ActionCommandeLigneService : IActionCommandeLigneService
    {
        private readonly string InvalidActionErrorMessage = CommandeResources.ActionInvalidInformation;
        private readonly string ActionCreationErrorMessage = CommandeResources.ActionCreationErrorMessage;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IActionCommandeLigneRepository actionCommandeLigneRepository;
        private readonly IActionStatusRepository actionStatusRepository;
        private readonly IActionTypeRepository actionTypeRepository;

        public ActionCommandeLigneService(IUnitOfWork unitOfWork,
            IUtilisateurManager utilisateurManager,
            IActionCommandeLigneRepository actionCommandeLigneRepository,
            IActionStatusRepository actionStatusRepository,
            IActionTypeRepository actionTypeRepository)
        {
            this.unitOfWork = unitOfWork;
            this.utilisateurManager = utilisateurManager;
            this.actionCommandeLigneRepository = actionCommandeLigneRepository;
            this.actionStatusRepository = actionStatusRepository;
            this.actionTypeRepository = actionTypeRepository;
        }

        private Logger Logger => LogManager.GetCurrentClassLogger();

        public async Task<ActionCommandeLigneEnt> FindByIdAsync(int id)
        {
            return await actionCommandeLigneRepository.FindByIdAsync(id);
        }

        public void CreateActionCommandeLigne(ActionCommandeLigneInputModel inputModel)
        {
            try
            {
                ValidateInputModel(inputModel);

                InsertActionCommandeLigne(inputModel);

                Save();
            }
            catch (Exception ex)
            {
                //Cette fonctionnalite ne doit pas empecher le fonctionnement optimale de l'application. Pas de remonter d'exceptions.
                Logger.Error(ex, ActionCreationErrorMessage);
            }
        }

        private void InsertActionCommandeLigne(ActionCommandeLigneInputModel inputModel)
        {
            ActionInputModel actionInputModel = inputModel.ActionInput;
            ActionStatusEnt actionStatus = actionStatusRepository.FindByName(actionInputModel.ActionStatus.Value.GetDescription());
            ActionTypeEnt actionType = actionTypeRepository.FindByCode(actionInputModel.ActionType.Value.GetDescription());

            //Insertion dans le repository
            ActionCommandeLigneEnt createdActionCommandeLigne = ConvertInputModelToActionCommandeLigne(inputModel, actionStatus, actionType);
            actionCommandeLigneRepository.InsertActionCommandeLigneAsync(createdActionCommandeLigne);
        }

        private ActionCommandeLigneEnt ConvertInputModelToActionCommandeLigne(ActionCommandeLigneInputModel inputModel, ActionStatusEnt actionStatus, ActionTypeEnt actionType)
        {
            return new ActionCommandeLigneEnt
            {
                Action = ConvertActionInputModelToActionEnt(inputModel.ActionInput, actionStatus, actionType),
                CommandeLigneId = inputModel.CommandeLigneId
            };
        }

        private ActionEnt ConvertActionInputModelToActionEnt(ActionInputModel inputModel, ActionStatusEnt actionStatus, ActionTypeEnt actionType)
        {
            return new ActionEnt
            {
                ActionStatusId = actionStatus.ActionStatusId,
                ActionTypeId = actionType.ActionTypeId,
                ActionJob = inputModel.JobActionInput != null ? ConvertJobActionInputModelToActionJobEnt(inputModel.JobActionInput) : null,
                DateAction = DateTime.Now,
                Message = inputModel.Message,
                AuteurId = inputModel.AuteurId.HasValue ? inputModel.AuteurId : utilisateurManager.GetContextUtilisateurId()
            };
        }

        private ActionJobEnt ConvertJobActionInputModelToActionJobEnt(JobActionInputModel jobActionInput)
        {
            return new ActionJobEnt
            {
                ExternalJobId = jobActionInput.ExternalJobId,
                ExternalJobName = jobActionInput.ExternalJobName
            };
        }

        private void ValidateInputModel(ActionCommandeLigneInputModel inputModel)
        {
            if (inputModel == null)
            {
                throw new ArgumentNullException(nameof(inputModel));
            }

            if (inputModel.ActionInput == null || !inputModel.ActionInput.ActionType.HasValue || !inputModel.ActionInput.ActionStatus.HasValue)
            {
                throw new ArgumentException(InvalidActionErrorMessage, nameof(inputModel));
            }
        }

        private void Save()
        {
            unitOfWork.Save();
        }
    }
}
