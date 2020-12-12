using Fred.Web.Models.Referential;
using System;

namespace Fred.Web.Shared.Models
{
    /// <summary>
    /// Représente une facturation
    /// </summary>
    public class FacturationModel
    {
        private DateTime dateCreation;

        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une facturation.
        /// </summary>
        public int FacturationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du type de facturation.
        /// </summary>
        public int FacturationTypeId { get; set; }

        /// <summary>
        /// Obtient ou définit le type de facturation.
        /// </summary>
        public FacturationTypeModel FacturationType { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant d'une commande.
        /// </summary>
        public int CommandeId { get; set; }

        /// <summary>
        /// Obtient ou définit l'idenfiant d'une dépense-achat réception.
        /// </summary>
        public int DepenseAchatReceptionId { get; set; }

        /// <summary>
        /// Obtient ou définit l'idenfiant d'une dépense-achat ajustement.
        /// </summary>
        public int? DepenseAchatFactureEcartId { get; set; }

        /// <summary>
        /// Obtient ou définit l'idenfiant d'une dépense-achat facture.
        /// </summary>
        public int? DepenseAchatFactureId { get; set; }

        /// <summary>
        /// Obtient ou définit l'idenfiant d'une dépense-achat far.
        /// </summary>
        public int? DepenseAchatFarId { get; set; }

        /// <summary>
        /// Obtient ou définit l'idenfiant d'une devise.
        /// </summary>
        public int? DeviseId { get; set; }

        /// <summary>
        /// Obtient ou définit la devise.
        /// </summary>
        public DeviseModel Devise { get; set; }

        /// <summary>
        /// Obtient ou définit le montant hors taxe.
        /// </summary>
        public decimal MontantHT { get; set; }

        /// <summary>
        /// Obtient ou définit la quantité.
        /// </summary>
        public decimal Quantite { get; set; }

        /// <summary>
        /// Obtient ou définit l'écart sur le prix unitaire.
        /// </summary>
        public decimal EcartPu { get; set; }

        /// <summary>
        /// Obtient ou définit l'écart en quantité.
        /// </summary>
        public decimal EcartQuantite { get; set; }

        /// <summary>
        /// Obtient ou définit s'il s'agit de la dernière facturation.
        /// </summary>
        public bool IsFacturationFinale { get; set; }

        /// <summary>
        /// Obtient ou définit la quantité reconduite.
        /// </summary>
        public decimal QuantiteReconduite { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro de facture FMFI.
        /// </summary>
        public string NumeroFactureFMFI { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro de facture SAP.
        /// </summary>
        public string NumeroFactureSAP { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro du fournisseur.
        /// </summary>
        public string NumeroFactureFournisseur { get; set; }

        /// <summary>
        /// Obtient ou définit la date de pièces Sap.
        /// </summary>
        public DateTime DatePieceSap { get; set; }

        /// <summary>
        /// Obtient ou définit la date saisie de la facture.
        /// </summary>
        public DateTime DateSaisie { get; set; }

        /// <summary>
        /// Obtient ou définit la date comptable.
        /// </summary>
        public DateTime DateComptable { get; set; }

        /// <summary>
        /// Obtient ou définit le montant total de la facture.
        /// </summary>
        public decimal MontantTotalHT { get; set; }

        /// <summary>
        /// Obtient ou définit le commentaire.
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// Obtient ou définit le code de la nature analytique ANAEL.
        public string NatureCode { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'un fournisseur.
        /// </summary>
        public string FournisseurCode { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de création de la dépense.
        /// </summary>
        public DateTime DateCreation
        {
            get
            {
                return DateTime.SpecifyKind(dateCreation, DateTimeKind.Utc);
            }
            set
            {
                dateCreation = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
        }

        /// <summary>
        /// Obtient ou définit le nom de l'auteur de la création.
        /// </summary>
        public string AuteurCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit le Prix Unitaire Facturé
        /// </summary>    
        public decimal PuFacture { get; set; }
    }
}
