using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Fred.ImportExport.Business.Reception.Migo;
using Fred.ImportExport.Business.Reception.RMigo;
using Fred.ImportExport.Models.Depense;

namespace Fred.ImportExport.Api.Controllers
{
    /// <summary>
    /// Controller Asp.Net des transferts des receptions vers SAP
    /// </summary>    
    public class ReceptionController : ApiControllerBase
    {
        private readonly IMigoManager migoManager;
        private readonly IRMigoManager rMigoManager;

        public ReceptionController(IMigoManager migoManager, IRMigoManager rMigoManager)
        {
            this.migoManager = migoManager;
            this.rMigoManager = rMigoManager;
        }

        /// <summary>
        ///  Exporte les receptions vers Sap (Flux MIGO)
        /// </summary>
        /// <param name="ids">La liste des identifants à récupérer</param>
        /// <returns>Une reponse HTTP.</returns>
        /// <response code="200">Succès de la requête .</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpPost]
        [Route("api/Reception/ExportReceptions")]
        [Authorize(Users = "userserviceStorm, userserviceFtp", Roles = "service")]
        public HttpResponseMessage ExportReceptions(List<int> ids)
        {
            return Post(() => migoManager.ManageExportReceptionToSap(ids));
        }

        /// <summary>
        /// Récupération de la réponse de SAP (Retour MIGO) suite à un export de Réceptions
        /// </summary>
        /// <param name="receptions">Liste des réceptions SAP</param>
        /// <returns>Une reponse HTTP.</returns>
        /// <response code="200">Succès de la requête .</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpPost]
        [Route("api/Reception/RetourMIGO")]
        [Authorize(Users = "userserviceStorm, userserviceFtp", Roles = "service")]
        public HttpResponseMessage RetourMigo(List<ReceptionSapModel> receptions)
        {
            return Post(() => rMigoManager.ImportRetourReceptionsFromSap(receptions));
        }
    }
}
