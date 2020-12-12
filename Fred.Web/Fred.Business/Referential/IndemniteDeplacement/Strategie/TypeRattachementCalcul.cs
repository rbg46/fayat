using Fred.Entities.CI;
using Fred.Entities.Personnel;

namespace Fred.Business.Referential.IndemniteDeplacement
{
    /// <summary>
    /// Classe generic des types de calcul des indemnités de déplacement
    /// </summary>
    /// <typeparam name="TTypeRattachementCalcul">classe qui définit le type de ratachement pour le calcul</typeparam>
    public class TypeRattachementCalcul<TTypeRattachementCalcul> : ITypeRattachementCalcul
        where TTypeRattachementCalcul : ITypeRattachementCalcul, new()
    {
        private readonly ITypeRattachementCalcul typeRattachementCalcul = new TTypeRattachementCalcul();

        /// <summary>
        /// PErmet de déterminer la zone déplacement
        /// </summary>
        /// <param name="ci">entre imputation</param>
        /// <param name="personnel">personnel</param>
        /// <returns>instance de CodeZoneDeplacementEnt</returns>
        public double CalculKilometre(CIEnt ci, PersonnelEnt personnel)
        {
            return typeRattachementCalcul.CalculKilometre(ci, personnel);
        }
    }
}
