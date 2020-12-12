namespace Fred.Web.Shared.Models.Moyen
{
    /// <summary>
    /// Représente les critéres de recherche d'un établissement comptable d'un moyen
    /// </summary>
    public class SearchEtablissementMoyenModel
    {
        /// <summary>
        /// Text de la recherche
        /// </summary>
        public string Recherche { get; set; }

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
        /// Société
        /// </summary>
        public string Societe { get; set; }
    }
}

