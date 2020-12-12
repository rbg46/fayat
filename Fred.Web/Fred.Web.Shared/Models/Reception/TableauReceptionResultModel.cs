using System.Collections.Generic;
using Fred.Web.Models.Depense;

namespace Fred.Web.Shared.Models
{
    /// <summary>
    /// Représente l'object résultat de la récupération de la liste des réceptions
    /// </summary>
    public class TableauReceptionResultModel
    {
        /// <summary>
        ///   Obtient ou définit la liste des réceptions
        /// </summary>
        public List<DepenseAchatModel> Receptions { get; set; }

        /// <summary>
        /// La liste completes des receptions ids du filtre, necessaire pour faire des requetes plus rapide apres le premier chargement
        /// </summary>
        public List<int> AllReceptionsIds { get; set; }

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
        /// Touts les ids des reception visable du filtre
        /// </summary>
        public List<int> AllVisableReceptionIds { get; internal set; }
    }
}
