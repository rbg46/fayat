using System;
using System.Threading.Tasks;
using Fred.Entities.Affectation;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Interface du répository des Astreintes
    /// </summary>
    public interface IAstreinteRepository : IRepository<AstreinteEnt>
    {
        /// <summary>
        /// Récupération ou New d'une Astreinte liée à une Affectation
        /// </summary>
        /// <param name="affectation">Affectation à laquelle est liée l'astreinte</param>
        /// <param name="date">date de l'astreinte</param>
        /// <returns>Entité Astreinte</returns>
        Task<AstreinteEnt> GetOrNewAstreinteAsync(AffectationEnt affectation, DateTime date);
    }
}
