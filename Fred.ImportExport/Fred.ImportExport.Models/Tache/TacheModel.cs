namespace Fred.ImportExport.Models.Tache
{
    /// <summary>
    /// Représente un model pour une tâche.
    /// </summary>
    public class TacheModel
    {
        /// <summary>
        /// Obtient ou définit le code d'un CI.
        /// </summary>
        public string CodeCi { get; set; }

        /// <summary>
        /// Obtient ou définit le code de ma société.
        /// </summary>
        public string CodeSociete { get; set; }

        /// <summary>
        /// Obtient ou définit le code de l'établissement.
        /// </summary>
        public string CodeEtablissement { get; set; }

        /// <summary>
        /// Obtient ou définit le code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé.
        /// </summary>
        public string Libelle { get; set; }

    }
}
