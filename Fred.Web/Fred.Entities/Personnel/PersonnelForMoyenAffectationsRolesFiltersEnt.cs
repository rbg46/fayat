using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Fred.Entities.Personnel
{
    /// <summary>
    /// Représente les critéres de recherche d'un personnel pour l'affecter à moyen en fonction des roles de l'utilisateur connecté
    /// </summary>
    public class PersonnelForMoyenAffectationsRolesFiltersEnt
    {
        /// <summary>
        /// Booléan indique si l'utilisateur connecté est un super admin
        /// </summary>
        public bool IsSuperAdmin { get; set; }

        /// <summary>
        /// Booléan indique si l'utilisateur connecté a le rôle manager des personnels
        /// </summary>
        public bool IsManagerPersonnel { get; set; }

        /// <summary>
        /// Liste des identifiants des personnels sous la responsabilité du personnel connecté
        /// </summary>
        public List<int> ManagerPersonnelsList { get; set; } = new List<int>();

        /// <summary>
        /// Booléan indique si l'utilisateur connecté a le rôle responsable CI
        /// </summary>
        public bool IsResponsableCI { get; set; }

        /// <summary>
        /// Liste des identifiants des personnels attaché au CI du reponsable connecté
        /// </summary>
        public List<int> ResponsableCiPersonnelList { get; set; } = new List<int>();

        /// <summary>
        /// Pour affectation des moyens ou non
        /// </summary>
        public bool ForAffectationMoyen { get; set; }

        /// <summary>
        /// Booléan indique si l'utilisateur connecté a la permission pour voir la liste des personnels
        /// </summary>
        public bool HasPermissionToSeePersonnelList { get; set; }

        /// <summary>
        ///   Permet de récupérer le prédicat de recherche d'un personnel en fonction des rôles de l'utilisateur.
        /// </summary>
        /// <returns>Retourne la condition de recherche de l'affectation moyen</returns>
        public Expression<Func<PersonnelEnt, bool>> GetPredicateWhere()
        {
            return p => !ForAffectationMoyen
                        || IsSuperAdmin
                        || HasPermissionToSeePersonnelList
                        || (IsManagerPersonnel && ManagerPersonnelsList.Contains(p.PersonnelId))
                        || (IsResponsableCI && ResponsableCiPersonnelList.Contains(p.PersonnelId));
        }
    }
}
