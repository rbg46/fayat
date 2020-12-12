using Fred.Business.Depense;
using Fred.Web.Models.Depense;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fred.Web.API
{
    public class RemplacementTacheController : ApiControllerBase
    {
        private readonly IRemplacementTacheManager remplacementTacheManager;

        public RemplacementTacheController(IRemplacementTacheManager remplacementTacheManager)
        {
            this.remplacementTacheManager = remplacementTacheManager;
        }

        /// <summary>
        ///   POST Création d'une tâche de remplacement
        /// </summary>
        /// <param name="remplacementTache">Tache de remplacement à ajouter</param>
        /// <returns>Retourne une réponse HTTP</returns>       
        [HttpPost]
        [Route("api/RemplacementTache")]
        public async Task<IHttpActionResult> AddRemplacementTacheAsync(RemplacementTacheModel remplacementTache)
        {
            await remplacementTacheManager.AddAsync(remplacementTache);

            return Ok();
        }

        /// <summary>
        ///   DELETE Suppression d'une dépense
        /// </summary>
        /// <param name="remplacementTId">Identifiant de la tache</param>    
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpDelete]
        [Route("api/RemplacementTache/{remplacementTId}")]
        public async Task<IHttpActionResult> DeleteRemplacementTacheAsync(int remplacementTId)
        {
            await remplacementTacheManager.DeleteByIdAsync(remplacementTId);

            return Ok();
        }

        /// <summary>
        ///   Récupération de l'historique des tâches
        /// </summary>
        /// <param name="groupeRemplacementId">Identifiant de la tache</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpGet]
        [Route("api/RemplacementTacheHistory/{groupeRemplacementId}")]
        public async Task<IHttpActionResult> GetRemplacementTacheHistoryAsync(int groupeRemplacementId)
        {
            return Ok(await remplacementTacheManager.GetHistoryAsync(groupeRemplacementId).ConfigureAwait(false));
        }
    }
}
