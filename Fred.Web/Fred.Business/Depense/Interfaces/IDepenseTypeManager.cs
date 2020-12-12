using Fred.Entities.Depense;

namespace Fred.Business.Depense
{
    /// <summary>
    /// Interface Depense Type manager
    /// </summary>
    public interface IDepenseTypeManager : IManager<DepenseTypeEnt>
    {
        /// <summary>
        ///     Récupère un DepenseType par son code
        /// </summary>
        /// <param name="code">Code (entier)</param>
        /// <returns>DepenseTypeEnt</returns>
        DepenseTypeEnt Get(int code);

        /// <summary>
        ///     Récupère un DepenseType par son code sans le tracker dans le context
        /// </summary>
        /// <param name="code">Code (entier)</param>
        /// <returns>DepenseTypeEnt</returns>
        DepenseTypeEnt GetByCode(int code);
    }
}
