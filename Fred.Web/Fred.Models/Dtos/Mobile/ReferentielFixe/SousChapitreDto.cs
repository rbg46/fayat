namespace Fred.Web.Dtos.Mobile.ReferentielFixe
{
    public class SousChapitreDto
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une sous-chapitre.
        /// </summary>
        public int SousChapitreId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un chapitre.
        /// </summary>
        public int ChapitreId { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'un sous-chapitre.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'un sous-chapitre.
        /// </summary>
        public string Libelle { get; set; }
    }
}