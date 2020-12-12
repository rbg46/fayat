using System;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Affectation;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Astreintes
{
    /// <summary>
    /// Repository des Astreintes
    /// </summary>
    public class AstreinteRepository : FredRepository<AstreinteEnt>, IAstreinteRepository
    {
        public AstreinteRepository(FredDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Récupération ou New d'une Astreinte liée à une Affectation
        /// </summary>
        /// <param name="affectation">Affectation à laquelle est liée l'astreinte</param>
        /// <param name="date">date de l'astreinte</param>
        /// <returns>Entité Astreinte</returns>
        public async Task<AstreinteEnt> GetOrNewAstreinteAsync(AffectationEnt affectation, DateTime date)
        {
            AstreinteEnt astreinte = await Context.Astreinte.FirstOrDefaultAsync(x => x.AffectationId == affectation.AffectationId
                                                                        && x.DateAstreinte.Date == date.Date);
            if (astreinte == null)
            {
                astreinte = new AstreinteEnt
                {
                    Affectation = affectation,
                    AffectationId = affectation.AffectationId,
                    DateAstreinte = date
                };
            }

            return astreinte;
        }
    }
}
