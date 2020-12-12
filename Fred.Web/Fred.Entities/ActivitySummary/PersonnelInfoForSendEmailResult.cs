namespace Fred.Entities.ActivitySummary
{
    /// <summary>
    /// Contient les information necessaires pour l'envoie d'un email
    /// </summary>
    public class PersonnelInfoForSendEmailResult
    {
        /// <summary>
        /// PersonnelId
        /// </summary>
        public int PersonnelId { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Nom
        /// </summary>
        public string Nom { get; set; }
        /// <summary>
        /// Prenom
        /// </summary>
        public string Prenom { get; set; }

        /// <summary>
        /// Matricule
        /// </summary>
        public string Matricule { get; set; }
    }
}
