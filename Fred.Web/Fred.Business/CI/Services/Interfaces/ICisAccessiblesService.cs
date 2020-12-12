using System.Collections.Generic;
using Fred.Entities.Organisation.Tree;

namespace Fred.Business.CI.Services
{
    /// <summary>
    /// Service qui permet de savoir quel sont les cis accessibleS par un utilisateur
    /// </summary>
    public interface ICisAccessiblesService : IService
    {
        /// <summary>
        ///  Permet de savoir quel sont les cis accessibleS par un utilisateur et pour une permission
        /// </summary>
        /// <param name="userId">utilisateur</param>
        /// <param name="permissionRequested">permission</param>
        /// <returns>Liste de cis</returns>
        List<OrganisationBase> GetCisAccessiblesForUserAndPermission(int userId, string permissionRequested);

        /// <summary>
        /// Filtre une liste d'identifiant ci pour ne récupérer que les ci apte à l'export
        /// </summary>
        /// <param name="ciIds">liste des identifiants ci</param>
        /// <returns>liste des identifiants ci filtré</returns>
        List<int> GetCiIdsAvailablesForReceptionInterimaire(IEnumerable<int> ciIds);
    }
}
