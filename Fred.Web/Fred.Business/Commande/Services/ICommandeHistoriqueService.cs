using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Entities.Avis;
using Fred.Web.Shared.Models.Commande;

namespace Fred.Business.Commande.Services
{
    /// <summary>
    /// Service permettant la gestion de l'historique d'une commande
    /// </summary>
    public interface ICommandeHistoriqueService : IService
    {
        /// <summary>
        /// Récupérer l'historique d'une commande
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <returns>Historique d'une commande</returns>
        List<CommandeEventModel> GetHistorique(int commandeId);
    }
}
