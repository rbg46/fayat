using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Role;
using Fred.Entities.Utilisateur;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.AffectationSeuilUtilisateur
{
    /// <summary>
    ///   Référentiel de données pour les affectations de seuils.
    /// </summary>
    public class AffectationSeuilUtilisateurRepository : FredRepository<AffectationSeuilUtilisateurEnt>, IAffectationSeuilUtilisateurRepository
    {
        private readonly IOrganisationRepository userRepo;

        public AffectationSeuilUtilisateurRepository(FredDbContext context, IOrganisationRepository userRepo)
          : base(context)
        {
            this.userRepo = userRepo;
        }

        /// <summary>
        ///   Déterminer si l'utilisateur a le droit d'accéder à cette entité.
        /// </summary>
        /// <param name="entity">L'entité concernée.</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <exception cref="System.UnauthorizedAccessException">Utilisateur n'a pas l'autorisation</exception>
        public override void CheckAccessToEntity(AffectationSeuilUtilisateurEnt entity, int userId)
        {
            //// A faire : Attention : c'est du code dupliqué, tous les CheckAccessToEntity copie/colle ce code...
            //// => Mettre l'implémentation dans la classe de basse
            //// => et si besoin dériver la méthode
            //// => c'est à ça que sert une méthode virtuelle

            if (entity.Organisation == null)
            {
                PerformEagerLoading(entity, x => x.Organisation);
            }

            if (entity.Organisation != null)
            {
                var orgaList = userRepo.GetOrganisationsAvailable(null, new List<int> { entity.Organisation.TypeOrganisationId }, userId);
                if (orgaList.All(o => o.OrganisationId != entity.Organisation.OrganisationId))
                {
                    throw new UnauthorizedAccessException();
                }
            }
        }

        /// <summary>
        /// Permet de récupérer une affectation en fonction de l'identifiant de l'utilisateur
        /// </summary>
        /// <param name="utilisateurId">Personnel de référence.</param>
        /// <returns>L'affectation</returns>
        public AffectationSeuilUtilisateurEnt GetAffectationByUtilisateurId(int utilisateurId)
        {
            return Query()
            .Include(a => a.Utilisateur)
            .Include(a => a.Organisation)
            .Include(a => a.Role)
            .Include(a => a.Devise)
            .Get()
            .Where(a => a.UtilisateurId == utilisateurId)
            .FirstOrDefault();
        }

        /// <summary>
        /// Permet de récupérer une liste des affectations et seuils liés à un utilisateur
        /// </summary>
        /// <param name="utilisateurId">identifiant unique d'un utilisateur.</param>
        /// <returns>Les affectations et seuils liés à l'utilisateur.</returns>
        public List<AffectationSeuilUtilisateurEnt> GetListAffectionDelegateByUtilisateurId(int utilisateurId)
        {
            return Query()
            .Include(a => a.Utilisateur)
            .Include(a => a.Organisation)
            .Include(a => a.Role)
            .Include(a => a.Devise)
            .Get()
            .Where(a => a.UtilisateurId == utilisateurId && a.DelegationId == null)
            .ToList();
        }


        /// <summary>
        /// Permet de récupérer une liste des affectations et seuils liés à un utilisateur
        /// </summary>
        /// <param name="delegationId">identifiant unique d'une délégation.</param>
        /// <returns>Les affectations et seuils liés à l'utilisateur.</returns>
        public List<AffectationSeuilUtilisateurEnt> GetListAffectionRecoverByUtilisateurId(int delegationId)
        {
            return Query()
            .Include(a => a.Utilisateur)
            .Include(a => a.Organisation)
            .Include(a => a.Role)
            .Include(a => a.Devise)
            .Get()
            .Where(a => a.DelegationId == delegationId)
            .ToList();
        }

        /// <summary>
        /// Permet d'attribuer une liste d'affection et seuil pour une délégation de déléguant à délégué
        /// </summary>
        /// <param name="delegationId">identifiant unique de la délégation.</param>
        /// <param name="delegantId">identifiant unique de l'utilisateur délégant son affectation.</param>
        /// <param name="delegueId">identifiant unique de l'utilisateur récupérant l'affectation.</param>
        public void DelegateAffectation(int delegationId, int delegantId, int delegueId)
        {
            List<AffectationSeuilUtilisateurEnt> affectationSeuilUtilisateurEntDelegateList = GetListAffectionDelegateByUtilisateurId(delegantId);

            foreach (AffectationSeuilUtilisateurEnt affectationSeuilUtilisateurEnt in affectationSeuilUtilisateurEntDelegateList)
            {
                AffectationSeuilUtilisateurEnt affectationSeuilUtilisateur = new AffectationSeuilUtilisateurEnt
                {
                    CommandeSeuil = affectationSeuilUtilisateurEnt.CommandeSeuil,
                    DelegationId = delegationId,
                    DeviseId = affectationSeuilUtilisateurEnt.DeviseId,
                    OrganisationId = affectationSeuilUtilisateurEnt.OrganisationId,
                    RoleId = affectationSeuilUtilisateurEnt.RoleId,
                    UtilisateurId = delegueId
                };
                AddAffectation(affectationSeuilUtilisateur);
            }
        }

        /// <summary>
        /// Permet d'enlever une liste d'affectation à la désactivation d'une délégation
        /// </summary>
        /// <param name="delegationId">identifiant unique de la délégation.</param>
        public void RecoverAffectation(int delegationId)
        {
            List<AffectationSeuilUtilisateurEnt> affectationSeuilUtilisateurEntRecoverList = GetListAffectionRecoverByUtilisateurId(delegationId);

            foreach (AffectationSeuilUtilisateurEnt affectationSeuilUtilisateurEnt in affectationSeuilUtilisateurEntRecoverList)
            {
                DeleteAffectation(affectationSeuilUtilisateurEnt.AffectationRoleId);
            }
        }

        /// <summary>
        /// Permet d'ajouter une affectation
        /// </summary>
        /// <param name="affectationSeuilUtilisateurEnt">affectation</param>
        /// <returns>L'affectation ajouté</returns>
        public AffectationSeuilUtilisateurEnt AddAffectation(AffectationSeuilUtilisateurEnt affectationSeuilUtilisateurEnt)
        {
            Insert(affectationSeuilUtilisateurEnt);

            return affectationSeuilUtilisateurEnt;
        }

        /// <summary>
        /// Permet de supprimer une affectation
        /// </summary>
        /// <param name="affectationRoleId">Identifiant unique de l'affectation</param>
        public void DeleteAffectation(int affectationRoleId)
        {
            DeleteById(affectationRoleId);
        }

        /// <summary>
        /// Permet de récupérer une liste des affectations et seuils liés à un utilisateur
        /// </summary>
        /// <param name="utilisateurId">identifiant unique d'un utilisateur.</param>
        /// <returns>Les affectations et seuils liés à l'utilisateur.</returns>
        public List<AffectationSeuilUtilisateurEnt> GetListByUtilisateurId(int utilisateurId)
        {
            return Query()
            .Include(a => a.Organisation.CI)
            .Include(a => a.Role)
            .Get()
            .Where(a => a.UtilisateurId == utilisateurId)
            .ToList();
        }

        /// <summary>
        /// Permet de récupérer toutes les affectations et seul.
        /// </summary>
        /// <returns>Une liste d'affectation.</returns>
        public List<AffectationSeuilUtilisateurEnt> GetAll()
        {
            return Query().Get().ToList();
        }

        /// <summary>
        /// Permet de récupérer toutes les affectations.
        /// </summary>
        /// <param name="utilisateursIds">liste des id des utilisateurs.</param>
        /// <param name="roleIds">liste des id des roles.</param>
        /// <param name="orgaIds">liste des id des organisations.</param>
        /// <returns>Une liste d'affectation.</returns>
        public List<AffectationSeuilUtilisateurEnt> GetAllByUtilAndRoleAndOrgaLists(List<int> utilisateursIds, List<int> roleIds, List<int> orgaIds)
        {
            return Query()
                .Include(a => a.Utilisateur.Personnel.EtablissementPaie)
                .Include(a => a.Utilisateur.Personnel.Ressource)
                .Include(a => a.Role)
                .Include(a => a.Organisation.Societe)
                .Include(a => a.Organisation.CI)
                .Include(a => a.Organisation.Etablissement)
                .Include(a => a.Organisation.Pere.TypeOrganisation)
                .Include(a => a.Organisation.TypeOrganisation)
                .Include(a => a.Devise)
                .Filter(x => utilisateursIds.Contains(x.UtilisateurId) && roleIds.Contains(x.RoleId) && orgaIds.Contains(x.OrganisationId))
                .Get()
                .ToList();
        }

        /// <summary>
        /// Retourne les affectationSeuilUtilisateur contenue dans la liste 'affectationSeuilUtilisateurIds'
        /// </summary>
        /// <param name="affectationSeuilUtilisateurIds">affectationSeuilUtilisateurIds</param>
        /// <returns>liste d'AffectationSeuilUtilisateurEnt</returns>
        public List<AffectationSeuilUtilisateurEnt> Get(List<int> affectationSeuilUtilisateurIds)
        {
            return Context.UtilOrgaRoleDevises.Where(x => affectationSeuilUtilisateurIds.Contains(x.AffectationRoleId)).AsNoTracking().ToList();
        }

        /// <summary>
        /// Permet d'obtenir la liste des affectations et roles pour les habilitations niveau gestion du personnel
        /// </summary>
        /// <param name="utilisateurId">idnetifiant du personnel</param>
        /// <returns>liste des affectations et seuil</returns>
        public IEnumerable<AffectationSeuilUtilisateurEnt> GetAffectationSeuilUtilisateursForDetailPersonnel(int utilisateurId)
        {
            return Context.UtilOrgaRoleDevises
                .Include(affectationSeuilUtilisateur => affectationSeuilUtilisateur.Role.SeuilsValidation)
                .Include(affectationSeuilUtilisateur => affectationSeuilUtilisateur.Devise)
                .Include(affectationSeuilUtilisateur => affectationSeuilUtilisateur.Organisation)
                .Include(affectationSeuilUtilisateur => affectationSeuilUtilisateur.Organisation).ThenInclude(organisation => organisation.TypeOrganisation)
                .Include(affectationSeuilUtilisateur => affectationSeuilUtilisateur.Organisation).ThenInclude(organisation => organisation.CI)
                .Include(affectationSeuilUtilisateur => affectationSeuilUtilisateur.Organisation).ThenInclude(organisation => organisation.Etablissement)
                .Include(affectationSeuilUtilisateur => affectationSeuilUtilisateur.Organisation).ThenInclude(organisation => organisation.Societe)
                .Include(affectationSeuilUtilisateur => affectationSeuilUtilisateur.Organisation).ThenInclude(organisation => organisation.OrganisationGenerique)
                .Include(affectationSeuilUtilisateur => affectationSeuilUtilisateur.Organisation).ThenInclude(organisation => organisation.Groupe)
                .Include(affectationSeuilUtilisateur => affectationSeuilUtilisateur.Organisation).ThenInclude(organisation => organisation.Pole)
                .Include(affectationSeuilUtilisateur => affectationSeuilUtilisateur.Organisation).ThenInclude(organisation => organisation.Holding)
                .Where(affectationSeuilUtilisateur => affectationSeuilUtilisateur.UtilisateurId == utilisateurId)
                .AsNoTracking();
        }

        /// <summary>
        /// Get Affectation By User And Roles
        /// </summary>
        /// <param name="utilisateurId">User id</param>
        /// <param name="roles">List des rôles</param>
        /// <returns></returns>
        public IEnumerable<AffectationSeuilUtilisateurEnt> GetAffectationByUserAndRoles(int utilisateurId, List<RoleEnt> roles)
        {
            IEnumerable<string> distinctRoles = roles.Select(r => r.CodeNomFamilier);
            return Context.UtilOrgaRoleDevises
                .Where(a => a.UtilisateurId == utilisateurId)
                .Where(x => x.Role.CodeNomFamilier.Equals(Web.Shared.Constantes.CodeRoleGSP) ||
                            x.Role.CodeNomFamilier.Equals(Web.Shared.Constantes.CodeRoleGSC) ||
                            x.Role.CodeNomFamilier.Equals(Web.Shared.Constantes.CodeRoleAdm) ||
                            x.Role.CodeNomFamilier.Equals(Web.Shared.Constantes.RoleGestionnaireMoyen) ||
                            distinctRoles.Contains(x.Role.CodeNomFamilier))
                .Include(x => x.Organisation)
                .Include(x => x.Organisation.TypeOrganisation)
                .ToList();
        }

        /// <summary>
        /// Get organisation by type without parent tree
        /// </summary>
        /// <param name="utilisateurId">Utilisateur id</param>
        /// <param name="organisationTypeCode">organisation type code</param>
        /// <returns>Organisation affectation list</returns>
        public async Task<List<AffectationSeuilUtilisateurEnt>> GetOrganisationByTypeWithoutParentTree(int utilisateurId, string organisationTypeCode)
        {
            return await Context.UtilOrgaRoleDevises.Include(x => x.Organisation.Pere.Societe)
                                                    .Include(x => x.Organisation.Pere.TypeOrganisation)
                                                    .Where(x => x.UtilisateurId == utilisateurId && x.Organisation.TypeOrganisation.Code.Equals(organisationTypeCode))
                                                    .AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Permet du supprimer une liste de ligne d'affectation by utilisateur id.
        /// </summary>
        /// <param name="affectationsToDelete">Liste d'affectation à supprimer.</param>
        public void DeleteAffectationList(List<AffectationSeuilUtilisateurEnt> affectationsToDelete)
        {
            if (affectationsToDelete != null && affectationsToDelete.Any())
            {
                foreach (var affectation in affectationsToDelete)
                {
                    Delete(affectation);
                }
            }
        }
    }
}
