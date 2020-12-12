using Fred.Entities.CI;
using Fred.Entities.Personnel;

namespace Fred.Business.Referential.IndemniteDeplacement
{
    /// <summary>
    /// Interface permettant de calculer les indemnités suivant un type de rattachement
    /// </summary>
    public interface ITypeRattachementCalcul
    {
        /// <summary>
        /// Permet de calculer le nombre de kilomètre
        /// </summary>
        /// <param name="ci">entre imputation</param>
        /// <param name="personnel">personnel</param>
        /// <returns>un nombre décimal</returns>
        double CalculKilometre(CIEnt ci, PersonnelEnt personnel);
    }
}
