using System;

namespace Fred.Web.Dtos.Mobile.ReferentielFixe
{
    /// <summary>
    /// Dto Chapitre
    /// </summary>
    public class ChapitreDto : DtoBase
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un chapitre.
        /// </summary>
        public int ChapitreId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un groupe.
        /// </summary>
        public int GroupeId { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'un chapitre.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'un chapitre.
        /// </summary>
        public string Libelle { get; set; }
    }
}