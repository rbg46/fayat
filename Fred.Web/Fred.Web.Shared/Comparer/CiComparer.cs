using Fred.Entities.CI;
using System.Collections.Generic;

namespace Fred.Web.Shared.Comparer
{
    /// <summary>
    /// Classe pour comparer deux objets de types CIEnt
    /// </summary>
    public class CiComparer : IEqualityComparer<CIEnt>
    {
        /// <summary>
        /// Vérifie l'égalité entre deux objets CiEnt
        /// </summary>
        /// <param name="x">Premier objet CiEnt</param>
        /// <param name="y">Deuxiéme objet CiEnt</param>
        /// <returns></returns>
        public bool Equals(CIEnt x, CIEnt y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.CiId == y.CiId;
        }

        /// <summary>
        /// Get hash code
        /// </summary>
        /// <param name="obj">l'objet courant</param>
        /// <returns>Hashcode de l'objet</returns>
        public int GetHashCode(CIEnt obj)
        {
            if (obj == null) return 0;

            if (obj.Code != null)
            {
                return obj.Code.GetHashCode();
            }

            return obj.CiId;
        }
    }
}
