namespace Fred.ImportExport.Models
{
    /// <summary>
    /// Model représentant un import agence.
    /// </summary>
    public class ImportAgenceModel
    {
        /// <summary>
        ///   Obtient ou définit le Code fournisseur
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le Libellé fournisseur
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Adresse fournisseur
        /// </summary>
        public string Adresse { get; set; }

        /// <summary>
        ///   Obtient ou définit le Code Postal fournisseur
        /// </summary>
        public string CodePostal { get; set; }

        /// <summary>
        ///   Obtient ou définit la Ville fournisseur
        /// </summary>
        public string Ville { get; set; }

        /// <summary>
        ///   Obtient ou définit le Téléphone fournisseur
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        ///   Obtient ou définit le Fax fournisseur
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Email fournisseur
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///   Obtient ou définit le SIRET fournisseur
        /// </summary>
        public string SIRET { get; set; }

        /// <summary>
        ///   Obtient ou définit le Siren fournisseur
        /// </summary>
        public string Siren { get; set; }

        /// <summary>
        ///   Obtient ou définit le Code Pays fournisseur
        /// </summary>
        public string CodePays { get; set; }

        public int? PaysId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code de la société.
        /// </summary>
        public string SocieteCode { get; set; }
    }
}
