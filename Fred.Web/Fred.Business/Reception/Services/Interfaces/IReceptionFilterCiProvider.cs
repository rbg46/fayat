using Fred.Entities.Depense;

namespace Fred.Business.Reception.Services
{
    /// <summary>
    /// Service qui determine les cis du filtre
    /// </summary>
    public interface IReceptionFilterCiProvider : IService
    {
        /// <summary>
        /// Initialise les cis dans le filtre
        /// </summary>
        /// <param name="filter">Le filtre</param>
        /// <param name="byPassCurrentUser">byPassCurrentUser</param>
        void InitializeCisOnFilter(SearchDepenseEnt filter, bool byPassCurrentUser);
    }
}
