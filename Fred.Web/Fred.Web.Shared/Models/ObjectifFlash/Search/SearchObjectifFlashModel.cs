using System;
using Fred.Web.Models;
using Fred.Web.Models.CI;

namespace Fred.Web.Shared.Models.ObjectifFlash.Search
{
    /// <summary>
    ///   Représente une recherche d'Objectif Flash
    /// </summary>
    public class SearchObjectifFlashModel
    {
        /// <summary>
        ///   Obtient ou définit le CI
        /// </summary>
        public CIModel CI { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de début de recherche
        /// </summary>
        public DateTime? DateDebut { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de fin de recherche
        /// </summary>
        public DateTime? DateFin { get; set; }

        /// <summary>
        ///   Obtient ou définit le flag de filtre sur les objectif flash cloturés
        /// </summary>
        public bool DisplayClosed { get; set; }
    }
}
