using System;
using Fred.Entities.OperationDiverse;

namespace Fred.Web.Shared.Models.EcritureComptable
{
    /// <summary>
    /// Modele des Ecriture Comptable
    /// </summary>
    public class EcritureComptableModel
    {
        /// <summary>
        /// Date Comptable
        /// </summary>
        public DateTime? DateComptable { get; set; }

        /// <summary>
        /// Libelle de l'écriture Comptable
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Numéro Pièce de l'écriture comptable
        /// </summary>
        public string NumeroPiece { get; set; }

        /// <summary>
        /// Montant de l'écriture comptable
        /// </summary>
        public decimal Montant { get; set; }

        /// <summary>
        /// Devise
        /// </summary>
        public int DeviseId { get; set; }

        /// <summary>
        /// Famille d'OD
        /// </summary>
        public int FamilleOperationDiverseId { get; set; }

        /// <summary>
        /// Nombre d'OD rattaché a une écriture comptable
        /// </summary>
        public int NombreOD { get; set; }

        /// <summary>
        /// Montant total des OD rattaché à une écriture comptable
        /// </summary>
        public decimal MontantTotalOD { get; set; }

        /// <summary>
        /// Famille d'operation diverse
        /// </summary>
        public FamilleOperationDiverseEnt FamilleOperationDiverse { get; set; }

        /// <summary>
        /// Identifiant de la commande associé à l'écriture comptable
        /// </summary>
        public int? CommandeId { get; set; }
    }
}


