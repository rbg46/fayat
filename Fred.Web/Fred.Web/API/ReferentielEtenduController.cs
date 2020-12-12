using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Fred.Business.Common.ExportDocument;
using Fred.Business.ReferentielEtendu;
using Fred.Entities.ReferentielEtendu;
using Fred.Framework.Reporting;
using Fred.Web.Models.ReferentielEtendu;
using Fred.Web.Models.ReferentielFixe;

namespace Fred.Web.API
{
    /// <summary>
    /// Controller WEB API des Référentiels étendus
    /// </summary>
    public class ReferentielEtenduController : ApiControllerBase
    {
        private readonly IReferentielEtenduManager referentielEtenduMgr;
        private readonly IMapper mapper;
        private readonly IExportDocumentService exportDocumentService;

        /// <summary>
        ///   Constructeur 
        /// </summary>
        /// <param name="referentielEtenduManager">Gestionnaire des référentiels étendus</param>    
        /// <param name="mapper">Mapper Model/Ent</param>
        /// <param name="exportDocumentService">exportDocumentService</param>
        public ReferentielEtenduController(IReferentielEtenduManager referentielEtenduManager,
                                          IMapper mapper,
                                          IExportDocumentService exportDocumentService)
        {
            this.referentielEtenduMgr = referentielEtenduManager;
            this.mapper = mapper;
            this.exportDocumentService = exportDocumentService;
        }

        /// <summary>
        ///   GET Récupère un référentiel étendu en fonction de l'identifiant de la société
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Une réponse HTTp</returns>
        [HttpGet]
        [Route("api/ReferentielEtendu/{societeId}")]
        public HttpResponseMessage GetAllReferentielEtenduAsChapitreList(int societeId)
        {
            return Get(() => mapper.Map<List<ChapitreModel>>(referentielEtenduMgr.GetAllReferentielEtenduAsChapitreList(societeId)));
        }

        /// <summary>
        ///   POST Ajout/Mise à jour/Suppression d'un référentiel étendu
        /// </summary>
        /// <param name="refEtenduList">Liste des référentiels étendus</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPost]
        [Route("api/ReferentielEtendu/ManageReferentielEtenduList")]
        public HttpResponseMessage ManageReferentielEtenduList(IEnumerable<ReferentielEtenduModel> refEtenduList)
        {
            return Post(() => referentielEtenduMgr.ManageReferentielEtenduList(mapper.Map<IEnumerable<ReferentielEtenduEnt>>(refEtenduList.ToList())));
        }

        /// <summary>
        /// Méthode de génération d'une liste de ReferentielEtendu au format excel
        /// </summary>   
        /// <param name="societeId">societeId</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpGet]
        [Route("api/ReferentielEtendu/GenerateExcel/{societeId}")]
        public HttpResponseMessage GenerateExcel(int societeId)
        {
            return Get(() =>
            {
                ExcelFormat excelFormat = new ExcelFormat();
                string pathName = HttpContext.Current.Server.MapPath("/Templates/TemplateReferentielEtendu.xls").ToString();
                var result = new List<ReferentielEtenduModel>();
                var chapitre = mapper.Map<List<ChapitreModel>>(referentielEtenduMgr.GetAllReferentielEtenduAsChapitreList(societeId));
                if (chapitre != null)
                {
                    var sousChapitres = chapitre.SelectMany(c => c.SousChapitres).ToList();
                    var ressources = sousChapitres.SelectMany(c => c.Ressources).ToList();
                    result = ressources.SelectMany(c => c.ReferentielEtendus).ToList();
                }

                byte[] excelBytes = excelFormat.GenerateExcel<ReferentielEtenduModel>(pathName, result);

                var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(30), Priority = CacheItemPriority.NotRemovable };
                var cacheId = Guid.NewGuid().ToString();
                MemoryCache.Default.Add("excelBytes_" + cacheId, excelBytes, policy);

                return new { id = cacheId };
            });
        }

        /// <summary>
        /// Méthode d'extraction d'une liste de commandes au format excel
        /// </summary>
        /// <param name="id">Identifiant de l'export excel</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/ReferentielEtendu/ExtractExcel/{id}")]
        public HttpResponseMessage ExtractExcel(string id)
        {
            var cacheName = exportDocumentService.GetCacheName(id, isPdf: false);
            var bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }
            var exportDocument = exportDocumentService.GetDocumentFileName(fileName: "ExtractReferentielEtendu", isPdf: false);
            return exportDocumentService.CreateResponseForDownloadDocument(exportDocument, bytes);
        }
    }
}
