using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.FonctionnaliteDesactive;
using Fred.Business.Organisation.Tree;
using Fred.Business.PermissionFonctionnalite;
using Fred.Business.RoleFonctionnalite;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Affectation;
using Fred.Entities.CI;
using Fred.Entities.Fonctionnalite;
using Fred.Entities.Organisation;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Permission;
using Fred.Entities.Referential;
using Fred.Entities.Role;
using Fred.Entities.RoleFonctionnalite;
using Fred.Entities.Utilisateur;
using Fred.Framework;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.Framework.Security;
using Fred.Web.Shared.Comparer;
using Fred.Web.Shared.Extentions;
using static Fred.Entities.Constantes;
using Shared = Fred.Web.Shared;

namespace Fred.Business.Utilisateur
{
    public class UtilisateurManager : Manager<UtilisateurEnt, IUtilisateurRepository>, IUtilisateurManager
    {
        /// <summary>
        ///   Constante des users CI
        /// </summary>
        private const string KeyUserCIs = "UserCIs";

        /// <summary>
        ///   Constante des users CI et Organisation pere
        /// </summary>
        private const string KeyUserCIsWithOrgPere = "UserCIsOrgPere";

        private readonly ICacheManager cacheManager;
        private readonly IAffectationSeuilUtilisateurRepository repoAffectation;
        private readonly ICIRepository repoCi;
        private readonly IOrganisationRepository repoOrga;
        private readonly IUtilisateurRepository utilisateurRepository;
        private readonly ISecurityManager secMgr;
        private readonly IRoleRepository repoRole;
        private readonly IExternalDirectoryRepository externalDirectoryRepo;
        private readonly ISeuilValidationRepository seuilValidationRepository;
        private readonly IRoleFonctionnaliteManager roleFonctionnaliteManager;
        private readonly IPermissionFonctionnaliteManager permissionFonctionnaliteManager;
        private readonly IFonctionnaliteDesactiveManager fonctionnaliteDesactiveManager;
        private readonly IOrganisationTreeService organisationTreeService;

        public UtilisateurManager(
            IUnitOfWork uow,
            IUtilisateurRepository utilisateurRepository,
            ISecurityManager securityManager,
            ICacheManager cacheManager,
            IRoleFonctionnaliteManager roleFonctionnaliteManager,
            IPermissionFonctionnaliteManager permissionFonctionnaliteManager,
            IFonctionnaliteDesactiveManager fonctionnaliteDesactiveManager,
            IOrganisationTreeService organisationTreeService,
            IOrganisationRepository repoOrga,
            ICIRepository repoCi,
            IAffectationSeuilUtilisateurRepository repoAffectation,
            IRoleRepository repoRole,
            IExternalDirectoryRepository externalDirectoryRepo,
            ISeuilValidationRepository seuilValidationRepository)
          : base(uow, utilisateurRepository)
        {
            this.utilisateurRepository = utilisateurRepository;
            secMgr = securityManager;
            this.repoOrga = repoOrga;
            this.repoCi = repoCi;
            this.repoAffectation = repoAffectation;
            this.repoRole = repoRole;
            this.externalDirectoryRepo = externalDirectoryRepo;
            this.seuilValidationRepository = seuilValidationRepository;
            this.cacheManager = cacheManager;
            this.roleFonctionnaliteManager = roleFonctionnaliteManager;
            this.permissionFonctionnaliteManager = permissionFonctionnaliteManager;
            this.fonctionnaliteDesactiveManager = fonctionnaliteDesactiveManager;
            this.organisationTreeService = organisationTreeService;
        }

        /// <inheritdoc />
        public IEnumerable<UtilisateurEnt> GetList()
        {
            return Repository.GetList();
        }

        /// <inheritdoc />
        public IEnumerable<UtilisateurEnt> GetListSync()
        {
            return Repository.GetListSync();
        }

        /// <summary>
        ///   Retourne l'utilisateur en fonction du Personnel ID.
        /// </summary>
        /// <param name="login">Identifiant du société à retrouver.</param>
        /// <returns>Le société retrouvée, sinon nulle.</returns>
        public UtilisateurEnt GetByLogin(string login)
        {
            return Repository.GetByLogin(login);
        }

        /// <summary>
        ///   Retourne l'utilisateur en fonction de son login.
        /// </summary>
        /// <param name="login">Login de l'utilisateur</param>
        /// <returns>L'utilisateur retrouvé en fonction de son login pour la vue ResetPassword</returns>
        public UtilisateurEnt GetByLoginForResetPassword(string login)
        {
            return Repository.GetByLoginForResetPassword(login);
        }

        /// <summary>
        ///   Retourne la liste des utilisateurs.
        /// </summary>
        /// <returns>Liste des utilisateurs.</returns>
        public IEnumerable<UtilisateurEnt> GetUtilisateurList()
        {
            return Repository.GetUtilisateurList() ?? new UtilisateurEnt[] { };
        }

        /// <summary>
        ///   Retourne la fiche utilisateur dont portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="utilisateurId">Identifiant du pays à retrouver.</param>
        /// <returns>L'utilisateur retrouvé, sinon vide</returns>
        [Obsolete("Prefer use GetUtilisateurByIdAsync instead")]
        public UtilisateurEnt GetUtilisateurById(int utilisateurId)
        {
            return utilisateurRepository.GetUtilisateurById(utilisateurId);
        }

        /// <summary>
        ///   Retourne la fiche utilisateur dont portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="utilisateurId">Identifiant du pays à retrouver.</param>
        /// <returns>L'utilisateur retrouvé, sinon vide</returns>
        public async Task<UtilisateurEnt> GetUtilisateurByIdAsync(int utilisateurId)
        {
            return await utilisateurRepository.GetUtilisateurByIdAsync(utilisateurId);
        }

        /// <inheritdoc />
        public UtilisateurEnt AddUtilisateur(UtilisateurEnt utilisateurEnt)
        {
            // Validation utilisateur (NB: validation utilisateur lors d'ajout d'un personnel en tant qu'utilisateur : check mot de passe, date expiration, folio)
            Repository.Insert(utilisateurEnt);
            Save();

            return utilisateurEnt;
        }

        // <summary>
        /// Modifie une liste d'utlidateur
        /// </summary>
        /// <param name="utilisateurList">Liste d'utilisateur à modifier</param>
        /// <returns></returns>
        public void UpdateUtilisateurList(IEnumerable<UtilisateurEnt> utilisateurList)
        {
            Repository.UpdateUtilisateurList(utilisateurList);
            Save();
        }

        /// <inheritdoc />
        public UtilisateurEnt UpdateUtilisateur(UtilisateurEnt utilisateurEnt)
        {
            if (utilisateurEnt == null)
            {
                throw new FredBusinessException("Utilisateur inconnu.");
            }

            // Gros fix dégueu suite à une erreur de conception : la date d'expiration est un datetime en base, et pas un datetime2
            if (utilisateurEnt.ExternalDirectory?.DateExpiration.HasValue == true
              && utilisateurEnt.ExternalDirectory.DateExpiration.Value.Date == DateTime.MinValue.Date)
            {
                utilisateurEnt.ExternalDirectory.DateExpiration = null;
            }

            if (utilisateurEnt.ExternalDirectory != null
                && !string.IsNullOrEmpty(utilisateurEnt.ExternalDirectory.MotDePasse)
                && (!utilisateurEnt.ExternalDirectory.DateExpiration.HasValue
                  || utilisateurEnt.ExternalDirectory.DateExpiration.Value.Date != DateTime.MinValue.Date))
            {
                utilisateurEnt.ExternalDirectory.FayatAccessDirectoryId = utilisateurEnt.FayatAccessDirectoryId ?? 0;
            }

            var utilisateurId = GetContextUtilisateurId();
            if (utilisateurId != 0)
            {
                utilisateurEnt.UtilisateurIdModification = utilisateurId;
            }
            else
            {
                utilisateurEnt.UtilisateurIdModification = GetByLogin("fred_ie")?.UtilisateurId;
            }

            utilisateurEnt.DateModification = DateTime.UtcNow;
            utilisateurEnt.Personnel = null;
            Repository.Update(utilisateurEnt);

            if (utilisateurEnt.ExternalDirectory != null)
            {
                externalDirectoryRepo.Update(utilisateurEnt.ExternalDirectory);
            }

            Save();

            return utilisateurEnt;
        }

        /// <inheritdoc />
        public bool UpdateDateDerniereConnexion(int utilisateurId, DateTime dateDerniereCo)
        {
            UtilisateurEnt user = Repository.FindById(utilisateurId);

            if (user != null)
            {
                user.DateDerniereConnexion = dateDerniereCo;
                Repository.Update(user);
                Save();
                return true;
            }

            return false;
        }

        /// <inheritdoc />  
        public void DeleteUtilisateurById(int utilisateurId)
        {
            UtilisateurEnt utilisateur = FindById(utilisateurId);

            if (utilisateur != null)
            {
                utilisateur.IsDeleted = true;
                utilisateur.DateSupression = DateTime.UtcNow;
                utilisateur.UtilisateurIdSupression = GetContextUtilisateurId();
                Repository.DeleteUtilisateur(utilisateur);
                Save();
            }
            else
            {
                throw new FredBusinessException("Impossible de supprimer cet utilisateur : Utilisateur introuvable.");
            }
        }

        /// <summary>
        ///   Obtient  l'ID de l'utilisateur en cours dans le contexte Claims d'habilitation
        /// </summary>
        /// <returns>Identifiant de l'utilisateur</returns>
        public int GetContextUtilisateurId()
        {
            int utilisateurId = 0;
            try
            {
                utilisateurId = this.secMgr.GetUtilisateurId();
            }
            catch (Exception)
            {
                return utilisateurId;
            }

            return utilisateurId;
        }

        /// <summary>
        ///   Obtient le détail d'un utilisateur en cours dans le contexte Claims d'habilitation
        /// </summary>
        /// <returns>Détail d'un utilisateur</returns>
        [Obsolete("Prefer use GetContextUtilisateurAsync instead")]
        public UtilisateurEnt GetContextUtilisateur()
        {
            return GetUtilisateurById(GetContextUtilisateurId());
        }

        public async Task<UtilisateurEnt> GetContextUtilisateurAsync()
        {
            return await GetUtilisateurByIdAsync(GetContextUtilisateurId());
        }

        /// <summary>
        ///   Retourne l'utilisateur dont portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="id">Identifiant du UtilisateurFredId.</param>
        /// <param name="asNoTracking">Sans tracking par défaut à oui</param>
        /// <returns>Le détail utilisateur Fred  retrouvée, sinon nulle.</returns>
        public UtilisateurEnt GetById(int id, bool asNoTracking = false)
        {
            if (!asNoTracking)
            {
                return Repository.GetById(id);
            }
            else
            {
                return Repository.GetByIdAsNoTracking(id);
            }
        }

        /// <summary>
        /// Vérifier si un personnel est un utilisateur Fred
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <returns>Boolean indique si le personnel est un utilisateur Fred</returns>
        public bool IsFredUser(int personnelId)
        {
            return Repository.IsFredUser(personnelId);
        }

        /// <summary>
        ///   Retourne le plus haut niveau de paie d'un utilisateur
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="orgaId">Identifiant de l'organisation</param>
        /// <returns>Le plus haut niveau de paie de l'utilisateur parmis ces rôles</returns>
        public int GetHigherPaieLevel(int userId, int? orgaId)
        {
            int? niveauPaieMax = null;
            List<RoleEnt> listRoles;
            if (orgaId.HasValue)
            {
                var organisationTree = organisationTreeService.GetOrganisationTree();

                var parentOrgaIdList = organisationTree.GetParentsWithCurrent(orgaId.Value)
                                                        .Select(x => x.OrganisationId);

                listRoles = this.repoAffectation.Query()
                    .Filter(a => a.UtilisateurId == userId)
                    .Filter(a => parentOrgaIdList.Contains(a.OrganisationId))
                    .Get()
                    .Select(r => r.Role)
                    .ToList();
            }
            else
            {
                listRoles = this.repoAffectation.Query()
                    .Filter(a => a.UtilisateurId == userId)
                    .Get()
                    .Select(r => r.Role)
                    .ToList();
            }

            if (listRoles?.Any() == true)
            {
                niveauPaieMax = listRoles.Max(role => role.NiveauPaie);
            }

            return niveauPaieMax ?? 0;
        }

        /// <summary>
        ///   Retourne le plus haut niveau de paie d'un utilisateur pour chaque organisationId
        /// </summary>
        /// <remarks>
        /// 3 appels à la BD :
        ///     - Récupération des TypeOrganisation
        ///     - proc stock GetOrganisations
        ///     - requête dans FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE
        /// </remarks>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="ciOrgaIds">Liste d'identifiants d'organisation de CI</param>
        /// <returns>Dictionnaire (organisationId, maxNiveauPaie)</returns>
        public Dictionary<int, int?> GetHigherPaieLevelByOrganisationIdList(int userId, List<int> ciOrgaIds)
        {
            ciOrgaIds = ciOrgaIds.Distinct().ToList();
            Dictionary<int, int?> dico = new Dictionary<int, int?>();
            List<int> allOrgaIds = new List<int>();
            List<int> parentOrgaIds = null;
            List<RoleEnt> roles;
            List<AffectationSeuilUtilisateurEnt> allUserRoleOrgaDevise;
            Dictionary<int, List<int>> ciOrgaIdOrgaParentId = new Dictionary<int, List<int>>();
            int? max = null;

            // Récupération de toutes les organisations de CI issues de la liste 'ciOrgaIds'
            int ciTypeOrgaId = repoOrga.GetTypeOrganisationIdByCode(Constantes.OrganisationType.CodeCi);
            List<OrganisationLightEnt> allUserCiOrga = repoOrga.GetOrganisationsAvailable(utilisateurId: userId, types: new List<int> { ciTypeOrgaId }).ToList();

            // Parcours la liste des CI Orga
            // Pour chaque CI, on créé un dico (int CiId, List<int> OrgaParentId)
            foreach (OrganisationLightEnt orga in allUserCiOrga.Where(x => ciOrgaIds.Contains(x.OrganisationId) && !string.IsNullOrEmpty(x.IdParents)).ToList())
            {
                // Split de chaque chaine de caractère IdParents et transformation en liste d'entier
                parentOrgaIds = orga.IdParents.Split('|').Select(int.Parse).ToList();

                ciOrgaIdOrgaParentId.Add(orga.OrganisationId, parentOrgaIds);

                allOrgaIds.AddRange(parentOrgaIds);
            }

            allOrgaIds.AddRange(ciOrgaIds);
            allOrgaIds = allOrgaIds.Distinct().ToList();

            allUserRoleOrgaDevise = repoAffectation.Query()
                                                   .Include(x => x.Role)
                                                   .Filter(a => a.UtilisateurId == userId && allOrgaIds.Contains(a.OrganisationId))
                                                   .Get()
                                                   .ToList();

            foreach (int orgaId in ciOrgaIds)
            {
                List<int> result;
                if (ciOrgaIdOrgaParentId.TryGetValue(orgaId, out result))
                {
                    roles = allUserRoleOrgaDevise.Where(x => x.OrganisationId == orgaId || (ciOrgaIdOrgaParentId[orgaId]?.Contains(x.OrganisationId) == true))?.Select(x => x.Role)?.ToList();
                    if (!roles.IsNullOrEmpty())
                    {
                        max = roles?.Count > 0 ? roles.Max(x => x.NiveauPaie) : 0;
                        dico.Add(orgaId, max);
                    }
                }
            }

            return dico;
        }

        /// <summary>
        ///   Renvoi la liste des affectations de rôle pour un utilisateur regroupement par role ensuite par orga
        /// </summary>
        /// <param name="utilisateurId">L'identifiant utilisateur.</param>
        /// <returns>Liste de roles</returns>
        public IEnumerable<RoleEnt> GetRoleOrganisationAffectations(int utilisateurId)
        {
            IEnumerable<AffectationSeuilUtilisateurEnt> affectSeulUtilisateursQuery = repoAffectation.GetAffectationSeuilUtilisateursForDetailPersonnel(utilisateurId);

            //Regroupement par role et par organisation
            var listAffectationsGroupByRoleQuery =
              affectSeulUtilisateursQuery.GroupBy(grouping => grouping.Role)
                                         .Select(grouping => new
                                         {
                                             Role = grouping.Key,
                                             grouping.Key.SeuilsValidation,
                                             OrganisationAffectations = grouping.GroupBy(affectationSeuilUtilisateur => affectationSeuilUtilisateur.Organisation)
                                             .Select(sousgrouping => new AffectationGroupByOrganisationEnt
                                             {
                                                 Organisation = sousgrouping.Key,
                                                 Affectations = sousgrouping.Where(affectationSeuilUtilisateur => affectationSeuilUtilisateur.DeviseId.HasValue)
                                                                            .OrderBy(affectationSeuilUtilisateur => affectationSeuilUtilisateur.Devise.Libelle).ToArray()
                                             })
                                         })
                                         .OrderBy(grouping => grouping.Role.Libelle);

            //Parcourir le résultat pour charger les données additionnelles des organisations (ci, société, groupe...etc.)
            foreach (var groupe in listAffectationsGroupByRoleQuery.ToList())
            {
                var role = groupe.Role;
                role.SeuilsValidation = groupe.SeuilsValidation;
                role.AffectationsByOrganisation = groupe.OrganisationAffectations.ToList();
                yield return role;
            }
        }

        /// <summary>
        ///   Met à jour les rôle d'un Utilisateur
        /// </summary>
        /// <param name="utilisateurId">Id de l'Utilisateur à mettre à jour</param>
        /// <param name="listAffectations">liste des affectations de l'utilisateur</param>
        public void UpdateRole(int utilisateurId, IEnumerable<AffectationSeuilUtilisateurEnt> listAffectations)
        {
            Managers.Affectation.UpdateDelegueRoleAffectation(utilisateurId, listAffectations);
            Repository.UpdateRole(utilisateurId, listAffectations);
            cacheManager.Remove(KeyUserCIs + utilisateurId);
            cacheManager.Remove(OrganisationTreeCacheKey.FredOrganisationTreeCacheKey);
            cacheManager.GetOrCreate(KeyUserCIs + utilisateurId, () => GetCIs(utilisateurId), new TimeSpan(1, 0, 0, 0, 0));
        }

        /// <summary>
        /// Add or remove role delegue for ci personnel
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation du CI</param>
        /// <param name="utilisateurIdLisToAdd">Utilisateur id list to add to the Ci delegation</param>
        /// <param name="utilisateurIdListToRemove">Utilisateur id list to remove from Ci delegation</param>
        public void ManageRoleDelegueForCiPersonnel(
            int organisationId,
            IEnumerable<int> utilisateurIdLisToAdd,
            IEnumerable<int> utilisateurIdListToRemove)
        {
            var organisationTree = this.organisationTreeService.GetOrganisationTree();

            int societeId = organisationTree.GetSocieteParent(organisationId).Id;

            RoleEnt delegueCiRole = repoRole.GetRoleListBySocieteId(societeId)?.FirstOrDefault(r =>
                (r.RoleSpecification.HasValue && r.RoleSpecification.Value == RoleSpecification.Delegue)
                || r.Code == CodeRole.CodeRoleDelegueCI || r.CodeNomFamilier == CodeRole.CodeRoleDelegueCI);
            if (delegueCiRole == null)
            {
                return;
            }

            int delegueRoleId = delegueCiRole.RoleId;
            if (utilisateurIdLisToAdd?.Any() == true)
            {
                Dictionary<int, List<AffectationSeuilUtilisateurEnt>> affectationsByUser =
                    utilisateurIdLisToAdd.ToDictionary(
                        u => u,
                        u => new List<AffectationSeuilUtilisateurEnt>
                        {
                            new AffectationSeuilUtilisateurEnt
                            {
                                UtilisateurId = u,
                                RoleId = delegueRoleId,
                                OrganisationId = organisationId
                            }
                        });

                Repository.UpdateRoleForUtilisateurList(affectationsByUser);
                utilisateurIdLisToAdd.ForEach(uId => this.cacheManager.Remove(KeyUserCIs + uId));
                this.cacheManager.Remove(OrganisationTreeCacheKey.FredOrganisationTreeCacheKey);
            }

            if (utilisateurIdListToRemove?.Any() == true)
            {
                Repository.RemoveRoleAffectationForCiUserList(utilisateurIdListToRemove, organisationId, delegueRoleId);
                Save();
            }
        }

        /// <summary>
        ///   Retourne le seuil du montant de la commande pour un utilisateur donné
        /// </summary>
        /// <param name="userId"> Identifiant de l'utilisateur </param>
        /// <param name="ciId"> Identifiant du CI</param>
        /// <param name="deviseId"> Identifiant de la devise</param>
        /// <returns> Le seuil de validation de l'utilisateur sur cette commande </returns>
        public decimal GetSeuilValidation(int userId, int ciId, int deviseId)
        {
            CIEnt ci = this.repoCi.Get(ciId);
            OrganisationEnt organisation = ci.Organisation;
            List<AffectationSeuilUtilisateurEnt> affectations = Repository.GetAffectationRoles(userId).ToList();

            if (organisation != null)
            {
                int organisationId = organisation.OrganisationId;

                // 1 - On récupère le seuil lié à la personne qui veut faire la commande avec son rôle dans l'organisation du CI
                var seuil = GetSeuilValidationNiveauUtilisateur(userId, organisationId, deviseId, affectations);

                // 2 - On récupère le seuil lié au ROLE dans l'organisation du C.I
                if (!seuil.HasValue)
                {
                    // Récupération des organisations parents du CI (jusqu'à Société)
                    var organisationTree = this.organisationTreeService.GetOrganisationTree();

                    var ciOrgaParentList = organisationTree.GetParentsWithCurrentUntilSociete(organisationId).Select(o => o.OrganisationId).ToList();

                    // Liste des rôles de l'utilisateur triés selon le type de l'organisation (ordre décroissant car type organisation CI = 8)
                    // NB : On prend toujours le rôle le plus proche du CI au niveau de l'organisation associé à ce rôle.
                    var eligibleRoleList = affectations
                                            .Where(x => ciOrgaParentList.Contains(x.OrganisationId))
                                            .OrderByDescending(x => x.Organisation.TypeOrganisationId)
                                            .ToList();

                    var iterator = eligibleRoleList.GetEnumerator();

                    while (iterator.MoveNext() && !seuil.HasValue)
                    {
                        var item = iterator.Current;
                        seuil = GetSeuilValidationNiveauOrganisation(organisationId, item.RoleId, deviseId);
                    }

                    // 3 - On récupère le seuil PAR DEFAUT du rôle
                    if (!seuil.HasValue)
                    {
                        iterator = eligibleRoleList.GetEnumerator();

                        while (iterator.MoveNext() && !seuil.HasValue)
                        {
                            var item = iterator.Current;
                            seuil = GetSeuilValidationNiveauRole(item.RoleId, deviseId);
                        }
                    }
                }
                return seuil ?? 0;
            }
            return 0;
        }

        /// <summary>
        ///   Retourne le seuil pour l'utilisateur en cours (Niveau 1)
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="organisationId">Identifiant de l'organisation</param>
        /// <param name="deviseId">Identifiant de la devise</param>
        /// <param name="affectations">Liste des affectation de l'utilisateur</param>
        /// <returns>le seuil d'un utilisateur</returns>
        /// <remarks>UN utilisateur a UN et UN seul Rôle pour UNE Organisation</remarks>
        private decimal? GetSeuilValidationNiveauUtilisateur(int userId, int organisationId, int deviseId, IEnumerable<AffectationSeuilUtilisateurEnt> affectations)
        {
            AffectationSeuilUtilisateurEnt affectation = affectations.FirstOrDefault(aff => aff.OrganisationId == organisationId && aff.DeviseId == deviseId);

            if (affectation?.CommandeSeuil.HasValue == true)
            {
                return Convert.ToInt32(affectation.CommandeSeuil.Value);
            }
            else
            {
                OrganisationEnt organisation = this.repoOrga.FindById(organisationId);
                if (organisation.PereId.HasValue)
                {
                    return GetSeuilValidationNiveauUtilisateur(userId, organisation.PereId.Value, deviseId, affectations);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        ///   Retourne le seuil au niveau de l'organisation (Niveau 2)
        /// </summary>
        /// <param name="organisationId"> Identifiant de l'organisation </param>
        /// <param name="roleId">Identifiant du rôle</param>
        /// <param name="deviseId"> Identifiant de la devise </param>    
        /// <returns> Le seuil d'un utilisateur </returns>
        private decimal? GetSeuilValidationNiveauOrganisation(int organisationId, int roleId, int deviseId)
        {
            IEnumerable<AffectationSeuilOrgaEnt> roleOrgaDeviseList = this.repoOrga.GetSeuilByOrganisationId(organisationId, roleId);

            AffectationSeuilOrgaEnt roleOrgaDevise = roleOrgaDeviseList.FirstOrDefault(x => x.DeviseId == deviseId && x.OrganisationId == organisationId);

            if (roleOrgaDevise != null)
            {
                return Convert.ToInt32(roleOrgaDevise.Seuil);
            }
            else
            {
                OrganisationEnt organisation = this.repoOrga.FindById(organisationId);
                if (organisation.PereId.HasValue)
                {
                    return GetSeuilValidationNiveauOrganisation(organisation.PereId.Value, roleId, deviseId);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        ///   Retourne le seuil du rôle (Niveau 3)
        /// </summary>
        /// <param name="roleId"> Identifiant du rôle </param>
        /// <param name="deviseId"> Identifiant de la devise </param>
        /// <returns> Le seuil d'un utilisateur </returns>
        private decimal? GetSeuilValidationNiveauRole(int roleId, int deviseId)
        {
            IEnumerable<SeuilValidationEnt> roleDeviseList = seuilValidationRepository.Get().ToList();

            if (roleDeviseList.Any())
            {
                return roleDeviseList.FirstOrDefault(x => x.RoleId == roleId && x.DeviseId == deviseId)?.Montant ?? null;
            }
            return null;
        }

        /// <summary>
        ///   Renvoi la liste des organisations d'un Utilisateur pour un type d'orgnisation
        /// </summary>
        /// <param name="utilisateurId">Id de l'Utilisateur</param>
        /// <param name="typeOrganisationId">Id du type d'organisation</param>
        /// <returns>Une liste d'organisations</returns>
        public IEnumerable<OrganisationLightEnt> GetOrganisationAvailableByUserAndByTypeOrganisation(int utilisateurId, int typeOrganisationId)
        {
            return repoOrga.GetOrganisationsAvailable(null, new List<int> { typeOrganisationId }, utilisateurId);
        }

        /// <summary>
        ///   Retourne la liste des roles pour l'utilisateur spécifié
        /// </summary>
        /// <param name="userId">Optionnel : identifiant de l'utilisateur ; par défaut l'utilisateur connecté</param>
        /// <param name="asNoTracking">Sans tracking par défaut à oui</param>
        /// <returns>Une liste de roles</returns>
        private IEnumerable<RoleEnt> GetUserRoles(int? userId = null, bool asNoTracking = false)
        {
            if (!userId.HasValue) { userId = GetContextUtilisateurId(); }
            UtilisateurEnt user = GetById(userId.Value, asNoTracking);
            return user.AffectationsRole.Select(o => o.Role).ToList();
        }

        /// <summary>
        ///   Renvoi vrai si l'utilisateur connecté a au moins le niveau du role définit
        /// </summary>
        /// <param name="niveauPaie"> Le niveau de paie </param>
        /// <returns> Renvoi vrai si l'utilisateur a au moins le niveau du role définit</returns>
        public bool HasAtLeastThisPaieLevel(int niveauPaie)
        {
            return GetUserRoles().Any(o => o.NiveauPaie >= niveauPaie);
        }

        /// <summary>
        ///   Retourne la liste CI du utilisateur et organisation Pere 
        /// </summary>
        /// <param name="userid">Identifiant de l'utilisateur</param>
        /// <param name="force">Force le rafraichissement de la liste des CI de l'utilisateur dans le cache</param>
        /// /// <param name="organisationPere">organisation Pere</param>
        /// <returns>Renvoie la liste des CI du utilisateur.</returns>
        [Obsolete("Prefer to use " + nameof(GetAllCIbyUserAsync) + " instead")]
        public IEnumerable<int> GetAllCIbyUser(int userid, bool force = false, int? organisationPere = null)
        {
            string cachekey = organisationPere == null ? KeyUserCIs + userid : KeyUserCIsWithOrgPere + userid + "_" + organisationPere;
            IEnumerable<int> cisCache = cacheManager.GetOrCreate(cachekey, () => null as IEnumerable<int>, new TimeSpan(1, 0, 0, 0, 0));

            if (force || cisCache == null)
            {
                cacheManager.Remove(cachekey);
                cisCache = cacheManager.GetOrCreate(cachekey, () => GetCIs(userid, organisationPere), new TimeSpan(1, 0, 0, 0, 0));
            }

            return cisCache;
        }

        [Obsolete("Prefer to use " + nameof(GetCIsAsync) + " instead")]
        private IEnumerable<int> GetCIs(int userId, int? organisationPere = null)
        {
            int idTypeCi = repoOrga.GetTypeOrganisationIdByCode(Constantes.OrganisationType.CodeCi);
            int idTypeSousCi = repoOrga.GetTypeOrganisationIdByCode(Constantes.OrganisationType.CodeSousCi);
            IEnumerable<int> orgaLightList = repoOrga.GetOrganisationsAvailable(null, new List<int> { idTypeCi, idTypeSousCi }, userId, organisationPere).Select(x => x.OrganisationId);
            return repoCi.GetCIIdListByOrganisationId(orgaLightList);
        }

        /// <summary>
        ///   Retourne la liste CI du utilisateur et organisation Pere de manière asynchrone
        /// </summary>
        /// <param name="userid">Identifiant de l'utilisateur</param>
        /// <param name="force">Force le rafraichissement de la liste des CI de l'utilisateur dans le cache</param>
        /// /// <param name="organisationPere">organisation Pere</param>
        /// <returns>Renvoie la liste des CI du utilisateur.</returns>
        public async Task<IEnumerable<int>> GetAllCIbyUserAsync(int userid, bool force = false, int? organisationPere = null)
        {
            string cachekey = organisationPere == null ? KeyUserCIs + userid : KeyUserCIsWithOrgPere + userid + "_" + organisationPere;
            var slidingExpiration = new TimeSpan(1, 0, 0, 0, 0);

            if (force)
            {
                return await ReplaceAsync();
            }

            IEnumerable<int> ciIds = cacheManager.GetOrCreate(cachekey, () => null as IEnumerable<int>, slidingExpiration);
            if (ciIds != null)
                return ciIds;

            return await ReplaceAsync();

            async Task<IEnumerable<int>> ReplaceAsync()
            {
                cacheManager.Remove(cachekey);

                return await cacheManager.GetOrCreateAsync(cachekey, async () => await GetCIsAsync(userid, organisationPere), slidingExpiration);
            }
        }

        private async Task<IEnumerable<int>> GetCIsAsync(int userId, int? organisationPere)
        {
            int idTypeCi = await repoOrga.GetTypeOrganisationIdByCodeAsync(Constantes.OrganisationType.CodeCi);
            int idTypeSousCi = await repoOrga.GetTypeOrganisationIdByCodeAsync(Constantes.OrganisationType.CodeSousCi);

            IEnumerable<OrganisationLightEnt> orgaLightList = repoOrga.GetOrganisationsAvailable(null, new List<int> { idTypeCi, idTypeSousCi }, userId, organisationPere);
            IEnumerable<int> organisationIds = orgaLightList.Select(x => x.OrganisationId);

            return await repoCi.GetCIIdListByOrganisationIdAsync(organisationIds);
        }

        /// <summary>
        ///   Retourne la liste CI du profil paie selon l'organisation choisie
        /// </summary>
        /// <param name="organisationId">OrganisationId choisie</param>
        /// <returns>Renvoie la liste des CI choisie par profil paie.</returns>
        public IEnumerable<int> GetAllCIIdbyOrganisation(int organisationId)
        {
            int idTypeCi = repoOrga.GetTypeOrganisationIdByCode(Constantes.OrganisationType.CodeCi);
            int idTypeSousCi = repoOrga.GetTypeOrganisationIdByCode(Constantes.OrganisationType.CodeSousCi);
            IEnumerable<int> orgaLightList = repoOrga.GetOrganisationsAvailable(types: new List<int> { idTypeCi, idTypeSousCi }, organisationIdPere: organisationId).Select(x => x.OrganisationId);
            return repoCi.GetCIIdListByOrganisationId(orgaLightList);
        }

        public bool DoesFolioExist(int userId, string folio, int userCompanyId)
        {
            return Repository.DoesFolioExist(userId, folio, userCompanyId);
        }

        /// <summary>
        /// Retourne le niveau de paie de l'utilisateur courant pour un CI donné ou général
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Le niveau de paie de utilisateur</returns>
        public int GetUserPaieLevel(int userId, int? ciId)
        {
            if (ciId.HasValue && ciId.Value > 0)
            {
                CIEnt ci = this.repoCi.Get(ciId.Value);
                return GetHigherPaieLevel(userId, ci.Organisation.OrganisationId);
            }
            else
            {
                return GetHigherPaieLevel(userId, null);
            }
        }

        /// <summary>
        ///   Retourne si l'utulisateur fait parti de la paie ou non
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne Vrai si l'utilisateur courant possède un rôle dans la Paie, sinon nulle.</returns>
        public bool IsRolePaie(int userId, int? ciId)
        {
            UtilisateurEnt currentUser = this.Repository.GetUtilisateurById(userId);
            CIEnt ci = null;
            if (ciId.HasValue && ciId.Value > 0)
            {
                ci = this.repoCi.Get(ciId.Value);
            }

            if (!currentUser.Personnel.Societe.Groupe.Code.Equals(Constantes.CodeGroupeFES))
            {
                return GetHigherPaieLevel(userId, ci?.Organisation?.OrganisationId) >= Constantes.NiveauPaie.LevelCSP;
            }

            return IsRoleGSPWithoutConsideringPaieLevel(userId, ci?.Organisation?.OrganisationId);
        }

        /// <summary>
        ///   Retourne un dictionnaire (ciId, isRolePaie) déterminant pour chaque CI si l'utilisateur userId est RolePaie ou non
        /// </summary>
        /// <remarks>
        /// 4 appels à la BD :
        ///     - GetOrganisationIdByCiId
        ///     - 3 appels dans GetHigherPaieLevelByOrganisationIdList
        /// </remarks>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="ciIds">Liste d'identifiants de CI</param>
        /// <returns>Dictionnaire (ciId, isRolePai)</returns>
        public Dictionary<int, bool> IsRolePaie(int userId, IEnumerable<int> ciIds)
        {
            Dictionary<int, bool> dico = new Dictionary<int, bool>();
            Dictionary<int, int?> ciIdOrgaIds = repoCi.GetOrganisationIdByCiId(ciIds);
            List<int> orgaIds = ciIdOrgaIds.Where(x => x.Value.HasValue).Select(x => x.Value.Value).ToList();
            Dictionary<int, int?> orgaIdNiveauPaieMax = GetHigherPaieLevelByOrganisationIdList(userId, orgaIds);

            foreach (KeyValuePair<int, int?> ciIdOrgaId in ciIdOrgaIds)
            {
                int? result = 0;
                if (ciIdOrgaId.Value.HasValue && orgaIdNiveauPaieMax.TryGetValue(ciIdOrgaId.Value.Value, out result))
                {
                    var niveauPaie = orgaIdNiveauPaieMax[ciIdOrgaId.Value.Value];
                    dico.Add(ciIdOrgaId.Key, niveauPaie >= Constantes.NiveauPaie.LevelCSP);
                }
                else
                {
                    dico.Add(ciIdOrgaId.Key, false);
                }
            }
            return dico;
        }

        /// <summary>
        ///   Retourne si l'utulisateur fait parti de la paie ou non
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne Vrai si l'utilisateur courant possède un rôle de gestionnaire de Paie, sinon nulle.</returns>
        public bool IsGSP(int userId, int? ciId)
        {
            if (ciId.HasValue && ciId.Value > 0)
            {
                CIEnt ci = this.repoCi.Get(ciId.Value);
                return GetHigherPaieLevel(userId, ci.Organisation.OrganisationId) >= Constantes.NiveauPaie.LevelGSP;
            }
            else
            {
                return GetHigherPaieLevel(userId, null) >= Constantes.NiveauPaie.LevelGSP;
            }
        }

        /// <summary>
        ///   Retourne si l'utulisateur fait parti lié au chantier ou non
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne Vrai si l'utilisateur courant possède un rôle lié au chantier, sinon nulle.</returns>
        public bool IsRoleChantier(int userId, int? ciId)
        {
            if (ciId.HasValue && ciId.Value > 0)
            {
                var ci = this.repoCi.Get(ciId.Value);
                return GetHigherPaieLevel(userId, ci.Organisation.OrganisationId) >= Constantes.NiveauPaie.LevelCDC;
            }
            else
            {
                return GetHigherPaieLevel(userId, null) >= Constantes.NiveauPaie.LevelCDC;
            }
        }

        /// <summary>
        ///   Retourne vrai si l'utilisateur à des droits sur la paie égal 1
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne Vrai si l'utilisateur courant possède un rôle lié au chantier, sinon nulle.</returns>
        public bool IsNiveauPaie1(int userId, int ciId)
        {
            var ci = this.repoCi.Get(ciId);
            return GetHigherPaieLevel(userId, ci.Organisation.OrganisationId) == Constantes.NiveauPaie.LevelCDC;
        }

        /// <summary>
        ///   Retourne vrai si l'utilisateur à des droits sur la paie égal 2
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne Vrai si l'utilisateur courant possède un rôle lié au chantier, sinon nulle.</returns>
        public bool IsNiveauPaie2(int userId, int ciId)
        {
            var ci = this.repoCi.Get(ciId);
            return GetHigherPaieLevel(userId, ci.Organisation.OrganisationId) == Constantes.NiveauPaie.LevelCDT;
        }

        /// <summary>
        ///   Retourne vrai si l'utilisateur à des droits sur la paie égal 3
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne Vrai si l'utilisateur courant possède un rôle lié au chantier, sinon nulle.</returns>
        public bool IsNiveauPaie3(int userId, int ciId)
        {
            CIEnt ci = this.repoCi.Get(ciId);
            return GetHigherPaieLevel(userId, ci.Organisation.OrganisationId) == Constantes.NiveauPaie.LevelDRC;
        }

        /// <summary>
        /// Evalue l'appartenance d'un ci au périmètre de l'utilisateur
        /// </summary>
        /// <param name="ciId">L'identifiant du Ci à évaluer</param>
        /// <returns>Retourne vrai si le CI fait parti du périmètre de l'utilisateur, faux sinon</returns>
        public bool IsInMyPerimetre(int ciId)
        {
            IEnumerable<int> allCisByUser = GetAllCIbyUser(GetContextUtilisateurId()).ToList();
            return allCisByUser.Contains(ciId);
        }

        /// <summary>
        /// Retourne l'AffectationSeuilUtilisateurEnt correspondant a l'utilisateur et a l'organisation.
        /// S'il n'y a pas de d'affectation pour l'organisationId, on remonte l'abrbre des organisation parente,
        /// jusqu'a trouvé une affectation. Sinon on renvoi null.
        /// Si le role n'est pas actif pour l'affectation, alors on ne selectionne pas l'affectation
        /// </summary>
        /// <param name="utilisateurId">utilisateurId</param>
        /// <param name="organisationId">organisationId</param>
        /// <returns>AffectationSeuilUtilisateurEnt</returns>
        public AffectationSeuilUtilisateurEnt GetFirstAffectationForOrganisationInTreeWithRoleActif(int utilisateurId, int organisationId)
        {
            AffectationSeuilUtilisateurEnt result = null;
            IEnumerable<AffectationSeuilUtilisateurEnt> affectationSeuilUtilsateurs = Repository.GetAffectationRoles(utilisateurId).ToList();

            List<OrganisationEnt> organisationTree = this.repoOrga.GetOrganisationParentsWithCurrent(organisationId).ToList();
            for (int i = 0; i < organisationTree.Count; i++)
            {
                AffectationSeuilUtilisateurEnt match = affectationSeuilUtilsateurs.FirstOrDefault(asu => asu.OrganisationId == organisationTree[i].OrganisationId);
                if (match?.Role != null && match.Role.Actif)
                {
                    result = match;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Get Ci list for role . Pour un delegue merci de privilégier la méthode : GetCiListForDelegue
        /// </summary>
        /// <param name="roleSpecification">Role specification pour identifier un délégué ou un responsable de chantier</param>
        /// <returns>IEnumerable of CIEnt</returns>
        public IEnumerable<CIEnt> GetCiListForRole(RoleSpecification roleSpecification)
        {
            IEnumerable<RoleEnt> roleList = GetUserRoles()?
                .Where(r => (r.RoleSpecification.HasValue && r.RoleSpecification.Value == roleSpecification)
                || (roleSpecification == RoleSpecification.ResponsableCI && (r.Code == CodeRole.CodeRoleRespCI || r.CodeNomFamilier == CodeRole.CodeRoleRespCI))
                || (roleSpecification == RoleSpecification.Delegue && (r.Code == CodeRole.CodeRoleDelegueCI || r.CodeNomFamilier == CodeRole.CodeRoleDelegueCI)));
            if (roleList.IsNullOrEmpty())
            {
                return new List<CIEnt>();
            }

            int currentUtilisateurId = GetContextUtilisateurId();
            return Repository.GetCiListForRoles(roleList.Select(r => r.RoleId), currentUtilisateurId);
        }

        /// <summary>
        /// Get ci list for delegue 
        /// </summary>
        /// <param name="userId">Optionnel : identifiant de l'utilisateur pour lequel récupérer les CI ; par défaut, l'utilisateur connecté</param>
        /// <returns>IEnumerable of CIEnt</returns>
        public IEnumerable<CIEnt> GetCiListForDelegue(int? userId = null)
        {
            if (!userId.HasValue) { userId = GetContextUtilisateurId(); }
            IEnumerable<CIEnt> ciAffectedByDelegationHabilitation = new List<CIEnt>();
            RoleEnt delegueCiRole = GetUserRoles(userId)?.FirstOrDefault(r => (r.RoleSpecification.HasValue && r.RoleSpecification.Value == RoleSpecification.Delegue)
            || r.Code == CodeRole.CodeRoleDelegueCI || r.CodeNomFamilier == CodeRole.CodeRoleDelegueCI);
            if (delegueCiRole == null)
            {
                return new List<CIEnt>();
            }
            else if (!delegueCiRole.AffectationSeuilUtilisateurs.IsNullOrEmpty())
            {
                ciAffectedByDelegationHabilitation = repoCi.GetByOrganisationId(delegueCiRole.AffectationSeuilUtilisateurs.Select(x => x.OrganisationId), true);
            }

            IEnumerable<CIEnt> ciAffectedByDelegationGestionCi = Repository.GetCiListForDelegue(userId.Value);
            return ciAffectedByDelegationGestionCi.Union(ciAffectedByDelegationHabilitation).Distinct();
        }

        /// <summary>
        /// Get affectation list of responsable (Delegue ou reponsable CI) . 
        /// personnel statut s'assure de retourner un type particuler d'employé : Ouvrier ,ETAM ou IAC etc ..
        /// </summary>
        /// <param name="personnelStatut">Personnel statut</param>
        /// <returns>IEnumerable of Affectation Ent</returns>
        public IEnumerable<AffectationEnt> GetAffectationList(string personnelStatut)
        {
            int currentUtilisateurId = GetContextUtilisateurId();
            IEnumerable<RoleEnt> roles = GetUserRoles().ToList();
            if (roles?.Any() == true)
            {
                // La liste des roles peut se faire sur différentes sociétés
                IEnumerable<RoleEnt> responsableCiRoleList = roles.Where(r => (r.RoleSpecification.HasValue && r.RoleSpecification.Value == RoleSpecification.ResponsableCI)
                || r.Code == CodeRole.CodeRoleRespCI || r.CodeNomFamilier == CodeRole.CodeRoleRespCI);
                IEnumerable<RoleEnt> delegueCiRoleList = roles.Where(r => (r.RoleSpecification.HasValue && r.RoleSpecification.Value == RoleSpecification.Delegue)
                || r.Code == CodeRole.CodeRoleDelegueCI || r.CodeNomFamilier == CodeRole.CodeRoleDelegueCI);

                IEnumerable<AffectationEnt> responsableAffectations = null;
                if (!responsableCiRoleList.IsNullOrEmpty())
                {
                    responsableAffectations = Repository.GetAffectationList(
                                  currentUtilisateurId,
                                  responsableCiRoleList.Select(v => v.RoleId).Distinct(),
                                  personnelStatut);
                }
                if (responsableAffectations == null)
                {
                    responsableAffectations = new List<AffectationEnt>();
                }

                IEnumerable<AffectationEnt> delegueAffectations = null;
                if (!delegueCiRoleList.IsNullOrEmpty())
                {
                    delegueAffectations = Repository.GetAffectationList(
                                  currentUtilisateurId,
                                  delegueCiRoleList.Select(b => b.RoleId).Distinct(),
                                  personnelStatut);
                }

                return delegueAffectations == null ?
                    responsableAffectations : delegueAffectations.Union(responsableAffectations);
            }

            return new List<AffectationEnt>();
        }

        /// <summary>
        /// Get ci list of responsable
        /// </summary>
        /// <returns>IEnumerable de CIEnt</returns>
        public IEnumerable<CIEnt> GetCiListOfResponsable()
        {
            IEnumerable<CIEnt> ciListIfDelegue = GetCiListForDelegue();
            IEnumerable<CIEnt> ciListIfReponsableCi = GetCiListForRole(RoleSpecification.ResponsableCI);

            return ciListIfDelegue.Union(ciListIfReponsableCi, new CiComparer());
        }

        /// <summary>
        /// Get ci list of responsable
        /// </summary>
        /// <param name="userId">L'identifiant de l'utilisateur</param>
        /// <returns>IEnumerable de CIEnt</returns>
        public IEnumerable<CIEnt> GetCiListOfResponsable(int userId)
        {
            UtilisateurEnt utilisateur = GetById(userId);
            if (utilisateur?.AffectationsRole == null)
            {
                return new List<CIEnt>();
            }

            IEnumerable<RoleEnt> roles = utilisateur.AffectationsRole
                                      .Where(o => o.UtilisateurId == utilisateur.UtilisateurId)
                                      .Select(o => o.Role)
                                      .Where(r => (r.RoleSpecification.HasValue && r.RoleSpecification.Value == RoleSpecification.ResponsableCI)
                                      || r.Code == CodeRole.CodeRoleRespCI || r.CodeNomFamilier == CodeRole.CodeRoleRespCI);

            if (roles.IsNullOrEmpty())
            {
                return new List<CIEnt>();
            }

            return Repository.GetCiListForRoles(roles.Select(d => d.RoleId), userId);
        }

        /// <summary>
        /// Permet de récupéré les droite pour gérér le personnel en fonction roleId
        /// </summary>
        /// <param name="roleId">identifiant unique du role</param>
        /// <returns>RoleFornctionnalite</returns>
        public RoleFonctionnaliteEnt GetRightPersonnelManagement(int roleId)
        {
            RoleFonctionnaliteEnt roleFonctionnaliteEnt = roleFonctionnaliteManager.GetByRoleIdAndFonctionnaliteId(roleId, 30);
            return roleFonctionnaliteManager.GetRoleFonctionnaliteDetail(roleFonctionnaliteEnt.RoleFonctionnaliteId);
        }

        /// <summary>
        ///   Vérifie que l'utilisateur est SuperAdmin
        /// </summary>
        /// <param name="utilisateurId">Id de l'Utilisateur</param>
        /// <returns>Vrai si l'utilisateur possède un rôle SuperAdmin</returns>
        public bool IsSuperAdmin(int utilisateurId)
        {
            return Repository.IsSuperAdmin(utilisateurId);
        }

        /// <summary>
        /// Vérifier si un utilisateur a la permission pour voir toute la liste des affectations des moyens
        /// </summary>
        /// <returns>Booléan indique si l'utilisateur a la permission pour voir toute la liste des affectations des moyens</returns>
        public bool HasPermissionToSeeAllAffectationMoyens()
        {
            return HasUserPermission(PermissionKeys.AffichageAllAffectationMoyen);
        }

        /// <summary>
        /// Vérifier si un utilisateur a la permission pour voir la liste des affectations des moyens d'un manager des personnels
        /// </summary>
        /// <returns>Booléan indique si l'utilisateur a la permission pour voir la liste des affectations des moyens d'un manager des personnels</returns>
        public bool HasPermissionToSeeManagerPersonnelAffectationMoyens()
        {
            return HasUserPermission(PermissionKeys.AffichageManagerPersonnelAffectationMoyen);
        }

        /// <summary>
        /// Vérifier si un utilisateur a la permission pour voir la liste des affectations des moyens d'un responsable CI
        /// </summary>
        /// <returns>Booléan indique si l'utilisateur a la permission pour voir la liste des affectations des moyens d'un responsable CI</returns>
        public bool HasPermissionToSeeResponsableCiAffectationMoyens()
        {
            return HasUserPermission(PermissionKeys.AffichageResponsableCiAffectationMoyen);
        }

        /// <summary>
        /// Vérifier si un utilisateur a la permission pour voir la liste des affectations des moyens d'un délégué CI
        /// </summary>
        /// <returns>Booléan indique si l'utilisateur a la permission pour voir la liste des affectations des moyens d'un délégué CI</returns>
        public bool HasPermissionToSeeDelegueCiAffectationMoyens()
        {
            return HasUserPermission(PermissionKeys.AffichageDelegueCiAffectationMoyen);
        }

        /// <summary>
        /// Vérifier si un utilisateur a la permission pour voir toute la liste des CI
        /// </summary>
        /// <returns>Booléan indique si l'utilisateur a la permission pour voir toute la liste des CI</returns>
        public bool HasPermissionToSeeAllCi()
        {
            return HasUserPermission(PermissionKeys.AffichageAllCi);
        }

        /// <summary>
        /// Vérifier si un utilisateur a la permission pour voir la liste des CI d'un responsable CI
        /// </summary>
        /// <returns>Booléan indique si l'utilisateur a la permission pour voir la liste des CI d'un responsable CI</returns>
        public bool HasPermissionToSeeResponsableCiList()
        {
            return HasUserPermission(PermissionKeys.AffichageResponsableCiList);
        }

        /// <summary>
        /// Vérifier si un utilisateur a la permission pour voir la liste des personnels
        /// </summary>
        /// <returns>Booléan indique si l'utilisateur a la permission pour voir la liste des personnels</returns>
        public bool HasPermissionToSeePersonnelsList()
        {
            return HasUserPermissionRole(PermissionKeys.AffichagePersonnelsList, true);
        }

        /// <summary>
        /// Vérifier si un utilisateur a la permission pour voir la liste des CI d'un délégué CI
        /// </summary>
        /// <returns>Booléan indique si l'utilisateur a la permission pour voir la liste des CI d'un délégué CI</returns>
        public bool HasPermissionToSeeDelegueCiList()
        {
            return HasUserPermission(PermissionKeys.AffichageDelegueCiList);
        }

        /// <summary>
        /// Vérifier si un utilisateur a le rôle gestionnaire des moyens
        /// </summary>
        /// <param name="utilisateurId">L'identifiant de l'utilisateur</param>
        /// <returns>Booléan indique si l'utilisateur a le rôle gestionnaire des moyens</returns>
        [Obsolete("Les rôles ne sont pas fixes ; il faut tester les permissions (ce n'est pas l'idéal, mais c'est mieux)")]
        public bool IsUserGestionnaireMoyen(int utilisateurId)
        {
            IEnumerable<RoleEnt> roles = GetUserRoles();
            RoleEnt responsableCiRole = roles.FirstOrDefault(r => r.CodeNomFamilier?.Equals(Shared.Constantes.RoleGestionnaireMoyen) == true);
            return responsableCiRole != null;
        }

        /// <summary>
        /// Vérifier si un utilisateur a le rôle manager des personnels
        /// </summary>
        /// <param name="utilisateurId">L'identifiant de l'utilisateur</param>
        /// <returns>Booléan indique si l'utilisateur a le rôle manager des personnels</returns>
        [Obsolete("Les rôles ne sont pas fixes ; il faut tester les permissions (ce n'est pas l'idéal, mais c'est mieux)")]
        public bool IsUserManagerPersonnel(int utilisateurId)
        {
            var utilisateur = GetById(utilisateurId);
            RoleEnt managerRole = utilisateur?.AffectationsRole?
                                      .Select(o => o.Role)
                                      .FirstOrDefault(r => r.CodeNomFamilier?.Equals(Shared.Constantes.RoleManager) == true);
            return managerRole != null;
        }

        /// <summary>
        /// Vérifier si un utilisateur a le rôle responsable CI
        /// </summary>
        /// <param name="utilisateurId">L'identifiant de l'utilisateur</param>
        /// <returns>Booléan indique si l'utilisateur a le rôle responsable CI</returns>
        [Obsolete("Les rôles ne sont pas fixes ; il faut tester les permissions (ce n'est pas l'idéal, mais c'est mieux)")]
        public bool IsUserResponsableCi(int utilisateurId)
        {
            var utilisateur = GetById(utilisateurId);
            RoleEnt responsableCiRole = utilisateur?.AffectationsRole?
                                      .Select(o => o.Role)
                                      .FirstOrDefault(r => (r.RoleSpecification.HasValue && r.RoleSpecification.Value == RoleSpecification.ResponsableCI)
                                      || r.Code == CodeRole.CodeRoleRespCI || r.CodeNomFamilier == CodeRole.CodeRoleRespCI);
            return responsableCiRole != null;
        }

        /// <summary>
        /// Indique si l'utilisateur connecté appartient au groupe indiqué.
        /// </summary>
        /// <param name="groupe">Le groupe (comme Constantes.CodeGroupeFES).</param>
        /// <returns>True si l'utilisateur appartient au groupe, sinon false.</returns>
        public bool IsUtilisateurOfGroupe(string groupe)
        {
            var utilisateur = GetContextUtilisateur();
            return utilisateur.Personnel.Societe?.Groupe?.Code?.Trim() == groupe;
        }

        /// <summary>
        /// Vérifier si un utilisateur a le rôle gestionnaire de paie sans prendre en considération sans niveau de paie
        /// </summary>
        /// <param name="utilisateurId">L'identifiant de l'utilisateur</param>
        /// <param name="organisationId">l'identifiant de l'organisation</param>
        /// <returns>Booléan indique si l'utilisateur a le rôle gestionnaire de paie</returns>
        public bool IsRoleGSPWithoutConsideringPaieLevel(int utilisateurId, int? organisationId)
        {
            List<RoleEnt> listRoles;
            if (organisationId.HasValue)
            {
                var organisationTree = organisationTreeService.GetOrganisationTree();

                var parentOrgaIdList = organisationTree.GetParentsWithCurrent(organisationId.Value)
                                                        .Select(x => x.OrganisationId);

                listRoles = this.repoAffectation.Query()
                    .Filter(a => a.UtilisateurId == utilisateurId)
                    .Filter(a => parentOrgaIdList.Contains(a.OrganisationId))
                    .Get()
                    .Select(r => r.Role)
                    .ToList();
            }
            else
            {
                listRoles = this.repoAffectation.Query()
                    .Filter(a => a.UtilisateurId == utilisateurId)
                    .Get()
                    .Select(r => r.Role)
                    .ToList();
            }
            RoleEnt gspRole = listRoles?.FirstOrDefault(r => r.CodeNomFamilier != null
                                        && (r.CodeNomFamilier.Equals(Shared.Constantes.CodeRoleGSP) || r.CodeNomFamilier.Equals(Shared.Constantes.CodeRoleCSP)));
            return gspRole != null;
        }

        /// <summary>
        ///   Retourne si l'utilisateur a la permission de voir menu edition
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne Vrai si l'utilisateur courant possède un rôle dans la Paie, sinon nulle.</returns>
        public bool IsUserHasMenuEditionPermission(int userId, int? ciId)
        {
            UtilisateurEnt currentUser = this.Repository.GetUtilisateurById(userId);
            CIEnt ci = null;
            if (ciId.HasValue && ciId.Value > 0)
            {
                ci = this.repoCi.Get(ciId.Value);
            }

            if (!currentUser.Personnel.Societe.Groupe.Code.Equals(Constantes.CodeGroupeFES))
            {
                return GetHigherPaieLevel(userId, ci?.Organisation?.OrganisationId) >= Constantes.NiveauPaie.LevelCSP;
            }

            return HasUserPermission(PermissionKeys.AffichageMenuEditionRapport);
        }

        /// <summary>
        /// Vérifie si l'utilisateur connecté à une permission
        /// </summary>
        /// <param name="permissionKey">Clé de la permission</param>
        /// <param name="asNoTracking">Sans tracking par défaut à oui</param>
        /// <returns>Booléan indique si l'utilisateur connecté à la permission fournit en paramétre</returns>
        private bool HasUserPermission(string permissionKey, bool asNoTracking = false)
        {
            return HasUserPermissionRole(permissionKey, false, asNoTracking);
        }

        /// <summary>
        /// Vérifer si l'utilisateur a bien une permission
        /// </summary>
        /// <param name="permissionKey">Clé de la permission</param>
        /// <param name="mode">Indique si on verifier l'etat de Fonctionnalite pour le role ou non </param>
        /// <param name="asNoTracking">Sans tracking par défaut à oui</param>
        /// <returns>Booléan indique si l'utilisateur connecté à la permission fournit en paramétre</returns>
        private bool HasUserPermissionRole(string permissionKey, bool mode, bool asNoTracking = false)
        {
            UtilisateurEnt user = GetContextUtilisateur();
            IEnumerable<int> roleIds = GetUserRoles(null, asNoTracking).Select(r => r.RoleId);
            if (!roleIds.IsNullOrEmpty())
            {
                IEnumerable<RoleFonctionnaliteEnt> roleFonctionnalites = roleFonctionnaliteManager.GetRoleFonctionnalitesByRoles(roleIds);
                List<FonctionnaliteEnt> fonctionnalites = roleFonctionnalites.Select(x => x.Fonctionnalite).ToList();
                if (mode)
                {
                    fonctionnalites = roleFonctionnalites.Where(x => x.Mode == FonctionnaliteTypeMode.Write || x.Mode == FonctionnaliteTypeMode.Read).Select(x => x.Fonctionnalite).ToList();
                }

                foreach (FonctionnaliteEnt fonctionnalite in fonctionnalites)
                {
                    var permissions = permissionFonctionnaliteManager.GetPermissionFonctionnalites(fonctionnalite.FonctionnaliteId)
                                        .Select(f => f.Permission);
                    if (permissions.Any(p => p.PermissionKey == permissionKey) && !fonctionnaliteDesactiveManager.IsFonctionnaliteDesactiveForSociete(fonctionnalite.FonctionnaliteId, user.Personnel.SocieteId.Value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Vérifier si un utilisateur a la permission de Créer des coummandes brouillons avec un fournisseur temporaire
        /// </summary>
        /// <returns>Booléan indique si l'utilisateur a la permission de Créer des coummandes brouillons avec un fournisseur temporaire</returns>
        public bool HasPermissionToCreateBrouillonWithFournisseurTemporaire()
        {
            return HasUserPermission(PermissionKeys.CreationCommandeBrouillonFournisseurTemporaire, true);
        }
    }
}
