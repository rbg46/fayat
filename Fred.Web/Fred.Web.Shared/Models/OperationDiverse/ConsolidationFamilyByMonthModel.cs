using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.OperationDiverse
{
    /// <summary>
    /// Informations d'une famille d'OD pour la consolidation des écritures
    /// </summary>
    public class ConsolidationFamilyByMonthModel : ConsolidationFamilyModel
    {

        /// <summary>
        /// Liste des montants issue de FRED par mois
        /// </summary>
        public List<Tuple<DateTime, decimal>> ListFredAmountByMonth { get; set; }

        /// <summary>
        /// Liste des montants issue d'ANAEL par mois
        /// </summary>
        public List<Tuple<DateTime, decimal>> ListAccountAmountByMonth { get; set; }

        /// <summary>
        /// Liste des montant d'écart par mois
        /// </summary>
        public List<Tuple<DateTime, decimal>> ListGapAmountByMonth { get; set; }
    }
}
