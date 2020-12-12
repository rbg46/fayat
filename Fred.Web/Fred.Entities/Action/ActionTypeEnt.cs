using System;

namespace Fred.Entities.Action
{
    /// <summary>
    ///   Représente un type d'action.
    /// </summary>
    [Serializable]
    public class ActionTypeEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un type d'action.
        /// </summary>
        public int ActionTypeId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'un type d'action.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'un type de commande.
        /// </summary>
        public string Libelle { get; set; }
    }
}
