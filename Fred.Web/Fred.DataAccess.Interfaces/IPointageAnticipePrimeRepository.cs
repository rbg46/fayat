
using System.Threading.Tasks;
using Fred.Entities.Rapport;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Interface du repository PointageAnticipePrime
    /// </summary>
    /// <seealso>
    ///   <cref>Fred.DataAccess.Common.IRepository{Fred.Entities.Rapport.PointageAnticipePrimeEnt}</cref>
    /// </seealso>
    public interface IPointageAnticipePrimeRepository : IRepository<PointageAnticipePrimeEnt>
    {
        /// <summary>
        /// Permet de récupérer l'id d'un pointage anticipé lié aà la prime spécifiée.
        /// </summary>
        /// <param name="primeId">Identifiant du code de déplacement</param>
        /// <returns>Retourne l'identifiant du 1er pointage anticipé</returns>
        Task<int> GetPointageAnticipePrimeIdAsync(int primeId);
    }
}