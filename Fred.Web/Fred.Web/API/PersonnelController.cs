using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Fred.Business.Affectation;
using Fred.Business.Common.ExportDocument;
using Fred.Business.Email.Subscription;
using Fred.Business.ExternalService.Personnel;
using Fred.Business.Personnel;
using Fred.Business.Personnel.Excel;
using Fred.Business.Referential;
using Fred.Entities;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Interimaire;
using Fred.Web.Models.AffectationInterimaire;
using Fred.Web.Models.CI;
using Fred.Web.Models.Groupe;
using Fred.Web.Models.Personnel;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Societe;
using Fred.Web.Models.Utilisateur;
using Fred.Web.Shared.Models;

namespace Fred.Web.API
{
    public class PersonnelController : ApiControllerBase
    {
        private readonly string templateFolderPath = HttpContext.Current.Server.MapPath("/Templates/");

        private readonly IMapper mapper;
        private readonly IPersonnelManager personnelManager;
        private readonly ITypeRattachementManager typeRattachementManager;
        private readonly IPersonnelImageManager personnelImageManager;
        private readonly IAffectationManager affectationManager;
        private readonly IExportDocumentService exportDocumentService;
        private readonly IPersonnelExcelManager personnelExcelManager;
        private readonly IEmailSubscriptionManager emailSubscriptionManager;
        private readonly IPersonnelManagerExterne personnelManagerExterne;

        public PersonnelController(
            IMapper mapper,
            IPersonnelManager personnelManager,
            ITypeRattachementManager typeRattachementManager,
            IPersonnelImageManager personnelImageManager,
            IAffectationManager affectationManager,
            IExportDocumentService exportDocumentService,
            IPersonnelExcelManager personnelExcelManager,
            IEmailSubscriptionManager emailSubscriptionManager,
            IPersonnelManagerExterne personnelManagerExterne)
        {
            this.mapper = mapper;
            this.personnelManager = personnelManager;
            this.typeRattachementManager = typeRattachementManager;
            this.personnelImageManager = personnelImageManager;
            this.affectationManager = affectationManager;
            this.exportDocumentService = exportDocumentService;
            this.personnelExcelManager = personnelExcelManager;
            this.emailSubscriptionManager = emailSubscriptionManager;
            this.personnelManagerExterne = personnelManagerExterne;
        }

        /// <summary>
        /// Méthode GET de récupération du personnel
        /// </summary>
        /// <returns>Retourne la liste du personnel</returns>
        [HttpGet]
        [Route("api/Personnel")]
        public HttpResponseMessage Get()
        {
            return Get(() => mapper.Map<IEnumerable<PersonnelModel>>(personnelManager.GetPersonnelList()));
        }

        [HttpPost]
        [Route("api/Personnel/Excel")]
        public object GetExportExcel(SearchPersonnelModel filters, bool haveHabilitation)
        {
            try
            {
                var searchPersonnel = mapper.Map<SearchPersonnelEnt>(filters);
                byte[] excelBytes = personnelExcelManager.GetExportExcel(searchPersonnel, haveHabilitation);
                string typeCache = "excelBytes_";
                string cacheId = Guid.NewGuid().ToString();

                var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(300), Priority = CacheItemPriority.NotRemovable };
                MemoryCache.Default.Add(typeCache + cacheId, excelBytes, policy);

                return new
                {
                    id = cacheId
                };
            }
            catch (Exception ex)
            {
                logger.Log(NLog.LogLevel.Error, ex);
                return null;
            }
        }

        [HttpGet]
        [Route("api/Personnel/Excel/{idExport}")]
        public HttpResponseMessage GetExportExcelAvancementBudget(string idExport)
        {
            var cacheName = exportDocumentService.GetCacheName(idExport, isPdf: false);
            var bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }

            var exportFilename = exportDocumentService.GetDocumentFileName("Personnels", false);
            return exportDocumentService.CreateResponseForDownloadDocument(exportFilename, bytes);
        }

        [HttpGet]
        [Route("api/Personnel/CreateExcelHabilitationsUtilisateurs/{utilisateurId}")]
        public HttpResponseMessage CreateExcelHabilitationsUtilisateurs(int utilisateurId)
        {
            return Get(() => new
            {
                id = personnelExcelManager.AddGeneratedExcelStreamToCache(utilisateurId, templateFolderPath)
            });
        }

        [HttpGet]
        [Route("api/Personnel/DownloadExcelHabilitationUtilisateurs/{idExport}/{idUtilisateur}")]
        public HttpResponseMessage DownloadExcelHabilitationsUtilisateurs(string idExport, int idUtilisateur)
        {
            PersonnelEnt selectedUser = personnelManager.GetPersonnelById(idUtilisateur);
            string fileName = string.Format("{0}-{1}-Habilitations", (selectedUser.Societe)?.Code, selectedUser.CodeNomPrenom);
            var exportFilename = exportDocumentService.GetDocumentFileName(fileName, false);

            var cacheName = exportDocumentService.GetCacheName(idExport, isPdf: false);
            var bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }

            return exportDocumentService.CreateResponseForDownloadDocument(exportFilename, bytes);
        }

        /// <summary>
        /// Méthode GET de récupération du détail d'un personnel Interne
        /// </summary>
        /// <returns>Retourne Détail d'un personnel Externe</returns>
        /// <example>http://localhost:6870/api/Personnel/Detail/7</example>
        [HttpGet]
        [Route("api/Personnel/New/")]
        public HttpResponseMessage New()
        {
            return Get(() => mapper.Map<PersonnelModel>(personnelManager.GetNewPersonnel()));
        }

        /// <summary>
        /// Méthode GET de récupération d'une nouvelle instance d'utilisateur
        /// </summary>
        /// <returns>Retourne une nouvelle instance d'utilisateur</returns>    
        [HttpGet]
        [Route("api/Personnel/NewUtilisateur/")]
        public HttpResponseMessage NewUtilisateur()
        {
            return Get(() => mapper.Map<UtilisateurSansPersonnelModel>(personnelManager.GetNewUtilisateur()));
        }

        /// <summary>
        /// Méthode GET de récupération du détail d'un personnel Interne
        /// </summary>
        /// <returns>Retourne Détail d'un personnel Externe</returns>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <example>http://localhost:6870/api/Personnel/Detail/7</example>
        [HttpGet]
        [Route("api/Personnel/{personnelId}")]
        public HttpResponseMessage GetPersonnelById([FromUri] int personnelId)
        {
            return Get(() => mapper.Map<PersonnelModel>(personnelManager.GetPersonnel(personnelId)));
        }

        /// <summary>
        /// Méthode GET de récupération du du CI par defaut d'un personnel
        /// </summary>
        /// <returns>Retourne le CI par defaut</returns>
        /// <param name="personnelId">Identifiant du personnel</param>
        [HttpGet]
        [Route("api/Personnel/GetDefaultCi/{personnelId}")]
        public HttpResponseMessage GetDefaultCi(int personnelId)
        {
            return Get(() => mapper.Map<CIModel>(affectationManager.GetDefaultCi(personnelId)));
        }

        /// <summary>
        /// Méthode GET de récupération d'un model DateEntreeSortie
        /// </summary>
        /// <returns>Retourne un model DateEntreeSortie</returns>
        /// <param name="personnelId">Identifiant du personnel</param>
        [HttpGet]
        [Route("api/Personnel/GetDateEntreeSortie/{personnelId}")]
        public HttpResponseMessage GetDateEntreeSortie(int personnelId)
        {
            return Get(() => personnelManager.GetDateEntreeSortie(personnelId));
        }

        /// <summary>
        /// Méthode GET de récupération du détail d'un personnel en fonction de son nom et prénom
        /// </summary>
        /// <returns>Retourne Détail d'un personnel</returns>
        /// <param name="nom">Nom du personnel</param>
        /// <param name="prenom">Prénom du personnel</param>    
        /// <param name="groupeId">Identifiant du groupe</param>
        [HttpGet]
        [Route("api/Personnel/GetByNomPrenom/{nom}/{prenom}/{groupeId}")]
        public HttpResponseMessage GetPersonnelByNomPrenom([FromUri] string nom, [FromUri] string prenom, [FromUri] int groupeId)
        {
            return Get(() => mapper.Map<PersonnelModel>(personnelManager.GetPersonnelByNomPrenom(nom, prenom, groupeId)));
        }

        /// <summary>
        /// Méthode GET de récupération du détail d'un personnel en fonction de sa société et son matricule
        /// </summary>
        /// <param name="societeId">Identifiant de la société du personnel</param>
        /// <param name="matricule">Matricule personnel</param>
        /// <returns>Retourne le personnel</returns>
        [HttpGet]
        [Route("api/Personnel/GetBySocieteMatricule/{societeId}/{matricule}")]
        public HttpResponseMessage GetPersonnelBySocieteMatricule([FromUri] int societeId, [FromUri] string matricule)
        {
            return Get(() => mapper.Map<PersonnelModel>(personnelManager.GetPersonnel(societeId, matricule)));
        }

        /// <summary>
        /// Méthode GET de récupération du personnel
        /// </summary>
        /// <returns>Retourne la liste du personnel</returns>
        [HttpGet]
        [Route("api/Personnel/Filter/")]
        public HttpResponseMessage Filters()
        {
            return Get(() => mapper.Map<SearchPersonnelModel>(personnelManager.GetNewFilter()));
        }

        [HttpPost]
        [Route("api/Personnel/SearchPersonnelWithFilters/{page?}/{pageSize?}/{recherche?}")]
        public HttpResponseMessage SearchPersonnelWithFilters(SearchPersonnelModel filters, int page = 1, int pageSize = 20)
        {
            return Get(() =>
            {
                var searchPersonnel = mapper.Map<SearchPersonnelEnt>(filters);
                var searchPersonnelsWithFiltersResult = personnelManager.SearchPersonnelsWithFilters(searchPersonnel, page, pageSize);
                return mapper.Map<SearchPersonnelsWithFiltersResultModel>(searchPersonnelsWithFiltersResult);
            });
        }

        /// <summary>
        /// Récupération optimisé des personnels
        /// </summary>
        /// <param name="filters">Filtre de récupération</param>
        /// <param name="page">Index de la page courante</param>
        /// <param name="pageSize">Nombre délement dans la page</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Personnel/SearchPersonnelWithFiltersOptimzed/{page?}/{pageSize?}/{recherche?}")]
        public HttpResponseMessage SearchPersonnelWithFiltersOptimzed(SearchPersonnelModel filters, int page = 1, int pageSize = 20)
        {
            return Get(() =>
            {
                var searchPersonnel = mapper.Map<SearchPersonnelEnt>(filters);
                var searchPersonnelsWithFiltersResult = personnelManager.SearchPersonnelsWithFiltersOptimized(searchPersonnel, page, pageSize);
                return mapper.Map<SearchPersonnelsWithFiltersResultModel>(searchPersonnelsWithFiltersResult);
            });
        }

        /// <summary>
        /// GET api/controller
        /// </summary>
        /// <param name="filter">Filtre Personnel</param>
        /// <returns>Liste de ci contenant les commandes groupées</returns>
        [HttpPost]
        [Route("api/Personnel/CountPersonnel/")]
        public HttpResponseMessage GetCountPersonnel(SearchPersonnelModel filter)
        {
            return Post(() =>
            {
                return personnelManager.GetCountPersonnel(mapper.Map<SearchPersonnelEnt>(filter));
            });
        }

        /// <summary>
        /// POST api/controller
        /// </summary>
        /// <param name="model">Objet Model envoyé</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Personnel")]
        public HttpResponseMessage AddPersonnel(PersonnelModel model)
        {
            return Post(() => mapper.Map<PersonnelModel>(personnelManager.Add(mapper.Map<PersonnelEnt>(model))));
        }

        /// <summary>
        /// PUT api/controller
        /// </summary>
        /// <param name="model">Objet Model envoyé</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPut]
        [Route("api/Personnel")]
        public async Task<IHttpActionResult> UpdatePersonnel(PersonnelModel model)
        {
            PersonnelEnt updatedPersonnel = await personnelManagerExterne.OnUpdatePersonnelAsync(model.PersonnelId, () => personnelManager.Update(mapper.Map<PersonnelEnt>(model)));

            return Ok(updatedPersonnel);
        }

        /// <summary>
        /// Suppression d'un personnel en fonction de son identifiant
        /// </summary>
        /// <param name="personnelId">Identifiant unique du personnel à supprimer</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpDelete]
        [Route("api/Personnel/{personnelId}")]
        public HttpResponseMessage DeletePersonnelById(int personnelId)
        {
            return Delete(() => personnelManager.DeleteById(personnelId));
        }

        [HttpPost]
        [Route("api/Personnel/AddPersonnelAsUtilisateur/")]
        public HttpResponseMessage AddPersonnelAsUtilisateur(PersonnelModel model)
        {
            return Post(() =>
            {
                return mapper.Map<PersonnelModel>(personnelManager.AddPersonnelAsUtilisateur(mapper.Map<PersonnelEnt>(model)));
            });
        }

        /// <summary>
        /// Rechercher les personnels
        /// {page?}/{pageSize?}/{recherche?}/{societeId?}/{ciId?}/{statut?}/{onlyUtilisateur?}
        /// </summary>
        /// <param name="search">Object de recherche</param>  
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/Personnel/SearchLight")]
        public async Task<IHttpActionResult> SearchLightAsync([FromUri] SearchLightPersonnelModel search)
        {
            IEnumerable<PersonnelEnt> searchResult = await personnelManager.SearchLightAsync(search).ConfigureAwait(false);
            return Ok(mapper.Map<IEnumerable<PersonnelModel>>(searchResult));
        }

        /// <summary>
        /// Rechercher les personnels
        /// {page?}/{pageSize?}/{recherche?}/{societeId?}/{ciId?}/{statut?}/{onlyUtilisateur?}
        /// </summary>
        /// <param name="search">Object de recherche</param>  
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/Personnel/SearchLightForTeam")]
        public HttpResponseMessage SearchLightForTeam([FromUri] SearchLightPersonnelModel search)
        {
            return Get(() => mapper.Map<IEnumerable<PersonnelModel>>(personnelManager.SearchLightForTeam(search)));
        }

        /// <summary>
        /// Rechercher les personnels pour les affectations des moyens en fonction des rôles de l'utilisateur
        /// {page?}/{pageSize?}/{recherche?}
        /// </summary>
        /// <param name="search">Object de recherche</param>  
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/PersonnelForAffectationMoyen/SearchLight")]
        public async Task<IHttpActionResult> PersonnelsSearchLightForAffectationMoyenAsync([FromUri] SearchLightPersonnelModel search)
        {
            search.ForAffectationMoyen = true;
            IEnumerable<PersonnelEnt> searchResult = await personnelManager.SearchLightAsync(search).ConfigureAwait(false);
            return Ok(mapper.Map<IEnumerable<PersonnelModel>>(searchResult));
        }

        /// <summary>
        /// Rechercher les personnels avec filtres nom, prenom et/ou autres infos
        /// </summary>
        /// <param name="search">Object de recherche</param>        
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/Personnel/SearchWithHabilitation")]
        public async Task<IHttpActionResult> SearchWithHabilitation([FromUri] SearchLightPersonnelModel search)
        {
            var personnelsResult = await personnelManager.SearchPersonnelEnteteCommandeAsync(search);
            return Ok(mapper.Map<IEnumerable<PersonnelModel>>(personnelsResult));
        }

        /// <summary>
        /// Rechercher les personnels avec filtres nom, prenom et/ou autres infos
        /// </summary>
        /// <param name="search">Object de recherche</param>        
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/Personnel/SearchLight2")]
        public async Task<IHttpActionResult> SearchLight2Async([FromUri] SearchLightPersonnelModel search)
        {
            IEnumerable<PersonnelEnt> searchResult = await personnelManager.SearchLightAsync(search).ConfigureAwait(false);
            return Ok(mapper.Map<IEnumerable<PersonnelModel>>(searchResult));
        }

        /// <summary>
        ///   Récupère la liste des personnels du niveau hierarchique inférieur
        /// </summary>
        /// <param name="search">Object de recherche</param> 
        /// <returns>Http Response</returns>
        [HttpGet]
        [Route("api/Personnel/SearchHierarchieNmoins1")]
        public HttpResponseMessage SearchHierarchieNmoins1([FromUri] SearchLightPersonnelModel search)
        {
            return Get(() => mapper.Map<IEnumerable<PersonnelModel>>(personnelManager.SearchNmoins1(search)));
        }

        /// <summary>
        /// Récupère le prochain Matricule d'un intérimaire
        /// </summary>
        /// <returns>Nouveau matricule intérimaire</returns>
        [HttpGet]
        [Route("api/Personnel/GetNextMatriculeInterimaire")]
        public HttpResponseMessage GetNextMatriculeInterimaire()
        {
            return Get(() => personnelManager.GetNextMatriculeInterimaire());
        }

        /// <summary>
        /// Méthode GET de récupération des types de rattachement
        /// </summary>
        /// <returns>Retourne la liste des types de rattachement</returns>    
        [HttpGet]
        [Route("api/Personnel/GetTypesRattachement/")]
        public HttpResponseMessage GetTypesRattachement()
        {
            return Get(() => mapper.Map<IEnumerable<TypeRattachementModel>>(typeRattachementManager.GetList()));
        }

        /// <summary>
        /// Méthode GET de récupération le matériel par défaut du personnel
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <returns>Retourne le matériel par défaut du personnel</returns>    
        [HttpGet]
        [Route("api/Personnel/GetMaterielDefault/{personnelId}")]
        public HttpResponseMessage GetMaterielDefault(int personnelId)
        {
            return Get(() => mapper.Map<MaterielModel>(personnelManager.GetMaterielDefault(personnelId)));
        }

        /// <summary>
        ///   Méthode Get de récupération des images d'un personnel
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <returns>Entité model PersonnelImage avec photo de profil et signature au format Base64</returns>
        [HttpGet]
        [Route("api/Personnel/Image/{personnelId}")]
        public HttpResponseMessage GetPersonnelImage(int personnelId)
        {
            return Get(() => mapper.Map<PersonnelImageModel>(personnelImageManager.GetPersonnelImage(personnelId)));
        }

        /// <summary>
        ///   Méthode Post d'enregistrement des images du personnel
        /// </summary>
        /// <param name="persoImage">Entité model PersonnelImage avec photo de profil et signature dans un tableau de bytes</param>
        /// <returns>Entité personnelImage créée</returns>
        [HttpPost]
        [Route("api/Personnel/Image/")]
        public HttpResponseMessage AddOrUpdatePersonnelImage(PersonnelImageModel persoImage)
        {
            return Post(() => mapper.Map<PersonnelImageModel>(personnelImageManager.AddOrUpdatePersonnelImage(mapper.Map<PersonnelImageEnt>(persoImage))));
        }

        /// <summary>
        /// Get groupe du personnel by personnel identifier
        /// </summary>
        /// <param name="personnelId">Personnel identifier</param>
        /// <returns>Groupe entité</returns>
        [HttpGet]
        [Route("api/Personnel/GetPersonnelGroupebyId/{personnelId}")]
        public HttpResponseMessage GetPersonnelGroupebyId(int personnelId)
        {
            return Get(() => mapper.Map<GroupeModel>(personnelManager.GetPersonnelGroupebyId(personnelId)));
        }

        #region Gestion des Intérimaires
        /// <summary>
        ///   Récupère la liste des affectations du personnel intérimaire
        /// </summary>
        /// <param name="personnelId">Personnel Id</param>
        /// <returns>Http Response</returns>
        [HttpGet]
        [Route("api/Personnel/AffectationInterimaire/{personnelId}")]
        public HttpResponseMessage GetAffectationInterimaireList(int personnelId)
        {
            return Get(() => mapper.Map<IEnumerable<AffectationInterimaireModel>>(personnelManager.GetAffectationInterimaireList(personnelId)));
        }

        /// <summary>
        ///   Récupère la liste des affectations du personnel intérimaire avec pagination
        /// </summary>
        /// <param name="personnelId">Personnel Id</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>Http Response</returns>
        [HttpGet]
        [Route("api/Personnel/AffectationInterimaire/{personnelId}/{page?}/{pageSize?}")]
        public HttpResponseMessage GetAffectationInterimaireListWithPagination(int personnelId, int page = 1, int pageSize = 20)
        {
            return Get(() => mapper.Map<IEnumerable<AffectationInterimaireModel>>(personnelManager.GetAffectationInterimaireList(personnelId, page, pageSize)));
        }

        /// <summary>
        ///   Ajoute une nouvelle affectation
        /// </summary>
        /// <param name="affectation">Affectation à ajouter</param>
        /// <returns>Http Response</returns>
        [HttpPost]
        [Route("api/Personnel/AffectationInterimaire/")]
        public HttpResponseMessage AddAffectationInterimaire(AffectationInterimaireModel affectation)
        {
            return Post(() => mapper.Map<AffectationInterimaireModel>(personnelManager.AddAffectationInterimaire(mapper.Map<ContratInterimaireEnt>(affectation))));
        }

        /// <summary>
        ///   Met à jour une affectation
        /// </summary>
        /// <param name="affectation">Affectation à mettre à jour</param>
        /// <returns>Http Response</returns>
        [HttpPut]
        [Route("api/Personnel/AffectationInterimaire/")]
        public HttpResponseMessage UpdateAffectationInterimaire(AffectationInterimaireModel affectation)
        {
            return Put(() => mapper.Map<AffectationInterimaireModel>(personnelManager.UpdateAffectationInterimaire(mapper.Map<ContratInterimaireEnt>(affectation))));
        }

        #endregion

        /// <summary>
        /// Souscrit à la mailling list qui recapitule les activités en cours
        /// </summary>
        /// <param name="personnelId">personnelId</param>
        /// <returns>Http Response</returns>
        [HttpGet]
        [Route("api/Personnel/HasSubscribeToEmailSummary/{personnelId}")]
        public HttpResponseMessage HasSubscribeToEmailSummary(int personnelId)
        {
            return Get(() =>
            {
                var hasSubscribeToMaillings = emailSubscriptionManager.HasSubscribeToMaillingList(Entities.Email.EmailSouscriptionKey.ActivitySummary, personnelId);
                return hasSubscribeToMaillings.FirstOrDefault();
            });
        }

        /// <summary>
        /// Souscrit à la mailling list qui recapitule les activités en cours
        /// </summary>
        /// <param name="personnelId">personnelId</param>
        /// <returns>Http Response</returns>
        [HttpPost]
        [Route("api/Personnel/ActiveEmailSummary/")]
        public HttpResponseMessage ActiveEmailSummary([FromBody] int personnelId)
        {
            return Post(() => emailSubscriptionManager.SubscribeToMaillingList(Entities.Email.EmailSouscriptionKey.ActivitySummary, personnelId));
        }

        /// <summary>
        /// Souscrit à la mailling list qui recapitule les activités en cours
        /// </summary>
        /// <param name="personnelId">personnelId</param>
        /// <returns>Http Response</returns>
        [HttpDelete]
        [Route("api/Personnel/DisableEmailSummary/{personnelId}")]
        public HttpResponseMessage DisableEmailSummary(int personnelId)
        {
            return Delete(() => emailSubscriptionManager.UnSubscribeToMaillingList(Entities.Email.EmailSouscriptionKey.ActivitySummary, personnelId));
        }

        /// <summary>
        /// POST Export des receptions intérimaires suivant les sociétés sélectionnées
        /// </summary>
        /// <param name="societes">liste de sociétés</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Personnel/ExportReceptionInterimaires")]
        public async Task<IHttpActionResult> ExportReceptionInterimairesAsync(List<SocieteModel> societes)
        {
            await personnelManagerExterne.ExportReceptionInterimairesAsync(societes);

            return Ok();
        }

        /// <summary>
        /// Update personnels pointable by etablisssemnt paie idenetifiant
        /// </summary>
        /// <param name="etablissementPaieId">Etablissement paie id</param>
        /// <param name="isNonPointable">true si les personnels sont non pointables</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Personnel/UpdatePersonnelsPointableByEtatPaieId/{etablissementPaieId}/{isNonPointable}")]
        public HttpResponseMessage UpdatePersonnelsPointableByEtatPaieId(int etablissementPaieId, bool isNonPointable)
        {
            return Post(() => personnelManager.UpdatePersonnelsPointableByEtatPaieId(etablissementPaieId, isNonPointable));
        }

        /// <summary>
        /// Get etab paie list for validation pointage vrac Fes Async
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="recherche">Recherche</param>
        /// <param name="societeId">Societe Id</param>
        /// <param name="dateDebut">Date debut</param>
        /// <param name="statutPersonnelList">List des personnels statut</param>
        /// <returns>Etab Paie List</returns>
        [HttpGet]
        [Route("api/Personnel/GetPersonnelListForValidationPointageVracFesAsync/{page?}/{pageSize?}/{recherche?}/{societeId?}/{dateDebut?}")]
        public async Task<IHttpActionResult> GetPersonnelListForValidationPointageVracFesAsync(int page, int pageSize, string recherche, int? societeId, DateTime dateDebut, [FromUri] IEnumerable<string> statutPersonnelList)
        {
            IEnumerable<PersonnelEnt> personnelList = await personnelManager.GetPersonnelListForValidationPointageVracFesAsync(page, pageSize, recherche, societeId, dateDebut, statutPersonnelList);
            return Ok(mapper.Map<IEnumerable<PersonnelModel>>(personnelList));
        }
    }
}
