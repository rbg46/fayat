using System;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Web.Shared.Enum;
using Fred.Web.Shared.Models.Enum;

namespace Fred.Web.Shared.Models.OperationDiverse
{
    public class OperationDiverseAbonnementModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une OD.
        /// </summary>
        public int OperationDiverseId { get; set; }

        /// <summary>
        /// Libelle de l'OD
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Commentaire de l'OD
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// Obtient ou définit l'affaire d'une OD.
        /// </summary>  
        public int CiId { get; set; }

        /// <summary>
        /// Obtient ou définit l'Id de la tâche d'une OD.
        /// </summary>
        public int TacheId { get; set; }

        /// <summary>
        /// Entité de la tache
        /// </summary>
        public TacheEnt Tache { get; set; }

        /// <summary>
        /// Obtient ou définit le prix unitaire d'une OD.
        /// </summary>   
        public decimal PUHT { get; set; }

        /// <summary>
        /// Obtient ou définit la quantité d'une OD.
        /// </summary>    
        public decimal Quantite { get; set; }

        /// <summary>
        /// Obtient le montant HT de la OD
        /// </summary>
        public decimal Montant { get; set; }

        /// <summary>
        ///  Obtient ou définit l'unité.
        /// </summary>
        public UniteEnt Unite { get; set; }

        /// <summary>
        ///  Obtient ou définit l'unité.
        /// </summary>
        public int UniteId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la devise associée à une OD.
        /// </summary>  
        public int DeviseId { get; set; }

        /// <summary>
        /// Obtiens ou determine si un OD est clôturée
        /// </summary>
        public bool Cloturee { get; set; }

        /// <summary>
        /// Indique si l'OD est une od d'écart
        /// </summary>
        public bool OdEcart { get; set; }

        /// <summary>
        /// Date de cloture de l'OD
        /// </summary>
        public DateTime? DateCloture { get; set; }

        /// <summary>
        /// DateComptable
        /// </summary>
        public DateTime? DateComptable { get; set; }

        /// <summary>
        /// Obtiens ou définit l'utilisateur qui a crée l'OD
        /// </summary>
        public int AuteurCreationId { get; set; }

        /// <summary>
        /// Famille d'opeation diverse
        /// </summary>
        public int FamilleOperationDiverseId { get; set; }

        /// <summary>
        /// RessourceID
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        /// Ressource
        /// </summary>
        public RessourceEnt Ressource { get; set; }

        /// <summary>
        /// Identifiant de l'écriture comptable
        /// </summary>
        public int? EcritureComptableId { get; set; }

        /// <summary>
        /// Date de création de l'OD
        /// </summary>
        public DateTime DateCreation { get; set; }

        /// <summary>
        /// Obtient ou définit l'appartenance à un abonnement
        /// </summary>
        public bool EstUnAbonnement { get; set; }

        /// <summary>
        /// Obtient ou définit la durée de l'abonnement (aka le nombre de récurrence)
        /// </summary>
        public int? DureeAbonnement { get; set; }

        /// <summary>
        /// Obtient ou définit le type de fréquence d'abonnement <see cref="FrequenceAbonnement"></see>
        /// </summary>
        public EnumModel FrequenceAbonnementModel { get; set; }

        /// <summary>
        /// Obtient ou définit la précédente date d'abonnement comparée à l'abonnement courant
        /// </summary>
        public DateTime? DatePreviousODAbonnement { get; set; }

        /// <summary>
        /// Obtient ou définit la prochaine date d'abonnement comparée à l'abonnement courant
        /// </summary>
        public DateTime? DateProchaineODAbonnement { get; set; }

        /// <summary>
        /// Obtient ou définit la première date de l'abonnement
        /// </summary>
        public DateTime? DatePremiereODAbonnement { get; set; }

        /// <summary>
        /// Obtient ou définit la dernière date de l'abonnement
        /// </summary>
        public DateTime? DateDerniereODAbonnement { get; set; }

        /// <summary>
        /// Obtient ou définit l'id de la première opération diverse d'un abonnement
        /// </summary>
        public int? OperationDiverseMereIdAbonnement { get; set; }

        /// <summary>
        /// Obtient ou définit le besoin de supprimer l'opération diverse
        /// </summary>
        public bool NeedToBeDeleted { get; set; }
    }
}
