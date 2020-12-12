namespace Fred.Web.Models
{
    /// <summary>
    /// Représente un modèle contenant les critères de filtrages types OD des dépenses affichées dans l'explorateur des dépenses
    /// </summary>
    public class TypeOdFilterExplorateurDepense
    {
        /// <summary>
        /// Obtient ou définit le champ de texte de recherche
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le champ de texte de recherche
        /// </summary>
        public string LibelleCourt { get; set; }
    }
}
