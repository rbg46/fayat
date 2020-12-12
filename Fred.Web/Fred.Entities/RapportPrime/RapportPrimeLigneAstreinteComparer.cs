using System.Collections.Generic;

namespace Fred.Entities.RapportPrime
{
    /// <summary>
    /// Comparateur des objets RapportPrimeLigneAstreinte
    /// </summary>
    public class RapportPrimeLigneAstreinteComparer : IEqualityComparer<RapportPrimeLigneAstreinteEnt>
    {
        /// <summary>
        /// Teste l'égalité entre 2 RapportPrimeLigneAstreinte
        /// </summary>
        /// <param name="x">1er élément à comparer</param>
        /// <param name="y">2eme élément à comparer</param>
        /// <returns>Boolean</returns>
        public bool Equals(RapportPrimeLigneAstreinteEnt x, RapportPrimeLigneAstreinteEnt y)
        {
            // Check whether the objects are the same object. 
            if (RapportPrimeLigneAstreinteEnt.ReferenceEquals(x, y))
            {
                return true;
            }

            // Check whete the properties are equa.l
            return x != null && y != null
                && x.RapportPrimeLigneId.Equals(y.RapportPrimeLigneId)
                && x.AstreinteId.Equals(y.AstreinteId);
        }

        /// <summary>
        /// Get hashcode du RapportPrimeLigneAstreinteEnt
        /// </summary>
        /// <param name="obj">Elément dont on récupère le hashcode</param>
        /// <returns>Hashcode du RapportPrimeLigneAstreinteEnt</returns>
        public int GetHashCode(RapportPrimeLigneAstreinteEnt obj)
        {
            int hashOfAstreinteId = obj.AstreinteId.GetHashCode();

            int hashOfRapportPrimeLigneId = obj.RapportPrimeLigneId.GetHashCode();

            // Calculate the hash code 
            return hashOfAstreinteId ^ hashOfRapportPrimeLigneId;
        }
    }
}
