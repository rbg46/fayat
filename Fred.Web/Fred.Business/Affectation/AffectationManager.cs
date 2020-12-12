using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Business.CI;
using Fred.Business.Personnel;
using Fred.Business.Rapport.Common.RapportParStatut;
using Fred.Business.Role;
using Fred.Business.Utilisateur;
using Fred.Business.Valorisation;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Affectation;
using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using Fred.Web.Models.Affectation;
using Fred.Web.Shared.Extentions;
using Fred.Web.Shared.Models.EtatPaie;
using MoreLinq;

namespace Fred.Business.Affectation
{
    public class AffectationManager : Manager<AffectationEnt, IAffectationRepository>, IAffectationManager
    {
        private readonly IUtilisateurManager userManager;
        private readonly IRoleManager roleManager;
        private readonly ICIManager ciManager;
        private readonly IValorisationManager valorisationManager;
        private readonly IRapportLigneCodeAstreinteRepository codeAstreinteRepository;
        private readonly IRapportRepository rapportRepository;
        private readonly IRapportLignePrimeRepository rapportLignePrimeRepository;
        private readonly IPrimeRepository primeRepository;
        private readonly IRepository<RapportLigneEnt> rapportLigneRepository;
        private readonly IPointageRepository pointageRepository;

        public AffectationManager(
            IUnitOfWork uow,
            IAffectationRepository affectationRepository,
            IUtilisateurManager userManager,
            IRoleManager roleManager,
            ICIManager ciManager,
            IRapportLigneCodeAstreinteRepository codeAstreinteRepository,
            IRapportRepository rapportRepository,
            IRapportLignePrimeRepository rapportLignePrimeRepository,
            IPrimeRepository primeRepository,
            IRepository<RapportLigneEnt> rapportLigneRepository,
            IValorisationManager valorisationManager,
            IPointageRepository pointageRepository)
            : base(uow, affectationRepository)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.codeAstreinteRepository = codeAstreinteRepository;
            this.rapportRepository = rapportRepository;
            this.rapportLignePrimeRepository = rapportLignePrimeRepository;
            this.primeRepository = primeRepository;
            this.rapportLigneRepository = rapportLigneRepository;
            this.ciManager = ciManager;
            this.valorisationManager = valorisationManager;
            this.pointageRepository = pointageRepository;
        }

        #region public method

        /// <summary>
        /// Ajouter ou modifier une list des affectations
        /// </summary>
        /// <param name="calendarAffectationViewEnt">Calendar affectation view entity</param>
        public void AddOrUpdateAffectationList(CalendarAffectationViewEnt calendarAffectationViewEnt)
        {
            if (calendarAffectationViewEnt != null)
            {
                CIEnt ci = ciManager.GetCiForAffectationByCiId(calendarAffectationViewEnt.CiId);
                if (ci == null)
                {
                    return;
                }

                string codeGrp = ci.Societe.Groupe.Code;
                ci.IsAstreinteActive = calendarAffectationViewEnt.IsAstreinteActive;
                ci.IsDisableForPointage = calendarAffectationViewEnt.IsDisableForPointage;
                ciManager.UpdateCI(ci);
                if (codeGrp.Equals(Constantes.CodeGroupeFES))
                {
                    HandleAddOrUpdateAffectationListFes(ci, calendarAffectationViewEnt);
                }
                else
                {
                    HandleAffectationListRzb(calendarAffectationViewEnt);
                }
            }
        }

        /// <summary>
        /// Get la liste des personnels affectés a un CI
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="dateDebut">Date debut de la semaine</param>
        /// <param name="dateFin">Date fin de la semaine</param>
        /// <returns>Calendar affectation</returns>
        public CalendarAffectationViewEnt GetAffectationListByCi(int ciId, DateTime dateDebut, DateTime dateFin)
        {
            CIEnt ci = ciManager.GetCiForAffectationByCiId(ciId);
            CalendarAffectationViewEnt calendarAffectationEnt = new CalendarAffectationViewEnt();
            if (ci != null)
            {
                calendarAffectationEnt.CiId = ci.CiId;
                calendarAffectationEnt.IsAstreinteActive = ci.IsAstreinteActive;
                calendarAffectationEnt.IsDisableForPointage = ci.IsDisableForPointage;
                calendarAffectationEnt.StartDateOfTheWeek = dateDebut;
                calendarAffectationEnt.EndDateOfTheWeek = dateFin;
                IEnumerable<AffectationEnt> affectationList = this.Repository.GetAffectationListByCi(ciId);
                if (affectationList != null && affectationList.Any())
                {
                    calendarAffectationEnt.IsCiAffectationsHasAstreintes = affectationList.Any(x => x.Astreintes.Any());
                    foreach (AffectationEnt affectation in affectationList)
                    {
                        if (affectation != null)
                        {
                            affectation.Astreintes = affectationList.FirstOrDefault(x => x.AffectationId == affectation.AffectationId).Astreintes?.Where(x => x.DateAstreinte >= dateDebut && x.DateAstreinte <= dateFin).ToList();
                        }
                    }

                    calendarAffectationEnt.AffectationList = AffectationListToAffectationViewList(affectationList, dateDebut);
                }
                else
                {
                    calendarAffectationEnt.AffectationList = new List<AffectationViewEnt>();
                    calendarAffectationEnt.IsCiAffectationsHasAstreintes = false;
                }
            }

            return calendarAffectationEnt;
        }

        /// <summary>
        /// Récuperer la liste des affectation par lsit  CI
        /// </summary>
        /// <param name="ciIds">Les identifiants des CIs</param>
        /// <param name="etablissementPaieIdList">List Etablissement Paie Id </param>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>La liste des affectations</returns>
        public IEnumerable<PersonnelEnt> GetPersonneListByCiIdList(IEnumerable<int> ciIds, IEnumerable<int?> etablissementPaieIdList, EtatPaieExportModel etatPaieExportModel)
        {
            return this.Repository.GetPersonnelListAffectedCiList(ciIds, etablissementPaieIdList, etatPaieExportModel).DistinctBy(i => i.PersonnelId);
        }

        /// <summary>
        /// Récuperer la liste des affectation par etablissement paie
        /// </summary>
        /// <param name="etablissementPaieIdList">List Etablissement Paie Id </param>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>La liste des affectations</returns>
        public IEnumerable<int> GetPersonnelIdListAffectedEtablissementList(IEnumerable<int> etablissementPaieIdList, EtatPaieExportModel etatPaieExportModel)
        {
            return this.Repository.GetPersonnelIdListAffectedEtablissementList(etablissementPaieIdList, etatPaieExportModel);
        }

        /// <summary>
        /// Supprimer une liste des affectations 
        /// </summary>
        /// <param name="affectationLisIds">liste des affectations identifiers</param>
        public void DeleteAffectations(List<int> affectationLisIds)
        {
            if (affectationLisIds != null && affectationLisIds.Any())
            {
                this.Repository.DeleteAffectations(affectationLisIds);
                Save();
            }
        }
        /// <summary>
        /// Supprimer une liste des affectations 
        /// </summary>
        /// <param name="ciId">Ci id</param>
        /// <param name="affectationLisIds">liste des affectations identifiers</param>
        public void DeleteAffectations(int ciId, List<int> affectationLisIds)
        {
            if (affectationLisIds == null || !affectationLisIds.Any())
            {
                return;
            }

            List<AffectationEnt> affectationEnList = new List<AffectationEnt>();
            List<int> personnelIdList = new List<int>();
            foreach (int affectationId in affectationLisIds)
            {
                AffectationEnt affectation = this.Repository.GetAffectationById(affectationId);
                if (affectation != null)
                {
                    affectationEnList.Add(affectation);
                    personnelIdList.Add(affectation.PersonnelId);
                }
            }
            this.Repository.DeleteAffectations(affectationEnList);
            Save();
            // Suppression de la délégation
            int? organisationId = ciManager.GetOrganisationIdByCiId(ciId);
            this.userManager.ManageRoleDelegueForCiPersonnel(
                organisationId.Value,
                utilisateurIdLisToAdd: null,
                utilisateurIdListToRemove: personnelIdList);
        }

        /// <summary>
        /// Récupérer une astreinte d'un personnel dans un CI et une date précise
        /// </summary>
        /// <param name="ciId">L'idenitifiant du CI</param>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="astreinteDate">La date de l'astreinte</param>
        /// <returns>L'astreinte</returns>
        public AstreinteEnt GetAstreinte(int ciId, int personnelId, DateTime astreinteDate)
        {
            return this.Repository.GetAstreinte(ciId, personnelId, astreinteDate);
        }

        /// <summary>
        /// Get list des astreintes par personnel et ci
        /// </summary>
        /// <param name="personnelId">personnel identifier</param>
        /// <param name="ciId">ci identifier</param>
        /// <param name="dateDebut">Date debut</param>
        /// <param name="dateFin">Date fin</param>
        /// <returns>List des astreintes</returns>
        public IEnumerable<AstreinteEnt> GetAstreintesByPersonnelIdAndCiId(int personnelId, int ciId, DateTime dateDebut, DateTime dateFin)
        {
            return this.Repository.GetAstreintesByPersonnelIdAndCiId(personnelId, ciId, dateDebut, dateFin);
        }

        /// <summary>
        /// Retourne les astreintes pour un personnel donné et qui appartiennent a la fois a un ci de la liste ciIds et a une DateAstreinte de la liste astreinteDates
        /// </summary>
        /// <param name="ciIds">ciIds</param>
        /// <param name="personnelId">personnelId</param>
        /// <param name="astreinteDates">astreinteDates</param>
        /// <returns>Retourne les astreintes pour un personnel donné</returns>
        public IEnumerable<AstreinteEnt> GetAstreintes(IEnumerable<int> ciIds, int personnelId, IEnumerable<DateTime> astreinteDates)
        {
            return this.Repository.GetAstreintes(ciIds, personnelId, astreinteDates);
        }

        /// <summary>
        ///  Récupérer une astreinte d'un personnel dans un CI et une date précise a partir d'une liste d'astreinte
        /// </summary>
        /// <param name="astreintes">astreintes</param>
        /// <param name="ciId">L'idenitifiant du CI</param>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="astreinteDate">La date de l'astreinte</param>
        /// <returns>L'astreinte</returns>
        public AstreinteEnt GetAstreinte(IEnumerable<AstreinteEnt> astreintes, int ciId, int personnelId, DateTime astreinteDate)
        {
            return this.Repository.GetAstreinte(astreintes, ciId, personnelId, astreinteDate);
        }

        /// <summary>
        /// Récupérer une astreinte d'un personnel dans un CI et une date précise
        /// </summary>
        /// <param name="ciId">L'idenitifiant du CI</param>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="astreinteDate">La date de l'astreinte</param>
        /// <returns>L'astreinte</returns>
        public int? GetAstreinteId(int ciId, int personnelId, DateTime astreinteDate)
        {
            return this.Repository.GetAstreinteId(ciId, personnelId, astreinteDate);
        }

        /// <summary>
        /// Récuperer la liste des affectation d'un CI
        /// </summary>
        /// <param name="ciId">L'identifiant du CI</param>
        /// <returns>La liste des affectations</returns>
        public IEnumerable<AffectationEnt> GetAffectationsByCiId(int ciId)
        {
            return this.Repository.GetAffectationListByCi(ciId);
        }

        /// <summary>
        ///  Récuperer l'affectation d'un CI et d'un personnel
        /// </summary>
        /// <param name="ciId">L'identifiant du CI</param>
        /// <param name="personnelId">L'identifiant du Personnel</param>
        /// <returns>affectations</returns>
        public AffectationEnt GetAffectationByCiAndPersonnel(int ciId, int personnelId)
        {
            return this.Repository.GetAffectationByCiAndPersonnel(ciId, personnelId);
        }

        /// <summary>
        /// Récuperer la liste des affectations des personnels passé en paramétres
        /// </summary>
        /// <param name="personneId">Identifiant des personnels</param>
        /// <returns>List d'affectation</returns>
        public List<AffectationEnt> GetAffectationByListPersonnelId(List<int> personneId)
        {
            return this.Repository.GetAffectationByListPersonnelId(personneId);
        }

        /// <summary>
        /// Récupérer la liste des CI dans le quel le personnel est affecté
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <returns>La liste des CI</returns>
        public List<CIEnt> GetPersonnelAffectationCiList(int personnelId)
        {
            return this.Repository.GetPersonnelAffectationCiList(personnelId);
        }

        /// <summary>
        /// Récupérer la liste des CI dans le quel le personnel est affecté
        /// </summary>
        /// <typeparam name="TCI">Le type de CI désiré.</typeparam>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="selector">Selector permettant de constuire un TCI en fonction d'un CIEnt.</param>
        /// <returns>La liste des CI</returns>
        public List<TCI> GetPersonnelAffectationCiList<TCI>(int personnelId, Expression<Func<CIEnt, TCI>> selector)
        {
            return Repository.GetPersonnelAffectationCiList<TCI>(personnelId, selector);
        }

        /// <summary>
        /// Get identifiers of Etam and Iac affected to a CI
        /// </summary>
        /// <param name="ciIdList">List ci Identifier</param>
        /// <returns>List des personnels Ids</returns>
        public IEnumerable<int> GetEtamAndIacAffectationListByCiList(IEnumerable<int> ciIdList)
        {
            return this.Repository.GetEtamAndIacAffectationListByCiList(ciIdList);
        }

        /// <summary>
        /// Get Personnel id list affected to a CI
        /// </summary>
        /// <param name="ciIdList">List ci Identifier</param>
        /// <returns>List des personnels Ids</returns>
        public IEnumerable<int> GetPersonnelsAffectationListByCiList(IEnumerable<int> ciIdList)
        {
            return this.Repository.GetPersonnelsAffectationListByCiList(ciIdList);
        }

        /// <summary>
        /// Obtient le ci  par defaut d'un personnel
        /// </summary>
        /// <param name="presonnelId">personnelid</param>
        /// <returns>return Ci </returns>
        public CIEnt GetDefaultCi(int presonnelId)
        {
            AffectationEnt defaultAffectation = this.Repository.Get().Where(e => e.PersonnelId == presonnelId && e.IsDefault).FirstOrDefault();

            if (defaultAffectation != null)
            {
                return this.ciManager.GetCI(defaultAffectation.CiId);
            }

            return null;
        }

        /// <summary>
        /// Récupération ou création d'une Affectation
        /// </summary>
        /// <param name="personnelId">Id du personnel</param>
        /// <param name="ciId">Id du CI</param>
        /// <param name="isDelegate">Delegation</param>
        /// <returns>Affectation Entité</returns>
        public AffectationEnt GetOrCreateAffectation(int personnelId, int ciId, bool isDelegate)
        {
            return Repository.GetOrCreateAffectation(personnelId, ciId, isDelegate);
        }

        /// <summary>
        /// Récupération ou New d'une Affectation sans besoin de Delegation (exemple : pour RazelBec)
        /// </summary>
        /// <param name="personnelId">Id du personnel</param>
        /// <param name="ciId">Id du CI</param>
        /// <returns>Affectation Entité</returns>
        public async Task<AffectationEnt> GetOrNewAffectationAsync(int personnelId, int ciId)
        {
            return await Repository.GetOrNewAffectationAsync(personnelId, ciId);
        }

        /// <summary>
        /// Update delegue role affectation . Fait un update de la déléguation au niveau de la table des affectations Ci/ Personnel
        /// </summary>
        /// <param name="utilisateurId">Utilisateur Id</param>
        /// <param name="listAffectations">list affectation</param>
        public void UpdateDelegueRoleAffectation(int utilisateurId, IEnumerable<AffectationSeuilUtilisateurEnt> listAffectations)
        {
            if (!listAffectations.IsNullOrEmpty())
            {
                IEnumerable<Tuple<int, int, int>> ciOrgaModelList = GetOrganisationCiList(listAffectations);
                IEnumerable<CIEnt> ciList = GetPersonnelAffectationCiList(utilisateurId);

                IEnumerable<CIEnt> ciDelgationList = ciOrgaModelList == null ? ciList : ciList?.Where(r => !ciOrgaModelList.Select(g => g.Item2).Any(v => v == r.CiId));
                IEnumerable<AffectationModel> delegationToDeleteList = ciDelgationList?.Select(ci => new AffectationModel { CiId = ci.CiId, PersonnelId = utilisateurId, IsDelegue = false });

                IEnumerable<AffectationModel> delegationToAddList = GetDelegationToAddList(utilisateurId, ciOrgaModelList);

                if (!delegationToAddList.IsNullOrEmpty())
                {
                    Repository.UpdateAffectationList(delegationToAddList);
                    Save();
                }

                if (!delegationToDeleteList.IsNullOrEmpty())
                {
                    Repository.UpdateAffectationList(delegationToDeleteList);
                    Save();
                }
            }
        }

        /// <summary>
        /// Get la liste des personnels affectés a un  etablissement de paie par l'organisation id
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>List des affectations</returns>
        public IEnumerable<int> GetPersonnelIdAffectedEtablissementByOrganisationId(EtatPaieExportModel etatPaieExportModel)
        {
            return Repository.GetPersonnelIdAffectedEtablissementByOrganisationId(etatPaieExportModel);
        }

        /// <summary>
        /// Récupérer la liste des CI dans le quel le personnel est affecté
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <returns>La liste des CI</returns>
        public List<CIEnt> GetPersonnelAffectationCiListFiggo(int personnelId)
        {
            return this.Repository.GetPersonnelAffectationCiListFiggo(personnelId);
        }

        /// <summary>
        /// verifier si le personnel a ci par defaut
        /// </summary>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <returns>true si le personnel a un ci par defaut</returns>
        public bool CheckIfPersonnelHasDefaultCi(int personnelId)
        {
            return this.Repository.CheckIfPersonnelHasDefaultCi(personnelId);
        }

        /// <summary>
        /// Recuperer list des ouvriers Ids par list des ci Ids
        /// </summary>
        /// <param name="ciList"></param>
        /// <returns>List des ouvriers ids</returns>
        public async Task<IEnumerable<AffectationEnt>> GetOuvriersListIdsByCiListAsync(List<int> ciList)
        {
            return await Repository.GetOuvriersListIdsByCiListAsync(ciList).ConfigureAwait(false);
        }

        /// <summary>
        /// Récupérer la liste des CI actifs dans le quel le personnel est affecté
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <returns>La liste des CI</returns>
        public IEnumerable<CIEnt> GetPersonnelActifAffectationCiList(int personnelId)
        {
            return this.Repository.GetPersonnelActifAffectationCiList(personnelId);
        }

        #endregion

        #region private method

        /// <summary>
        /// Manage la liste des affectations
        /// </summary>
        /// <param name="organisationId">Organisation id used to update Ci delegations</param>
        /// <param name="calendarAffectationViewEnt">Calendar affectation view entity</param>
        private void ManageAffectationList(int? organisationId, CalendarAffectationViewEnt calendarAffectationViewEnt)
        {
            if (calendarAffectationViewEnt != null && calendarAffectationViewEnt.AffectationList != null && calendarAffectationViewEnt.AffectationList.Any())
            {
                int currentUserId = userManager.GetContextUtilisateurId();
                ManageCiDelegation(organisationId, calendarAffectationViewEnt);

                foreach (AffectationViewEnt affectationView in calendarAffectationViewEnt.AffectationList)
                {
                    if (affectationView != null && affectationView.AffectationId == 0)
                    {
                        AddAffectation(calendarAffectationViewEnt.CiId, calendarAffectationViewEnt.StartDateOfTheWeek, affectationView);
                        Repository.AddUpdateToFavoriteTeam(affectationView.PersonnelId, currentUserId, affectationView.IsInFavoriteTeam);
                        Save();
                    }

                    if (affectationView != null && affectationView.AffectationId > 0)
                    {
                        UpdateAffectation(calendarAffectationViewEnt.StartDateOfTheWeek, affectationView);
                        Repository.AddUpdateToFavoriteTeam(affectationView.PersonnelId, currentUserId, affectationView.IsInFavoriteTeam);
                        Save();
                    }
                }
            }
        }

        /// <summary>
        /// Manage la liste des délégations
        /// </summary>
        /// <param name="organisationId">Organisation id used to update Ci delegations</param>
        /// <param name="calendarAffectationViewEnt">Calendar affectation view entity</param>
        private void ManageCiDelegation(int? organisationId, CalendarAffectationViewEnt calendarAffectationViewEnt)
        {
            if (!organisationId.HasValue || calendarAffectationViewEnt?.AffectationList == null || !calendarAffectationViewEnt.AffectationList.Any())
            {
                return;
            }

            // Restreindre le traitement des délégations pour les utilisateur fred .
            List<AffectationViewEnt> fredUtilisateurList = calendarAffectationViewEnt.AffectationList.Where(u => userManager.IsFredUser(u.PersonnelId)).ToList();
            List<int> delegateListToAdd = fredUtilisateurList.Where(a => a.IsDelegate).Select(a => a.PersonnelId).ToList();
            List<int> delegateListToRemove = fredUtilisateurList.Where(a => a.AffectationId > 0 && !a.IsDelegate).Select(a => a.PersonnelId).ToList();

            userManager.ManageRoleDelegueForCiPersonnel(
                organisationId.Value,
                utilisateurIdLisToAdd: delegateListToAdd,
                utilisateurIdListToRemove: delegateListToRemove);
        }

        /// <summary>
        /// Transform list des affectations entities to list of affectations view
        /// </summary>
        /// <param name="affectationEntList">List des affectations entities</param>
        /// <param name="dateDebut">Date debut de la semaine</param>
        /// <returns>List des affectations view</returns>
        private IEnumerable<AffectationViewEnt> AffectationListToAffectationViewList(IEnumerable<AffectationEnt> affectationEntList, DateTime dateDebut)
        {
            List<AffectationViewEnt> affectationViewListToAdd = new List<AffectationViewEnt>();
            if (affectationEntList != null && affectationEntList.Any())
            {
                int currentUserId = userManager.GetContextUtilisateurId();
                foreach (AffectationEnt affectation in affectationEntList)
                {
                    if (affectation != null)
                    {
                        AffectationViewEnt affectationViewEnt = CreateAffectationViewEnt(affectation, currentUserId);
                        affectationViewEnt.Astreintes = CreateAstreinteViewEnt(affectation, dateDebut);
                        affectationViewListToAdd.Add(affectationViewEnt);
                    }
                }
            }

            return affectationViewListToAdd;
        }

        /// <summary>
        /// Create affectation view entity from affectation entity
        /// </summary>
        /// <param name="affectationEnt">Affectation entity</param>
        /// <param name="currentUserId">Current user identifier</param>
        /// <returns>Affectation view entity</returns>
        private AffectationViewEnt CreateAffectationViewEnt(AffectationEnt affectationEnt, int currentUserId)
        {
            AffectationViewEnt affectationViewEnt = new AffectationViewEnt();
            affectationViewEnt.AffectationId = affectationEnt.AffectationId;
            affectationViewEnt.PersonnelId = affectationEnt.PersonnelId;
            affectationViewEnt.IsDefault = affectationEnt.IsDefault;
            if (affectationEnt.Personnel != null)
            {
                affectationViewEnt.CodeSociete = affectationEnt.Personnel.Societe != null ? affectationEnt.Personnel.Societe.Code : string.Empty;
                affectationViewEnt.Nom = affectationEnt.Personnel.Nom;
                affectationViewEnt.Prenom = affectationEnt.Personnel.Prenom;
                affectationViewEnt.Statut = PersonnelManager.GetStatut(affectationEnt.Personnel.Statut);
                affectationViewEnt.Matricule = affectationEnt.Personnel.Matricule;

                if (affectationEnt.Personnel.EquipePersonnels != null && affectationEnt.Personnel.EquipePersonnels.Any())
                {
                    affectationViewEnt.IsInFavoriteTeam = affectationEnt.Personnel.EquipePersonnels.Any(x => x.Equipe?.ProprietaireId == currentUserId);
                }

                affectationViewEnt.IsDelegate = affectationEnt.IsDelegue;
                affectationViewEnt.IsDelete = affectationEnt.IsDelete;
            }

            return affectationViewEnt;
        }

        /// <summary>
        /// Create liste des astreintes view entities
        /// </summary>
        /// <param name="affectation">Affectation entity</param>
        /// <param name="dateDebut">Date debut</param>
        /// <returns>List des astreintes view entities</returns>
        private IEnumerable<AstreinteViewEnt> CreateAstreinteViewEnt(AffectationEnt affectation, DateTime dateDebut)
        {
            List<AstreinteViewEnt> astreinteViewEnt = new List<AstreinteViewEnt>();
            if (affectation.Astreintes != null && affectation.Astreintes.Any())
            {
                foreach (AstreinteEnt astreinte in affectation.Astreintes)
                {
                    AstreinteViewEnt astreinteView = InstantiateViewEnt(affectation, astreinte);
                    astreinteViewEnt.Add(astreinteView);
                }
            }

            if (astreinteViewEnt.Count < 7)
            {
                for (int i = 0; i < 7; i++)
                {
                    AstreinteViewEnt astreinteWeekDay = astreinteViewEnt.FirstOrDefault(a => a.DayOfWeek == i);
                    if (astreinteWeekDay == null)
                    {
                        DateTime astreinteDate = i == 0 ? dateDebut.AddDays(6) : dateDebut.AddDays(i - 1);
                        astreinteWeekDay = new AstreinteViewEnt
                        {
                            DayOfWeek = i,
                            AstreinteId = 0,
                            IsRapportLigneVerouille = CheckRapportLigneStatut(affectation.CiId, affectation.PersonnelId, astreinteDate, RapportStatutEnt.RapportStatutVerrouille.Value)
                        };

                        astreinteViewEnt.Add(astreinteWeekDay);
                    }
                }
            }

            return astreinteViewEnt.OrderBy(x => x.DayOfWeek);
        }

        /// <summary>
        /// Get organisation ci list
        /// </summary>
        /// <param name="listAffectations">Liste des affectations</param>
        /// <returns>List de couple organisation id , ci id</returns>
        private List<Tuple<int, int, int>> GetOrganisationCiList(IEnumerable<AffectationSeuilUtilisateurEnt> listAffectations)
        {
            if (listAffectations.IsNullOrEmpty())
            {
                return new List<Tuple<int, int, int>>();
            }

            List<Tuple<int, int, int>> ciOrgaModelList = new List<Tuple<int, int, int>>();
            foreach (AffectationSeuilUtilisateurEnt model in listAffectations)
            {
                int? ciId = ciManager.GetCiByOrganisationId(model.OrganisationId)?.CiId;
                if (ciId.HasValue)
                {
                    ciOrgaModelList.Add(Tuple.Create(model.OrganisationId, ciId.Value, model.RoleId));
                }
            }

            return ciOrgaModelList;
        }

        /// <summary>
        /// Get delegation to add list
        /// </summary>
        /// <param name="utilisateurId">Utilisateur id</param>
        /// <param name="ciOrgaModelList">Ci organisation model list</param>
        /// <returns>List de AffectationModel</returns>
        private List<AffectationModel> GetDelegationToAddList(int utilisateurId, IEnumerable<Tuple<int, int, int>> ciOrgaModelList)
        {
            if (ciOrgaModelList.IsNullOrEmpty())
            {
                return new List<AffectationModel>();
            }

            List<AffectationModel> delegationToAddList = new List<AffectationModel>();
            foreach (Tuple<int, int, int> ciOrg in ciOrgaModelList)
            {
                int organisationId = ciOrg.Item1;
                int ciId = ciOrg.Item2;
                int roleId = ciOrg.Item3;
                int? delegueRoleId = roleManager.GetDelegueRoleByOrganisationId(organisationId)?.RoleId;
                if (delegueRoleId.HasValue)
                {
                    IEnumerable<AffectationSeuilUtilisateurEnt> userRoleList = roleManager.GetUtilisateurRoleList(utilisateurId, delegueRoleId.Value);
                    bool isNewDelegation = userRoleList.IsNullOrEmpty() || !userRoleList.Any(u => u.OrganisationId == organisationId);
                    if (isNewDelegation)
                    {
                        bool isDelegue = roleManager.IsRoleDelegue(roleId);
                        delegationToAddList.Add(new AffectationModel { CiId = ciId, PersonnelId = utilisateurId, IsDelegue = isDelegue });
                    }
                }
            }

            return delegationToAddList;
        }

        /// <summary>
        /// Ajouter une affectation
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="startDateOfTheWeek">Date de premier jour de la semaine</param>
        /// <param name="affectationView">Affectation view </param>
        private void AddAffectation(int ciId, DateTime startDateOfTheWeek, AffectationViewEnt affectationView)
        {
            AffectationEnt affectation = this.Repository.GetOrCreateAffectation(affectationView.PersonnelId, ciId, affectationView.IsDelegate);
            AddUpdateAstreints(affectation.AffectationId, startDateOfTheWeek, affectationView.Astreintes);
        }

        /// <summary>
        /// Ajouter des astreintes ou les supprimer
        /// </summary>
        /// <param name="affectationId">Affectation identifier</param>
        /// <param name="startDate">Start Date</param>
        /// <param name="astreintesViewList">List des astreintes view </param>
        private void AddUpdateAstreints(int affectationId, DateTime startDate, IEnumerable<AstreinteViewEnt> astreintesViewList)
        {
            if (astreintesViewList != null && astreintesViewList.Any())
            {
                foreach (AstreinteViewEnt astreinteView in astreintesViewList)
                {
                    if (astreinteView != null && astreinteView.AstreinteId == 0 && astreinteView.IsAstreinte)
                    {
                        this.Repository.AddAstreinte(affectationId, startDate, astreinteView.DayOfWeek);
                        Save();
                        DateTime dateAstreinte = GetAstreinteDate(startDate, astreinteView.DayOfWeek);
                        AjoutPrimeAStreinte(affectationId, dateAstreinte);
                    }

                    if (astreinteView != null && astreinteView.AstreinteId > 0 && !astreinteView.IsAstreinte)
                    {
                        DeletePrimeAstreinte(astreinteView.AstreinteId);
                        this.Repository.UpdateAstreinte(astreinteView.AstreinteId);
                        Save();
                        DateTime dateAstreinte = GetAstreinteDate(startDate, astreinteView.DayOfWeek);
                        DeleteRapportLigneCodePrimeAstreinte(affectationId, dateAstreinte);
                    }
                }
            }
        }

        /// <summary>
        /// Modifier une affectation
        /// </summary>
        /// <param name="startDateOfTheWeek">Date du premier jour de la semaine</param>
        /// <param name="affectationView">Affectatioin view</param>
        private void UpdateAffectation(DateTime startDateOfTheWeek, AffectationViewEnt affectationView)
        {
            AffectationEnt affectation = this.Repository.GetAffectationById(affectationView.AffectationId);
            if (affectation != null)
            {
                this.Repository.UpdateAffectation(affectation, affectationView.IsDelegate);
                Save();
                AddUpdateAstreints(affectation.AffectationId, startDateOfTheWeek, affectationView.Astreintes);
            }
        }

        /// <summary>
        /// Récupérer la date de l'astreinte à partir la date de lundi et le jour de la semaine
        /// </summary>
        /// <param name="mondayDate">Date de lundi</param>
        /// <param name="dayOfWeek">Jour de la semaine</param>
        /// <returns>Date de l'astreinte</returns>
        private DateTime GetAstreinteDate(DateTime mondayDate, int dayOfWeek)
        {
            if (dayOfWeek == 0)
            {
                return mondayDate.AddDays(6);
            }
            else
            {
                return mondayDate.AddDays(dayOfWeek - 1);
            }
        }

        /// <summary>
        /// Get Affectation par l'identifiant
        /// </summary>
        /// <param name="affectationId">Affectation identifier</param>
        /// <returns>Affectation</returns>
        private AffectationEnt GetAffectationbyId(int affectationId)
        {
            return Repository.GetAffectationById(affectationId);
        }

        /// <summary>
        /// Ajout du prime pour les astreintes
        /// </summary>
        /// <param name="affectationId">Affectation identifier</param>
        /// <param name="dateAstreinte">Date astreinte</param>
        private void AjoutPrimeAStreinte(int affectationId, DateTime dateAstreinte)
        {
            AffectationEnt affectation = GetAffectationbyId(affectationId);
            int statusPersonnel = 0;

            if (affectation.Personnel?.Societe?.Groupe?.Code == Constantes.CodeGroupeFES &&
                int.TryParse(affectation.Personnel.Statut, out statusPersonnel)) ;

            RapportEnt rapportEnt = rapportRepository.Get().FirstOrDefault(x => x.CiId == affectation.CiId && x.DateChantier == dateAstreinte && x.TypeStatutRapport == statusPersonnel);
            if (rapportEnt == null)
            {
                HandleCreateRapportLigneForPrimeAstreinte(affectation, dateAstreinte);
            }
            else
            {
                RapportLigneEnt rapportLigneEnt = rapportLigneRepository.Get().FirstOrDefault(x => x.RapportId == rapportEnt.RapportId && x.PersonnelId == affectation.PersonnelId && x.DateSuppression == null);
                if (rapportLigneEnt == null)
                {
                    rapportEnt = HandleUpdateRapportLigneForPrimeAstreint(rapportEnt, affectation, dateAstreinte);
                    rapportRepository.Update(rapportEnt);
                    Save();
                }
                else
                {
                    PrimeEnt primeAstreinte = GetPrimeAstreinteBySociete(dateAstreinte, affectation?.Personnel?.SocieteId ?? 0);
                    RapportLignePrimeEnt rapportLignePrime = new RapportLignePrimeEnt
                    {
                        RapportLigneId = rapportLigneEnt.RapportLigneId,
                        PrimeId = primeAstreinte.PrimeId,
                        IsChecked = true
                    };

                    rapportLignePrimeRepository.Insert(rapportLignePrime);
                    Save();
                }
            }
        }

        /// <summary>
        /// Delete prime d'astreinte
        /// </summary>
        /// <param name="affectationId">Affectation identifier</param>
        /// <param name="dateAstreinte">Date astreinte</param>
        private void DeleteRapportLigneCodePrimeAstreinte(int affectationId, DateTime dateAstreinte)
        {
            AffectationEnt affectation = GetAffectationbyId(affectationId);
            if (affectation != null)
            {
                RapportLigneEnt rapportLigneEnt = pointageRepository.GetpointageByPersonnelAndCiAndDateAstreinte(affectation.PersonnelId, affectation.CiId, dateAstreinte);
                if (rapportLigneEnt != null)
                {
                    PrimeEnt primeAstreinte = GetPrimeAstreinteBySociete(dateAstreinte, rapportLigneEnt.Personnel?.SocieteId ?? 0);
                    RapportLignePrimeEnt rapportLignePrime = rapportLignePrimeRepository.Get().FirstOrDefault(x => x.RapportLigneId == rapportLigneEnt.RapportLigneId && x.PrimeId == primeAstreinte.PrimeId);
                    if (rapportLignePrime != null)
                    {
                        rapportLignePrimeRepository.Delete(rapportLignePrime);
                        Save();
                    }

                    bool isSave = Managers.Pointage.CheckRapportLigneBeforeSave(rapportLigneEnt);
                    if (!isSave)
                    {
                        rapportLigneEnt.DateSuppression = DateTime.Now;
                        rapportLigneEnt.IsDeleted = true;
                        rapportLigneRepository.Update(rapportLigneEnt);

                        valorisationManager.DeleteValorisationFromPointage(rapportLigneEnt);

                        Save();
                    }
                }
            }
        }

        /// <summary>
        /// Delete prime d'astreinte
        /// </summary>
        /// <param name="astreinteId">Affectation identifier</param>
        private void DeletePrimeAstreinte(int astreinteId)
        {
            codeAstreinteRepository.DeletePrimesAstreinteByAstreinteId(astreinteId);
        }

        /// <summary>
        /// Get Prime Astreinte
        /// </summary>
        /// <param name="dateAstreinte">Date affectation d'astreinte</param>
        /// <returns>Prime</returns>
        private PrimeEnt GetPrimeAstreinte(DateTime dateAstreinte)
        {
            string codePrime = GetCodePrime(dateAstreinte);
            return primeRepository.Get().FirstOrDefault(x => x.Code == codePrime);
        }

        /// <summary>
        /// Get Prime Astreinte by societe
        /// </summary>
        /// <param name="dateAstreinte">Date affectation d'astreinte</param>
        /// <param name="societeId">societe identifier</param>
        /// <returns>Prime by siciete</returns>
        private PrimeEnt GetPrimeAstreinteBySociete(DateTime dateAstreinte, int societeId)
        {
            string codePrime = GetCodePrime(dateAstreinte);
            return primeRepository.Get().FirstOrDefault(x => x.Code == codePrime && x.SocieteId == societeId);
        }

        /// <summary>
        /// Get Code Prime
        /// </summary>
        /// <param name="dateAstreinte">Date affectation d'astreinte</param>
        /// <returns>Code Prime</returns>
        private string GetCodePrime(DateTime dateAstreinte)
        {
            return (dateAstreinte.DayOfWeek == DayOfWeek.Sunday || dateAstreinte.DayOfWeek == DayOfWeek.Saturday)
                           ? Constantes.CodePrime.ASTRWE
                           : Constantes.CodePrime.ASTRS;
        }

        /// <summary>
        /// Create Rapport pour affecter les primes astreintes
        /// </summary>
        /// <param name="affectation">Affectation</param>
        /// <param name="dateAstreinte">Date astreinte</param>
        private void HandleCreateRapportLigneForPrimeAstreinte(AffectationEnt affectation, DateTime dateAstreinte)
        {
            int userId = this.userManager.GetContextUtilisateurId();
            RapportEnt rapportEnt = new RapportEnt
            {
                RapportStatutId = RapportStatutEnt.RapportStatutEnCours.Key,
                CiId = affectation.CiId,
                AuteurCreationId = userId,
                DateCreation = DateTime.UtcNow.Date,
                DateChantier = dateAstreinte,
                HoraireDebutM = DateTime.UtcNow.Date.AddHours(8),
                HoraireFinM = DateTime.UtcNow.Date.AddHours(12),
            };
            rapportEnt = RapportStatutHelper.CheckPersonnelStatut(rapportEnt, affectation?.Personnel?.Statut);
            rapportEnt = HandleUpdateRapportLigneForPrimeAstreint(rapportEnt, affectation, dateAstreinte);

            rapportRepository.Insert(rapportEnt);
            Save();
        }

        /// <summary>
        /// Update rapport ligne for prime astreinte
        /// </summary>
        /// <param name="rapportEnt">Rapport</param>
        /// <param name="affectation">Affectation</param>
        /// <param name="dateAstreinte">Date Astreinte</param>
        /// <returns>Rapport entite</returns>
        private RapportEnt HandleUpdateRapportLigneForPrimeAstreint(RapportEnt rapportEnt, AffectationEnt affectation, DateTime dateAstreinte)
        {
            PrimeEnt primeAstreinte = GetPrimeAstreinteBySociete(dateAstreinte, affectation?.Personnel?.SocieteId ?? 0);
            var ligne = new RapportLigneEnt
            {
                CiId = affectation.CiId,
                PersonnelId = affectation.PersonnelId,
                DatePointage = rapportEnt.DateChantier,
                DateCreation = rapportEnt.DateCreation,
                RapportLigneStatutId = RapportStatutEnt.RapportStatutEnCours.Key
            };

            if (ligne.ListRapportLignePrimes == null)
            {
                ligne.ListRapportLignePrimes = new List<RapportLignePrimeEnt>();
            }
            else
            {
                ligne.ListRapportLignePrimes.Add(new RapportLignePrimeEnt
                {
                    PrimeId = primeAstreinte.PrimeId,
                    IsChecked = true
                });
            }

            rapportEnt.ListLignes.Add(ligne);

            return rapportEnt;
        }

        /// <summary>
        /// Vérifier si le personnel a un pointage sur un ci avant de le supprimer
        /// </summary>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <param name="ciId">identifiant de l'affaire</param>
        /// <returns>true or false</returns>
        public bool CheckPersonnelBeforeDelete(int personnelId, int ciId)
        {
            return Managers.Pointage.GetpointageByPersonnelAndCi(personnelId, ciId);
        }

        /// <summary>
        /// Compareer le statut du rapport ligne a statut donné
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="personnelId">Personnel identifier</param>
        /// <param name="datePointage">Date Pointage</param>
        /// <param name="statutCode">Statut code a comparer</param>
        /// <returns>True si le rapport statut egal au statut passer en param</returns>
        private bool CheckRapportLigneStatut(int ciId, int personnelId, DateTime datePointage, string statutCode)
        {
            return Managers.Pointage.CheckRapportLigneStatut(ciId, personnelId, datePointage, statutCode);
        }

        /// <summary>
        /// Instantiate Astreinte view entity
        /// </summary>
        /// <param name="affectation">Affectation entity</param>
        /// <param name="astreinte">Astreinte entity</param>
        /// <returns>Astreinte view entity</returns>
        private AstreinteViewEnt InstantiateViewEnt(AffectationEnt affectation, AstreinteEnt astreinte)
        {
            return new AstreinteViewEnt
            {
                AstreinteId = astreinte.AstreintId,
                DayOfWeek = (int)astreinte.DateAstreinte.DayOfWeek,
                IsRapportLigneVerouille = CheckRapportLigneStatut(affectation.CiId, affectation.PersonnelId, astreinte.DateAstreinte, RapportStatutEnt.RapportStatutVerrouille.Value),
                IsAstreinte = true
            };
        }

        /// <summary>
        /// Handle add or update Affectation List pour le groupe FES
        /// </summary>
        /// <param name="ci">CI entity</param>
        /// <param name="calendarAffectationViewEnt">calendar Affectation View</param>
        private void HandleAddOrUpdateAffectationListFes(CIEnt ci, CalendarAffectationViewEnt calendarAffectationViewEnt)
        {
            if (ci.IsAstreinteActive)
            {
                ManageAffectationList(ci.Organisation?.OrganisationId, calendarAffectationViewEnt);

            }
            else
            {
                ManageAffectationList(ci.Organisation?.OrganisationId, calendarAffectationViewEnt);
                List<AstreinteEnt> astreintesList = this.Repository.Get().Where(x => x.CiId == ci.CiId).SelectMany(x => x.Astreintes).ToList();
                foreach (AstreinteEnt astreinte in astreintesList)
                {
                    DeletePrimeAstreinte(astreinte.AstreintId);
                }

                Repository.DeleteAstreintes(astreintesList);
                Save();
            }
        }

        /// <summary>
        /// Handle delete affectation pour le groupe RZB
        /// </summary>
        /// <param name="calendarAffectationViewEnt">calendar Affectation View</param>
        private void HandleAffectationListRzb(CalendarAffectationViewEnt calendarAffectationViewEnt)
        {
            List<int> affectationIdListToDelete = new List<int>();
            foreach (AffectationViewEnt affectationView in calendarAffectationViewEnt.AffectationList)
            {
                if (affectationView?.AffectationId == 0)
                {
                    AddAffectation(calendarAffectationViewEnt.CiId, calendarAffectationViewEnt.StartDateOfTheWeek, affectationView);
                }

                if (affectationView?.AffectationId > 0 && affectationView.IsDelete)
                {
                    affectationIdListToDelete.Add(affectationView.AffectationId);
                }
            }

            this.Repository.DeleteAffectations(affectationIdListToDelete);
            Save();
        }

        #endregion
    }
}
