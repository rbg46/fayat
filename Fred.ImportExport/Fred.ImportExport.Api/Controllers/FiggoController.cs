using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fred.ImportExport.Business.Figgo;
using Fred.ImportExport.Models.Figgo;

namespace Fred.ImportExport.Api.Controllers
{
    /// <summary>
    /// Controller Asp.Net des transferts des absence ci depuis Figgo
    /// </summary>  
    public class FiggoController : ApiControllerBase
    {
        private readonly FiggoManager figgoManager;

        public FiggoController(FiggoManager figgoManager)
        {
            this.figgoManager = figgoManager;
        }

        /// <summary>
        /// Importation des Pointages D'absence depuis Figgo
        /// </summary>
        /// <param name="absenceFiggomodel">Model contenant l'absence Ci</param>
        /// <returns>Une reponse HTTP.</returns>
        /// <response code="200">Succès de la requête .</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpPost]
        [Route("api/Figgo/ImportAbsenceFiggo")]

        public async Task<IHttpActionResult> ImportAbsenceFiggoAsync(FiggoAbsenceModel absenceFiggomodel)
        {
            JsonErrorFiggo result = await figgoManager.ImportAbsenceFiggoAsync(absenceFiggomodel);
            return Created(string.Empty, result);
        }
    }
}
