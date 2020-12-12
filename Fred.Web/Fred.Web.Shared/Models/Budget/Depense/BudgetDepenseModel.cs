using System;
using System.Collections.Generic;
using Fred.Entities.CI;
using Fred.Entities.Depense;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;

namespace Fred.Web.Shared.Models.Budget.Depense
{
    /// <summary>
    /// Représente un model de dépense dédié au budget
    /// </summary>
    public class BudgetDepenseModel
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
    }
}
