using AutoMapper;
using Fred.Business.Common.ExportDocument;
using Fred.Business.Referential.Nature;
using Fred.Entities.Referential;
using Fred.Framework.Reporting;
using Fred.Web.Models.Referential;
using Fred.Web.Shared.Models.Nature;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Web;
using System.Web.Http;

namespace Fred.Web.API
{
    /// <summary>
    /// Controller WEB API des codes nature
    /// </summary>
    public class NatureController : ApiControllerBase
    {
        /// <summary>
        /// Manager business codes nature
        /// </summary>
        protected readonly INatureManager NatMgr;

        /// <summary>
        /// Mapper Model / ModelVue
        /// </summary>
        protected readonly IMapper Mapper;

        /// <summary>
        /// Service des exports de document
        /// </summary>
        protected readonly IExportDocumentService exportDocumentService;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="NatureController" />.
        /// </summary>
        /// <param name="natureMgr">Manager de fournisseurs</param>
        /// <param name="mapper">Mapper Model / ModelVue</param>
        /// <param name="exportDocumentService">Service des exports de document</param>
        public NatureController(INatureManager natureMgr, IMapper mapper, IExportDocumentService exportDocumentService)
        {
            this.NatMgr = natureMgr;
            this.Mapper = mapper;
            this.exportDocumentService = exportDocumentService;
        }

        /// <summary>
        /// Méthode GET de récupération de tous les codes absence par societe id.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Retourne la liste de tous les codes absences</returns>
        [HttpGet]
        [Route("api/Nature/GetNatureBySocieteId{societeId}")]
        public HttpResponseMessage GetNatureBySocieteId(int societeId)
        {
            try
            {
                IEnumerable<NatureModel> natures = this.Mapper.Map<IEnumerable<NatureModel>>(this.NatMgr.GetNatureBySocieteId(societeId));
                return Request.CreateResponse(HttpStatusCode.OK, natures);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [Route("api/Nature/")]
        public HttpResponseMessage Post(NatureModel natureModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    int natureId = this.NatMgr.AddNature(this.Mapper.Map<NatureEnt>(natureModel));
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, natureId);
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
        /// DELETE api/controller/5
        /// </summary>
        /// <param name="nature">code nature à supprimer</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Nature/Delete")]
        public HttpResponseMessage Delete(NatureModel nature)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
                }

                if (nature != null)
                {
                    if (this.NatMgr.DeleteNatureById(this.Mapper.Map<NatureEnt>(nature)))
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
        /// PUT api/controller/5 met à jour la Nature avec uniquement les champs nécessaires (Code, Libelle, IsActif, RessourceEntId)
        /// </summary>
        /// <param name="nature">Nature à traiter</param>
        /// <returns>Retourne une réponse HTTP</returns>    
        [HttpPut]
        [Route("api/Nature/")]
        public HttpResponseMessage Put(NatureModel nature)
        {
            try
            {
                List<Expression<Func<NatureEnt, object>>> fieldToUpdate = new List<Expression<Func<NatureEnt, object>>>
                {
                    x => x.Code,
                    x => x.Libelle,
                    x => x.IsActif,
                    x => x.RessourceId
                };

                if (ModelState.IsValid)
                {
                    this.NatMgr.UpdateNature(this.Mapper.Map<NatureEnt>(nature), fieldToUpdate);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// GET api/controller/5 Permet de déterminer si une nature ayant un code similaire pour la même société existe ou non
        /// </summary>
        /// <param name="idCourant">Identifiant de la nature courante</param>
        /// <param name="codeNature">Code de la nature courante</param>
        /// <param name="societeId">Identifiant de la société liée</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpGet]
        [Route("api/Nature/CheckExistsCode/{idCourant}/{codeNature}/{societeId}")]
        public HttpResponseMessage IsCodeNatureExistsByCodeAndSocieteId(int idCourant, string codeNature, int societeId)
        {
            try
            {
                bool exists = this.NatMgr.IsNatureExistsByCode(idCourant, codeNature, societeId);
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
        /// <param name="societeId">Identifiant de la société liée</param>
        /// <returns>Retourne une nouvelle instance code absence intialisée</returns>
        [HttpGet]
        [Route("api/Nature/New/{societeId}")]
        public HttpResponseMessage New(int societeId)
        {
            try
            {
                NatureModel nature = this.Mapper.Map<NatureModel>(this.NatMgr.GetNewNature(societeId));
                return Request.CreateResponse(HttpStatusCode.OK, nature);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [Route("api/Nature/GetNatures")]
        public HttpResponseMessage GetNatures()
        {
            try
            {
                IEnumerable<NatureModel> natures = this.Mapper.Map<IEnumerable<NatureModel>>(this.NatMgr.GetNatureListAll());
                return Request.CreateResponse(HttpStatusCode.OK, natures);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode GET de recherche des codes absences
        /// </summary>
        /// <param name="filters">Filtre</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="recherche">Texte à rechercher</param>
        /// <returns>Retourne la liste des codes absences</returns>
        [HttpPost]
        [Route("api/Nature/SearchAllBySocieteId/{societeId}/{recherche?}")]
        public HttpResponseMessage SearchAllBySocieteId(SearchValueAndSocietyActiveModel filters, int societeId, string recherche = "")
        {
            filters.SocieteId = societeId;
            IEnumerable<NatureEnt> natures = this.NatMgr.SearchNatureWithFilters(recherche, this.Mapper.Map<SearchCriteriaEnt<NatureEnt>>(filters));
            return Request.CreateResponse(HttpStatusCode.OK, this.Mapper.Map<IEnumerable<NatureModel>>(natures));
        }

        /// <summary>
        ///   GET Récupération des natures analytiques d'une société
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Retourne la liste de toutes natures d'une société</returns>    
        [HttpGet]
        [Route("api/Nature/{societeId}")]
        public HttpResponseMessage GetNatureListBySocieteId(int societeId)
        {
            return Get(() => Mapper.Map<IEnumerable<NatureModel>>(this.NatMgr.GetNatureActiveBySocieteId(societeId)));
        }

        /// <summary>
        ///  Verifie si l'entité est déja utilisée.
        /// </summary>
        /// <param name="natureId">natureId</param>  
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/Nature/IsAlreadyUsed/{natureId}")]
        public HttpResponseMessage IsAlreadyUsed(int natureId)
        {
            return Get(() => new
            {
                id = natureId,
                isAlreadyUsed = this.NatMgr.IsAlreadyUsed(natureId)
            });
        }

        /// <summary>
        /// Génère un fichier Excel avec les résultats filtrés ou non de la page
        /// </summary>
        /// <param name="natures">Liste de nature filtrées</param>
        /// <returns>Le GUID du fichier Excel</returns>
        [HttpPost]
        [Route("api/Nature/GenerateExportExcel")]
        public IHttpActionResult GenerateExportExcel(List<NatureModel> natures)
        {
            try
            {
                using (var excelFormat = new ExcelFormat())
                {
                    var resourceId = excelFormat.GenerateExcelAndSaveOnServer("/Templates/Nature/TemplateNature.xls", natures, null);

                    return Ok(new { id = resourceId });
                }
            }
            catch (Exception ex)
            {
                logger.Log(NLog.LogLevel.Error, ex);
                return null;
            }
        }

        /// <summary>
        /// Télécharge l'export Excel des natures
        /// </summary>
        /// <param name="id">Identifiant du cache</param>
        /// <returns>Fichier excel des natures</returns>
        [HttpGet]
        [Route("api/Nature/DownloadExportExcel/{id}")]
        public HttpResponseMessage DownloadExportExcel(string id)
        {
            string cacheName = exportDocumentService.GetCacheName(id, isPdf: false);
            byte[] bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }
            string exportFilename = "Natures.xlsx";
            return exportDocumentService.CreateResponseForDownloadDocument(exportFilename, bytes);
        }

        /// <summary>
        /// Mets à jour une liste de nature
        /// </summary>
        /// <param name="natures"><see cref="NatureModel"/></param>
        /// <returns>Code de retour de la requête</returns>
        [HttpPut]
        [Route("api/Nature/UpdateNatures")]
        public HttpResponseMessage UpdateNatures(List<NatureFamilleOdModel> natures)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.NatMgr.UpdateNatures(natures);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// Récupère la liste des natures actives pour une société
        /// </summary>
        /// <param name="societeId">Identifiant de la societe</param>
        /// <returns>Code de retour de la requête</returns>
        [HttpGet]
        [Route("api/Nature/GetNatures/{societeId}")]
        public HttpResponseMessage GetNatureFamilleOdBySocieteId(int societeId)
        {
            try
            {
                IEnumerable<NatureFamilleOdModel> natures = this.NatMgr.GetNatureActiveFamilleOds(societeId);
                return Request.CreateResponse(HttpStatusCode.OK, natures);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
