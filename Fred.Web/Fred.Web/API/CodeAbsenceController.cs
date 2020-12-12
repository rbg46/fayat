using AutoMapper;
using Fred.Business.Referential.CodeAbsence;
using Fred.Business.Societe;
using Fred.Entities.Referential;
using Fred.Web.Models.CodeAbsence;
using Fred.Web.Models.Societe;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Fred.Web.API
{
    /// <summary>
    /// Controller WEB API des codes d'absence
    /// </summary>
    public class CodeAbsenceController : ApiControllerBase
    {
        /// <summary>
        /// Manager business codes d'absence
        /// </summary>
        protected readonly ICodeAbsenceManager codeAbsMgr;

        /// <summary>
        /// Manager business sociétés
        /// </summary>
        protected readonly ISocieteManager societeMgr;

        /// <summary>
        /// Mapper Model / ModelVue
        /// </summary>
        protected readonly IMapper Mapper;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="CodeAbsenceController" />.
        /// </summary>
        /// <param name="codeAbsMgr">Manager de fournisseurs</param>
        /// <param name="societeMgr">Manager des sociétés</param>
        /// <param name="mapper">Mapper Model / ModelVue</param>
        public CodeAbsenceController(ICodeAbsenceManager codeAbsMgr, ISocieteManager societeMgr, IMapper mapper)
        {
            this.codeAbsMgr = codeAbsMgr;
            this.societeMgr = societeMgr;
            this.Mapper = mapper;
        }

        /// <summary>
        /// Méthode GET de récupération de toutes les sociétés.
        /// </summary>
        /// <returns>Retourne la liste de toutes les sociétés</returns>
        [HttpGet]
        [Route("api/CodeAbsence/GetSocieteAll/")]
        public HttpResponseMessage GetSocieteAll()
        {
            try
            {
                var societes = this.Mapper.Map<IEnumerable<SocieteModel>>(this.societeMgr.GetSocieteListAll());
                return Request.CreateResponse(HttpStatusCode.OK, societes);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode GET de récupération de tous les codes absence par societe id.
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>Retourne la liste de tous les codes absences</returns>
        [HttpGet]
        [Route("api/CodeAbsence/GetCodeAbsenceBySocieteId{societeId}")]
        public HttpResponseMessage GetCodeAbsenceBySocieteId(int societeId)
        {
            try
            {
                var codeAbsences = this.Mapper.Map<IEnumerable<CodeAbsenceModel>>(this.codeAbsMgr.GetCodeAbsenceBySocieteId(societeId));
                return Request.CreateResponse(HttpStatusCode.OK, codeAbsences);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [Route("api/CodeAbsence/")]
        public HttpResponseMessage Post(CodeAbsenceModel codeAbsenceModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    this.codeAbsMgr.AddCodeAbsence(this.Mapper.Map<CodeAbsenceEnt>(codeAbsenceModel));
                    return Request.CreateResponse(HttpStatusCode.Created, codeAbsenceModel);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// DELETE api/controller/5
        /// </summary>
        /// <param name="codeAbsence">code absence à supprimer</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/CodeAbsence/Delete")]
        public HttpResponseMessage Delete(CodeAbsenceModel codeAbsence)
        {
            return this.Delete(() => this.codeAbsMgr.DeleteCodeAbsenceById(this.Mapper.Map<CodeAbsenceEnt>(codeAbsence)));
        }

        /// <summary>
        /// PUT api/controller/5
        /// </summary>
        /// <param name="codeAbs">Code absence à traiter</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPut]
        [Route("api/CodeAbsence/")]
        public HttpResponseMessage Put(CodeAbsenceModel codeAbs)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
                }

                this.codeAbsMgr.UpdateCodeAbsence(this.Mapper.Map<CodeAbsenceEnt>(codeAbs));
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [Route("api/CodeAbsence/CheckExistsCode/{codeCodeAbsence}/{idCourant}/{societeId}/")]
        public HttpResponseMessage IsSocieteExistsBySocieteId(string codeCodeAbsence, int idCourant, int societeId)
        {
            try
            {
                var exists = this.codeAbsMgr.IsCodeAbsenceExistsByCode(idCourant, codeCodeAbsence, societeId);
                return Request.CreateResponse(HttpStatusCode.OK, exists);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode GET de récupération d'une nouvelle instance code absence intialisée.
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>Retourne une nouvelle instance code absence intialisée</returns>
        [HttpGet]
        [Route("api/CodeAbsence/New/{societeId}")]
        public HttpResponseMessage New(int societeId)
        {
            try
            {
                var societe = this.Mapper.Map<CodeAbsenceModel>(this.codeAbsMgr.GetNewCodeAbsence(societeId));
                return Request.CreateResponse(HttpStatusCode.OK, societe);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode GET de recherche des codes absences
        /// </summary>
        /// <param name="filters">filters</param>
        /// <param name="recherche">recherche</param>
        /// <returns>Retourne la liste des codes absences</returns>
        [HttpPost]
        [Route("api/CodeAbsence/SearchAll/{recherche?}")]
        public HttpResponseMessage SearchAll(SearchSocieteModel filters, string recherche = "")
        {
            var codeAbsence = this.codeAbsMgr.SearchCodeAbsenceAllWithFilters(recherche, this.Mapper.Map<SearchCodeAbsenceEnt>(filters));
            return Request.CreateResponse(HttpStatusCode.OK, this.Mapper.Map<IEnumerable<CodeAbsenceModel>>(codeAbsence));
        }

        /// <summary>
        /// Méthode GET de recherche des CodeAbsences
        /// </summary>
        /// <param name="filters">filters</param>
        /// <param name="recherche">recherche</param>
        /// <returns>Retourne la liste des CodeAbsences</returns>
        [HttpPost]
        [Route("api/CodeAbsence/Search/{recherche?}")]
        public HttpResponseMessage Search(SearchSocieteModel filters, string recherche = "")
        {
            try
            {
                var societe = this.codeAbsMgr.SearchCodeAbsenceWithFilters(recherche, this.Mapper.Map<SearchCodeAbsenceEnt>(filters));
                return Request.CreateResponse(HttpStatusCode.OK, this.Mapper.Map<IEnumerable<SocieteModel>>(societe));
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode GET de recherche des CodeAbsences
        /// </summary>
        /// <param name="filters">filters</param>
        /// <param name="societeId">societeId</param>
        /// <param name="recherche">recherche</param>
        /// <returns>Retourne la liste des CodeAbsences</returns>
        [HttpPost]
        [Route("api/CodeAbsence/SearchCodeAbsenceAllBySocieteId/{societeId}/{recherche?}")]
        public HttpResponseMessage SearchCodeAbsenceAllBySocieteId(SearchCodeAbsenceModel filters, int societeId, string recherche = "")
        {
            try
            {
                var codeAbsences = this.codeAbsMgr.SearchCodeAbsenceAllBySocieteIdWithFilters(this.Mapper.Map<SearchCodeAbsenceEnt>(filters), societeId, recherche);
                return Request.CreateResponse(HttpStatusCode.OK, this.Mapper.Map<IEnumerable<CodeAbsenceModel>>(codeAbsences));
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode POST de récupération des filtres de recherche sur CodeAbsence
        /// </summary>
        /// <returns>Retourne la liste des filtres de recherche sur CodeAbsence</returns>
        [HttpGet]
        [Route("api/CodeAbsence/Filter/")]
        public HttpResponseMessage Filters()
        {
            try
            {
                var filters = this.Mapper.Map<SearchCodeAbsenceModel>(this.codeAbsMgr.GetDefaultFilter());
                return Request.CreateResponse(HttpStatusCode.OK, filters);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Rechercher les référentiels CodeDeplacement
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <param name="societeId">Identifiant du societe</param>
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/CodeAbsence/SearchLight/{page?}/{pageSize?}/{recherche?}/{ciId?}/{societeId?}/{isOuvrier?}/{isEtam?}/{isCadre?}")]

        public HttpResponseMessage SearchLight(int? ciId = 0, int page = 1,
                                               int pageSize = 20, 
                                               string recherche = "",
                                               int? societeId = 0,
                                               bool? isOuvrier = null,
                                               bool? isEtam = null,
                                               bool? isCadre = null)
        {
            return this.Get(() => this.Mapper.Map<IEnumerable<CodeAbsenceModel>>(this.codeAbsMgr.SearchLight(recherche, page, pageSize, ciId.Value, societeId.Value,isOuvrier,isEtam,isCadre)));
        }

        /// <summary>
        ///  Verifie si l'entité est déja utilisée.
        /// </summary>
        /// <param name="codeAbsenceId">codeAbsenceId</param>  
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/CodeAbsence/IsAlreadyUsed/{codeAbsenceId}")]
        public HttpResponseMessage IsAlreadyUsed(int codeAbsenceId)
        {
            return Get(() => new
            {
                id = codeAbsenceId,
                isAlreadyUsed = this.codeAbsMgr.IsAlreadyUsed(codeAbsenceId)
            });
        }
    }
}
