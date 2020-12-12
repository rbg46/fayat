using System;

namespace Fred.Web.Shared.Models.OperationDiverse
{
    /// <summary>
    /// Modèle contant le total d'une commande avec son identifiant
    /// Ce modèle est utilisé lors de l'import des écriture comptable
    /// </summary>
    public class OperationDiverseEcritureComptableModel
    {
        /// <summary>
        /// Identifiant de la commande
        /// </summary>
        public int? CommandeId { get; set; }

        /// <summary>
        /// Total
        /// </summary>
        public decimal Total { get; set; }

        public DateTime DateComptable { get; set; }
    }
}
