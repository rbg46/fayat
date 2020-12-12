using Fred.Entities.Personnel;
using System;

namespace Fred.Entities.Action
{
    /// <summary>
    ///   Représente une action.
    /// </summary>
    [Serializable]
    public class ActionEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant action commande ligne.
        /// </summary>
        public int ActionId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du action type.
        /// </summary>
        public int ActionTypeId { get; set; }

        /// <summary>
        ///   Obtient ou définit le type d'action
        /// </summary>
        public ActionTypeEnt ActionType { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du action job.
        /// </summary>
        public int? ActionJobId { get; set; }

        /// <summary>
        ///   Obtient ou définit le job action
        /// </summary>
        public ActionJobEnt ActionJob { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du action type.
        /// </summary>
        public int? ActionStatusId { get; set; }

        /// <summary>
        ///   Obtient ou définit le statut de l'action
        /// </summary>
        public ActionStatusEnt ActionStatus { get; set; } = null;

        /// <summary>
        ///   Obtient ou définit la date de l'action.
        /// </summary>
        public DateTime DateAction { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id du personnel qui a cree l'action.
        /// </summary>
        public int? AuteurId { get; set; }

        /// <summary>
        ///   Obtient ou définit le message.
        /// </summary>
        public string Message { get; set; }
    }
}
