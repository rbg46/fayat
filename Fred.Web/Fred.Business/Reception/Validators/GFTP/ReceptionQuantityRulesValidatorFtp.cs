using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Validators;
using Fred.DataAccess.Interfaces;
using Fred.Entities.CommandeLigne.QuantiteNegative;
using Fred.Entities.Depense;
using Fred.Entities.Reception.QuantiteNegative;

namespace Fred.Business.Reception.Validators.GFTP
{
    public class ReceptionQuantityRulesValidatorFtp : ReceptionQuantityRulesValidator, IReceptionQuantityRulesValidatorGFTP
    {
        private const string ErrorMessage = "Cas normalement impossible, une reception est soit ajoutée, modifiée ou supprimée.";
        private readonly ICommandeLignesRepository commandeLignesRepository;
        public ReceptionQuantityRulesValidatorFtp(ICommandeLignesRepository commandeLignesRepository)
        {
            this.commandeLignesRepository = commandeLignesRepository;
        }

        /// <summary>
        /// SURCHARGE POUR LE GROUPE FTP 
        /// </summary>
        /// <param name="receptions"></param>
        /// <param name="context"></param>
        public override void ExecuteQuantiteRuleForGroupe(ReceptionsValidationModel receptionsForValidate, CustomContext context)
        {
            List<CommandeLigneQuantiteNegativeModel> commandeLigneQuantiteNegativeModels = GetInfosForValidateReceptionsNegatives(receptionsForValidate);

            CreateReceptionQuantiteModelsWithNewValues(commandeLigneQuantiteNegativeModels, receptionsForValidate);

            CalculateQuantityReceptionneeOfCommandeLignes(commandeLigneQuantiteNegativeModels);

            List<DepenseAchatEnt> allReceptions = receptionsForValidate.ReceptionToAddOrUpdates.Concat(receptionsForValidate.ReceptionToDeletes).ToList();

            foreach (DepenseAchatEnt reception in allReceptions)
            {
                CommandeLigneQuantiteNegativeModel commandeLigneQuantiteNegative = commandeLigneQuantiteNegativeModels.First(x => x.CommandeLigneId == reception.CommandeLigneId);

                AddFaillureForLigneCommandePossitifIfNecessary(context, reception, commandeLigneQuantiteNegative);

                AddFaillureForLigneCommandeNegativeIfNecessary(context, reception, commandeLigneQuantiteNegative);
            }
        }


        private void AddFaillureForLigneCommandeNegativeIfNecessary(CustomContext context, DepenseAchatEnt reception, CommandeLigneQuantiteNegativeModel commandeLigneQuantiteNegative)
        {
            if (commandeLigneQuantiteNegative.LigneDeCommandeNegative)
            {
                if (commandeLigneQuantiteNegative.NewQuantiteReceptionnee > 0)
                {
                    if (ToBeAdded(reception) || ToBeUpdated(commandeLigneQuantiteNegative, reception))
                    {
                        var oldReceptionQuantite = commandeLigneQuantiteNegative.ReceptionQuantites.FirstOrDefault(x => x.ReceptionId == reception.DepenseId);

                        var oldQuantite = oldReceptionQuantite == null ? 0 : oldReceptionQuantite.Quantity;

                        var newQuantitePossible = -(commandeLigneQuantiteNegative.QuantiteReceptionnee - oldQuantite);

                        string errorMessage = string.Format(BusinessResources.Reception_Quantity_Rules_Validator_GFTP_Error_Quantity_Inf_Or_Egual, newQuantitePossible);

                        context.AddFailure("Quantite_" + reception.DepenseId, errorMessage);
                        context.AddFailure("Quantite_" + reception.DepenseId, BusinessResources.Reception_Quantity_Rules_Validator_GFTP_Error_Quantity_Negative);
                        return;
                    }

                    if (ToBeDelete(commandeLigneQuantiteNegative, reception))
                    {
                        context.AddFailure("Quantite_" + reception.DepenseId, BusinessResources.Reception_Quantity_Rules_Validator_GFTP_Error_Quantity_Negative);
                        return;
                    }

                    throw new Framework.Exceptions.FredBusinessException(ErrorMessage);
                }
            }
        }

        private void AddFaillureForLigneCommandePossitifIfNecessary(CustomContext context, DepenseAchatEnt reception, CommandeLigneQuantiteNegativeModel commandeLigneQuantiteNegative)
        {
            if (!commandeLigneQuantiteNegative.LigneDeCommandeNegative)
            {
                if (commandeLigneQuantiteNegative.NewQuantiteReceptionnee < 0)
                {
                    if (ToBeAdded(reception) || ToBeUpdated(commandeLigneQuantiteNegative, reception))
                    {
                        var oldReceptionQuantite = commandeLigneQuantiteNegative.ReceptionQuantites.FirstOrDefault(x => x.ReceptionId == reception.DepenseId);

                        var oldQuantite = oldReceptionQuantite == null ? 0 : oldReceptionQuantite.Quantity;

                        var newQuantitePossible = (commandeLigneQuantiteNegative.QuantiteReceptionnee - oldQuantite);

                        string errorMessage = string.Format(BusinessResources.Reception_Quantity_Rules_Validator_GFTP_Error_Quantity_Supp_Or_Egual, newQuantitePossible);

                        context.AddFailure("Quantite_" + reception.DepenseId, errorMessage);
                        context.AddFailure("Quantite_" + reception.DepenseId, BusinessResources.Reception_Quantity_Rules_Validator_GFTP_Error_Quantity_Positive);
                        return;
                    }

                    if (ToBeDelete(commandeLigneQuantiteNegative, reception))
                    {
                        context.AddFailure("Quantite_" + reception.DepenseId, BusinessResources.Reception_Quantity_Rules_Validator_GFTP_Error_Quantity_Positive);
                        return;
                    }

                    throw new Framework.Exceptions.FredBusinessException(ErrorMessage);

                }
            }
        }

        private bool ToBeDelete(CommandeLigneQuantiteNegativeModel commandeLigneQuantiteNegative, DepenseAchatEnt reception)
        {
            return commandeLigneQuantiteNegative.NewReceptionQuantites.FirstOrDefault(x => x.ReceptionId == reception.DepenseId) == null && commandeLigneQuantiteNegative.ReceptionQuantites.FirstOrDefault(x => x.ReceptionId == reception.DepenseId) != null;
        }

        private bool ToBeUpdated(CommandeLigneQuantiteNegativeModel commandeLigneQuantiteNegative, DepenseAchatEnt reception)
        {
            return commandeLigneQuantiteNegative.NewReceptionQuantites.FirstOrDefault(x => x.ReceptionId == reception.DepenseId) != null && commandeLigneQuantiteNegative.ReceptionQuantites.FirstOrDefault(x => x.ReceptionId == reception.DepenseId) != null;
        }

        private bool ToBeAdded(DepenseAchatEnt reception)
        {
            return reception.DepenseId == 0;
        }

        private void CreateReceptionQuantiteModelsWithNewValues(List<CommandeLigneQuantiteNegativeModel> commandeLigneQuantiteNegativeModels, ReceptionsValidationModel receptionsForValidate)
        {
            foreach (CommandeLigneQuantiteNegativeModel commandeLigneQuantiteNegativeModel in commandeLigneQuantiteNegativeModels)
            {
                List<int> receptionIdsToUpdate = receptionsForValidate.ReceptionToAddOrUpdates.Select(x => x.DepenseId).ToList();

                CreateReceptionQuantiteModelForUpdates(commandeLigneQuantiteNegativeModel, receptionsForValidate.ReceptionToAddOrUpdates, receptionIdsToUpdate);

                CreateReceptionQuantiteModelForNotModifieds(commandeLigneQuantiteNegativeModel, receptionIdsToUpdate);

                CreateReceptionQuantiteModelForNews(commandeLigneQuantiteNegativeModel, receptionsForValidate.ReceptionToAddOrUpdates);

                DeleteReceptionQuantiteModelForDeletes(commandeLigneQuantiteNegativeModel, receptionsForValidate.ReceptionToDeletes);
            }
        }

        private void CreateReceptionQuantiteModelForUpdates(CommandeLigneQuantiteNegativeModel commandeLigneQuantiteNegativeModel, List<DepenseAchatEnt> receptionsToAddOrUpdate, List<int> receptionIdsToUpdate)
        {
            if (commandeLigneQuantiteNegativeModel == null)
                throw new ArgumentNullException(nameof(commandeLigneQuantiteNegativeModel));

            if (receptionsToAddOrUpdate == null)
                throw new ArgumentNullException(nameof(receptionsToAddOrUpdate));

            if (receptionIdsToUpdate == null)
                throw new ArgumentNullException(nameof(receptionIdsToUpdate));

            if (commandeLigneQuantiteNegativeModel.ReceptionQuantites == null)
            {
                return;
            }

            List<ReceptionQuantiteModel> updates = commandeLigneQuantiteNegativeModel.ReceptionQuantites.Where(x => receptionIdsToUpdate.Contains(x.ReceptionId)).ToList();
            foreach (ReceptionQuantiteModel quantiteNegativeModel in updates)
            {
                DepenseAchatEnt receptionWithNewQuantity = receptionsToAddOrUpdate.FirstOrDefault(x => x.DepenseId == quantiteNegativeModel.ReceptionId);
                ReceptionQuantiteModel newQuantiteOfReceptionModel = new ReceptionQuantiteModel();
                newQuantiteOfReceptionModel.ReceptionId = quantiteNegativeModel.ReceptionId;
                newQuantiteOfReceptionModel.Quantity = receptionWithNewQuantity.Quantite;
                commandeLigneQuantiteNegativeModel.NewReceptionQuantites.Add(newQuantiteOfReceptionModel);
            }
        }

        private void CreateReceptionQuantiteModelForNotModifieds(CommandeLigneQuantiteNegativeModel commandeLigneQuantiteNegativeModel, List<int> receptionIdsToUpdate)
        {
            if (commandeLigneQuantiteNegativeModel == null)
                throw new ArgumentNullException(nameof(commandeLigneQuantiteNegativeModel));

            if (receptionIdsToUpdate == null)
                throw new ArgumentNullException(nameof(receptionIdsToUpdate));

            if (commandeLigneQuantiteNegativeModel.ReceptionQuantites == null)
            {
                return;
            }

            List<ReceptionQuantiteModel> notModifieds = commandeLigneQuantiteNegativeModel.ReceptionQuantites.Where(x => !receptionIdsToUpdate.Contains(x.ReceptionId)).ToList();

            foreach (ReceptionQuantiteModel quantiteNegativeModel in notModifieds)
            {
                ReceptionQuantiteModel newQuantiteOfReceptionModel = new ReceptionQuantiteModel();
                newQuantiteOfReceptionModel.ReceptionId = quantiteNegativeModel.ReceptionId;
                newQuantiteOfReceptionModel.Quantity = quantiteNegativeModel.Quantity;
                commandeLigneQuantiteNegativeModel.NewReceptionQuantites.Add(newQuantiteOfReceptionModel);
            }
        }

        private void CreateReceptionQuantiteModelForNews(CommandeLigneQuantiteNegativeModel commandeLigneQuantiteNegativeModel, List<DepenseAchatEnt> receptionsToAddOrUpdate)
        {
            if (commandeLigneQuantiteNegativeModel == null)
                throw new ArgumentNullException(nameof(commandeLigneQuantiteNegativeModel));

            List<DepenseAchatEnt> newReceptions = receptionsToAddOrUpdate.Where(x => x.DepenseId == 0 && x.CommandeLigneId == commandeLigneQuantiteNegativeModel.CommandeLigneId).ToList();
            foreach (DepenseAchatEnt newReception in newReceptions)
            {
                // nouvelle reception                 
                ReceptionQuantiteModel newQuantiteOfReceptionModel = new ReceptionQuantiteModel();
                newQuantiteOfReceptionModel.ReceptionId = newReception.DepenseId;
                newQuantiteOfReceptionModel.Quantity = newReception.Quantite;
                commandeLigneQuantiteNegativeModel.NewReceptionQuantites.Add(newQuantiteOfReceptionModel);
            }
        }

        private void DeleteReceptionQuantiteModelForDeletes(CommandeLigneQuantiteNegativeModel commandeLigneQuantiteNegativeModel, List<DepenseAchatEnt> receptionToDeletes)
        {
            if (commandeLigneQuantiteNegativeModel == null)
                throw new ArgumentNullException(nameof(commandeLigneQuantiteNegativeModel));
            List<DepenseAchatEnt> deletedReceptions = receptionToDeletes.Where(x => x.CommandeLigneId == commandeLigneQuantiteNegativeModel.CommandeLigneId).ToList();
            foreach (DepenseAchatEnt newReception in deletedReceptions)
            {
                // nouvelle reception                 
                ReceptionQuantiteModel newQuantiteOfReceptionModel = commandeLigneQuantiteNegativeModel.NewReceptionQuantites.First(x => x.ReceptionId == newReception.DepenseId);
                commandeLigneQuantiteNegativeModel.NewReceptionQuantites.Remove(newQuantiteOfReceptionModel);
            }
        }
        private void CalculateQuantityReceptionneeOfCommandeLignes(List<CommandeLigneQuantiteNegativeModel> commandeLigneQuantiteNegativeModels)
        {
            foreach (CommandeLigneQuantiteNegativeModel commandeLigneQuantiteNegativeModel in commandeLigneQuantiteNegativeModels)
            {
                commandeLigneQuantiteNegativeModel.NewQuantiteReceptionnee = commandeLigneQuantiteNegativeModel.NewReceptionQuantites.Sum(x => x.Quantity);
            }
        }

        private List<CommandeLigneQuantiteNegativeModel> GetInfosForValidateReceptionsNegatives(ReceptionsValidationModel receptionsForValidate)
        {
            List<int> commandesLigneToAddOrUpdateIds = receptionsForValidate.ReceptionToAddOrUpdates.Where(x => x.CommandeLigneId.HasValue).Select(x => x.CommandeLigneId.Value).ToList();
            List<int> commandesLigneToDeleteIds = receptionsForValidate.ReceptionToDeletes.Where(x => x.CommandeLigneId.HasValue).Select(x => x.CommandeLigneId.Value).ToList();
            List<int> commandesLigneIds = commandesLigneToAddOrUpdateIds.Concat(commandesLigneToDeleteIds).ToList();
            return commandeLignesRepository.GetCommandeLignesWithReceptionsQuantities(commandesLigneIds);
        }
    }
}
