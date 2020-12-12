namespace Fred.Web.Shared.Models.Budget.Search
{
    /// <summary>
    /// Représente une recherche pour la bibliothèque des prix.
    /// </summary>
    public class SearchBibliothequePrixModel
    {
        /// <summary>
        /// Identifiant de l'organisation.
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        /// Identifiant de la devise.
        /// </summary>
        public int DeviseId { get; set; }
    }
}
