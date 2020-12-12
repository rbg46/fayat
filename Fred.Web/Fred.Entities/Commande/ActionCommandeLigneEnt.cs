using Fred.Entities.Action;
using System;

namespace Fred.Entities.Commande
{
    /// <summary>
    ///   Représente une action.
    /// </summary>
    [Serializable]
    public class ActionCommandeLigneEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant action commande ligne.
        /// </summary>
        public int ActionCommandeLigneId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la commande ligne.
        /// </summary>
        public int CommandeLigneId { get; set; }

        /// <summary>
        ///   Obtient ou définit la ligne de commande
        /// </summary>
        public CommandeLigneEnt CommandeLigne { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'action.
        /// </summary>
        public int ActionId { get; set; }

        /// <summary>
        ///   Obtient ou définit le type d'action
        /// </summary>
        public ActionEnt Action { get; set; }
    }
}
