using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Fred.Entities.CI
{
    /// <summary>
    /// Représente les critéres de recherche d'un Ci pour l'affecter à un moyen en fonction des roles de l'utilisateur connecté
    /// </summary>
    public class CiForMoyenAffectationsRolesFiltersEnt
    {
        /// <summary>
        /// Booléan indique si l'utilisateur connecté est un super admin
        /// </summary>
        public bool IsSuperAdmin { get; set; }

        /// <summary>
        /// Booléan indique si l'utilisateur connecté a la permission pour voir toute la liste des CI
        /// </summary>
        public bool HasPermissionToSeeAllCi { get; set; }

        /// <summary>
        /// Booléan indique si l'utilisateur connecté a la permission pour voir la liste des CI d'un responsable CI
        /// </summary>
        public bool HasPermissionToSeeResponsableCiList { get; set; }

        /// <summary>
        /// Liste des identifiants des CI du reponsable connecté
        /// </summary>
        public List<int> ResponsableCiList { get; set; } = new List<int>();

        /// <summary>
        /// Booléan indique si l'utilisateur connecté a la permission pour voir la liste des CI d'un délégué CI
        /// </summary>
        public bool HasPermissionToSeeDelegueCiList { get; set; }

        /// <summary>
        /// Liste des identifiants des CI d'un délégué connecté
        /// </summary>
        public List<int> DelegueCiList { get; set; } = new List<int>();

        /// <summary>
        ///   Permet de récupérer le prédicat de recherche d'un personnel en fonction des rôles de l'utilisateur.
        /// </summary>
        /// <returns>Retourne la condition de recherche de l'affectation moyen</returns>
        public Expression<Func<CIEnt, bool>> GetPredicateWhere()
        {
            return c => IsSuperAdmin == true
                        || HasPermissionToSeeAllCi == true
                        || (HasPermissionToSeeResponsableCiList == true && ResponsableCiList.Contains(c.CiId))
                        || (HasPermissionToSeeDelegueCiList == true && DelegueCiList.Contains(c.CiId));
        }
    }
}
