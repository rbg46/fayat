using System;

namespace Fred.Web.Shared.Models.Moyen
{
    /// <summary>
    /// Représente les critéres de recherche d'une ligne de rapport d'un moyen
    /// </summary>
    public class SearchRapportLigneMoyenModel : SearchBaseMoyenModel
    {
        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la date minimale pour laquelle récupérer les pointages
        /// </summary>
        public DateTime DatePointageMin { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la date maximale pour laquelle récupérer les pointages
        /// </summary>
        public DateTime DatePointageMax { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si l'ordre de tri sur les dates
        /// </summary>
        public bool? DatePointageAsc { get; set; }
    }
}
