using Fred.Entities.Affectation;
using System;
using System.Threading.Tasks;

namespace Fred.Business.Astreinte
{
    /// <summary>
    /// Gestionnaire des Astreintes
    /// </summary>
    public interface IAstreinteManager : IManager<AstreinteEnt>
    {
        /// <summary>
        /// Récupération ou New d'une Astreinte liée à une Affectation
        /// </summary>
        /// <param name="affectation">Affectation à laquelle est liée l'astreinte</param>
        /// <param name="date">date de l'astreinte</param>
        /// <returns>Entité Astreinte</returns>
        Task<AstreinteEnt> GetOrNewAstreinteAsync(AffectationEnt affectation, DateTime date);

        /// <summary>
        /// Supprime une astreinte
        /// </summary>
        /// <param name="astreinte">Astreinte à supprimer</param>
        void Delete(AstreinteEnt astreinte);
    }
}
