namespace Fred.Web.Shared.Models.Moyen
{
    /// <summary>
    /// Représente les critéres de recherche d'une immatriculation d'un moyen
    /// </summary>
    public class SearchImmatriculationMoyenModel
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
        /// Société
        /// </summary>
        public string Societe { get; set; }

        /// <summary>
        /// Etablissement id
        /// </summary>
        public int? EtablissementId { get; set; }

        /// <summary>
        /// Numéro de parc (code du moyen)
        /// </summary>
        public string NumParc { get; set; }
    }
}

