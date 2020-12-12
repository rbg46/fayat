using Fred.Entities.Organisation.Tree;

namespace Fred.Business.Organisation.Tree
{

    /// <summary>
    /// Service qui retourne l'arbre des organisations de Fayat
    /// </summary>
    public interface IOrganisationTreeService : IService
    {
        /// <summary>
        /// Retourne l'arbre des organisations de Fayat ou le recupere du cache s'il existe en cache.
        /// Construit l"abre des organisations de fred avec les organisations et les affectations de tous les utilisateurs
        /// </summary>
        /// <param name="forceCreation">Force la creation de l'arbre</param>
        /// <returns>l'arbre des organisations de Fayat</returns>
        OrganisationTree GetOrganisationTree(bool forceCreation = false);
    }
}
