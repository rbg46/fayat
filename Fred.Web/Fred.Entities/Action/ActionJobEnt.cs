using System;

namespace Fred.Entities.Action
{
    /// <summary>
    ///   Représente un type d'action.
    /// </summary>
    [Serializable]
    public class ActionJobEnt
    {
        /// <summary>
        ///   Obtient ou définit lidentifiant unique .
        /// </summary>
        public int ActionJobId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'un type de commande.
        /// </summary>
        public string ExternalJobId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'un type de commande.
        /// </summary>
        public string ExternalJobName { get; set; }
    }
}
