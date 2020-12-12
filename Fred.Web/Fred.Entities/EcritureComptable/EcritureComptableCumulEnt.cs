using System;

namespace Fred.Entities.EcritureComptable
{
    /// <summary>
    /// Table regroupant les écritures comptables dont la famille d'OD est cumulée
    /// Cette table permet de ne pas réintégré les écritures comptables pour un mois 
    /// Les données dans cette table ont une durée de vie d'un mois
    /// </summary>
    public class EcritureComptableCumulEnt
    {
        private DateTime dateCreation;
        private DateTime? dateComptable;

        /// <summary>
        /// Obtiens ou définit l'identifiant unique d'une écriture comptable cumulée
        /// </summary>
        public int EcritureComptableCumulId { get; set; }

        /// <summary>
        ///   Obtient ou définit la date du Import
        /// </summary>
        public DateTime DateCreation
        {
            get { return DateTime.SpecifyKind(dateCreation, DateTimeKind.Utc); }
            set { dateCreation = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
        }

        /// <summary>
        /// Obtiens ou définit la date comptable de l'écriture comptable
        /// </summary>
        public DateTime? DateComptable
        {
            get { return (dateComptable.HasValue) ? DateTime.SpecifyKind(dateComptable.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateComptable = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        /// Obtiens ou définit le numéro de pièce
        /// </summary>
        public string NumeroPiece { get; set; }

        /// <summary>
        /// Obtiens ou définit l'identifiant du CI
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Obtient le montant HT de la écriture comptable
        /// </summary>
        public decimal Montant { get; set; }

        /// <summary>
        /// Obtiens ou définis l'identifiant de l'écriture comptable 
        /// </summary>
        public int EcritureComptableId { get; set; }
    }
}
