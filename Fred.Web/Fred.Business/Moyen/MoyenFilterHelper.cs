using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Fred.Business.Moyen
{
    /// <summary>
    /// Helper du moyen Filter manager 
    /// </summary>
    public class MoyenFilterHelper
    {
        /// <summary>
        /// Get materiel groupe filter (Restrictions des chapitres par code groupe)
        /// </summary>
        /// <param name="chapitresIds">chapitres Ids</param>
        /// <returns>Expression Materiel > bool</returns>
        public Expression<Func<MaterielEnt, bool>> GetMaterielGroupeFilter(IEnumerable<int> chapitresIds)
        {
            return i => i.Ressource != null
                            && i.Ressource.SousChapitre != null
                            && i.Ressource.SousChapitre.Chapitre != null
                            && i.Ressource.SousChapitre.Chapitre.Code != null
                            && chapitresIds.Any(f => f == i.Ressource.SousChapitre.Chapitre.ChapitreId);
        }

        /// <summary>
        /// Get sous chapitre groupe filter (Restrictions des chapitres par code groupe)
        /// </summary>
        /// <param name="chapitresIds">chapitres Ids</param>
        /// <returns>Expression Sous Chapitre > bool</returns>
        public Expression<Func<SousChapitreEnt, bool>> GetSousChapitreGroupeFilter(IEnumerable<int> chapitresIds)
        {
            return sc => sc.Chapitre != null
                            && sc.Chapitre.Code != null
                            && chapitresIds.Any(f => f == sc.Chapitre.ChapitreId);
        }

        /// <summary>
        /// Get ressource groupe filter (Restrictions des ressources par code groupe)
        /// </summary>
        /// <param name="chapitresIds">chapitres Ids</param>
        /// <returns>Expression ressource > bool</returns>
        public Expression<Func<RessourceEnt, bool>> GetRessourceGroupeFilter(IEnumerable<int> chapitresIds)
        {
            return rs => rs.SousChapitre != null
                            && rs.SousChapitre.Chapitre != null
                            && rs.SousChapitre.Chapitre.Code != null
                            && chapitresIds.Any(f => f == rs.SousChapitre.Chapitre.ChapitreId);
        }
    }
}
