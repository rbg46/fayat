using System;
using System.Threading.Tasks;
using Fred.Business.Utilisateur;
using Fred.DataAccess.ExternalService.FredImportExport.CommandeLigne;
using Fred.Web.Shared.Models.Commande;

namespace Fred.Business.ExternalService.CommandeLigne
{
    public class CommandeLigneManagerExterne : ICommandeLigneManagerExterne
    {
        private readonly ICommandeLigneRepositoryExterne commandeLigneRepositoryExterne;
        private readonly IUtilisateurManager utilisateurManager;

        public CommandeLigneManagerExterne(ICommandeLigneRepositoryExterne commandeLigneRepositoryExterne,
            IUtilisateurManager utilisateurManager)
        {
            this.commandeLigneRepositoryExterne = commandeLigneRepositoryExterne;
            this.utilisateurManager = utilisateurManager;
        }

        public async Task ExportManualLockLigneDeCommandeToSap(int commandeId, Func<Task> onLockOnFredWeb)
        {
            await onLockOnFredWeb();

            ExportManualLockUnlockLigneDeCommandeToSapModel request = await CreateRequestAsync(commandeId);

            await this.commandeLigneRepositoryExterne.ExportManualLockLigneDeCommandeToSapAsync(request);
        }

        public async Task ExportManualUnlockLigneDeCommandeToSap(int commandeId, Func<Task> onUnLockOnFredWeb)
        {
            await onUnLockOnFredWeb();

            ExportManualLockUnlockLigneDeCommandeToSapModel request = await CreateRequestAsync(commandeId);

            await this.commandeLigneRepositoryExterne.ExportManualUnLockLigneDeCommandeToSapAsync(request);
        }

        private async Task<ExportManualLockUnlockLigneDeCommandeToSapModel> CreateRequestAsync(int commandeId)
        {
            var user = await utilisateurManager.GetContextUtilisateurAsync();

            var request = new ExportManualLockUnlockLigneDeCommandeToSapModel()
            {
                UtilisateurId = user.UtilisateurId,
                UtilisateurGroupeCode = user.Personnel.Societe.Groupe.Code,
                UtilisateurSocieteId = user.Personnel.SocieteId.Value,
                CommandeLigneId = commandeId,
                Date = DateTime.UtcNow
            };
            return request;
        }
    }
}
