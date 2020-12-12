using System.Collections.Generic;

namespace Fred.Business.ObjectifFlash.Reporting.Models
{
    /// <summary>
    /// Model d'export d'une tache de bilan flash
    /// </summary>
    public class BilanFlashTacheExportModel
    {
        /// <summary>
        /// Quantite objectif
        /// </summary>
        public decimal? QuantiteObjectif { get; set; }
        
        /// <summary>
        /// Quantite unité
        /// </summary>
        public string QuantiteUnite { get; set; }

        /// <summary>
        /// Quantité réalisée
        /// </summary>
        public decimal? QuantiteRealise { get; set; }
        
        /// <summary>
        /// Rendement Objectif
        /// </summary>
        public decimal? RendementObjectif { get; set; }

        /// <summary>
        /// Rendement unité
        /// </summary>
        public string RendementUnite { get; set; }

        /// <summary>
        /// Rendement réalisé
        /// </summary>
        public decimal? RendementRealise { get; set; }

        /// <summary>
        /// Liste des ressources périmètre
        /// </summary>
        public List<BilanFlashTacheRessourceExportModel> BilanFlashRessourcePerimetre { get; set; }

        /// <summary>
        /// Liste des ressources hors périmètre
        /// </summary>
        public List<BilanFlashTacheRessourceExportModel> BilanFlashRessourceHorsPerimetre { get; set; }

        /// <summary>
        /// Total montant objectif
        /// </summary>
        public decimal? TotalMontantObjectif { get; set; }

        /// <summary>
        /// Total montant réalisé
        /// </summary>
        public decimal? TotalMontantRealise { get; set; }

        /// <summary>
        /// Total montant hors périmètre objectif
        /// </summary>
        public decimal? TotalMontantHorsPerimetreObjectif { get; set; }

        /// <summary>
        /// Total montant hors périmètre réalisé
        /// </summary>
        public decimal? TotalMontantHorsPerimetreRealise { get; set; }

        /// <summary>
        /// Total montant périmètre objectif
        /// </summary>
        public decimal? TotalMontantPerimetreObjectif { get; set; }

        /// <summary>
        /// Total montant périmètre réalisé
        /// </summary>
        public decimal? TotalMontantPerimetreRealise { get; set; }

        /// <summary>
        /// Cout unitaire objectif
        /// </summary>
        public decimal? CoutUnitaireObjectif { get; set; }

        /// <summary>
        /// Cout unitaire réalisé
        /// </summary>
        public decimal? CoutUnitaireRealise { get; set; }

        /// <summary>
        /// Cout unitaire unité
        /// </summary>
        public string CoutUnitaireUnite { get; set; }
    }
}
