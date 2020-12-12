using System;
using System.Collections.Generic;
using System.Diagnostics;
using Fred.Entities.CI;
using Fred.Entities.Depense;
using Fred.Entities.EcritureComptable;
using Fred.Entities.EntityBase;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;

namespace Fred.Entities.OperationDiverse
{
    /// <summary>
    ///   Représente un module.
    /// </summary>
    [DebuggerDisplay("OperationDiverseId = {OperationDiverseId} TacheId = {TacheId} Libelle = {Libelle} PUHT = {PUHT} CiId = {CiId} Quantite = {Quantite} PUHT = {PUHT} ")]
    public class OperationDiverseEnt : Creatable
    {
        private DateTime? dateCloture;
        private DateTime? dateComptable;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une OD.
        /// </summary>
        public int OperationDiverseId { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'une OD.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit le commentaire.
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        ///   Obtient ou définit l'affaire d'une OD.
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'affaire d'une OD.
        /// </summary>
        public CIEnt CI { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Id de la tâche d'une OD.
        /// </summary>
        public int TacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'objet tache
        /// </summary>
        public TacheEnt Tache { get; set; }

        /// <summary>
        ///   Obtient ou définit le prix unitaire d'une OD.
        /// </summary>
        public decimal PUHT { get; set; }

        /// <summary>
        ///   Obtient ou définit la quantité d'une OD.
        /// </summary>
        public decimal Quantite { get; set; }

        /// <summary>
        ///   Obtient le montant HT de la OD
        /// </summary>
        public decimal Montant { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id de l'unité .
        /// </summary>
        public int UniteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'unité.
        /// </summary>
        public UniteEnt Unite { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la devise associée à une OD.
        /// </summary>
        public int DeviseId { get; set; }

        /// <summary>
        ///   Obtient ou définit la devise associée à une OD.
        /// </summary>
        public DeviseEnt Devise { get; set; }

        /// <summary>
        ///   Obtient ou définit si l'OD est cloturee.
        /// </summary>
        public bool Cloturee { get; set; }

        /// <summary>
        /// Obtient ou définit si l'OD est une od d'ecart.
        /// Ce booléeen permet avant tout de savoir si ces OD seront supprimé lors de la déclôture du mois
        /// </summary>
        public bool OdEcart { get; set; }

        /// <summary>
        ///   Obtient ou définit la DateCloture.
        /// </summary>
        public DateTime? DateCloture
        {
            get
            {
                return (dateCloture.HasValue) ? DateTime.SpecifyKind(dateCloture.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateCloture = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit la DateCloture.
        /// </summary>
        public DateTime? DateComptable
        {
            get
            {
                return (dateComptable.HasValue) ? DateTime.SpecifyKind(dateComptable.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateComptable = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        /// Identifiant de la famille d'opération diverses
        /// </summary>
        public int FamilleOperationDiverseId { get; set; }

        /// <summary>
        /// Famille d'opération diverse
        /// </summary>
        public FamilleOperationDiverseEnt FamilleOperationDiverse { get; set; }

        /// <summary>
        /// Identifiant de la ressource
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        /// Ressource
        /// </summary>
        public RessourceEnt Ressource { get; set; }

        /// <summary>
        ///   Obtient ou définit le groupe de tâches de remplacement.
        /// </summary>
        public GroupeRemplacementTacheEnt GroupeRemplacementTache { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id du groupe de tâches de remplacement.
        /// </summary>
        public int? GroupeRemplacementTacheId { get; set; }

        /// <summary>
        /// EcritureComptableId
        /// </summary>
        public int? EcritureComptableId { get; set; }

        /// <summary>
        /// Ecriture Comptable
        /// </summary>
        public EcritureComptableEnt EcritureComptable { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des tâches remplacées    
        /// </summary>
        public IEnumerable<RemplacementTacheEnt> RemplacementTaches { get; set; }

        /// <summary>
        /// Obtiens ou définit l'identifiant de la ligne de rapport
        /// </summary>
        public int? RapportLigneId { get; set; }

        /// <summary>
        /// Obitnes ou définit l'identifiant de la commande
        /// </summary>
        public int? CommandeId { get; set; }

        /// <summary>
        /// Obtient ou définit l'appartenance à un abonnement
        /// </summary>
        public bool EstUnAbonnement { get; set; }

        /// <summary>
        /// Obtient ou définit l'id de la première opération diverse d'un abonnement
        /// </summary>
        public int? OperationDiverseMereIdAbonnement { get; set; }

        /// <summary>
        /// Obtient ou définit la première opération diverse d'un abonnement
        /// </summary>
        public OperationDiverseEnt OperationDiverseMereAbonnement { get; set; }
    }
}
