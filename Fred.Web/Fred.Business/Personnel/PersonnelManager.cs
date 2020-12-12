using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Fred.Business.Affectation;
using Fred.Business.AffectationSeuilUtilisateur;
using Fred.Business.CI;
using Fred.Business.FeatureFlipping;
using Fred.Business.Organisation.Tree;
using Fred.Business.Parametre;
using Fred.Business.Personnel.Import;
using Fred.Business.Personnel.Interimaire;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Business.Valorisation;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Budget.Dao.Budget.ExcelExport;
using Fred.Entities.CI;
using Fred.Entities.Directory;
using Fred.Entities.Groupe;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Import;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.Framework.FeatureFlipping;
using Fred.Framework.Services.Google;
using Fred.Web.Models.Personnel;
using Microsoft.EntityFrameworkCore;
using NLog;
using static Fred.Entities.Constantes;

namespace Fred.Business.Personnel
{
    /// <summary>
    /// Gestionnaire du personnel
    /// </summary>
    public class PersonnelManager : Manager<PersonnelEnt, IPersonnelRepository>, IPersonnelManager
    {
        private readonly IGeocodeService geocodeService;
        private readonly IEtablissementPaieRepository etablissementPaieRepo;
        private readonly IMatriculeExterneManager matriculeExterneManager;
        private readonly IValorisationManager ValorisationManager;
        private readonly IUtilisateurManager UtilisateurManager;
        private readonly IAffectationManager AffectationManager;
        private readonly ISepService SepService;
        private readonly IFeatureFlippingManager FeatureFlippingManager;
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IParametreManager ParametreManager;
        private readonly ISocieteManager SocieteManager;
        private readonly ICIManager CIManager;
        private readonly IContratInterimaireRepository contratInterimaireRepository;
        private const string PREFIXEINTERIM = "I_";
        private UtilisateurEnt currentUser;
        private readonly IAffectationSeuilUtilisateurManager affectationManager;
        private readonly IEtablissementComptableManager etablissementComptableManager;

        public PersonnelManager(
            IUnitOfWork uow,
            IPersonnelRepository personnelRepository,
            IPersonnelValidator validator,
            IGeocodeService geocodeService,
            IEtablissementPaieRepository etablissementPaieRepo,
            IMatriculeExterneManager matriculeExterneManager,
            IOrganisationTreeService organisationTreeService,
            IParametreManager ParametreManager,
            ISocieteManager SocieteManager,
            ICIManager CIManager,
            IValorisationManager ValorisationManager,
            IUtilisateurManager UtilisateurManager,
            IAffectationManager AffectationManager,
            ISepService SepService,
            IFeatureFlippingManager FeatureFlippingManager,
            IContratInterimaireRepository contratInterimaireRepository,
            IEtablissementComptableManager etablissementComptableManager,
            IAffectationSeuilUtilisateurManager affectationManager)
                : base(uow, personnelRepository, validator)
        {
            this.geocodeService = geocodeService;
            this.etablissementPaieRepo = etablissementPaieRepo;
            this.matriculeExterneManager = matriculeExterneManager;
            this.organisationTreeService = organisationTreeService;
            this.ParametreManager = ParametreManager;
            this.SocieteManager = SocieteManager;
            this.CIManager = CIManager;
            this.ValorisationManager = ValorisationManager;
            this.UtilisateurManager = UtilisateurManager;
            this.AffectationManager = AffectationManager;
            this.SepService = SepService;
            this.FeatureFlippingManager = FeatureFlippingManager;
            this.contratInterimaireRepository = contratInterimaireRepository;
            this.affectationManager = affectationManager;
            this.etablissementComptableManager = etablissementComptableManager;
        }

        /// <summary>
        /// Utilisateur Courant
        /// </summary>
        private UtilisateurEnt CurrentUser
        {
            get
            {
                if (this.currentUser == null)
                {
                    this.currentUser = UtilisateurManager.GetContextUtilisateur();
                }
                return this.currentUser;
            }
        }

        /// <inheritdoc />
        public List<PersonnelEnt> Get(List<Expression<Func<PersonnelEnt, bool>>> filters,
                    Func<IQueryable<PersonnelEnt>, IOrderedQueryable<PersonnelEnt>> orderBy = null,
                    List<Expression<Func<PersonnelEnt, object>>> includeProperties = null,
                    int? page = null,
                    int? pageSize = null,
                    bool asNoTracking = true)
        {
            return Repository.Get(filters, orderBy, includeProperties, page, pageSize, asNoTracking);
        }

        /// <summary>
        /// Vérifie la validité et enregistre le personnel importé depuis ANAËL Paie
        /// </summary>
        /// <param name="personnels">Liste des entités dont il faut vérifier la validité</param>
        /// <param name="societeId">Société pour laquelle on vérifie le personnel</param>
        /// <returns>Liste des entités qui sont ajoutées en base</returns>
        public List<PersonnelEnt> ManageImportedPersonnels(IEnumerable<PersonnelEnt> personnels, int? societeId)
        {
            Logger logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                logger.Trace("Import des personnel depuis ANAEL " + DateTime.UtcNow.ToString());
                // Test de paramètres d'entrées
                if (societeId == null || !personnels.Any())
                {
                    return new List<PersonnelEnt>();
                }

                //Récupération des paramètres Google API
                GoogleApiParam param = ParametreManager.GetGoogleApiParams();

                if (param.DateCourante.Date != DateTime.UtcNow.Date)
                {
                    param.DateCourante = DateTime.UtcNow.Date;
                    param.IndexCourant = 0;
                }

                // Chargement de la liste de références des salariés pour la société d'import
                string codeSociete = SocieteManager.GetSocieteById(societeId.Value).CodeSocietePaye;
                IEnumerable<PersonnelEnt> listByCodeSocietePaye = Repository.GetPersonnelListByCodeSocietePaye(codeSociete).ToList();
                List<PersonnelEnt> personnelList = new List<PersonnelEnt>();
                //Itération sur la liste de personnel reçu d'ANAEL afin de vérifier s'il faut les importer ou non
                foreach (PersonnelEnt persoInterneAnael in personnels)
                {
                    PersonnelImportHelper.TransformPersonnelAnaelBeforeImport(persoInterneAnael, societeId, etablissementPaieRepo);
                    // Test d'existence du salarié suivant le matricule et la société d'appartenance
                    PersonnelEnt persoInterneFred = listByCodeSocietePaye.FirstOrDefault(p => p.Matricule == persoInterneAnael.Matricule
                                                                                              && p.SocieteId == persoInterneAnael.SocieteId);
                    PersonnelEnt personnelToAddOrUpdate = PersonnelImportHelper.HandlePersonnel(persoInterneAnael, persoInterneFred, param, geocodeService);

                    if (personnelToAddOrUpdate != null)
                    {
                        personnelList.Add(personnelToAddOrUpdate);
                    }
                }

                if (personnelList.Count > 0)
                {
                    Repository.AddOrUpdatePersonnelList(personnelList);
                    // Pas très optimale mais pour le peu d'utilisateur, il n'y a pour l'instant pas de pb (à optimiser avec une transaction :) )                                    
                    personnelList.ForEach(UpdateUtilisateur);
                }

                //MAJ des paramètres Google API
                ParametreManager.UpdateGoogleApiParams(param);
                return personnelList;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                throw;
            }

            void UpdateUtilisateur(PersonnelEnt personnel)
            {
                if (!UtilisateurManager.IsFredUser(personnel.PersonnelId))
                    return;

                DateTime? exitDate = personnel.DateSortie;
                if (!exitDate.HasValue || exitDate.Value > DateTime.Now)
                    return;

                UtilisateurEnt user = UtilisateurManager.GetById(personnel.PersonnelId);
                user.IsDeleted = true;

                UtilisateurManager.UpdateUtilisateur(user);
            }
        }

        /// <summary>
        /// Creation ou mise a jour des affectation suite a un import du personnel
        /// </summary>
        /// <param name="newOrUpdatedAffectations">Liste d'affectation a mettre a jour ou a créer</param>
        public void ImportPersonnelsAffectations(List<PersonnelAffectationResult> newOrUpdatedAffectations)
        {
            Repository.ImportPersonnelsAffectations(newOrUpdatedAffectations);
        }

        /// <inheritdoc />
        public PersonnelEnt GetNewPersonnel()
        {
            var personnel = new PersonnelEnt
            {
                DateEntree = DateTime.Today,
                DateSortie = null
            };

            return personnel;
        }

        /// <inheritdoc />
        public UtilisateurEnt GetNewUtilisateur()
        {
            return new UtilisateurEnt
            {
                IsDeleted = true,
                CommandeManuelleAllowed = false,
                ExternalDirectory = new ExternalDirectoryEnt
                {
                    IsActived = true,
                    DateExpiration = DateTime.Today.AddYears(1)
                }
            };
        }

        /// <inheritdoc />
        public SearchPersonnelEnt GetNewFilter()
        {
            return new SearchPersonnelEnt
            {
                ValueText = string.Empty,
                ValueTextNom = string.Empty,
                ValueTextPrenom = string.Empty,
                SearchEtab = null,
                SearchSociete = null,
                Nom = true,
                Prenom = true,
                Matricule = true,
                SocieteCodeLibelle = true,
                NomPrenomAsc = true,
                SocieteAsc = true,
                MatriculeAsc = true,
                IsActif = true,
                IsInterne = false,
                IsInterimaire = false,
                IsUtilisateur = false,
                SocieteCode = string.Empty,
                EtablissementPaieCode = string.Empty
            };
        }

        /// <inheritdoc />
        public int GetCountPersonnel(SearchPersonnelEnt filter)
        {
            return Repository.Query().Filter(c => !c.DateSuppression.HasValue).Filter(filter.GetPredicateWhere()).Get().Count();
        }

        /// <inheritdoc />
        public PersonnelEnt Add(PersonnelEnt personnel, int? userId = null)
        {
            if (personnel.IsInterimaire)
            {
                personnel.Matricule = GetNextMatriculeInterimaire();
            }

            if (userId.HasValue)
            {
                personnel.UtilisateurIdCreation = userId.Value;
            }
            else
            {
                personnel.UtilisateurIdCreation = this.CurrentUser.UtilisateurId;
            }

            personnel.Utilisateur = null;
            personnel.DateCreation = DateTime.UtcNow;
            BusinessValidation(personnel);
            UpdatePersonnelCategorie(personnel);
            MatriculeExterneEnt matriculeToAdd = null;
            if (personnel.MatriculeExterne != null && personnel.MatriculeExterne.Any())
            {
                matriculeToAdd = personnel.MatriculeExterne.FirstOrDefault();
                personnel.MatriculeExterne = null;
            }

            personnel.Materiel = null;
            this.Repository.AddPersonnel(personnel);
            Save();

            var personnelEnt = personnel;

            if (matriculeToAdd != null)
            {
                matriculeToAdd.PersonnelId = personnelEnt.PersonnelId;
                this.matriculeExterneManager.AddMatriculeExterne(matriculeToAdd);
            }
            return personnelEnt;
        }

        /// <inheritdoc />
        public PersonnelEnt Update(PersonnelEnt personnel, int? userId = null)
        {
            var p = GetPersonnel(personnel.PersonnelId);
            if (userId.HasValue)
            {
                personnel.UtilisateurIdModification = userId.Value;
            }
            else
            {
                personnel.UtilisateurIdModification = this.CurrentUser.UtilisateurId;
            }

            personnel.DateModification = DateTime.UtcNow;
            if (string.IsNullOrEmpty(personnel.Email))
            {
                personnel.Email = null;
            }
            if (personnel.EtablissementPaieId != p.EtablissementPaieId)
            {
                personnel.IsPersonnelNonPointable = personnel.EtablissementPaie != null ? personnel.EtablissementPaie.IsPersonnelsNonPointables : false;
            }
            BusinessValidation(personnel);
            UpdatePersonnelCategorie(personnel);
            Repository.UpdatePersonnel(personnel);
            Save();

            var perso = personnel;

            if (p?.RessourceId != perso.RessourceId)
            {
                ValorisationManager.NewValorisationJob(personnel.PersonnelId, personnel.UtilisateurIdModification.Value, ValorisationManager.UpdateValorisationFromPersonnel);
            }

            if (personnel.MatriculeExterne != null)
            {
                foreach (var matriculeExterne in personnel.MatriculeExterne)
                {
                    if (matriculeExterne.MatriculeExterneId > 0)
                    {
                        this.matriculeExterneManager.UpdateMatriculeExterne(matriculeExterne);
                    }
                    else
                    {
                        matriculeExterne.PersonnelId = perso.PersonnelId;
                        this.matriculeExterneManager.AddMatriculeExterne(matriculeExterne);
                    }
                }
            }
            return perso;
        }

        /// <inheritdoc />
        public IEnumerable<PersonnelEnt> GetPersonnelListByCodeSocietePaye(string codeSociete)
        {
            return Repository.GetPersonnelListByCodeSocietePaye(codeSociete);
        }

        public void UpdateDateEntreePersonnelInterimaire(int interimaireId, DateTime? interimaireDateEntree, int? userId = null)
        {
            PersonnelEnt personnelInterimaire = GetPersonnel(interimaireId);
            personnelInterimaire.UtilisateurIdModification = userId ?? this.CurrentUser.UtilisateurId;
            personnelInterimaire.DateModification = DateTime.UtcNow;
            personnelInterimaire.DateEntree = interimaireDateEntree;

            Repository.UpdatePersonnel(personnelInterimaire);
            Save();
        }

        /// <inheritdoc />
        public PersonnelEnt GetPersonnel(int societeId, string matricule)
        {
            return Repository.GetPersonnel(societeId, matricule);
        }

        /// <inheritdoc />
        public PersonnelEnt GetPersonnelByNomPrenom(string nom, string prenom)
        {
            return Repository.GetPersonnelByNomPrenom(nom, prenom, null);
        }

        /// <inheritdoc />
        public PersonnelEnt GetPersonnelById(int? personnelId)
        {
            return Repository.GetPersonnelById(personnelId);
        }

        /// <inheritdoc />
        public PersonnelEnt GetPersonnelByNomPrenom(string nom, string prenom, int groupeId)
        {
            return Repository.GetPersonnelByNomPrenom(nom, prenom, groupeId);
        }

        /// <summary>
        /// Retourne le matériel du personnel
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <returns>Le matériel par défaut du personnel.</returns>
        public MaterielEnt GetMaterielDefault(int personnelId)
        {
            return Repository.GetMaterielDefault(personnelId);
        }

        /// <inheritdoc />
        public IEnumerable<PersonnelEnt> GetPersonnelList()
        {
            return Repository.GetPersonnelList();
        }

        /// <summary>
        /// Retourne la liste du personnel
        /// </summary>
        /// <returns>Liste du personnel.</returns>
        public IEnumerable<PersonnelEnt> GetOutgoingPersonnelsList(string suffixDisableLogin = "-old")
        {
            return Repository.GetOutgoingPersonnelsList(suffixDisableLogin);
        }

        /// <inheritdoc />
        public IEnumerable<PersonnelEnt> GetPersonnelListSync()
        {
            return Repository.GetPersonnelListSync();
        }

        /// <inheritdoc />
        public PersonnelEnt GetPersonnel(int personnelId)
        {
            return Repository.GetPersonnel(personnelId);
        }

        /// <inheritdoc />
        public TPersonnel GetPersonnel<TPersonnel>(int personnelId, Expression<Func<PersonnelEnt, TPersonnel>> selector)
        {
            return Repository.Get().Where(p => p.PersonnelId == personnelId).Select(selector).FirstOrDefault();
        }

        /// <inheritdoc />
        public PersonnelEnt GetSimplePersonnel(int personnelId)
        {
            return Repository.GetSimplePersonnel(personnelId);
        }

        /// <summary>
        /// Retourne le personnel en fonction de son email
        /// </summary>
        /// <param name="email">Email du personnel</param>
        /// <returns>Personnel</returns>
        public PersonnelEnt GetPersonnelByEmail(string email)
        {
            return Repository.GetPersonnelByEmail(email);
        }

        /// <inheritdoc />
        public PersonnelEnt AddPersonnelAsUtilisateur(PersonnelEnt personnel)
        {
            var utilisateur = personnel.Utilisateur;
            utilisateur.DateCreation = DateTime.UtcNow;
            utilisateur.UtilisateurIdCreation = this.CurrentUser.UtilisateurId;
            utilisateur.UtilisateurId = personnel.PersonnelId;
            var isInterne = personnel.IsInterne;
            if (isInterne)
            {
                utilisateur.ExternalDirectory = null;
            }
            UtilisateurManager.AddUtilisateur(utilisateur);
            return personnel;
        }

        /// <summary>
        /// Récupère l'affectation intérimaire active pour une date donnée
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="date">date</param>
        /// <returns>l'affectation intérimaire active pour une date donnée</returns>
        public ContratInterimaireEnt GetAffectationInterimaireActive(int personnelId, DateTime date)
        {
            return Repository.GetContratInterimaireActive(personnelId, date);
        }

        /// <inheritdoc />
        public IEnumerable<ContratInterimaireEnt> GetAffectationInterimaireList()
        {
            return Repository.GetContratInterimaireList();
        }

        /// <inheritdoc />
        public IEnumerable<ContratInterimaireEnt> GetAffectationInterimaireList(int personnelId)
        {
            return Repository.GetContratInterimaireList(personnelId);
        }

        /// <inheritdoc />
        public IEnumerable<ContratInterimaireEnt> GetAffectationInterimaireList(int personnelId, int page, int pageSize)
        {
            return Repository.GetContratInterimaireList(personnelId, page, pageSize);
        }

        /// <inheritdoc />
        public ContratInterimaireEnt AddAffectationInterimaire(ContratInterimaireEnt affectation)
        {
            Repository.AddContratInterimaire(affectation);
            Save();

            return affectation;
        }

        /// <inheritdoc />
        public ContratInterimaireEnt UpdateAffectationInterimaire(ContratInterimaireEnt affectation)
        {
            Repository.UpdateContratInterimaire(affectation);
            Save();

            return affectation;
        }

        /// <inheritdoc />
        public string GetNextMatriculeInterimaire()
        {
            List<PersonnelEnt> interimList = Repository.GetPersonnelList().Where(x => x.IsInterimaire).ToList();
            int maxCode = 0, tmp = 0;
            foreach (PersonnelEnt p in interimList)
            {
                string numMatricule = p.Matricule.Replace(PREFIXEINTERIM, string.Empty);
                if (int.TryParse(numMatricule, out tmp) && tmp > maxCode)
                {
                    maxCode = tmp;
                }
            }
            maxCode++;
            return PREFIXEINTERIM + maxCode.ToString();
        }

        /// <inheritdoc />
        public SearchPersonnelsWithFiltersResult SearchPersonnelsWithFilters(SearchPersonnelEnt filters, int page, int pageSize)
        {
            SearchPersonnelsWithFiltersResult result = new SearchPersonnelsWithFiltersResult();
            int? currentUserGroupeId = this.CurrentUser?.Personnel?.Societe?.GroupeId;
            int totalCount = 0;
            string codeGroue = this.CurrentUser?.Personnel?.Societe?.Groupe?.Code;
            bool splitIsActived = false;
            if (string.IsNullOrEmpty(codeGroue) || !codeGroue.Equals(Constantes.CodeGroupeFES))
            {
                splitIsActived = this.FeatureFlippingManager.IsActivated(EnumFeatureFlipping.PersonnelsNouveauxFiltres);
            }

            result.Personnels = Repository.Query()
                                .Include(ep => ep.EtablissementPaie)
                                .Include(er => er.EtablissementRattachement)
                                .Include(p => p.Pays)
                                .Include(s => s.Societe)
                                .Include(u => u.Utilisateur)
                                .Include(u => u.Utilisateur.ExternalDirectory)
                                .Include(r => r.Ressource)
                                .Filter(filters.GetPredicateWhere(splitIsActived))
                                .Filter(x => this.CurrentUser.SuperAdmin || currentUserGroupeId == x.Societe.GroupeId)
                                .OrderBy(filters.ApplyOrderBy).GetPage(page, pageSize, out totalCount)
                                .ToList();
            result.TotalCount = totalCount;
            IgnoreSelfReferencing(result.Personnels);
            return result;
        }

        /// <summary>
        /// Search Personnels With Filters Optimized
        /// </summary>
        /// <param name="filters">Filtre de recherche</param>
        /// <param name="page">Numero de page à récupéré</param>
        /// <param name="pageSize">Nombre d'éléments par page</param>
        /// <returns>Retourne la condition de recherche du personnel</returns>
        public SearchPersonnelsWithFiltersResult SearchPersonnelsWithFiltersOptimized(SearchPersonnelEnt filters, int page, int pageSize)
        {
            SearchPersonnelsWithFiltersResult result = new SearchPersonnelsWithFiltersResult();
            int? currentUserGroupeId = this.CurrentUser?.Personnel?.Societe?.GroupeId;
            int totalCount = 0;
            string codeGroue = this.CurrentUser?.Personnel?.Societe?.Groupe?.Code;
            bool splitIsActived = false;
            if (string.IsNullOrEmpty(codeGroue) || !codeGroue.Equals(Constantes.CodeGroupeFES))
            {
                splitIsActived = this.FeatureFlippingManager.IsActivated(EnumFeatureFlipping.PersonnelsNouveauxFiltres);
            }

            result.PersonnelList = Repository.GetPersonnelsByFilterOptimzed(filters, page, pageSize, splitIsActived, this.currentUser.SuperAdmin, currentUserGroupeId, out totalCount).ToList();
            result.TotalCount = totalCount;
            return result;
        }

        private void IgnoreSelfReferencing(List<PersonnelEnt> result)
        {
            foreach (var p in result)
            {
                if (p.EtablissementPaie != null)
                {
                    p.EtablissementPaie.Societe = null;
                }
                if (p.EtablissementRattachement != null)
                {
                    p.EtablissementRattachement.Societe = null;
                }
                if (p.Utilisateur != null)
                {
                    p.Utilisateur.Personnel = null;
                }
                if (p.Pays != null)
                {
                    p.Pays.Personnels = null;
                }
                if (p.Ressource != null)
                {
                    p.Ressource.Personnels = null;
                }
                if (p.Societe != null)
                {
                    p.Societe.EtablissementComptables = null;
                    p.Societe.EtablissementPaies = null;
                    p.Societe.Personnels = null;
                }
            }
        }

        /// <summary>
        /// Retourne la liste des Interimaires actifs pour un ci donné
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>La liste des Interimaires actifs pour un ci donné</returns>
        public IRepositoryQuery<PersonnelEnt> GetInterimaireActifList(int ciId)
        {
            return Repository.GetInterimaireActifList(ciId);
        }

        /// <summary>
        /// Recherche de personnels dans le référentiel
        /// </summary>
        /// <param name="search">Model SearchLightPersonnel</param>        
        /// <returns>Une liste de personnel</returns>
        public async Task<IEnumerable<PersonnelEnt>> SearchLightAsync(SearchLightPersonnelModel search)
        {
            string codeGroupe = CurrentUser.Personnel.Societe.Groupe.Code;
            if (codeGroupe == CodeGroupeFES)
            {
                return await SearchLightFesAsync(search).ConfigureAwait(false);
            }

            return SearchLightRzb(search);
        }

        /// <summary>
        /// Recherche de personnels dans le référentiel
        /// </summary>
        /// <param name="search">Model SearchLightPersonnel</param>        
        /// <returns>Une liste de personnel</returns>
        public IEnumerable<PersonnelEnt> SearchLightForTeam(SearchLightPersonnelModel search)
        {
            int? currentUserGroupeId = CurrentUser?.Personnel?.Societe?.GroupeId;
            SetSearchLightPersonnelParams(search);

            return Repository.Query().Include(p => p.Societe)
                                          .Filter(x => this.CurrentUser.SuperAdmin || currentUserGroupeId == x.Societe.GroupeId)
                                          .Filter(p => !search.SocieteId.HasValue || p.SocieteId == search.SocieteId.Value)
                                          .Filter(search.GetSearchedTextPredicat())
                                          .Filter(search.GetWithInterimairePredicat())
                                          .Filter(search.GetWithoutInterimairePredicat())
                                          .Filter(search.GetStatutPredicat())
                                          .Filter(search.GetStatutPredicatFes())
                                          .Filter(search.GetOnlyUtilisateurPredicat())
                                          .Filter(search.GetActifOnlyPredicat())
                                          .Filter(search.GetFullPersonnel())
                                          .OrderBy(list => list.OrderBy(pe => pe.Societe.Code).ThenBy(pe => pe.Matricule))
                                          .GetPage(search.Page, search.PageSize);
        }

        /// <inheritdoc />
        public void DeleteById(int personnelId)
        {
            PersonnelEnt p = GetPersonnel(personnelId);
            if (p != null)
            {
                if (p.Utilisateur != null)
                {
                    this.UtilisateurManager.DeleteUtilisateurById(p.Utilisateur.UtilisateurId);
                    p.Utilisateur = null;
                }

                p.UtilisateurIdSuppression = this.CurrentUser.UtilisateurId;
                p.DateSuppression = DateTime.UtcNow;
                Update(p);
                Save();
            }
            else
            {
                throw new FredBusinessException("Impossible de supprimer ce personnel : Personnel introuvable.");
            }
        }

        /// <summary>
        /// Permet de récupérer une liste de délégués potentiels
        /// </summary>
        /// <param name="delegantId">Identifiant du délégant</param>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <param name="recherche">Recherche</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>LA liste de délégués potentiels</returns>
        public IEnumerable<PersonnelEnt> GetDelegue(int delegantId, int societeId, string recherche, int page, int pageSize)
        {
            try
            {
                return Repository.GetDelegue(delegantId, societeId, recherche, page, pageSize);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de récupérer une liste de délégués potentiels
        /// </summary>
        /// <param name="delegantId">Identifiant du délégant</param>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <param name="recherche">Recherche</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="recherche2">Prénom recherché</param>
        /// <param name="recherche3">Autres infos recherchées</param>
        /// <returns>LA liste de délégués potentiels</returns>
        public IEnumerable<PersonnelEnt> GetDelegue(int delegantId, int societeId, string recherche, int page, int pageSize, string recherche2, string recherche3)
        {
            try
            {
                return Repository.GetDelegue(delegantId, societeId, recherche, page, pageSize, recherche2, recherche3);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Récupérer les identifiants des personnels qui sont sous la responsabilité d'un manager
        /// </summary>
        /// <param name="managerId">L'identifiant du manager</param>
        /// <returns>La liste des identifiants</returns>
        public List<int> GetManagedPersonnelIds(int managerId)
        {
            if (FeatureFlippingManager.IsActivated(EnumFeatureFlipping.ActivateDesactivateFiltrePointageSyntheseMensuelle))
            {
                return Repository.GetManagedPersonnelIds(managerId);
            }

            return Repository.GetManagedEtamsAndIacs(managerId);
        }

        /// <summary>
        /// Récupérer les identifiants des personnels qui sont sous la responsabilité d'un manager
        /// </summary>
        /// <param name="managerId">L'identifiant du manager</param>
        /// <returns>La liste des identifiants</returns>
        public List<int> GetManagedEmployeeIdList(int managerId)
        {
            return Repository.GetManagedEmployeeIdList(managerId);
        }

        /// <summary>
        /// Get groupe du personnel by personnel identifier
        /// </summary>
        /// <param name="personnelId">Personnel identifier</param>
        /// <returns>Groupe entité</returns>
        public GroupeEnt GetPersonnelGroupebyId(int personnelId)
        {
            return Repository.GetPersonnelGroupebyId(personnelId);
        }

        /// <summary>
        /// Get personnel statut by statut id
        /// </summary>
        /// <param name="statutId">Statut identifier</param>
        /// <returns>Statut</returns>
        public static string GetStatut(string statutId)
        {
            switch (statutId)
            {
                case "1":
                    return PersonnelStatutValue.Ouvrier;
                case "2":
                    return PersonnelStatutValue.ETAM;
                case "3":
                    return PersonnelStatutValue.Cadre;
                case "4":
                    return PersonnelStatutValue.ETAM;
                case "5":
                    return PersonnelStatutValue.ETAM;
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Retourne un model DateEntreeSortie
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <returns>un model DateEntreeSortie</returns>
        public DateEntreeSortieModel GetDateEntreeSortie(int personnelId)
        {
            var personnel = Repository.FindById(personnelId);
            if (personnel != null)
            {
                return new DateEntreeSortieModel
                {
                    DateEntree = personnel.DateEntree,
                    DateSortie = personnel.DateSortie
                };
            }
            return null;
        }

        /// <summary>
        /// Retourne une liste du personnel dont l'id est contenu dans la liste passé ene parametre
        /// </summary>
        /// <param name="personnelIds">Liste des personnels a selectionner</param>
        /// <returns>Liste du personnel.</returns>
        public List<PersonnelEnt> GetPersonnelsByIds(List<int> personnelIds)
        {
            return Repository.Query().Filter(x => personnelIds.Contains(x.PersonnelId)).Get().AsNoTracking().ToList();
        }

        /// <summary>
        /// Retourne un personnel sans le mettre dans le context(AsNoTracking)
        /// Utile pour faire une comparaison des valeurs de champs.
        /// </summary>
        /// <param name="personnelId">ciId</param>
        /// <returns>Un ci détaché du contexte</returns>
        public PersonnelEnt GetPersonnelForCompare(int personnelId)
        {
            return Repository.Query().Filter(x => x.PersonnelId == personnelId).Get().AsNoTracking().FirstOrDefault();
        }

        /// <summary>
        /// Retourne la liste des personnels du niveau hierarchique inférieur
        /// </summary>
        /// <param name="search">Objet de recherche SearchLightPersonnel</param>
        /// <returns>La liste des personnels du niveau hierarchique inférieur</returns>
        public IEnumerable<PersonnelEnt> SearchNmoins1(SearchLightPersonnelModel search)
        {
            int? currentUserGroupeId = CurrentUser?.Personnel?.Societe?.GroupeId;
            return Repository.Query().Filter(x => this.CurrentUser.SuperAdmin || currentUserGroupeId == x.Societe.GroupeId)
                                          .Filter(search.GetSearchedTextPredicat()).Filter(search.GetHierarchieNmoins1())
                                          .OrderBy(list => list.OrderBy(pe => pe.Societe.Code).ThenBy(pe => pe.Matricule))
                                          .GetPage(search.Page, search.PageSize);
        }

        /// <summary>
        /// Retourne la liste des auteurs de rapport
        /// </summary>
        /// <param name="search">Modèle de recherche personnel</param>
        /// <param name="groupeId">Groupe de l'utilisateur courant</param>
        /// <param name="listUserId">Liste des users, auteur de rapport</param>
        /// <returns>La liste des auteurs de rapport</returns>
        public IEnumerable<PersonnelEnt> SearchRapportAuthor(SearchLightPersonnelModel search, int? groupeId, IEnumerable<int> listUserId)
        {
            return Repository.Query().Include(p => p.Societe).Include(p => p.Ressource)
                                    .Filter(p => listUserId.Contains(p.PersonnelId))
                                    .Filter(p => !groupeId.HasValue || p.Societe.GroupeId == groupeId)
                                    .Filter(search.GetImprovedSearchedTextPredicat())
                                    .OrderBy(list => list.OrderBy(pe => pe.Societe.Code).ThenBy(pe => pe.Matricule))
                                    .GetPage(search.Page, search.PageSize);
        }

        /// <summary>
        /// Retourne une liste de personnel Fes dernièrement créé ou mise à jour pour l'export vers tibco
        /// </summary>
        /// <param name="byPassDate">booléan qui indique si l'on se base sur la dernière date d'execution ou non</param>
        /// <param name="lastExecutionDate">DateTime dernière date d'execution du flux</param>
        /// <returns>Une liste de personnel fes</returns>
        public IEnumerable<PersonnelEnt> GetPersonnelFesForExportToTibco(bool byPassDate, DateTime? lastExecutionDate)
        {
            return Repository.GetPersonnelFesForExportToTibco(byPassDate, lastExecutionDate);
        }

        /// <summary>
        /// Retourne si true si le personnel est manager d'un autre personnel
        /// </summary>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <returns>true ou false</returns>
        public bool PersonnelIsManager(int personnelId)
        {
            return Repository.PersonnelIsManager(personnelId);
        }

        /// <summary>
        /// Update personnels pointable by etablisssemnt paie idenetifiant
        /// </summary>
        /// <param name="etablissementPaieId">Etablissement paie id</param>
        /// <param name="isNonPointable">is pointable</param>
        public void UpdatePersonnelsPointableByEtatPaieId(int etablissementPaieId, bool isNonPointable)
        {
            IEnumerable<PersonnelEnt> personnelsList = Repository.GetPersonnelsListByEtabPaieId(etablissementPaieId);
            foreach (PersonnelEnt personnel in personnelsList)
            {
                personnel.IsPersonnelNonPointable = isNonPointable;
                Repository.DetachDependancies(personnel);
                Repository.UpdatePersonnel(personnel);
            }

            Save();
        }

        /// <summary>
        /// Get le matricule, le nom et le prénom de l'utilisateur
        /// </summary>
        /// <param name="personnel">personnel</param>
        /// <returns>infos utilisateur</returns>
        public string GetPersonnelMatriculeNomPrenom(PersonnelDao personnel)
        {
            var data = new List<string>(3);
            if (!string.IsNullOrEmpty(personnel.Matricule))
            {
                data.Add(personnel.Matricule);
            }
            if (!string.IsNullOrEmpty(personnel.Nom))
            {
                data.Add(personnel.Nom);
            }
            if (!string.IsNullOrEmpty(personnel.Prenom))
            {
                data.Add(personnel.Prenom);
            }
            var sb = new StringBuilder();
            for (var i = 0; i < data.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(" - ");
                }
                sb.Append(data[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Recherche de personnels dans le référentiel+ les habilitations
        /// </summary>
        /// <param name="search">Model SearchLightPersonnel</param>        
        /// <returns>Une liste de personnel</returns>
        public async Task<List<PersonnelEnt>> SearchPersonnelEnteteCommandeAsync(SearchLightPersonnelModel search)
        {
            SetSearchLightPersonnelParams(search);
            List<int> listPerso = GetUserIdsFromTreeOraganisationByCiId(search.CiId.Value);
            int? currentUserSocieteId = CurrentUser?.Personnel?.Societe?.SocieteId;
            int? currentUserGroupeId = currentUser.Personnel?.Societe?.Groupe?.GroupeId;

            var predicateForPersonelWithAffectation = PredicateBuilder.True<PersonnelEnt>();
            predicateForPersonelWithAffectation = predicateForPersonelWithAffectation
                                                            .And(p => !p.IsPersonnelNonPointable)
                                                            .And(x => this.CurrentUser.SuperAdmin || currentUserGroupeId.Value == x.Societe.GroupeId || currentUserSocieteId.Value == x.Societe.SocieteId)
                                                            .And(p => !search.SocieteId.HasValue || p.SocieteId == search.SocieteId.Value)
                                                            .And(search.GetImprovedSearchedTextPredicat())
                                                            .And(search.GetWithInterimairePredicat())
                                                            .And(search.GetWithoutInterimairePredicat())
                                                            .And(search.GetGrzbPredicat())
                                                            .And(search.GetSepPredicat())
                                                            .And(search.GetOnlyUtilisateurPredicat())
                                                            .And(search.GetFullPersonnel());


            var predicatePersonnelWithHabilitation = PredicateBuilder.True<PersonnelEnt>();
            predicatePersonnelWithHabilitation = predicatePersonnelWithHabilitation
                                                            .And(p => listPerso.Contains(p.PersonnelId))
                                                            .And(search.GetImprovedSearchedTextPredicat());

            return await Repository.GetPersonnelEnteteCommandeAsync(predicateForPersonelWithAffectation, predicatePersonnelWithHabilitation, search.Page, search.PageSize);
        }

        /// <summary>
        /// Retourne une liste de personnel 
        /// </summary>
        /// <param name="ListPersonnelId">Id des personnels</param>
        /// <returns>List de personnel</returns>
        public List<PersonnelEnt> GetPersonnelByListPersonnelId(List<int?> ListPersonnelId)
        {
            return Repository.GetPersonnelByListPersonnelId(ListPersonnelId);
        }


        /// <summary>
        /// Cherche un interimaire par matricule externe et groupe code
        /// </summary>
        /// <param name="matriculeExterne">Matricule externe</param>
        /// <param name="groupeCode">Groupe code</param>
        /// <param name="systemeInterimaire">System interimaire (Pixid par exemple)</param>
        /// <returns>Personnel interimaire si existe</returns>
        public PersonnelEnt GetPersonnelInterimaireByExternalMatriculeAndGroupeId(string matriculeExterne, string groupeCode, string systemeInterimaire)
        {
            return Repository.GetPersonnelInterimaireByExternalMatriculeAndGroupeId(matriculeExterne, groupeCode, systemeInterimaire);
        }

        /// <summary>
        /// Get etab paie list for validation pointage vrac Fes Async
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="recherche">Recherche</param>
        /// <param name="societeId">Societe Id</param>
        /// <param name="dateDebut">Date debut</param>
        /// <param name="StatutPersonnelList">List des personnels statut</param>
        /// <returns>Etab Paie List</returns>
        public async Task<IEnumerable<PersonnelEnt>> GetPersonnelListForValidationPointageVracFesAsync(int page, int pageSize, string recherche, int? societeId, DateTime dateDebut, IEnumerable<string> StatutPersonnelList)
        {
            IEnumerable<PersonnelEnt> personnelList = new List<PersonnelEnt>();
            if (societeId.HasValue)
            {
                personnelList = await Repository.GetPersonnelBySocieteId(societeId.Value, StatutPersonnelList, dateDebut).ConfigureAwait(false);
            }
            else
            {
                IEnumerable<SocieteEnt> societeList = await SocieteManager.GetSocietesListForRemonteeVracFesAsync(0, 0, string.Empty).ConfigureAwait(false);
                if (societeList.Any())
                {
                    List<int> societeIds = societeList.Select(x => x.SocieteId).ToList();
                    personnelList = await Repository.GetPersonnelsBySocieteIdsListAsync(societeIds, dateDebut, StatutPersonnelList).ConfigureAwait(false);
                }
            }

            return personnelList.Distinct().Where((o) => FilterText(recherche, o)).OrderBy(x => x.Matricule).Skip((page - 1) * pageSize).Take(pageSize);
        }

        #region private

        private void UpdatePersonnelCategorie(PersonnelEnt personnel)
        {
            KeyValuePair<string, string> categoriePerso = Constantes.TypePersonnel.StatutCategorie.FirstOrDefault(x => x.Key.Equals(personnel.Statut, StringComparison.OrdinalIgnoreCase));
            personnel.CategoriePerso = (!categoriePerso.Equals(default(KeyValuePair<string, string>))) ? categoriePerso.Value : string.Empty;
        }

        private PersonnelForMoyenAffectationsRolesFiltersEnt GetPersonnelForMoyenAffectationsRolesFilters(bool isForAffectationMoyen)
        {
            PersonnelForMoyenAffectationsRolesFiltersEnt personnelForMoyenAffectationsRolesFilters = new PersonnelForMoyenAffectationsRolesFiltersEnt
            {
                ForAffectationMoyen = isForAffectationMoyen
            };

            int userId = UtilisateurManager.GetContextUtilisateurId();
            if (UtilisateurManager.IsSuperAdmin(userId))
            {
                personnelForMoyenAffectationsRolesFilters.IsSuperAdmin = true;
            }

            if (UtilisateurManager.HasPermissionToSeePersonnelsList())
            {
                personnelForMoyenAffectationsRolesFilters.HasPermissionToSeePersonnelList = true;
            }

            if (UtilisateurManager.IsUserManagerPersonnel(userId))
            {
                personnelForMoyenAffectationsRolesFilters.IsManagerPersonnel = true;
                personnelForMoyenAffectationsRolesFilters.ManagerPersonnelsList = GetManagedEmployeeIdList(userId);
            }

            if (UtilisateurManager.IsUserResponsableCi(userId))
            {
                personnelForMoyenAffectationsRolesFilters.IsResponsableCI = true;
                List<int> responsableCiList = UtilisateurManager.GetCiListOfResponsable().Select(c => c.CiId).ToList();
                personnelForMoyenAffectationsRolesFilters.ResponsableCiPersonnelList = AffectationManager.GetPersonnelsAffectationListByCiList(responsableCiList).ToList();
            }

            return personnelForMoyenAffectationsRolesFilters;
        }

        private List<int> GetRzbEligibleSocieteIds(int societeId, int groupeId)
        {
            List<int> societeIds = new List<int>() { societeId };
            int? societeInterimId = SocieteManager.GetSocieteInterim(groupeId)?.SocieteId;
            if (societeInterimId.HasValue)
            {
                societeIds.Add(societeInterimId.Value);
            }

            return societeIds;
        }

        private List<int> GetSepEligibleSocieteIds(int societeId, int groupeId, bool? onlySocieteGerante)
        {
            List<int> societeParticipanteIds = new List<int>();
            if (onlySocieteGerante == true)
            {
                SocieteEnt societeGerante = SepService.GetSocieteGerante(societeId);
                if (societeGerante != null)
                {
                    societeParticipanteIds.Add(societeGerante.SocieteId);
                }
            }
            else
            {
                // Si on veut toutes les sociétés participantes de la SEP
                List<SocieteEnt> societeParticipantes = SepService.GetSocieteParticipantes(societeId);
                if (societeParticipantes.Count > 0)
                {
                    societeParticipanteIds.AddRange(societeParticipantes.Select(x => x.SocieteId));
                }
            }

            int? societeInterimId = SocieteManager.GetSocieteInterim(groupeId)?.SocieteId;
            if (societeInterimId.HasValue)
            {
                societeParticipanteIds.Add(societeInterimId.Value);
            }

            return societeParticipanteIds;
        }

        private void SetSearchLightPersonnelParams(SearchLightPersonnelModel search)
        {
            search.IsGrzb = CurrentUser?.Personnel?.Societe?.Groupe.Code == CodeGroupeRZB;
            if (search.CiId.HasValue)
            {
                search.EtablissementComptableId = CIManager.GetEtablissementComptableByCIId(search.CiId.Value)?.EtablissementComptableId;
                SocieteEnt societe = CIManager.GetSocieteByCIId(search.CiId.Value);
                if (societe != null)
                {
                    if (SepService.IsSep(societe))
                    {
                        search.IsSep = true;
                        search.SepEligibleSocieteIds = GetSepEligibleSocieteIds(societe.SocieteId, societe.GroupeId, search.OnlySocieteGerante);
                    }

                    search.GrzbEligibleSocieteIds = GetRzbEligibleSocieteIds(societe.SocieteId, societe.GroupeId);
                }
            }
        }

        private IEnumerable<PersonnelEnt> SearchLightRzb(SearchLightPersonnelModel search)
        {
            SetSearchLightPersonnelParams(search);
            if (search.IsGrzb && search.CiId.HasValue && (!search.IsForAffectationWithoutInterim.HasValue || !search.IsForAffectationWithoutInterim.Value))
            {
                CIEnt ci = CIManager.GetCI(search.CiId.Value);
                if (ci != null && ci.IsDisableForPointage)
                {
                    return GetRzbPersonnelAffectedForSearchLight(search);
                }
            }

            return SearchLightRzbStandard(search);
        }

        private IEnumerable<PersonnelEnt> GetRzbPersonnelAffectedForSearchLight(SearchLightPersonnelModel search)
        {
            List<PersonnelEnt> personnelsAffected = new List<PersonnelEnt>();
            var societeId = CurrentUser.Personnel.Societe.SocieteId;
            personnelsAffected = GetRzbPersonnelListAffectedForSearchLight(societeId, search);
            personnelsAffected.AddRange(GetRzbInterimForSearchLight(search));
            return personnelsAffected;
        }

        private async Task<IEnumerable<PersonnelEnt>> SearchLightFesAsync(SearchLightPersonnelModel search)
        {
            int? currentUserGroupeId = CurrentUser?.Personnel?.Societe?.GroupeId;
            SetSearchLightPersonnelParams(search);
            PersonnelForMoyenAffectationsRolesFiltersEnt personnelForMoyenAffectationsRolesFilters = GetPersonnelForMoyenAffectationsRolesFilters(search.ForAffectationMoyen);

            var tuple = await GetSocieteAndEtablissementIdsByRoleAsync(search.ForAffectationMoyen).ConfigureAwait(false);
            var societeIds = tuple.Item1;
            var etablissementsPayIds = tuple.Item2;

            if (societeIds.Any() ||
                etablissementsPayIds.Any() ||
                personnelForMoyenAffectationsRolesFilters.ResponsableCiPersonnelList.Any())
            {
                return Repository.Query().Include(p => p.Societe).Include(p => p.Ressource)
                               .Include(p => p.EtablissementPaie)
                               .Include(p => p.EtablissementRattachement)
                               .Include(p => p.Manager)
                               .Filter(x => this.CurrentUser.SuperAdmin || currentUserGroupeId == x.Societe.GroupeId)
                               .Filter(p => !search.SocieteId.HasValue || p.SocieteId == search.SocieteId.Value)
                               .Filter(search.GetSearchedTextPredicat())
                               .Filter(search.GetWithInterimairePredicat())
                               .Filter(search.GetWithoutInterimairePredicat())
                               .Filter(search.GetStatutPredicat())
                               .Filter(search.GetStatutPredicatFes())
                               .Filter(search.GetOnlyUtilisateurPredicat())
                               .Filter(search.GetActifOnlyPredicat())
                               .Filter(search.GetFullPersonnel())
                               .Filter(personnelForMoyenAffectationsRolesFilters.GetPredicateWhere())
                               .Filter(x => societeIds.Contains(x.SocieteId.Value) ||
                                            etablissementsPayIds.Contains(x.EtablissementPaieId.Value) ||
                                            personnelForMoyenAffectationsRolesFilters.ResponsableCiPersonnelList.Contains(x.PersonnelId))
                               .OrderBy(list => list.OrderBy(pe => pe.Societe.Code).ThenBy(pe => pe.Matricule))
                               .GetPage(search.Page, search.PageSize);
            }

            return new List<PersonnelEnt>();
        }

        private async Task<Tuple<List<int>, List<int>>> GetSocieteAndEtablissementIdsByRoleAsync(bool isGestionMoyen)
        {
            List<int> societeOrgaIds = new List<int>();
            List<int> etablissementOrgaIds = new List<int>();
            List<int> societeIds = new List<int>();
            List<int> etablissementsPayIds = new List<int>();

            var utilisateursRoleOrga = await affectationManager.GetAffectationByUserAndRolesAsync(CurrentUser.UtilisateurId, isGestionMoyen).ConfigureAwait(false);

            foreach (var utilisateurRoleOrga in utilisateursRoleOrga)
            {
                if (utilisateurRoleOrga.Organisation.TypeOrganisation.Code.Equals(Constantes.OrganisationType.CodeSociete))
                {
                    societeOrgaIds.Add(utilisateurRoleOrga.OrganisationId);
                }

                if (utilisateurRoleOrga.Organisation.TypeOrganisation.Code.Equals(Constantes.OrganisationType.CodeEtablissement))
                {
                    etablissementOrgaIds.Add(utilisateurRoleOrga.OrganisationId);
                }
            }

            if (societeOrgaIds.Any())
            {
                societeIds.AddRange(SocieteManager.GetSocieteByOrganisationIds(societeOrgaIds).Select(x => x.SocieteId).Distinct().ToList());
            }

            if (etablissementOrgaIds.Any())
            {
                etablissementsPayIds.AddRange(etablissementComptableManager.GetEtablissementComptableByOrganisationIds(etablissementOrgaIds)
                           .SelectMany(x => x.EtablissementsPaie).Select(x => x.EtablissementPaieId).Distinct().ToList());
            }

            return Tuple.Create(societeIds, etablissementsPayIds);
        }

        private IEnumerable<PersonnelEnt> SearchLightRzbStandard(SearchLightPersonnelModel search)
        {
            int? currentUserSocieteId = CurrentUser?.Personnel?.Societe?.SocieteId;
            int? currentUserGroupeId = currentUser?.Personnel?.Societe?.Groupe?.GroupeId;
            bool isForOneDay = search.DateDebutChantier.HasValue && search.DateFinChantier.HasValue && search.DateDebutChantier == search.DateFinChantier;

            return this.Repository.Query().Include(p => p.Societe)
                                           .Include(p => p.Ressource)
                                           .Include(p => p.EtablissementPaie)
                                           .Include(p => p.EtablissementRattachement)
                                           .Include(p => p.ContratInterimaires.Select(c => c.ZonesDeTravail))
                                           .Filter(p => !p.IsPersonnelNonPointable)
                                           .Filter(x => this.CurrentUser.SuperAdmin || currentUserGroupeId.Value == x.Societe.GroupeId || currentUserSocieteId.Value == x.Societe.SocieteId)
                                           .Filter(p => !search.SocieteId.HasValue || p.SocieteId == search.SocieteId.Value)
                                           .Filter(search.GetImprovedSearchedTextPredicat())
                                           .Filter(search.GetWithInterimairePredicat())
                                           .Filter(search.GetWithoutInterimairePredicat())
                                           .Filter(p => !p.IsInterimaire || !isForOneDay || GetPersonnelInterimaireIds(search).Contains(p.PersonnelId))
                                           .Filter(search.GetGrzbPredicat())
                                           .Filter(search.GetSepPredicat())
                                           .Filter(search.GetOnlyUtilisateurPredicat())
                                           .Filter(search.GetFullPersonnel())
                                           .OrderBy(list => list.OrderBy(pe => pe.Societe.Code).ThenBy(pe => pe.Matricule))
                                           .GetPage(search.Page, search.PageSize);
        }

        private List<int> GetPersonnelInterimaireIds(SearchLightPersonnelModel search)
        {
            List<int> personnelIds = new List<int>();

            var contrats = this.contratInterimaireRepository.GetListContratsInterimaires(search.DateDebutChantier, search.DateFinChantier);
            if (contrats != null)
            {
                List<int> personnelIdList = contrats.Select(p => p.InterimaireId).Distinct().ToList();
                foreach (int personnelId in personnelIdList)
                {
                    var contratsCurrentPersonnel = contrats.Where(x => x.InterimaireId == personnelId).ToList();
                    if (contratsCurrentPersonnel != null && contratsCurrentPersonnel.Any())
                    {
                        var withoutSoupless = contratsCurrentPersonnel.Where(c => c.DateDebut.Date <= search.DateDebutChantier && search.DateFinChantier <= c.DateFin.Date).ToList();
                        if (withoutSoupless != null && withoutSoupless.Any())
                        {
                            withoutSoupless = withoutSoupless.Where(c => ((!search.CiId.HasValue || c.CiId == search.CiId)
                                                        || (!search.EtablissementComptableId.HasValue
                                                        || c.ZonesDeTravail.Any(z => z.EtablissementComptableId == search.EtablissementComptableId)))).ToList();

                            if (withoutSoupless != null && withoutSoupless.Any())
                            {
                                personnelIds.AddRange(withoutSoupless.Select(x => x.InterimaireId).Distinct());
                            }
                        }
                        else
                        {
                            var contratsCurrentPersonnelSouplesse = new List<ContratInterimaireEnt>();
                            contratsCurrentPersonnelSouplesse.AddRange(contratsCurrentPersonnel);
                            contratsCurrentPersonnelSouplesse.ForEach(x => x.DateFin = x.DateFin.AddDays(x.Souplesse));

                            var withSoupless = contratsCurrentPersonnelSouplesse.Where(c => c.DateDebut.Date <= search.DateDebutChantier && search.DateFinChantier <= c.DateFin.Date)
                                                .OrderByDescending(x => x.ContratInterimaireId)
                                                .FirstOrDefault(c => ((!search.CiId.HasValue || c.CiId == search.CiId)
                                                        || (!search.EtablissementComptableId.HasValue
                                                        || c.ZonesDeTravail.Any(z => z.EtablissementComptableId == search.EtablissementComptableId))));

                            if (withSoupless != null)
                            {
                                personnelIds.Add(withSoupless.InterimaireId);
                            }
                        }
                    }
                }
            }

            return personnelIds.Distinct().ToList();
        }

        private List<PersonnelEnt> GetRzbPersonnelListAffectedForSearchLight(int societeId, SearchLightPersonnelModel search)
        {
            return Repository.Query().Include(p => p.Societe)
                                .Include(p => p.Ressource)
                                .Include(p => p.EtablissementPaie)
                                .Include(p => p.EtablissementRattachement)
                                .Filter(p => p.SocieteId == societeId)
                                .Filter(p => !p.IsInterimaire)
                                .Filter(p => !p.IsPersonnelNonPointable)
                                .Filter(p => !p.DateSortie.HasValue || (search.DateChantier.HasValue && p.DateSortie >= search.DateChantier))
                                .Filter(search.GetSearchedTextPredicat())
                                .OrderBy(list => list.OrderBy(pe => pe.Societe.Code).ThenBy(pe => pe.Matricule))
                                .GetPage(search.Page, search.PageSize).ToList();
        }

        private List<PersonnelEnt> GetRzbInterimForSearchLight(SearchLightPersonnelModel search)
        {
            List<PersonnelEnt> interimaires = this.Repository.Query().Include(p => p.Societe)
                                           .Include(p => p.Ressource)
                                           .Include(p => p.EtablissementPaie)
                                           .Include(p => p.EtablissementRattachement)
                                           .Include(p => p.ContratInterimaires.Select(c => c.ZonesDeTravail))
                                           .Filter(p => p.ContratInterimaires.Any(c =>
                                            (search.DateFinChantier.HasValue &&
                                            search.DateDebutChantier.HasValue &&
                                            search.DateDebutChantier.Value.Date >= c.DateDebut.Date &&
                                            search.DateFinChantier.Value.Date <= c.DateFin.AddDays(c.Souplesse).Date) &&
                                            ((!search.EtablissementComptableId.HasValue || c.ZonesDeTravail.Any(z => z.EtablissementComptableId == search.EtablissementComptableId.Value))
                                            || (!search.CiId.HasValue || c.CiId == search.CiId))))
                                           .Filter(search.GetImprovedSearchedTextPredicat())
                                           .OrderBy(list => list.OrderBy(pe => pe.Societe.Code).ThenBy(pe => pe.Matricule))
                                           .GetPage(search.Page, search.PageSize).ToList();

            List<PersonnelEnt> interimairesRemove = new List<PersonnelEnt>();
            foreach (var interimaire in interimaires)
            {
                List<ContratInterimaireEnt> contratsActifInPeriode = new List<ContratInterimaireEnt>();
                contratsActifInPeriode = interimaire.ContratInterimaires.Where(c => search.DateDebutChantier.Value.Date >= c.DateDebut.Date && search.DateFinChantier.Value.Date <= c.DateFin.AddDays(c.Souplesse).Date).ToList();
                if (contratsActifInPeriode.Count > 1)
                {
                    ContratInterimaireEnt contratInterimaireMaxDate = contratsActifInPeriode.OrderByDescending(c => c.DateDebut).FirstOrDefault();
                    if ((!search.EtablissementComptableId.HasValue || contratInterimaireMaxDate.ZonesDeTravail.Any(z => z.EtablissementComptableId != search.EtablissementComptableId.Value))
                                       || (!search.CiId.HasValue || contratInterimaireMaxDate.CiId != search.CiId))
                    {
                        interimairesRemove.Add(interimaire);
                    }
                }
            }

            return interimaires.Where(i => !interimairesRemove.Contains(i)).ToList();
        }

        private List<int> GetUserIdsFromTreeOraganisationByCiId(int ciId)
        {
            OrganisationTree organisationTree = organisationTreeService.GetOrganisationTree();
            var tree = organisationTree.GetSocieteParentOfCi(ciId);
            List<int> listPerso = tree.Affectations.ConvertAll(x => x.UtilisateurId);
            return listPerso;
        }

        private bool FilterText(string text, PersonnelEnt o)
        {
            if (string.IsNullOrEmpty(text) || (o.Nom != null && ComparatorHelper.ComplexContains(o.Nom, text)) || (o.Societe != null && ComparatorHelper.ComplexContains(o.Societe.Code, text))

               || (o.Prenom != null && ComparatorHelper.ComplexContains(o.Prenom, text)) || (o.Matricule != null && ComparatorHelper.ComplexContains(o.Matricule, text)))
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}
