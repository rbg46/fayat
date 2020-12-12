using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Business.Commande;
using Fred.DataAccess.ExternalService.FredImportExport.Commande;
using Fred.Entities.Models;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.Models.Commande;

namespace Fred.Business.ExternalService.Materiel
{
    /// <summary>
    /// Gestionnaire externe des commandes
    /// </summary>
    public class CommandeManagerExterne : ICommandeManagerExterne
    {
        private readonly ICommandeRepositoryExterne commandRepo;
        protected readonly ICommandeManager commandeMgr;

        public CommandeManagerExterne(ICommandeRepositoryExterne commandRepo, ICommandeManager commandeMgr)
        {
            this.commandRepo = commandRepo;
            this.commandeMgr = commandeMgr;
        }

        /// <inheritdoc />
        public async Task<ResultModel<string>> ExportCommandeToSapAsync(int commandeId)
        {
            ResultModel<string> hangfireJobIdResult = null;
            try
            {
                // On envoie la commande validée à SAP et on sauvegarde l'idenfiant du job Hangfire.
                hangfireJobIdResult = await commandRepo.ExportCommandeToSapAsync(commandeId);
                if (hangfireJobIdResult.Success)
                {
                    commandeMgr.UpdateHangfireJobId(commandeId, hangfireJobIdResult.Value);
                }
                return hangfireJobIdResult;
            }
            catch (FredRepositoryException fre)
            {
                throw new FredBusinessException(fre.Message, fre.InnerException);
            }
        }

        /// <inheritdoc />
        public async Task<ResultModel<string>> ExportCommandeAvenantToSapAsync(int commandeId, int numeroAvenant)
        {
            try
            {
                // On envoie la commande validée à SAP et on sauvegarde l'idenfiant du job Hangfire.
                var hangfireJobIdResult = await commandRepo.ExportCommandeAvenantToSapAsync(commandeId, numeroAvenant);
                if (hangfireJobIdResult.Success)
                {
                    commandeMgr.UpdateAvenantHangfireJobId(commandeId, numeroAvenant, hangfireJobIdResult.Value);
                }
                return hangfireJobIdResult;
            }
            catch (FredRepositoryException fre)
            {
                throw new FredBusinessException(fre.Message, fre.InnerException);
            }
        }

        /// <inheritdoc />
        public async Task<CommandeReturnToSap.ResultModel> ReturnCommandeToSapAsync(int commandeId)
        {
            bool returnCommande;
            IEnumerable<int> returnNumeroAvenants;
            commandeMgr.GetItemsToReturnToSap(commandeId, out returnCommande, out returnNumeroAvenants);

            var ret = new CommandeReturnToSap.ResultModel();

            if (returnCommande)
            {
                ret.CommandeTraitee = true;
                try
                {
                    var hangfireJobIdResult = await ExportCommandeToSapAsync(commandeId);
                    if (hangfireJobIdResult.Success)
                    {
                        ret.CommandeHangfireJobId = hangfireJobIdResult.Value;
                    }
                }
                catch
                {
                    // Rien à faire de plus                
                }
            }

            foreach (var numeroAvenant in returnNumeroAvenants)
            {
                try
                {
                    var hangfireJobIdResult = await ExportCommandeAvenantToSapAsync(commandeId, numeroAvenant);
                    if (hangfireJobIdResult.Success)
                    {
                        ret.Avenants.Add(new CommandeReturnToSap.AvenantModel(numeroAvenant, hangfireJobIdResult.Value));
                    }

                }
                catch
                {
                    // Rien à faire de plus                
                }
            }

            return ret;
        }
    }
}
