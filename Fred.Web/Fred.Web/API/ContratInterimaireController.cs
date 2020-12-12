using AutoMapper;
using Fred.Business.Commande.Services;
using Fred.Business.ExternalService.Materiel;
using Fred.Business.Personnel.Interimaire;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Valorisation;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Rapport;
using Fred.Web.Models.CI;
using Fred.Web.Models.Organisation;
using Fred.Web.Models.Personnel.Interimaire;
using Fred.Web.Shared.Models.Personnel.Interimaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fred.Web.API
{
    public class ContratInterimaireController : ApiControllerBase
    {

        private readonly IContratInterimaireManager contratInterimaireManager;
        private readonly IPointageManager pointageManager;
        private readonly IContratAndCommandeInterimaireGeneratorService contratAndCommandeInterimaireGeneratorService;
        private readonly IZoneDeTravailManager zoneDeTravailManager;
        private readonly IMapper mapper;
        private readonly ICommandeManagerExterne commandeExternalMrg;
        private readonly IValorisationManager valorisationManager;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="ContratInterimaireController" />.
        /// </summary>  
        /// <param name="contratInterimaireManager">Manager des contrats d'interimaire</param>
        /// <param name="pointageManager">Manager des pointages</param>
        /// <param name="commandeManger">Manager des commandes</param>
        /// <param name="zoneDeTravailManager">Manager des zones de travail</param>
        /// <param name="commandeExternalMrg">commandeExternalMrg</param>
        /// <param name="mapper">Mapper Model / ModelVue</param>
        public ContratInterimaireController(IMapper mapper,
            IContratInterimaireManager contratInterimaireManager,
            IPointageManager pointageManager,
            IValorisationManager valorisationManager,
            IContratAndCommandeInterimaireGeneratorService contratAndCommandeInterimaireGeneratorService,
            ICommandeManagerExterne commandeExternalMrg,
            IZoneDeTravailManager zoneDeTravailManager)
        {
            this.contratInterimaireManager = contratInterimaireManager;
            this.pointageManager = pointageManager;
            this.contratAndCommandeInterimaireGeneratorService = contratAndCommandeInterimaireGeneratorService;
            this.zoneDeTravailManager = zoneDeTravailManager;
            this.mapper = mapper;
            this.commandeExternalMrg = commandeExternalMrg;
            this.valorisationManager = valorisationManager;
        }

        /// <summary>
        /// Méthode GET de récupération d'un contrat d'intérimaire
        /// </summary>
        /// <param name="contratInterimaireId">Identifiant du contrat d'interimaire</param>
        /// <returns>Retourne le contrat d'interimaire</returns>
        [HttpGet]
        [Route("api/ContratInterimaire/{contratInterimaireId}")]
        public HttpResponseMessage GetContratInterimaireById(int contratInterimaireId)
        {
            return Get(() => this.mapper.Map<IEnumerable<ContratInterimaireModel>>(this.contratInterimaireManager.GetContratInterimaireById(contratInterimaireId)));
        }

        /// <summary>
        /// Méthode GET de récupération d'un contrat d'intérimaire
        /// </summary>
        /// <param name="numeroContratInterimaireModel">model numéro du contrat d'interimaire</param>
        /// <returns>Retourne le contrat d'interimaire</returns>
        [HttpPost]
        [Route("api/ContratInterimaire/Numero")]
        public HttpResponseMessage GetContratInterimaireByNumeroContrat(NumeroContratInterimaireModel numeroContratInterimaireModel)
        {
            return Post(() => this.mapper.Map<ContratInterimaireModel>(this.contratInterimaireManager.GetContratInterimaireByNumeroContrat(numeroContratInterimaireModel.NumContrat, numeroContratInterimaireModel.ContratInterimaireId)));
        }

        /// <summary>
        /// Méthode GET de récupération d'un contrat d'intérimaire actif
        /// </summary>
        /// <param name="contratInterimaireId">Identifiant Unique du Contrat Intérimaire</param>
        /// <param name="interimaireId">Identifiant Unique de l'Intérimaire</param>
        /// <param name="dateDebut">Date de début du Contrat Intérimaire</param>
        /// <param name="dateFin">Date de fin du Contrat Intérimaire</param>
        /// <returns>Retourne le contrat d'interimaire</returns>
        [HttpGet]
        [Route("api/ContratInterimaire/Actif/{contratInterimaireId}/{interimaireId}/{dateDebut}/{dateFin}")]
        public HttpResponseMessage GetContratInterimaireAlreadyActif(int contratInterimaireId, int interimaireId, DateTime dateDebut, DateTime dateFin)
        {
            return Get(() => this.contratInterimaireManager.GetContratInterimaireAlreadyActif(contratInterimaireId, interimaireId, dateDebut, dateFin));
        }

        /// <summary>
        /// Méthode GET de récupération des contrats d'intérimaire lié au personnel id
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel intérimaire</param>
        /// <returns>Retourne la liste des contrats d'intérimaire lié au personnel id</returns>
        [HttpGet]
        [Route("api/ContratInterimaire/Personnel/{personnelId}")]
        public HttpResponseMessage GetContratInterimaireByPersonnelId(int personnelId)
        {
            return Get(() => this.mapper.Map<IEnumerable<ContratInterimaireModel>>(this.contratInterimaireManager.GetContratInterimaireByPersonnelId(personnelId)));
        }

        /// <summary>
        /// Méthode GET de récupération des contrats d'intérimaire lié au personnel id
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel intérimaire</param>
        /// <param name="date">Date </param>
        /// <returns>Retourne la liste des contrats d'intérimaire lié au personnel id</returns>
        [HttpGet]
        [Route("api/ContratInterimaire/GetCIList/personnelId/{personnelId}/date/{date}")]
        public HttpResponseMessage GetCIList(int personnelId, DateTime date)
        {
            return Get(() => this.mapper.Map<IEnumerable<CIModel>>(this.contratInterimaireManager.GetCIList(personnelId, date)));
        }

        /// <summary>
        /// Méthode GET de récupération des motifs de remplacement
        /// </summary>
        /// <returns>Liste des motifs de remplacement</returns>
        [HttpGet]
        [Route("api/ContratInterimaire/MotifRemplacement")]
        public HttpResponseMessage GetMotifRemplacement()
        {
            return Get(() => this.contratInterimaireManager.GetMotifRemplacement());
        }

        /// <summary>
        /// Ajout d'un contrat d'intérimaire
        /// </summary>
        /// <param name="contratInterimaireModel">Objet Model envoyé</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/ContratInterimaire/Add")]
        public async Task<IHttpActionResult> AddContratInterimaireAsync(ContratInterimaireModel contratInterimaireModel)
        {
            var contratInterimaire = this.mapper.Map<ContratInterimaireEnt>(contratInterimaireModel);

            var contratInterimaireCreated = this.contratInterimaireManager.AddContratInterimaire(contratInterimaire);
            List<RapportLigneEnt> pointagesInterimairesNonReceptionnees = this.pointageManager.GetPointagesInterimaireNonReceptionnees(contratInterimaire.InterimaireId, contratInterimaire.DateDebut, contratInterimaire.DateFin);

            if (pointagesInterimairesNonReceptionnees.Any())
            {
                List<int> rapportLignesIds = pointagesInterimairesNonReceptionnees.Select(x => x.RapportLigneId).ToList();
                valorisationManager.UpdateValorisationMontant(rapportLignesIds, contratInterimaireModel.Valorisation);
                pointageManager.UpdateContratId(rapportLignesIds, contratInterimaireCreated.ContratInterimaireId);
            }

            await contratAndCommandeInterimaireGeneratorService.CreateCommandesForPointagesInterimairesAsync(
                   contratInterimaireCreated, callback: (commandeId) => commandeExternalMrg.ExportCommandeToSapAsync(commandeId));

            return Created($"api/ContratInterimaire/{contratInterimaireCreated.ContratInterimaireId}", contratInterimaireCreated.ContratInterimaireId);
        }

        /// <summary>
        /// Moification d'un contrat d'intérimaire
        /// </summary>
        /// <param name="contratInterimaireModel">Objet Model envoyé</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPut]
        [Route("api/ContratInterimaire/Update")]
        public HttpResponseMessage UpdateContratInterimaire(ContratInterimaireModel contratInterimaireModel)
        {
            return Put(() => this.contratInterimaireManager.UpdateContratInterimaire(this.mapper.Map<ContratInterimaireEnt>(contratInterimaireModel)));
        }

        /// <summary>
        /// Suppression d'une contrat d'intérimaire en fonction de son identifiant
        /// </summary>
        /// <param name="contratInterimaireId">Identifiant unique du contrat d'intérimaire à supprimer</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpDelete]
        [Route("api/ContratInterimaire/Delete/{contratInterimaireId}")]
        public HttpResponseMessage DeleteContratInterimaireById(int contratInterimaireId)
        {
            return Delete(() => this.contratInterimaireManager.DeleteContratInterimaireById(contratInterimaireId));
        }

        /// <summary>
        /// Méthode GET de récupération d'un contrat d'intérimaire
        /// </summary>
        /// <param name="personnelId">Identifiant unique de l'intériamire</param>
        /// <param name="date">Date</param>
        /// <returns>Retourne le contrat d'interimaire</returns>
        [HttpGet]
        [Route("api/ContratInterimaire/GetDatesMax/{personnelId}/{date}")]
        public HttpResponseMessage GetContratInterimaireById(int personnelId, DateTime date)
        {
            return Get(() => this.contratInterimaireManager.GetDatesMax(personnelId, date));
        }


        /// <summary>
        /// Retourne une liste de libelle de ci sur lesquels l'intérimaire a été pointé lors de sa période de contrat souplesse incluse
        /// </summary>
        /// <param name="contratInterimaireModel">contrat intérimaire</param>
        /// <returns>liste des libelle des ci</returns>
        [HttpPost]
        [Route("api/ContratInterimaire/LibelleCi")]
        public HttpResponseMessage GetCiInRapportLigneByDateContratInterimaire(ContratInterimaireModel contratInterimaireModel)
        {
            return Post(() => this.contratInterimaireManager.GetCiInRapportLigneByDateContratInterimaire(this.mapper.Map<ContratInterimaireEnt>(contratInterimaireModel)));
        }

        /// <summary>
        ///   Retourne le nombre de pointage sur un contrat interimaire
        /// </summary>
        /// <param name="contratInterimaireModel">contrat intérimaire</param>
        /// <returns>Un nombre de pointage</returns>
        [HttpPost]
        [Route("api/ContratInterimaire/Pointage")]
        public HttpResponseMessage GetPointageForContratInterimaire(ContratInterimaireModel contratInterimaireModel)
        {
            return Post(() => this.pointageManager.GetPointageForContratInterimaire(this.mapper.Map<ContratInterimaireEnt>(contratInterimaireModel)));
        }

        /// <summary>
        /// Recherche les organisations à partir d'un identifiant unique contrat intérimaire
        /// </summary>
        /// <param name="contratInterimaireId">Identifiant unique d'un contrat intérimaire</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="recherche">recherche</param>
        /// <returns>Une liste  d'organisation</returns>
        [HttpGet]
        [Route("api/ContratInterimaire/SearchLight/{contratInterimaireId}/{page?}/{pageSize?}/{recherche?}")]
        public HttpResponseMessage SearchLightForContratInterimaireId(int contratInterimaireId, int page = 1, int pageSize = 20, string recherche = "")
        {
            return this.Get(() =>
            {
                return this.mapper.Map<IEnumerable<OrganisationModel>>(this.zoneDeTravailManager.SearchLightForContratInterimaireId(contratInterimaireId, page, pageSize, recherche));
            });
        }

        /// <summary>
        /// Méthode GET de récupération d'un contrat d'intérimaire
        /// </summary>
        /// <param name="personnelId">Identifiant unique de l'intériamire</param>
        /// <param name="date">Date</param>
        /// <returns>Retourne le contrat d'interimaire</returns>
        [HttpGet]
        [Route("api/ContratInterimaire/GetListOfDaysAvailable/{personnelId}/{date}")]
        public HttpResponseMessage GetListOfDaysAvailable(int personnelId, DateTime date)
        {
            return Get(() => this.contratInterimaireManager.GetListDaysAvailableInPeriod(personnelId, date));
        }
    }
}
