using System;
using System.Diagnostics;

namespace Fred.Entities.ActivitySummary
{
    /// <summary>
    /// Contient le resultat d'une requette a la db pour un Ci et un type de requette
    /// Exemple : pour la requette de la recherche du Jalon « Clôture Dépenses et pour le CiId = 419 la 'Date' du jalon est 23/02/2018
    /// </summary>
    [DebuggerDisplay("CiId = {CiId} RequestName = {RequestName} Date = {Date}")]
    public class ActivityRequestWithDateResult
    {

        /// <summary>
        /// Nom de la requette
        /// </summary>
        public TypeJalon RequestName { get; set; }
        /// <summary>
        /// Ciid
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Nombre correspondant a la date recherchée
        /// </summary>
        public DateTime? Date { get; set; }
    }
}
