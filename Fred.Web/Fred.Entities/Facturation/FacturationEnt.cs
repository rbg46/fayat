using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Entities.Referential;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.Facturation
{
    /// <summary>
    /// Représente une facturation
    /// </summary>
    public class FacturationEnt
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
        public FacturationTypeEnt FacturationType { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant d'une commande.
        /// </summary>
        public int? CommandeId { get; set; }

        /// <summary>
        /// Obtient ou définit la commande.
        /// </summary>
        public CommandeEnt Commande { get; set; }

        /// <summary>
        /// Obtient ou définit l'idenfiant d'une dépense-achat réception.
        /// </summary>
        public int? DepenseAchatReceptionId { get; set; }

        /// <summary>
        /// Obtient ou définit la dépense-achat réception.
        /// </summary>
        public DepenseAchatEnt DepenseAchatReception { get; set; }

        /// <summary>
        /// Obtient ou définit l'idenfiant d'une dépense-achat ajustement.
        /// </summary>
        public int? DepenseAchatFactureEcartId { get; set; }

        /// <summary>
        /// Obtient ou définit la dépense-achat ajustement.
        /// </summary>
        public DepenseAchatEnt DepenseAchatFactureEcart { get; set; }

        /// <summary>
        /// Obtient ou définit l'idenfiant d'une dépense-achat facture.
        /// </summary>
        public int? DepenseAchatFactureId { get; set; }

        /// <summary>
        /// Obtient ou définit la dépense-achat facture.
        /// </summary>
        public DepenseAchatEnt DepenseAchatFacture { get; set; }

        /// <summary>
        /// Obtient ou définit l'idenfiant d'une dépense-achat extourne far ou ajustement far sap
        /// </summary>
        public int? DepenseAchatFarId { get; set; }

        /// <summary>
        /// Obtient ou définit la dépense-achat extourne far ou ajustement far sap
        /// </summary>
        public DepenseAchatEnt DepenseAchatFar { get; set; }

        /// <summary>
        /// Obtient ou définit l'idenfiant d'une devise.
        /// </summary>
        public int? DeviseId { get; set; }

        /// <summary>
        /// Obtient ou définit la devise.
        /// </summary>
        public DeviseEnt Devise { get; set; }

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
        public decimal? EcartPu { get; set; }

        /// <summary>
        /// Obtient ou définit l'écart en quantité.
        /// </summary>
        public decimal? EcartQuantite { get; set; }

        /// <summary>
        /// Obtient ou définit s'il s'agit de la dernière facturation.
        /// </summary>
        public bool IsFacturationFinale { get; set; }

        /// <summary>
        /// Obtient ou définit la quantité reconduite.
        /// </summary>
        public decimal? QuantiteReconduite { get; set; }

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
        /// Obtient ou définit le montant total de la facture hors taxe.
        /// </summary>
        public decimal MontantTotalHT { get; set; }

        /// <summary>
        /// Obtient ou définit le commentaire.
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// Obtient ou définit le code de la nature analytique ANAEL.
        /// </summary>
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
            get { return DateTime.SpecifyKind(dateCreation, DateTimeKind.Utc); }
            set { dateCreation = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
        }

        /// <summary>
        /// Obtient ou définit le nom de l'auteur de la création.
        /// </summary>
        public string AuteurCreation { get; set; }

        /// <summary>
        /// Obtient ou définit le montant total des FAR sur toute la commande donnée
        /// </summary>
        public decimal? TotalFarHt { get; set; }

        /// <summary>
        /// Obtient ou définit la quantité FAR
        /// </summary>
        public decimal? MouvementFarHt { get; set; }

        /// <summary>
        /// Obtient ou définit la quantité restante à facturer
        /// </summary>    
        public decimal? QuantiteFar { get; set; }

        /// <summary>
        /// Obtient ou définit le code litige
        /// </summary>
        public string LitigeCode { get; set; }

        /// <summary>
        /// Obtient ou définit le mouvement comptable (H: crédit, S: débit)
        /// </summary>
        public string DebitCredit { get; set; }

        /// <summary>
        /// Obtient ou définit le compte comptable
        /// </summary>
        public string CompteComptable { get; set; }

        /// <summary>
        ///     Obtient ou définit l'identifiant du CI
        /// </summary>
        public int? CiId { get; set; }

        /// <summary>
        ///     Obtient ou définit le CI
        /// </summary>
        public CIEnt CI { get; set; }

        /// <summary>
        /// Obtient ou définit le Code CI ANAEL
        /// </summary>
        public string CodeCi { get; set; }

        /// <summary>
        /// Obtient ou définit le code compta de la société (/!\ Code société comptabilité, ex: 1000 pour Razel-Bec et non "RZB")
        /// </summary>
        public string SocieteCode { get; set; }

        /// <summary>
        ///   Obtient ou définit le Prix Unitaire Facturé
        /// </summary>
        public decimal PuFacture { get; set; }

        /// <summary>
        /// Obtient ou définit l'idenfiant d'une dépense-achat ajustement.
        /// </summary>
        public int? DepenseAchatAjustementId { get; set; }

        /// <summary>
        /// Obtient ou définit la dépense-achat ajustement.
        /// </summary>
        public DepenseAchatEnt DepenseAchatAjustement { get; set; }
    }
}
