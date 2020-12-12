using System.Threading.Tasks;
using Fred.Entities;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// TypeSocieteRepository interface
    /// </summary>
    public interface ITypeSocieteRepository : IRepository<TypeSocieteEnt>
    {
        /// <summary>
        /// Get TypeSociete by id
        /// </summary>
        /// <param name="typeSocieteId">TypeSociete id</param>
        /// <returns>TypeSociete object</returns>
        Task<TypeSocieteEnt> FindByIdAsync(int typeSocieteId);

        /// <summary>
        /// Get TypeSociete by id
        /// </summary>
        /// <param name="typeSocieteId">TypeSociete id</param>
        /// <returns>TypeSociete object</returns>
        TypeSocieteEnt FindById(int typeSocieteId);
    }
}
