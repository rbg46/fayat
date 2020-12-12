using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Rapport;
using Fred.Framework;
using System.Linq;
using Fred.EntityFramework;

namespace Fred.DataAccess.Rapport.Pointage
{
    /// <summary>
    /// Référentiel de données pour les lignes de majoration.
    /// </summary>
    public class RapportLigneMajorationRepository : FredRepository<RapportLigneMajorationEnt>, IRapportLigneMajorationRepository
    {
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="RapportLigneMajorationRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        public RapportLigneMajorationRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        /// Find rapport majoration by rapport ligne id and majoration id
        /// </summary>
        /// <param name="rapportLigneId">Rapport ligne identifier</param>
        /// <param name="codeMajorationId">Code majoration identifier</param>
        /// <returns>Rapport ligne majoration</returns>
        public RapportLigneMajorationEnt FindMajorationByMajorationId(int rapportLigneId, int codeMajorationId)
        {
            return this.Context.RapportLigneMajorations.FirstOrDefault(x => x.RapportLigneId == rapportLigneId && x.CodeMajorationId == codeMajorationId);
        }
    }
}
