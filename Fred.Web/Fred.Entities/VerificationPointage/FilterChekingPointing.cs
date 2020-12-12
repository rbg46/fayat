using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Entities.Rapport;

namespace Fred.Entities.VerificationPointage
{
    /// <summary>
    /// Représente le modèle des filtres pour Verification de pointage
    /// </summary>
    public class FilterChekingPointing
    {
        /// <summary>
        /// Liste cis
        /// </summary>
        public ICollection<int> Cis { get; set; }
        /// <summary>
        /// Type Pointage (Personnel/Materiel)
        /// </summary>
        public int TypePointing { get; set; }

        /// <summary>
        /// Obtient ou définit  la periode 
        /// </summary>
        public DateTime? Period { get; set; }

        /// <summary>
        ///   Permet de récupérer le prédicat de recherche
        /// </summary>
        /// <returns>Retourne la condition de recherche</returns>
        public Expression<Func<RapportLigneEnt, bool>> GetPredicateWhere()
        {
            return rl => (rl.Rapport.DateChantier.Month == Period.Value.Month)
                         && (rl.Rapport.DateChantier.Year == Period.Value.Year)
                         && ((TypePointing == (int)DisplayTypePointing.Personnels)? rl.PersonnelId != null : rl.MaterielId != null)
                         && (Cis.Contains(rl.Ci.CiId))
                         && (rl.DateSuppression ==null) ;
        }
    }
}
