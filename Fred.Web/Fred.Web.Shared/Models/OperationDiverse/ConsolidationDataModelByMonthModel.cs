﻿using System.Collections.Generic;
using System.Linq;

namespace Fred.Web.Shared.Models.OperationDiverse
{
    /// <summary>
    /// Informations d'une famille d'OD pour la consolidation des écritures par mois
    /// </summary>
    public class ConsolidationDataModelByMonthModel
    {
        /// <summary>
        /// Liste des informations sur les familles d'OD, dont les différents montants
        /// </summary>
        public IEnumerable<ConsolidationFamilyByMonthModel> FamiliesAmounts { get; set; }

        /// <summary>
        /// Symbôle monétaire de la devise utilisé
        /// </summary>
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// Montant total des saisies dans Fred
        /// </summary>
        public decimal TotalFredAmount
        {
            get { return FamiliesAmounts.Sum(fa => fa.FredAmount); }
        }

        /// <summary>
        /// Montant total des écritures comptables
        /// </summary>
        public decimal TotalAccountingAmount
        {
            get { return FamiliesAmounts.Sum(fa => fa.AccountingAmount); }
        }

        /// <summary>
        /// Montant total de l'écart entre les saisies Fred et les écritures comptables
        /// </summary>
        public decimal TotalGapAmount
        {
            get { return FamiliesAmounts.Sum(fa => fa.GapAmount); }
        }
    }
}