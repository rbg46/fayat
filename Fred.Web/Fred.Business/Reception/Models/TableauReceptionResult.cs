using System.Collections.Generic;
using Fred.Entities.Depense;

namespace Fred.Business.Reception
{
    /// <summary>
    /// Représente l'object résultat de la récupération de la liste des réceptions
    /// </summary>
    public class TableauReceptionResult
    {
        /// <summary>
        ///   Obtient ou définit la liste des réceptions
        /// </summary>
        public List<DepenseAchatEnt> Receptions { get; set; } = new List<DepenseAchatEnt>();

        /// <summary>
        ///   Obtient ou définit la somme des solde far
        /// </summary>
        public decimal SoldeFarTotal { get; set; }

        /// <summary>
        ///   Obtient ou définit la somme des montants HT
        /// </summary>
        public decimal MontantHTTotal { get; set; }

        /// <summary>
        ///     Obtient ou définit le nombre de total de réceptions (non paginées)
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Toutes les receptions 
        /// </summary>
        public List<DepenseAchatEnt> AllReceptionsForTotals { get; internal set; }

        /// <summary>
        /// Touts les ids des reception du filtre
        /// </summary>
        public List<int> AllReceptionsIds { get; internal set; }

        /// <summary>
        /// Touts les ids des reception visable du filtre
        /// </summary>
        public List<int> AllVisableReceptionIds { get; internal set; }
    }
}
