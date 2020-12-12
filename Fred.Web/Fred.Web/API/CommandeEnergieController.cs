using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Fred.Business.CommandeEnergies;
using Fred.Business.Common.ExportDocument;
using Fred.Business.ExternalService;
using Fred.Business.Societe;
using Fred.Entities.Commande;
using Fred.Web.Models.CI;
using Fred.Web.Models.Referential;
using Fred.Web.Shared.Models;

namespace Fred.Web.API
{
    [Authorize]
    public class CommandeEnergieController : ApiControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICommandeEnergieManager commandeEnergieManager;
        private readonly ICommandeEnergieService commandeEnergieService;
        private readonly ICommandeEnergieManagerExterne commandeEnergieManagerExterne;
        private readonly IExportDocumentService exportDocumentService;
        private readonly ISepService sepService;

        public CommandeEnergieController(
            IMapper mapper,
            ICommandeEnergieManager commandeEnergieManager,
            ICommandeEnergieService commandeEnergieService,
            ICommandeEnergieManagerExterne commandeEnergieManagerExterne,
            IExportDocumentService exportDocumentService,
            ISepService sepService)
        {
            this.mapper = mapper;
            this.commandeEnergieManager = commandeEnergieManager;
            this.commandeEnergieService = commandeEnergieService;
            this.commandeEnergieManagerExterne = commandeEnergieManagerExterne;
            this.exportDocumentService = exportDocumentService;
            this.sepService = sepService;
        }

        /// <summary>
        /// Recherche des commandes énergies
        /// </summary>
        /// <param name="filter">Filtre de recherche</param>
        /// <returns>Liste de commandes</returns>
        [HttpGet]
        [Route("api/CommandeEnergie/Search")]
        public HttpResponseMessage Search([FromUri] SearchCommandeEnergieModel filter)
        {
            var includeProperties = new List<Expression<Func<CommandeEnt, object>>>
            {
                x => x.StatutCommande,
                x => x.Fournisseur,
                x => x.TypeEnergie,
                x => x.CI.Societe,
                x => x.Lignes
            };

            return Get(() => mapper.Map<List<CommandeEnergieItemModel>>(commandeEnergieManager.Search(filter, includeProperties, filter.Page, filter.PageSize)));
        }

        /// <summary>
        /// Préchargement d'une commande énergie (préchargement des lignes de commandes)
        /// </summary>
        /// <param name="commande">Commande Energie Model avec info CI, FOURNISSEUR, PERIODE, TYPE ENERGIE</param>
        /// <returns>Commande energie préchargée</returns>
        [HttpPost]
        [Route("api/CommandeEnergie/Preloading")]
        public HttpResponseMessage Preloading([FromBody] CommandeEnergieModel commande)
        {
            return Post(() =>
            {
                commande.CI.Organisation = null;
                CommandeEnergie cmd = commandeEnergieService.CommandeEnergiePreloading(mapper.Map<CommandeEnergie>(commande));
                return mapper.Map<CommandeEnergieModel>(cmd);
            });
        }

        /// <summary>
        /// Récupération d'une commande énergie avec son détail
        /// </summary>
        /// <param name="commandeId">Identifiant commande énergie</param>
        /// <returns>Réponse HTTP</returns>
        [HttpGet]
        [Route("api/CommandeEnergie/{commandeId}")]
        public HttpResponseMessage Get(int commandeId)
        {
            return Get(() => mapper.Map<CommandeEnergieModel>(commandeEnergieService.GetCommandeEnergie(commandeId)));
        }

        /// <summary>
        /// Validation d'une commande énergie
        /// </summary>
        /// <param name="commande">Commande énergie à valider</param>
        /// <returns>Retourne un notification sur l'etat de la commande</returns>
        [HttpPost]
        [Route("api/CommandeEnergie/Validate")]
        public async Task<IHttpActionResult> ValidateAsync(CommandeEnergieModel commande)
        {
            var cmdEnergie = mapper.Map<CommandeEnergie>(commande);

            return Created(string.Empty, await commandeEnergieManagerExterne.ValidateAsync(cmdEnergie));
        }

        /// <summary>
        /// Ajout d'une commande énergie en base
        /// </summary>
        /// <param name="commande">Model Commande énergie</param>
        /// <returns>Réponse HTTP</returns>
        [HttpPost]
        [Route("api/CommandeEnergie/")]
        public HttpResponseMessage Add([FromBody] CommandeEnergieModel commande)
        {
            return Post(() =>
            {
                CommandeEnergie cmd = commandeEnergieManager.Add(mapper.Map<CommandeEnergie>(commande));
                return mapper.Map<CommandeEnergieModel>(commandeEnergieService.GetCommandeEnergie(cmd.CommandeId));
            });
        }

        /// <summary>
        /// Mise à jour d'une commande énergie
        /// </summary>
        /// <param name="commande">Model Commande énergie</param>
        /// <returns>Réponse HTTP</returns>
        [HttpPut]
        [Route("api/CommandeEnergie/")]
        public HttpResponseMessage Update([FromBody] CommandeEnergieModel commande)
        {
            return Put(() =>
            {
                CommandeEnergie cmd = commandeEnergieManager.Update(mapper.Map<CommandeEnergie>(commande));
                return mapper.Map<CommandeEnergieModel>(commandeEnergieService.GetCommandeEnergie(cmd.CommandeId));
            });
        }

        /// <summary>
        /// Suppression d'une commande énergie
        /// </summary>
        /// <param name="commandeId">Identifiant Commande énergie</param>
        /// <returns>Réponse HTTP</returns>
        [HttpDelete]
        [Route("api/CommandeEnergie/{commandeId}")]
        public HttpResponseMessage Delete(int commandeId)
        {
            return Delete(() => commandeEnergieManager.Delete(commandeId));
        }

        /// <summary>
        /// Méthode de génération d'un export Excel d'une commande énergie
        /// Appele la génération de l'excel et le place en cache
        /// </summary>
        /// <param name="commandeEnergieId">Commande dont on veut générer le bon de commande</param>
        /// <returns>Identifiant de l'édition générée</returns>
        [HttpPost]
        [Route("api/CommandeEnergie/GenerateExportCommandeEnergie/{commandeEnergieId}")]
        public object GenerateBonDeCommande(int commandeEnergieId)
        {
            var excelBytes = commandeEnergieManager.ExportExcel(commandeEnergieId);
            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(30), Priority = CacheItemPriority.NotRemovable };
            var cacheId = Guid.NewGuid().ToString();
            MemoryCache.Default.Add("excelBytes_" + cacheId, excelBytes, policy);

            return new { id = cacheId };
        }

        /// <summary>
        /// Retourne le fichier précédemment exporté et stocké en mémoire
        /// </summary>
        /// <param name="id">identifiant de l'export</param>
        /// <param name="fileName">nom du fichier</param>
        /// <returns>fichier sous forme de tableau d'octets</returns>
        [HttpGet]
        [Route("api/CommandeEnergie/GetExport/{id}/{fileName}")]
        public HttpResponseMessage GetExport(string id, string fileName)
        {
            var cacheName = exportDocumentService.GetCacheName(id, false);
            var bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }
            var exportFilename = exportDocumentService.GetDocumentFileName(fileName, false);
            return exportDocumentService.CreateResponseForDownloadDocument(exportFilename, bytes);
        }

        /// <summary>
        /// SearchLight pour Lookup des CI Sep
        /// CI visibles par l’utilisateur ET rattachés à une société de type SEP.
        /// </summary>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="recherche">Texte a rechercher</param>
        /// <returns>Liste de CI</returns>
        [HttpGet]
        [Route("api/CommandeEnergie/Search/Ci/{page?}/{pageSize?}/{recherche?}")]
        public HttpResponseMessage SearchLightCiSep(int page, int pageSize, string recherche)
        {
            return Get(() => mapper.Map<List<CIModel>>(sepService.SearchLightCiSep(page, pageSize, recherche)));
        }

        /// <summary>
        /// SearchLight pour Lookup des fournisseurs SEP
        /// les Fournisseurs liés à une ou plusieurs sociétés SEP du Groupe de l’utilisateur
        /// (dans la table [FRED_ASSOCIE_SEP], rechercher tous les fournisseurs liés à des SEP du Groupe de l'utilisateur).
        /// </summary>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="recherche">Texte a rechercher</param>
        /// <param name="ciId">Ci Id</param>    
        /// <returns>Liste des Fournisseurs</returns>
        [HttpGet]
        [Route("api/CommandeEnergie/Search/Fournisseur/{page?}/{pageSize?}/{recherche?}/{ciId?}")]
        public HttpResponseMessage SearchLightFournisseurSep(int page, int pageSize, string recherche, int? ciId)
        {
            return Get(() => mapper.Map<List<FournisseurModel>>(sepService.SearchLightFournisseurSep(page, pageSize, recherche, ciId)));
        }
    }
}
