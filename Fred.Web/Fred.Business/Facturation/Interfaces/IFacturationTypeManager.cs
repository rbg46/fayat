using Fred.Entities.Facturation;

namespace Fred.Business.Facturation
{
    /// <summary>
    /// Interface Facturation Type Manager
    /// </summary>
    public interface IFacturationTypeManager : IManager<FacturationTypeEnt>
    {
        /// <summary>
        /// Récupération du type de facturation par son code
        /// </summary>
        /// <param name="code">Code (entier)</param>
        /// <returns>FacturationTypeEnt</returns>
        FacturationTypeEnt Get(int code);
    }
}
