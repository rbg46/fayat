using System;
using System.Collections.Generic;
using Fred.Web.Models.CI;
using Fred.Web.Models.Commande;
using Fred.Web.Models.Referential;
using Fred.Web.Models.ReferentielFixe;
using Fred.Web.Models.Utilisateur;
using Fred.Web.Shared.Models;
using Fred.Web.Shared.Models.PieceJointe;

namespace Fred.Web.Models.Depense
{
    /// <summary>
    /// Représente le modèle d'une dépense
    /// </summary>
    public class DepenseAchatModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une dépense.
        /// </summary>
        public int DepenseId { get; set; }

        /// <summary>
        /// Obtient ou définit la ligne de commande dont dépend la dépense.
        /// </summary>
        public CommandeLigneModel CommandeLigne { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'une dépense.
        /// </summary>
        public int? CommandeLigneId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique du CI d'une dépense.
        /// </summary>
        public CIModel CI { get; set; }

        /// <summary>
        /// Obtient ou définit le CI d'une dépense.
        /// </summary>
        public int? CiId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique du fournisseur d'une dépense.
        /// </summary>
        public FournisseurModel Fournisseur { get; set; }

        /// <summary>
        /// Obtient ou définit le fournisseur d'une dépense.
        /// </summary>
        public int? FournisseurId { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'une dépense.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit l'Id de la tâche d'une dépense.
        /// </summary>
        public int? TacheId { get; set; }

        /// <summary>
        /// Obtient ou définit l'objet tache
        /// </summary>
        public TacheModel Tache { get; set; }

        /// <summary>
        /// Obtient ou définit l'Id ressource d'une dépense.
        /// </summary>
        public int? RessourceId { get; set; }

        /// <summary>
        /// Obtient ou définit l'objet Ressource
        /// </summary>
        public RessourceModel Ressource { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de la ligne de commande d'une dépense.
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// Obtient ou définit la quantité d'une dépense.
        /// </summary>
        public decimal Quantite { get; set; }

        /// <summary>
        /// Obtient ou définit le prix unitaire d'une dépense.
        /// </summary>
        public decimal PUHT { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'unité
        /// </summary>
        public int? UniteId { get; set; }

        /// <summary>
        /// Obtient ou définit l'unité d'une ligne de commande.
        /// </summary>
        public UniteModel Unite { get; set; }

        /// <summary>
        /// Obtient ou définit la date de la dépense.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Obtient le montant d'une ligne de dépense.
        /// </summary>
        public decimal MontantHT
        {
            get
            {
                return this.Quantite * this.PUHT;
            }
        }

        /// <summary>
        /// Obtient ou définit l'identifiant de la devise associée à une dépense.
        /// </summary>
        public int? DeviseId { get; set; }

        /// <summary>
        /// Obtient ou définit la devise associée à une dépense.
        /// </summary>
        public DeviseModel Devise { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro du bon de livraison associé à une dépense.
        /// </summary>
        public string NumeroBL { get; set; }

        /// <summary>
        ///  Obtient ou définit la date du rapprochement
        /// </summary>
        public DateTime? DateRapprochement { get; set; }

        /// <summary>
        ///  Obtient ou définit l'identifiant unique de l'auteur du rapprochement
        /// </summary>
        public int? AuteurRapprochementId { get; set; }

        /// <summary>
        /// Obtient ou définit la date de création de la dépense.
        /// </summary>
        public DateTime? DateCreation { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID de la personne ayant créé la dépense.
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité du membre du personnel ayant créé la dépense
        /// </summary>
        public UtilisateurModel AuteurCreation { get; set; } = null;

        /// <summary>
        /// Obtient ou définit la date de modification de la dépense.
        /// </summary>
        public DateTime? DateModification { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID de la personne ayant modifié la dépense.
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité du membre du personnel ayant modifié la dépense
        /// </summary>
        public UtilisateurModel AuteurModification { get; set; } = null;

        /// <summary>
        /// Obtient ou définit la date de suppression de la dépense.
        /// </summary>
        public DateTime? DateSuppression { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID de la personne ayant supprimer la dépense.
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité du membre du personnel ayant supprimer la dépense
        /// </summary>
        public UtilisateurModel AuteurSuppression { get; set; } = null;

        public bool IsModifying { get; set; } = false;

        /// <summary>
        /// Obtient ou définit la liste des erreurs de la dépense détectées dans le manager
        /// </summary>
        public ICollection<string> ListErreurs { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de la ligne de facture
        /// </summary>
        public int? FactureLigneId { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des erreurs de la dépense détectées dans le manager
        /// </summary>
        public string CommandeLigneDataGridColumn
        {
            get
            {
                return this.CommandeLigne != null && this.CommandeLigne.Commande != null ? this.CommandeLigne.Commande.Numero + this.CommandeLigne.Commande.Libelle : "";
            }
        }

        /// <summary>
        /// Obtient ou définit la liste des erreurs de la dépense détectées dans le manager
        /// </summary>
        public string FournisseurDataGridColumn
        {
            get
            {
                return this.Fournisseur != null ? this.Fournisseur.CodeLibelle : "";
            }
        }

        /// <summary>
        /// Obtient ou définit la liste des erreurs de la dépense détectées dans le manager
        /// </summary>
        public string RessourceDataGridColumn
        {
            get
            {
                return this.Ressource != null ? this.Ressource.CodeLibelle : "";
            }
        }

        /// <summary>
        /// Obtient ou définit la liste des erreurs de la dépense détectées dans le manager
        /// </summary>
        public string TacheGridColumnData
        {
            get
            {
                return this.Tache != null ? this.Tache.CodeLibelle : "";
            }
        }

        /// <summary>
        /// Obtient ou définit la liste des erreurs de la dépense détectées dans le manager
        /// </summary>
        public string AuteurCreationDataGridColumn
        {
            get
            {
                return this.AuteurCreation != null ? this.AuteurCreation.NomPrenom : "";
            }
        }

        /// <summary>
        /// Obtient ou définit la liste des erreurs de la dépense détectées dans le manager
        /// </summary>
        public string AuteurModificationDataGridColumn
        {
            get
            {
                return this.AuteurModification != null ? this.AuteurModification.NomPrenom : "";
            }
        }

        /// <summary>
        /// Obtient ou définit la liste des erreurs de la dépense détectées dans le manager
        /// </summary>
        public string DeviseDataGridColumn
        {
            get
            {
                return this.Devise != null ? this.Devise.CodeLibelle : "";
            }
        }

        /// <summary>
        ///   Obtient ou définit l'identifiant du lot
        /// </summary>
        public int? LotFarId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du type de dépense.
        /// </summary>    
        public int? DepenseTypeId { get; set; }

        /// <summary>
        /// Obtient ou définit le type de dépense.
        /// </summary>    
        public DepenseTypeModel DepenseType { get; set; }

        /// <summary>
        /// Obtient ou définit la date comptable.
        /// </summary>    
        public DateTime? DateComptable { get; set; }

        /// <summary>
        /// Obtient ou définit la date visa reception
        /// </summary>    
        public DateTime? DateVisaReception { get; set; }

        /// <summary>
        /// Obtient ou définit la date de facturation.
        /// </summary>    
        public DateTime? DateFacturation { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la personne ayant fait le visa reception.
        /// </summary>    
        public int? AuteurVisaReceptionId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du personnel ayant fait le visa reception
        /// </summary>    
        public UtilisateurModel AuteurVisaReception { get; set; } = null;

        /// <summary>
        ///  Obtient ou définit la quantité d'une dépense.
        /// </summary>    
        public decimal QuantiteDepense { get; set; }

        /// <summary>
        ///   Obtient ou définit la somme des montants facturés de toutes les facturations de type "Reception"    
        /// </summary>    
        public decimal MontantFacturationReception { get; set; }

        /// <summary>
        ///   Obtient ou définit la somme des quantités facturés de toutes les facturations de type "Reception"    
        /// </summary>
        public decimal QuantiteFacturationReception { get; set; }

        /// <summary>
        ///   Obtient ou définit la somme des montants facturés de toutes les facturations de type "FactureEcart"
        /// </summary>
        public decimal MontantFacturationFactureEcart { get; set; }

        /// <summary>
        ///   Obtient ou définit la somme des quantités facturés de toutes les facturations de type "FactureEcart"    
        /// </summary>
        public decimal QuantiteFacturationFactureEcart { get; set; }

        /// <summary>
        ///   Obtient ou définit la somme des montants facturés de toutes les facturations de type "Facture"    
        /// </summary>
        public decimal MontantFacturationFacture { get; set; }

        /// <summary>
        ///   Obtient ou définit la somme des quantités facturés de toutes les facturations de type "Facture"    
        /// </summary>
        public decimal QuantiteFacturationFacture { get; set; }

        /// <summary>
        ///   Obtient ou définit la somme des montants facturés de toutes les facturations 
        ///   (tous types confondus : « facturation », « coût additionnel » et « annulation ») rattachées à la dépense de type Réception
        /// </summary>    
        public decimal MontantFacturation { get; set; }

        /// <summary>
        ///   Obtient ou définit la somme des quantités facturés de toutes les facturations de type "Facturation"
        ///   (tous types confondus : « facturation », « coût additionnel » et « annulation ») rattachées à la dépense de type Réception
        /// </summary>    
        public decimal QuantiteFacturation { get; set; }

        /// <summary>
        ///   Obtient ou définit le Solde FAR
        /// </summary>    
        public decimal SoldeFar { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant facturé
        ///   PUHT Dépense Achat * (Quantité Réceptionnée Dépense Achat - Quantité Dépense Dépense Achat) + Somme des Montants HT des lignes de facturation de type 2
        ///   (Coût additionnels) rattachés à la réception (à Récupérer via le champ "Dépense Achat Réception" de la table Faturation)
        /// </summary>    
        public decimal MontantFacture { get; set; }

        /// <summary>
        ///   Obtient ou définit la Nature de la Ressource
        /// </summary>
        public NatureModel Nature { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant Facture Hors Ecart
        /// </summary>    
        public decimal MontantFactureHorsEcart { get; set; }

        /// <summary>
        ///   Obtient ou définit un booléen déterminant si la Far est annulée ou pas
        /// </summary>    
        public bool FarAnnulee { get; set; }

        /// <summary>
        ///   Obtient ou définit l'idenfiant du job hangfire.
        /// </summary>
        public string HangfireJobId { get; set; }

        /// <summary>
        ///   Obtient ou définit s'il y a eu au moins une opération d'ajustement FAR (Annulation Far, Chargement, Déchargement)
        /// </summary>
        public bool HasAjustementFar { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de transfert Far du CI associé à la commande
        /// </summary>    
        public DateTime? DateTransfertFar { get; set; }

        /// <summary>
        /// Obtient ou définit une collection de facturation pour les réceptions.
        /// </summary>
        public ICollection<FacturationModel> FacturationsReception { get; set; }

        /// <summary>
        /// Pièces jointes attachées à la réception
        /// </summary>
        public List<PieceJointeReceptionModel> PiecesJointesReception { get; set; }

        /// <summary>
        /// Obtient ou définit une collection de facturation pour les écarts de facture.
        /// </summary>
        public ICollection<FacturationModel> FacturationsFactureEcart { get; set; }

        /// <summary>
        /// Obtient ou définit une collection de facturation pour les écarts de facture.
        /// </summary>
        public ICollection<FacturationModel> FacturationsFacture { get; set; }

        /// <summary>
        /// Obtient ou définit une collection de facturation pour les FAR.
        /// </summary>
        public ICollection<FacturationModel> FacturationsFar { get; set; }

        /// <summary>
        /// Obtient ou définit si la dépense est une réception intérimaire ou non 
        /// </summary>      
        public bool IsReceptionInterimaire { get; set; }
    }
}
