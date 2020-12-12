using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Fred.Business.Common.ExportDocument;
using Fred.Business.EtatPaie;
using Fred.Business.Rapport;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Utilisateur;
using Fred.Framework;
using Fred.Framework.Reporting;
using Fred.Web.Shared.Models.EtatPaie;

namespace Fred.Web.API
{
    public class EtatPaieController : ApiControllerBase
    {
        private readonly string templateFolderPath = HttpContext.Current.Server.MapPath("/Templates/");

        /// <summary>
        ///   Manager des pointage
        /// </summary>
        protected readonly IPointageManager pointageManager;

        /// <summary>
        ///   Manager des pointage
        /// </summary>
        protected readonly IEtatPaieManager etatPaieManager;

        /// <summary>
        ///   Manager du mapper
        /// </summary>
        protected readonly IMapper mapper;

        /// <summary>
        ///   Manager de l'authentification
        /// </summary>
        protected readonly ILogManager logMgr;

        /// <summary>
        ///   Manager des utilisateurs
        /// </summary>
        protected readonly IUtilisateurManager utilisateurManager;

        private readonly IExportDocumentService exportDocumentService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="manager">manager</param>
        /// <param name="pointageManager">pointageManager</param>
        /// <param name="etatPaieManager">etatPaieManager</param>
        /// <param name="mapper">mapper</param>
        /// <param name="logMgr">logMgr</param>
        /// <param name="utilisateurManager">utilisateurManager</param>   
        /// <param name="exportDocumentService">exportDocumentService</param>
        public EtatPaieController(IRapportManager manager, IPointageManager pointageManager, IEtatPaieManager etatPaieManager, IMapper mapper, ILogManager logMgr, IUtilisateurManager utilisateurManager, IExportDocumentService exportDocumentService)
        {
            this.pointageManager = pointageManager;
            this.mapper = mapper;
            this.logMgr = logMgr;
            this.utilisateurManager = utilisateurManager;
            this.etatPaieManager = etatPaieManager;
            this.exportDocumentService = exportDocumentService;
        }


        /// <summary>
        ///   Renvoie le model pour les éditions de paie
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/EtatPaie/GetEtatPaieExportModel")]
        public HttpResponseMessage GetEtatPaieExportModel()
        {
            return Get(() => new EtatPaieExportModel());
        }

        /// <summary>
        ///   Méthode de génération
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>Identifiant guid du document</returns>
        [HttpPost]
        [Route("api/EtatPaie/GenerateExcelVerificationTemps")]
        public object GenerateExcelVerificationTemps(EtatPaieExportModel etatPaieExportModel)
        {
            try
            {
                MemoryStream stream = etatPaieManager.GenerateVerificationTemps(etatPaieExportModel, utilisateurManager.GetContextUtilisateurId(), templateFolderPath);

                return TransformMemoryStreamToPdfOrExcel(etatPaieExportModel.Pdf, stream);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        ///   Méthode de génération
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>Identifiant guid du document</returns>
        [HttpPost]
        [Route("api/EtatPaie/GenerateExcelControlePointages")]
        public object GenerateExcelControlePointages(EtatPaieExportModel etatPaieExportModel)
        {
            try
            {
                var stream = etatPaieManager.GenerateControlePointages(etatPaieExportModel, utilisateurManager.GetContextUtilisateurId(), templateFolderPath);

                return TransformMemoryStreamToPdfOrExcel(etatPaieExportModel.Pdf, stream);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
        /// <summary>
        ///   Méthode de génération
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>Identifiant guid du document</returns>
        [HttpPost]
        [Route("api/EtatPaie/GenerateExcelControlePointagesFes")]
        public object GenerateExcelControlePointagesFes(EtatPaieExportModel etatPaieExportModel)
        {
            return Post(() =>
            {
                MemoryStream stream = etatPaieManager.GenerateControlePointagesFes(etatPaieExportModel, utilisateurManager.GetContextUtilisateurId(), templateFolderPath);

                return TransformMemoryStreamToPdfOrExcel(etatPaieExportModel.Pdf, stream);
            });
        }

        /// <summary>
        ///   Méthode de génération
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>Identifiant guid du document</returns>
        [HttpPost]
        [Route("api/EtatPaie/GenerateExcelControlePointagesHebdomadaire")]
        public object GenerateExcelControlePointagesHebdomadaire(EtatPaieExportModel etatPaieExportModel)
        {
            return Post(() =>
            {
                MemoryStream stream = etatPaieManager.GenerateControlePointagesHebdomadaire(etatPaieExportModel, utilisateurManager.GetContextUtilisateur(), templateFolderPath);

                return TransformMemoryStreamToPdfOrExcel(etatPaieExportModel.Pdf, stream);
            });
        }

        /// <summary>
        ///   Point d'entrée de l'état de la paie Liste des primes
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>Identifiant guid du document</returns>
        [HttpPost]
        [Route("api/EtatPaie/GenerateDocumentListePrimes")]
        public object GenerateDocumentListePrimes(EtatPaieExportModel etatPaieExportModel)
        {
            return Post(() =>
            {
                MemoryStream stream = etatPaieManager.GenerateDocumentListePrimes(etatPaieExportModel, utilisateurManager.GetContextUtilisateurId(), templateFolderPath);

                return TransformMemoryStreamToPdfOrExcel(etatPaieExportModel.Pdf, stream);
            });
        }

        /// <summary>
        ///   Point d'entrée de l'état de la paie Liste des IGD
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>Identifiant guid du document</returns>
        [HttpPost]
        [Route("api/EtatPaie/GenerateDocumentListeIGD")]
        public object GenerateDocumentListeIGD(EtatPaieExportModel etatPaieExportModel)
        {
            try
            {
                MemoryStream stream = etatPaieManager.GenerateDocumentListeIGD(etatPaieExportModel, utilisateurManager.GetContextUtilisateurId(), templateFolderPath);

                return TransformMemoryStreamToPdfOrExcel(etatPaieExportModel.Pdf, stream);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Point d'entrée de l'état de la paie Edition récapitulative des heures spécifiques
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>Identifiant guid du document</returns>
        [HttpPost]
        [Route("api/EtatPaie/GenerateDocumentListeHeuresSpecifiques")]
        public object GenerateDocumentListeHeuresSpecifiques(EtatPaieExportModel etatPaieExportModel)
        {
            try
            {                
                MemoryStream stream = etatPaieManager.GenerateDocumentListeHeuresSpecifiques(etatPaieExportModel, utilisateurManager.GetContextUtilisateur(), templateFolderPath);

                return TransformMemoryStreamToPdfOrExcel(etatPaieExportModel.Pdf, stream);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        ///   Generate Excel Salarie Acompte
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>Identifiant guid du document</returns>
        [HttpPost]
        [Route("api/EtatPaie/GenerateExcelSalarieAcompte")]
        public object GenerateExcelSalarieAcompte(EtatPaieExportModel etatPaieExportModel)
        {
            try
            {
                MemoryStream stream = etatPaieManager.GenerateSalarieAcompte(etatPaieExportModel, utilisateurManager.GetContextUtilisateur(), templateFolderPath);

                return TransformMemoryStreamToPdfOrExcel(etatPaieExportModel.Pdf, stream);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        ///   Méthode d'extraction d'une liste de pointage mensuel au format excel
        /// </summary>
        /// <param name="id">id du cache</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/EtatPaie/ExtractExcel/{id}")]
        public HttpResponseMessage ExtractExcel(string id)
        {
            var cacheName = exportDocumentService.GetCacheName(id, isPdf: false);
            var bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }
            var exportDocument = exportDocumentService.GetDocumentFileName("ExtractPointageMensuel", false);
            return exportDocumentService.CreateResponseForDownloadDocument(exportDocument, bytes);
        }

        /// <summary>
        ///   Méthode d'extraction du document du cache
        /// </summary>
        /// <param name="id">id du cache</param>
        /// <param name="filename">nom du fichier</param>
        /// <param name="pdf">type de fichier, si oui pdf sinon excel</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/EtatPaie/ExtractDocument/{id}/{filename}/{pdf}")]
        public HttpResponseMessage ExtractDocument(string id, string filename, bool pdf)
        {
            var cacheName = exportDocumentService.GetCacheName(id, isPdf: pdf);
            var bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }
            var exportDocument = exportDocumentService.GetDocumentFileName(filename, pdf);
            var result = exportDocumentService.CreateResponseForDownloadDocument(exportDocument, bytes);
            return result;
        }

        /// <summary>
        ///   Méthode d'extraction d'une liste de pointage mensuel au format pdf
        /// </summary>
        /// <param name="id">id du cache</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/EtatPaie/ExtractPDF/{id}")]
        public HttpResponseMessage ExtractPDF(string id)
        {
            var cacheName = exportDocumentService.GetCacheName(id, isPdf: true);
            var bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }
            var exportDocument = exportDocumentService.GetDocumentFileName("ExtractPointageMensuel", true);
            return exportDocumentService.CreateResponseForDownloadDocument(exportDocument, bytes);
        }

        /// <summary>
        /// Transforme un MemoryStream en Pdf ou Excel
        /// </summary>
        /// <param name="pdf">True si on doit créer un PDF</param>
        /// <param name="stream">Objet MemoryStream à transformer</param>
        /// <returns>Un ID</returns>
        private object TransformMemoryStreamToPdfOrExcel(bool pdf, MemoryStream stream)
        {
            byte[] bytes = stream.GetBuffer();
            string typeCache;
            string cacheId = Guid.NewGuid().ToString();
            ExcelFormat excelFormat = new ExcelFormat();

            if (pdf)
            {
                typeCache = "pdfBytes_";
            }
            else
            {
                typeCache = "excelBytes_";
                // --> Comme ça, ça marche pour l'excel mais pas pour le pdf!        
                stream.Position = 0;
                bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
            }
            excelFormat.Dispose();
            stream.Dispose();
            CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(300), Priority = CacheItemPriority.NotRemovable };
            MemoryCache.Default.Add(typeCache + cacheId, bytes, policy);

            return new { id = cacheId };
        }

        /// <summary>
        ///   Generate Excel List Absences Mensuels
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>Identifiant guid du document</returns>
        [HttpPost]
        [Route("api/EtatPaie/GenerateExcelListAbsencesMensuels")]
        public object GenerateExcelListAbsencesMensuels(EtatPaieExportModel etatPaieExportModel)
        {
            try
            {
                MemoryStream stream = etatPaieManager.GenerateListeAbsencesMensuelles(etatPaieExportModel, utilisateurManager.GetContextUtilisateur(), templateFolderPath);

                return TransformMemoryStreamToPdfOrExcel(etatPaieExportModel.Pdf, stream);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


    }
}
