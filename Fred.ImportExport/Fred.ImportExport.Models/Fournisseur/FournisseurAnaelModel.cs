using System;

namespace Fred.ImportExport.Models
{
    /// <summary>
    ///   Model représentant un Fournisseur issu d'ANAEL
    /// </summary>
    public class FournisseurAnaelModel
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
        ///   Obtient ou définit l'ISO code TVA intracommunautaire fournisseur
        /// </summary>
        public string IsoTVA { get; set; }

        /// <summary>
        ///   Obtient ou définit le Numéro TVA intracommunautaire fournisseur
        /// </summary>
        public string NumeroTVA { get; set; }

        /// <summary>
        ///   Obtient ou définit le Critère de Recherche fournisseur
        /// </summary>
        public string CritereRecherche { get; set; }

        /// <summary>
        ///   Obtient ou définit le Compte de Contrepartie 1 fournisseur
        /// </summary>
        public string CC1 { get; set; }

        /// <summary>
        ///   Obtient ou définit le Compte de Contrepartie 2 fournisseur
        /// </summary>
        public string CC2 { get; set; }

        /// <summary>
        ///   Obtient ou définit le Compte de Contrepartie 3 fournisseur
        /// </summary>
        public string CC3 { get; set; }

        /// <summary>
        ///   Obtient ou définit le Compte de Contrepartie 4 fournisseur
        /// </summary>
        public string CC4 { get; set; }

        /// <summary>
        ///   Obtient ou définit le Compte de Contrepartie 5 fournisseur
        /// </summary>
        public string CC5 { get; set; }

        /// <summary>
        ///   Obtient ou définit le Compte de Contrepartie 6 fournisseur
        /// </summary>
        public string CC6 { get; set; }

        /// <summary>
        ///   Obtient ou définit le Compte de Contrepartie 7 fournisseur
        /// </summary>
        public string CC7 { get; set; }

        /// <summary>
        ///   Obtient ou définit le Compte de Contrepartie 8 fournisseur
        /// </summary>
        public string CC8 { get; set; }

        /// <summary>
        ///   Obtient ou définit le code de la société.
        /// </summary>
        public string CodeSociete { get; set; }

        /// <summary>
        /// Obtient ou définit si le fournisseur est une profession libérale
        /// </summary>
        public string IsProfessionLiberale { get; set; }
    }
}
