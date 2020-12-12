using System;
using System.Threading.Tasks;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Affectation;

namespace Fred.Business.Astreinte
{
    /// <summary>
    /// Gestionnaire des Astreintes
    /// </summary>
    public class AstreinteManager : Manager<AstreinteEnt, IAstreinteRepository>, IAstreinteManager
    {
        public AstreinteManager(IUnitOfWork uow, IAstreinteRepository astreinteRepository)
            : base(uow, astreinteRepository)
        {
        }

        /// <summary>
        /// Récupération ou création d'une Astreinte liée à une Affectation
        /// </summary>
        /// <param name="affectation">Affectation à laquelle est liée l'astreinte</param>
        /// <param name="date">date de l'astreinte</param>
        /// <returns>Entité Astreinte</returns>
        public async Task<AstreinteEnt> GetOrNewAstreinteAsync(AffectationEnt affectation, DateTime date)
        {
            return await Repository.GetOrNewAstreinteAsync(affectation, date);
        }

        /// <summary>
        /// Supprime une astreinte
        /// </summary>
        /// <param name="astreinte">Astreinte à supprimer</param>
        public void Delete(AstreinteEnt astreinte)
        {
            Repository.Delete(astreinte);
            Save();
        }
    }
}
