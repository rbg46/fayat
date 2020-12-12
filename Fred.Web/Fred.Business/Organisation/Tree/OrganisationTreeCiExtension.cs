using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Organisation.Tree;

namespace Fred.Business.Organisation.Tree
{
    /// <summary>
    /// Classe d'extension pour OrganisationTree centree sur les CIs
    /// </summary>
    public static class OrganisationTreeCiExtension
    {

        /// <summary>
        /// Recupere les cis d'un utilisateur et pour une liste de roles
        /// </summary>
        /// <param name="organisationTree">organisationTree</param>
        /// <param name="userId">userId</param>
        /// <param name="roleIdsRequested">liste de roles</param>
        /// <returns>Liste de cis</returns>
        public static List<OrganisationBase> GetCisByUserAndRoles(this OrganisationTree organisationTree, int userId, List<int> roleIdsRequested)
        {
            IEnumerable<Node<OrganisationBase>> userNodes = organisationTree.Nodes.Where(n => n.Data.Affectations.Any(a => a.UtilisateurId == userId && roleIdsRequested.Contains(a.RoleId)));

            List<OrganisationBase> allChildren = userNodes.SelectMany(un => un.LevelOrder()).Distinct().ToList();

            return allChildren.Where(x => x.IsCi() || x.IsSousCi()).ToList();
        }

        /// <summary>
        /// Retourne les roles ids d'un utilisateur
        /// </summary>
        /// <param name="organisationTree">organisationTree</param>
        /// <param name="userId">userId</param>
        /// <returns>Liste de roles ids</returns>
        public static List<int> GetRolesIdsOfUser(this OrganisationTree organisationTree, int userId)
        {
            IEnumerable<Node<OrganisationBase>> userNodes = organisationTree.Nodes.Where(n => n.Data.Affectations.Any(a => a.UtilisateurId == userId));

            List<int> rolesIdsOfUser = userNodes
                .SelectMany(x => x.Data.Affectations.Where(a => a.UtilisateurId == userId))
                .Select(x => x.RoleId)
                .Distinct()
                .ToList();

            return rolesIdsOfUser;
        }
    }
}
