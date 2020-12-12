using Fred.ImportExport.Business.CommandeLigne.VME22;
using Fred.ImportExport.Business.Hangfire.Parameters;
using Fred.Web.Shared.Models.Commande;
using System.Web.Http;

namespace Fred.ImportExport.Api.Controllers
{

    /// <summary>
    /// Controller Web API des lignes de commandes
    /// </summary>    
    public class CommandeLigneController : ApiControllerBase
    {
        private readonly IVme22FluxManager vme22FluxManager;

        public CommandeLigneController(IVme22FluxManager vme22FluxManager)
        {
            this.vme22FluxManager = vme22FluxManager;
        }

        /// <summary>
        /// Exporter le vérouillage / déverouillage manuel d'une ligne de commande vers SAP
        /// Api interne fred web - fred Ie / Fonctionnalité demandée initialement pour FTP  
        /// </summary>
        /// <param name="model">model</param>      
        /// <returns>IHttpActionResult</returns>
        [HttpPost]
        [Authorize(Users = "userserviceFTP", Roles = "service")]
        [Route("api/CommandeLigne/ExportManualLockLigneDeCommandeToSap/")]
        public IHttpActionResult ExportManualLockLigneDeCommandeToSap([FromBody] ExportManualLockUnlockLigneDeCommandeToSapModel model)
        {
            var parameters = new ExportManualLockUnlockLigneDeCommandeParameters()
            {
                AuteurModificationId = model.UtilisateurId,
                AuteurModificationSocieteId = model.UtilisateurSocieteId,
                AuteurModificationGroupeCode = model.UtilisateurGroupeCode,
                CommandeLigneId = model.CommandeLigneId,
                DateVerouillage = model.Date,
                IsLocked = true
            };

            vme22FluxManager.EnqueueExportManualLockUnlockLigneDeCommande(parameters);

            return this.Ok();
        }

        /// <summary>
        /// Exporter le vérouillage / déverouillage manuel d'une ligne de commande vers SAP
        /// Api interne fred web - fred Ie / Fonctionnalité demandée initialement pour FTP  
        /// </summary>
        /// <param name="model">parametres</param>
        /// <returns>IHttpActionResult</returns>
        [HttpPost]
        [Authorize(Users = "userserviceFTP", Roles = "service")]
        [Route("api/CommandeLigne/ExportManualUnlockLigneDeCommandeToSap/")]
        public IHttpActionResult ExportManualUnlockLigneDeCommandeToSap([FromBody] ExportManualLockUnlockLigneDeCommandeToSapModel model)
        {
            var parameters = new ExportManualLockUnlockLigneDeCommandeParameters()
            {
                AuteurModificationId = model.UtilisateurId,
                AuteurModificationSocieteId = model.UtilisateurSocieteId,
                AuteurModificationGroupeCode = model.UtilisateurGroupeCode,
                CommandeLigneId = model.CommandeLigneId,
                DateVerouillage = model.Date,
                IsLocked = false
            };
            vme22FluxManager.EnqueueExportManualLockUnlockLigneDeCommande(parameters);

            return this.Ok();
        }
    }
}
