using System;
using System.Collections.Generic;
using Fred.Entities.Utilisateur;

namespace Fred.Entities.ValidationPointage
{
    /// <summary>
    ///   Classe Controle Pointage
    /// </summary>
    public class ControlePointageEnt
    {
        private DateTime dateDebut;
        private DateTime? dateFin;

        /// <summary>
        ///   Obtient ou définit l'identifiant d'une ControlePointageEnt
        /// </summary>
        public int ControlePointageId { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de début
        /// </summary>
        public DateTime DateDebut
        {
            get { return DateTime.SpecifyKind(dateDebut, DateTimeKind.Utc); }
            set { dateDebut = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
        }

        /// <summary>
        ///   Obtient ou définit la date de fin
        /// </summary>
        public DateTime? DateFin
        {
            get { return (dateFin.HasValue) ? DateTime.SpecifyKind(dateFin.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateFin = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        ///   Obtient ou définti le statut
        /// </summary>
        public int Statut { get; set; }

        /// <summary>
        ///   Obtient ou définit le type de Controle (Contrôle Chantier, Contrôle Vrac, Remontée Vrac)
        /// </summary>
        public int TypeControle { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du lot de pointage
        /// </summary>
        public int LotPointageId { get; set; }

        /// <summary>
        ///   Obtient ou définit le lot de pointage
        /// </summary>
        public LotPointageEnt LotPointage { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la création
        /// </summary>
        public int AuteurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'auteur de la création
        /// </summary>    
        public UtilisateurEnt AuteurCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des erreurs liés au CONTROLE CHANTIER
        /// </summary>
        public virtual ICollection<ControlePointageErreurEnt> Erreurs { get; set; }
    }
}