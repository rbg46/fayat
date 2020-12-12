using System;
using System.Collections.Generic;
using System.Text;

namespace Fred.Entities.Budget
{
    /// <summary>
    /// Tache nécessaire au recalage budgétaire
    /// </summary>
    public class TacheRecalageParameter
    {
        /// <summary>
        /// Identifiant de la tâche
        /// </summary>
        public int TacheId { get; set; }

        /// <summary>
        /// Code de la tâche
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Type de la tâche
        /// </summary>
        public int TacheType { get; set; }
    }
}
