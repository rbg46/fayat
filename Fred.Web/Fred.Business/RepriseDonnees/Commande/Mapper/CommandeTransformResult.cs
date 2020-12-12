using System.Collections.Generic;
using Fred.Entities.Commande;
using Fred.Entities.Depense;

namespace Fred.Business.RepriseDonnees.Commande.Mapper
{
    /// <summary>
    /// Container du resultat de la creation des commandes , commandes lignes et des receptions
    /// </summary>
    public class CommandeTransformResult
    {

        /// <summary>
        /// Contient les commandes céees
        /// </summary>
        public List<CommandeEnt> Commandes { get; internal set; } = new List<CommandeEnt>();
        /// <summary>
        /// Contient les receptions céees
        /// </summary>
        public List<DepenseAchatEnt> Receptions { get; internal set; } = new List<DepenseAchatEnt>();

        /// <summary>
        /// Contient les commandes lignes crées
        /// </summary>
        public List<CommandeLigneEnt> CommandeLignes { get; internal set; } = new List<CommandeLigneEnt>();
    }
}
