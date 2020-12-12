using AutoMapper;
using Fred.Business.CI;
using Fred.Business.Common.ExportDocument;
using Fred.Business.ExternalService.EcritureComptable;
using Fred.Business.OperationDiverse;
using Fred.Business.Organisation.Tree;
using Fred.Business.Referential.Tache;
using Fred.Business.RepartitionEcart;
using Fred.Entities.OperationDiverse;
using Fred.Entities.OperationDiverse.Excel;
using Fred.Entities.Referential;
using Fred.Framework.Reporting;
using Fred.Web.Models.Referential;
using Fred.Web.Models.RepartitionEcart;
using Fred.Web.Shared.Models.OperationDiverse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Fred.Web.API
{
    public class OperationDiverseController : ApiControllerBase
    {
        private const string FirstFamilyCodeOrder = "RCT";
        private readonly IMapper mapper;
        private readonly IEcritureComptableManagerExterne ecritureComptableManagerExterne;
        private readonly ICIManager ciManager;
        private readonly IOperationDiverseManager operationDiverseManager;
        private readonly IRepartitionEcartManager repartitionEcartManager;
        private readonly ITacheManager tacheManager;
        private readonly IConsolidationManager consolidationManager;
        private readonly IExportDocumentService exportDocumentService;
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IOperationDiverseAbonnementManager operationDiverseAbonnementManager;

        public OperationDiverseController(IMapper mapper,
            ICIManager ciManager,
            IOperationDiverseManager operationDiverseManager,
            IRepartitionEcartManager repartitionEcartManager,
            IEcritureComptableManagerExterne ecritureComptableManagerExterne,
            ITacheManager tacheManager,
            IConsolidationManager consolidationManager,
            IExportDocumentService exportDocumentService,
            IOrganisationTreeService organisationTreeService,
            IOperationDiverseAbonnementManager operationDiverseAbonnementManager)
        {
            this.mapper = mapper;
            this.ecritureComptableManagerExterne = ecritureComptableManagerExterne;
            this.ciManager = ciManager;
            this.operationDiverseManager = operationDiverseManager;
            this.repartitionEcartManager = repartitionEcartManager;
            this.tacheManager = tacheManager;
            this.consolidationManager = consolidationManager;
            this.exportDocumentService = exportDocumentService;
            this.organisationTreeService = organisationTreeService;
            this.operationDiverseAbonnementManager = operationDiverseAbonnementManager;
        }

        /// <summary>
        /// Retourne l'identifiant de la société d'un CI
        /// </summary>
        /// <param name="organisationCiId">Identifiant de l'organisation du CI</param>
        /// <returns>Identifiant de la société du CI</returns>
        [HttpGet]
        [Route("api/OperationDiverse/GetCiSocietyId/{organisationCiId}")]
        public IHttpActionResult GetCiSocietyId(int organisationCiId)
        {
            return Ok(organisationTreeService.GetOrganisationTree().GetSocieteParent(organisationCiId).Id);
        }

        /// <summary>
        /// Verifie si on peux faire un import
        /// </summary>
        /// <param name="societeId">societeId</param>    
        /// <param name="ciId">ciId</param>
        /// <param name="date">date</param>
        /// <returns>object avec canImport = true ou false</returns>
        [HttpGet]
        [Route("api/OperationDiverse/CanImportEcritureComptables/{societeId}/{ciId}/{date}")]
        public async Task<IHttpActionResult> CanImportEcritureComptables(int societeId, int ciId, DateTime date)
        {
            bool canImport = await ecritureComptableManagerExterne.CanImportEcritureComptablesAsync(societeId, ciId, date);
            return Ok(new { canImport });
        }

        /// <summary>
        /// Execute l'import des ecriture comptables
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="ciId">ciId</param>
        /// <param name="date">date</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/OperationDiverse/ImportEcritureComptables/{societeId}/{ciId}/{date}")]
        public async Task<IHttpActionResult> ImportEcritureComptablesAsync(int societeId, int ciId, DateTime date)
        {
            return Ok(await ecritureComptableManagerExterne.ImportEcritureComptablesAsync(societeId, ciId, date));
        }

        /// <summary>
        /// Execute l'import des ecriture comptables
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="ciId">ciId</param>
        /// <param name="dateDebut">Date de début</param>
        /// <param name="dateFin">Date de fin</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/OperationDiverse/ImportEcritureComptables/{societeId}/{ciId}/{dateDebut}/{dateFin}")]
        public async Task<IHttpActionResult> ImportEcritureComptablesAsync(int societeId, int ciId, DateTime dateDebut, DateTime dateFin)
        {
            return Ok(await ecritureComptableManagerExterne.ImportEcritureComptablesAsync(societeId, ciId, dateDebut, dateFin));
        }

        /// <summary>
        /// Permet la création d'une OD
        /// </summary>
        /// <param name="operationDiverse">><see cref="OperationDiverseEnt"/></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/OperationDiverse/CreateOD/")]
        public IHttpActionResult CreateOD(OperationDiverseEnt operationDiverse)
        {
            return Ok(operationDiverseManager.AddOperationDiverse(mapper.Map<OperationDiverseEnt>(operationDiverse)));
        }

        /// <summary>
        /// Permet la création d'un abonnement d'OD
        /// </summary>
        /// <param name="operationDiverse">><see cref="OperationDiverseAbonnementModel"/></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/OperationDiverse/CreateODAbonnement/")]
        public IHttpActionResult CreateODAbonnement(OperationDiverseAbonnementModel operationDiverse)
        {
            return Ok(operationDiverseAbonnementManager.Add(mapper.Map<OperationDiverseAbonnementModel>(operationDiverse)));
        }

        /// <summary>
        /// Met à jour un OD
        /// </summary>
        /// <param name="operationDiverse">><see cref="OperationDiverseEnt"/></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/OperationDiverse/Update/")]
        public async Task<IHttpActionResult> UpdateAsync(OperationDiverseEnt operationDiverse)
        {
            return Ok(await operationDiverseManager.UpdateAsync(operationDiverse));
        }

        /// <summary>
        /// Met à jour un abonnement d'OD
        /// </summary>
        /// <param name="operationDiverse">><see cref="OperationDiverseAbonnementModel"/></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/OperationDiverse/UpdateAbonnement/")]
        public IHttpActionResult UpdateAbonnement(OperationDiverseAbonnementModel operationDiverse)
        {
            return Ok(operationDiverseAbonnementManager.Update(operationDiverse));
        }

        /// <summary>
        /// Détache plusieurs OD simultanément
        /// </summary>
        /// <param name="operationsDiverses">><see cref="OperationDiverseEnt"/></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/OperationDiverse/UpdateList/")]
        public IHttpActionResult UpdateList(List<OperationDiverseAbonnementModel> operationsDiverses)
        {
            List<OperationDiverseEnt> odsToUpdate = mapper.Map<List<OperationDiverseEnt>>(operationsDiverses);
            List<OperationDiverseAbonnementModel> odsAbonnement = mapper.Map<List<OperationDiverseAbonnementModel>>(operationDiverseManager.Update(odsToUpdate));
            return Ok(odsAbonnement);
        }

        /// <summary>
        /// Recuperation des ecarts
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="date">date</param>
        /// <returns>RepartitionEcartWrapperModel</returns>
        [HttpGet]
        [Route("api/OperationDiverse/GetRepartitionsEcarts/{ciId}/{date}")]
        public async Task<IHttpActionResult> GetRepartitionsEcartsAsync(int ciId, DateTime date)
        {
            return Ok(mapper.Map<RepartitionEcartWrapperModel>(await repartitionEcartManager.GetByCiIdAndDateComptableAsync(ciId, date).ConfigureAwait(false)));
        }

        /// <summary>
        /// Recupere la liste des tache du ci
        /// </summary>
        /// <param name="ci">ci</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="recherche">recherche</param>
        /// <returns>Liste de taches</returns>
        [HttpGet]
        [Route("api/OperationDiverse/GetTaches/{ci}/{page?}/{pageSize?}/{recherche?}")]
        public IHttpActionResult GetTaches(int ci, int page = 1, int pageSize = 20, string recherche = "")
        {
            return Ok(mapper.Map<IEnumerable<TacheModel>>(tacheManager.SearchLight(recherche, page, pageSize, ci)));
        }

        /// <summary>
        /// Sauvagerde une liste d'od
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">dateComptable</param>
        /// <param name="operationDiverses">operationDiverses</param>
        /// <returns>success</returns>
        [HttpPost]
        [Route("api/OperationDiverse/{ciId}/{dateComptable}")]
        public IHttpActionResult Save(int ciId, DateTime dateComptable, List<OperationDiverseAbonnementModel> operationDiverses)
        {
            operationDiverseManager.Save(ciId, dateComptable, mapper.Map<IEnumerable<OperationDiverseEnt>>(operationDiverses));
            return Ok(new { result = "success" });
        }

        /// <summary>
        /// Supprime une opération diverse
        /// </summary>
        /// <param name="operationDiverse">Opération diverse à supprimer</param>
        /// <returns>Réponse HTTP</returns>
        [HttpPost]
        [Route("api/OperationDiverse/Delete/")]
        public IHttpActionResult Delete(OperationDiverseEnt operationDiverse)
        {
            operationDiverseManager.Delete(operationDiverse);
            return Ok();
        }

        /// <summary>
        /// Supprime tout ou partie d'un abonnement d'OD
        /// </summary>
        /// <param name="operationDiverse">Opération diverse à supprimer</param>
        /// <returns>Réponse HTTP</returns>
        [HttpPost]
        [Route("api/OperationDiverse/DeleteAbonnement/")]
        public IHttpActionResult Delete(OperationDiverseAbonnementModel operationDiverse)
        {
            operationDiverseAbonnementManager.Delete(operationDiverse);
            return Ok();
        }

        /// <summary>
        /// Retourne les écritures comptable pour un CI pour une date (mois) données et une famille d'OD
        /// </summary>
        /// <param name="ciId">Le CI</param>
        /// <param name="date">Date(mois)</param>
        /// <param name="famille">Famille d'OD</param>
        /// <returns>Données consolidées</returns>
        [HttpGet]
        [Route("api/OperationDiverse/GetEcritureComptables/{ciId}/{date}/{famille}")]
        public async Task<IHttpActionResult> GetEcritureComptablesAsync(int ciId, DateTime date, int famille)
        {
            return Ok(await consolidationManager.GetEcritureComptablesAsync(ciId, date, famille).ConfigureAwait(false));
        }

        /// <summary>
        /// Retourne les OD non rattachée  pour un CI pour une date (mois) données et une famille d'OD
        /// </summary>
        /// <param name="ciId">Le CI</param>
        /// <param name="date">Date(mois)</param>
        /// <param name="famille">Famille d'OD</param>
        /// <returns>Données consolidées</returns>
        [HttpGet]
        [Route("api/OperationDiverse/GetNotRelatedOD/{ciId}/{date}/{famille}")]
        public async Task<IHttpActionResult> GetNotRelatedODAsync(int ciId, DateTime date, int famille)
        {
            return Ok(await consolidationManager.GetListNotRelatedODAsync(ciId, date, famille).ConfigureAwait(false));
        }

        /// <summary>
        /// Retourne les OD non rattachée pour une écriture comptable pour un CI pour une date (mois) données et une famille d'OD
        /// </summary>
        /// <param name="ciId">Le CI</param>
        /// <param name="date">Date(mois)</param>
        /// <param name="famille">Famille d'OD</param>
        /// <param name="selectedAccountingEntries">Chaîne des identifiant des écritures comptables sélectionnées, à convertir en liste pour l'exploiter</param>
        /// <returns>Données consolidées</returns>
        [HttpGet]
        [Route("api/OperationDiverse/GetRelatedOD/{ciId}/{date}/{famille}/{selectedAccountingEntries}")]
        public async Task<IHttpActionResult> GetRelatedODAsync(int ciId, DateTime date, int famille, string selectedAccountingEntries)
        {
            return Ok(await consolidationManager.GetListRelatedODAsync(ciId, date, famille, selectedAccountingEntries).ConfigureAwait(false));
        }

        /// <summary>
        /// Retourne les données consolidées pour un CI pour une date (mois) données
        /// </summary>
        /// <param name="ciId">Le CI</param>
        /// <param name="date">Date(mois)</param>
        /// <returns>Données consolidées</returns>
        [HttpGet]
        [Route("api/OperationDiverse/GetConsolidationDatas/{ciId}/{date}")]
        public async Task<IHttpActionResult> GetConsolidationDatasAsync(int ciId, DateTime date)
        {
            DeviseEnt devise = ciManager.GetDeviseRef(ciId);
            ConsolidationDataModel model = new ConsolidationDataModel { CurrencySymbol = devise == null ? "€" : devise.Symbole };
            IDictionary<FamilleOperationDiverseEnt, Tuple<decimal, decimal>> consolidations = await consolidationManager.GetConsolidationDatasAsync(ciId, date).ConfigureAwait(false);
            List<ConsolidationFamilyModel> familiesAmounts = new List<ConsolidationFamilyModel>(0);

            foreach (KeyValuePair<FamilleOperationDiverseEnt, Tuple<decimal, decimal>> consolidation in consolidations)
            {
                familiesAmounts.Add(new ConsolidationFamilyModel
                {
                    FamilyId = consolidation.Key.FamilleOperationDiverseId,
                    FamilyName = consolidation.Key.Libelle,
                    FamilyCode = consolidation.Key.Code,
                    MustHaveOrder = consolidation.Key.MustHaveOrder,
                    IsAccrued = consolidation.Key.IsAccrued,
                    FredAmount = consolidation.Value.Item1,
                    AccountingAmount = consolidation.Value.Item2
                });
            }
            model.FamiliesAmounts = familiesAmounts;
            return Ok(model);
        }

        /// <summary>
        /// Retourne les données consolidées pour un CI pour une période
        /// </summary>
        /// <param name="ciId">Le CI</param>
        /// <param name="dateDebut">Date de début</param>
        /// <param name="dateFin">Date de fin</param>
        /// <returns>Données consolidées</returns>
        [HttpGet]
        [Route("api/OperationDiverse/GetConsolidationDatas/{ciId}/{dateDebut?}/{dateFin}")]
        public async Task<IHttpActionResult> GetConsolidationDatasAsync(int ciId, DateTime? dateDebut, DateTime dateFin)
        {
            dateDebut = dateDebut ?? new DateTime(1900, 1, 1);

            DeviseEnt devise = ciManager.GetDeviseRef(ciId);
            IDictionary<FamilleOperationDiverseEnt, ConsolidationDetailPerAmountByMonthModel> consolidations = await consolidationManager.GetConsolidationDatasAsync(ciId, dateDebut.Value, dateFin).ConfigureAwait(false);
            ConsolidationDataModelByMonthModel model = new ConsolidationDataModelByMonthModel { CurrencySymbol = devise == null ? "€" : devise.Symbole };

            List<ConsolidationFamilyByMonthModel> familiesAmountsByMonth = new List<ConsolidationFamilyByMonthModel>(0);
            foreach (KeyValuePair<FamilleOperationDiverseEnt, ConsolidationDetailPerAmountByMonthModel> consolidation in consolidations)
            {
                familiesAmountsByMonth.Add(new ConsolidationFamilyByMonthModel
                {
                    FamilyId = consolidation.Key.FamilleOperationDiverseId,
                    FamilyName = consolidation.Key.Libelle,
                    FamilyCode = consolidation.Key.Code,
                    MustHaveOrder = consolidation.Key.MustHaveOrder,
                    IsAccrued = consolidation.Key.IsAccrued,
                    FredAmount = consolidation.Value.FredAmount,
                    AccountingAmount = consolidation.Value.AccountingAmount,
                    ListFredAmountByMonth = consolidation.Value.ListFredAmountByMonth,
                    ListAccountAmountByMonth = consolidation.Value.ListAccountingAmountByMonth,
                    ListGapAmountByMonth = consolidation.Value.ListGapAmountByMonth
                });
            }

            familiesAmountsByMonth = familiesAmountsByMonth.OrderBy(f => f.FamilyCode != FirstFamilyCodeOrder).ToList();

            model.FamiliesAmounts = familiesAmountsByMonth;

            return Ok(model);
        }

        /// <summary>
        /// Retourne la liste des OD pour une famille sur une période comptable
        /// </summary>
        /// <param name="ciID">Identifiant du CI</param>
        /// <param name="familyId">Identifiant de la famille d'OD</param>
        /// <param name="date">Date permettant de déterminer la période comptable</param>
        /// <returns>Liste des OD de la famille sur la période comptable</returns>
        [HttpGet]
        [Route("api/OperationDiverse/GetOdListByFamilyAndPeriod/{ciID}/{familyId}/{date}")]
        public async Task<IHttpActionResult> GetOdListByFamilyAndPeriodAsync(int ciID, int familyId, DateTime date)
        {
            IEnumerable<OperationDiverseEnt> list = (await operationDiverseManager.GetAllByCiIdAndDateComptableAsync(ciID, date).ConfigureAwait(false)).Where(q => q.FamilleOperationDiverseId == familyId);
            List<VentilationModel> listVentilation = new List<VentilationModel>();
            foreach (OperationDiverseEnt operationDiverse in list)
            {
                listVentilation.Add(new VentilationModel
                {
                    UnitName = operationDiverse.Unite.Libelle,
                    UnitId = operationDiverse.UniteId,
                    Libelle = operationDiverse.Libelle,
                    PUHT = operationDiverse.PUHT,
                    Quantity = operationDiverse.Quantite,
                    ResourceId = operationDiverse.RessourceId,
                    ResourceName = operationDiverse.Ressource?.CodeLibelle,
                    Amount = operationDiverse.Montant,
                    Commentaire = operationDiverse.Commentaire,
                    TaskId = operationDiverse.TacheId,
                    TaskName = operationDiverse.Tache?.CodeLibelle,
                    VentilationId = operationDiverse.OperationDiverseId
                });
            }
            return Ok(listVentilation);
        }

        /// <summary>
        /// Méthode de génération d'un fichier exemple de chargement OD au format excel
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateComptable">Date comptable</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPost]
        [Route("api/OperationDiverse/GenerateExempleExcel/{ciId}/{dateComptable}")]
        public async Task<IHttpActionResult> GenerateExempleExcel(int ciId, DateTime dateComptable)
        {
            byte[] excelBytes = await operationDiverseManager.GetFichierExempleChargementODAsync(ciId, dateComptable);

            CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(30), Priority = CacheItemPriority.NotRemovable };
            string cacheId = Guid.NewGuid().ToString();
            MemoryCache.Default.Add("excelBytes_" + cacheId, excelBytes, policy);

            return Ok(new { id = cacheId });
        }

        /// <summary>
        /// Méthode d'extraction du fichier excel
        /// </summary>
        /// <param name="id">Identifiant de l'export excel</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/OperationDiverse/ExtractExempleExcel/{id}")]
        public HttpResponseMessage ExtractExempleExcel(string id)
        {
            string cacheName = exportDocumentService.GetCacheName(id, isPdf: false);
            byte[] bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }
            string exportDocument = exportDocumentService.GetDocumentFileName(fileName: "FichierExempleChargementOD", isPdf: false);
            return exportDocumentService.CreateResponseForDownloadDocument(exportDocument, bytes);
        }

        /// <summary>
        /// Méthode de d'import des OD par fichier excel
        /// </summary>
        /// <param name="dateComptable">Date comptable</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPost]
        [Route("api/OperationDiverse/ImportOperationDiverses/{dateComptable}")]
        public async Task<IHttpActionResult> ImportOperationDiversesAsync(DateTime dateComptable)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            MultipartMemoryStreamProvider provider = await Request.Content.ReadAsMultipartAsync();
            Stream stream = await provider.Contents[0].ReadAsStreamAsync();
            ImportODResult importODResult = operationDiverseManager.ImportOperationDiverses(dateComptable, stream);
            return Ok(new { valid = importODResult.IsValid, errors = importODResult.ErrorMessages });
        }

        /// <summary>
        /// Méthode de génération d'une liste des résultats de l'import de fichier au format excel
        /// </summary>
        /// <param name="erreurs">Liste des erreurs</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPost]
        [Route("api/OperationDiverse/GenerateExcelImportODResult/")]
        public IHttpActionResult GenerateExcelImportODResult(List<string> erreurs)
        {
            ExcelFormat excelFormat = new ExcelFormat();
            string pathName = HttpContext.Current.Server.MapPath("/Templates/TemplateImportODResult.xlsx").ToString();
            List<ErreurImportODExcelModel> erreursExcel = new List<ErreurImportODExcelModel>();
            erreurs.ForEach(s => erreursExcel.Add(new ErreurImportODExcelModel { Message = s.ToString() }));

            byte[] excelBytes = excelFormat.GenerateExcel<ErreurImportODExcelModel>(pathName, erreursExcel);

            CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(30), Priority = CacheItemPriority.NotRemovable };
            string cacheId = Guid.NewGuid().ToString();
            MemoryCache.Default.Add("excelBytes_" + cacheId, excelBytes, policy);

            return Ok(new { id = cacheId });
        }

        /// <summary>
        /// Méthode d'extraction d'une liste des erreurs au format excel
        /// </summary>
        /// <param name="id">Identifiant de l'export excel</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/OperationDiverse/ExtractExcelImportODResult/{id}")]
        public IHttpActionResult ExtractExcelImportODResult(string id)
        {
            string cacheName = exportDocumentService.GetCacheName(id, isPdf: false);
            byte[] bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }
            string exportDocument = exportDocumentService.GetDocumentFileName(fileName: "ExtractImportODResult", isPdf: false);

            HttpResponseMessage result = exportDocumentService.CreateResponseForDownloadDocument(exportDocument, bytes);

            return ResponseMessage(result);
        }

        /// <summary>
        /// Récupère la liste des fréquences d'abonnement
        /// </summary>
        /// <returns>List of all Frequence Abonnement</returns>
        [HttpGet]
        [Route("api/OperationDiverse/GetListFrequenceAbonnement")]
        public IHttpActionResult GetListFrequenceAbonnement()
        {
            return Ok(operationDiverseAbonnementManager.GetFrequenceAbonnement());
        }

        /// <summary>
        /// Récupère de la dernière date de génération d'une réception
        /// </summary>
        /// <param name="firstODAbonnementDate">Date du premiere OD abonnement</param>
        /// <param name="frequenceAbonnement">Fréquence abonnement</param>
        /// <param name="nombreReccurence">Nombre de récurrence de l'abonnement</param>
        /// <returns>Datetime</returns>
        [HttpGet]
        [Route("api/OperationDiverse/GetLastDayOfODAbonnement/{firstODAbonnementDate}/{frequenceAbonnement}/{nombreReccurence}")]
        public IHttpActionResult GetLastDayOfODAbonnement(DateTime firstODAbonnementDate, int frequenceAbonnement, int nombreReccurence)
        {
            return Ok(operationDiverseAbonnementManager.GetLastDayOfODAbonnement(firstODAbonnementDate, frequenceAbonnement, nombreReccurence));
        }

        /// <summary>
        /// Récupère une opération diverse pré-remplie (en vue d'une création)
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="ecritureComptableId">Identifiant de l'écriture comptable</param>
        /// <param name="familleOdId">Identifiant de la famille d'OD</param>
        /// <returns>une opération diverse pré-remplie</returns>
        [HttpGet]
        [Route("api/OperationDiverse/GetPreFillingOD/{ciId}/{ecritureComptableId}/{familleOdId}")]
        public IHttpActionResult GetPreFillingOD(int ciId, int? ecritureComptableId, int familleOdId)
        {
            return Ok(operationDiverseManager.GetPreFillingOD(ciId, ecritureComptableId, familleOdId));
        }
    }
}
