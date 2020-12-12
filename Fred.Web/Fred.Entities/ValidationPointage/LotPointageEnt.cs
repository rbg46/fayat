using System;
using System.Collections.Generic;
using Fred.Entities.EntityBase;
using Fred.Entities.Rapport;
using Fred.Entities.Utilisateur;

namespace Fred.Entities.ValidationPointage
{
    /// <summary>
    ///   Classe Lot Pointage
    /// </summary>
    public class LotPointageEnt : AuditableEntity
    {
        private DateTime periode;

        /// <summary>
        ///   Obtient ou définit l'identifiant du lot de Pointage
        /// </summary>
        public int LotPointageId { get; set; }

        /// <summary>
        ///   Obtient ou définit la date comptable
        /// </summary>
        public DateTime Periode
        {
            get { return DateTime.SpecifyKind(periode, DateTimeKind.Utc); }
            set { periode = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
        }

        /// <summary>
        ///   Obtient ou définit la date du Visa
        /// </summary>
        public DateTime? DateVisa { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'utilisateur ayant visé le lot de pointage
        /// </summary>
        public int? AuteurVisaId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'auteur du Visa
        /// </summary>
        public UtilisateurEnt AuteurVisa { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des contrôles de pointages (Contrôle Chantier, Contrôle Vrac, Remontée Vrac)
        /// </summary>
        public virtual ICollection<ControlePointageEnt> ControlePointages { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des lignes de rapports du lot de pointage
        /// </summary>
        public virtual ICollection<RapportLigneEnt> RapportLignes { get; set; }
    }
}