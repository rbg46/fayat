namespace Fred.Web.Shared.Models.Moyen
{
    /// <summary>
    /// Représente les critéres de recherche d'un moyen
    /// </summary>
    public class SearchMoyenModel
    {
        /// <summary>
        /// Text de la recherche
        /// </summary>
        public string TextRecherche { get; set; }

        /// <summary>
        /// Type de moyen
        /// </summary>
        public string TypeMoyen { get; set; }

        /// <summary>
        /// Sous type de moyen
        /// </summary>
        public string SousTypeMoyen { get; set; }

        /// <summary>
        /// Modèle de moyen
        /// </summary>
        public string ModelMoyen { get; set; }

        /// <summary>
        /// Is location view
        /// </summary>
        public bool? IsLocationView { get; set; }

        /// <summary>
        /// Etablissement Id
        /// </summary>
        public int? EtablissementId { get; set; }
    }
}
