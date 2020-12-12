using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Facturation;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Utilisateur;
using System;
using System.Collections.Generic;

namespace Fred.Entities.Depense
{
    /// <summary>
    ///   Représente une Dépense Achat
    ///   Les dépenses achat peuvent être de 3 types :
    ///     - Réception
    ///     - Facture
    ///     - Facture Ecart
    /// </summary>
    public class DepenseAchatEnt
    {
        private DateTime? dateModification;
        private DateTime? dateCreation;
        private DateTime? dateSuppression;
        private DateTime? dateComptable;
        private DateTime? dateVisaReception;
        private DateTime? dateFacturation;
        private DateTime? dateOperation;
        private DateTime? dateControleFar;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une dépense achat.
        /// </summary>
        public int DepenseId { get; set; }

        /// <summary>
        ///   Obtient ou définit la ligne de commande dont dépend la dépense.
        /// </summary>
        public CommandeLigneEnt CommandeLigne { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la ligne de commande d'une dépense.
        /// </summary>
        public int? CommandeLigneId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'affaire d'une dépense.
        /// </summary>
        public CIEnt CI { get; set; }

        /// <summary>
        ///   Obtient ou définit l'affaire d'une dépense.
        /// </summary>
        public int? CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique du fournisseur d'une dépense.
        /// </summary>
        public FournisseurEnt Fournisseur { get; set; }

        /// <summary>
        ///   Obtient ou définit le fournisseur d'une dépense.
        /// </summary>
        public int? FournisseurId { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'une dépense.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Id de la tâche d'une dépense.
        /// </summary>
        public int? TacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'objet tache
        /// </summary>
        public TacheEnt Tache { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Id de la tâche d'une dépense.
        /// </summary>
        public int? RessourceId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'objet ressource
        /// </summary>
        public RessourceEnt Ressource { get; set; }

        /// <summary>
        ///   Obtient ou définit le commentaire.
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        ///   Obtient ou définit la quantité receptionnée.
        /// </summary>
        public decimal Quantite { get; set; }

        /// <summary>
        ///   Obtient ou définit le prix unitaire d'une dépense.
        /// </summary>
        public decimal PUHT { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'unité
        /// </summary>
        public int? UniteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'unité
        /// </summary>
        public UniteEnt Unite { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de la dépense.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de création de la dépense.
        /// </summary>
        public DateTime? DateCreation
        {
            get { return (dateCreation.HasValue) ? DateTime.SpecifyKind(dateCreation.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateCreation = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        ///   Obtient ou définit l'ID de la personne ayant créé la dépense.
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du personnel ayant créé la dépense
        /// </summary>
        public UtilisateurEnt AuteurCreation { get; set; } = null;

        /// <summary>
        ///   Obtient ou définit la date de modification de la dépense.
        /// </summary>
        public DateTime? DateModification
        {
            get { return (dateModification.HasValue) ? DateTime.SpecifyKind(dateModification.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateModification = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        ///   Obtient ou définit l'ID de la personne ayant modifié la dépense.
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du personnel ayant modifié la dépense
        /// </summary>
        public UtilisateurEnt AuteurModification { get; set; } = null;

        /// <summary>
        ///   Obtient ou définit la date de suppression de la dépense.
        /// </summary>
        public DateTime? DateSuppression
        {
            get { return (dateSuppression.HasValue) ? DateTime.SpecifyKind(dateSuppression.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateSuppression = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        ///   Obtient ou définit l'ID de la personne ayant supprimer la dépense.
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du personnel ayant supprimer la dépense
        /// </summary>
        public UtilisateurEnt AuteurSuppression { get; set; } = null;

        /// <summary>
        ///   Obtient ou définit l'identifiant de la devise associée à une dépense.
        /// </summary>
        public int? DeviseId { get; set; }

        /// <summary>
        ///   Obtient ou définit la devise associée à une dépense.
        /// </summary>
        public DeviseEnt Devise { get; set; }

        /// <summary>
        ///   Obtient ou définit le numéro du bon de livraison associé à une dépense.
        /// </summary>
        public string NumeroBL { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la dépense parent
        /// </summary>
        public int? DepenseParentId { get; set; }

        /// <summary>
        ///   Obtient ou définit la dépense parent
        /// </summary>
        public DepenseAchatEnt DepenseParent { get; set; }

        /// <summary>
        ///   Obtient ou définit un booléen déterminant si la Far est annulée ou pas
        /// </summary>
        public bool FarAnnulee { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des erreurs de la dépense détectées dans le manager
        /// </summary>
        public ICollection<string> ListErreurs { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du type de dépense.
        /// </summary>
        public int? DepenseTypeId { get; set; }

        /// <summary>
        /// Obtient ou définit le type de dépense.
        /// </summary>
        public DepenseTypeEnt DepenseType { get; set; }

        /// <summary>
        /// Obtient ou définit la date comptable.
        /// </summary>
        public DateTime? DateComptable
        {
            get { return (dateComptable.HasValue) ? DateTime.SpecifyKind(dateComptable.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateComptable = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        /// Obtient ou définit la date visa reception
        /// </summary>
        public DateTime? DateVisaReception
        {
            get { return (dateVisaReception.HasValue) ? DateTime.SpecifyKind(dateVisaReception.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateVisaReception = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        /// Obtient ou définit la date de facturation.
        /// </summary>
        public DateTime? DateFacturation
        {
            get { return (dateFacturation.HasValue) ? DateTime.SpecifyKind(dateFacturation.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateFacturation = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la personne ayant fait le visa reception.
        /// </summary>
        public int? AuteurVisaReceptionId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du personnel ayant fait le visa reception
        /// </summary>
        public UtilisateurEnt AuteurVisaReception { get; set; } = null;

        /// <summary>
        ///  Obtient ou définit la quantité d'une dépense.
        /// </summary>
        public decimal QuantiteDepense { get; set; }

        /// <summary>
        /// Obtient ou définit l'idenfiant du job hangfire.
        /// </summary>
        public string HangfireJobId { get; set; }

        /// <summary>
        /// Obtient ou définit s'il faut afficher le prix unitaire
        /// </summary>
        public bool AfficherPuHt { get; set; }

        /// <summary>
        /// Obtient ou définit s'il faut afficher la quantité
        /// </summary>
        public bool AfficherQuantite { get; set; }

        /// <summary>
        /// Obtient ou définit le compte comptable
        /// </summary>
        public string CompteComptable { get; set; }

        /// <summary>
        /// Obtient ou définit s'il y a une erreur de contrôle far
        /// </summary>
        public bool? ErreurControleFar { get; set; }

        /// <summary>
        /// Obtient ou définit le date de dernier contrôle far
        /// </summary>
        public DateTime? DateControleFar
        {
            get { return (dateControleFar.HasValue) ? DateTime.SpecifyKind(dateControleFar.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateControleFar = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        /// Obtient ou définit l'identifiant du statut visa des réceptions :
        /// « vide » (par défaut pour toutes les lignes créées dans la table)
        /// « 1 – OK »
        /// « 2 – Erreur »
        /// </summary>
        public int? StatutVisaId { get; set; }

        /// <summary>
        /// Obtient ou définit la date de facturation.
        /// </summary>
        public DateTime? DateOperation
        {
            get { return (dateOperation.HasValue) ? DateTime.SpecifyKind(dateOperation.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateOperation = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        ///     Obtient ou définit le montant hors taxe initial
        /// </summary>
        public decimal? MontantHtInitial { get; set; }

        /// <summary>
        /// Pièces jointes attachées à la réception
        /// </summary>
        public ICollection<PieceJointeReceptionEnt> PiecesJointesReception { get; set; }

        /// <summary>
        /// Obtient ou définit si la dépense est une réception intérimaire ou non 
        /// </summary>
        public bool IsReceptionInterimaire { get; set; }

        /// <summary>
        /// Obtient ou définit si la dépense est une réception matériel externe ou non 
        /// </summary>
        public bool IsReceptionMaterielExterne { get; set; }

        #region Champs calculés

        /// <summary>
        ///   Obtient ou définit la dépense si oui ou non elle peut être rapprochée par l'utilisateur courant
        /// </summary>
        public bool RapprochableParUserCourant { get; set; }

        /// <summary>
        ///   Obtient le montant HT de la dépense
        /// </summary>
        public decimal MontantHT { get; set; }

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
        ///   ∑ Quantité x PU HT de toutes les Dépenses Achat de type ‘Facture’ + ’Facture Ecart’ + ‘Avoir’ + ‘Avoir Ecart’ associées (via Dépense Parent ID) à cette réception dont la Date Opération est antérieure ou égale à J        
        /// </summary>
        public decimal MontantFacture { get; set; }

        /// <summary>
        ///   Obtient ou définit la Nature Analytique de la Ressource
        /// </summary>
        public NatureEnt Nature { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant Facture Hors Ecart
        /// </summary>
        public decimal MontantFactureHorsEcart { get; set; }

        /// <summary>
        ///   Obtient ou définit s'il y a eu au moins une opération d'ajustement FAR (Annulation Far, Chargement, Déchargement)
        /// </summary>
        public bool HasAjustementFar { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de transfert Far du CI associé à la commande
        /// </summary>
        public DateTime? DateTransfertFar { get; set; }

        #endregion

        /// <summary>
        /// Obtient ou définit une collection de facturation pour les réceptions.
        /// </summary>
        public virtual ICollection<FacturationEnt> FacturationsReception { get; set; }

        /// <summary>
        /// Obtient ou définit une collection de facturation pour les écarts de facture.
        /// </summary>
        public virtual ICollection<FacturationEnt> FacturationsFactureEcart { get; set; }

        /// <summary>
        /// Obtient ou définit une collection de facturation pour les écarts de facture.
        /// </summary>
        public virtual ICollection<FacturationEnt> FacturationsFacture { get; set; }

        /// <summary>
        /// Obtient ou définit une collection de facturation pour les ajustement far ou extourne far
        /// </summary>
        public virtual ICollection<FacturationEnt> FacturationsFar { get; set; }

        /// <summary>
        /// Child Depenses where [FRED_DEPENSE].[DepenseParentId] point to this entity (FK_DEPENSE_PARENT)
        /// </summary>
        public virtual ICollection<DepenseAchatEnt> Depenses { get; set; }

        /// <summary>
        /// Child DepenseTemporaires where [FRED_DEPENSE_TEMPORAIRE].[DepenseOrigineId] point to this entity (FK_DEPENSE_TEMPORAIRE_ORIGINE)
        /// </summary>
        public virtual ICollection<DepenseTemporaireEnt> DepenseOrigine { get; set; }

        /// <summary>
        /// Child DepenseTemporaires where [FRED_DEPENSE_TEMPORAIRE].[DepenseParentId] point to this entity (FK_DEPENSE_TEMPORAIRE_PARENT)
        /// </summary>
        public virtual ICollection<DepenseTemporaireEnt> DepenseTemporaireEnts { get; set; }

        /// <summary>
        ///   Obtient ou définit le groupe de tâches de remplacement.
        /// </summary>
        public GroupeRemplacementTacheEnt GroupeRemplacementTache { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id du groupe de tâches de remplacement.
        /// </summary>
        public int? GroupeRemplacementTacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des tâches remplacées    
        /// </summary>
        public IEnumerable<RemplacementTacheEnt> RemplacementTaches { get; set; }
    }
}
