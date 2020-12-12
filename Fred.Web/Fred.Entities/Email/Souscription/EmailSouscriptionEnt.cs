using System;
using System.Diagnostics;
using Fred.Entities.Personnel;

namespace Fred.Entities.Email
{
    /// <summary>
    /// Représente une facturation
    /// </summary>
    [DebuggerDisplay("EmailSubscriptionId = {EmailSubscriptionId} PersonnelId = {PersonnelId} SouscriptionKey = {SouscriptionKey} DateDernierEnvoie = {DateDernierEnvoie}")]
    public class EmailSouscriptionEnt
    {
        private DateTime? dateDernierEnvoie;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une Subscription d'Email.
        /// </summary>
        public int EmailSouscriptionId { get; set; }

        /// <summary>
        ///   Obtient ou définit la date du dernier envoie du mail
        /// </summary>
        public DateTime? DateDernierEnvoie
        {
            get { return (dateDernierEnvoie.HasValue) ? DateTime.SpecifyKind(dateDernierEnvoie.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateDernierEnvoie = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        ///   Obtient ou définit l'identifiant du personnel
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité personnel,
        ///   ICI JE FAIT LE CHOIX de mettre un personnel et non un utilisateur car cela OUVRE le champ à d'autre application
        /// </summary>
        public PersonnelEnt Personnel { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant qui servira a differencie le type de souscription
        ///   Exemple : je souscrit a l'evoie de mail qui recapitule mes activité a faire sur Fred
        ///   SouscriptionKey = ActivitySummary
        /// </summary>
        public EmailSouscriptionKey SouscriptionKey { get; set; }
    }
}
