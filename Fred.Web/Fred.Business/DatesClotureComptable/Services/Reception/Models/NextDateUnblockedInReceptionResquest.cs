using System;
using System.Diagnostics;

namespace Fred.Business.DatesClotureComptable.Reception.Models
{
    /// <summary>
    /// Object qui permet de faire un requette pour savoir qu'elle est la prochaine date disponible en reception
    /// </summary>
    [DebuggerDisplay("CiId = {CiId} StartDate = {StartDate}")]

    public class NextDateUnblockedInReceptionResquest
    {
        /// <summary>
        /// Date a partir de laquelle on fait la recherche
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Le ci Concernée
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// tostring
        /// </summary>
        /// <returns>tosotring</returns>
        public override string ToString()
        {
            return $"CiId = {CiId} StartDate = {StartDate }";
        }

    }
}
