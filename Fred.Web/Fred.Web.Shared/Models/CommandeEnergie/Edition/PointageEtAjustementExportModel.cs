namespace Fred.Web.Shared.Models
{
    /// <summary>
    /// View Model Commande Ligne Enérgie
    /// </summary>
    public class PointageEtAjustementExportModel
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
        ///   Obtient ou définit la quantité (ligne supplémentaire) OU la quantité ajustée (pointage et ajustement)
        /// </summary>        
        public decimal? Quantite { get; set; }

        /// <summary>
        ///   Obtient ou définit la quantité pointée : somme des heures travaillées pointées sur ce Personnel pour le CI SEP et la Période
        /// </summary>        
        public decimal? QuantitePointee { get; set; }

        /// <summary>
        ///   Obtient ou définit la quantité convertie
        /// </summary>        
        public decimal? QuantiteConvertie { get; set; }

        /// <summary>
        ///   Obtient ou définit l'unité barème d’exploitation en application pour le personnel et pour la période
        /// </summary>        
        public string UniteBareme { get; set; }

        /// <summary>
        /// Obtient ou définit la valeur du barème d’exploitation en application pour le personnel et pour la période
        /// </summary>
        public decimal? Bareme { get; set; }

        /// <summary>
        ///   Obtient ou définit le prix unitaire (ligne supplémentaire) OU prix unitaire ajusté (pointage et ajustement)
        /// </summary>        
        public decimal PUHT { get; set; }

        /// <summary>
        /// Obtient ou définit le commentaire d'une ligne
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        ///   Obtient ou définit l'unité 
        /// </summary>        
        public string Unite { get; set; }

        public decimal? DifferenceQuantite
        {
            get { return Quantite - QuantiteConvertie; }
        }

        public decimal? DifferencePU
        {
            get { return PUHT - Bareme; }
        }

        public decimal? MontantValorise
        {
            get { return QuantiteConvertie * Bareme; }
        }

        public decimal? Montant
        {
            get { return Quantite * PUHT; }
        }
        public decimal? DifferenceMontant
        {
            get { return Montant - MontantValorise; }
        }
    }
}
