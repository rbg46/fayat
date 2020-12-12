using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Fred.Business.Commande;
using Fred.Business.CommandeEnergies;
using Fred.Business.Reception;
using Fred.DataAccess.ExternalService.FredImportExport.Commande;
using Fred.Entities.Commande;
using Fred.Entities.Models;
using Fred.Framework.Exceptions;

namespace Fred.Business.ExternalService
{
    /// <summary>
    /// Gestionnaire externe des commandes Energie
    /// </summary>
    public class CommandeEnergieManagerExterne : ICommandeEnergieManagerExterne
    {
        private readonly ICommandeRepositoryExterne commandeRepoExterne;
        private readonly ICommandeEnergieManager commandeEnergieManager;
        private readonly IStatutCommandeManager statutCommandeManager;
        private readonly ICommandeEnergieService commandeEnergieService;
        private readonly IReceptionManagerExterne receptionManagerExterne;
        private readonly ICommandeManager commandeManager;
        private readonly IReceptionManager receptionManager;

        public CommandeEnergieManagerExterne(ICommandeRepositoryExterne commandeRepoExterne,
            ICommandeEnergieManager commandeEnergieManager,
            IStatutCommandeManager statutCommandeManager,
            ICommandeEnergieService commandeEnergieService,
            IReceptionManagerExterne receptionManagerExterne,
            ICommandeManager commandeManager,
            IReceptionManager receptionManager)
        {
            this.commandeRepoExterne = commandeRepoExterne;
            this.commandeEnergieManager = commandeEnergieManager;
            this.statutCommandeManager = statutCommandeManager;
            this.commandeEnergieService = commandeEnergieService;
            this.receptionManagerExterne = receptionManagerExterne;
            this.commandeManager = commandeManager;
            this.receptionManager = receptionManager;
        }

        /// <summary>
        /// Process de validation d'une commande énergie : RG_5438_030 : Validation de la commande d’Energies
        /// 1) Enregirement en BD
        /// 2) Envoi de la commande à SAP
        /// 3) Reception auto à 100% de la commande et envoi des réceptions à SAP
        /// 4) Clôturer la commande
        /// </summary>
        /// <param name="commandeEnergie">Commande énergie</param>
        /// <returns>L'identifiant du job Hangfire.</returns>
        public async Task<bool> ValidateAsync(CommandeEnergie commandeEnergie)
        {
            try
            {

                CommandeEnt commande = null;

                if (commandeEnergie.StatutCommande.Code != StatutCommandeEnt.CommandeStatutCL)
                {
                    if (commandeEnergie.StatutCommande.Code != StatutCommandeEnt.CommandeStatutVA)
                    {
                        // Etape 1 : Mise à jour de la commande (stop en cas d'échec)
                        // Exception gérée par le validator du business 'CommandeEnergieManager'
                        commandeEnergie = commandeEnergieManager.Update(commandeEnergie);

                        commande = commandeManager.FindById(commandeEnergie.CommandeId);

                        // Etape 2 : Envoie de la commande vers SAP (stop en cas d'échec)
                        await ProcessCommandeAsync(commande);
                    }

                    commande = commandeManager.FindById(commandeEnergie.CommandeId);

                    // Etape 3 : Création des réceptions et export vers SAP (continue en cas d'échec)
                    await ProcessReceptionAsync(commande);
                }

                return true;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (FredBusinessException)
            {
                throw;
            }
            catch (FredRepositoryException fre)
            {
                throw new FredBusinessException(fre.Message, fre);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Exporte la commande vers SAP
        /// - Si succès : commande enregistrée au statut Validé + JobId hangfire enregistré pour la commande
        /// - Si échec : Exception avec erreur FRED IE
        /// </summary>
        /// <param name="commande">Commande Energie</param>        
        private async Task ProcessCommandeAsync(CommandeEnt commande)
        {
            try
            {
                if (string.IsNullOrEmpty(commande.HangfireJobId))
                {
                    ResultModel<string> hangfireJobIdResult = await commandeRepoExterne.ExportCommandeToSapAsync(commande.CommandeId);

                    if (hangfireJobIdResult.Success)
                    {
                        commande.HangfireJobId = hangfireJobIdResult.Value;

                        StatutCommandeEnt statutValide = statutCommandeManager.GetByCode(StatutCommandeEnt.CommandeStatutVA);

                        commande.StatutCommandeId = statutValide.StatutCommandeId;

                        List<Expression<Func<CommandeEnt, object>>> fieldsToUpdate = new List<Expression<Func<CommandeEnt, object>>>
                        {
                            x => x.HangfireJobId,
                            x => x.StatutCommandeId
                        };
                        commandeEnergieManager.UpdateFieldsAndSaveWithValidation(commande, fieldsToUpdate);
                    }
                    else
                    {
                        // On lance une exception car il ne faut pas passer à l'étape 3 si l'envoie de la commande à échoué                       
                        throw new ValidationException(hangfireJobIdResult.Error, new List<ValidationFailure>() { new ValidationFailure("STEP_2", "L'export de la commande vers SAP a échoué") });
                    }
                }
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ValidationException(e.Message, new List<ValidationFailure>() { new ValidationFailure("STEP_2", "L'export de la commande vers SAP a échoué") });
            }
        }

        /// <summary>
        /// Exporte les réceptions de la commande énergie vers SAP
        /// - Si succès : Mise à jour de toutes les réceptions avec le jobId hangfire + Clôture de la commande énergie
        /// - Si échec : Exception lancée par "ExportReceptionListToSap"
        /// </summary>
        /// <param name="commande">Commande énergie</param>
        private async Task ProcessReceptionAsync(CommandeEnt commande)
        {
            try
            {
                StatutCommandeEnt statutValide = statutCommandeManager.GetByCode(StatutCommandeEnt.CommandeStatutVA);

                if (commande.StatutCommandeId == statutValide.StatutCommandeId)
                {
                    // Création des réceptions si elle n'existe pas
                    commandeEnergieService.ReceptionAuto(commande.CommandeId);

                    // Récupération des réceptions éligibles à l'export vers SAP 
                    var dbReceptions = commandeEnergieService.GetReceptionsForSap(commande.CommandeId);

                    if (dbReceptions?.Count > 0)
                    {
                        var receptionIds = dbReceptions.Select(r => r.DepenseId).ToList();
                        await receptionManager.ViserReceptionsAsync(receptionIds,
                                callFredIeAndSetHangfireJobIdAction: (receptions) => receptionManagerExterne.ExportReceptionListToSapAsync(receptions));
                        
                    }

                    // Clôture de la commande en cas de succès 
                    if (!commandeEnergieService.GetReceptionsForSap(commande.CommandeId).Any())
                    {
                        StatutCommandeEnt statutCloture = statutCommandeManager.GetByCode(StatutCommandeEnt.CommandeStatutCL);

                        commande.StatutCommandeId = statutCloture.StatutCommandeId;

                        List<Expression<Func<CommandeEnt, object>>> fieldsToUpdate = new List<Expression<Func<CommandeEnt, object>>>
                        {
                            x => x.HangfireJobId,
                            x => x.StatutCommandeId
                        };

                        commandeEnergieManager.UpdateFieldsAndSave(commande, fieldsToUpdate);
                    }
                    else
                    {
                        throw new ValidationException(new List<ValidationFailure>() { new ValidationFailure("STEP_3", "L'export des réceptions vers SAP a échoué") });
                    }
                }
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ValidationException(e.Message, new List<ValidationFailure>() { new ValidationFailure("STEP_3", "L'export des réceptions vers SAP a échoué") });
            }
        }
    }
}
