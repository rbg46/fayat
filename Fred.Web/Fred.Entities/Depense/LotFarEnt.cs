using System;
using Fred.Entities.Utilisateur;

namespace Fred.Entities.Depense
{
    /// <summary>
    ///   Entité LotFar
    /// </summary>
    public class LotFarEnt
    {
        private DateTime dateComptable;
        private DateTime dateCreation;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un lot de FAR
        /// </summary>
        public int LotFarId { get; set; }

        /// <summary>
        ///   Obtient ou définit le numéro de lot
        /// </summary>
        public int NumeroLot { get; set; }

        /// <summary>
        ///   Obtient ou définit la date comptable 
        /// </summary>
        public DateTime DateComptable
        {
            get { return DateTime.SpecifyKind(dateComptable, DateTimeKind.Utc); }
            set { dateComptable = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
        }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'utilisateur ayant demandé l'envoi des FARS
        /// </summary>
        public int AuteurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Utilisateur ayant demandé l'envoi des FARS
        /// </summary>
        public UtilisateurEnt AuteurCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit la date
        /// </summary>
        public DateTime DateCreation
        {
            get { return DateTime.SpecifyKind(dateCreation, DateTimeKind.Utc); }
            set { dateCreation = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
        }
    }
}
