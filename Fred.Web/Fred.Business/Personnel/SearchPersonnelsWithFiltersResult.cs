using Fred.Entities.Personnel;
using System.Collections.Generic;

namespace Fred.Business.Personnel
{
    /// <summary>
    ///   Classe representant le resultat d'une recheche de personnel
    /// </summary>
    public class SearchPersonnelsWithFiltersResult
    {
        /// <summary>
        /// Liste de personnel en fonction du paging
        /// </summary>
        public List<PersonnelEnt> Personnels { get; internal set; }

        /// <summary>
        /// Nombre total d'element sans le paging
        /// </summary>
        public int TotalCount { get; internal set; }

        /// <summary>
        /// Utilisé lors de l'optimisation du picklist personnel
        /// </summary>
        public List<PersonnelListResultViewModel> PersonnelList { get; internal set; }
    }
}
