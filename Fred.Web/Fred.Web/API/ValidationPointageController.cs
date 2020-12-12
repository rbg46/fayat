using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Fred.Business.Common.ExportDocument;
using Fred.Business.ExternalService.ValidationPointage;
using Fred.Business.Utilisateur;
using Fred.Business.ValidationPointage;
using Fred.Entities.CI;
using Fred.Entities.ValidationPointage;
using Fred.Web.Models;
using Fred.Web.Shared.Models;

namespace Fred.Web.API
{
    public class ValidationPointageController : ApiControllerBase
    {
        private readonly IMapper mapper;
        private readonly IExportDocumentService exportDocumentService;
        private readonly IValidationPointageManagerExterne validationPointageManagerExterne;
        private readonly IValidationPointageManager validationPointageManager;
        private readonly ILotPointageManager lotPointageManager;
        private readonly IRemonteeVracManager remonteeVracManager;
        private readonly IUtilisateurManager utilisateurManager;

        public ValidationPointageController(IMapper mapper,
            IExportDocumentService exportDocumentService,
            IValidationPointageManagerExterne validationPointageManagerExterne,
            IValidationPointageManager validationPointageManager,
            ILotPointageManager lotPointageManager,
            IRemonteeVracManager remonteeVracManager,
            IUtilisateurManager utilisateurManager)
        {
            this.mapper = mapper;
            this.exportDocumentService = exportDocumentService;
            this.validationPointageManagerExterne = validationPointageManagerExterne;
            this.validationPointageManager = validationPointageManager;
            this.lotPointageManager = lotPointageManager;
            this.remonteeVracManager = remonteeVracManager;
            this.utilisateurManager = utilisateurManager;
        }

        /// <summary>
        ///   GET Récupère les lots de pointages comprenant :
        ///     - le lot de pointage de l'utilisateur connecté
        ///     - les lots de pointages des autres utilisateurs ayant verrouillés des rapports dans le même périmètre et la même période
        ///   Rappel : un lot de pointage représente un ensemble de "pointages" (RapportLigneEnt) dont le Rapport a été verrouillé par un utilisateur
        /// </summary>        
        /// <param name="periode">Période sélectionné</param>
        /// <returns>Retourne la liste des lots de pointages en fonction de l'utilisateur</returns>
        [HttpGet]
        [Route("api/ValidationPointage/{periode:datetime}")]
        public IHttpActionResult GetAllLotPointage(DateTime periode)
        {
            IEnumerable<LotPointageEnt> lotPointages = validationPointageManager.GetAllLotPointage(periode);
            var lotPointageModels = mapper.Map<IEnumerable<LotPointageModel>>(lotPointages);

            return Ok(lotPointageModels);
        }

        /// <summary>
        ///   GET Récupère le nombre de pointages dont les rapports n'ont pas été verrouillés
        /// </summary>
        /// <param name="periode">Période sélectionné</param>
        /// <returns>Nombre de pointages non verrouillés pour la période</returns>
        [HttpGet]
        [Route("api/ValidationPointage/AucunVerrouillageCount/{periode:datetime}")]
        public IHttpActionResult AucunVerrouillageCount(DateTime periode)
        {
            validationPointageManager.CountPointageNonVerrouille(periode);

            return Ok();
        }

        /// <summary>
        ///   POST Exécution du controle chantier pour un lot de pointage pour une période donnée
        /// </summary>
        /// <param name="lotPointageId">Identifiant du Lot de Pointage</param>       
        /// <returns>Lot de pointage avec statut en cours</returns>
        [HttpPost]
        [Route("api/ValidationPointage/ControleChantier/{lotPointageId}")]
        public async Task<IHttpActionResult> ExecuteControleChantierAsync(int lotPointageId)
        {
            int userId = utilisateurManager.GetContextUtilisateurId();
            ControlePointageEnt controlePointage = await validationPointageManager.ExecuteControleChantierAsync(lotPointageId, userId);
            var controlePointageModels = mapper.Map<ControlePointageModel>(controlePointage);

            return Ok(controlePointageModels);
        }

        /// <summary>
        ///   POST Exécution du controle vrac pour un lot de pointage pour une période donnée
        /// </summary>
        /// <param name="lotPointageId">Identifiant du lot de pointage</param>    
        /// <param name="filtre">Filtre pour lot de pointage</param>
        /// <returns>Lot de pointage avec statut en cours</returns>
        [HttpPost]
        [Route("api/ValidationPointage/ControleVrac/{lotPointageId}")]
        public async Task<IHttpActionResult> ExecuteControleVrac(int lotPointageId, PointageFiltreModel filtre)
        {
            ControlePointageResult controlePointage = await validationPointageManagerExterne.ExecuteControleVracAsync(lotPointageId, mapper.Map<PointageFiltre>(filtre));

            return Ok(controlePointage);
        }

        /// <summary>
        ///   POST Vérification du passage Ci Sep au CI interne avant l'execution du controle vrac
        /// </summary>
        /// <param name="lotPointageId">Identifiant du lot de pointage</param>    
        /// <param name="filtre">Filtre pour lot de pointage</param>
        /// <returns>Liste Des Ci Sep non configurés</returns>
        [HttpPost]
        [Route("api/ValidationPointage/ControleVrac/VerificationCiSep/{lotPointageId}")]
        public IHttpActionResult VerificationCiSepControleVrac(int lotPointageId, PointageFiltreModel filtre)
        {
            List<CIEnt> cis = validationPointageManager.VerificationCiSep(lotPointageId, null, mapper.Map<PointageFiltre>(filtre));
            var ciModels = mapper.Map<List<CILightModel>>(cis);

            return Ok(ciModels);
        }

        /// <summary>
        ///   POST Apport du visa sur un lot de pointage
        /// </summary>
        /// <param name="lotPointageId">Identifiant du lot de pointage</param>    
        /// <returns>Lot de pointage signé</returns>
        [HttpPost]
        [Route("api/ValidationPointage/Visa/{lotPointageId}")]
        public IHttpActionResult ExecuteVisa(int lotPointageId)
        {
            LotPointageEnt lotPointages = lotPointageManager.SignLotPointage(lotPointageId);
            var lotPointageModel = mapper.Map<LotPointageModel>(lotPointages);

            return Ok(lotPointageModel);
        }

        /// <summary>
        ///   POST Exécution de la remontée vrac pour une période donnée
        /// </summary>        
        /// <param name="periode">Période choisie</param>
        /// <param name="filtre">Filtre pour lot de pointage</param>
        /// <returns>Lot de pointage avec statut en cours</returns>
        [HttpPost]
        [Route("api/ValidationPointage/RemonteeVrac/{periode}")]
        public async Task<IHttpActionResult> ExecuteRemonteeVrac(DateTime periode, PointageFiltreModel filtre)
        {
            RemonteeVracResult remonteeVrac = await validationPointageManagerExterne.ExecuteRemonteeVracAsync(periode, mapper.Map<PointageFiltre>(filtre));

            return Ok(remonteeVrac);
        }

        /// <summary>
        ///   POST Vérification du passage Ci Sep au CI interne avant l'execution du remontee vrac
        /// </summary>
        /// <param name="periode">Période choisie</param>
        /// <param name="filtre">Filtre pour lot de pointage</param>
        /// <returns>Liste Des Ci Sep non configurés</returns>
        [HttpPost]
        [Route("api/ValidationPointage/RemonteeVrac/VerificationCiSep/{periode}")]
        public IHttpActionResult VerificationCiSepRemonteeVrac(DateTime periode, PointageFiltreModel filtre)
        {
            List<CIEnt> cis = validationPointageManager.VerificationCiSep(null, periode, mapper.Map<PointageFiltre>(filtre));
            var ciModels = mapper.Map<List<CILightModel>>(cis);

            return Ok(ciModels);
        }

        /// <summary>
        ///   POST Exécution de la remontée prime pour une période donnée
        /// </summary>        
        /// <param name="periode">Période choisie</param>
        /// <returns>Lot de pointage avec statut en cours</returns>
        [HttpPost]
        [Route("api/ValidationPointage/RemonteePrimes/{periode}")]
        public async Task<IHttpActionResult> ExecuteRemonteePrimeAsync(DateTime periode)
        {
            await validationPointageManagerExterne.ExecuteRemonteePrimeAsync(periode);

            return Ok();
        }

        /// <summary>
        ///   GET Récupère un nouveau filtre pour le contrôle vrac ou remontée vrac
        /// </summary>
        /// <param name="typeControle">Type contrôle pour le filtre</param>
        /// <returns>Filtre</returns>
        [HttpGet]
        [Route("api/ValidationPointage/Filter/{typeControle}")]
        public IHttpActionResult GetNewFilter(int typeControle)
        {
            PointageFiltre pointageFilter = validationPointageManager.GetFilter(typeControle);
            var pointageFilterModel = mapper.Map<PointageFiltreModel>(pointageFilter);

            return Ok(pointageFilterModel);
        }

        /// <summary>
        ///   GET Récupère la liste du personnels avec leur erreurs de contrôle
        /// </summary>
        /// <param name="controlePointageId">Identifiant du controle pointage</param>    
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="searchText">Texte a rechercher</param>
        /// <returns>Liste du personnel avec erreurs</returns>
        [HttpGet]
        [Route("api/ValidationPointage/ControlePointageErreur/{controlePointageId}/{page?}/{pageSize?}/{searchText?}")]
        public IHttpActionResult GetControlePointageErreurList(int controlePointageId, int page = 1, int pageSize = 20, string searchText = "")
        {
            SearchValidationResult<ControlePointageErreurEnt> result = validationPointageManager.GetAllPersonnelList(controlePointageId, searchText, page, pageSize);
            var resultModel = mapper.Map<SearchValidationResultModel<ControlePointageErreurModel>>(result);

            return Ok(resultModel);
        }

        /// <summary>
        ///   GET Récupère la liste du personnels avec leur erreurs de remontée vrac
        /// </summary>
        /// <param name="remonteeVracId">Identifiant de la remontée vrac</param>    
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="searchText">Texte a rechercher</param>
        /// <returns>Liste du personnel avec erreurs</returns>
        [HttpGet]
        [Route("api/ValidationPointage/RemonteeVracErreur/{remonteeVracId}/{page?}/{pageSize?}/{searchText?}")]
        public IHttpActionResult GetRemonteeVracErreurList(int remonteeVracId, int page = 1, int pageSize = 20, string searchText = "")
        {
            SearchValidationResult<RemonteeVracErreurEnt> result = validationPointageManager.GetRemonteeVracErreurList(remonteeVracId, searchText, page, pageSize);
            var resultModel = mapper.Map<SearchValidationResultModel<RemonteeVracErreurModel>>(result);

            return Ok(resultModel);
        }

        /// <summary>
        ///   Récupère la dernière Remontée vrac d'un utilisateur
        /// </summary>
        /// <param name="periode">Période choisie</param>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        /// <returns>Dernière remontée vrac</returns>
        [HttpGet]
        [Route("api/ValidationPointage/GetLastRemonteeVrac/{periode}/{utilisateurId}")]
        public IHttpActionResult GetLastRemonteeVrac(DateTime periode, int utilisateurId)
        {
            RemonteeVracEnt remonteeVrac = remonteeVracManager.GetLatest(utilisateurId, periode);
            var remonteeVracModel = mapper.Map<RemonteeVracModel>(remonteeVrac);

            return Ok(remonteeVracModel);
        }

        /// <summary>
        ///   Télécharge l'export PDF des erreurs de validation (Contrôle Chantier et vrac)
        /// </summary>
        /// <param name="controlePointageId">Identifiant du controle pointage</param>
        /// <returns>Document PDF</returns>
        [HttpGet]
        [Route("api/ValidationPointage/ControlePointageErreur/Export/{controlePointageId}")]
        public HttpResponseMessage GetControlePointageErreurPdf(int controlePointageId)
        {
            string fileName = validationPointageManager.GetControlePointageErreurFilename(controlePointageId);
            byte[] data = validationPointageManager.GetControlePointageErreurPdf(controlePointageId);
            HttpResponseMessage response = CreateDownloadResponse(fileName, data);

            return response;
        }

        /// <summary>
        ///   Télécharge l'export PDF des erreurs de remontée vrac
        /// </summary>
        /// <param name="remonteeVracId">Identifiant de la remontée vrac</param>
        /// <returns>Document PDF</returns>
        [HttpGet]
        [Route("api/ValidationPointage/RemonteeVracErreur/Export/{remonteeVracId}")]
        public HttpResponseMessage GetRemonteeVracErreurPdf(int remonteeVracId)
        {
            string fileName = validationPointageManager.GetRemonteeVracErreurFilename(remonteeVracId);
            byte[] data = validationPointageManager.GetRemonteeVracErreurPdf(remonteeVracId);
            HttpResponseMessage response = CreateDownloadResponse(fileName, data);

            return response;
        }

        private HttpResponseMessage CreateDownloadResponse(string fileName, byte[] data)
        {
            string attachmentFileName = exportDocumentService.GetDocumentFileName(fileName, true);
            HttpResponseMessage response = exportDocumentService.CreateResponseForDownloadDocument(attachmentFileName, data);

            return response;
        }
    }
}
