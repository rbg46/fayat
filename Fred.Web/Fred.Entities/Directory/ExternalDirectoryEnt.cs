using Fred.Entities.Utilisateur;
using System;
using System.Collections.Generic;

namespace Fred.Entities.Directory
{
    /// <summary>
    ///   Représente le ticket sécurité de l'utilisateur Externe
    /// </summary>
    public class ExternalDirectoryEnt
    {
        private DateTime? dateExpiration;

        /// <summary>
        ///   Obtient ou définit identifiant du ticket de l'utilisateur externe
        /// </summary>
        public int FayatAccessDirectoryId { get; set; }

        /// <summary>
        ///   Obtient ou définit le mot de passe de l'utilisateur externe
        /// </summary>
        public string MotDePasse { get; set; }

        /// <summary>
        ///   Obtient ou définit la date d'expiration de l'abonnement l'utilisateur externe
        /// </summary>
        public DateTime? DateExpiration
        {
            get
            {
                return dateExpiration.HasValue ? DateTime.SpecifyKind(dateExpiration.Value, DateTimeKind.Utc) : null as DateTime?;
            }
            set
            {
                dateExpiration = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : null as DateTime?;
            }
        }

        // DENORMALISATION RENOMMER LA COLONNE IsActived
        // L'ATTRIBUT '[Column] EST DIFFERENT DU NOM DE LA PROPRIETE.
        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le statut de connexion de l'utilisateur externe
        /// </summary>
        public bool IsActived { get; set; }

        /// <summary>
        ///   Obtient ou définit le guid pour la réinitialisation des mot de passe
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        ///   Obtient ou définit la date d'expiration du guid
        /// </summary>
        public DateTime? DateExpirationGuid { get; set; }

        /// <summary>
        /// Child Utilisateurs where [FRED_UTILISATEUR].[FayatAccessDirectoryId] point to this entity (FK_UTILISATEUR_EXTERNEDIRECTORY)
        /// </summary>
        public virtual ICollection<UtilisateurEnt> Utilisateurs { get; set; }
    }
}