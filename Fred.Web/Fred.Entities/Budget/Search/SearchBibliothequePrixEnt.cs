using System;
using Fred.Entities.Search;

namespace Fred.Entities.Budget.Search
{
    /// <summary>
    /// Représente une recherche pour la bibliothèque des prix.
    /// </summary>
    [Serializable]
    public class SearchBibliothequePrixEnt : AbstractSearch
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
