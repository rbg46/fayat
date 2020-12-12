using System;
using Fred.Entities.Personnel;

namespace Fred.Entities.Delegation
{
    /// <summary>
    ///   Représente une délégation 
    /// </summary>
    public class DelegationEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une délégation.
        /// </summary>
        public int DelegationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique du personnel auteur de la délégation
        /// </summary>
        public int PersonnelAuteurId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité personnel auteur de la délégation
        /// </summary>   
        public PersonnelEnt PersonnelAuteur { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique du personnel délégant son habilitation
        /// </summary>
        public int PersonnelDelegantId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du personnel délégant son habilitation
        /// </summary>
        public PersonnelEnt PersonnelDelegant { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique du personnel délégué qui recevra l'habilitation
        /// </summary>
        public int PersonnelDelegueId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du personnel délégué qui recevra l'habilitation
        /// </summary>
        public PersonnelEnt PersonnelDelegue { get; set; }

        /// <summary>
        ///   Obtient ou définit si la délégation est activée
        /// </summary>
        public bool Activated { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de début de la délégation
        /// </summary>
        public DateTime DateDeDebut { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de fin de la délégation 
        /// </summary>
        public DateTime DateDeFin { get; set; }

        /// <summary>
        ///   Obtient ou définit un commentaire à la délégation
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de création de la délégation
        /// </summary>
        public DateTime DateCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de modification de la délégation 
        /// </summary>
        public DateTime? DateModification { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de désactivation de la délégation 
        /// </summary>
        public DateTime? DateDesactivation { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de suppression de la délégation 
        /// </summary>
        public DateTime? DateSuppression { get; set; }
    }
}
