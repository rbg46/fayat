namespace Fred.Web.Shared.Models
{
    /// <summary>
    /// View Model Commande Ligne Enérgie
    /// </summary>
    public class LigneSupplementaireExportModel
    {
        /// <summary>
        ///   Obtient ou définit le libellé/désignation d'une ligne de commande.
        /// </summary>        
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit l'objet ressource
        /// </summary>        
        public string Ressource { get; set; }

        /// <summary>
        ///  Obtient ou définit l'objet Tache
        /// </summary>
        public string Tache { get; set; }

        /// <summary>
        ///   Obtient ou définit la quantité (ligne supplémentaire) OU la quantité ajustée (pointage et ajustement)
        /// </summary>        
        public decimal? Quantite { get; set; }

        /// <summary>
        ///   Obtient ou définit l'unité 
        /// </summary>        
        public string Unite { get; set; }

        /// <summary>
        ///   Obtient ou définit le prix unitaire (ligne supplémentaire) OU prix unitaire ajusté (pointage et ajustement)
        /// </summary>        
        public decimal PUHT { get; set; }

        /// <summary>
        /// Obtient ou définit le montant HT de la ligne sup.
        /// </summary>
        public decimal? MontantHT
        {
            get { return Quantite * PUHT; }
        }

    }
}
