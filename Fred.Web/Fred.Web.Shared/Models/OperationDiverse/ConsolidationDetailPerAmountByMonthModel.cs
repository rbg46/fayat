using System;
using System.Collections.Generic;
using System.Linq;

namespace Fred.Web.Shared.Models.OperationDiverse
{
    /// <summary>
    /// Modèle permettant récupérer, par mois, la liste des montants issue d'ANAEL et de FRED
    /// </summary>
    public class ConsolidationDetailPerAmountByMonthModel
    {
        /// <summary>
        /// Liste des écritures comptables par mois
        /// </summary>
        public List<Tuple<DateTime, decimal>> ListAccountingAmountByMonth { get; set; }

        /// <summary>
        /// Liste des montants issue de fred par mois
        /// </summary>
        public List<Tuple<DateTime, decimal>> ListFredAmountByMonth { get; set; }

        /// <summary>
        /// Montant total de la somme des écritures comptable
        /// </summary>
        public decimal AccountingAmount { get { return ListAccountingAmountByMonth.Sum(q => q.Item2); } }

        /// <summary>
        /// Montant Total du montant issue de FRED
        /// </summary>
        public decimal FredAmount { get { return ListFredAmountByMonth.Sum(q => q.Item2); } }

        /// <summary>
        /// Liste de montant d'écart par mois
        /// </summary>
        public List<Tuple<DateTime, decimal>> ListGapAmountByMonth { get; set; }
    }
}
