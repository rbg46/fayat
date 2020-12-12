using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Fred.Business;
using Fred.Business.Common.ExportDocument;
using Fred.Business.ReferentielEtendu;
using Fred.Business.Societe;
using Fred.Entities.Organisation;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.ReferentielFixe;
using Fred.Framework.Reporting;
using Fred.Web.Models.Referential;
using Fred.Web.Models.ReferentielEtendu;
using Fred.Web.Models.ReferentielFixe;
using Fred.Web.Shared.Models.ReferentielEtendu;

namespace Fred.Web.API
{
    public class ParamTarifsReferentielsController : ApiControllerBase
    {
        private readonly IReferentielEtenduManager referentielEtenduMgr;
        private readonly ISocieteManager societeMgr;
        private readonly IMapper mapper;
        private readonly IParametrageReferentielEtenduManager parametrageReferentielEtenduManager;
        private readonly IExportDocumentService exportDocumentService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="referentielEtenduManager">referentielEtenduManager</param>
        /// <param name="organisationManager">organisationManager</param>
        /// <param name="referentielFixeManager">referentielFixeManager</param>
        /// <param name="deviseManager">deviseManager</param>
        /// <param name="societeManager">societeManager</param>
        /// <param name="mpper">mpper</param>
        /// <param name="parametrageReferentielEtenduManager">parametrageReferentielEtenduManager</param>
        /// <param name="exportDocumentService">exportDocumentService</param>
        public ParamTarifsReferentielsController(IReferentielEtenduManager referentielEtenduManager,
                                                 ISocieteManager societeManager,
                                                 IMapper mpper,
                                                 IParametrageReferentielEtenduManager parametrageReferentielEtenduManager,
                                                 IExportDocumentService exportDocumentService)
        {
            this.referentielEtenduMgr = referentielEtenduManager;
            this.societeMgr = societeManager;
            this.mapper = mpper;
            this.parametrageReferentielEtenduManager = parametrageReferentielEtenduManager;
            this.exportDocumentService = exportDocumentService;
        }

        [HttpGet]
        [Route("api/ParamTarifsReferentiels")]
        public HttpResponseMessage Get(int id, int deviseId, string filter = "")
        {
            try
            {
                //On récupère l'intégralité des paramétrages Référentiels étendus (existants et non existants)
                var orgasAndlistParamRefEtendus = this.referentielEtenduMgr.GetParametrageReferentielEtendu(id, deviseId, filter);

                IEnumerable<OrganisationEnt> orgaList = orgasAndlistParamRefEtendus.Item1;

                IEnumerable<ChapitreEnt> chapitres = orgasAndlistParamRefEtendus.Item2;

                var headerColumns = new List<CollectionListItem>();

                int order = 0;

                foreach (var orga in orgaList)
                {
                    var header = new CollectionListItem()
                    {
                        Text = orga.TypeOrganisation.Code,
                        Order = order++
                    };

                    headerColumns.Add(header);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new ParametrageResponseObject { HeaderColumns = headerColumns.OrderBy(h => h.Order).ToList(), Referentiels = this.mapper.Map<IEnumerable<ChapitreModel>>(chapitres) });
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [Route("api/UpdateParametrages")]
        public HttpResponseMessage Post(ParametrageReferentielEtenduModel data)
        {
            return this.Post(() =>
             {
                 ParametrageReferentielEtenduEnt parametrageReferentielEtenduEnt = this.mapper.Map<ParametrageReferentielEtenduEnt>(data);

                 return parametrageReferentielEtenduManager.AddOrUpdateParametrageReferentielEtendu(parametrageReferentielEtenduEnt);
             });
        }


        [HttpDelete]
        [Route("api/ParamTarifsReferentiels/DeleteParam/{parametrageReferentielEtenduModelId}")]
        public HttpResponseMessage Delete(int parametrageReferentielEtenduModelId)
        {
            return Delete(() => this.parametrageReferentielEtenduManager.DeleteById(parametrageReferentielEtenduModelId));
        }


        [HttpPost]
        [Route("api/ParamTarifsReferentiels")]
        public HttpResponseMessage Post(ParametrageReferentielEtenduModel[] data)
        {
            try
            {
                List<ParametrageReferentielEtenduEnt> mappedParametrages =
                  this.mapper.Map<List<ParametrageReferentielEtenduEnt>>(data);
                foreach (var parametrage in mappedParametrages)
                {
                    referentielEtenduMgr.AddOrUpdateParametrageReferentielEtendu(parametrage);
                }
                return Request.CreateResponse(HttpStatusCode.OK, data.ToList());
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [Route("api/ParamTarifsReferentiels/ListDevises/{orgaId}")]
        public HttpResponseMessage Get(int orgaId)
        {
            try
            {
                var orga = this.societeMgr.GetSocieteParentByOrgaId(orgaId);
                var devisesEnt = this.societeMgr.GetListDeviseBySocieteId(orga.SocieteId);
                var devises = this.mapper.Map<List<DeviseModel>>(devisesEnt);
                return Request.CreateResponse(HttpStatusCode.OK, devises);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        /// <summary>
        /// Méthode de génération d'une liste de commandes au format excel
        /// TODO: remplacer les chemins spécifiés par des valeurs remontées depuis le paramétrage
        ///       spécifier taille des fichiers ?
        /// </summary>
        /// <param name="orgaId">Identifiant organisation</param>
        /// <param name="deviseId">Identifiant devise</param>
        /// <param name="filter">Filtre</param>
        /// <returns>Identifiant du cache</returns>
        [HttpPost]
        [Route("api/ParamReferentielEtendu/GenerateExcel")]
        public object GenerateExcel(int orgaId, int deviseId, string filter)
        {
            string pathName = HttpContext.Current.Server.MapPath("/Templates/TemplateParamReferentielEtendu.xlsx").ToString();

            var dataForExport = this.referentielEtenduMgr.GenerateListForExportExcel(orgaId, deviseId, filter);

            var mappedDataForExport = this.mapper.Map<List<ParametrageReferentielEtenduExportModel>>(dataForExport);

            var cacheId = Guid.NewGuid().ToString();

            // Génération du fichier excel
            using (ExcelFormat excelFormat = new ExcelFormat())
            {
                byte[] excelfile = excelFormat.GenerateExcel(pathName, mappedDataForExport);

                // Ajout dans le cache
                var policy = new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(30),
                    Priority = CacheItemPriority.NotRemovable
                };

                MemoryCache.Default.Add("excelBytes_" + cacheId, excelfile, policy);
            }

            return new { id = cacheId };
        }


        /// <summary>
        /// Méthode d'extraction d'une liste de commandes au format excel
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>HttpResponseMessage</returns>
        /// 
        [HttpGet]
        [Route("api/ParamReferentielEtendu/ExtractExcel/{id}")]
        public HttpResponseMessage ExtractExcel(string id)
        {
            var cacheName = exportDocumentService.GetCacheName(id, isPdf: false);
            var bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }
            var exportDocument = exportDocumentService.GetDocumentFileName(fileName: "ExtractParamReferentielEtendu" + id, isPdf: false);
            return exportDocumentService.CreateResponseForDownloadDocument(exportDocument, bytes);
        }

        public class CollectionListItem
        {
            public int Order { get; set; }
            public string Text { get; set; }
        }

        private class ParametrageResponseObject
        {
            public List<CollectionListItem> HeaderColumns { get; set; }
            public IEnumerable<ChapitreModel> Referentiels { get; set; }

        }
    }
}