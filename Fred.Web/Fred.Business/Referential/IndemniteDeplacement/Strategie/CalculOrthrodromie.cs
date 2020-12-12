using Fred.Framework.Tool;

namespace Fred.Business.Referential.IndemniteDeplacement
{
    /// <summary>
    /// classe permettant de calculer suivant la manière Orthrodromique
    /// </summary>
    public class CalculOrthrodromie : IManiereCalcul
    {
        /// <summary>
        /// Permet de calculer le kilométarge selon la manière choisi 
        /// </summary>
        /// <param name="chantier">coordonnée du chantier</param>
        /// <param name="rattachement">coordonnée du rattachement</param>
        /// <returns>nombre décimale</returns>
        public double CalculKilometre(GeographicCoordinate chantier, GeographicCoordinate rattachement)
        {
            return DistancesTool.GetOrthodromieInKm(chantier, rattachement);
        }
    }
}
