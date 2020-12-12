using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Rapport;
using Fred.EntityFramework;

namespace Fred.DataAccess.Rapport.Pointage
{
    /// <summary>
    ///   Référentiel de données pour les lignes d'astreinte.
    /// </summary>
    public class RapportLigneAstreinteRepository : FredRepository<RapportLigneAstreinteEnt>, IRapportLigneAstreinteRepository
    {
        private readonly IUnitOfWork uow;
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="RapportLigneAstreinteRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        /// <param name="uow">UnitOfWork</param>
        public RapportLigneAstreinteRepository(FredDbContext context)
          : base(context)
        {
            this.uow = uow;
        }

        /// <summary>
        /// Find rapport ligne astreinte by rapport ligne id and astreinte id
        /// </summary>
        /// <param name="rapportLigneId">Rapport ligne identifier</param>
        /// <param name="astreinteId">astreinte identifier</param>
        /// <returns>Rapport ligne astreinte</returns>
        public RapportLigneAstreinteEnt FindAstreinte(int rapportLigneId, int astreinteId)
        {
            return this.Context.RapportLigneAstreintes.FirstOrDefault(x => x.RapportLigneId == rapportLigneId && x.AstreinteId == astreinteId);
        }

        /// <summary>
        /// Delete astreinte by Id
        /// </summary>
        /// <param name="astreinteId">astreinteId</param>
        public void DeleteAstreintesById(int astreinteId)
        {
            var astreinteToDelite = Context.RapportLigneAstreintes;
            RapportLigneAstreinteEnt astreinte = astreinteToDelite.Find(astreinteId);
            if (astreinte != null)
            {
                astreinteToDelite.Remove(astreinte);
            }
        }
    }
}
