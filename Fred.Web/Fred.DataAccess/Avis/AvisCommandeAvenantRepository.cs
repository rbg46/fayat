using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.Entities.Avis;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Avis
{
    /// <summary>
    /// Repository des avis
    /// </summary>
    public class AvisCommandeAvenantRepository : FredRepository<AvisCommandeAvenantEnt>, IAvisCommandeAvenantRepository
    {
        public AvisCommandeAvenantRepository(FredDbContext context)
          : base(context)
        {

        }

        /// <summary>
        /// Récupérer l'historique des avis sur une validation d'un avenat sur une commande
        /// </summary>
        /// <param name="commandeAvenantId">Identifiant de l'avenant sur une commande</param>
        /// <returns>Historique des avis</returns>
        public List<AvisEnt> GetAvisByCommandeAvenantId(int commandeAvenantId)
        {
            return Context.AvisCommandesAvenants
                .Where(x => x.CommandeAvenantId == commandeAvenantId)
                .Select(y => y.Avis)
                .Include(x => x.Expediteur.Personnel)
                .Include(x => x.Destinataire.Personnel)
                .AsNoTracking()
                .ToList();
        }
    }
}
