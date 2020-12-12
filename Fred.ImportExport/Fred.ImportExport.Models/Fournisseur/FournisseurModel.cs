using System;

namespace Fred.ImportExport.Models
{
    /// <summary>
    /// Model représentant un fournisseur.
    /// </summary>
    public class FournisseurModel
    {
        /// <summary>
        ///   Obtient ou définit le Code fournisseur
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le Type Séquence fournisseur
        /// </summary>
        public string TypeSequence { get; set; }

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
        ///   Obtient ou définit le SIREN fournisseur
        /// </summary>
        public string SIREN { get; set; }

        /// <summary>
        ///   Obtient ou définit le Type Tiers fournisseur
        /// </summary>
        public string TypeTiers { get; set; }

        /// <summary>
        ///   Obtient ou définit le Mode Règlement fournisseur
        /// </summary>
        public string ModeReglement { get; set; }

        /// <summary>
        ///   Obtient ou définit la Règle de Gestion fournisseur
        /// </summary>
        public string RegleGestion { get; set; }

        /// <summary>
        ///   Obtient ou définit le Code Pays fournisseur
        /// </summary>
        public string CodePays { get; set; }

        /// <summary>
        ///   Obtient ou définit la Date Ouverture fournisseur
        /// </summary>
        public DateTime? DateOuverture { get; set; }

        /// <summary>
        ///   Obtient ou définit la Date Fermeture fournisseur
        /// </summary>
        public DateTime? DateCloture { get; set; }

        /// <summary>
        ///   Obtient ou définit le code de la société.
        /// </summary>
        public string SocieteCode { get; set; }

        public bool IsProfessionLiberale { get; set; }
    }
}
