using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Fred.Business.Habilitation.Core;
using Fred.Business.Habilitation.Interfaces;
using Fred.Business.Utilisateur;
using Fred.Entities.Habilitation;
using Fred.Entities.Permission;
using Fred.Entities.Utilisateur;

namespace Fred.Business.Habilitation
{
    /// <summary>
    /// Manager des habilitation
    /// </summary>
    public class HabilitationManager : IHabilitationManager
    {
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IHabilitationCoreManager habilitationCoreManager;

        /// <summary>
        /// ctor
        /// </summary>  
        /// <param name="utilisateurManager">utilisateurManager</param>   
        /// <param name="habilitationCoreManager">habilitationCoreManager</param>
        public HabilitationManager(IUtilisateurManager utilisateurManager,
                                    IHabilitationCoreManager habilitationCoreManager)
        {
            this.utilisateurManager = utilisateurManager;
            this.habilitationCoreManager = habilitationCoreManager;
        }

        /// <summary>
        /// Retourne les habilitation en fonction de l'organisation.
        /// </summary>
        /// <param name="organisationId">organisationId</param>
        /// <returns>HabilitationEnt</returns>
        public HabilitationEnt GetHabilitation(int? organisationId = null)
        {
            HabilitationEnt result = new HabilitationEnt();
            UtilisateurEnt utilisateur = utilisateurManager.GetContextUtilisateur();

            if (utilisateur == null)
            {
                result.IsSuperAdmin = false;
                result.SocieteId = null;
                return result;
            }

            result.UtilisateurId = utilisateur.UtilisateurId;
            result.IsSuperAdmin = utilisateur.SuperAdmin;
            result.SocieteId = utilisateur.Personnel.SocieteId;

            IEnumerable<PermissionEnt> concatenedPermissions = habilitationCoreManager.GetPermissionsForUtilisateur(utilisateur.UtilisateurId);

            result.Permissions = concatenedPermissions.RemoveDuplicatesPermissions()
                                                      .SelectGlobalsPermissions();

            if (organisationId.HasValue)
            {
                result.PermissionsContextuelles = habilitationCoreManager.GetPermissionsForUtilisateurAndOrganisation(organisationId.Value)
                                                                          .RemoveDuplicatesPermissions()
                                                                          .SelectContextuellesPermissions();
            }

            return result;
        }

        /// <summary>
        /// Recupere les permissions pour l'utilisateur actuel et pour un organisationId.
        /// </summary>
        /// <param name="organisationId">organisationId</param>
        /// <returns>Une liste de permissions</returns>
        public IEnumerable<PermissionEnt> GetContextuellesPermissionsForUtilisateurAndOrganisation(int? organisationId)
        {
            return habilitationCoreManager.GetPermissionsForUtilisateurAndOrganisation(organisationId)
                                          .RemoveDuplicatesPermissions()
                                          .SelectContextuellesPermissions();
        }

        /// <summary>
        /// Retourne la liste de claims,qui correspond aux permissions globlales de l'utilisateur.
        /// </summary>    
        /// <param name="utilisateur">utilisateur</param>
        /// <returns>la liste de claims</returns>
        public IEnumerable<Claim> GetGlobalsClaims(UtilisateurEnt utilisateur)
        {
            var result = new List<Claim>();
            IEnumerable<Claim> globalPermissionsClaims = GetGlobalPermissionsClaims(utilisateur.UtilisateurId);
            result.AddRange(globalPermissionsClaims);
            IEnumerable<Claim> globalRolesClaims = GetGlobalRolesClaims(utilisateur);
            result.AddRange(globalRolesClaims);
            return result;
        }

        /// <summary>
        /// Methode permettant de recuperer une liste de claim correspondant aux permissions globales de l'application.
        /// Par exemple l'affichage des page.
        /// </summary>
        /// <param name="utilisateurId">utilisateurId</param>
        /// <returns>une liste de claims correspondant aux permissions globales et non contextuelles</returns>
        private IEnumerable<Claim> GetGlobalPermissionsClaims(int utilisateurId)
        {
            var result = new List<Claim>();
            var permissions = habilitationCoreManager.GetPermissionsForUtilisateur(utilisateurId)
                                                      .RemoveDuplicatesPermissions()
                                                      .SelectGlobalsPermissions();
            permissions.ToList().ForEach(p => result.Add(CreateGlobalClaim(p.PermissionKey, (int)p.Mode)));
            return result;
        }

        private Claim CreateGlobalClaim(string permissionKey, int mode)
        {
            var claim = new Claim(permissionKey, mode.ToString());

            return claim;
        }

        /// <summary>
        /// Methode permettant de recuperer une liste de claim correspondant aux roles globaux de l'application.
        /// </summary>
        /// <param name="utilisateur">utilisateur</param>
        /// <returns>Une liste de claim correspondant aux roles globaux de l'application</returns>
        private IEnumerable<Claim> GetGlobalRolesClaims(UtilisateurEnt utilisateur)
        {
            var result = new List<Claim>();

            if (utilisateur.SuperAdmin)
            {
                var roleAdmin = new Claim(ClaimTypes.Role, "SuperAdmin");
                result.Add(roleAdmin);
            }
            return result;
        }
    }
}
