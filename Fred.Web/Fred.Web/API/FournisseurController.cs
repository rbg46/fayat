using AutoMapper;
using Fred.Business.ExternalService;
using Fred.Business.Referential;
using Fred.Business.Utilisateur;
using Fred.Entities.Referential;
using Fred.Web.Models.Personnel;
using Fred.Web.Models.Referential;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace Fred.Web.API
{
    /// <summary>
    /// Controller WEB API des fournisseurs
    /// </summary>
    public class FournisseurController : ApiControllerBase
    {
        /// <summary>
        /// Manager business des fournisseurs
        /// </summary>
        protected readonly IFournisseurManager fournisseurMgr;

        /// <summary>
        /// Mapper Model / ModelVue
        /// </summary>
        protected readonly IMapper mapper;

        protected readonly IUtilisateurManager utilisateurManager;

        protected readonly IFournisseurManagerExterne fournisseurManagerExterne;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="FournisseurController" />.
        /// </summary>
        /// <param name="fournisseurMgr">Manager de fournisseurs</param>
        /// <param name="mapper">Mapper Model / ModelVue</param>
        /// <param name="utilisateurManager">Manager des utilisateurs</param>
        /// <param name="fournisseurManagerExterne">Gestion externe des fournisseurs</param>
        public FournisseurController(IFournisseurManager fournisseurMgr, IMapper mapper, IUtilisateurManager utilisateurManager,
            IFournisseurManagerExterne fournisseurManagerExterne)
        {
            this.fournisseurMgr = fournisseurMgr;
            this.mapper = mapper;
            this.utilisateurManager = utilisateurManager;
            this.fournisseurManagerExterne = fournisseurManagerExterne;
        }

        /// <summary>
        /// Méthode GET de récupération d'un fournisseur
        /// </summary>
        /// <param name="fournisseurId">Identifiant du fournisseur</param>
        /// <returns>Retourne le fournisseur</returns>
        [HttpGet]
        [Route("api/Fournisseur/{fournisseurId}")]
        public HttpResponseMessage GetById(int fournisseurId)
        {
            return Get(() => this.mapper.Map<FournisseurModel>(this.fournisseurMgr.GetFournisseur(fournisseurId, null)));
        }

        /// <summary>
        /// Méthode GET de récupération du personnel intérimaire lié au fournisseur
        /// </summary>
        /// <param name="fournisseurId">Identifiant du fournisseur</param>
        /// <returns>Retourne la liste des personnels</returns>
        [HttpGet]
        [Route("api/Fournisseur/GetPersonnelInterimaireList/{fournisseurId}")]
        public HttpResponseMessage GetPersonnelInterimaireList(int fournisseurId)
        {
            return Get(() => mapper.Map<IEnumerable<PersonnelModel>>(fournisseurMgr.GetPersonnelInterimaireList(fournisseurId)));
        }

        /// <summary>
        /// Méthode GET de récupération du nombre de personnel intérimaire lié au fournisseur
        /// </summary>
        /// <param name="fournisseurId">Identifiant du fournisseur</param>
        /// <returns>Retourne le nombre de personnel intérimaire </returns>
        [HttpGet]
        [Route("api/Fournisseur/GetCountPersonnelInterimaire/{fournisseurId}")]
        public HttpResponseMessage GetCountPersonnelInterimaire(int fournisseurId)
        {
            return Get(() => fournisseurMgr.GetCountPersonnelList(fournisseurId));
        }

        /// <summary>
        /// Rechercher les référentiels Fournisseur
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">Texte de recherche sur le libellé du fournisseur</param>
        /// <param name="recherche2">Autres information de Recherche (Adresse, Code, SIREN)</param>
        /// <param name="withCommandValide">Fournisseur avec Commande valide</param>
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/Fournisseur/SearchLight/{page?}/{pageSize?}/{recherche?}/{groupeId?}/{recherche2?}/{ciId?}/{withCommandValide?}")]

        public HttpResponseMessage SearchLight(
            int? groupeId = null,
            int? ciId = null,
            int page = 1,
            int pageSize = 20,
            string recherche = "",
            string recherche2 = "",
            bool? withCommandValide = false)
        {
            return Get(() => mapper.Map<IEnumerable<FournisseurModel>>(fournisseurMgr.SearchLight(recherche, page, pageSize, groupeId, recherche2, ciId, withCommandValide)));
        }

        /// <summary>
        ///   Recherche avec filtres des fournisseurs
        /// </summary>
        /// <param name="filters">Filtre</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Taille page</param>    
        /// <returns>Une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Fournisseur/SearchFournisseurWithFilter/{page?}/{pageSize?}")]
        public HttpResponseMessage SearchFournisseurWithFilter(SearchFournisseurModel filters, int page = 1, int pageSize = 20)
        {
            return this.Post(() => mapper.Map<IEnumerable<FournisseurModel>>(fournisseurMgr.SearchFournisseurWithFilters(mapper.Map<SearchFournisseurEnt>(filters), page, pageSize)));
        }

        /// <summary>
        /// Méthode GET de récupération d'un filter (SearchFournisseurModel)
        /// </summary>    
        /// <returns>Retourne un nouveau filter</returns>
        [HttpGet]
        [Route("api/Fournisseur/Filter")]
        public HttpResponseMessage GetFilter()
        {
            return Get(() => mapper.Map<SearchFournisseurModel>(fournisseurMgr.GetFilter()));
        }

        /// <summary>
        /// Méthode GET de récupération d'un filter (SearchFournisseurModel)
        /// </summary>   
        /// <param name="groupeId">identifiant du groupe</param>
        /// <returns>Retourne un nouveau filter</returns>
        [HttpGet]
        [Route("api/Fournisseur/ETT/{groupeId}")]
        public HttpResponseMessage GetFournisseurETT(int groupeId)
        {
            return Get(() => mapper.Map<IEnumerable<FournisseurModel>>(fournisseurMgr.GetFournisseurETT(groupeId)));
        }

        /// <summary>
        ///   Exécution de l'import des fournisseurs ANAEL vers FRED
        /// </summary>
        /// <returns>Vrai si l'opération s'est bien lancée</returns>
        [HttpGet]
        [Route("api/Fournisseur/ImportFournisseur")]
        public HttpResponseMessage ExecuteImportFournisseur()
        {
            var codeSocieteComptable = utilisateurManager.GetContextUtilisateur()?.Personnel?.Societe?.CodeSocieteComptable;
            return Get(() => fournisseurManagerExterne.ExecuteImport(codeSocieteComptable));
        }
    }
}
