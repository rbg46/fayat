namespace Fred.Web.Dtos.Mobile.ReferentielFixe
{
    /// <summary>
    /// Dto ressource
    /// </summary>
    public class RessourceDto : DtoBase
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une ressource.
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un sous-chapitre.
        /// </summary>
        public int SousChapitreId { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'une ressource.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'une ressource.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'une ressource.
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Obtient ou définit l
        /// </summary>
        public int? TypeRessourceId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du carburant
        /// </summary>
        public int? CarburantId { get; set; }

        /// <summary>
        /// Obtient ou définit la consommation d'une ressource matérielle
        /// </summary>
        public decimal? Consommation { get; set; }
    }
}