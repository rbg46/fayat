using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Fred.Entities.Notification
{
    /// <summary>
    ///   Représente un type de Notification
    /// </summary>
    [DebuggerDisplay("NotificationId = {NotificationId} UtilisateurId = {UtilisateurId} TypeNotification = {TypeNotification} Message = {Message} EstConsulte = {EstConsulte} ")]
    public class NotificationEnt
    {
        private DateTime dateCreation;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une Notification.
        /// </summary>
        public int NotificationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id de l'utilisateur auquel est affecté la Notification.
        /// </summary>
        public int UtilisateurId { get; set; }

        /// <summary>
        ///   Obtient ou définit le type d'une Notification.
        /// </summary>
        public TypeNotification TypeNotification { get; set; }

        /// <summary>
        ///   Obtient ou définit le message d'une Notification.
        /// </summary>
        [Required]
        public string Message { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de création d'une Notification.
        /// </summary>
        public DateTime DateCreation
        {
            get { return DateTime.SpecifyKind(dateCreation, DateTimeKind.Utc); }
            set { dateCreation = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
        }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la notification a été consultée.
        /// </summary>
        public bool EstConsulte { get; set; }
    }
}