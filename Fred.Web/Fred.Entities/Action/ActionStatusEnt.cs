using System;

namespace Fred.Entities.Action
{
    /// <summary>
    ///   Représente un statut d'action.
    /// </summary>
    [Serializable]
    public class ActionStatusEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un statut d'action.
        /// </summary>
        public int ActionStatusId { get; set; }

        /// <summary>
        ///   Obtient le nom du statut.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///   Obtient la description du statut.
        /// </summary>
        public string Description { get; set; }
    }
}
