using Fred.Entities.Affectation;

namespace Fred.ImportExport.Business.Personnel.Etl.Transform.Custom.Fes.Comparator
{
    /// <summary>
    /// Comparateur pour les affectations
    /// </summary>
    public class AffectationComparator
    {

        /// <summary>
        /// Permet de savoir si l'affectation existe
        /// </summary>
        /// <param name="dbAffectation">affectation de la base fred</param>
        /// <returns>vrai ou faux</returns>
        public bool ExistInFred(AffectationEnt dbAffectation)
        {
            return dbAffectation != null;
        }

        /// <summary>
        /// Permet de savoir si l'affectaion a changée
        /// </summary>
        /// <param name="dbAffectation">affectation de la base fred</param>
        /// <param name="newAffectation">affectation crée a la volée</param>
        /// <returns>vrai ou faux</returns>
        public bool IsModified(AffectationEnt dbAffectation, AffectationEnt newAffectation)
        {
            if (dbAffectation.CiId != newAffectation.CiId)
            {
                return true;
            }
            if (dbAffectation.PersonnelId != newAffectation.PersonnelId)
            {
                return true;
            }

            return false;
        }


    }
}
