namespace Fred.Web.Shared.Models
{
    public class PersonnelLightModel2
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique du membre du personnel
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        ///   Obtient la chaîne Code Société + Matricule + Prénom Nom du personnel
        /// </summary>
        public string CodeSocieteMatriculePrenomNom { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de la société.
        /// </summary>
        public int? SocieteId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du groupe
        /// </summary>
        public int? GroupeId { get; set; }

        /// <summary>
        /// Obtient ou définit le matricule du membre du personnel
        /// </summary>
        public string Matricule { get; set; }

        /// <summary>
        /// Obtient ou définit le nom du membre du personnel
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Obtient ou définit le prénom du membre du personnel
        /// </summary>
        public string Prenom { get; set; }

        /// <summary>
        /// Obtient ou défini une valeur indiquant le matricule - prenom - nom du personnel
        /// </summary>
        public string MatriculePrenomNom => $"{this.Matricule} - {this.PrenomNom}";

        /// <summary>
        /// Obtient une concaténation du nom et du prénom du membre du personnel
        /// </summary>
        public string NomPrenom { get; set; }

        /// <summary>
        /// Obtient une concaténation du prénom et du nom du membre du personnel
        /// </summary>
        public string PrenomNom { get; set; }
    }
}
