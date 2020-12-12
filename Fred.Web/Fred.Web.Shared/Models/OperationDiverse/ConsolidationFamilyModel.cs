namespace Fred.Web.Shared.Models.OperationDiverse
{
    /// <summary>
    /// Informations d'une famille d'OD pour la consolidation des écritures
    /// </summary>
    public class ConsolidationFamilyModel
    {
        /// <summary>
        /// Identifiant de la famille d'OD
        /// </summary>
        public int FamilyId { get; set; }

        /// <summary>
        /// Nom de la famille d'OD
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// Code de la famille d'OD
        /// </summary>
        public string FamilyCode { get; set; }

        /// <summary>
        /// Indique si les écritures de la famille doivent avoir un bon de commande
        /// </summary>
        public bool MustHaveOrder { get; set; }

        /// <summary>
        /// Indique si les montants des écritures comptables sont cumulées
        /// </summary>
        public bool IsAccrued { get; set; }

        /// <summary>
        /// Montant calculé depuis les données de Fred
        /// </summary>
        public decimal FredAmount { get; set; }

        /// <summary>
        /// Montant de la comptabilité, importé
        /// </summary>
        public decimal AccountingAmount { get; set; }

        /// <summary>
        /// Ecart entre les montants Fred et comptable
        /// </summary>
        public decimal GapAmount
        {
            get { return AccountingAmount - FredAmount; }
        }
    }
}
