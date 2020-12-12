namespace Fred.Web.Dtos.Mobile
{
    /// <summary>
    /// Représente un membre du personnel
    /// </summary>
    public class PersonnelDto : DtoBase
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique du membre du personnel
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de la société.
        /// </summary>
        public int? SocieteId { get; set; }

        /// <summary>
        /// Obtient ou définit le matricule du membre du personnel
        /// </summary>
        public string Matricule { get; set; }

        /// <summary>
        /// Obtient ou définit le code personnel Externe
        /// </summary>
        public int? PersonnelExterneId { get; set; }

        /// <summary>
        /// Obtient ou définit le nom du membre du personnel
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Obtient ou définit le prénom du membre du personnel
        /// </summary>
        public string Prenom { get; set; }

        /// <summary>
        /// Obtient ou définit le prénom du membre du personnel
        /// </summary>
        public string IsInterne { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'un utilisateur lié au personnel.
        /// </summary>
        public int? UtilisateurId { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro de téléphone 1 du personnel.
        /// </summary>
        public string Telephone1 { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro de téléphone 2 du personnel.
        /// </summary>
        public string Telephone2 { get; set; }
    }
}