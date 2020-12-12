using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Fred.Business.CI;
using Fred.Business.Common.ExportDocument;
using Fred.Business.FeatureFlipping;
using Fred.Business.Referential;
using Fred.Business.ReferentielFixe;
using Fred.Business.Utilisateur;
using Fred.Entities;
using Fred.Entities.ReferentielFixe;
using Fred.Framework.FeatureFlipping;
using Fred.Framework.Reporting;
using Fred.Web.Models.Referential;
using Fred.Web.Models.ReferentielFixe;

namespace Fred.Web.API
{
    public class ReferentielFixesController : ApiControllerBase
    {
        private readonly IReferentielFixeManager referentielFixeManager;
        private readonly IUtilisateurManager userManager;
        private readonly IMapper mapper;
        private readonly IExportDocumentService exportDocumentService;
        private readonly ICIManager ciManager;
        private readonly IEtablissementComptableManager etablissementComptableManager;
        private readonly IFeatureFlippingManager featureFlippingManager;

        public ReferentielFixesController(
            IReferentielFixeManager referentielFixeManager,
            IUtilisateurManager userManager,
            IMapper mapper,
            IExportDocumentService exportDocumentService,
            ICIManager ciManager,
            IEtablissementComptableManager etablissementComptableManager,
            IFeatureFlippingManager featureFlippingManager)
        {
            this.referentielFixeManager = referentielFixeManager;
            this.userManager = userManager;
            this.mapper = mapper;
            this.exportDocumentService = exportDocumentService;
            this.ciManager = ciManager;
            this.etablissementComptableManager = etablissementComptableManager;
            this.featureFlippingManager = featureFlippingManager;
        }

        /// <summary>
        ///   GET : Récupération du Référentiel Fixe en fonction de l'utilisateur connecté. (En fonction du Groupe de l'utilisateur)
        /// </summary>
        /// <returns>Une réponse HTTP</returns>
        [HttpGet]
        [Route("api/ReferentielFixes")]
        public HttpResponseMessage GetChapitreListByUser()
        {
            return Get(() => this.mapper.Map<List<ChapitreModel>>(referentielFixeManager.GetChapitreListByUtilisateurId((int)this.GetCurrentUserId())));
        }

        /// <summary>
        ///   GET : Récupération du Référentiel Fixe en fonction de l'utilisateur connecté. (En fonction du Groupe de l'utilisateur)
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpGet]
        [Route("api/ReferentielFixes/{groupeId}")]
        public HttpResponseMessage GetChapitreListByGroupeId(int groupeId)
        {
            return Get(() => this.mapper.Map<List<ChapitreModel>>(referentielFixeManager.GetChapitreListByGroupeId(groupeId)));
        }

        /// <summary>
        ///   GET : Récupère un chapitre en fonction de son identifiant
        /// </summary>
        /// <param name="chapitreId">identifient chapitre</param>
        /// <returns>Retourne un chapitre par son identifiant</returns>
        [HttpGet]
        [Route("api/ReferentielFixes/GetChapitre/{chapitreId}")]
        public HttpResponseMessage GetChapitre(int chapitreId)
        {
            return Get(() => this.mapper.Map<ChapitreModel>(referentielFixeManager.GetChapitreById(chapitreId)));
        }

        /// <summary>
        /// GET : Récupère la liste des Sous Chapitre d'un Chapitre
        /// </summary>
        /// <param name="chapitreId">identifiant du chapitre parent</param>
        /// <returns>Retourne la liste des sous chapitres</returns>
        [HttpGet]
        [Route("api/ReferentielFixes/SousChapitres/{chapitreId}")]
        public HttpResponseMessage GetListSousChapitres(int chapitreId)
        {
            return Get(() => this.mapper.Map<List<SousChapitreModel>>(referentielFixeManager.GetSousChapitreListByChapitreId(chapitreId)));
        }

        /// <summary>
        ///   GET : Récupère le Sous Chapitre en fonction de son Identifiant
        /// </summary>
        /// <param name="sousChapitreId">identifiant sous chapitre</param>
        /// <returns>Retourne un sous chapitre par son identifiant</returns>
        [HttpGet]
        [Route("api/ReferentielFixes/GetSousChapitre/{sousChapitreId}")]
        public HttpResponseMessage GetSousChapitre(int sousChapitreId)
        {
            return Get(() => this.mapper.Map<SousChapitreModel>(referentielFixeManager.GetSousChapitreById(sousChapitreId)));
        }

        /// <summary>
        ///   GET de récupération des ressources
        /// </summary>
        /// <returns>Retourne la liste des ressources</returns>
        [HttpGet]
        [Route("api/ReferentielFixes/Ressources")]
        public IEnumerable<RessourceModel> GetRessourceList()
        {
            var ressource = this.referentielFixeManager.GetRessourceList();
            return this.mapper.Map<IEnumerable<RessourceModel>>(ressource);
        }

        /// <summary>
        ///   GET : Retourne la liste des ressources liés à un sous chapitre
        /// </summary>
        /// <param name="sousChapitreId">identifiant sous chapitre</param>
        /// <returns>Retourne la liste des ressources liés à un sous chapitre</returns>
        [HttpGet]
        [Route("api/ReferentielFixes/Ressources/{sousChapitreId}")]
        public HttpResponseMessage GetRessourceListBySousChapitreId(int sousChapitreId)
        {
            return Get(() => this.mapper.Map<IEnumerable<RessourceModel>>(referentielFixeManager.GetRessourceListBySousChapitreId(sousChapitreId)));
        }

        /// <summary>
        ///   GET : Retourne une ressource par son identifiant
        /// </summary>
        /// <param name="ressourceId">identifiant ressource</param>
        /// <returns>Retourne une ressource par son identifiant</returns>
        [HttpGet]
        [Route("api/ReferentielFixes/GetRessource/{ressourceId}")]
        public HttpResponseMessage GetRessource(int ressourceId)
        {
            return this.Get(() => this.mapper.Map<RessourceModel>(referentielFixeManager.GetRessourceById(ressourceId)));
        }

        /// <summary>
        ///   POST : Inserer un nouveau chapitre
        /// </summary>
        /// <param name="chapter">Nouveau chapitre</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPost]
        [Route("api/ReferentielFixes/AddChapitre")]
        public HttpResponseMessage AddChapitre(ChapitreModel chapter)
        {
            return Post(() => this.mapper.Map<ChapitreModel>(referentielFixeManager.AddChapitre(this.mapper.Map<ChapitreEnt>(chapter))));
        }

        /// <summary>
        ///   POST : Inserer un nouveau sous chapitre
        /// </summary>
        /// <param name="subChapter">Sous chapitre</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPost]
        [Route("api/ReferentielFixes/AddSousChapitre")]
        public HttpResponseMessage AddSousChapitre(SousChapitreModel subChapter)
        {
            return Post(() => this.mapper.Map<SousChapitreModel>(referentielFixeManager.AddSousChapitre(this.mapper.Map<SousChapitreEnt>(subChapter))));
        }

        /// <summary>
        ///   POST : Inserer une nouvelle ressource
        /// </summary>
        /// <param name="ressource">code de ressource</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPost]
        [Route("api/ReferentielFixes/AddRessource")]
        public HttpResponseMessage AddRessource(RessourceModel ressource)
        {
            return Post(() => this.mapper.Map<RessourceModel>(referentielFixeManager.AddRessource(this.mapper.Map<RessourceEnt>(ressource))));
        }

        /// <summary>
        ///   PUT : Mettre à jour d'un chapitre
        /// </summary>
        /// <param name="chapter">Chapitre à mettre à jour</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPut]
        [Route("api/ReferentielFixes/UpdateChapitre")]
        public HttpResponseMessage UpdateChapitre(ChapitreModel chapter)
        {
            return Put(() => this.mapper.Map<ChapitreModel>(referentielFixeManager.UpdateChapitre(this.mapper.Map<ChapitreEnt>(chapter))));
        }

        /// <summary>
        ///   PUT : Mettre à jour d'un sous chapitre
        /// </summary>
        /// <param name="subChapter">Sous chapitre à mettre à jour</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPut]
        [Route("api/ReferentielFixes/UpdateSousChapitre")]
        public HttpResponseMessage UpdateSousChapitre(SousChapitreModel subChapter)
        {
            return Put(() => this.mapper.Map<SousChapitreModel>(referentielFixeManager.UpdateSousChapitre(this.mapper.Map<SousChapitreEnt>(subChapter))));
        }

        /// <summary>
        ///   PUT : Mettre à jour une ressource
        /// </summary>
        /// <param name="ressource">Ressource à mettre à jour</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPut]
        [Route("api/ReferentielFixes/UpdateRessource")]
        public HttpResponseMessage UpdateRessource(RessourceModel ressource)
        {
            return Put(() => this.mapper.Map<RessourceModel>(referentielFixeManager.UpdateRessource(this.mapper.Map<RessourceEnt>(ressource))));
        }

        /// <summary>
        ///   DELETE : Supprimer un chapitre
        /// </summary>
        /// <param name="chapitreId">Identifiant du Chapitre</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpDelete]
        [Route("api/ReferentielFixes/DeleteChapitre/{chapitreId}")]
        public HttpResponseMessage DeleteChapitre(int chapitreId)
        {
            return Delete(() => referentielFixeManager.DeleteChapitreById(chapitreId));
        }

        /// <summary>
        ///   DELETE : Supprimer un sous chapitre
        /// </summary>
        /// <param name="sousChapitreId">Identifiant du Sous Chapitre à supprimer</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpDelete]
        [Route("api/ReferentielFixes/DeleteSousChapitre/{sousChapitreId}")]
        public HttpResponseMessage DeleteSousChapitre(int sousChapitreId)
        {
            return Delete(() => referentielFixeManager.DeleteSousChapitreById(sousChapitreId));
        }

        /// <summary>
        ///   DELETE : Supprimer une ressource
        /// </summary>
        /// <param name="ressourceId">Identifiant de la ressource à supprimer</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpDelete]
        [Route("api/ReferentielFixes/DeleteRessource/{ressourceId}")]
        public HttpResponseMessage DeleteRessource(int ressourceId)
        {
            return Delete(() => referentielFixeManager.DeleteRessourceById(ressourceId));
        }

        [HttpGet]
        [Route("api/TypeRessource/SearchLight/{page?}/{pageSize?}/{recherche?}")]
        public async Task<IHttpActionResult> SearchLightByTypeRessource(string recherche = "")
        {
            if (featureFlippingManager.IsActivated(EnumFeatureFlipping.ActivationUS13085_6000)) { 
                IEnumerable<TypeRessourceEnt> ressourceTypes = await referentielFixeManager.SearchRessourceTypesByCodeOrLabelAsync(recherche);
                return Ok(mapper.Map<IEnumerable<TypeRessourceModel>>(ressourceTypes));
            }

            return NotFound();
        }

        [HttpGet]
        [Route("api/ReferentielFixes/GetListCarburants")]
        public HttpResponseMessage GetListCarburants()
        {
            return Get(() => this.mapper.Map<IEnumerable<CarburantModel>>(this.referentielFixeManager.GetCarburantList()));
        }

        [HttpGet]
        [Route("api/ReferentielFixes/GetNextRessourceCode/{sousChapitreId}")]
        public HttpResponseMessage GetNextRessourceCode(int sousChapitreId)
        {
            return Get(() => this.referentielFixeManager.GetNextRessourceCode(this.referentielFixeManager.GetSousChapitreById(sousChapitreId)));
        }

        /// <summary>
        /// Méthode de génération d'une liste de commandes au format excel
        /// TODO: remplacer les chemins spécifiés par des valeurs remontées depuis le paramétrage
        ///       spécifier taille des fichiers ?
        /// </summary>
        /// <returns>Une réponse HTTP</returns>
        [HttpPost]
        [Route("api/ReferentielFixes/GenerateExcel")]
        public HttpResponseMessage GenerateExcel()
        {
            return Post(() =>
            {
                int userGroupId = this.userManager.GetContextUtilisateur().Personnel.Societe.GroupeId;
                List<RessourceModel> ressources = this.mapper.Map<List<RessourceModel>>(this.referentielFixeManager.GetRessourceListByGroupeId(userGroupId));
                ExcelFormat excelFormat = new ExcelFormat();
                string pathName = HttpContext.Current.Server.MapPath("/Templates/TemplateReferentielFixe.xlsx").ToString();
                byte[] excelBytes = excelFormat.GenerateExcel(pathName, ressources);
                var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(30), Priority = CacheItemPriority.NotRemovable };
                var cacheId = Guid.NewGuid().ToString();
                MemoryCache.Default.Add("excelBytes_" + cacheId, excelBytes, policy);
                return new { id = cacheId };
            });
        }

        /// <summary>
        /// Méthode d'extraction d'une liste de commandes au format excel
        /// </summary>
        /// <param name="id">Identifiant de l'exctraction</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/ReferentielFixes/ExtractExcel/{id}")]
        public HttpResponseMessage ExtractExcel(string id)
        {
            var cacheName = exportDocumentService.GetCacheName(id, isPdf: false);
            var bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }
            var exportDocument = exportDocumentService.GetDocumentFileName(fileName: "ExtractReferentielFixe", isPdf: false);
            return exportDocumentService.CreateResponseForDownloadDocument(exportDocument, bytes);
        }

        /// <summary>
        /// Rechercher les référentiels ressources
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <param name="ressourceId">Identifiant de la ressource courante dans la lookup</param>
        /// <param name="achats">Indique si on veut uniquement les ressources disponible dans le module achat</param>
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/Ressource/SearchLight/{page?}/{pageSize?}/{recherche?}/{societeId?}/{ressourceId?}/{achats?}")]
        public HttpResponseMessage SearchLight(int? societeId, int page = 1, int pageSize = 20, string recherche = "", int? ressourceId = null, bool? achats = false)
        {
            if (societeId.HasValue)
            {
                return Get(() => this.mapper.Map<IEnumerable<RessourceModel>>(this.referentielFixeManager.SearchLight(recherche, societeId.Value, page, pageSize, null, ressourceId, achats)));
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Empty);
            }
        }
        /// <summary>
        /// Rechercher les référentiels ressources
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <param name="ressourceId">Identifiant de la nature</param>
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/Ressource/SearchLightByNature/{page?}/{pageSize?}/{recherche?}/{societeId?}/{ressourceId?}")]
        public HttpResponseMessage SearchLightByNature(int? societeId, int page = 1, int pageSize = 20, string recherche = "", int? ressourceId = null)
        {
            if (societeId.HasValue)
            {
                return Get(() => this.mapper.Map<IEnumerable<RessourceModel>>(this.referentielFixeManager.SearchLightByNature(recherche, societeId.Value, page, pageSize, ressourceId)));
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Empty);
            }
        }

        /// <summary>
        /// Recherche des ressources par CI
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">texte de recherche</param>
        /// <param name="ressourcesRecommandeesOnly">flag de filtre sur les ressources recommandées uniquement</param>
        /// <param name="ressourceIdNatureFilter">identifiant de la nature</param>
        /// <returns>retoune une liste de ressources</returns>
        [HttpGet]
        [Route("api/Ressource/SearchLightByCi/{ciId?}/{page?}/{pageSize?}/{recherche?}/{ressourcesRecommandeesOnly?}/{ressourceIdNatureFilter?}")]
        public HttpResponseMessage GetReferentielEtenduByCI(int ciId, int page = 1, int pageSize = 20, string recherche = "", int ressourcesRecommandeesOnly = 0, int? ressourceIdNatureFilter = null)
        {
            return Get(() =>
            {
                var ci = ciManager.GetCI(ciId);
                if (ci.EtablissementComptable != null)
                {
                    ci.EtablissementComptable.Organisation = etablissementComptableManager.GetOrganisationByEtablissementId(ci.EtablissementComptable.EtablissementComptableId);
                }

                return mapper.Map<List<RessourceModel>>(this.referentielFixeManager.SearchRessourcesRecommandees(recherche, ci, page, pageSize, ressourcesRecommandeesOnly == 1, ressourceIdNatureFilter));
            });
        }

        /// <summary>
        /// Rechercher les référentiels ressources personnel
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <param name="ressourceId">Identifiant de la ressource courante dans la lookup</param>
        /// <returns>retouner une liste de ressource personnel</returns>
        [HttpGet]
        [Route("api/RessourcePersonnel/SearchLight/{page?}/{pageSize?}/{recherche?}/{societeId?}/{ressourceId?}")]
        public HttpResponseMessage SearchLightRessourcePersonnel(int? societeId, int page = 1, int pageSize = 20, string recherche = "", int? ressourceId = null)
        {
            int personnelTypeRessourceId = referentielFixeManager.GetTypeRessourceByCode(Constantes.TypeRessource.CodeTypePersonnel).TypeRessourceId;
            if (societeId.HasValue)
            {
                return Get(() => this.mapper.Map<IEnumerable<RessourceModel>>(this.referentielFixeManager.SearchLight(recherche, societeId.Value, page, pageSize, personnelTypeRessourceId, ressourceId)).ToList());
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Empty);
            }
        }

        /// <summary>
        /// Rechercher les référentiels ressources matériel
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <param name="ressourceId">Identifiant de la ressource courante dans la lookup</param>
        /// <returns>retouner une liste de ressource matériel</returns>
        [HttpGet]
        [Route("api/RessourceMateriel/SearchLight/{page?}/{pageSize?}/{recherche?}/{societeId?}/{ressourceId?}")]
        public HttpResponseMessage SearchLightRessourceMateriel(int? societeId, int page = 1, int pageSize = 20, string recherche = "", int? ressourceId = null)
        {
            int materielTypeRessourceId = referentielFixeManager.GetTypeRessourceByCode(Constantes.TypeRessource.CodeTypeMateriel).TypeRessourceId;
            if (societeId.HasValue)
            {
                return Get(() => this.mapper.Map<IEnumerable<RessourceModel>>(this.referentielFixeManager.SearchLight(recherche, societeId.Value, page, pageSize, materielTypeRessourceId, ressourceId)).ToList());
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Empty);
            }
        }
    }
}
