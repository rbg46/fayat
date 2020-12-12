using AutoMapper;
using Fred.Business.Referential.CodeDeplacement;
using Fred.Entities.Referential;
using Fred.Web.Models.Referential;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace Fred.Web.API
{
    /// <summary>
    ///   Controller WEB API des codesDeplacement
    /// </summary>
    public class CodeDeplacementController : ApiControllerBase
    {
        /// <summary>
        ///   Manager business des codesDeplacement
        /// </summary>
        private readonly ICodeDeplacementManager codeDeplacementMgr;

        /// <summary>
        ///   Mapper Model / ModelVue
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="CodeDeplacementController" />.
        /// </summary>
        /// <param name="codeDeplacementMgr">Manager de codesDeplacement</param>
        /// <param name="mapper">Mapper Model / ModelVue</param>
        public CodeDeplacementController(ICodeDeplacementManager codeDeplacementMgr, IMapper mapper)
        {
            this.codeDeplacementMgr = codeDeplacementMgr;
            this.mapper = mapper;
        }

        /// <summary>
        ///   Méthode GET de vérification d'existance d'un code pour une société donnée.
        ///   <param name="code">Code a rechercher</param>
        ///   <param name="societeId">Id de la société</param>
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="code">code</param>
        /// <returns>Le codeDeplacement pour un code et un societeId</returns>
        [HttpGet]
        [Route("api/CodeDeplacement/GetBySocieteIdAndCode")]
        public HttpResponseMessage GetBySocieteIdAndCode([FromUri] int societeId, [FromUri] string code)
        {
            return Get(() => this.codeDeplacementMgr.GetBySocieteIdAndCode(societeId, code));
        }

        /// <summary>
        ///   Méthode GET de récupération d'une nouvelle instance code déplacement intialisée.
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>Retourne une nouvelle instance code déplacement intialisée</returns>
        [HttpGet]
        [Route("api/CodeDeplacement/New/{societeId}")]
        public HttpResponseMessage New(int societeId)
        {
            return Get(() => this.mapper.Map<CodeDeplacementModel>(this.codeDeplacementMgr.GetNewCodeDeplacement(societeId)));
        }

        /// <summary>
        ///   Méthode GET de recherche des sociétés
        /// </summary>
        /// <param name="filters">filters</param>
        /// <param name="societeId">societeId</param>
        /// <param name="recherche">recherche</param>
        /// <returns>Retourne la liste des sociétés</returns>
        [HttpPost]
        [Route("api/CodeDeplacement/SearchAll/{societeId}/{recherche?}")]
        public HttpResponseMessage SearchAll(SearchCodeDeplacementModel filters, int societeId, string recherche = "")
        {
            //ATTENTION ICI LA METHODE HTTP EST UN POST MAIS ON ATTEND UNE LISTE D ELEMENT 
            //DONC JE RETOURNE UN GET. LA METHODE DE BASE PUT RENVOIT LE STATUS CODE CREATED
            return Get(() =>
            {
                var codesDeplacement = this.codeDeplacementMgr.SearchCodeDepAllWithFilters(societeId, recherche, this.mapper.Map<SearchCodeDeplacementEnt>(filters));

                return this.mapper.Map<IEnumerable<CodeDeplacementModel>>(codesDeplacement);
            });
        }

        /// <summary>
        ///   Rechercher les référentiels CodeDeplacement
        /// </summary>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/CodeDeplacement/SearchLight/{page?}/{pageSize?}/{recherche?}/{ciId?}/{isOuvrier?}/{isEtam?}/{isCadre?}")]
        public HttpResponseMessage SearchLight(int? ciId, int page = 1, int pageSize = 20, string recherche = "", bool? isOuvrier = null, bool? isEtam = null, bool? isCadre = null)
        {
            return Get(() => this.mapper.Map<IEnumerable<CodeDeplacementModel>>(this.codeDeplacementMgr.SearchLight(recherche, page, pageSize, ciId, isOuvrier, isEtam, isCadre)));
        }

        /// <summary>
        ///   Méthode POST de création d'un codesDeplacement
        /// </summary>
        /// <param name="codeDep">codeDep</param>
        /// <returns>Retourne le code Deplacement créé</returns>
        [HttpPost]
        [Route("api/CodeDeplacement")]
        public HttpResponseMessage Post(CodeDeplacementModel codeDep)
        {
            //Une methode POST doit retourner l'object créé.
            return Post(() => this.codeDeplacementMgr.AddCodeDeplacement(this.mapper.Map<CodeDeplacementEnt>(codeDep)));
        }

        /// <summary>
        ///   Méthode PUT de mise à jour des codesDeplacement
        /// </summary>
        /// <param name="codeDep">codeDep</param>
        /// <returns>Retourne le code Deplacement modifié</returns>
        [HttpPut]
        [Route("api/CodeDeplacement/")]
        public HttpResponseMessage Put(CodeDeplacementModel codeDep)
        {
            //Une methode PUT doit retourner l'object mis a jour.
            return Put(() => this.codeDeplacementMgr.UpdateCodeDeplacement(this.mapper.Map<CodeDeplacementEnt>(codeDep)));
        }

        /// <summary>
        ///   DELETE api/controller/5
        /// </summary>
        /// <param name="codeDeplacementId">ID du code deplacement</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpDelete]
        [Route("api/CodeDeplacement/{codeDeplacementId}")]
        public HttpResponseMessage Delete(int codeDeplacementId)
        {
            return Delete(() => this.codeDeplacementMgr.DeleteCodeDeplacementById(codeDeplacementId));
        }

        /// <summary>
        ///  Verifie si l'entité est déja utilisée.
        /// </summary>
        /// <param name="codeDeplacementId">codeDeplacementId</param>  
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/CodeDeplacement/IsAlreadyUsed/{codeDeplacementId}")]
        public HttpResponseMessage IsAlreadyUsed(int codeDeplacementId)
        {
            return Get(() => new
            {
                id = codeDeplacementId,
                isAlreadyUsed = this.codeDeplacementMgr.IsAlreadyUsed(codeDeplacementId)
            });
        }
    }
}
