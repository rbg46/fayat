using System.Collections.Generic;
using AutoMapper;
using Fred.Business.ExplorateurDepense;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Bareme.Search;
using Fred.Entities.Budget.Search;
using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.CompteExploitation;
using Fred.Entities.Depense;
using Fred.Entities.Facture;
using Fred.Entities.Favori;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Personnel;
using Fred.Entities.PointagePersonnel.Search;
using Fred.Entities.Rapport.Search;
using Fred.Entities.Referential;
using Fred.Entities.Search;
using Fred.Web.Models;
using Fred.Web.Models.CI;
using Fred.Web.Models.Commande;
using Fred.Web.Models.Facture;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static Fred.Entities.Constantes;

namespace Fred.Business.Favori
{
    public class FavoriManager : Manager<FavoriEnt, IFavoriRepository>, IFavoriManager
    {
        private readonly IUtilisateurManager userManager;
        private readonly IMapper mapper;

        public FavoriManager(IUnitOfWork uow, IFavoriRepository favoriRepository, IUtilisateurManager userManager, IMapper mapper)
          : base(uow, favoriRepository)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public IEnumerable<FavoriEnt> GetFavoriList()
        {
            return Repository.GetFavoriList() ?? new FavoriEnt[] { };
        }

        /// <inheritdoc />
        public FavoriEnt GetNewFavori(string type)
        {
            FavoriEnt favori = new FavoriEnt();

            if (!string.IsNullOrEmpty(type))
            {
                favori.TypeFavori = type;
                if (type.Equals("Rapport"))
                {
                    favori.UrlFavori = "/Pointage/" + type + "/Index/";
                }
                else
                {
                    favori.UrlFavori = "/" + type + "/" + type + "/Index/";
                }
            }

            return favori;
        }

        /// <inheritdoc />
        public IEnumerable<FavoriEnt> GetFavoriList(int userId)
        {
            return Repository.GetFavoriList(userId) ?? new FavoriEnt[] { };
        }

        /// <inheritdoc />
        public FavoriEnt GetFavoriById(int favoriId)
        {
            return Repository.GetFavoriById(favoriId);
        }

        /// <inheritdoc />
        public FavoriEnt AddFavori(AbstractSearch filters, FavoriEnt favori)
        {
            return Repository.AddFavori(filters, favori, userManager.GetContextUtilisateurId());
        }

        /// <inheritdoc />
        public FavoriEnt AddFavori(FavoriEnt favori)
        {
            Repository.Insert(favori);
            Save();

            return favori;
        }

        /// <inheritdoc />
        public FavoriEnt UpdateFavori(FavoriEnt favoriEnt)
        {
            Repository.Update(favoriEnt);
            Save();

            return favoriEnt;
        }

        /// <inheritdoc />
        public void DeleteFavoriById(int id)
        {
            Repository.DeleteFavoriById(id);
        }

        /// <inheritdoc />
        public IEnumerable<FavoriEnt> SearchFavoris(int idUtilisateur)
        {
            if (idUtilisateur < 0)
            {
                return GetFavoriList();
            }

            return Repository.SearchFavorisByIdUtilisateur(idUtilisateur);
        }

        /// <inheritdoc />
        public IEnumerable<FavoriEnt> GetUtilisateurFavoriList()
        {
            return GetFavoriList(userManager.GetContextUtilisateurId());
        }

        /// <summary>
        /// Handle Add favoris
        /// </summary>
        /// <param name="favEnt">Favori entity</param>
        /// <param name="filtre">Filtres</param>
        /// <returns>Favori</returns>
        public FavoriEnt HandleAddFavoris(FavoriEnt favEnt, object filtre)
        {
            if (favEnt == null)
            {
                return null;
            }

            favEnt.UrlFavori = favEnt.UrlFavori + "?favoriId=";
            var format = "dd/MM/yyyy";
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };

            switch (favEnt.TypeFavori)
            {
                case EntityType.OperationDiverse:
                    SearchOperationDiverseModel mySearchOperationDiverse = JsonConvert.DeserializeObject<SearchOperationDiverseModel>(filtre?.ToString(), dateTimeConverter);
                    return AddFavori(mapper.Map<SearchOperationDiverseEnt>(mySearchOperationDiverse), favEnt);
                case EntityType.CompteExploitationEdition:
                    SearchCompteExploitationEditionModel mySearchCompteExploitationEdition = JsonConvert.DeserializeObject<SearchCompteExploitationEditionModel>(filtre?.ToString(), dateTimeConverter);
                    return AddFavori(mapper.Map<SearchCompteExploitationEditionEnt>(mySearchCompteExploitationEdition), favEnt);
                case EntityType.ExplorateurDepense:
                    SearchExplorateurDepenseModel mySearchDepenseExplorateur = JsonConvert.DeserializeObject<SearchExplorateurDepenseModel>(filtre?.ToString());
                    return AddFavori(mapper.Map<SearchExplorateurDepense>(mySearchDepenseExplorateur), favEnt);
                case EntityType.TableauReception:
                    SearchDepenseModel mySearchDepenseTableau = JsonConvert.DeserializeObject<SearchDepenseModel>(filtre?.ToString(), dateTimeConverter);
                    return AddFavori(mapper.Map<SearchDepenseEnt>(mySearchDepenseTableau), favEnt);
                case EntityType.CommandeReception:
                    SearchReceivableOrdersFilterModel mySearchCommandeReception = JsonConvert.DeserializeObject<SearchReceivableOrdersFilterModel>(filtre?.ToString(), dateTimeConverter);
                    return AddFavori(mapper.Map<SearchReceivableOrdersFilter>(mySearchCommandeReception), favEnt);
                case EntityType.ListePointagePersonnel:
                    SearchListePointagePersonnelModel mySearchListePointagePersonnel = JsonConvert.DeserializeObject<SearchListePointagePersonnelModel>(filtre?.ToString());
                    return AddFavori(mapper.Map<SearchListePointagePersonnelEnt>(mySearchListePointagePersonnel), favEnt);
                case EntityType.Avancement:
                    SearchAvancementModel mySearchAvancement = JsonConvert.DeserializeObject<SearchAvancementModel>(filtre?.ToString());
                    return AddFavori(mapper.Map<SearchAvancementEnt>(mySearchAvancement), favEnt);
                case EntityType.ControleBudgetaire:
                    SearchControleBudgetaireModel mySearchControleBudgetaire = JsonConvert.DeserializeObject<SearchControleBudgetaireModel>(filtre?.ToString());
                    return AddFavori(mapper.Map<SearchControleBudgetaireEnt>(mySearchControleBudgetaire), favEnt);
                case EntityType.ListeBudget:
                    SearchListeBudgetModel mySearchListeBudget = JsonConvert.DeserializeObject<SearchListeBudgetModel>(filtre?.ToString());
                    return AddFavori(mapper.Map<SearchListeBudgetEnt>(mySearchListeBudget), favEnt);
                case EntityType.DetailBudget:
                    SearchDetailBudgetModel mySearchDetailBudget = JsonConvert.DeserializeObject<SearchDetailBudgetModel>(filtre?.ToString());
                    return AddFavori(mapper.Map<SearchDetailBudgetEnt>(mySearchDetailBudget), favEnt);
                case EntityType.BudgetBibliothequePrix:
                    SearchBibliothequePrixModel mySearchBibliothequePrix = JsonConvert.DeserializeObject<SearchBibliothequePrixModel>(filtre?.ToString());
                    return AddFavori(mapper.Map<SearchBibliothequePrixEnt>(mySearchBibliothequePrix), favEnt);
                case EntityType.CI:
                    SearchCIModel mySearchCI = JsonConvert.DeserializeObject<SearchCIModel>(filtre?.ToString());
                    return AddFavori(mapper.Map<SearchCIEnt>(mySearchCI), favEnt);
                case EntityType.Commande:
                    SearchCommandeModel mySearchCommande = JsonConvert.DeserializeObject<SearchCommandeModel>(filtre?.ToString(), dateTimeConverter);
                    return AddFavori(mapper.Map<SearchCommandeEnt>(mySearchCommande), favEnt);
                case EntityType.Rapport:
                    SearchRapportModel mySearchRapport = JsonConvert.DeserializeObject<SearchRapportModel>(filtre?.ToString());
                    return AddFavori(mapper.Map<SearchRapportEnt>(mySearchRapport), favEnt);
                case EntityType.Depense:
                    SearchDepenseModel mySearchDepense = JsonConvert.DeserializeObject<SearchDepenseModel>(filtre?.ToString());
                    return AddFavori(mapper.Map<SearchDepenseEnt>(mySearchDepense), favEnt);
                case EntityType.Facture:
                    SearchFactureModel mySearchFacture = JsonConvert.DeserializeObject<SearchFactureModel>(filtre?.ToString());
                    return AddFavori(mapper.Map<SearchFactureEnt>(mySearchFacture), favEnt);
                case EntityType.Personnel:
                    SearchPersonnelModel mySearchPersonnel = JsonConvert.DeserializeObject<SearchPersonnelModel>(filtre?.ToString());
                    return AddFavori(mapper.Map<SearchPersonnelEnt>(mySearchPersonnel), favEnt);
                case EntityType.Fournisseur:
                    SearchFournisseurModel mySearchFournisseur = JsonConvert.DeserializeObject<SearchFournisseurModel>(filtre?.ToString());
                    return AddFavori(mapper.Map<SearchFournisseurEnt>(mySearchFournisseur), favEnt);
                case EntityType.Tache:
                    SearchTacheModel mySearchTache = JsonConvert.DeserializeObject<SearchTacheModel>(filtre?.ToString());
                    return AddFavori(mapper.Map<SearchTacheEnt>(mySearchTache), favEnt);
                case EntityType.BaremeExploitationCI:
                    SearchBaremeExploitationCIModel mySearchBaremeExplotationCI = JsonConvert.DeserializeObject<SearchBaremeExploitationCIModel>(filtre?.ToString());
                    return AddFavori(mapper.Map<SearchBaremeExploitationCIEnt>(mySearchBaremeExplotationCI), favEnt);
                case EntityType.BaremeExploitationOrganisation:
                    SearchBaremeExploitationOrganisationModel mySearchBaremeExplotationOrganisation = JsonConvert.DeserializeObject<SearchBaremeExploitationOrganisationModel>(filtre?.ToString());
                    return AddFavori(mapper.Map<SearchBaremeExploitationOrganisationEnt>(mySearchBaremeExplotationOrganisation), favEnt);
                default:
                    return favEnt;
            }
        }
    }
}
