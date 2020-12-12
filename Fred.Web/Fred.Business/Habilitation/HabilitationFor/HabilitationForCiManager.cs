using System.Collections.Generic;
using System.Linq;
using Fred.Business.CI;
using Fred.Business.Habilitation.Interfaces;
using Fred.Business.Organisation;
using Fred.Business.Utilisateur;
using Fred.Entities;
using Fred.Entities.Habilitation;
using Fred.Entities.Permission;
using Fred.Framework.Extensions;

namespace Fred.Business.Habilitation
{
    /// <summary>
    /// Manager pour les habilitation des ci
    /// </summary>
    public class HabilitationForCiManager : IHabilitationForCiManager
    {
        private readonly IHabilitationManager habilitationManager;
        private readonly ICIManager ciManager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IOrganisationManager organisationManager;

        /// <summary>
        /// ctor
        /// </summary>     
        /// <param name="habilitationManager">habilitationManager</param>
        /// <param name="utilisateurManager">utilisateurManager</param>
        /// <param name="ciManager">ciManager</param>  
        /// <param name="organisationManager">Gestionnaire des organisations</param>
        public HabilitationForCiManager(IHabilitationManager habilitationManager,
                                        IUtilisateurManager utilisateurManager,
                                        ICIManager ciManager,
                                        IOrganisationManager organisationManager)
        {
            this.habilitationManager = habilitationManager;
            this.ciManager = ciManager;
            this.utilisateurManager = utilisateurManager;
            this.organisationManager = organisationManager;
        }

        /// <summary>
        /// Retourne les habilitations en fonction de l'id de l'entité.
        /// Si id = null cela renvoie les habilitations globales.
        /// </summary>   
        /// <param name="id">id</param>
        /// <returns>HabilitationEnt</returns>
        public HabilitationEnt GetHabilitationForEntityId(int? id = null)
        {
            HabilitationEnt globalsHabilitation = habilitationManager.GetHabilitation();
            globalsHabilitation.PermissionsContextuelles = GetContextuellesPermissionsForEntityId(id);

            return globalsHabilitation;
        }

        /// <summary>
        /// Retourne une liste de permissions contextuelles en fonction de l'id de l'entité
        /// </summary>
        /// <param name="id">id de l'entité</param>
        /// <returns>Liste de permissions contextuelles</returns>
        public IEnumerable<PermissionEnt> GetContextuellesPermissionsForEntityId(int? id = null)
        {
            if (id.HasValue)
            {
                int? organisationId = this.ciManager.GetOrganisationIdByCiId(id.Value);
                if (organisationId.HasValue)
                {
                    return habilitationManager.GetContextuellesPermissionsForUtilisateurAndOrganisation(organisationId);
                }
            }

            return new List<PermissionEnt>();
        }

        /// <summary>
        /// Determine si on a accès a l'entité. 
        /// </summary>
        /// <param name="id">id de l'entité</param>
        /// <param name="nullIsOk">Si on passe null alors nous somme Authorisé</param>
        /// <returns>true si authorisé</returns>
        public bool IsAuthorizedForEntity(int? id = default(int?), bool nullIsOk = true)
        {
            if (id.HasValue)
            {
                int? organisationId = this.ciManager.GetOrganisationIdByCiId(id.Value);
                if (!organisationId.HasValue) { return false; }

                int utilisateurId = utilisateurManager.GetContextUtilisateurId();
                var orgaList = organisationManager.GetOrganisationsAvailable(null, new List<int> { OrganisationType.Ci.ToIntValue(), OrganisationType.SousCi.ToIntValue() }, utilisateurId);
                return orgaList.Any(o => o.OrganisationId == organisationId.Value);
            }

            return nullIsOk;
        }

        /// <summary>
        /// Get the list of the current user's contextual authorizations for a specific permission key
        /// </summary>
        /// <param name="ciId">CI identifier</param>
        /// <param name="permissionKey">Identifier of the permission</param>
        /// <returns>A list of contextual permission</returns>
        public IEnumerable<PermissionEnt> GetContextualAuthorization(int ciId, string permissionKey)
        {
            int? organisationId = ciManager.GetOrganisationIdByCiId(ciId);
            IEnumerable<PermissionEnt> permissions = habilitationManager.GetContextuellesPermissionsForUtilisateurAndOrganisation(organisationId);

            return permissions?.Any() == true ? permissions.Where(p => p.PermissionKey == permissionKey) : null;
        }
    }
}
