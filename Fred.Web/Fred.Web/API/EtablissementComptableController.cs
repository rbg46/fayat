using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Fred.Business.Organisation;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Entities.Referential;
using Fred.Web.Models.Organisation;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Societe;
using static Fred.Entities.Constantes;

namespace Fred.Web.API
{
    /// <summary>
    /// Controller WEB API des Etablissements Comptables
    /// </summary>
    public class EtablissementComptableController : ApiControllerBase
    {
        /// <summary>
        /// Manager business Etablissements Comptables
        /// </summary>
        private readonly IEtablissementComptableManager etabComptMgr;

        /// <summary>
        /// Manager business sociétés
        /// </summary>
        private readonly ISocieteManager societeMgr;

        /// <summary>
        /// Manager business organisation
        /// </summary>
        private readonly IOrganisationManager orgaMgr;

        /// <summary>
        /// Mapper Model / ModelVue
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Id de l'utilisateur courant
        /// </summary>
        private readonly int utilisateurId;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="EtablissementComptableController" />.
        /// </summary>
        /// <param name="etabComptMgr">Manager des établissements comptables</param>
        /// <param name="societeMgr">Manager de sociétés</param>
        /// <param name="orgaMgr">Manager des organisations</param>
        /// <param name="utilisateurMgr">Manager de l'utilisateur</param>
        /// <param name="mapper">Mapper Model / ModelVue</param>
        public EtablissementComptableController(IEtablissementComptableManager etabComptMgr, ISocieteManager societeMgr, IOrganisationManager orgaMgr, IUtilisateurManager utilisateurMgr, IMapper mapper)
        {
            this.etabComptMgr = etabComptMgr;
            this.societeMgr = societeMgr;
            this.orgaMgr = orgaMgr;
            this.mapper = mapper;
            this.utilisateurId = utilisateurMgr.GetContextUtilisateurId();
        }

        /// <summary>
        /// Méthode GET de récupération des types de commandes
        /// </summary>
        /// <returns>Retourne la liste des types de commandes</returns>
        [HttpGet]
        [Route("api/EtablissementComptable")]
        public IEnumerable<EtablissementComptableModel> Get()
        {
            var etablissementComptable = this.etabComptMgr.GetListWithCGA(this.etabComptMgr.GetEtablissementComptableList());
            return this.mapper.Map<IEnumerable<EtablissementComptableModel>>(etablissementComptable);
        }

        /// <summary>
        /// Méthode GET de récupération d'un établissement comptable
        /// </summary>
        /// <param name="id">Identifiant de l'établissement</param>
        /// <returns>Retourne un établissement comptable d'après son ID</returns>
        [HttpGet]
        [Route("api/EtablissementComptable/GetEtablissementComptableById/{id}")]
        public EtablissementComptableModel GetEtablissementComptableById(int id)
        {
            var etab = this.etabComptMgr.GetEtablissementComptableById(id);
            return this.mapper.Map<EtablissementComptableModel>(etab);
        }

        /// <summary>
        /// Méthode GET de récupération de toutes les sociétés.
        /// </summary>
        /// <returns>Retourne la liste de toutes les sociétés</returns>
        [HttpGet]
        [Route("api/EtablissementComptable/GetSocieteAll/")]
        public HttpResponseMessage GetSocieteAll()
        {
            try
            {
                var societes = this.mapper.Map<IEnumerable<SocieteModel>>(this.societeMgr.GetSocieteListAll());
                return Request.CreateResponse(HttpStatusCode.OK, societes);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode GET de récupération de toutes les sociétés.
        /// </summary>
        /// <returns>Retourne la liste de toutes les sociétés</returns>
        [HttpGet]
        [Route("api/EtablissementComptable/GetOrganisationAll/")]
        public HttpResponseMessage GetOrganisationAll()
        {
            try
            {
                var organisations = this.mapper.Map<IEnumerable<OrganisationModel>>(this.orgaMgr.GetList());
                return Request.CreateResponse(HttpStatusCode.OK, organisations);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode GET de récupération de tous les codes absence par societe id.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Retourne la liste de tous les codes absences</returns>
        [HttpGet]
        [Route("api/EtablissementComptable/All/{societeId}")]
        public HttpResponseMessage GetEtablissementComptableBySocieteId(int societeId)
        {
            try
            {
                var etablissementComptables = this.mapper.Map<IEnumerable<EtablissementComptableModel>>(this.etabComptMgr.GetListBySocieteId(societeId));
                return Request.CreateResponse(HttpStatusCode.OK, etablissementComptables);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [Route("api/EtablissementComptable/")]
        public HttpResponseMessage Post(EtablissementComptableModel etablissementComptableModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    etablissementComptableModel.AuteurCreationId = utilisateurId;
                    etablissementComptableModel.DateCreation = DateTime.UtcNow;
                    this.etabComptMgr.AddEtablissementComptable(this.mapper.Map<EtablissementComptableEnt>(etablissementComptableModel));
                    string cgaFournitureFileName = $"{etablissementComptableModel.SocieteId}_{etablissementComptableModel.Code}{BondeCommande.CGAFournitureSuffixe}";
                    string cgaLocationFileName = $"{etablissementComptableModel.SocieteId}_{etablissementComptableModel.Code}{BondeCommande.CGALocationSuffixe}";
                    string cgaPrestationFileName = $"{etablissementComptableModel.SocieteId}_{etablissementComptableModel.Code}{BondeCommande.CGAPrestationSuffixe}";
                    this.etabComptMgr.UploadCGA(etablissementComptableModel.CGAFourniture, etablissementComptableModel.CGALocation, etablissementComptableModel.CGAPrestation, cgaFournitureFileName, cgaLocationFileName, cgaPrestationFileName);
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, this.etabComptMgr.GetEtablissementComptableWithCGA(etablissementComptableModel));
                    return response;
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
        /// Méthode POST de récupération des filtres de recherche sur établissement comptable
        /// </summary>
        /// <returns>Retourne la liste des filtres de recherche sur établissement comptable</returns>
        [HttpGet]
        [Route("api/EtablissementComptable/Filter/")]
        public HttpResponseMessage Filters()
        {
            try
            {
                var filters = this.mapper.Map<SearchEtablissementComptableModel>(this.etabComptMgr.GetDefaultFilter());
                return Request.CreateResponse(HttpStatusCode.OK, filters);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// DELETE api/controller/5
        /// </summary>
        /// <param name="etablissementComptableModel">L'établissement comptable à supprimer</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/EtablissementComptable/Delete")]
        public HttpResponseMessage Delete(EtablissementComptableModel etablissementComptableModel)
        {
            return this.Delete(() => this.etabComptMgr.DeleteEtablissementComptableById(this.mapper.Map<EtablissementComptableEnt>(etablissementComptableModel)));
        }

        /// <summary>
        /// PUT api/controller/5
        /// </summary>
        /// <param name="etabComp">Etablissement Comptable</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPut]
        [Route("api/EtablissementComptable/")]
        public HttpResponseMessage Put(EtablissementComptableModel etabComp)
        {
            try
            {
                this.etabComptMgr.UpdateEtablissementComptable(this.mapper.Map<EtablissementComptableEnt>(etabComp));
                string cgaFournitureFileName = $"{etabComp.SocieteId}_{etabComp.Code}{BondeCommande.CGAFournitureSuffixe}";
                string cgaLocationFileName = $"{etabComp.SocieteId}_{etabComp.Code}{BondeCommande.CGALocationSuffixe}";
                string cgaPrestationFileName = $"{etabComp.SocieteId}_{etabComp.Code}{BondeCommande.CGAPrestationSuffixe}";
                this.etabComptMgr.UploadCGA(etabComp.CGAFourniture, etabComp.CGALocation, etabComp.CGAPrestation, cgaFournitureFileName, cgaLocationFileName, cgaPrestationFileName);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [Route("api/EtablissementComptable/CheckExistsCode/{codeEtablissementComptable}/{idCourant}/{societeId}/")]
        public HttpResponseMessage IsCodeExistsBySocieteId(string codeEtablissementComptable, int idCourant, int societeId)
        {
            try
            {
                var exists = this.etabComptMgr.IsEtablissementComptableExistsByCode(idCourant, codeEtablissementComptable, societeId);
                return Request.CreateResponse(HttpStatusCode.OK, exists);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode GET de récupération d'une nouvelle instance établissement comptable intialisée.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Retourne une nouvelle instance  établissement comptable intialisée</returns>
        [HttpGet]
        [Route("api/EtablissementComptable/New/{societeId}")]
        public HttpResponseMessage New(int societeId)
        {
            try
            {
                var etablissementComptable = this.mapper.Map<EtablissementComptableModel>(this.etabComptMgr.GetNewEtablissementComptable(societeId));
                return Request.CreateResponse(HttpStatusCode.OK, etablissementComptable);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode GET de recherche des  établissements comptables
        /// </summary>
        /// <param name="filters">Filtre de la recherche</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="recherche">Saisie de la recherche</param>
        /// <returns>Retourne la liste des  établissements comptables</returns>
        [HttpPost]
        [Route("api/EtablissementComptable/SearchAll/{societeId}/{recherche?}")]
        public HttpResponseMessage SearchAll(SearchEtablissementComptableModel filters, int societeId, string recherche = "")
        {
            try
            {
                var etablissementComptables = this.etabComptMgr.SearchEtablissementComptableAllBySocieteIdWithFilters(this.mapper.Map<SearchEtablissementComptableEnt>(filters), societeId, recherche);
                var etablissementComptablesList = this.etabComptMgr.GetListWithCGA(etablissementComptables);
                return Request.CreateResponse(HttpStatusCode.OK, this.mapper.Map<IEnumerable<EtablissementComptableModel>>(etablissementComptablesList));
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        ///   Recherche light des établissements comptable (Lookup)
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="showAllOrganisations">désactivation du filtre sur les organisations de l'utilisateur connecté</param>
        /// <param name="organisationId">Identifiant de l'organisation</param>
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/EtablissementComptable/SearchLight/{page?}/{pageSize?}/{recherche?}/{societeId?}/{showAllOrganisations?}/{organisationId?}")]

        public HttpResponseMessage SearchLight(int page = 1, int pageSize = 20, string recherche = "", int? societeId = null, bool? showAllOrganisations = false, int? organisationId = null)
        {
            return this.Get(() => this.mapper.Map<IEnumerable<EtablissementComptableModel>>(this.etabComptMgr.SearchLight(page, pageSize, recherche, societeId, showAllOrganisations ?? false, organisationId)));
        }

        /// <summary>
        /// Ge current user etab comptable without parent tree
        /// </summary>
        /// <param name="organisationPereId">Pere Id</param>
        /// <returns>IHttp action result</returns>
        [HttpGet]
        [Route("api/EtablissementComptable/GeCurrentUserEtabComptableWithOrganisationPartentId/{page?}/{pageSize?}/{recherche?}/{organisationPereId?}")]
        public async Task<IHttpActionResult> GeCurrentUserEtabComptableWithOrganisationPartentId(int page, int pageSize, string recherche, int? organisationPereId = null)
        {
            IEnumerable<EtablissementComptableEnt> etabs = await this.etabComptMgr.GeCurrentUserEtabComptableWithOrganisationPartentId(page, pageSize, recherche, organisationPereId);
            return this.Ok(this.mapper.Map<IEnumerable<EtablissementComptableModel>>(etabs));
        }
    }
}
