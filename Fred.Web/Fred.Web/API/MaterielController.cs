using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Fred.Business.Referential;
using Fred.Business.Referential.Materiel;
using Fred.Entities.Referential;
using Fred.Web.Models.Referential;
using Fred.Web.Shared.Models.Materiel.Search;
using Fred.Web.Shared.Models.Referential;

namespace Fred.Web.API
{
    /// <summary>
    ///   Controller WEB API des Matériels
    /// </summary>
    public class MaterielController : ApiControllerBase
    {
        private readonly IMaterielManager materielMgr;
        private readonly IMapper mapper;
        protected readonly IFournisseurManager fournisseurManager;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="MaterielController" />.
        /// </summary>
        /// <param name="materielMgr">Gestionnaire des Materiels</param>    
        /// <param name="mapper">Mapper Model / ModelVue</param>
        /// <param name="fournisseurManager">Gestionnaire des fournisseurs</param>
        public MaterielController(IMaterielManager materielMgr, IMapper mapper, IFournisseurManager fournisseurManager)
        {
            this.materielMgr = materielMgr;
            this.fournisseurManager = fournisseurManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Méthode GET de récupération d'un établissement comptable
        /// </summary>
        /// <param name="id">Identifiant du matériel</param>
        /// <returns>Retourne un établissement comptable d'après son ID</returns>
        [HttpGet]
        [Route("api/Materiel/GetMaterielById/{id}")]
        public async Task<IHttpActionResult> GetMaterielById(int id)
        {
            var materiel = await this.materielMgr.GetMaterielDetailByIdAsync(id);
            return Ok(this.mapper.Map<MaterielDetailModel>(materiel));
        }

        /// <summary>
        /// Méthode GET de récupération de tous les codes absence par societe id.
        /// </summary>
        /// <returns>Retourne la liste de tous les codes absences</returns>
        [HttpGet]
        [Route("api/Materiel/GetMaterielAll")]
        public HttpResponseMessage GetMaterielAll()
        {
            return this.Get(() =>
            {
                return this.mapper.Map<IEnumerable<MaterielModel>>(this.materielMgr.GetMaterielListAll());
            });
        }

        [HttpPost]
        [Route("api/Materiel/")]
        public HttpResponseMessage Post(MaterielDetailsPostModel materielModel)
        {
            return this.Post(() =>
            {
                this.materielMgr.AddMateriel(this.mapper.Map<MaterielEnt>(materielModel));
                return materielModel;
            });
        }

        /// <summary>
        /// DELETE api/controller/5
        /// </summary>
        /// <param name="materielModel">Le materiel à supprimer</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Materiel/Delete")]
        public HttpResponseMessage Delete(MaterielModel materielModel)
        {
            if (!this.ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
            }

            if (materielModel != null)
            {
                if (this.materielMgr.DeleteMaterielById(this.mapper.Map<MaterielEnt>(materielModel)))
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// PUT api/controller/5
        /// </summary>
        /// <param name="materielModel">Etablissement Comptable</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPut]
        [Route("api/Materiel/")]
        public HttpResponseMessage Update(MaterielDetailModel materielModel)
        {
            return this.Put(() =>
            {
                return this.mapper.Map<MaterielDetailModel>(this.materielMgr.UpdateMateriel(this.mapper.Map<MaterielEnt>(materielModel)));
            });
        }

        [HttpGet]
        [Route("api/Materiel/CheckExistsCode/{codeMateriel}/{idCourant}/{societeId}/")]
        public HttpResponseMessage IsCodeExistsBySocieteId(string codeMateriel, int idCourant, int societeId)
        {
            return this.Get(() =>
            {
                return this.materielMgr.IsMaterielExistsByCode(idCourant, codeMateriel, societeId);
            });
        }

        /// <summary>
        /// Méthode GET de récupération d'une nouvelle instance établissement comptable intialisée.
        /// </summary>
        /// <returns>Retourne une nouvelle instance  établissement comptable intialisée</returns>
        /// <param name="societeId">Identifiant de la société</param>
        [HttpGet]
        [Route("api/Materiel/New/{societeId}")]
        public HttpResponseMessage New(int societeId)
        {
            return this.Get(() =>
            {
                return this.mapper.Map<MaterielModel>(this.materielMgr.GetNewMateriel(societeId));
            });
        }

        /// <summary>
        ///   GET de recherche des matériels d'une société
        /// </summary>
        /// <returns>Retourne la liste des matériels d'une société</returns>
        [HttpPost]
        [Route("api/Materiel/SearchMateriels/{societeId}/{pageSize?}/{pageIndex?}/{searchText?}")]
        public async Task<IHttpActionResult> SearchMaterielsAsync(int societeId, int pageSize = 25, int pageIndex = 0, string searchText = "")
        {
            var parameter = new MaterielSearchParameter { SocieteId = societeId, SearchText = searchText, PageSize = pageSize, PageIndex = pageIndex };
            IEnumerable<MaterielSearchModel> materiels = await materielMgr.SearchMaterielsAsync(parameter);

            return Ok(materiels);
        }

        /// <summary>
        ///   GET Rechercher Light des matériels d'une société
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="statut">Indique s'il faut retourner les matériel externe ou interne</param>
        /// <param name="includeStorm">Indique s'il faut inclure ou non le matériel STORM. Si null, inclut le matériel STORM.</param>
        /// <returns>retouner une liste de  matériel</returns>
        [HttpGet]
        [Route("api/Materiel/SearchLight/{page?}/{pageSize?}/{recherche?}/{ciId?}/{statut?}")]
        public HttpResponseMessage SearchLight(int page = 1, int pageSize = 20, string recherche = "", int? societeId = null, int? ciId = null, int? statut = null, bool? includeStorm = null)
        {
            bool materielLocation = false;
            if (statut == 2)
            {
                materielLocation = true;
            }

            return this.Get(() => this.mapper.Map<IEnumerable<MaterielModel>>(this.materielMgr.SearchLight(recherche, page, pageSize, societeId, ciId, materielLocation, includeStorm)));
        }

        /// <summary>
        /// Rechercher les référentiels CI
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <param name="groupeId">groupeId</param>
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/Materiel/GetActiveRentersForMateriel/{page?}/{pageSize?}/{recherche?}/{groupeId?}")]
        public HttpResponseMessage GetActiveRentersForMateriel(int page = 1, int pageSize = 20, string recherche = "", int? groupeId = null)
        {
            return this.Get(() =>
            {
                var activeRentersForMateriel = this.fournisseurManager.GetActiveRentersForMateriel(recherche, page, pageSize, groupeId.Value);
                var mappage = this.mapper.Map<IEnumerable<FournisseurModel>>(activeRentersForMateriel);
                return mappage;
            });
        }

        /// <summary>
        ///  Verifie si l'entité est déja utilisée.
        /// </summary>
        /// <param name="materielId">materielId</param>  
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/Materiel/IsAlreadyUsed/{materielId}")]
        public HttpResponseMessage IsAlreadyUsed(int materielId)
        {
            return Get(() => new
            {
                id = materielId,
                isAlreadyUsed = this.materielMgr.IsAlreadyUsed(materielId)
            });
        }
    }
}
