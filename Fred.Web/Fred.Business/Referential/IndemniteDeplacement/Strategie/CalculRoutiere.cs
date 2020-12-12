using Fred.Framework.Exceptions;
using Fred.Framework.Services.Google;
using Fred.Framework.Tool;

namespace Fred.Business.Referential.IndemniteDeplacement
{
    /// <summary>
    /// classe permettant de calculer suivant la manière routière
    /// </summary>
    public class CalculRoutiere : IManiereCalcul
    {
        /// <summary>
        /// Permet de calculer le kilométrage selon google
        /// </summary>
        /// <param name="chantier">coordonnée du chantier</param>
        /// <param name="rattachement">coordonnée du rattachement</param>
        /// <returns>nombre décimale</returns>
        public double CalculKilometre(GeographicCoordinate chantier, GeographicCoordinate rattachement)
        {
            try
            {
                return new GeocodeService().GetDrivingDistanceInKm(chantier, rattachement);
            }
            catch (FredTechnicalException)
            {
                return DistancesTool.GetOrthodromieInKm(chantier, rattachement);
            }
        }
    }
}
