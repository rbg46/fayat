using System;

namespace Fred.Entities.Log
{
    /// <summary>
    /// Représente un log pour gestionnaire de log 'NLog'
    /// </summary>
    public class NLogEnt
    {
        private DateTime logged;

        /// <summary>
        /// Obtient ou définit l'identifiant unique
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtient ou définit le nom de l'application
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// Obtient ou définit la date 
        /// </summary>
        public DateTime Logged
        {
            get
            {
                return DateTime.SpecifyKind(logged, DateTimeKind.Utc);
            }
            set
            {
                logged = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
        }

        /// <summary>
        /// Obtient ou définit le niveau 
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Obtient ou définit le message 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Obtient ou définit le nom de l'utilisateur
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Obtient ou définit le nom du serveur
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Obtient ou définit le numero de port
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// Obtient ou définit l'url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la commande contient si l'url est en HTTPS
        /// </summary>
        public bool Https { get; set; }

        /// <summary>
        /// Obtient ou définit l'adresse du serveur
        /// </summary>
        public string ServerAddress { get; set; }

        /// <summary>
        /// Obtient ou définit l'adresse du serveur distant
        /// </summary>
        public string RemoteAddress { get; set; }

        /// <summary>
        /// Obtient ou définit le nom du logger
        /// </summary>
        public string Logger { get; set; }

        /// <summary>
        /// Obtient ou définit le Callsite
        /// </summary>
        public string Callsite { get; set; }

        /// <summary>
        /// Obtient ou définit l'exception
        /// </summary>
        public string Exception { get; set; }
    }
}
