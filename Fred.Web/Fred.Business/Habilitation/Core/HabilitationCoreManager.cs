using System.Collections.Generic;
using System.Linq;
using Fred.Business.Fonctionnalite;
using Fred.Business.Habilitation.Interfaces;
using Fred.Business.PermissionFonctionnalite;
using Fred.Business.Utilisateur;
using Fred.Entities.Fonctionnalite;
using Fred.Entities.Permission;
using Fred.Entities.PermissionFonctionnalite;
using Fred.Entities.Utilisateur;

namespace Fred.Business.Habilitation.Core
{
    /// <summary>
    /// Manager Core pour les Habilitations.
    /// </summary>
    public class HabilitationCoreManager : IHabilitationCoreManager
    {
        private readonly IPermissionFonctionnaliteManager permissionFonctionnaliteManager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IFonctionnaliteManager fonctionnaliteManager;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="permissionFonctionnaliteManager">permissionFonctionnaliteManager</param>
        /// <param name="utilisateurManager">utilisateurManager</param>  
        /// <param name="fonctionnaliteManager">fonctionnaliteManager</param>   
        public HabilitationCoreManager(IPermissionFonctionnaliteManager permissionFonctionnaliteManager,
                                    IUtilisateurManager utilisateurManager,
                                    IFonctionnaliteManager fonctionnaliteManager)
        {
            this.permissionFonctionnaliteManager = permissionFonctionnaliteManager;
            this.utilisateurManager = utilisateurManager;
            this.fonctionnaliteManager = fonctionnaliteManager;
        }

        /// <summary>
        ///  Recupere toutes les permissions pour l'utilisateur actuel.
        /// </summary>
        /// <param name="utilisateurId">utilisateurId</param>
        /// <returns>Une liste de permissions</returns>
        public IEnumerable<PermissionEnt> GetPermissionsForUtilisateur(int utilisateurId)
        {
            IEnumerable<FonctionnaliteEnt> fonctionnalitesForUtilisateur = fonctionnaliteManager.GetFonctionnalitesForUtilisateur(utilisateurId);
            return GetPermissionForFonctionalites(fonctionnalitesForUtilisateur);
        }

        /// <summary>
        /// Recupere les permissions pour l'utilisateur actuel et pour une organisationId
        /// </summary>
        /// <param name="organisationId">organisationId</param>
        /// <returns>Une liste de permissions</returns>
        public IEnumerable<PermissionEnt> GetPermissionsForUtilisateurAndOrganisation(int? organisationId)
        {
            var result = new List<PermissionEnt>();

            if (organisationId == null)
            {
                return result;
            }
            var utilisateur = utilisateurManager.GetContextUtilisateur();

            return GetPermissionsForUtilisateurAndOrganisation(utilisateur.UtilisateurId, organisationId.Value);
        }

        /// <summary>
        /// Recupere les permissions pour un utilisateur et pour une organisationId
        /// </summary>
        /// <param name="utilisateurId">utilisateurId</param>
        /// <param name="organisationId">organisationId</param>
        /// <returns>Une liste de permissions</returns>
        private IEnumerable<PermissionEnt> GetPermissionsForUtilisateurAndOrganisation(int utilisateurId, int organisationId)
        {
            AffectationSeuilUtilisateurEnt affectationSeuilUtilisateur = utilisateurManager.GetFirstAffectationForOrganisationInTreeWithRoleActif(utilisateurId, organisationId);

            if (affectationSeuilUtilisateur == null)
            {
                return new List<PermissionEnt>();
            }

            IEnumerable<FonctionnaliteEnt> fonctionnalitesForRole = fonctionnaliteManager.GetFonctionnalitesForRoleId(affectationSeuilUtilisateur.RoleId);

            return GetPermissionForFonctionalites(fonctionnalitesForRole).ToList();
        }

        /// <summary>
        ///  Recupere les permissions pour une liste de fonctionnalité
        /// </summary>
        /// <param name="fonctionnalites">fonctionnalites</param>
        /// <returns>Une liste de permissions</returns>
        private IEnumerable<PermissionEnt> GetPermissionForFonctionalites(IEnumerable<FonctionnaliteEnt> fonctionnalites)
        {
            List<PermissionEnt> result = new List<PermissionEnt>();

            IEnumerable<PermissionFonctionnaliteEnt> allPerms = permissionFonctionnaliteManager.GetAllPermissionFonctionnalites().ToList();
            foreach (var fonctionnalite in fonctionnalites)
            {
                var permissions = allPerms.Where(pf => pf.FonctionnaliteId == fonctionnalite.FonctionnaliteId)
                                                                  .Select(pf => pf.Permission)
                                                                  .ToList();
                permissions.ForEach(p => p.Mode = fonctionnalite.Mode);
                result = result.Concat(permissions).ToList();
            }

            return result;
        }
    }
}
