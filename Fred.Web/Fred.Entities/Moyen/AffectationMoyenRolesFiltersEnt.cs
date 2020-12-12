using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Fred.Entities.Moyen
{
    /// <summary>
    /// Représente les critéres de recherche d'une affectation d'un moyen en fonction des roles de l'utilisateur connecté
    /// </summary>
    public class AffectationMoyenRolesFiltersEnt
    {
        /// <summary>
        /// L'identifiant de l'utilisateur connecté
        /// </summary>
        public int UtilisateurId { get; set; }

        /// <summary>
        /// Booléan indique si l'utilisateur est un super admin
        /// </summary>
        public bool IsSuperAdmin { get; set; }

        /// <summary>
        /// Booléan indique si l'utilisateur connecté a la permission pour voir toute la liste des affectations des moyens
        /// </summary>
        public bool HasPermissionToSeeAllAffectationMoyens { get; set; }

        /// <summary>
        /// Booléan indique si l'utilisateur connecté a la permission pour voir la liste des affectations des moyens qui concernent les personnels dont il est manager
        /// </summary>
        public bool HasPermissionToSeeManagerPersonnelsAffectationMoyens { get; set; }

        /// <summary>
        /// Liste des identifiants des personnels sous la responsabilité du personnel connecté
        /// </summary>
        public List<int> ManagerPersonnelsList { get; set; } = new List<int>();

        /// <summary>
        /// Booléan indique si l'utilisateur connecté a la permission pour voir la liste des affectations des moyens qui concernent le responsable CI
        /// </summary>
        public bool HasPermissionToSeeResponsableCiAffectationMoyens { get; set; }

        /// <summary>
        /// Liste des identifiants des CI du responsable connecté
        /// </summary>
        public List<int> ResponsableCiList { get; set; } = new List<int>();

        /// <summary>
        /// Liste des identifiants des personnels attaché au CI du reponsable connecté
        /// </summary>
        public List<int> ResponsableCiPersonnelList { get; set; } = new List<int>();

        /// <summary>
        /// Booléan indique si l'utilisateur connecté a la permission pour voir la liste des affectations des moyens qui concernent le délégué CI
        /// </summary>
        public bool HasPermissionToSeeDelegueCiAffectationMoyens { get; set; }

        /// <summary>
        /// Liste des identifiants des personnels attaché au CI du délégué connecté
        /// </summary>
        public List<int> DelegueCiList { get; set; } = new List<int>();

        /// <summary>
        ///   Permet de récupérer le prédicat de recherche d'une affectation moyen en fonction des rôles de l'utilisateur.
        /// </summary>
        /// <returns>Retourne la condition de recherche de l'affectation moyen</returns>
        public Expression<Func<AffectationMoyenEnt, bool>> GetPredicateWhere()
        {
            return a => IsSuperAdmin == true
                        || HasPermissionToSeeAllAffectationMoyens == true
                        || (HasPermissionToSeeManagerPersonnelsAffectationMoyens == true && ManagerPersonnelsList.Contains(a.PersonnelId.Value))
                        || (HasPermissionToSeeResponsableCiAffectationMoyens == true && (ResponsableCiList.Contains(a.CiId.Value) || ResponsableCiPersonnelList.Contains(a.PersonnelId.Value)))
                        || (HasPermissionToSeeDelegueCiAffectationMoyens == true && DelegueCiList.Contains(a.CiId.Value))
                        || a.PersonnelId == UtilisateurId;
        }
    }
}
