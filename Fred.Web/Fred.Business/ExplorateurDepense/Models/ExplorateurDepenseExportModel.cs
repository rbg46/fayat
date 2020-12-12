namespace Fred.Business.ExplorateurDepense
{
    /// <summary>
    ///   Model Explorateur depense export
    /// </summary>
    public class ExplorateurDepenseExportModel
    {
        /// <summary>
        ///   Obtient ou définit le Code CI avec Code CI + Libelle
        /// </summary>
        public string CodeCiLibelle { get; set; }

        /// <summary>
        ///   Obtient ou définit la ressource
        /// </summary>
        public string CodeLibelleChapitre { get; set; }

        /// <summary>
        ///   Obtient ou définit la ressource
        /// </summary>
        public string CodeLibelleSousChapitre { get; set; }

        /// <summary>
        ///   Obtient ou définit la ressource
        /// </summary>
        public string CodeLibelleRessource { get; set; }

        /// <summary>
        ///   Obtient ou définit la tache
        /// </summary>
        public string CodeLibelleTacheT1 { get; set; }

        /// <summary>
        ///   Obtient ou définit la tache
        /// </summary>
        public string CodeLibelleTacheT2 { get; set; }

        /// <summary>
        ///   Obtient ou définit la tache
        /// </summary>
        public string CodeLibelleTacheT3 { get; set; }

        /// <summary>
        ///   Obtient ou définit le libelle1
        /// </summary>
        public string Libelle1 { get; set; }

        /// <summary>
        ///   Obtient ou définit l'unité
        /// </summary>
        public string CodeUnite { get; set; }

        /// <summary>
        ///   Obtient ou définit la quantité
        /// </summary>
        public string Quantite { get; set; }

        /// <summary>
        ///   Obtient ou définit le prix unitaire hors taxe
        /// </summary>
        public string PUHT { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant hors taxe
        /// </summary>
        public string MontantHT { get; set; }

        /// <summary>
        ///   Obtient ou définit la devise
        /// </summary>
        public string SymboleDevise { get; set; }

        /// <summary>
        ///   Obtient ou définit le code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé 2
        /// </summary>
        public string Libelle2 { get; set; }

        /// <summary>
        ///   Obtient ou définit le commentaire
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        ///   Obtient ou définit la date facture
        /// </summary>
        public string DateFacture { get; internal set; }

        /// <summary>
        ///   Obtient ou définit la date de la dépense
        /// </summary>
        public string DateDepense { get; set; }

        /// <summary>
        ///   Obtient ou définit la période
        /// </summary>
        public string Periode { get; set; }

        /// <summary>
        ///   Obtient ou définit la nature
        /// </summary>
        public string CodeLibelleNature { get; set; }

        /// <summary>
        ///   Obtient ou définit le type de dépense ["OD", "Valorisation", "Reception", "Facture"]
        /// </summary>
        public string TypeDepense { get; set; }

        /// <summary>
        ///   Obtient ou définit le type de sous dépense
        /// </summary>
        public string SousTypeDepense { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de rapprochement facture
        /// </summary>
        public string DateRapprochement { get; set; }

        /// <summary>
        ///   Obtient ou définit le numéro de facture
        /// </summary>
        public string NumeroFacture { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant de la facture
        /// </summary>
        public string MontantFacture { get; set; }
    }
}
