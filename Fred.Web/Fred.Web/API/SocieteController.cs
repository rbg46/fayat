using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Fred.Business.Images;
using Fred.Business.IndemniteDeplacement;
using Fred.Business.Journal;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Entities;
using Fred.Entities.Journal;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using Fred.Web.Models.Groupe;
using Fred.Web.Models.Journal;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Societe;
using Fred.Web.Shared.Models;
using Fred.Web.Shared.Models.Image;
using Fred.Web.Shared.Models.IndemniteDeplacement;

namespace Fred.Web.API
{
    public class SocieteController : ApiControllerBase
    {
        private readonly ISocieteManager societeManager;
        private readonly IMapper mapper;
        private readonly IUtilisateurManager userManager;
        private readonly IJournalManager journalManager;
        private readonly IImageManager imageManager;
        private readonly IIndemniteDeplacementManager indemniteDeplacementManager;
        private readonly IAssocieSepManager associeSepManager;

        public SocieteController(
            ISocieteManager societeManager,
            IMapper mapper,
            IUtilisateurManager userManager,
            IJournalManager journalManager,
            IImageManager imageManager,
            IIndemniteDeplacementManager indemniteDeplacementManager,
            IAssocieSepManager associeSepManager)
        {
            this.societeManager = societeManager;
            this.mapper = mapper;
            this.userManager = userManager;
            this.journalManager = journalManager;
            this.imageManager = imageManager;
            this.indemniteDeplacementManager = indemniteDeplacementManager;
            this.associeSepManager = associeSepManager;
        }

        /// <summary>
        /// Méthode GET de récupération de toutes les sociétés.
        /// </summary>
        /// <returns>Retourne la liste de toutes les sociétés</returns>
        [HttpGet]
        [Route("api/Societe/All")]
        public HttpResponseMessage GetAll()
        {
            return this.Get(() => this.mapper.Map<IEnumerable<SocieteModel>>(this.societeManager.GetSocieteListAll()));
        }

        /// <summary>
        /// Méthode GET de récupération d'une société à partir de son dientifiant
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Retourne une société</returns>
        [HttpGet]
        [Route("api/Societe/GetSocieteById/{societeId}")]
        public HttpResponseMessage GetSocieteById(int societeId)
        {
            return this.Get(() => this.mapper.Map<SocieteModel>(societeManager.GetSocieteByIdWithParameters(societeId)));
        }

        /// <summary>
        /// Méthode GET de récupération de la société intérimaire par défaut
        /// </summary>   
        /// <returns>Retourne la société intérimaire par défaut</returns>
        [HttpGet]
        [Route("api/Societe/GetDefaultSocieteInterim")]
        public HttpResponseMessage GetDefaultSocieteInterim()
        {
            return Get(() => mapper.Map<SocieteModel>(societeManager.GetDefaultSocieteInterim()));
        }

        /// <summary>
        /// Méthode GET de récupération des sociétés
        /// </summary>
        /// <returns>Retourne la liste des sociétés</returns>
        [HttpGet]
        [Route("api/Societe/")]
        public HttpResponseMessage Get()
        {
            return this.Get(() => this.mapper.Map<IEnumerable<SocieteModel>>(this.societeManager.GetSocieteList()));
        }

        /// <summary>
        /// Vérifie si le code société existe déjà en base ou pas
        /// </summary>
        /// <param name="codeSociete">Code société</param>
        /// <param name="libelle">libelle</param>
        /// <param name="idCourant">Identifiant courant de la société</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpGet]
        [Route("api/Societe/CheckSocieteExists/{codeSociete}/{libelle}/{idCourant}/")]
        public HttpResponseMessage CheckSocieteExists(string codeSociete, string libelle, int idCourant)
        {
            return this.Get(() => this.societeManager.GetSocieteExistsWithSameCodeOrLibelle(idCourant, codeSociete, libelle));
        }

        /// <summary>
        /// Méthode GET de récupération d'une nouvelle instance société intialisée.
        /// </summary>
        /// <returns>Retourne une nouvelle instance société intialisée</returns>
        [HttpGet]
        //[FredWebApiAuthorizeAttribute(claimPermissionType:"societeNew")]
        [Route("api/Societe/New")]
        public HttpResponseMessage New()
        {
            return this.Get(() => this.mapper.Map<SocieteModel>(this.societeManager.GetNewSociete()));
        }

        /// <summary>
        ///   Méthode GET de recherche des sociétés
        /// </summary>
        /// <param name="filters">Filtre</param>
        /// <returns>Retourne la liste des sociétés</returns>
        [HttpPost]
        [Route("api/Societe/SearchAll")]
        public HttpResponseMessage SearchAll(SearchSocieteModel filters)
        {
            return this.Post(() => mapper.Map<IEnumerable<SocieteModel>>(this.societeManager.SearchSocieteAllWithFilters(this.mapper.Map<SearchSocieteEnt>(filters))));
        }

        /// <summary>
        /// Méthode POST de récupération des filtres de recherche sur Société
        /// </summary>
        /// <returns>Retourne la liste des filtres de recherche sur Société</returns>
        [HttpGet]
        [Route("api/Societe/Filter/")]
        public HttpResponseMessage Filters()
        {
            return this.Get(() => this.mapper.Map<SearchSocieteModel>(this.societeManager.GetDefaultFilter()));
        }

        /// <summary>
        /// POST api/controller
        /// </summary>
        /// <param name="societeModel">Société à traiter</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Societe/")]
        public async Task<IHttpActionResult> PostAsync(SocieteModel societeModel)
        {
            if (societeModel.GroupeId == 0)
            {
                UtilisateurEnt currentUser = await this.userManager.GetContextUtilisateurAsync();
                societeModel.GroupeId = currentUser.Personnel.Societe.GroupeId;
            }

            return Ok(this.mapper.Map<SocieteModel>(await this.societeManager.AddSocieteAsync(this.mapper.Map<SocieteEnt>(societeModel))));
        }

        /// <summary>
        /// PUT api/controller/5
        /// </summary>
        /// <param name="societe">Société à traiter</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPut]
        [Route("api/Societe/")]
        public async Task<IHttpActionResult> PutAsync(SocieteModel societe)
        {
            return Ok(mapper.Map<SocieteModel>(await this.societeManager.UpdateSocieteAsync(this.mapper.Map<SocieteEnt>(societe))));
        }

        /// <summary>
        ///   DELETE Supprime une Société par son identifiant
        /// </summary>
        /// <param name="societeId">Identifiant de la société à supprimer</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpDelete]
        [Route("api/Societe/{societeId}")]
        public HttpResponseMessage Delete(int societeId)
        {
            return this.Delete(() => this.societeManager.DeleteSocieteById(societeId));
        }

        /// <summary>
        /// DELETE api/controller/5
        /// </summary>
        /// <param name="model">Association à supprimer</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpDelete]
        [Route("api/Societe/DeleteSocieteDevise")]
        public HttpResponseMessage DeleteSocieteDevise(SocieteDeviseModel model)
        {
            return this.Delete(() => societeManager.DeleteDeviseBySocieteDevise(this.mapper.Map<SocieteDeviseEnt>(model)));
        }

        /// <summary>
        /// Retourne la liste des devises associé a la societe
        /// </summary>
        /// <param name="societe">Filtres de recherche.</param>
        /// <returns>La représentation JSON des fonctions recherché.</returns>
        [HttpPost]
        [Route("api/Societe/GetSocieteDevise/")]
        public HttpResponseMessage GetSocieteDevise(SocieteModel societe)
        {
            return this.Post(() => mapper.Map<IEnumerable<SocieteDeviseModel>>(this.societeManager.GetListSocieteDevise(this.mapper.Map<SocieteEnt>(societe))));
        }

        /// <summary>
        /// Retourne la liste des devises associé a la societe
        /// </summary>
        /// <param name="societe">Filtres de recherche.</param>
        /// <returns>La représentation JSON des fonctions recherché.</returns>
        [HttpPost]
        [Route("api/Societe/GetSocieteDeviseReference/")]
        public HttpResponseMessage GetSocieteDeviseReference(SocieteModel societe)
        {
            return this.Post(() => mapper.Map<DeviseModel>(this.societeManager.GetListSocieteDeviseRef(this.mapper.Map<SocieteEnt>(societe))));
        }

        /// <summary>
        /// Retourne la liste des devises secondaire associée à la societe
        /// </summary>
        /// <param name="societeId">Filtres de recherche.</param>
        /// <returns>La représentation JSON des devises recherché.</returns>
        [HttpPost]
        [Route("api/Societe/GetSocieteDeviseSec/{societeId}")]
        public HttpResponseMessage GetSocieteDeviseSec(int societeId)
        {
            return this.Post(() => mapper.Map<IEnumerable<DeviseModel>>(this.societeManager.GetListSocieteDeviseSec(societeId)));
        }

        /// <summary>
        ///   Rechercher les référentiels Societe
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>        
        /// <param name="typeSocieteCodes">Liste des codes Type Société</param>
        /// <param name="isExterne">Société Externe ou non</param>
        /// <returns>retouner une liste de Sociétés</returns>
        [HttpGet]
        [Route("api/Societe/SearchLight/{page?}/{pageSize?}/{recherche?}/{isExterne?}/{typeSocieteCodes?}")]
        public HttpResponseMessage SearchLight(int page = 1, int pageSize = 20, string recherche = "", [FromUri] string[] typeSocieteCodes = null, bool? isExterne = null)
        {
            var typeSocieteCodesList = typeSocieteCodes.Where(x => !string.IsNullOrEmpty(x)).ToList();
            return this.Get(() => this.mapper.Map<IEnumerable<SocieteModel>>(this.societeManager.SearchLight(recherche, page, pageSize, typeSocieteCodesList, isExterne)));
        }

        /// <summary>
        ///   Rechercher les référentiels Societe
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <param name="typeSocieteCodes">Liste des codes Type Société</param>
        /// <returns>retouner une liste de Sociétés</returns>
        [HttpGet]
        [Route("api/Societe/SearchLightForRoles/{page?}/{pageSize?}/{recherche?}/{typeSocieteCodes?}")]
        public HttpResponseMessage SearchLightForRoles(int page = 1, int pageSize = 20, string recherche = "", [FromUri] List<string> typeSocieteCodes = null)
        {
            typeSocieteCodes = typeSocieteCodes.Where(x => !string.IsNullOrEmpty(x)).ToList();
            return this.Get(() => this.mapper.Map<IEnumerable<SocieteModel>>(this.societeManager.SearchLightForRoles(recherche, page, pageSize, typeSocieteCodes)));
        }

        /// <summary>
        ///   GET Récupère la liste des associations SocieteDevise d'une société
        /// </summary>
        /// <param name="societeId">identifiant de la société</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpGet]
        [Route("api/Societe/GetSocieteDeviseList/{societeId}")]
        public HttpResponseMessage GetSocieteDeviseList(int societeId)
        {
            return Get(() => this.mapper.Map<IEnumerable<SocieteDeviseModel>>(this.societeManager.GetSocieteDeviseList(societeId)));
        }

        /// <summary>
        ///   POST Ajout ou Mise à jour des devises d'une Société
        /// </summary>
        /// <param name="societeId">Identifiant de la Société</param>
        /// <param name="societeDeviseList">Liste des relations Sociétés Devises</param>
        /// <returns>Met à jour les relations Sociétés/Devise pour une Société donné</returns>
        [HttpPost]
        [Route("api/Societe/ManageSocieteDeviseList/{societeId}")]
        public HttpResponseMessage ManageSocieteDeviseList(int societeId, IEnumerable<SocieteDeviseModel> societeDeviseList)
        {
            return Post(() => mapper.Map<IEnumerable<SocieteDeviseModel>>(societeManager.ManageSocieteDeviseList(societeId, mapper.Map<IEnumerable<SocieteDeviseEnt>>(societeDeviseList))));
        }

        /// <summary>
        ///   GET Récupère la liste des associations SocieteDevise d'une société
        /// </summary>
        /// <param name="societeId">identifiant de la société</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpGet]
        [Route("api/Societe/GetSocieteUniteList/{societeId}")]
        public HttpResponseMessage GetSocieteUniteList(int societeId)
        {
            return Get(() => this.mapper.Map<IEnumerable<UniteSocieteModel>>(this.societeManager.GetListSocieteUnite(societeId)));
        }

        /// <summary>
        ///   POST Ajout ou Mise à jour des devises d'une Société
        /// </summary>
        /// <param name="societeId">Identifiant de la Société</param>
        /// <param name="societeUniteList">Liste des relations Sociétés Devises</param>
        /// <returns>Met à jour les relations Sociétés/Devise pour une Société donné</returns>
        [HttpPost]
        [Route("api/Societe/ManageSocieteUniteList/{societeId}")]
        public HttpResponseMessage ManageSocieteUniteList(int societeId, IEnumerable<UniteSocieteModel> societeUniteList)
        {
            return Post(() => mapper.Map<IEnumerable<UniteSocieteModel>>(societeManager.ManageSocieteUniteList(societeId, mapper.Map<IEnumerable<UniteSocieteEnt>>(societeUniteList))));
        }

        /// <summary>
        ///   GET Récupère la liste des journaux d'une société
        /// </summary>
        /// <param name="societeId">identifiant de la société</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpGet]
        [Route("api/Societe/GetJournalList/{societeId}")]
        public HttpResponseMessage GetJournalListBySocieteId(int societeId)
        {
            return Get(() => this.mapper.Map<IEnumerable<JournalModel>>(this.journalManager.GetJournalList(societeId)));
        }

        /// <summary>
        ///   POST Ajout/Modification/Suppression des journaux d'une société
        /// </summary>
        /// <param name="societeId">Identifiant de la Société</param>
        /// <param name="journalList">Liste des relations Sociétés Devises</param>
        /// <returns>Met à jour les relations Sociétés/Devise pour une Société donné</returns>
        [HttpPost]
        [Route("api/Societe/ManageJournalList/{societeId}")]
        public HttpResponseMessage ManageJournalList(int societeId, IEnumerable<JournalModel> journalList)
        {
            return Post(() => mapper.Map<IEnumerable<JournalModel>>(journalManager.ManageJournalList(societeId, mapper.Map<IEnumerable<JournalEnt>>(journalList))));
        }

        /// <summary>
        /// Méthode GET de récupération de toutes les Images des Logins.
        /// </summary>
        /// <returns>Retourne la liste de toutes les images logins</returns>
        [HttpGet]
        [Route("api/Societe/LoginImages")]
        public HttpResponseMessage GetLoginImages()
        {
            return this.Get(() => this.mapper.Map<IEnumerable<ImageModel>>(this.imageManager.GetLoginImages()));
        }

        /// <summary>
        /// Méthode GET de récupération de toutes les Images des Logos.
        /// </summary>
        /// <returns>Retourne la liste de toutes les images logos</returns>
        [HttpGet]
        [Route("api/Societe/LogoImages")]
        public HttpResponseMessage GetLogoImages()
        {
            return this.Get(() => this.mapper.Map<IEnumerable<ImageModel>>(this.imageManager.GetLogoImages()));
        }

        /// <summary>
        /// Méthode GET de récupération de l'image Login de la societe.
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>L'image login de la société</returns>
        [HttpGet]
        [Route("api/Societe/{societeId}/GetLoginImage")]
        public HttpResponseMessage GetLoginImage(int societeId)
        {
            return this.Get(() => this.mapper.Map<ImageModel>(this.imageManager.GetLoginImage(societeId)));
        }

        /// <summary>
        /// Méthode GET de récupération de l'image Logo de la societe.
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>L'image logo de la société</returns>
        [HttpGet]
        [Route("api/Societe/{societeId}/GetLogoImage")]
        public HttpResponseMessage GetLogoImage(int societeId)
        {
            return this.Get(() => this.mapper.Map<ImageModel>(this.imageManager.GetLogoImage(societeId)));
        }

        /// <summary>
        /// Méthode de mise a jour de l'image Login de la societe.
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="imageId">imageId</param>
        /// <returns>L'image login de la société</returns>
        [HttpPut]
        [Route("api/Societe/{societeId}/UpdateLoginImage/{imageId}")]
        public async Task<IHttpActionResult> UpdateLoginImage(int societeId, int imageId)
        {
            return Ok(this.mapper.Map<ImageModel>(await this.imageManager.UpdateSocieteLoginImageAsync(societeId, imageId)));
        }

        /// <summary>
        /// Méthode de mise a jour de l'image Logo de la societe.
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="imageId">imageId</param>
        /// <returns>L'image logo de la société</returns>
        [HttpPut]
        [Route("api/Societe/{societeId}/UpdateLogoImage/{imageId}")]
        public async Task<IHttpActionResult> UpdateLogoImage(int societeId, int imageId)
        {
            return Ok(this.mapper.Map<ImageModel>(await this.imageManager.UpdateSocieteLogoImageAsync(societeId, imageId)));
        }

        /// <summary>
        ///   Retourne vrai si la société est paramétré pour la limitation des unités par ressource
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Vrai si la société est paramétré pour la limitation des unités par ressource</returns>
        [HttpGet]
        [Route("api/Societe/IsLimitationUnitesRessource/{societeId}")]
        public HttpResponseMessage IsLimitationUnitesRessource(int societeId)
        {
            // Appel vers la méthode de récupération des paramètres sociétés quand ceux-ci seront en place, en attendant on teste si la société appartient au groupe Fayat TP
            return Get(() => societeManager.IsSocieteInGroupe(societeId, Constantes.CodeGroupeFTP));
        }

        /// <summary>
        ///   Retourne vrai si la société est paramétré pour la modification des ressource dans les réceptions
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Vrai si la société est paramétré pour la modification des ressource dans les réceptions</returns>
        [HttpGet]
        [Route("api/Societe/IsModificationRessourceReceptions/{societeId}")]
        public HttpResponseMessage IsModificationRessourceReceptions(int societeId)
        {
            // Appel vers la méthode de récupération des paramètres sociétés quand ceux-ci seront en place, en attendant on teste si la société appartient au groupe Razel-Bec 
            return Get(() => societeManager.IsSocieteInGroupe(societeId, Constantes.CodeGroupeRZB));
        }

        /// <summary>
        /// Méthode GET de récupération des types de calcul des indemnités de déplacement.
        /// </summary>
        /// <returns>La liste des types de calcul des indemnités de déplacement.</returns>
        [HttpGet]
        [Route("api/Societe/GetIndemniteDeplacementCalculTypes")]
        public HttpResponseMessage GetIndemniteDeplacementCalculTypes()
        {
            return Get(() => this.mapper.Map<List<IndemniteDeplacementCalculTypeModel>>(this.indemniteDeplacementManager.GetCalculTypes().ToList()));
        }

        /// <summary>
        /// Méthode GET de récupération de la société en fonction de son organisation ID
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation</param>
        /// <returns>Le société retrouvée grâce à son identifiant unique d'organisation</returns>
        [HttpGet]
        [Route("api/Societe/Organisation/{organisationId}")]
        public HttpResponseMessage GetSocieteByOrganisationId(int organisationId)
        {
            return Get(() => this.mapper.Map<SocieteModel>(this.societeManager.GetSocieteByOrganisationId(organisationId)));
        }

        /// <summary>
        /// Get societes list for remontee vrac fes Async
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="recherche">Recherche</param>
        /// <returns>List des societes</returns>
        [HttpGet]
        [Route("api/Societe/GetSocietesListForRemonteeVracFesAsync/{page?}/{pageSize?}/{recherche?}")]
        public async Task<IHttpActionResult> GetSocietesListForRemonteeVracFesAsync(int page, int pageSize, string recherche)
        {
            IEnumerable<SocieteEnt> societes = await societeManager.GetSocietesListForRemonteeVracFesAsync(page, pageSize, recherche);
            return this.Ok(this.mapper.Map<IEnumerable<SocieteModel>>(societes));
        }

        #region Associe SEP

        /// <summary>
        /// Récupération de tous les associés SEP d'une Société
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Liste des associés SEP</returns>
        [HttpGet]
        [Route("api/Societe/{societeId}/AssocieSep")]
        public HttpResponseMessage GetAll(int societeId)
        {
            return Get(() => this.mapper.Map<List<AssocieSepModel>>(this.associeSepManager.GetAll(societeId)));
        }

        /// <summary>
        ///  POST Insertion d'une liste d'associés SEP
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="associeSeps">Liste de model d'associés SEP</param>
        /// <returns>Liste ajoutée</returns>
        [HttpPost]
        [Route("api/Societe/{societeId}/AssocieSep")]
        public HttpResponseMessage CreateOrUpdateAssocieSepRange(int societeId, List<AssocieSepModel> associeSeps)
        {
            return Post(() => this.mapper.Map<List<AssocieSepModel>>(this.associeSepManager.CreateOrUpdateRange(societeId, this.mapper.Map<List<AssocieSepEnt>>(associeSeps))));
        }

        /// <summary>
        ///  DELETE Suppression d'une liste d'associés SEP
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="associeSepIds">Liste d'identifiants d'associés SEP</param>        
        /// <returns>Code Http</returns>
        [HttpDelete]
        [Route("api/Societe/{societeId}/AssocieSep/{associeSepIds?}")]
        public HttpResponseMessage DeleteAssocieSepList(int societeId, [FromUri] List<int> associeSepIds)
        {
            return Delete(() => this.associeSepManager.DeleteRange(associeSepIds));
        }

        /// <summary>
        /// POST : Obtient la liste des groupes et sociétés en tenant compte des habilitations de l’utilisateur 
        /// </summary>
        /// <returns>liste des sociétés groupé par Groupe</returns>
        [HttpGet]
        [Route("api/Societe/GetSocietesGroupesByUserHabibilitation")]
        public HttpResponseMessage GetSocietesGroupesByUserHabibilitation()
        {
            return Get(() => mapper.Map<IEnumerable<GroupeModel>>(societeManager.GetSocietesGroupesByUserHabibilitation()));
        }

        #endregion
    }
}
