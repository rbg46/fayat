using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Fred.Business;
using Fred.Business.Commande;
using Fred.Business.Commande.Models;
using Fred.Business.Commande.Services;
using Fred.Business.Common.ExportDocument;
using Fred.Business.ExternalService.Materiel;
using Fred.Business.Personnel;
using Fred.Business.SeuilValidation.Models;
using Fred.Business.SeuilValidation.Services.Interfaces;
using Fred.Business.Utilisateur;
using Fred.Entities.Avis;
using Fred.Entities.Commande;
using Fred.Entities.Models;
using Fred.Entities.Permission;
using Fred.Entities.Rapport;
using Fred.Entities.Utilisateur;
using Fred.Framework.Reporting;
using Fred.Web.Models;
using Fred.Web.Models.Commande;
using Fred.Web.Modules.Authorization;
using Fred.Web.Modules.Authorization.Api;
using Fred.Web.Shared.Models;
using Fred.Web.Shared.Models.Avis;
using Fred.Web.Shared.Models.Commande;
using Fred.Web.Shared.Models.Commande.Light;
using Fred.Web.Shared.Models.Commande.List;
using Fred.Web.Shared.Models.Personnel.Light;

namespace Fred.Web.API
{
    [Authorize]
    public class CommandeController : ApiControllerBase
    {
        private readonly ICommandeManager commandeManager;
        private readonly IMapper mapper;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IExportDocumentService exportDocumentService;
        private readonly ICommandeManagerExterne commandeManagerExterne;
        private readonly ICommandeImportExportExcelService commandeImportExportExcelService;
        private readonly ICommandeValidationRequestService commandeValidationRequestService;
        private readonly ICommandeHistoriqueService commandeHistoriqueService;
        private readonly ISearchCommandeService searchCommandeService;
        private readonly ICommandeTypeManager commandeTypeManager;
        private readonly IUtilisateursWithPermissionAndSeuilValidationProviderService utilisateursWithPermissionAndSeuilValidationProviderService;
        private readonly IContratAndCommandeInterimaireGeneratorService contratAndCommandeInterimaireGeneratorService;

        public CommandeController(
            ICommandeManager commandeManager,
            IMapper mapper,
            IUtilisateurManager utilisateurManager,
            IExportDocumentService exportDocumentService,
            ICommandeManagerExterne commandeManagerExterne,
            ICommandeImportExportExcelService commandeImportExportExcelService,
            ICommandeValidationRequestService commandeValidationRequestService,
            ICommandeHistoriqueService commandeHistoriqueService,
            ISearchCommandeService searchCommandeService,
            ICommandeTypeManager commandeTypeManager,
            IUtilisateursWithPermissionAndSeuilValidationProviderService utilisateursWithPermissionAndSeuilValidationProviderService,
            IContratAndCommandeInterimaireGeneratorService contratAndCommandeInterimaireGeneratorService)
        {
            this.commandeManager = commandeManager;
            this.mapper = mapper;
            this.utilisateurManager = utilisateurManager;
            this.exportDocumentService = exportDocumentService;
            this.commandeManagerExterne = commandeManagerExterne;
            this.commandeImportExportExcelService = commandeImportExportExcelService;
            this.commandeValidationRequestService = commandeValidationRequestService;
            this.commandeHistoriqueService = commandeHistoriqueService;
            this.searchCommandeService = searchCommandeService;
            this.commandeTypeManager = commandeTypeManager;
            this.utilisateursWithPermissionAndSeuilValidationProviderService = utilisateursWithPermissionAndSeuilValidationProviderService;
            this.contratAndCommandeInterimaireGeneratorService = contratAndCommandeInterimaireGeneratorService;
        }

        /// <summary>
        /// GET api/controller
        /// </summary>
        /// <returns>Liste de commandes</returns>
        [HttpGet]
        [Route("api/Commande/")]
        public HttpResponseMessage Get()
        {
            return this.Get(() => mapper.Map<IEnumerable<CommandeModel>>(this.commandeManager.GetCommandeList()));
        }

        /// <summary>
        /// GET api/controller
        /// </summary>
        /// <param name="filter">Filtre sur comande</param>
        /// <param name="page">Numéro de la page</param>
        /// <param name="pageSize">Nombre d'éléments sur une page</param>
        /// <returns>Liste de ci contenant les commandes groupées</returns>
        [HttpPost]
        [Route("api/Commande/ListByCI/{page?}/{pageSize?}/")]
        public HttpResponseMessage GetListGroupByCI(SearchCommandeModel filter, int? page = 1, int? pageSize = 25)
        {
            return this.Post(() =>
            {
                SearchCommandeListWithFilterResult filteredCommandes = searchCommandeService.SearchCommandeListWithFilter(mapper.Map<SearchCommandeEnt>(filter), page, pageSize);
                var commandesModels = mapper.Map<IEnumerable<CommandeListModel>>(filteredCommandes.Commandes);
                IEnumerable<CommandeGroupByCIModel> groupedCommandesModels = commandesModels.GroupBy(c => c.CI).Select(g => new CommandeGroupByCIModel { CI = g.Key, Commandes = g.ToArray() });
                return new GetListGroupByCIResultModel
                {
                    GroupedCommandes = groupedCommandesModels,
                    TotalCount = filteredCommandes.TotalCount
                };
            });
        }

        /// <summary>
        /// GET api/controller
        /// </summary>
        /// <returns>Objet de recherche des commandes</returns>
        [HttpGet]
        [Route("api/Commande/Filter")]
        public HttpResponseMessage Filter()
        {
            return this.Get(() => mapper.Map<SearchCommandeModel>(this.commandeManager.GetNewFilter()));
        }

        /// <summary>
        /// GET api/controller
        /// </summary>
        /// <param name="text">Texte recherché</param>
        /// <returns>Liste de commandes</returns>
        [HttpGet]
        [Route("api/Commande/Search/{text?}")]
        public HttpResponseMessage Search(string text)
        {
            return this.Get(() => mapper.Map<IEnumerable<CommandeModel>>(searchCommandeService.SearchCommandes(text, null, null)));
        }

        /// <summary>
        /// GET api/controller/Détail/5
        /// </summary>
        /// <param name="id">Identifiant de la commande</param>
        /// <returns>Retourne une commande</returns>
        [HttpGet]
        [Route("api/Commande/Detail/{id?}")]
        public HttpResponseMessage Detail(int? id = null)
        {
            return this.Get(() =>
            {
                CommandeEnt commandeEnt = id.HasValue ? this.commandeManager.GetCommandeById(id.Value) : this.commandeManager.GetNewCommande();
                return mapper.Map<CommandeLightModel>(commandeEnt);
            });
        }

        /// <summary>
        /// DELETE api/controller/5
        /// </summary>
        /// <param name="id">Identifiant de la commande</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpDelete]
        public HttpResponseMessage DeleteCommande(int id)
        {
            return Delete(() => this.commandeManager.DeleteCommandeById(id));
        }

        /// <summary>
        /// PUT api/controller/5
        /// </summary>
        /// <param name="id">Identifiant de la commande</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpGet]
        [Route("api/Commande/Detail/Duplicate/{id}")]
        public HttpResponseMessage Duplicate(int id)
        {
            return this.Get(() => mapper.Map<CommandeLightModel>(this.commandeManager.DuplicateCommande(id)));
        }

        /// <summary>
        /// Permet de valider l'entête de la commande
        /// Actuellement cette méthode n'est disponible qu'avec le featureflipping Actif
        /// </summary>
        /// <param name="commandeModel">Commande à traiter</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Commande/ValidateHeader")]
        public IHttpActionResult ValidateHeader(CommandeModel commandeModel)
        {
            this.commandeManager.ValidateHeaderCommande(mapper.Map<CommandeEnt>(commandeModel));
            return Ok();
        }

        /// <summary>
        ///   Permet de renvoyer une commande et ses avenants vers SAP
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <returns>Le résultat de la demande</returns>
        [HttpGet]
        [Route("api/Commande/ReturnCommandeToSap/{commandeId}")]
        public async Task<IHttpActionResult> ReturnCommandeToSap(int commandeId)
        {
            return Ok(await commandeManagerExterne.ReturnCommandeToSapAsync(commandeId));
        }

        /// <summary>
        /// PUT api/controller/5
        /// </summary>
        /// <param name="codeEtab">Code de l'établissement</param>
        /// <param name="dateDebut">Date de début</param>
        /// <param name="dateFin">Date de fin</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpGet]
        [Route("api/Commande/GetNombreCommandesBuyer/{codeEtab}/{dateDebut}/{dateFin}")]
        public HttpResponseMessage GetNombreCommandesBuyer(string codeEtab, DateTime dateDebut, DateTime dateFin)
        {
            return this.Get(() => this.commandeManager.GetNombreCommandesBuyer(codeEtab, dateDebut, dateFin));
        }

        /// <summary>
        /// PUT api/controller/5
        /// </summary>
        /// <param name="codeEtab">Code de l'établissement dont on veut importer les commandes</param>
        /// <param name="dateDebut">Date de début des commandes à importer</param>
        /// <param name="dateFin">Date de fin des commandes à importer</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Commande/ImporterCmdsBuyer/{codeEtab}/{dateDebut}/{dateFin}")]
        public HttpResponseMessage ImporterCmdsBuyer(string codeEtab, DateTime dateDebut, DateTime dateFin)
        {
            return Post(() => { this.commandeManager.ImporterCmdsBuyer(codeEtab, dateDebut, dateFin); return true; });
        }

        /// <summary>
        /// Méthode de génération d'une liste de commandes au format excel
        /// </summary>
        /// <param name="filter">Filtre sur commande</param>
        /// <returns>Retourne l'identifiant du fichier excel généré</returns>
        [HttpPost]
        [Route("api/Commande/GenerateExcel")]
        public string GenerateExcel(SearchCommandeModel filter)
        {
            if (filter.MesCommandes)
            {
                UtilisateurEnt userContext = utilisateurManager.GetContextUtilisateur();
                filter.AuteurCreationId = userContext?.UtilisateurId;
            }

            var searchCommandeEnt = mapper.Map<SearchCommandeEnt>(filter);

            SearchCommandeListWithFilterResult searchCommandeListWithFilterResult = searchCommandeService.SearchCommandeListWithFilter(searchCommandeEnt, 1, int.MaxValue);

            var commandeModels = mapper.Map<IEnumerable<CommandeModel>>(searchCommandeListWithFilterResult.Commandes);

            using (ExcelFormat excelFormat = new ExcelFormat())
            {
                return commandeManager.CustomizeExcelFileForExport(commandeModels, "/Templates/TemplateCommandes.xlsx");
            }
        }

        /// <summary>
        /// Méthode de génération d'un bon de commande au format PDF
        /// Appelle la génération du pdf et le place en cache
        /// </summary>
        /// <param name="commande">Commande dont on veut générer le bon de commande</param>
        /// <returns>Identifiant de l'édition générée</returns>
        [HttpPost]
        [Route("api/Commande/GeneratePdfBonDeCommande")]
        public object GenerateBonDeCommande(CommandeModel commande)
        {
            byte[] pdfBytes = commandeManager.ExportPdf(mapper.Map<CommandeEnt>(commande));
            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(30), Priority = CacheItemPriority.NotRemovable };
            string cacheId = Guid.NewGuid().ToString();
            MemoryCache.Default.Add("pdfBytes_" + cacheId, pdfBytes, policy);

            return new { id = cacheId };
        }

        /// <summary>
        /// Méthode de génération d'un bon de commande brouillon avec fournisseur provisoire au format PDF
        /// Appelle la génération du pdf et le place en cache
        /// </summary>
        /// <param name="commande">Commande brouillon dont on veut générer le de bon de commande</param>
        /// <returns>Identifiant de l'édition générée</returns>
        [HttpPost]
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.CreationCommandeBrouillonFournisseurTemporaire)]
        [Route("api/Commande/GenerateBonDeCommandeDeCommandeBrouillon")]
        public object GenerateBonDeCommandeDeCommandeBrouillon(CommandeModel commande)
        {
            byte[] pdfBytes = commandeManager.ExportPdfCommandeBrouillon(mapper.Map<CommandeEnt>(commande));
            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(30), Priority = CacheItemPriority.NotRemovable };
            string cacheId = Guid.NewGuid().ToString();
            MemoryCache.Default.Add("pdfBytes_" + cacheId, pdfBytes, policy);

            return new { id = cacheId };
        }

        /// <summary>
        /// Méthode de génération d'un brouillon de bon de commande au format PDF
        /// Appelle la génération du pdf et le place en cache
        /// </summary>
        /// <param name="commande">Commande dont on veut générer le brouillon de bon de commande</param>
        /// <returns>Identifiant de l'édition générée</returns>
        [HttpPost]
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageBoutonApercuBrouillon)]
        [Route("api/Commande/GenerateBrouillonDeBonDeCommande")]
        public object GenerateBrouillonDeBonDeCommande(CommandeModel commande)
        {
            byte[] pdfBytes = commandeManager.ExportBrouillonPdf(mapper.Map<CommandeEnt>(commande));
            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(30), Priority = CacheItemPriority.NotRemovable };
            string cacheId = Guid.NewGuid().ToString();
            MemoryCache.Default.Add("pdfBytes_" + cacheId, pdfBytes, policy);

            return new { id = cacheId };
        }

        /// <summary>
        ///   Méthode d'extraction d'un bon de commande au format PDF
        /// </summary>
        /// <param name="id">Identifiant de l'édition</param>
        /// <param name="numero">Numéro de commande</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/Commande/ExtractPdfBonDeCommande/{id}/{numero}")]
        public HttpResponseMessage ExtractBonDeCommande(string id, string numero)
        {
            string cacheName = exportDocumentService.GetCacheName(id, isPdf: true);
            var bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }
            string exportDocument = exportDocumentService.GetDocumentFileName("BonDeCommande_" + numero, true);
            return exportDocumentService.CreateResponseForDownloadDocument(exportDocument, bytes);
        }

        /// <summary>
        ///   Méthode d'extraction d'un bon de commande au format PDF
        /// </summary>
        /// <param name="id">Identifiant de l'édition</param>
        /// <param name="numero">Numéro de commande</param>
        /// <param name="isPdf">PDF or Excel</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/Commande/ExtractBonDeCommande")]
        public HttpResponseMessage ExtractBonDeCommande(string id, string numero, bool? isPdf)
        {
            string cacheName = exportDocumentService.GetCacheName(id, isPdf ?? false);
            var bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }
            string exportDocument = exportDocumentService.GetDocumentFileName("BonDeCommande_" + numero, isPdf ?? false);
            return exportDocumentService.CreateResponseForDownloadDocument(exportDocument, bytes);
        }

        /// <summary>
        /// Envoie la commande passée en paramètre par mail
        /// en cas d'erreur une FredBusinessException est envoyée
        /// </summary>
        /// <param name="commande">la commande a envoyer par mail</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Commande/SendByMail")]
        public HttpResponseMessage SendByMail(CommandeModel commande)
        {
            Stream pdf = new MemoryStream(commandeManager.ExportPdf(mapper.Map<CommandeEnt>(commande)));
            return Post(() =>
            {
                commandeManager.SendByMail(mapper.Map<CommandeEnt>(commande), pdf);
                return true;
            });
        }

        /// <summary>
        /// Méthode GET de récupération des types de commandes
        /// </summary>
        /// <returns>Retourne la liste des types de commandes</returns>
        [HttpGet]
        [Route("api/Commande/CommandeTypeList")]
        public HttpResponseMessage GetCommandeTypeList()
        {
            return Get(() => this.mapper.Map<IEnumerable<CommandeTypeModel>>(this.commandeTypeManager.GetAll()));
        }

        /// <summary>
        ///   Clôture de la commande
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande à clôturer</param>
        /// <returns>Commande si la clôture est effectuée, sinon faux</returns>
        [HttpGet]
        [Route("api/Commande/Cloturer/{commandeId}")]
        public IHttpActionResult CloturerCommande(int commandeId)
        {
            var result = commandeManager.CloturerCommande(commandeId);
            var cloturerCommandeModel = this.mapper.Map<CloturerCommandeModel>(result);
            return Ok(cloturerCommandeModel);
        }

        /// <summary>
        ///   Déclôture de la commande
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande à déclôturer</param>
        /// <returns>Commande si la déclôture est effectuée, sinon faux</returns>
        [HttpGet]
        [Route("api/Commande/Decloturer/{commandeId}")]
        [FredWebApiAuthorizeAttribute(globalPermissionKey: PermissionKeys.AffichageDeclotureCommandeIndex)]
        public IHttpActionResult DecloturerCommande(int commandeId)
        {
            var result = commandeManager.DecloturerCommande(commandeId);
            var cloturerCommandeModel = this.mapper.Map<CloturerCommandeModel>(result);
            return Ok(cloturerCommandeModel);
        }

        /// <summary>
        ///   Get récupération de la dernière date de génération d'une réception
        /// </summary>
        /// <param name="nextReceptionDate">Date de la prochaine réception</param>
        /// <param name="frequenceAbo">Fréquence abonnement</param>
        /// <param name="dureeAbo">Nombre restant de réception à générer</param>
        /// <returns>Datetime</returns>
        [HttpGet]
        [Route("api/Commande/GetLastDateOfReceptionGeneration/{nextReceptionDate}/{frequenceAbo}/{dureeAbo}")]
        public HttpResponseMessage GetLastDateOfReceptionGeneration(DateTime nextReceptionDate, int frequenceAbo, int dureeAbo)
        {
            return Get(() => commandeManager.GetLastDateOfReceptionGeneration(nextReceptionDate, frequenceAbo, dureeAbo));
        }

        /// <summary>
        /// Génère les commandes intérmaire au pointages des rapport
        /// </summary>
        /// <param name="listRapport">Le modèle d'enregistrement.</param>
        [HttpPost]
        [Route("api/Commande/Interimaire")]
        public async Task<IHttpActionResult> GenerateCommandeInterimaireAsync(List<RapportEnt> listRapport)
        {
            await contratAndCommandeInterimaireGeneratorService.CreateCommandesForPointagesInterimairesAsync(listRapport,
                 callback: (commandeId) => commandeManagerExterne.ExportCommandeToSapAsync(commandeId));
            return Ok();
        }

        ///// <summary>
        ///// Rechercher les auteurs des commandes
        ///// </summary>
        ///// <param name="authorType">Type d'auteur</param>
        ///// <returns>retouner Auteurs</returns>
        [HttpGet]
        [Route("api/Commande/SearchAuthor")]
        public HttpResponseMessage SearchAuthor([FromUri] SearchLightPersonnelModel search)
        {
            return Get(() => mapper.Map<IEnumerable<PersonnelLightForPickListModel>>(searchCommandeService.SearchCommandeAuthors(search)));
        }

        /// <summary>
        /// PUT api/controller/5
        /// </summary>
        /// <param name="commandeModel">Commande à traiter</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPut]
        [Route("api/Commande/")]
        public IHttpActionResult PutCommande(CommandeModel commandeModel)
        {
            commandeManager.UpdateCommande(mapper.Map<CommandeEnt>(commandeModel));
            return Ok();
        }

        [HttpPost]
        [Route("api/Commande/")]
        public IHttpActionResult PostCommande(CommandeModel commandeModel, bool setTacheParDefaut = true)
        {
            var commande = mapper.Map<CommandeEnt>(commandeModel);
            var commandeId = this.commandeManager.AddCommande(commande, setTacheParDefaut);
            return Ok(new
            {
                CommandeId = commandeId
            });
        }

        [HttpPut]
        [Route("api/Commande/Valider")]
        public async Task<IHttpActionResult> PutCommandeValiderAsync(int commandeId, StatutCommandeModel statutCommandeModel)
        {
            commandeManager.ValidateCommande(commandeId, mapper.Map<StatutCommandeEnt>(statutCommandeModel));

            try
            {
                // Envoie la commande validée à SAP.
                ResultModel<string> canExport = await commandeManagerExterne.ExportCommandeToSapAsync(commandeId);
                if (!canExport.Success)
                {
                    logger.Error(canExport.Error);
                }
            }
            catch (Exception ex)
            {
                // On ne tient pas compte de l'exception car on n'informe pas l'utilisateur du succès ou de
                // l'echec de l'export vers SAP. En cas d'echec, il suffira juste de relancer le job dans Hangfire.
                logger.Error(ex);
            }

            return Ok(true);
        }

        #region Avenants

        /// <summary>
        /// PUT Enregistre les lignes d'avenant d'une commande.
        /// </summary>
        /// <param name="model">Le modèle d'enregistrement.</param>
        /// <returns>Le modèle de résultat d'enregistrement.</returns>
        [HttpPut]
        [Route("api/Commande/SaveAvenant")]
        public HttpResponseMessage SaveAvenant(CommandeAvenantSave.Model model)
        {
            return Put(() => commandeManager.SaveAvenant(model));
        }

        /// <summary>
        /// PUT Enregistre les lignes d'avenant d'une commande et valide l'avenant.
        /// </summary>
        /// <param name="model">Le modèle d'enregistrement.</param>
        /// <returns>Le modèle de résultat d'enregistrement.</returns>
        [HttpPut]
        [Route("api/Commande/SaveAvenantAndValidate")]
        public async Task<IHttpActionResult> SaveAvenantAndValidateAsync(CommandeAvenantSave.Model model)
        {
            // Enregistre et valide l'avenant de commande
            CommandeAvenantSave.ResultModel resultModel = commandeManager.ValideAvenant(model);

            // Envoie la commande validée à SAP.
            try
            {
                ResultModel<string> canExport = await commandeManagerExterne.ExportCommandeAvenantToSapAsync(model.CommandeId, resultModel.Avenant.NumeroAvenant);
                if (!canExport.Success)
                {
                    logger.Error(canExport.Error);
                }
            }
            catch (Exception ex)
            {
                // On ne tient pas compte de l'exception car on n'informe pas l'utilisateur du succès ou de
                // l'echec de l'export vers SAP. En cas d'echec, il suffira juste de relancer le job dans Hangfire.
                logger.Error(ex);
            }

            return Ok(resultModel);
        }

        #endregion Avenants

        [HttpGet]
        [Route("api/Commande/PersonnelsForValidation/{seuilMinimum}/{deviseId}/{ciId?}/{page?}/{pageSize?}/{recherche?}/{authorizedOnly?}")]
        public HttpResponseMessage PersonnelsForValidation(decimal seuilMinimum, int deviseId, int ciId, int page = 1, int pageSize = 20, string recherche = "", bool authorizedOnly = true)
        {
            return this.Get(() =>
            {
                var request = new PersonnelWithPermissionAndSeuilValidationRequest()
                {
                    CiId = ciId,
                    SeuilMinimum = seuilMinimum,
                    Page = page,
                    PageSize = pageSize,
                    Recherche = recherche,
                    DeviseId = deviseId,
                    AuthorizedOnly = authorizedOnly
                };

                List<PersonnelWithPermissionAndSeuilValidationResult> response = utilisateursWithPermissionAndSeuilValidationProviderService.GetUtilisateursWithPermissionToShowCommandeAndWithMinimunSeuilValidation(request);

                return mapper.Map<List<PersonnelWithPermissionAndSeuilModel>>(response);
            });
        }

        #region Avis

        /// <summary>
        /// Sauvegarder un avis
        /// </summary>
        /// <param name="model">Modèle représentant un avis</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Commande/RequestValidation")]
        public HttpResponseMessage RequestValidation(AvisModel model)
        {
            return Post(() =>
            {
                AvisEnt avis = mapper.Map<AvisEnt>(model);

                // Base url
                string baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);

                // Demander une validation
                commandeValidationRequestService.RequestValidation(model.CommandeId, model.CommandeAvenantId, avis, baseUrl);
            });
        }

        #endregion Avis

        #region Historique

        /// <summary>
        /// Récupérer l'historique d'une commande
        /// </summary>
        /// <param name="commandeId">Identifiant d'une commande</param>
        /// <returns>Liste des évènements d'une commande</returns>
        [HttpGet]
        [Route("api/Commande/GetHistorique/{commandeId}")]
        public HttpResponseMessage GetHistorique(int commandeId)
        {
            return Get(() => commandeHistoriqueService.GetHistorique(commandeId));
        }

        #endregion Historique

        #region ImportLigneExcel

        /// <summary>
        /// Méthode de génération d'un fichier exemple lignes de commande  au format excel
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="isAvenant">Is Avenant</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Commande/GenerateExempleExcel/{ciId}/{isAvenant?}")]
        public HttpResponseMessage GenerateExempleExcel(int ciId, bool isAvenant = false)
        {
            return Post(() => commandeImportExportExcelService.GenerateExempleExcel(ciId, isAvenant));
        }

        /// <summary>
        /// Méthode de d'import des OD par fichier excel
        /// </summary>
        /// <param name="checkinValue">Date Commande</param>
        /// <param name="ciId">Id CI</param>
        /// <param name="isAvenant">Is Avenant</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPost]
        [Route("api/commande/ImportCommandeLignes")]
        public async Task<HttpResponseMessage> ImportCommandeLignes(string checkinValue, int ciId, bool isAvenant = false)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            // Lire le multipart
            MultipartMemoryStreamProvider provider = await Request.Content.ReadAsMultipartAsync();

            // Lire le stream
            Stream stream = await provider.Contents[0].ReadAsStreamAsync();

            return Post(() =>
            {
                ImportResultImportLignesCommande importResult = commandeImportExportExcelService.ImportCommandeLignes(checkinValue, ciId, stream, isAvenant);

                return importResult;
            });
        }

        #endregion ImportLigneExcel
    }
}
