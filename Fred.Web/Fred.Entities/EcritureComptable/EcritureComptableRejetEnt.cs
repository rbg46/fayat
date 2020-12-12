using System;

namespace Fred.Entities.EcritureComptable
{
    /// <summary>
    /// Table regourpant les écritures comptables rejetées.
    /// </summary>
    public class EcritureComptableRejetEnt
    {
        private DateTime dateRejet;

        /// <summary>
        /// Obtiens ou définit l'identifiant unique d'une écriture comptable rejetée
        /// </summary>
        /// 
        public int EcritureComptableRejet { get; set; }

        /// <summary>
        /// Obtiens ou définit le numéro de la piéce rejetée
        /// </summary>
        public string NumeroPiece { get; set; }

        /// <summary>
        /// Obtiens ou définit la date du rejet
        /// </summary>
        public DateTime DateRejet
        {
            get { return DateTime.SpecifyKind(dateRejet, DateTimeKind.Utc); }
            set { dateRejet = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
        }
        /// <summary>
        /// Obtiens ou définit l'identifiant du CI
        /// </summary>
        public int CiID { get; set; }

        /// <summary>
        /// Obtiens ou définit la raison du rejet
        /// </summary>
        public string RejetMessage { get; set; }
    }
}
