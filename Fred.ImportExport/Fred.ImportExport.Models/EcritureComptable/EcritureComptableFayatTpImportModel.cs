using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Fred.ImportExport.Models.EcritureComptable
{
    /// <summary>
    /// Model utilisé pour l'import des Ecritures Comptables
    /// Correspond aux écriture comptable venant de SAP
    /// </summary>
    public class EcritureComptableFayatTpImportModel
    {
        [JsonProperty(PropertyName = "DateCreation")]
        public string DateCreationString { get; set; }

        /// <summary>
        /// Date de création de l'écriture comptable
        /// </summary>
        public DateTime? CreationDateFormate { get => DateTime.ParseExact(DateCreationString, "dd.MM.yyyy", CultureInfo.InvariantCulture); set => DateCreationString = value?.ToString("dd.MM.yyyy"); }

        [JsonProperty(PropertyName = "DateComptable")]
        public string DateComptableString { get; set; }

        /// <summary>
        /// Date comptable de l'écriture comptable
        /// </summary>
        public DateTime? DateComptableFormate { get => DateTime.ParseExact(DateComptableString, "dd.MM.yyyy", CultureInfo.InvariantCulture); set => DateComptableString = value?.ToString("dd.MM.yyyy"); }

        /// <summary>
        /// Groupe (Société) de l'écriture comptable
        /// </summary>
        public string Groupe { get; set; }

        /// <summary>
        /// Code de la société de l'écriture comptable
        /// </summary>
        public string SocieteCode { get; set; }

        /// <summary>
        /// Libelle de l'écriture comptable
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Code de la  nature analytique de l'écriture comptable
        /// </summary>
        public string NatureAnalytique { get; set; }

        /// <summary>
        /// Montant de l'écriture comptable avec la devise interne
        /// </summary>
        public decimal? MontantDeviseInterne { get; set; }

        /// <summary>
        /// Code de la devise interne de l'écriture comptable
        /// </summary>
        public string DeviseInterne { get; set; }

        /// <summary>
        /// Montant de l'écriture comptable avec la devise transaction
        /// </summary>
        public decimal? MontantDeviseTransaction { get; set; }

        /// <summary>
        /// Code de la devise transaction de l'écriture comptable
        /// </summary>
        public string DeviseTransaction { get; set; }

        /// <summary>
        /// Code du CI de l'écriture comptable
        /// </summary>
        public string CiCode { get; set; }

        /// <summary>
        /// Numéro de la piéce comptable de l'écriture comptable
        /// </summary>
        public string NumeroPiece { get; set; }

        /// <summary>
        /// Numéro de la commande a laquelle est rattaché l'écriture comptable
        /// </summary>
        public string NumeroCommande { get; set; }

        /// <summary>
        /// Ressource de l'écriture comptable (non non c'est n'est pas une erreur )
        /// </summary>
        public string Ressource { get; set; }

        /// <summary>
        /// Quantité 
        /// </summary>
        public string Quantite { get; set; }

        /// <summary>
        /// Unité Quantité
        /// </summary>
        public string UniteQuantite { get; set; }

        /// <summary>
        /// Identifiant de la ligne de pointage
        /// </summary>
        public string RapportLigneId { get; set; }

        /// <summary>
        /// Identifiant de la personne quie à fait l'écriture comptable
        /// </summary>
        public string Personne { get; set; }

        /// <summary>
        /// Code Materiel de la société
        /// </summary>
        public string MaterielSocieteCode { get; set; }

        /// <summary>
        /// Code du materiel
        /// </summary>
        public string MaterielCode { get; set; }

        /// <summary>
        /// Code Ref
        /// </summary>
        public string CodeRef { get; set; }

        public string NumFactureSAP { get; set; }
    }
}
