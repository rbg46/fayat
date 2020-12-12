using System;
using System.Collections.Generic;
using Fred.Entities.CI;
using Fred.Entities.Depense;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;

namespace Fred.Web.Shared.Models.Depense
{
    /// <summary>
    /// Représente la liste des dépenses exposées vers l'exterieur. Attention ce model NE DOIT PAS être utiliser tel quel.
    /// Il est OBLIGATOIRE de refaire un model si vous voulez l'utiliser. 
    /// </summary>
    public class DepenseExhibition
    {
        /// <summary>
        /// Obtient ou définit le CI Code CI + Libelle
        /// </summary>
        public string InfoCI => (Ci != null) ? Ci.Code + " - " + Ci.Libelle : string.Empty;

        /// <summary>
        /// Obtient ou définit le CI Code CI + Libelle
        /// </summary>
        /// 
        public CIEnt Ci { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de la dépense
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la ressource
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        /// Obtient ou définit la ressource
        /// </summary>
        public RessourceEnt Ressource { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la tâche
        /// </summary>
        public int TacheId { get; set; }

        /// <summary>
        /// Obtient ou définit la tache
        /// </summary>
        public TacheEnt Tache { get; set; }

        /// <summary>
        /// Obtient ou définit le libelle1
        /// </summary>
        public string Libelle1 { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'unité
        /// </summary>
        public int UniteId { get; set; }

        /// <summary>
        /// Obtient ou définit l'unité
        /// </summary>
        public UniteEnt Unite { get; set; }

        /// <summary>
        /// Obtient ou définit la quantité
        /// </summary>
        public decimal? Quantite { get; set; }

        /// <summary>
        /// Obtient ou définit le prix unitaire hors taxe
        /// </summary>
        public decimal? PUHT { get; set; }

        /// <summary>
        /// Obtient ou définit le montant hors taxe
        /// </summary>
        public decimal MontantHT { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la devise
        /// </summary>
        public int DeviseId { get; set; }

        /// <summary>
        /// Obtient ou définit la devise
        /// </summary>
        public DeviseEnt Devise { get; set; }

        /// <summary>
        /// Obtient ou définit le code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé 2
        /// </summary>
        public string Libelle2 { get; set; }

        /// <summary>
        /// Obtient ou définit le commentaire
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// Obtient ou définit la date comptable de remplacement
        /// </summary>
        public DateTime DateComptableRemplacement { get; set; }

        /// <summary>
        /// Obtient ou définit la date de la dépense
        /// </summary>
        public DateTime DateDepense { get; set; }

        /// <summary>
        /// Obtient ou définit la période
        /// </summary>
        public DateTime Periode { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la nature
        /// </summary>
        public int NatureId { get; set; }

        /// <summary>
        /// Obtient ou définit la nature
        /// </summary>
        public NatureEnt Nature { get; set; }

        /// <summary>
        /// Obtient ou définit le type de dépense ["OD", "Valorisation", "Reception", "Facture"]
        /// </summary>
        public string TypeDepense { get; set; }

        /// <summary>
        /// Obtient ou définit le type de sous dépense
        /// </summary>
        public string SousTypeDepense { get; set; }

        /// <summary>
        /// Obtient ou définit la date rapprochement
        /// </summary>
        public DateTime? DateRapprochement { get; set; }

        /// <summary>
        /// Obtient ou définit la date facture (ou date opération pour une Réception)
        /// </summary>
        public DateTime? DateFacture { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro de facture
        /// </summary>
        public string NumeroFacture { get; set; }

        /// <summary>
        /// Obtient ou définit le montant de la facture
        /// </summary>
        public decimal? MontantFacture { get; set; }

        /// <summary>
        /// Obtient ou définit le montant hors taxe initial
        /// </summary>
        public decimal? MontantHtInitial { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la commande
        /// </summary>
        public int? CommandeId { get; set; }

        /// <summary>
        /// Indique si la commande est une commande Energie
        /// </summary>
        public bool IsEnergie { get; set; }

        /// <summary>
        /// Obtient ou définit si la tâche est remplaçable
        /// </summary>
        public bool TacheRemplacable { get; set; }

        /// <summary>
        /// Obtient ou définit si la dépense a été visée
        /// </summary>
        public bool DepenseVisee { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la dépense
        /// </summary>
        public int DepenseId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du groupe de remplacement
        /// </summary>
        public int GroupeRemplacementTacheId { get; set; }

        /// <summary>
        /// Obtient ou définit le code et libellé de la Tâche d'origine si elle a été remplacée dans la dépense
        /// </summary>
        public string TacheOrigineCodeLibelle { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la Tâche d'origine si elle a été remplacée dans la dépense
        /// </summary>
        public int? TacheOrigineId { get; set; }

        /// <summary>
        /// Obtient ou définit la Tâche d'origine si elle a été remplacée dans la dépense
        /// </summary>
        public TacheEnt TacheOrigine { get; set; }

        /// <summary>
        /// Obtient ou définit le personnel
        /// </summary>
        public PersonnelEnt Personnel { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du fournisseur
        /// </summary>
        public int? FournisseurId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'agence
        /// </summary>
        public int? AgenceId { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des tâches remplacées sur la dépense
        /// </summary>
        public IEnumerable<RemplacementTacheEnt> RemplacementTaches { get; set; }

        /// <summary>
        /// Montant du solde FAR 
        /// Champ utilisé pour l'export excel des Comptes d'Exploitation
        /// </summary>
        public decimal SoldeFar { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de la ligne de commande d'une dépense.
        /// </summary>
        public int? CommandeLigneId { get; set; }

        /// <summary>
        /// Obtient ou définit l'affaire d'une dépense.
        /// </summary>
        public int? CiId { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'une dépense.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit la date de la dépense.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID de la personne ayant créé la dépense.
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        /// Obtient ou définit la date de création de la dépense.
        /// </summary>
        public DateTime? DateCreation { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID de la personne ayant modifié la dépense.
        /// </summary>
        public int? AuteurModificationId { get; set; }
        /// <summary>
        /// Obtient ou définit la date de modification de la dépense.
        /// </summary>
        public DateTime? DateModification { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID de la personne ayant supprimer la dépense.
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        /// Obtient ou définit la date de suppression de la dépense.
        /// </summary>
        public DateTime? DateSuppression { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro du bon de livraison associé à une dépense.
        /// </summary>
        public string NumeroBL { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de la dépense parent
        /// </summary>
        public int? DepenseParentId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du type de dépense.
        /// </summary>
        public int? DepenseTypeId { get; set; }

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
        /// Obtient ou définit l'identifiant de la personne ayant fait le visa reception.
        /// </summary>
        public int? AuteurVisaReceptionId { get; set; }

        /// <summary>
        /// Obtient ou définit la quantité d'une dépense.
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
        /// Obtient ou définit un booléen déterminant si la Far est annulée ou pas
        /// </summary>
        public bool FarAnnulee { get; set; }

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
        public DateTime? DateControleFar { get; set; }

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
        public DateTime? DateOperation { get; set; }

        /// <summary>
        /// Obtient ou définit si la dépense est une réception intérimaire ou non 
        /// </summary>
        public bool IsReceptionInterimaire { get; set; }

        /// <summary>
        /// Obtient ou définit si la dépense est une réception matériel externe ou non 
        /// </summary>
        public bool IsReceptionMaterielExterne { get; set; }
    }
}
