using System.Diagnostics;

namespace Fred.Business.DatesClotureComptable.Reception.Models
{
    /// <summary>
    /// Object qui permet de faire un requette pour savoir si le ci est bloqué pour une annee et un mois donnée
    /// </summary>
    [DebuggerDisplay("CiId = {CiId} Year = {Year} Month = {Month} ")]
    public class ReceptionBlockedResquest
    {
        /// <summary>
        /// Le cidi
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// L'année
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Le mois
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// tostring
        /// </summary>
        /// <returns>tosotring</returns>
        public override string ToString()
        {
            return $"CiId = {CiId} Year = {Year} Month = {Month}";
        }

    }
}
