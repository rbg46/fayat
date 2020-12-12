using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Framework;
using Fred.Framework.Exceptions;
using System.Linq;
using Fred.EntityFramework;

namespace Fred.DataAccess.Parametre
{
    /// <summary>
    ///   Référentiel de données pour du personnel.
    /// </summary>
    public class ParametreRepository : FredRepository<ParametreEnt>, IParametreRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="ParametreRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        public ParametreRepository(FredDbContext context)
          : base(context)
        { }

        /// <summary>
        /// Retourne un paramètre.
        /// </summary>
        /// <param name="parametreId">Identifiant du paramètre.</param>
        /// <param name="groupeId">Le groupe si le paramètre en dépend, sinon null.</param>
        /// <returns>le paramètre ou null s'il n'existe pas.</returns>
        public ParametreEnt Get(ParametreId parametreId, int? groupeId = null)
        {
            var code = ((int)parametreId).ToString();

            if (parametreId == ParametreId.URLScanFacture)
            {
                if (groupeId == null)
                {
                    throw new FredRepositoryException($"L'identifiant du groupe est requis pour le paramètre code {code}");
                }
            }
            else
            {
                if (groupeId != null)
                {
                    throw new FredRepositoryException($"L'identifiant du groupe n'est pas requis pour le paramètre code {code}");
                }
            }

            var parametres = Context.Parametres.Where(p => p.Code == code && p.GroupeId == groupeId).ToList();
            if (parametres.Count == 0)
            {
                return null;
            }
            else if (parametres.Count == 1)
            {
                return parametres[0];
            }
            else if (groupeId == null)
            {
                throw new FredRepositoryException($"Plusieurs paramètres correspondent au code {code}");
            }
            else
            {
                throw new FredRepositoryException($"Plusieurs paramètres correspondent au code {code} pour le groupe d'identifiant {groupeId}");
            }
        }
    }
}
