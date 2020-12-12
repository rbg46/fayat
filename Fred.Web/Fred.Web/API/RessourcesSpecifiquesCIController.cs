using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Fred.Business.Common.ExportDocument;
using Fred.Business.ReferentielFixe;
using Fred.Business.RessourcesSpecifiquesCI;
using Fred.Entities.ReferentielFixe;
using Fred.Framework.Reporting;
using Fred.Web.Models.ReferentielEtendu;
using Fred.Web.Models.ReferentielFixe;

namespace Fred.Web.API
{
    public class RessourcesSpecifiquesCIController : ApiControllerBase
    {
        private readonly IMapper mapper;
        private readonly IExportDocumentService exportDocumentService;
        private readonly IRessourcesSpecifiquesCiManager ressourcesSpecifiquesCiManager;
        private readonly IReferentielFixeManager referentielFixeManager;

        /// <summary>
        ///   Constructeur 
        /// </summary>   
        /// <param name="mapper">Mapper Model/Ent</param>
        /// <param name="ressourcesSpecifiquesCiManager">Manager</param>
        /// <param name="refFixeManager">Manager des ressources</param>
        /// <param name="exportDocumentService">exportDocumentService</param>
        public RessourcesSpecifiquesCIController(IMapper mapper, IRessourcesSpecifiquesCiManager ressourcesSpecifiquesCiManager, IReferentielFixeManager refFixeManager, IExportDocumentService exportDocumentService)
        {
            this.mapper = mapper;
            this.ressourcesSpecifiquesCiManager = ressourcesSpecifiquesCiManager;
            this.exportDocumentService = exportDocumentService;
            this.referentielFixeManager = refFixeManager;
        }

        /// <summary>
        ///   GET de récupération des ressources
        /// </summary>
        /// <param name="ciId">ID du CI</param>
        /// <returns>Retourne la liste des ressources</returns>
        [HttpGet]
        [Route("api/RessourcesSpecifiquesCI/Ressources/{ciId}")]
        public HttpResponseMessage GetRessourcesSpecifiquesCI(int ciId)
        {
            return Get(() => this.mapper.Map<List<ChapitreModel>>(ressourcesSpecifiquesCiManager.GetAllReferentielEtenduAsChapitreList(ciId)));
        }

        /// <summary>
        ///   POST : Inserer une nouvelle ressource
        /// </summary>
        /// <param name="ressource">code de ressource</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPost]
        [Route("api/RessourcesSpecifiquesCI/AddRessource")]
        public HttpResponseMessage AddRessource(RessourceModel ressource)
        {
            ressource.RessourceRattachement = null;
            return Post(() => this.mapper.Map<RessourceModel>(referentielFixeManager.AddRessource(this.mapper.Map<RessourceEnt>(ressource))));
        }


        /// <summary>
        ///   PUT : Mettre à jour une ressource
        /// </summary>
        /// <param name="ressource">Ressource à mettre à jour</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPut]
        [Route("api/RessourcesSpecifiquesCI/UpdateRessource")]
        public HttpResponseMessage UpdateRessource(RessourceModel ressource)
        {
            ressource.RessourceRattachement = null;
            ressource.SousChapitre = null;
            return Put(() => this.mapper.Map<RessourceModel>(referentielFixeManager.UpdateRessource(this.mapper.Map<RessourceEnt>(ressource))));
        }

        /// <summary>
        ///   PUT : Désactiver une ressource
        /// </summary>
        /// <param name="ressourceId">Identifiant de la ressource à supprimer</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPut]
        [Route("api/RessourcesSpecifiquesCI/DeleteRessource/{ressourceId}")]
        public HttpResponseMessage DeleteRessource(int ressourceId)
        {
            return Put(() => this.mapper.Map<RessourceModel>(ressourcesSpecifiquesCiManager.DeleteById(ressourceId)));
        }


        /// <summary>
        /// Méthode de génération d'une liste de ressources au format excel
        /// </summary>
        /// <param name="ciId">ID du CI</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPost]
        [Route("api/RessourcesSpecifiquesCI/GenerateExcel/{ciId}")]
        public IHttpActionResult GenerateExcel(int ciId)
        {
            ExcelFormat excelFormat = new ExcelFormat();
            string pathName = HttpContext.Current.Server.MapPath("/Templates/TemplateRessourcesSpecifiquesCI.xls").ToString();
            var result = new List<ReferentielEtenduModel>();
            var chapitre = this.mapper.Map<List<ChapitreModel>>(ressourcesSpecifiquesCiManager.GetAllReferentielEtenduAsChapitreList(ciId));
            if (chapitre != null)
            {
                var sousChapitres = chapitre.SelectMany(c => c.SousChapitres).ToList();
                var ressources = sousChapitres.SelectMany(c => c.Ressources).ToList();
                result = ressources.SelectMany(c => c.ReferentielEtendus).ToList();
            }

            byte[] excelBytes = excelFormat.GenerateExcel(pathName, result);

            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(30), Priority = CacheItemPriority.NotRemovable };
            var cacheId = Guid.NewGuid().ToString();
            MemoryCache.Default.Add("excelBytes_" + cacheId, excelBytes, policy);

            return Ok(new { id = cacheId });
        }

        /// <summary>
        /// Méthode d'extraction d'une liste des ressources au format excel
        /// </summary>
        /// <param name="id">Identifiant de l'export excel</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/RessourcesSpecifiquesCI/ExtractExcel/{id}")]
        public HttpResponseMessage ExtractExcel(string id)
        {
            var cacheName = exportDocumentService.GetCacheName(id, isPdf: false);
            var bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }
            var exportDocument = exportDocumentService.GetDocumentFileName(fileName: "ExtractRessourcesSpecifiquesCI", isPdf: false);
            return exportDocumentService.CreateResponseForDownloadDocument(exportDocument, bytes);
        }

        /// <summary>
        /// Permet d'obtenir un code incrementé pour la nouvelle ressource créée
        /// </summary>
        /// <param name="ressourceRattachementId">Identifiant de la ressource rattaché</param>
        /// <returns>le code incrémenté</returns>
        [HttpGet]
        [Route("api/RessourcesSpecifiquesCI/GetNextRessourceCode/{ressourceRattachementId}")]
        public HttpResponseMessage GetNextRessourceCode(int ressourceRattachementId)
        {
            return Get(() => this.ressourcesSpecifiquesCiManager.GetNextRessourceCode(ressourceRattachementId));
        }
    }
}
