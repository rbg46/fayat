using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.ReferentielEtendu;
using Fred.EntityFramework;
using Fred.Framework;

namespace Fred.DataAccess.ReferentielEtendu
{
    /// <summary>
    ///   Référentiel de données pour les unités/référentiel étendu
    /// </summary>
    public class UniteReferentielEtenduRepository : FredRepository<UniteReferentielEtenduEnt>, IUniteReferentielEtenduRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="UniteReferentielEtenduRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        public UniteReferentielEtenduRepository(FredDbContext context)
          : base(context)
        {
        }
    }
}
