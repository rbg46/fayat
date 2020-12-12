using AutoMapper;
using Fred.Business;
using Fred.Business.Favori;
using Fred.Business.Utilisateur;
using Fred.Entities.Depense;
using Fred.Entities.Favori;
using Fred.Entities.Search;
using Fred.Framework.Exceptions;
using Fred.Web.Models;
using Fred.Web.Models.CI;
using Fred.Web.Models.Commande;
using Fred.Web.Models.Facture;
using Fred.Web.Models.Favori;
using Fred.Web.Models.Personnel;
using Fred.Web.Models.Rapport;
using Fred.Web.Models.Referential;
using Fred.Web.Shared.Models.Bareme.Search;
using Fred.Web.Shared.Models.Budget.Search;
using Fred.Web.Shared.Models.Commande;
using Fred.Web.Shared.Models.CompteExploitation;
using Fred.Web.Shared.Models.OperationDiverse;
using Fred.Web.Shared.Models.PointagePersonnel;
using Fred.Web.Shared.Models.Referential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using static Fred.Entities.Constantes;

namespace Fred.Web.API
{
    /// <summary>
    /// Controller WEB API des fournisseurs
    /// </summary>
    public class FavoriController : ApiControllerBase
    {
        private readonly IFavoriManager favoriManager;
        private readonly IUtilisateurManager userManager;
        private readonly IMapper mapper;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="FavoriController" />.
        /// </summary>
        /// <param name="favoriManager">Manager de Favoris</param>
        /// <param name="userManager">Manager des Utilisateurs</param>
        /// <param name="mapper">Mapper Model / ModelVue</param>
        public FavoriController(IFavoriManager favoriManager, IUtilisateurManager userManager, IMapper mapper)
        {
            this.favoriManager = favoriManager;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Méthode GET de récupération des favoris
        /// </summary>
        /// <returns>Retourne la liste des favoris</returns>
        public HttpResponseMessage Get()
        {
            return Get(() => this.mapper.Map<IEnumerable<FavoriModel>>(this.favoriManager.GetFavoriList()));
        }

        /// <summary>
        ///   GET de récupération d'un nouveau favori
        /// </summary>
        /// <param name="type">Type de favori</param>
        /// <returns>Retourne un nouvel objet favori</returns>
        [HttpGet]
        [Route("api/Favoris/New/{type?}")]
        public HttpResponseMessage New(string type)
        {
            return Get(() => this.mapper.Map<FavoriModel>(this.favoriManager.GetNewFavori(type)));
        }

        /// <summary>
        ///   GET Récupère un favori unique à partir de son identifiant
        /// </summary>
        /// <param name="favoriId">Identifiant du favori</param>
        /// <returns>Retourne un favori</returns>
        [HttpGet]
        [Route("api/Favoris/{favoriId}")]
        public async Task<IHttpActionResult> GetByIdAsync(int favoriId)
        {

            FavoriModel fav = this.mapper.Map<FavoriModel>(this.favoriManager.GetFavoriById(favoriId));
            var search = Framework.Tool.SerialisationTools.Deserialisation(fav.Search);

            await CheckAbilitiesAsync(search);

            switch (fav.TypeFavori)
            {
                case EntityType.OperationDiverse:
                    return Ok(this.mapper.Map<SearchOperationDiverseModel>(search));
                case EntityType.TableauReception:
                    return Ok(this.mapper.Map<SearchDepenseModel>(search));
                case EntityType.CompteExploitationEdition:
                    return Ok(this.mapper.Map<SearchCompteExploitationEditionModel>(search));
                case EntityType.ExplorateurDepense:
                    return Ok(this.mapper.Map<SearchExplorateurDepenseModel>(search));
                case EntityType.CommandeReception:
                    return Ok(this.mapper.Map<SearchReceivableOrdersFilterModel>(search));
                case EntityType.ListePointagePersonnel:
                    return Ok(this.mapper.Map<SearchListePointagePersonnelModel>(search));
                case EntityType.Avancement:
                    return Ok(this.mapper.Map<SearchAvancementModel>(search));
                case EntityType.ControleBudgetaire:
                    return Ok(this.mapper.Map<SearchControleBudgetaireModel>(search));
                case EntityType.ListeBudget:
                    return Ok(this.mapper.Map<SearchListeBudgetModel>(search));
                case EntityType.DetailBudget:
                    return Ok(this.mapper.Map<SearchDetailBudgetModel>(search));
                case EntityType.BudgetBibliothequePrix:
                    return Ok(this.mapper.Map<SearchBibliothequePrixModel>(search));
                case EntityType.CI:
                    return Ok(this.mapper.Map<SearchCIModel>(search));
                case EntityType.Commande:
                    return Ok(this.mapper.Map<SearchCommandeModel>(search));
                case EntityType.Depense:
                    return Ok(this.mapper.Map<SearchDepenseModel>(search));
                case EntityType.Rapport:
                    return Ok(this.mapper.Map<SearchRapportModel>(search));
                case EntityType.Facture:
                    return Ok(this.mapper.Map<SearchFactureModel>(search));
                case EntityType.Personnel:
                    return Ok(this.mapper.Map<SearchPersonnelModel>(search));
                case EntityType.Fournisseur:
                    return Ok(this.mapper.Map<SearchFournisseurModel>(search));
                case EntityType.Tache:
                    return Ok(this.mapper.Map<SearchTacheModel>(search));
                case EntityType.BaremeExploitationCI:
                    return Ok(this.mapper.Map<SearchBaremeExploitationCIModel>(search));
                case EntityType.BaremeExploitationOrganisation:
                    return Ok(this.mapper.Map<SearchBaremeExploitationOrganisationModel>(search));
                default:
                    return Ok();
            }
        }

        // Vérification des habilitations sur le CI de recherche
        private async Task CheckAbilitiesAsync(object search)
        {
            const string ciPropertyName = "CI";
            const string ciIdPropertyName = "CiId";

            var ci = GetValue(search, ciPropertyName);
            if (ci != null)
            {
                var ciId = GetValue(ci, ciIdPropertyName);
                int ciIdParsed = 0;
                if (ciId != null && int.TryParse(ciId.ToString(), out ciIdParsed))
                {
                    IEnumerable<int> userCiIds = await userManager.GetAllCIbyUserAsync(userManager.GetContextUtilisateurId());
                    if (userCiIds == null || !userCiIds.ToList().Contains(ciIdParsed))
                    {
                        throw new FredBusinessException($"{BusinessResources.Favori_UnauthorizedCI} {ciId}");
                    }
                }
            }
        }

        private object GetValue(object value, string propertyName)
        {
            var ciPropertyInfo = value.GetType().GetProperty(propertyName);
            if (ciPropertyInfo != null)
            {
                return ciPropertyInfo.GetValue(value, null);
            }

            return null;
        }

        /// <summary>
        ///   GET Récupère tout les favoris à partir d'un id utilisateur
        /// </summary>    
        /// <returns>Retourne la liste des societes partenaires</returns>
        [HttpGet]
        [Route("api/Favoris/")]
        public HttpResponseMessage GetList()
        {
            return Get(() =>
            {
                var favs = this.mapper.Map<IEnumerable<FavoriModel>>(this.favoriManager.GetUtilisateurFavoriList());

                AbstractSearch searchEnt = null;

                foreach (FavoriModel fav in favs)
                {
                    searchEnt = (AbstractSearch)Fred.Framework.Tool.SerialisationTools.Deserialisation(fav.Search);

                    fav.description = string.Format("Texte recherche : {0}", searchEnt?.ValueText ?? string.Empty);
                }

                return favs;
            });
        }

        /// <summary>
        ///   POST Ajout d'un favori
        /// </summary>
        /// <param name="favori">Favori à ajouter</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Favoris/")]
        public IHttpActionResult Add(FavoriModel favori)
        {
            var filtre = favori.Filtre;
            favori.Filtre = null;
            return Ok(favoriManager.HandleAddFavoris(mapper.Map<FavoriEnt>(favori), filtre));
        }

        /// <summary>
        ///   PUT Mise à jour d'un favori
        /// </summary>
        /// <param name="favori">Favori à mettre à jour</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPut]
        [Route("api/Favoris/")]
        public HttpResponseMessage Update(FavoriModel favori)
        {
            return Put(() =>
            {
                this.favoriManager.UpdateFavori(this.mapper.Map<FavoriEnt>(favori));
                return favori;
            });
        }

        /// <summary>
        ///   DELETE Suppression d'un favori
        /// </summary>
        /// <param name="favoriId">Identifiant du favori</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpDelete]
        [Route("api/Favoris/{favoriId}")]
        public HttpResponseMessage Delete(int favoriId)
        {
            return Delete(() => this.favoriManager.DeleteFavoriById(favoriId));
        }
    }
}
