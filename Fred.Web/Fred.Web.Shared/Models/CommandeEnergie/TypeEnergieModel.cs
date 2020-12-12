namespace Fred.Web.Shared.Models
{
    /// <summary>
    /// Model Type Energie
    /// </summary>
    public class TypeEnergieModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique
        /// </summary>
        public int TypeEnergieId { get; set; }

        /// <summary>
        /// Obtient ou définit le code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libélle 
        /// </summary>
        public string Libelle { get; set; }
    }
}
