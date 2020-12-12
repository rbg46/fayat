namespace Fred.Entities.Budget.Dao.Budget.ExcelExport
{
    /// <summary>
    /// Représente un personnel.
    /// </summary>
    public class PersonnelDao
    {
        /// <summary>
        /// Le matricule.
        /// </summary>
        public string Matricule { get; set; }

        /// <summary>
        /// Le nom.
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Le prénom.
        /// </summary>
        public string Prenom { get; set; }

        /// <summary>
        /// L'identifiant de la société.
        /// </summary>
        public int? SocieteId { get; set; }
    }
}
