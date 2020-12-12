using Fred.Web.Models;
using Fred.Web.Models.Search;

namespace Fred.Web.Shared.Models.Referential
{
    /// <summary>
    ///   Représente une recherche de plan de tache
    /// </summary>
    public class SearchTacheModel : ISearchValueModel
    {
        /// <summary>
        ///   Obtient ou définit la valeur recherchée
        /// </summary>
        public string ValueText { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un ci
        /// </summary>
        public CILightModel CI { get; set; }
    }
}
