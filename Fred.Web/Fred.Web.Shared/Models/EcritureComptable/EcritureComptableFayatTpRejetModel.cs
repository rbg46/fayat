using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.EcritureComptable
{
    /// <summary>
    /// Modele des Ecriture Comptable
    /// </summary>
    public class EcritureComptableFayatTpRejetModel
    {
        /// <summary>
        /// Numéro de la pièce en rejet 
        /// </summary>
        public string NumeroPiece { get; set; }

        /// <summary>
        /// Date du rejet
        /// </summary>
        public DateTime DateRejet { get; set; }

        /// <summary>
        /// Identifiant du Ci
        /// </summary>
        public int CiID { get; set; }

        /// <summary>
        /// Messages de rejet
        /// </summary>
        public List<string> RejetMessage { get; set; }
    }
}
