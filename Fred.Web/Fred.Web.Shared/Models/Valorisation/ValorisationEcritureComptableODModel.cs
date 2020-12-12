namespace Fred.Web.Shared.Models.Valorisation
{
    /// <summary>
    /// Modèle utiliser lors de la génération des OD pour l'intégration des écritures de Fayat TP
    /// </summary>
    public class ValorisationEcritureComptableODModel
    {
        /// <summary>
        /// Valorisation ID
        /// </summary>
        public int ValorisationId { get; set; }

        /// <summary>
        /// Quantité
        /// </summary>
        public decimal Quantite { get; set; }

        /// <summary>
        /// Unité
        /// </summary>
        public string Unite { get; set; }

        /// <summary>
        /// Identifiant de l'unité
        /// </summary>
        public int UniteId { get; set; }

        /// <summary>
        /// Montant
        /// </summary>
        public decimal Montant { get; set; }

        /// <summary>
        /// Identifiant de la ligne du rapport
        /// </summary>
        public int RapportLigneId { get; set; }

        /// <summary>
        /// PUHT
        /// </summary>
        public decimal PUHT { get; set; }

        /// <summary>
        /// Identifiant du personnel
        /// </summary>
        public int? PersonnelId { get; set; }

        /// <summary>
        /// Identifiant du matériel
        /// </summary>
        public int? MaterielId { get; set; }
    }
}
