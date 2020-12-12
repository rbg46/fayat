using System.Collections.Generic;
using Fred.Entities.Commande;

namespace Fred.Business.RepriseDonnees.ValidationCommande.ContextProviders
{
    /// <summary>
    /// Contient les données necessaire pour faire la validation en masse des commandes
    /// </summary>
    public class ContextForValidationCommande
    {
        /// <summary>
        /// les commandes utilisées pour la validation des commande
        /// </summary>
        public List<CommandeEnt> CommandesUsedInExcel { get; set; } = new List<CommandeEnt>();// init evite les erreur NullReferenceException

        /// <summary>
        /// Le statut commande validée
        /// </summary>
        public StatutCommandeEnt StatutCommandeValidee { get; set; }
    }
}
