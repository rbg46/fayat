using AutoMapper;
using Fred.Business.Referential;
using Fred.Entities.Referential;
using Fred.Web.Models.Referential;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fred.Web.API
{
    /// <summary>
    /// Controller WEB API des Sociétés
    /// </summary>
    public class EtablissementPaieController : ApiControllerBase
    {
        /// <summary>
        /// Manager business Sociétés
        /// </summary>
        protected readonly IEtablissementPaieManager Manager;

        /// <summary>
        /// Mapper Model / ModelVue
        /// </summary>
        protected readonly IMapper Mapper;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="EtablissementPaieController" />.
        /// </summary>
        /// <param name="manager">Manager de fournisseurs</param>
        /// <param name="mapper">Mapper Model / ModelVue</param>
        public EtablissementPaieController(IEtablissementPaieManager manager, IMapper mapper)
        {
            this.Manager = manager;
            this.Mapper = mapper;
        }

        /// <summary>
        /// Méthode GET de récupération des établissements de paie
        /// </summary>
        /// <returns>Retourne la liste des établissements de paie</returns>
        [HttpGet]
        [Route("api/EtablissementPaie/All")]
        public IEnumerable<EtablissementPaieModel> GetAll()
        {
            var etablissement = this.Manager.GetEtablissementPaieList();
            return this.Mapper.Map<IEnumerable<EtablissementPaieModel>>(etablissement);
        }

        /// <summary>
        /// Méthode GET de récupération des établissements de paie par societeId
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>Retourne la liste des établissements de paie</returns>
        [HttpGet]
        [Route("api/EtablissementPaie/BySocieteID/{societeId}")]
        public IEnumerable<EtablissementPaieModel> GetBySocieteId(int societeId)
        {
            var etablissements = this.Manager.GetEtablissementPaieBySocieteId(societeId);
            return this.Mapper.Map<IEnumerable<EtablissementPaieModel>>(etablissements);
        }

        /// <summary>
        /// Méthode GET de récupération des établissements de paie par ID et par code et par libellé
        /// </summary>
        /// <param name="code">code</param>
        /// <param name="libelle">libelle</param>
        /// <param name="idCourant">idCourant</param>
        /// <returns>Retourne la liste des établissements de paie</returns>
        [HttpGet]
        [Route("api/EtablissementPaie/CheckExistsCodeLibelle/{code}/{libelle}/{idCourant}/")]
        public HttpResponseMessage IsEtablissementPaieExists(string code, string libelle, int idCourant)
        {
            try
            {
                var exists = this.Manager.IsEtablissementPaieExistsByCodeLibelle(idCourant, code, libelle);
                return Request.CreateResponse(HttpStatusCode.OK, exists);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode GET de récupération des codes déplacement par ID et par code
        /// </summary>
        /// <param name="idCourant">idCourant</param>
        /// <param name="code">code</param>
        /// <param name="societeId">societeId</param>
        /// <returns>Retourne la liste des codes déplacement</returns>
        [HttpGet]
        [Route("api/EtablissementPaie/CheckExistsCode/{idCourant}/{code}/{societeId}")]
        public HttpResponseMessage IsCodeDeplacementExists(int idCourant, string code, int societeId)
        {
            try
            {
                var exists = this.Manager.IsCodeEtablissementPaieExistsByCode(idCourant, code, societeId);
                return Request.CreateResponse(HttpStatusCode.OK, exists);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode GET de récupération d'un établissement de paie
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Retourne un établissement de paie d'après son ID</returns>
        [HttpGet]
        [Route("api/EtablissementPaie/GetEtablissementPaieById/{id}")]
        public EtablissementPaieModel GetEtablissementPaieById(int id)
        {
            var etab = this.Manager.GetEtablissementPaieById(id);
            return this.Mapper.Map<EtablissementPaieModel>(etab);
        }

        /// <summary>
        /// Méthode GET de récupération d'une nouvelle instance d'établissement de paie intialisée.
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>Retourne une nouvelle instance d'établissement de paie intialisée</returns>
        [HttpGet]
        [Route("api/EtablissementPaie/New/{societeId}")]
        public HttpResponseMessage New(int societeId)
        {
            try
            {
                var etablissement = this.Mapper.Map<EtablissementPaieModel>(this.Manager.GetNewEtablissementPaie(societeId));
                return Request.CreateResponse(HttpStatusCode.OK, etablissement);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode GET de récupération de tous les établissements de paie éligibles à être une agence de rattachement
        /// </summary>
        /// <param name="id">ID de l'établissement de paie à exclure de la recherche</param>
        /// <returns>Retourne une nouvelle instance d'établissement de paie intialisée</returns>
        [HttpGet]
        [Route("api/EtablissementPaie/GetAgencesDeRattachement/{id}")]
        public HttpResponseMessage GetAgencesDeRattachement(int id)
        {
            var agences = this.Manager.AgencesDeRattachement(id);
            return Request.CreateResponse(HttpStatusCode.OK, this.Mapper.Map<IEnumerable<EtablissementPaieModel>>(agences));
        }


        /// <summary>
        /// Méthode POST de création des établissements de paie
        /// </summary>
        /// <param name="etablissementPaieModel">etablissementPaieModel</param>
        /// <returns>Retourne la liste des établissements de paie</returns>
        [HttpPost]
        [Route("api/EtablissementPaie")]
        public HttpResponseMessage Post(EtablissementPaieModel etablissementPaieModel)
        {
            try
            {
                if (etablissementPaieModel != null)
                {
                    Manager.AddEtablissementPaie(this.Mapper.Map<EtablissementPaieEnt>(etablissementPaieModel));

                    return Request.CreateResponse(HttpStatusCode.Created);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ControllerRessources.Etablissement_Paie_Controller_ErreurCode);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode PUT de mise à jour des établissements de paie
        /// </summary>
        /// <param name="etablissementPaieModel">etablissementPaieModel</param>
        /// <returns>Retourne une réponse HTTP</returns>
        /// <exception cref="ValidationException">"Le code déplacement est invalide."</exception>
        [HttpPut]
        [Route("api/EtablissementPaie")]
        public HttpResponseMessage Put(EtablissementPaieModel etablissementPaieModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.Manager.UpdateEtablissementPaie(this.Mapper.Map<EtablissementPaieEnt>(etablissementPaieModel));
                }
                else
                {
                    throw new ValidationException("Le code déplacement est invalide.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// DELETE api/controller/5
        /// </summary>
        /// <param name="etablissementPaieModel">L'établissement paie à supprimer</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/EtablissementPaie/Delete")]
        public HttpResponseMessage Delete(EtablissementPaieModel etablissementPaieModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
                }

                if (etablissementPaieModel != null)
                {
                    if (this.Manager.DeleteEtablissementPaieById(this.Mapper.Map<EtablissementPaieEnt>(etablissementPaieModel)))
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
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Rechercher les référentiels CI
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <param name="societeId">societeId</param>
        /// <param name="agenceId">agenceId</param>
        /// <param name="isHorsRegion">Est hors région</param>
        /// <param name="isAgenceRattachement">Est agence de rattachement</param>
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/EtablissementPaie/SearchLight/{page?}/{pageSize?}/{recherche?}/{societeId?}/{agenceId?}/{isHorsRegion?}/{isAgenceRattachement?}")]

        public HttpResponseMessage SearchLight(
            int page = 1,
            int pageSize = 20,
            string recherche = "",
            int? societeId = null,
            int? agenceId = null,
            bool? isHorsRegion = null,
            bool? isAgenceRattachement = null)
        {
            return this.Get(() => this.Mapper.Map<IEnumerable<EtablissementPaieModel>>(this.Manager.SearchLight(recherche, page, pageSize, societeId, agenceId, isHorsRegion, isAgenceRattachement)));
        }

        /// <summary>
        /// Méthode GET de recherche des sociétés
        /// </summary>
        /// <param name="filters">filters</param>
        /// <param name="societeId">societeId</param>
        /// <param name="recherche">recherche</param>
        /// <returns>Retourne la liste des sociétés</returns>
        [HttpPost]
        [Route("api/EtablissementPaie/SearchAll/{societeId}/{recherche?}")]
        public HttpResponseMessage SearchAll(SearchActiveModel filters, int societeId, string recherche = "")
        {
            var etabPaie = this.Manager.SearchEtablissementPaieAllWithFilters(societeId, recherche, this.Mapper.Map<SearchEtablissementPaieEnt>(filters));
            return Request.CreateResponse(HttpStatusCode.OK, this.Mapper.Map<IEnumerable<EtablissementPaieModel>>(etabPaie));
        }

        /// <summary>
        /// Get etab paie list for validation pointage vrac Fes Async
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="recherche">Recherche</param>
        /// <param name="societeId">Societe Id</param>
        /// <returns>Etab Paie List</returns>
        [HttpGet]
        [Route("api/EtablissementPaie/GetEtabPaieListForValidationPointageVracFesAsync/{page?}/{pageSize?}/{recherche?}/{societeId?}")]
        public async Task<IHttpActionResult> GetEtabPaieListForValidationPointageVracFesAsync(int page = 1, int pageSize = 20, string recherche = "", int? societeId = null)
        {
            IEnumerable<EtablissementPaieEnt> etabs = await Manager.GetEtabPaieListForValidationPointageVracFesAsync(page, pageSize, recherche, societeId);
            return this.Ok(this.Mapper.Map<IEnumerable<EtablissementPaieModel>>(etabs));
        }
    }
}