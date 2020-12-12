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
    public class AvisCommandeRepository : FredRepository<AvisCommandeEnt>, IAvisCommandeRepository
    {
        public AvisCommandeRepository(FredDbContext context)
          : base(context)
        {

        }

        /// <summary>
        /// Ajouter une relation avis - commande
        /// </summary>
        /// <param name="avisCommande">AvisCommande à ajouter</param>
        /// <returns>AvisCommande ajoutée (attachée)</returns>
        public void Add(AvisCommandeEnt avisCommande)
        {
            // Afin de ne pas attacher l'objet en paramètre
            AvisCommandeEnt avisCommandeToAdd = new AvisCommandeEnt()
            {
                Avis = avisCommande.Avis,
                CommandeId = avisCommande.CommandeId
            };

            // Retourner l'object attaché
            Context.AvisCommandes.Add(avisCommandeToAdd);
        }

        /// <summary>
        /// Récupérer l'historique des avis sur une validation de commande
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <returns>Historique des avis</returns>
        public List<AvisEnt> GetAvisByCommandeId(int commandeId)
        {
            return Context.AvisCommandes
                .Where(x => x.CommandeId == commandeId)
                .Select(y => y.Avis)
                .Include(x => x.Expediteur.Personnel)
                .Include(x => x.Destinataire.Personnel)
                .AsNoTracking()
                .ToList();
        }
    }
}
