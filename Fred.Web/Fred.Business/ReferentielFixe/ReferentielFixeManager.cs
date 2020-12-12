using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Fred.Business.CI;
using Fred.Business.Commande.Models;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Carburant;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using Fred.Framework.Exceptions;
using Fred.Framework.Tool;
using Fred.Web.Shared.Models.RessourceRecommandee;

namespace Fred.Business.ReferentielFixe
{
    public class ReferentielFixeManager : Manager, IReferentielFixeManager
    {
        private readonly IUnitOfWork uow;
        private readonly IChapitreRepository chapitreRepo;
        private readonly ISousChapitreRepository sousChapitreRepo;
        private readonly IRessourceRepository ressourceRepo;
        private readonly IRessourceRecommandeeRepository ressourceRecommendeeRepo;
        private readonly ISocieteManager societeManager;
        private readonly ICIManager ciManager;
        private readonly IEtablissementComptableManager etablissementComptableMgr;
        private readonly ISepService sepService;
        private readonly IUtilisateurManager userManager;
        private readonly IReferentielEtenduRepository refEtenduRepo;

        public ReferentielFixeManager(
            IUnitOfWork uow,
            IChapitreRepository chapitreRepo,
            ISousChapitreRepository sousChapitreRepo,
            IRessourceRepository ressourceRepo,
            IRessourceRecommandeeRepository ressourceRecommendeeRepo,
            ISocieteManager societeManager,
            ICIManager ciManager,
            IEtablissementComptableManager etablissementComptableMgr,
            ISepService sepService,
            IUtilisateurManager userManager,
            IReferentielEtenduRepository refEtenduRepo)
        {
            this.chapitreRepo = chapitreRepo;
            this.sousChapitreRepo = sousChapitreRepo;
            this.ressourceRepo = ressourceRepo;
            this.ressourceRecommendeeRepo = ressourceRecommendeeRepo;
            this.societeManager = societeManager;
            this.ciManager = ciManager;
            this.etablissementComptableMgr = etablissementComptableMgr;
            this.sepService = sepService;
            this.userManager = userManager;
            this.refEtenduRepo = refEtenduRepo;
            this.uow = uow;
        }

        /// <summary>
        /// Méthode de recherche d'un item de référentiel via son LibelleRef ou son codeRef
        /// </summary>
        /// <param name="text">Le texte recherché</param>
        /// <param name="societeId">L'identifiant de la société</param>
        /// <param name="page">La page courante</param>
        /// <param name="pageSize">La taille de la page</param>
        /// <param name="resourceTypeId">Identifiant du type de ressource</param>
        /// <param name="ressourceId">L'identifiant de la ressource. Si ce paramètre est renseigné, cela veut dire qu'on veut récupérer les ressources qui ont la même nature</param>
        /// <param name="achats">Indique si on veut uniquement les ressources disponible dans le module achat</param>
        /// <returns>Une liste d' items de référentiel</returns>
        public IEnumerable<RessourceEnt> SearchLight(string text, int societeId, int page, int pageSize, int? resourceTypeId, int? ressourceId, bool? achats = false)
        {
            SocieteEnt societe = societeManager.GetSocieteById(societeId, new List<Expression<Func<SocieteEnt, object>>> { x => x.TypeSociete });

            if (societe.TypeSociete.Code == Constantes.TypeSociete.Partenaire)
            {
                // Model
                SearchRessourcesAchatModel model = new SearchRessourcesAchatModel()
                {
                    Page = page,
                    PageSize = pageSize,
                    Text = text,
                    RessourceTypeId = resourceTypeId
                };

                return SearchLightReferentielFixe(model);
            }

            if (societe.TypeSociete.Code == Constantes.TypeSociete.Sep)
            {
                societeId = sepService.GetSocieteGerante(societeId).SocieteId;
            }

            // Model
            SearchRessourcesAchatModel searchRessourcesAchatModel = new SearchRessourcesAchatModel()
            {
                Text = text,
                SocieteId = societeId,
                Page = page,
                RessourceTypeId = resourceTypeId,
                RessourceId = ressourceId,
                AchatsEnable = (bool)achats
            };

            return SearchLightReferentielEtendu(searchRessourcesAchatModel);
        }

        /// <summary>
        /// Rechercher dans le référentiel selon les filtres passés en paramètres
        /// </summary>
        /// <param name="filter">Le filtre</param>
        /// <returns>Une liste d'items de référentiel</returns>
        public IEnumerable<RessourceEnt> SearchLight(SearchRessourcesAchatModel filter)
        {
            SocieteEnt societe = societeManager.GetSocieteById(filter.SocieteId, new List<Expression<Func<SocieteEnt, object>>> { x => x.TypeSociete });

            if (societe.TypeSociete.Code == Constantes.TypeSociete.Partenaire)
            {
                return SearchLightReferentielFixe(filter);
            }

            if (societe.TypeSociete.Code == Constantes.TypeSociete.Sep)
            {
                filter.SocieteId = sepService.GetSocieteGerante(filter.SocieteId).SocieteId;
            }

            return SearchLightReferentielEtendu(filter);
        }

        public async Task<List<RessourceEnt>> SearchRessourcesForAchatAsync(SearchRessourcesAchatModel searchFilters)
        {
            if (searchFilters == null)
                throw new ArgumentNullException(nameof(searchFilters));

            var currentUser = await userManager.GetContextUtilisateurAsync();

            if (currentUser == null)
                throw new FredBusinessException(BusinessResources.Search_Ressources_Current_user_Is_Null);

            var ciId = searchFilters.CiId;

            var ci = ciManager.GetCI(ciId);

            var organisationOfEtablissement = etablissementComptableMgr.GetOrganisationByEtablissementId(ci.EtablissementComptable.EtablissementComptableId);

            if (ci.EtablissementComptable == null || organisationOfEtablissement == null)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("EtablissementEmpty", BusinessResources.Search_Ressources_Etablissement_Empty)
                });
            }

            var etablissementOrganisationId = organisationOfEtablissement.OrganisationId;

            int societeId = sepService.GetSocieteGeranteForSep(ci.Societe).SocieteId;

            var request = new SearchRessourcesRequestModel()
            {
                CiId = ciId,
                EtablissementOrganisationId = etablissementOrganisationId,
                Page = searchFilters.Page,
                PageSize = searchFilters.PageSize,
                Recherche = searchFilters.Recherche,
                Recherche2 = searchFilters.Recherche2,
                RessourceIdNatureFilter = searchFilters.RessourceIdNatureFilter,
                RessourcesRecommandeesOnly = searchFilters.RessourcesRecommandeesOnly,
                SocieteId = societeId
            };

            var ressourcesFiltered = await ressourceRepo.SearchRessourcesForAchatAsync(request);

            return MapAndCleanRessources(ci, etablissementOrganisationId, ressourcesFiltered);
        }

        private static List<RessourceEnt> MapAndCleanRessources(CIEnt ci, int etablissementOrganisationId, List<RessourceEnt> resssourcesFiltered)
        {
            bool isEnabled = ci.EtablissementComptable.RessourcesRecommandeesEnabled;

            var resssources = resssourcesFiltered.Select(r => new
            {
                Ressource = r,
                IsRecommandee = isEnabled && r.ReferentielEtendus.Any(re => re.RessourcesRecommandees.Any(rr => rr.OrganisationId == etablissementOrganisationId))
            }).ToList();

            // Définition du flag IsRecommandee et clean des propriété parent/enfants inutiles
            resssources.ForEach(rr =>
            {
                rr.Ressource.IsRecommandee = rr.IsRecommandee;
                rr.Ressource.Parent = null;
                rr.Ressource.RessourcesEnfants = null;
            });

            return resssources.ConvertAll(x => x.Ressource);
        }

        /// <summary>
        /// SearchLight des ressources natures en fonction du référentiel étendu (FRED_SOCIETE_RESSOURCE_NATURE)
        /// </summary>
        /// <param name="text">Texte à rechercher</param>
        /// <param name="ci">CI</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="ressourceRecommandeesOnly">Identifiant Type de ressource</param>
        /// <param name="ressourceIdNatureFilter">identifiant de la nature</param>
        /// <returns>Liste de ressources avec le flag de recommandation</returns>
        public List<RessourceEnt> SearchRessourcesRecommandees(string text, CIEnt ci, int page, int pageSize, bool ressourceRecommandeesOnly, int? ressourceIdNatureFilter)
        {
            int? etablissementOrganisationId = ci.EtablissementComptable?.Organisation.OrganisationId;
            bool isEnabled = ci.EtablissementComptable?.RessourcesRecommandeesEnabled ?? false;

            var query = GetQueryRessourcesEntBase(text, ci, ressourceRecommandeesOnly, ressourceIdNatureFilter, etablissementOrganisationId);

            var ressourcesRecommandees = query.OrderBy(list => list.OrderBy(r => r.Libelle)).GetPage(page, pageSize)
                .Select(r => new
                {
                    Ressource = r,
                    IsRecommandee = isEnabled && r.ReferentielEtendus.Any(re => re.RessourcesRecommandees.Any(rr => rr.OrganisationId == etablissementOrganisationId))
                }).ToList();

            // Définition du flag IsRecommandee et clean des propriétés parent/enfants inutiles
            ressourcesRecommandees.ForEach(
            rr =>
            {
                rr.Ressource.IsRecommandee = rr.IsRecommandee;
                rr.Ressource.Parent = null;
                rr.Ressource.RessourcesEnfants = null;
            });

            return ressourcesRecommandees.ConvertAll(x => x.Ressource);
        }

        private IRepositoryQuery<RessourceEnt> GetQueryRessourcesEntBase(string text, CIEnt ci, bool ressourceRecommandeesOnly, int? ressourceIdNatureFilter, int? etablissementOrganisationId, string recherche2 = null)
        {
            int? societeId = sepService.GetSocieteGeranteForSep(ci.Societe)?.SocieteId;

            var query = GetResourcesFilter(text, ci, societeId ?? 0, recherche2);

            if (etablissementOrganisationId.HasValue && ressourceRecommandeesOnly)
            {
                query.Filter(r => r.ReferentielEtendus.Any(re => re.RessourcesRecommandees.Any(rr => rr.OrganisationId == etablissementOrganisationId)));
            }

            if (ressourceIdNatureFilter.HasValue)
            {
                int? natureIdRessource = ressourceRepo.GetNatureIdRessource(ressourceIdNatureFilter, societeId);
                query.Filter(r => r.ReferentielEtendus.Any(re => re.NatureId == natureIdRessource)
                    || r.RessourceRattachement.ReferentielEtendus.Any(re => re.NatureId == natureIdRessource));
            }
            return query;
        }

        //refacto
        private IRepositoryQuery<RessourceEnt> GetResourcesFilter(string text, CIEnt ci, int societeId, string recherche2 = null)
        {
            var query = ressourceRepo.Query()
                              .Include(r => r.TypeRessource)
                              .Include(r => r.ReferentielEtendus)
                              .Include(r => r.ReferentielEtendus.Select(re => re.RessourcesRecommandees))
                              .Filter(r => r.Active && r.DateSuppression == null)
                              .Filter(r => (r.ReferentielEtendus.Any(re => re.SocieteId == societeId && re.NatureId != null)
                                              || (r.IsRessourceSpecifiqueCi && r.SpecifiqueCiId == ci.CiId)))
                              .Filter(r => string.IsNullOrEmpty(text) || r.Code.Contains(text) || r.Libelle.Contains(text));

            // Filtre sur les mots clés
            if (!string.IsNullOrEmpty(recherche2))
            {
                string strFormatted = StringTool.RemoveDiacritics(recherche2.ToLower());

                query
                    .Filter(r => !string.IsNullOrEmpty(r.Keywords) && r.Keywords.ToLower().Contains(strFormatted));
            }

            return query;
        }

        /// <summary>
        /// Méthode de recherche d'un item de référentiel via son LibelleRef ou son codeRef
        /// </summary>
        /// <param name="text">Le texte recherché</param>
        /// <param name="societeId">L'identifiant de la société</param>
        /// <param name="page">La page courante</param>
        /// <param name="pageSize">La taille de la page</param>
        /// <param name="ressourceId">Identifiant de la ressource</param>
        /// <returns>Une liste d' items de référentiel</returns> 
        public IEnumerable<RessourceEnt> SearchLightByNature(string text, int societeId, int page, int pageSize, int? ressourceId)
        {
            var natureId = refEtenduRepo
                            .Query()
                            .Filter(r => r.SocieteId == societeId && r.RessourceId == ressourceId)
                            .Get()
                            .Select(r => r.NatureId)
                            .SingleOrDefault();

            var query = refEtenduRepo
                            .Query()
                            .Include(r => r.Ressource)
                            .Filter(r => r.SocieteId == societeId && r.NatureId == natureId && r.Achats && r.Ressource.Active && r.Ressource.DateSuppression == null)
                            .Filter(r => string.IsNullOrEmpty(text)
                                         || r.Ressource.Code.Contains(text)
                                         || r.Ressource.Libelle.Contains(text));

            return query.OrderBy(list => list.OrderBy(r => r.Ressource.Libelle)).GetPage(page, pageSize).Select(r => r.Ressource).ToList();
        }

        /// <summary>
        /// Retourne la liste de toutes les ressources d'un CI et d'une société,
        /// avec leur sous-chapitre et chapitre respectif
        /// </summary>
        /// <param name="societeId">L'identifiant de la société concernée</param>
        /// <param name="ciId">L'identifiant du CI concerné</param>
        /// <returns>Liste de Ressource</returns>
        public List<RessourceEnt> GetListRessourceBySocieteIdWithSousChapitreEtChapitre(int societeId, int ciId)
        {
            return ressourceRepo.GetListBySocieteIdWithSousChapitreEtChapitre(societeId, ciId).ToList();
        }

        /// <summary>
        /// Récupère la liste des ressources recommandées correspondant aux référentiels étendus
        /// </summary>
        /// <param name="etablissementCIOrganisationId">Identifiant de l'organisation à laquelle l'établissement comptable du CI courant appartient</param>
        /// <returns>Une liste de ressources recommandées</returns>
        public List<RessourceRecommandeeFromEtablissementCIOrganisationModel> GetRessourceRecommandeeList(int etablissementCIOrganisationId)
        {
            return ressourceRecommendeeRepo.GetRessourceRecommandeeList(etablissementCIOrganisationId);
        }

        /// <summary>
        /// Obtient la collection des types de ressources
        /// </summary>
        /// <returns>La collection des types de ressource</returns>
        public IEnumerable<CarburantEnt> GetCarburantList()
        {
            var result = this.ressourceRepo.GetCarburantList();
            return result;
        }

        #region Chapitre

        /// <summary>
        /// Retourne le chapitre avec l'identifiant unique indiqué.
        /// </summary>
        /// <param name="chapitreId">Identifiant du chapitre à retrouver.</param>
        /// <returns>Le chapitre retrouvé, sinon null.</returns> 
        public ChapitreEnt GetChapitreById(int chapitreId)
        {
            return this.chapitreRepo.GetById(chapitreId);
        }

        /// <summary>
        /// Retourne la liste des chapitres.
        /// </summary>
        /// <returns>Liste des chapitres.</returns>        
        public IEnumerable<ChapitreEnt> GetChapitreList()
        {
            return this.chapitreRepo.GetList();
        }

        /// <summary>
        /// Obtient la collection des chapitres (suppprimés inclus)
        /// </summary>
        /// <returns>La collection des chapitres</returns>
        public IEnumerable<ChapitreEnt> GetAllChapitreList()
        {
            return this.chapitreRepo.GetAllList();
        }

        /// <summary>
        /// Obtient la collection des chapitres appartenant à un groupe spécifié
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe.</param>
        /// <returns>La collection des chapitres</returns>
        public IEnumerable<ChapitreEnt> GetChapitreListByGroupeId(int groupeId)
        {
            return this.chapitreRepo.GetChapitreListByGroupeId(groupeId);
        }

        /// <summary>
        /// Obtient la liste des chapitres en fonction du groupeId de l'utilisateur connecté
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur connecté</param>
        /// <returns>Retourne la liste des chapitres</returns>
        public IEnumerable<ChapitreEnt> GetChapitreListByUtilisateurId(int userId)
        {
            int userGroupId = this.userManager.GetContextUtilisateur().Personnel.Societe.GroupeId;
            return (userGroupId != 0) ? GetChapitreListByGroupeId(userGroupId) : null;
        }

        /// <summary>
        /// Ajoute un nouveau chapitre
        /// </summary>
        /// <param name="chapitreEnt">Rôle à ajouter</param>
        /// <returns> L'identifiant du chapitre ajouté</returns>
        public ChapitreEnt AddChapitre(ChapitreEnt chapitreEnt)
        {
            var currentUser = this.userManager.GetContextUtilisateur();

            if (!IsCodeChapitreExist(chapitreEnt.Code, currentUser.Personnel.Societe.GroupeId))
            {
                chapitreEnt.GroupeId = currentUser.Personnel.Societe.GroupeId;
                chapitreEnt.Code = chapitreEnt.Code.ToUpper();
                chapitreEnt.AuteurCreationId = currentUser.UtilisateurId;
                chapitreEnt.DateCreation = DateTime.UtcNow;

                this.chapitreRepo.Insert(chapitreEnt);
                this.uow.Save();
                return chapitreEnt;
            }
            else
            {
                throw new FredBusinessException(string.Format(BusinessResources.CodeDejaExistant, BusinessResources.Chapitre));
            }
        }

        /// <summary>
        /// Supprime un ChapitreModule
        /// </summary>
        /// <param name="chapitreId">ID du chapitre à dissocier du module</param>
        public void DeleteChapitreById(int chapitreId)
        {
            var chapter = GetChapitreById(chapitreId);
            if (this.chapitreRepo.IsDeletable(chapter))
            {
                this.chapitreRepo.Delete(chapter);
                this.uow.Save();
            }
            else
            {
                throw new FredBusinessException(ReferentielFixeResources.Chapitre_SuppressionImpossible);
            }
        }

        /// <summary>
        /// Met à rout un chapitre
        /// </summary>
        /// <param name="chapitreEnt">Rôle à mettre à jour</param>
        /// <returns>Chapitre mis à jour</returns>
        public ChapitreEnt UpdateChapitre(ChapitreEnt chapitreEnt)
        {
            chapitreEnt.AuteurModificationId = this.userManager.GetContextUtilisateurId();
            chapitreEnt.DateModification = DateTime.UtcNow;
            this.chapitreRepo.Update(chapitreEnt);
            this.uow.Save();

            return chapitreEnt;
        }

        /// <summary>
        /// Indique si le code existe déjà pour les chapitres d'un groupe
        /// </summary>
        /// <param name="code">Chaine de caractère du code.</param>
        /// <param name="groupeId">Identifiant du groupe.</param>
        /// <returns>Vrai si le code existe, faux sinon</returns>
        public bool IsCodeChapitreExist(string code, int groupeId)
        {
            return this.chapitreRepo.IsCodeChapitreExist(code, groupeId);
        }

        /// <summary>
        /// Cherche une liste de Chapitre.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des Chapitres.</param>
        /// <returns>Une liste de Chapitre.</returns>
        public IEnumerable<ChapitreEnt> SearchChapitres(string text)
        {
            return string.IsNullOrEmpty(text) ? GetChapitreList() : this.chapitreRepo.SearchChapitres(text);
        }

        /// <summary>
        /// Cherche une liste de Chapitre.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des Chapitres.</param>
        /// <param name="groupeId">Identifiant du groupe auquel appartiennent les Chapitres.</param>
        /// <returns>Une liste de Chapitre.</returns>
        public IEnumerable<ChapitreEnt> SearchChapitres(string text, int groupeId)
        {
            if (string.IsNullOrEmpty(text))
            {
                return GetChapitreListByGroupeId(groupeId);
            }

            return this.chapitreRepo.SearchChapitres(text, groupeId);
        }

        /// <summary>
        /// Récupération des chapitres des listes des moyen
        /// </summary>
        /// <returns>Liste des  chapitres qui conçernent les moyens de FES</returns>
        public IEnumerable<int> GetFesChapitreListMoyen()
        {
            return this.chapitreRepo.GetFesChapitreListMoyen();
        }

        #endregion

        #region Sous-Chapitre

        /// <summary>
        /// Retourne le sousChapitre avec l'identifiant unique indiqué.
        /// </summary>
        /// <param name="sousChapitreID">Identifiant du sousChapitre à retrouver.</param>
        /// <returns>Le sousChapitre retrouvé, sinon null.</returns>
        public SousChapitreEnt GetSousChapitreById(int sousChapitreID)
        {
            return this.sousChapitreRepo.GetById(sousChapitreID);
        }

        /// <summary>
        /// Retourne la liste des sousChapitres.
        /// </summary>
        /// <returns>Liste des sousChapitres.</returns>
        public IEnumerable<SousChapitreEnt> GetSousChapitreList()
        {
            return this.sousChapitreRepo.GetList();
        }

        /// <summary>
        /// Obtient la collection des sous-chapitres (suppprimés inclus)
        /// </summary>
        /// <returns>La collection des sous-chapitres</returns>
        public IEnumerable<SousChapitreEnt> GetAllSousChapitreList()
        {
            return this.sousChapitreRepo.GetAllList();
        }

        /// <summary>
        /// Obtient la collection des sous-chapitres appartenant à un chapitre spécifié
        /// </summary>
        /// <param name="chapitreId">Identifiant du chapitre.</param>
        /// <returns>La collection des sous-chapitres</returns>
        public IEnumerable<SousChapitreEnt> GetSousChapitreListByChapitreId(int chapitreId)
        {
            return this.sousChapitreRepo.GetListByChapitreId(chapitreId);
        }

        /// <summary>
        /// Ajoute un nouveau sousChapitre
        /// </summary>
        /// <param name="sousChapitreEnt">Rôle à ajouter</param>
        /// <returns> L'identifiant du sousChapitre ajouté</returns>
        public SousChapitreEnt AddSousChapitre(SousChapitreEnt sousChapitreEnt)
        {
            int groupId = this.chapitreRepo.GetById(sousChapitreEnt.ChapitreId).GroupeId;
            if (!IsCodeSousChapitreExist(sousChapitreEnt.Code, groupId))
            {
                sousChapitreEnt.Code = sousChapitreEnt.Code.ToUpper();
                sousChapitreEnt.AuteurCreationId = this.userManager.GetContextUtilisateurId();
                sousChapitreEnt.DateCreation = DateTime.UtcNow;
                this.sousChapitreRepo.Insert(sousChapitreEnt);
                this.uow.Save();
                return sousChapitreEnt;
            }
            else
            {
                throw new FredBusinessException(string.Format(BusinessResources.CodeDejaExistant, BusinessResources.SousChapitre));
            }
        }

        /// <summary>
        /// Supprime un SousChapitreModule
        /// </summary>
        /// <param name="sousChapitreId">ID du sousChapitre à dissocier du module</param>
        public void DeleteSousChapitreById(int sousChapitreId)
        {
            var sousChapitre = GetSousChapitreById(sousChapitreId);

            if (this.sousChapitreRepo.IsDeletable(sousChapitre))
            {
                this.sousChapitreRepo.Delete(sousChapitre);
                this.uow.Save();
            }
            else
            {
                throw new FredBusinessException(ReferentielFixeResources.SousChapitre_SuppressionImpossible);
            }
        }

        /// <summary>
        /// Met à rout un sousChapitre
        /// </summary>
        /// <param name="sousChapitreEnt">Rôle à mettre à jour</param>
        /// <returns>Sous chapitre mis à jour</returns>
        public SousChapitreEnt UpdateSousChapitre(SousChapitreEnt sousChapitreEnt)
        {
            sousChapitreEnt.AuteurModificationId = this.userManager.GetContextUtilisateurId();
            sousChapitreEnt.DateModification = DateTime.UtcNow;
            this.sousChapitreRepo.Update(sousChapitreEnt);
            this.uow.Save();

            return sousChapitreEnt;
        }

        /// <summary>
        /// Indique si le code existe déjà pour les sous-chapitres d'un groupe
        /// </summary>
        /// <param name="code">Chaine de caractère du code.</param>
        /// <param name="groupeId">Identifiant du groupe.</param>
        /// <returns>Vrai si le code existe, faux sinon</returns>
        public bool IsCodeSousChapitreExist(string code, int groupeId)
        {
            return this.sousChapitreRepo.IsCodeSousChapitreExist(code, groupeId);
        }

        /// <summary>
        /// Cherche une liste de SousChapitre.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des SousChapitres.</param>
        /// <returns>Une liste de SousChapitre.</returns>
        public IEnumerable<SousChapitreEnt> SearchSousChapitres(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return GetSousChapitreList();
            }

            return this.sousChapitreRepo.SearchSousChapitres(text);
        }

        /// <summary>
        /// Cherche une liste de SousChapitre.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des SousChapitres.</param>
        /// <param name="groupeId">Identifiant du groupe auquel appartiennent les SousChapitres.</param>
        /// <returns>Une liste de SousChapitre.</returns>
        public IEnumerable<SousChapitreEnt> SearchSousChapitres(string text, int groupeId)
        {
            if (string.IsNullOrEmpty(text))
            {
                return this.sousChapitreRepo.SearchSousChapitres(groupeId);
            }

            return this.sousChapitreRepo.SearchSousChapitres(text, groupeId);
        }

        #endregion

        #region Ressource

        /// <summary>
        /// Retourne le ressource avec l'identifiant unique indiqué.
        /// </summary>
        /// <param name="ressourceID">Identifiant du ressource à retrouver.</param>
        /// <returns>Le ressource retrouvé, sinon null.</returns>
        public RessourceEnt GetRessourceById(int ressourceID)
        {
            return this.ressourceRepo.GetById(ressourceID);
        }

        /// <summary>
        /// Retourne le ressource avec l'identifiant unique indiqué.
        /// </summary>
        /// <param name="ressourceID">Identifiant du ressource à retrouver.</param>
        /// <returns>Le ressource retrouvé, sinon null.</returns>
        public RessourceEnt FindById(int ressourceID)
        {
            return this.ressourceRepo.FindById(ressourceID);
        }

        /// <summary>
        /// Retourne le ressource en fonction de son code
        /// </summary>
        /// <param name="code">Code de la ressource à retrouver.</param>
        /// <returns>Le ressource retrouvé, sinon null.</returns>
        public RessourceEnt GetRessource(string code)
        {
            if (code == null)
            {
                return null;
            }

            return this.ressourceRepo.GetAllList().FirstOrDefault(x => x.Code.Equals(code, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Retourne les ressources en fonction d'un code et d'un groupe.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="groupId">Identifiant du groupe.</param>
        /// <returns>Les ressources concernées.</returns>
        public IEnumerable<RessourceEnt> GetRessources(string code, int groupId)
        {
            return ressourceRepo.Get(code, groupId);
        }

        /// <summary>
        /// Retourne la liste des ressources.
        /// </summary>
        /// <returns>Liste des ressources.</returns>
        public IEnumerable<RessourceEnt> GetRessourceList()
        {
            return this.ressourceRepo.GetList();
        }

        /// <summary>
        /// Recupére une liste de RessourceEnt pour une liste de code ressource
        /// </summary>
        /// <param name="ressourceCodes">Liste de code ressource</param>
        /// <returns>Une liste en lecture seul de <see cref="RessourceEnt" /></returns>
        public IReadOnlyList<RessourceEnt> GetRessourceList(List<string> ressourceCodes)
        {
            return this.ressourceRepo.GetList(ressourceCodes);
        }

        /// <summary>
        /// Recupére une liste de RessourceEnt pour une liste de code ressource
        /// </summary>
        /// <param name="ressourceCodes">Liste de code ressource</param>
        /// <returns>Une liste de <see cref="RessourceEnt" /></returns>
        public IEnumerable<RessourceEnt> GetRessourceList(List<string> ressourceCodes, List<int> societeIds)
        {
            return ressourceRepo.GetList(ressourceCodes, societeIds);
        }

        /// <summary>
        /// Renvoi la liste des ressources
        /// </summary>
        /// <param name="groupId">Identifiant du groupe</param>
        /// <returns>retourne la liste des ressources appartenant à un groupe</returns>
        public IEnumerable<RessourceEnt> GetRessourceListByGroupeId(int groupId)
        {
            return this.ressourceRepo.GetListByGroupeId(groupId);
        }

        /// <summary>
        /// Obtient la collection des ressources (suppprimées inclus)
        /// </summary>
        /// <returns>La collection des ressources</returns>
        public IEnumerable<RessourceEnt> GetAllRessourceList()
        {
            return this.ressourceRepo.GetAllList();
        }

        /// <summary>
        /// Obtient la collection des ressources appartenant à un sous-chapitre spécifié
        /// </summary>
        /// <param name="sousChapitreId">Identifiant du sous-chapitre.</param>
        /// <returns>La collection des ressources</returns>
        public IEnumerable<RessourceEnt> GetRessourceListBySousChapitreId(int sousChapitreId)
        {
            return this.ressourceRepo.GetListBySousChapitreId(sousChapitreId);
        }

        /// <summary>
        /// Retourne le type de ressource en fonction de son code
        /// </summary>
        /// <param name="code">Code de la ressource</param>
        /// <returns>Le type de ressource</returns>
        public TypeRessourceEnt GetTypeRessourceByCode(string code)
        {
            return this.ressourceRepo.GetTypeRessourceByCode(code);
        }

        /// <summary>
        /// Ajoute une nouvelle ressource
        /// </summary>
        /// <param name="ressourceEnt">Rôle à ajouter</param>
        /// <returns> L'identifiant du ressource ajouté</returns>
        public RessourceEnt AddRessource(RessourceEnt ressourceEnt)
        {
            int groupId = this.chapitreRepo.GetById(this.sousChapitreRepo.GetById(ressourceEnt.SousChapitreId).ChapitreId).GroupeId;

            if (!IsCodeRessourceExist(ressourceEnt.Code, groupId))
            {
                ressourceEnt.Code = ressourceEnt.Code.ToUpper();
                ressourceEnt.AuteurCreationId = this.userManager.GetContextUtilisateurId();
                ressourceEnt.DateCreation = DateTime.UtcNow;
                this.ressourceRepo.AddRessource(ressourceEnt);
                uow.Save();

                return ressourceEnt;
            }
            else
            {
                throw new FredBusinessException(string.Format(BusinessResources.CodeDejaExistant, BusinessResources.Ressource));
            }
        }

        /// <summary>
        /// Supprime un RessourceModule
        /// </summary>
        /// <param name="ressourceId">ID du ressource à dissocier du module</param>
        public void DeleteRessourceById(int ressourceId)
        {
            var resource = GetRessourceById(ressourceId);

            if (this.ressourceRepo.IsDeletable(resource))
            {
                this.ressourceRepo.Delete(resource);
                this.uow.Save();
            }
            else
            {
                throw new FredBusinessException(ReferentielFixeResources.Ressource_SuppressionImpossible);
            }
        }

        /// <summary>
        /// Met à rout un ressource
        /// </summary>
        /// <param name="ressourceEnt">Rôle à mettre à jour</param>
        /// <returns>La ressource mise à jour</returns>
        public RessourceEnt UpdateRessource(RessourceEnt ressourceEnt)
        {
            ressourceEnt.AuteurModificationId = this.userManager.GetContextUtilisateurId();
            ressourceEnt.DateModification = DateTime.UtcNow;
            this.ressourceRepo.UpdateRessource(ressourceEnt);
            uow.Save();

            return ressourceEnt;
        }

        /// <summary>
        /// Indique si le code existe déjà pour les ressources d'un groupe
        /// </summary>
        /// <param name="code">Chaine de caractère du code.</param>
        /// <param name="groupeId">Identifiant du groupe.</param>
        /// <returns>Vrai si le code existe, faux sinon</returns>
        public bool IsCodeRessourceExist(string code, int groupeId)
        {
            return this.ressourceRepo.IsCodeRessourceExist(code, groupeId);
        }

        /// <summary>
        /// Renvoi le dernier code incrémenté de un pour un sous-chapitre spécifique
        /// </summary>
        /// <param name="sousChapitre">Sous-chapitre dont on recherche le prochain code.</param>
        /// <returns>Le prochain code disponible</returns>
        public string GetNextRessourceCode(SousChapitreEnt sousChapitre)
        {
            int increment = 1, maxCode = 0;
            const string separator = "-";
            var resourceListTmp = this.ressourceRepo.Query().Filter(r => r.SousChapitreId == sousChapitre.SousChapitreId).Get().ToList();

            sousChapitre.Code += separator;
            if (resourceListTmp.Count > 0)
            {
                int index = 0, tmp = 0;
                string currentCode = string.Empty;

                foreach (var t in resourceListTmp)
                {
                    index = t.Code.IndexOf(sousChapitre.Code);
                    currentCode = index < 0 ? t.Code : t.Code.Remove(index, sousChapitre.Code.Length);
                    if (int.TryParse(currentCode, out tmp) && tmp > maxCode)
                    {
                        maxCode = tmp;
                    }
                }
            }

            maxCode += increment;
            return sousChapitre.Code + maxCode.ToString().PadLeft(3, '0');
        }

        /// <summary>
        /// Cherche une liste de Ressource.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des Ressources.</param>
        /// <returns>Une liste de Ressource.</returns>
        public IEnumerable<RessourceEnt> SearchRessources(string text)
        {
            return string.IsNullOrEmpty(text) ? GetRessourceList() : this.ressourceRepo.SearchRessources(text);
        }

        /// <summary>
        /// Vérifie que le code de la ressource n'est pas déjà utilisé
        /// </summary>
        /// <param name="code">Code de la ressource à comparer</param>
        /// <returns>Vrai si le code de la ressource existe</returns>
        public bool IsRessourceExistByCode(string code)
        {
            return this.ressourceRepo.IsExistByCode(code);
        }

        /// <summary>
        /// GetRessourcesByChapitres
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="societeId">societeId</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="chapitreCodes">chapitreCodes</param>
        /// <returns> Liste de ReferentielEtenduEnt</returns>
        public IEnumerable<ReferentielEtenduEnt> GetRessourcesByChapitres(string text, int societeId, int page, int pageSize, List<string> chapitreCodes)
        {
            var query = refEtenduRepo
                            .Query()
                            .Include(r => r.Ressource)
                            .Include(r => r.Ressource.SousChapitre)
                            .Include(r => r.Ressource.SousChapitre.Chapitre)
                            .Filter(r => r.SocieteId == societeId && r.NatureId != null && r.Ressource.Active && r.Ressource.DateSuppression == null)
                            .Filter(r => chapitreCodes.Contains(r.Ressource.SousChapitre.Chapitre.Code))
                            .Filter(r => string.IsNullOrEmpty(text) || r.Ressource.Code.Contains(text) || r.Ressource.Libelle.Contains(text));

            return query.OrderBy(list => list.OrderBy(r => r.Ressource.Libelle)).GetPage(page, pageSize).ToList();
        }

        /// <summary>
        /// Retourne la liste des identifiants des ressources possédant la même nature que la ressource en entrée
        /// </summary>
        /// <param name="ressourceId">Identifiant de la ressource</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>La liste des identifiants des ressources possédant la même nature que la ressource source</returns>
        public List<int> GetListRessourceIdByRessourceWithSameNatureInRefEtendu(int ressourceId, int societeId)
        {
            List<int> listresources = new List<int>();
            var natureId = refEtenduRepo
                            .Query().Filter(r => r.RessourceId == ressourceId && r.SocieteId == societeId).Get().Select(r => r.NatureId).FirstOrDefault();
            if (natureId != null && natureId.HasValue)
            {
                listresources = refEtenduRepo
                               .Query()
                               .Filter(r => r.NatureId == natureId).Get().Select(r => r.RessourceId).ToList();
            }
            return listresources;
        }

        /// <summary>
        /// Méthode de recherche d'un type de ressource
        /// </summary>
        /// <param name="text">Le texte recherché</param>
        /// <returns>Une liste d' items de référentiel</returns> 
        public async Task<IEnumerable<TypeRessourceEnt>> SearchRessourceTypesByCodeOrLabelAsync(string text)
        {
            return await ressourceRepo.SearchRessourceTypesByCodeOrLabelAsync(text).ConfigureAwait(false);
        }

        #endregion

        #region private

        /// <summary>
        /// SearchLight en fonction du Groupe de la société
        /// </summary>
        /// <param name="filter">Filtre de la recherche</param>
        /// <returns>Liste des items récupérés</returns>
        private List<RessourceEnt> SearchLightReferentielFixe(SearchRessourcesAchatModel filter)
        {
            int groupeId = userManager.GetContextUtilisateur().Personnel.Societe.GroupeId;

            return ressourceRepo.Query()
                                .Include(x => x.TypeRessource)
                                .Filter(x => x.SousChapitre.Chapitre.GroupeId == groupeId)
                                .Filter(x => x.Active && !x.DateSuppression.HasValue)
                                .Filter(r => string.IsNullOrEmpty(filter.Text) || r.Code.Contains(filter.Text) || r.Libelle.Contains(filter.Text))
                                .Filter(r => !filter.RessourceTypeId.HasValue || r.TypeRessourceId == filter.RessourceTypeId)
                                .Filter(r => !r.IsRessourceSpecifiqueCi)
                                .Filter(r => !r.Libelle.ToLower().StartsWith("od ") && !r.Libelle.ToLower().StartsWith("litige ") && !r.Libelle.ToLower().StartsWith("litiges "))
                                .OrderBy(list => list.OrderBy(r => r.Libelle))
                                .GetPage(filter.Page, filter.PageSize)
                                .ToList();
        }

        /// <summary>
        /// SearchLight en fonction du référentiel étendu (FRED_SOCIETE_RESSOURCE_NATURE)
        /// </summary>
        /// <param name="filter">Filtre de la recherche</param>
        /// <returns>Liste des items trouvés</returns>
        private List<RessourceEnt> SearchLightReferentielEtendu(SearchRessourcesAchatModel filter)
        {
            var query = refEtenduRepo
                            .Query()
                            .Include(r => r.Ressource)
                            .Include(r => r.Ressource.TypeRessource)
                            .Filter(r => r.SocieteId == filter.SocieteId && r.NatureId != null && r.Ressource.Active && r.Ressource.DateSuppression == null)
                            .Filter(r => string.IsNullOrEmpty(filter.Text) || r.Ressource.Code.Contains(filter.Text) || r.Ressource.Libelle.Contains(filter.Text))
                            .Filter(r => string.IsNullOrEmpty(filter.Recherche) || r.Ressource.Code.Contains(filter.Recherche) || r.Ressource.Libelle.Contains(filter.Recherche))
                            .Filter(r => !r.Ressource.Libelle.ToLower().StartsWith("od ") && !r.Ressource.Libelle.ToLower().StartsWith("litige ")
                                    && !r.Ressource.Libelle.ToLower().StartsWith("litiges "));

            if (filter.RessourceTypeId.HasValue)
            {
                query.Filter(r => r.Ressource.TypeRessourceId == filter.RessourceTypeId);
            }

            if (filter.RessourceId.HasValue)
            {
                var refEtendu = refEtenduRepo.Query().Get().FirstOrDefault(x => x.RessourceId == filter.RessourceId.Value && x.SocieteId == filter.SocieteId);
                query.Filter(r => r.NatureId == refEtendu.NatureId);
            }

            if (filter.AchatsEnable == true)
            {
                query.Filter(r => r.Achats);
            }

            if (!string.IsNullOrEmpty(filter.Recherche2))
            {
                string strFormatted = StringTool.RemoveDiacritics(filter.Recherche2.ToLower());

                query.Filter(r => !string.IsNullOrEmpty(r.Ressource.Keywords) && r.Ressource.Keywords.ToLower().Contains(strFormatted));
            }

            return query.OrderBy(list => list.OrderBy(r => r.Ressource.Libelle)).GetPage(filter.Page, filter.PageSize).Select(r => r.Ressource).ToList();
        }

        #endregion
    }
}
